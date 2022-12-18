using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkShortenerService.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public Users User { get; set; }

        [DisplayName("Fotoğraf")]
        public HttpPostedFileBase Photograph { get; set; }

        [DisplayName("Yeni Şifre"), MinLength(8, ErrorMessage = "Şifre alanı 8 karakterden küçük olamaz."), MaxLength(30, ErrorMessage = "Şifre alanı 30 karakterden büyük olamaz.")]
        public string Password { get; set; }

    }
}