using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrekkingGuideApp.Data;
using TrekkingGuideApp.Models;
using TrekkingGuideApp.ViewModels;

namespace TrekkingGuideApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlacesApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string _imageFolder = @"D:\Image\Places";

        public PlacesApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/placesapi
        [HttpGet]
        public async Task<IActionResult> GetAllPlaces()
        {
            // return all the places from the database
            var places = await _context.Places.ToListAsync();
            return Ok(places);
        }

        // (Optional) GET: /api/places/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlace(int id)
        {
            var place = await _context.Places.FindAsync(id);
            if (place == null) return NotFound();

            return Ok(place);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        // POST: api/placesapi
        [HttpPost]
        public async Task<IActionResult> CreatePlace([FromForm] PlaceViewModel model)
        {
            // Photo upload is handled separately or via base64 in dto?
            // for simplicity, let's assume no immediate file upload in this route.

            if (!ModelState.IsValid) return BadRequest(ModelState);

            string uniqueFileName = null;
            if (model.Photo != null)
            {
                // ensure folder exists
                if (!Directory.Exists(_imageFolder))
                    Directory.CreateDirectory(_imageFolder);

                uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Photo.FileName);
                var fullPath = Path.Combine(_imageFolder, uniqueFileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                    await model.Photo.CopyToAsync(stream);
            }

            var place = new Place
            {
                Title = model.Title,
                Description = model.Description,
                PhotoPath = uniqueFileName
            };

            _context.Places.Add(place);
            await _context.SaveChangesAsync();

            return Ok(place);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        // PUT: api/placesapi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlace(int id, [FromForm] PlaceViewModel model)
        {
            if (id != model.Id) return BadRequest("Id mismatch");

            var place = await _context.Places.FindAsync(id);
            if (place == null) return NotFound();

            place.Title = model.Title;
            place.Description = model.Description;
            
            // if a new photo is upladed, replace the old one
            if (model.Photo != null)
            {
                // optionally delete the old photo
                if (!string.IsNullOrEmpty(place.PhotoPath))
                {
                    var oldPath = Path.Combine(_imageFolder, place.PhotoPath);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // save the new file
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Photo.FileName);
                var fullPath = Path.Combine(_imageFolder, uniqueFileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                    await model.Photo.CopyToAsync(stream);

                place.PhotoPath = uniqueFileName;
            }

            _context.Update(place);
            await _context.SaveChangesAsync();
            return Ok(place);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        // DELETE: api/placesapi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlace(int id)
        {
            var place = await _context.Places.FindAsync(id);
            if (place == null) return NotFound();

            /// optionally delete the existing photo from D:\Image\Places
            if (!string.IsNullOrEmpty(place.PhotoPath))
            {
                var fullPath = Path.Combine(_imageFolder, place.PhotoPath);
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }

            _context.Places.Remove(place);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Place deleted" });
        }

        // GET: /api/placesapi/image/filename.jpg
        // serve images from D:\Image\Places
        [HttpGet("image/{fileName}")]
        [AllowAnonymous]
        public IActionResult GetImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return NotFound();
            var fullpath = Path.Combine(_imageFolder, fileName);
            if (!System.IO.File.Exists(fullpath)) return NotFound();

            // Content type detection
            var ext = GetContentType(fullpath);

            return PhysicalFile(fullpath, ext);
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
                case ".png":
                    return "image/png";
                default:
                    return "application/octet-stream";
            }
        }
    }
}