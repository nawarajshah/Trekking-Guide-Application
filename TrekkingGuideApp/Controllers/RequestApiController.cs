using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrekkingGuideApp.Data;
using TrekkingGuideApp.Models;

namespace TrekkingGuideApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RequestApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public RequestApiController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: /api/requestapi
        // user creates a request for a given itinerary
        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromQuery] int itineraryId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            // check if itinerary exists
            var itinerary = await _context.Itineraries.FindAsync(itineraryId);
            if (itinerary == null)
                return NotFound("Itinerary not found.");

            // check if there's already a request
            var existing = await _context.Requests
                .AnyAsync(r => r.ItineraryId == itineraryId && r.UserId == user.Id);
            if (existing)
                return BadRequest("You have already requested this itinerary.");

            var request = new Request
            {
                UserId = user.Id,
                UserName = user.FullName,
                ItineraryId = itineraryId,
                Status = "Pending",
                CreatedDate = DateTime.Now
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Request created successfully.", requestId = request.Id });
        }

        // GET: /api/requestapi/myrequests
        // for normal user to see their requests
        [HttpGet]
        [Route("myrequests")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetRequests()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var request = await _context.Requests
                .Include(r => r.Itinerary)
                .ThenInclude(i => i.Place)
                .Where(r => r.UserId == user.Id)
                .Select(r => new Request
                {
                    Id = r.Id,
                    UserId = user.Id,
                    UserName = _context.Users
                                .Where(u => u.Id == r.UserId)
                                .Select(u => u.FullName)
                                .FirstOrDefault(),
                    ItineraryId = r.ItineraryId,
                    Status = r.Status,
                    CreatedDate = r.CreatedDate,
                    Itinerary = r.Itinerary
                })
                .ToListAsync();

            return Ok(request);
        }

        // GET: /api/requestapi/guide
        // for guide to see request for his itinersries
        [HttpGet]
        [Route("guide")]
        [Authorize(Roles = "Guide,Admin,SuperAdmin")]
        public async Task<IActionResult> GetGuideRequests()
        {
            var guide = await _userManager.GetUserAsync(User);
            if (guide == null)
                return Unauthorized();

            // a guide's itinerary => itinerary.guideid == guide.id
            // we want al requests whose itinerary belomgs to the current guide
            var requests = await _context.Requests
                .Include(r => r.Itinerary)
                .ThenInclude(i => i.Place)
                .Where(r => r.Itinerary.GuideId == guide.Id)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            return Ok(requests);
        }

        // PUT: /api/requestsapi/accept?requestId=xxx
        [HttpPut]
        [Route("accept")]
        [Authorize(Roles = "Guide,Admin,SuperAdmin")]
        public async Task<IActionResult> AcceptRequest([FromQuery] int requestId)
        {
            var guide = await _userManager.GetUserAsync (User);
            if (guide == null)
                return Unauthorized();

            var request = await _context.Requests
                .Include (r => r.Itinerary)
                .FirstOrDefaultAsync(r => r.Id == requestId);
            if (request == null)
                return NotFound("Request not found.");

            // check if the itinerary belongs to the current guide
            if (request.Itinerary.GuideId != guide.Id)
                return Forbid();

            request.Status = "Accepted";
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Request accepted." });
        }

        // PUT: /api/requestapi/reject?requestId=xxx
        [HttpPut]
        [Route("reject")]
        [Authorize(Roles = "Guide,Admin,SuperAdmin")]
        public async Task<IActionResult> RejectRequest([FromQuery] int requestId)
        {
            var guide = await _userManager.GetUserAsync(User);
            if (guide == null)
                return Unauthorized();

            var request = await _context.Requests
                .Include(r => r.Itinerary)
                .FirstOrDefaultAsync (r => r.Id == requestId);
            if (request == null)
                return NotFound("Request not found.");

            // check if this itinerary belongs to the current guide
            if (request.Itinerary.GuideId != guide.Id)
                return Forbid();

            request.Status = "Rejected";
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Request rejected." });
        }

        // GET: /api/itineraryapi/{api}
        // return itinerary details along with the guide name
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetItinerary(int id)
        {
            var itinerary = await _context.Itineraries
                .Include(i => i.Place)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (itinerary == null)
                return NotFound();

            var guide = await _userManager.FindByIdAsync(itinerary.GuideId);
            var dto = new ItineraryDto
            {
                Id = itinerary.Id,
                PlaceId = itinerary.PlaceId,
                GuideId = itinerary.GuideId,
                GuideName = guide?.FullName,
                Cost = itinerary.Cost,
                Duration = itinerary.Duration,
                Description = itinerary.Description,
                CreatedDate = itinerary.CreatedDate,
                Place = itinerary.Place
            };

            return Ok(dto);
        }
    }
}

public class ItineraryDto
{
    public int Id { get; set; }
    public int? PlaceId { get; set; }
    public string GuideId { get; set; }
    public string GuideName { get; set; }
    public decimal Cost { get; set; }
    public string Duration { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public Place Place { get; set; }
}