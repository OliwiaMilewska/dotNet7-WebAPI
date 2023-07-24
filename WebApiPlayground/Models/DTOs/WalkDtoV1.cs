using WebApiPlayground.Models.Domain;

namespace WebApiPlayground.Models.DTOs
{
    public class WalkDtoV1
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        public RegionDto Region { get; set; }
        public Difficulty Difficulty { get; set; }
    }

    public class WalkDtoV2
    {
        public Guid Id { get; set; }
        public string NameOfWalk { get; set; } // change
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        public RegionDto Region { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}
