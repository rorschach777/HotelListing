using System;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.Models
{
    public class LoginUserDTO
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} characters", MinimumLength = 6)]
        public string Password { get; set; }
    }

    // we have to make sure that our mapper knows about the DTO to link the Data and DTO together. 
    public class UserDTO : LoginUserDTO
    {
        // neither of these are required. 
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

    }
}
