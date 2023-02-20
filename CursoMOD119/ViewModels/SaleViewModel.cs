using CursoMOD119.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CursoMOD119.ViewModels
{
    public class SaleViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Sale Date")]
        public DateTime SaleDate { get; set; }
        
        [Display(Name = "Amount")]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Display(Name = "Client")]
        public int ClientID { get; set; }

        [Display(Name = "Items")]
        public int[] ItemIDs { get; set; }
    }
}
