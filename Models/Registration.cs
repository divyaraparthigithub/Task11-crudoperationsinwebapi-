using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace Task11_crud_.Models
{
    public class Registration
    {
        [Required(ErrorMessage="First Name is required")]
        public string FirstName { get;set; }

        [Required(ErrorMessage ="Last Name is required")]
        public string LastName { get;set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number. It must be a 10-digit number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get;set; }

    
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string SelectedCountry { get;set; }

        [Required(ErrorMessage = "State is required.")]
        public string SelectedState { get;set; }
        public bool EmailConfirmed { get; set; }    
        public IEnumerable<string> Countries { get; set; }
        public Dictionary<string,IEnumerable<string>> States { get; set;}
        public Registration()
        {
            Countries = new List<string>();
            States = new Dictionary<string, IEnumerable<string>>();

        }
        

    }
}
