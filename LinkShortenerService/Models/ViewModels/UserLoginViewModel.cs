using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkShortenerService.Models.ViewModels
{
    public class UserLoginViewModel
    {
        [DisplayName("Email*"), Required(ErrorMessage = "Email alanı boş geçilemez."), RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Lütfen geçerli bir Email adresi girin")]
        public string Email { get; set; }

        [DisplayName("Şifre*"), MinLength(3, ErrorMessage = "Şifre alanı 3 karakterden küçük olamaz."), MaxLength(50, ErrorMessage = "Şifre alanı 50 karakterden küçük olamaz."), Required(ErrorMessage = "Şifre alanı gereklidir.")]
        public string Password { get; set; }
    }
}