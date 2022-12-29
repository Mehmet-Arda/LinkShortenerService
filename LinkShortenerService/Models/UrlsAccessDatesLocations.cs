using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkShortenerService.Models
{
    public class UrlsAccessDatesLocations
    {

        [BsonId, BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public ObjectId ID { get; set; }

        [BsonElement("url_id")]
        public ObjectId UrlID { get; set; }

        [BsonElement("access_date")]
        public DateTime AccessDate { get; set; }

        [BsonElement("access_location")]
        public string AccessLocation { get; set; }

    }
}