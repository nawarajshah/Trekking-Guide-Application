using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrekkingGuideApp.Data;
using TrekkingGuideApp.Models;
using TrekkingGuideApp.ViewModels;

namespace TrekkingGuideApp.Controllers
{
    public class PlaceController : Controller
    {
        private readonly AppDbContext _context;
        //private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string _imageFolder = @"D:\Image\Places";

        public PlaceController(AppDbContext context)
        {
            _context = context; 
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        // GET: /Place/
        public async Task<IActionResult> Index()
        {
            var places = await _context.Places.ToListAsync();
            return View(places);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        // GET: /Place/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        // POST: /Place/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlaceViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                // handle file upload
                if (model.Photo != null)
                {
                    //string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "places");
                    if (!Directory.Exists(_imageFolder))
                        Directory.CreateDirectory(_imageFolder);

                    uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Photo.FileName);
                    string filePath = Path.Combine(_imageFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(fileStream);
                    }
                }

                Place place = new Place
                {
                    Title = model.Title,
                    Description = model.Description,
                    //PhotoPath = uniqueFileName != null ? "D:/Image" + uniqueFileName : null
                    PhotoPath = uniqueFileName
                };

                _context.Places.Add(place);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        // GET: /place/edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var place = await _context.Places.FindAsync(id);
            if (place == null)
                return NotFound();

            var model = new PlaceViewModel
            {
                Id = place.Id,
                Title = place.Title,
                Description = place.Description,
                ExistingPhotoPath = place.PhotoPath
            };

            return View(model);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        // POST: /place/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlaceViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var place = await _context.Places.FindAsync(id);
                    if (place == null)
                        return NotFound();

                    place.Title = model.Title;
                    place.Description = model.Description;

                    // check if a new photo was uploaded
                    if (model.Photo != null)
                    {
                        // delete the existing photo if it exists
                        if (!string.IsNullOrEmpty(model.ExistingPhotoPath))
                        {
                            string existingFilePath = Path.Combine(_imageFolder, model.ExistingPhotoPath);
                            if (System.IO.File.Exists(existingFilePath))
                                System.IO.File.Delete(existingFilePath);
                        }

                        //string uploadFolder = Path.Combine(_imageFolder, "images", "places");
                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Photo.FileName);
                        string filePath = Path.Combine(_imageFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.Photo.CopyToAsync(fileStream);
                        }
                        place.PhotoPath = uniqueFileName;
                    }

                    _context.Update(place);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Places.Any(e => e.Id == model.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private bool PlaceExists(int id)
        {
            return _context.Places.Any(e => e.Id == id);
        }

        // GET: /Place/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            // decoding the hased id using the same salt and minimum length
            var hasdids = new Hashids("my-secret-salt", 8);
            var decoded = hasdids.Decode(id);
            if (decoded.Length == 0) return NotFound();

            int placedId = decoded[0];
            var place = await _context.Places
                .Include(p => p.Itineraries)
                .FirstOrDefaultAsync(p => p.Id == placedId);
            if (place == null) return NotFound();

            return View(place);
        }

        [HttpGet]
        public IActionResult GetImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return NotFound();

            string filePath = Path.Combine(_imageFolder, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            // description content type based on file extension
            string contentType = GetContentType(filePath);
            return PhysicalFile(filePath, contentType);
        }

        private string GetContentType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                default:
                    return "application/octet-stream";
            }
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var place = await _context.Places.FindAsync(id);
            if (place == null) return NotFound();

            // optionally delete the image file if stored outside of wwwroot.
            if (!string.IsNullOrEmpty(place.PhotoPath))
            {
                string filePath = Path.Combine(@"D:\Image\Places", place.PhotoPath);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            _context.Places.Remove(place);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
