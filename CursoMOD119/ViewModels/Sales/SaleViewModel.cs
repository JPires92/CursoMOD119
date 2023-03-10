using CursoMOD119.ViewModels.Items;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CursoMOD119.ViewModels.Sales
{
    public class SaleViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Sale Date")]
        [DataType(DataType.Date)]
        public DateTime SaleDate { get; set; } = DateTime.Now;

        [Display(Name = "Amount")]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Display(Name = "Client")]
        public int ClientID { get; set; }

        [Display(Name = "Items")]
        public int[]? ItemIDs { get; set; } = Array.Empty<int>();

        public List<SelectableItemViewModel> SelectableItems { get; set; }
    }
}
