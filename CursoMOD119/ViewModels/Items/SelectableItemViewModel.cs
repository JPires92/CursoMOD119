using System.ComponentModel.DataAnnotations;

namespace CursoMOD119.ViewModels.Items
{
    public class SelectableItemViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; } = "";

       
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public decimal? Price { get; set; }

        [Display(Name = "Selected")]
        public bool Selected { get; set; }
    }
}
