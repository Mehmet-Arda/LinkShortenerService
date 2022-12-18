using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkShortenerService.Models.ViewModels
{
    public class LinkEditOffcanvasViewModel
    {
        public Urls Url { get; set; }

        [DisplayName("Özel Url(isteğe bağlı)"),MinLength(4, ErrorMessage = "En az 4 karakter uzunluğunda bir özel url girebilirsiniz."), MaxLength(10, ErrorMessage = "En fazla 10 karakter uzunluğunda bir özel url girebilirsiniz."), RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Lütfen geçerli bir özel url giriniz.")]
        public string HalfUrlAlias { get; set; }

        [DataType(DataType.DateTime)]
        public Nullable<DateTime> ValidityTime { get; set; }

    }
}