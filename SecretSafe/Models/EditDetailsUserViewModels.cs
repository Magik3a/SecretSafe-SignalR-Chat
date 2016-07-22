using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SecretSafe.Models
{
    public class EditDetailsUserViewModels
    {

        [Required]
        [Display(Name = "Display name")]
        public string NickName { get; set; }
    }
}