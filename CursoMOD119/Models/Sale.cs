using CursoMOD119.ViewModels.Sales;
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

        //Convert sale to saleViewModel
        public static implicit operator SaleViewModel(Sale sale)
        {
            if (sale == null)
                return null;

            return new SaleViewModel
            {
                ID = sale.ID,
                Amount = sale.Amount,
                ClientID = sale.ClientID,
                SaleDate = sale.SaleDate
            };
        }

        //Convert 'saleViewModel' to 'sale'
        public static implicit operator Sale(SaleViewModel saleViewModel)
        {
            if (saleViewModel == null)
                return null;

            return new Sale
            {
                ID = saleViewModel.ID,
                Amount = saleViewModel.Amount,
                ClientID = saleViewModel.ClientID,
                SaleDate = saleViewModel.SaleDate
            };
        }
    }
}
