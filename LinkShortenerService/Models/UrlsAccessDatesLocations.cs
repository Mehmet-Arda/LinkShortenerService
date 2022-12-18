using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkShortenerService.Models
{
    public class UrlsAccessDatesLocations
    {

        public ObjectId UrlID { get; set; }

        public DateTime AccessDate { get; set; }

        public string AccessLocation { get; set; }

    }
}