using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkShortenerService.Models.ViewModels
{
    public class UserDashboardViewModel
    {
        public Users User { get; set; }

        public Urls MostVisitedLinkCreatedByUser { get; set; }


        public int UserTotalShortenedLinkCount { get; set; }

        public int TotalClicksCreatedByUser { get; set; }

        public int UserTotalActiveLinkCount { get; set; }

        public int UserTotalInactiveLinkCount { get; set; }


    }
}