using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkShortenerService.Models.ViewModels
{
    public class UserCreateShortUrlViewModel
    {
        public Users User { get; set; }

        public Urls NewUrl { get; set; }

        [MinLength(4,ErrorMessage ="En az 4 karakter uzunluğunda bir özel url girebilirsiniz."), MaxLength(10, ErrorMessage = "En fazla 10 karakter uzunluğunda bir özel url girebilirsiniz."),RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage ="Lütfen geçerli bir özel url giriniz.")]
        public string HalfUrlAlias { get; set; }

        [DataType(DataType.DateTime)]
        public Nullable<DateTime> ValidityTime { get; set; }


        public Nullable<int> OptionalValidationTimeInput { get; set; }

        public string SelectedValidationTimeTypeID { get; set; }

        public SelectList DdlOptionalValidationTimeType { get; set; }




    }
}