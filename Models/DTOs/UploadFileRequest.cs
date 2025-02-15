using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BackTelega.Models.DTOs
{
    public class UploadFileRequest
    {
        [Required]
        public int ChatId { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
