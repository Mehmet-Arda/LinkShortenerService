using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkShortenerService.Models.ViewModels
{
    public class UserSigninViewModel
    {
        public Users User { get; set; }

        [DisplayName("Şifre*"), MinLength(3, ErrorMessage = "Şifre alanı 3 karakterden küçük olamaz."), MaxLength(20, ErrorMessage = "Şifre alanı 20 karakterden büyük olamaz."), Required(ErrorMessage = "Şifre alanı gereklidir.")]
        public string Password { get; set; }

        
    }
}