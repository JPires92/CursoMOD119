using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CursoMOD119.Models
{
    public class StockMovement
    {
        public int ID { get; set; }

        // True => Entrada de stock, False => Saída de stock
        [Display(Name = "Type")]
        public bool Type { get; set; }

        // Movement Quantity
        [Display(Name = "Amount")]
        public int Amount { get; set; }

        // Date of Movement
        [Display(Name = "Movement Date")]
        public DateTime MovementDate { get; set; }

        // Foreign Key
        [Display(Name = "Item")]
        public int ItemID { get; set; }

        
        public Item? Item { get; set; }
    }
}
