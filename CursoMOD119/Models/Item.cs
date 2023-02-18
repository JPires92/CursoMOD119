using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoMOD119.Models
{
    public class Item
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; } = "";

        [Display(Name = "Price")]
        [Column(TypeName = "money")]
        [DisplayFormat(DataFormatString ="{0:N2}", ApplyFormatInEditMode = true)]
        [Range(0.00, 999.99, ErrorMessage = "RangeErrorMessage")]
        public decimal? Price { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Discontinued")]
        public bool Discontinued { get; set; }

        [Display(Name = "Sales")]
        public ICollection<Sale> Sales { get; set; }
    }
}
