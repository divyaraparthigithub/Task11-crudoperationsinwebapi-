using System.ComponentModel.DataAnnotations;

namespace Task11_crud_.Models
{
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public string GenderName { get; set; }
    }
}
