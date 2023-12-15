using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task11_crud_.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Product is required")]
        public int ProductId { get; set; }

        public int GenderId { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        //public string ProductName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number should contain 10 digits")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        //public string GenderName { get; set; }
        //public string ProductName { get; set; }
        public List<SelectListItem> ProductList { get; set; }


        public Customer()
        {
            ProductList = new List<SelectListItem>();
        }
    }
}
