using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrekkingGuideApp.Data;
using TrekkingGuideApp.Models;
using TrekkingGuideApp.ViewModels;

namespace TrekkingGuideApp.Controllers
{
    [Authorize(Roles = "Guide")]
    public class GuideController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly AppDbContext _context;

        public GuideController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Guide/SearchPlaces?query=...
        public async Task<IActionResult> SearchPlaces(string query)
        {
            var places = string.IsNullOrEmpty(query) ?
                await _context.Places.ToListAsync() :
                await _context.Places
                    .Where(p => p.Title.Contains(query) || p.Description.Contains(query))
                    .ToListAsync();
            return View(places); // View shows a list of places with an "Add Itinerary" options
        }

        // GET: /Guide/AddItinerary/{placeId}
        public async Task<IActionResult> AddItinerary(int placesId)
        {
            var place = await _context.Places.FindAsync(placesId);
            if (place == null) 
                return NotFound();

            var model = new ItineraryViewModel
            {
                PlaceId = place.Id,
                PlaceTitle = place.Title,
            };
            return View(model);
        }

        // POST: /Guide/AddItinerary
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItinerary(ItineraryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var guide = await _userManager.GetUserAsync(User);
            if (guide == null)
                return Unauthorized();

            var itinerary = new Itinerary
            {
                PlaceId = model.PlaceId,
                GuideId = guide.Id,
                ItineraryDetails = model.ItineraryDetails
            };

            _context.Itineraries.Add(itinerary);
            await _context.SaveChangesAsync();
            // After saving, redirect back to search results or details page
            return RedirectToAction("SearchPlaces");
        }
    }
}
