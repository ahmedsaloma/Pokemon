using Pokemon.Data.ENum;
using Pokemon.Models;

namespace Pokemon.ViewModels
{
    public class EditRaceViewModel
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }

        public int AddressId { get; set; }

        public IFormFile Image { get; set; }

        public string? URL { get; set; }
        public RaceCategory RaceCategory { get; set; }
    }
}
