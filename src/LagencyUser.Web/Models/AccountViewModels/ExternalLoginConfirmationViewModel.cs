using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LagencyUser.Web.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "&nbsp;")]
        [Display(Name = "Given name")]
        public string GivenName { get; set; }

        [Required(ErrorMessage = "&nbsp;")]
        [Display(Name = "Family name")]
        public string FamilyName { get; set; }

        [Required(ErrorMessage = "&nbsp;")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
