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
    public class Users
    {
        [BsonId,BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public ObjectId ID { get; set; }

        [BsonElement("tcno"),RegularExpression("[0-9]{11}", ErrorMessage = "Lütfen geçerli bir TCNo girin")]
        public string TCNo { get; set; }

        [BsonElement("name"), DisplayName("Adı*"), RegularExpression("^[a-zA-ZıIİoöuüOÖUÜ]+(([',. -][a-zA-ZıIİoöuüOÖUÜ])?[a-zA-ZıIİoöuüOÖUÜ]*)*$", ErrorMessage = "Lütfen geçerli bir isim giriniz."), Required(ErrorMessage = "Adı alanı gereklidir."), MinLength(3, ErrorMessage = "Ad alanı 3 karakterden küçük olamaz."), MaxLength(26, ErrorMessage = "Ad alanı 26 karakterden büyük olamaz.")]
        public string Name { get; set; }

        [BsonElement("surname"), DisplayName("Soyadı*"), RegularExpression("^[a-zA-ZıIİoöuüOÖUÜ]+(([',. -][a-zA-ZıIİoöuüOÖUÜ])?[a-zA-ZıIİoöuüOÖUÜ]*)*$", ErrorMessage = "Lütfen geçerli bir soyad giriniz."), Required(ErrorMessage = "Soyadı alanı gereklidir."), MinLength(3, ErrorMessage = "Soyadı alanı 3 karakterden küçük olamaz."), MaxLength(26, ErrorMessage = "Soyad alanı 26 karakterden büyük olamaz.")]
        public string Surname { get; set; }

        [BsonElement("email"), DisplayName("Email*"), Required(ErrorMessage = "Email alanı gereklidir."), DataType(DataType.EmailAddress), RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Lütfen geçerli bir email adresi girin")]
        public string Email { get; set; }

        [BsonElement("password"), DisplayName("Şifre*"), MinLength(8, ErrorMessage = "Şifre alanı 8 karakterden küçük olamaz."), MaxLength(200), Required(ErrorMessage = "Şifre alanı gereklidir.")]
        public string Password { get; set; }

        [BsonElement("tel"), DisplayName("Telefon"), StringLength(10, ErrorMessage = "Telefon alanı 10 karakter olmalıdır."), RegularExpression(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$", ErrorMessage = "Lütfen geçerli bir telefon numarası girin")]
        public string Tel { get; set; }

        [BsonElement("photograph"), DisplayName("Fotoğraf"), MaxLength(50)]
        public string Photograph { get; set; }


        [BsonElement("dateofregistration")]
        public Nullable<System.DateTime> DateOfRegistration { get; set; }

        [BsonElement("isactive")]
        public bool IsActive { get; set; }

        public static implicit operator Users(List<Users> v)
        {
            throw new NotImplementedException();
        }
    }
}