using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ErlezWebUI.Models
{
    public class ArticleCreate
    {
        public string ArticleName { get; set; }
        public Nullable<int> CompanySellerId { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^\$?\d+(\,(\d{1,4}))?$", ErrorMessage = "Siffror med kommatecken för max 4 decimaler.")]
        public string UnitPrice { get; set; }
        [Required]
        public string UnitType { get; set; }
        public int SelectedValue { get; set; }
    }

    public class ArticleEdit
    {
        public int Id { get; set; }
        public string ArticleName { get; set; }
        public Nullable<int> CompanySellerId { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^\$?\d+(\,(\d{1,4}))?$", ErrorMessage = "Siffror med kommatecken för max 4 decimaler.")]
        public string UnitPrice { get; set; }
        [Required]
        public string UnitType { get; set; }
        public int SelectedValue { get; set; }
    }
}