using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkShortenerService.Models
{
    public class Urls
    {
        [BsonId, BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public ObjectId ID { get; set; }

        [BsonElement("original_url"),Url(ErrorMessage ="Lütfen geçerli bir url giriniz."), DisplayName("Orijinal Url*"), Required(ErrorMessage = "Url alanı gereklidir."), MaxLength(2048, ErrorMessage = "Url alanı 2048 karakterden büyük olamaz."),DataType(DataType.Url,ErrorMessage ="Geçerli bir url giriniz.")]
        public string OriginalUrl { get; set; }

        [BsonElement("shortened_url"), DisplayName("Kısaltılmış Url")]
        public string ShortenedUrl { get; set; }

        [BsonElement("created_at"), DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("created_by"), DisplayName("Oluşturan Kullanıcı")]
        public ObjectId CreatedBy { get; set; }

        [BsonElement("click_count"), DisplayName("Tıklanma Sayısı")]
        public int ClickCount { get; set; }

        [BsonElement("validity_date"), DisplayName("Geçerlilik Tarihi")]
        public string ValidityDate { get; set; }

        [BsonElement("tag_name"), DisplayName("Etiket(isteğe bağlı)"),MinLength(3,ErrorMessage ="Etiket ismi 3 karakterden küçük olamaz."),MaxLength(15,ErrorMessage ="Etiket ismi 15 karakterden büyük olamaz.")]
        public string TagName { get; set; }

        [BsonElement("title"), DisplayName("Başlık(isteğe bağlı)"),MinLength(4,ErrorMessage ="Başlık alanı 4 karakterden küçük olamaz."),MaxLength(50,ErrorMessage ="Başlık alanı 50 karakterden büyük olamaz.")]
        public string Title { get; set; }

        [BsonElement("is_active"), DisplayName("Aktif mi")]
        public bool IsActive { get; set; }

    }
}