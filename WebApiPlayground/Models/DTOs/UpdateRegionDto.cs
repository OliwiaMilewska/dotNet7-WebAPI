using System.ComponentModel.DataAnnotations;

namespace WebApiPlayground.Models.DTOs
{
    public class UpdateRegionDto
    {
        [Required]
        [MaxLength(3, ErrorMessage = "Code has to be max 3 characters")]
        public string Code { get; set; }
        [MaxLength(50, ErrorMessage = "Name needs to be shorter than 50 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
