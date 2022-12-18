using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkShortenerService.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Urls> MostVisitedLinks { get; set; }

        public Users MostLinkShortener { get; set; }

        public int MostLinkShortenerLinkCount { get; set; }

        public int TotalShortenedLinkCount { get; set; }
        public int TotalClicks { get; set; }

        public int TotalActiveLinkCount { get; set; }

        public int AverageLinkCountCreatedByUsers { get; set; }


    }
}