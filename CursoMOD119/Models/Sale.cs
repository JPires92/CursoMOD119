using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoMOD119.Models
{
    public class Sale
    {
        public int ID { get; set; }

        [Display(Name = "Sale Date")]
        public DateTime SaleDate { get; set; }

        [Display(Name = "Amount")]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Display(Name = "Items")]
        public ICollection<Item> Items { get; set; }

        // Foreign Key
        [Display(Name = "Client")]
        public int ClientID { get; set; }


        public Client? Client { get; set; }

    }
}
