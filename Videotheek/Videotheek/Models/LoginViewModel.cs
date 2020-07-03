using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Videotheek.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Familienaam is verplicht in te vullen !")]
        public string Familienaam { get; set; }
        [Required(ErrorMessage = "Postcode is verplicht in te vullen !")]
        public int Postcode { get; set; }
    }
}