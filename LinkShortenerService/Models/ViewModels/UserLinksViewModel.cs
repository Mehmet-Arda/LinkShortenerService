using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkShortenerService.Models.ViewModels
{
    public class UserLinksViewModel
    {
        public Users User { get; set; }

        public List<Urls> Urls { get; set; }

        public LinkDetailPartialViewModel LinkDetailPartialViewModel { get; set; }

        public string FilterTagName { get; set; }
        public SelectList DdlFilterTagNames { get; set; }


    }
}