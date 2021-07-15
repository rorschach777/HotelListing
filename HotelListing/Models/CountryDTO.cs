using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace HotelListing.Models
{

    // All the fields needed for the create, or an update. (NON-ID update.)
    // This method basically allowss you not to have to repeat the validation. 
    public class CreateCountryDTO
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "County Name Is Too Long")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 2, ErrorMessage = "Short Country Name Is Too Long")]
        public string ShortName { get; set; }
    }

    public class UpdateCountryDTO : CreateCountryDTO
    {
        public IList<CreateHotelDTO> Hotels { get; set; }
    }

    public class CountryDTO : CreateCountryDTO
    {
        public int Id { get; set; }

        // any type of sanitation / calcualtions / embelishment can occur here,
        // and it's never returned as the final datatype etc. 

        public IList<HotelDTO> Hotels { get; set; }

    }
}
