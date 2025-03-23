using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrekkingGuideApp.Data;
using TrekkingGuideApp.Models;
using TrekkingGuideApp.ViewModels;

namespace TrekkingGuideApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItinerariesApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public ItinerariesApiController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/itineraryapi/myitineraries
        // Returns all itineraries of the current guide
        [HttpGet]
        [Route("myitineraries")]
        public async Task<IActionResult> GetMyItineraries()
        {
            var guide = await _userManager.GetUserAsync(User);
            if (guide == null) 
                return Unauthorized();

            var itineraries = await _context.Itineraries
                .Where(i => i.GuideId == guide.Id)
                .ToListAsync();

            return Ok(itineraries);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllItineraries()
        {
            var itinerariess = await _context.Itineraries
                .Include(i => i.Place)
                .ToListAsync();
            return Ok(itinerariess);
        }

        // GET: /api/itineraryapi{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetItinerary(int id)
        {
            var itinerary = await _context.Itineraries
                .Include(i => i.Place)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (itinerary == null) return NotFound();

            return Ok(itinerary);
        }

        // GET: api/itineraryapi/byplaceId=5
        // return the itinerary for a specific place for the current guide (or null if not exists)
        [HttpGet]
        [Route("byplace")]
        public async Task<IActionResult> GetItineraryByPlace([FromQuery] int placeId)
        {
            var guide = await _userManager.GetUserAsync(User);
            if (guide == null)
                return Unauthorized();

            var itinerary = await _context.Itineraries
                .FirstOrDefaultAsync(i => i.PlaceId == placeId && i.GuideId == guide.Id);

            return Ok(itinerary);
        }

        // GET: /api/itineraryapi/search?title=abc
        // return itineraries for place whose title contains the query
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchItineraries([FromQuery] string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                var allItineraries = await _context.Itineraries
                    .Include(i => i.Place)
                    .ToListAsync();
                return Ok(allItineraries);
            }

            var itineraries = await _context.Itineraries
                .Include(i => i.Place)
                .Where(i => EF.Functions.Like(i.Place.Title, $"%{title}%"))
                .ToListAsync();
            return Ok(itineraries);
        }

        // POST: /api/itineraryapi/request
        // simulate sending a request to the guide for a given itinerary.
        [HttpPost]
        [Route("request")]
        public async Task<IActionResult> RequestItinerary([FromQuery] int itineraryId)
        {
            var itinerary = await _context.Itineraries.FindAsync(itineraryId);
            if (itinerary == null)
                return NotFound("Itinerary not found.");

            return Ok(new { message = "Request sent to the guide successfully!" });
        }

        // POST: api/itineraries
        // this endpoint creates a new itineraries for a given place.
        [HttpPost]
        [Authorize(Roles = "Guide,Admin,SuperAdmin")]
        public async Task<IActionResult> CreateItitnerary([FromBody] ItineraryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the current guide user
            var guide = await _userManager.GetUserAsync(User);
            if (guide == null)
            {
                return Unauthorized("Guide not found.");
            }

            // check if itinerary already exists for this place
            var exists = await _context.Itineraries
                .AnyAsync(i => i.PlaceId == model.PlaceId && i.GuideId == guide.Id);
            if (exists)
                return BadRequest("Itinerary already exists for this place.");

            var itinerary = new Itinerary
            {
                PlaceId = model.PlaceId,
                GuideId = guide.Id,
                Cost = model.Cost,
                Duration = model.Duration,
                Description = model.Description,
                CreatedDate = DateTime.UtcNow,
            };

            _context.Itineraries.Add(itinerary);
            await _context.SaveChangesAsync();

            return Ok(itinerary);
        }

        // PUT: api/itineraryapi/{api}
        // updates an existing itinerary
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItinerary(int id, [FromBody] ItineraryViewModel model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var itinerary = await _context.Itineraries.FindAsync(id);
            if (itinerary == null) 
                return NotFound();

            var guide = await _userManager.GetUserAsync(User);
            if (itinerary.GuideId != guide.Id)
                return Forbid();

            itinerary.Cost = model.Cost;
            itinerary.Duration = model.Duration;
            itinerary.Description = model.Description;

            _context.Itineraries.Update(itinerary);
            await _context.SaveChangesAsync();

            return Ok(itinerary);
        }
    }
}
