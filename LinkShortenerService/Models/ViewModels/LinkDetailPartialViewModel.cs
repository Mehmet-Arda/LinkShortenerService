using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkShortenerService.Models.ViewModels
{
    public class LinkDetailPartialViewModel
    {
        public Users User { get; set; }

        public Urls Url { get; set; }

    }
}