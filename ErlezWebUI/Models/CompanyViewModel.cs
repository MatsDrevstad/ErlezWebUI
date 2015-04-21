using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ErlezWebUI.Models
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Company")]
        public string CompanyName { get; set; }
    }
}