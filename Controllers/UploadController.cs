using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BackTelega.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        public UploadController()
        {
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(_uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            return Ok(new { fileUrl });
        }
    }
}
