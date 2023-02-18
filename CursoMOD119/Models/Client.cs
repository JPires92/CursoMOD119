using System.ComponentModel.DataAnnotations;

namespace CursoMOD119.Models
{
    public class Client
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; } = "";

        [Display(Name = "Age")]
        public int Age { get; set; }
        
        [Display(Name = "Email")]
        public string Email { get; set; } = "";
        
        [Display(Name = "Active")]
        public bool Active { get; set; }

        [Display(Name = "Sales")]
        public ICollection<Sale> Sales { get; set; }
    }
}
