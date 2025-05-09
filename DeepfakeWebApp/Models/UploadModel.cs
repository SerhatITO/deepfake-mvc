using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DeepfakeWebApp.Models
{
    public class UploadModel
    {
        [Required]
        public IFormFile VideoFile { get; set; }
    }
}
