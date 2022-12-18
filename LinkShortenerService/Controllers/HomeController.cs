﻿using LinkShortenerService.Models;
using LinkShortenerService.Models.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace LinkShortenerService.Controllers
{
    public class HomeController : Controller
    {
        MongoClient client = new MongoClient("mongodb+srv://localuser:4969@cluster0.lnlkhhr.mongodb.net/?retryWrites=true&w=majority");

        public ActionResult Index()
        {

            IMongoDatabase database = client.GetDatabase("LinkShortenerService");
            IMongoCollection<Users> UsersCollection = database.GetCollection<Users>("Users");

            IMongoCollection<Urls> UrlsCollection = database.GetCollection<Urls>("Urls");



            if (TempData["resultmessage"] != null)
            {
                ViewBag.result = TempData["resultmessage"];
                ViewBag.resultbg = TempData["resultbg"];
            }

            HomeIndexViewModel homeIndexViewModel = new HomeIndexViewModel();



            List<Urls> t = UrlsCollection.AsQueryable().OrderByDescending(x => x.ClickCount).Take(3).ToList();

            for (int i = 0; i < t.Count; i++)
            {
                var decodedOriginalUrlBytes = Convert.FromBase64String(t[i].OriginalUrl);
                var OriginalUrl = Encoding.UTF8.GetString(decodedOriginalUrlBytes);

                var decodedShortenedUrlBytes = Convert.FromBase64String(t[i].ShortenedUrl);
                var ShortenedUrl = Encoding.UTF8.GetString(decodedShortenedUrlBytes);


                t[i].OriginalUrl = OriginalUrl;
                t[i].ShortenedUrl = ShortenedUrl;

            }

            homeIndexViewModel.MostVisitedLinks = t;



            var j = UrlsCollection.AsQueryable().GroupBy(x => x.CreatedBy).Select(y => new { UserID = y.Key, Total = y.Count() }).OrderByDescending(z => z.Total).Take(1).FirstOrDefault();

            homeIndexViewModel.MostLinkShortener = UsersCollection.Find(x => x.ID == j.UserID).FirstOrDefault();

            homeIndexViewModel.MostLinkShortenerLinkCount = j.Total;

            homeIndexViewModel.TotalShortenedLinkCount = UrlsCollection.AsQueryable().ToList().Count();

            homeIndexViewModel.TotalClicks = UrlsCollection.AsQueryable().Sum(x => x.ClickCount);

            homeIndexViewModel.TotalActiveLinkCount = UrlsCollection.AsQueryable().Where(x => x.IsActive).ToList().Count();

            homeIndexViewModel.AverageLinkCountCreatedByUsers = (int)(UrlsCollection.AsQueryable().GroupBy(x => x.CreatedBy).Select(y => new { UserID = y.Key, Total = y.Count() }).Average(z => z.Total));

            return View(homeIndexViewModel);

        }





        [HttpGet]
        public ActionResult Signin()
        {

            UserSigninViewModel userSigninViewModel = new UserSigninViewModel()
            {

            };


            return View(userSigninViewModel);
        }




        [HttpPost]
        public ActionResult Signin(UserSigninViewModel model)
        {

           
            ModelState.Remove("User.Password");

            IMongoDatabase database = client.GetDatabase("LinkShortenerService");

            IMongoCollection<Users> users = database.GetCollection<Users>("Users");

            var t = users.Find(x => x.Email == model.User.Email && x.IsActive == true).FirstOrDefault();



            if (ModelState.IsValid && t == null)
            {

                model.User.IsActive = true;
                model.User.DateOfRegistration = DateTime.Now;
                model.User.Password = Crypto.HashPassword(model.Password);


                try
                {
                    users.InsertOneAsync(model.User).Wait();
                }
                catch (Exception)
                {
                    TempData["resultmessage"] = "Kayıt oluşturma başarısız.";
                    TempData["resultbg"] = "bg-danger";

                    return RedirectToAction("Signin");
                }

                TempData["resultmessage"] = "Yeni kayıt başarıyla oluşturuldu.";
                TempData["resultbg"] = "bg-success";

                return RedirectToAction("Login");


            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                else if (t != null)
                {
                    //ModelState.AddModelError("", "Girilen bilgilere göre kayıtlı öğrenci bulunmaktadır.");

                    ViewBag.result = "Girilen bilgilere göre daha önce oluşturulmuş kayıt bulunmaktadır.";
                    ViewBag.resultbg = "bg-danger";

                    UserSigninViewModel userSigninViewModel = new UserSigninViewModel()
                    {

                    };

                    userSigninViewModel.User = model.User;


                    return View(userSigninViewModel);
                }
            }



            return RedirectToAction("Signin");
        }


        [HttpGet]
        public ActionResult Login()
        {

            if (TempData["resultmessage"] != null)
            {
                ViewBag.result = TempData["resultmessage"];
                ViewBag.resultbg = TempData["resultbg"];
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel model)
        {



            IMongoDatabase database = client.GetDatabase("LinkShortenerService");

            IMongoCollection<Users> users = database.GetCollection<Users>("Users");

            var sifreDogrumu = false;

            var user = users.Find(x => x.Email == model.Email && x.IsActive).FirstOrDefault();

            if (user != null)
            {
                sifreDogrumu = Crypto.VerifyHashedPassword(user.Password, model.Password);
            }

            if (user != null && ModelState.IsValid && sifreDogrumu)
            {

                //Başarılı Giriş yap.
                //TempData["result"] = "Başarıyla giriş yapıldı. Yönlendiriliyorsunuz.";
                //TempData["resultbg"] = "bg-success";

                Session["user"] = user;

                return RedirectToAction("Links", "User");

            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                else if (user == null || !sifreDogrumu)
                {
                    ViewBag.result = "Email veya şifre hatalı.";
                    ViewBag.resultbg = "bg-danger";

                    return View(model);
                }

            }

            return RedirectToAction("Login");
        }






        [HttpGet]
        public ActionResult ForgotPassword()
        {

            return View();
        }





        [HttpPost]
        public JsonResult SendEmail(string email)
        {

            System.Threading.Thread.Sleep(1000);

            List<string> result = new List<string>();

            IMongoDatabase database = client.GetDatabase("LinkShortenerService");

            IMongoCollection<Users> users = database.GetCollection<Users>("Users");

            var user = users.Find(x => x.Email == email && x.IsActive == true).FirstOrDefault();

            if (user == null)
            {
                result.Add("error");
                result.Add("Girilen Email'e ait bir kullanıcı bulunmamaktadır.");
                return Json(result);
            }
            else
            {
                try
                {
                    string kod = Crypto.GenerateSalt(8);
                    kod = Crypto.Hash(kod, algorithm: "md5");
                    kod = kod.Substring(3, 6);

                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("m.arda.yuksel@gmail.com", "zkxhcovvjadtjial");
                    MailMessage msgObj = new MailMessage();
                    msgObj.To.Add(email);
                    msgObj.From = new MailAddress("m.arda.yuksel@gmail.com");
                    msgObj.Subject = "Şifre Yenileme";
                    msgObj.Body = $"Doğrulama kodunuz: {kod}";
                    client.Send(msgObj);


                    result.Add("ok");
                    result.Add("Doğrulama email'i başarıyla gönderilmiştir.");


                    TempData["kodEmail"] = new List<string> { kod, email };
                    TempData.Keep("kodEmail");


                    return Json(result);


                }
                catch (Exception)
                {
                    result.Add("error");
                    result.Add("Email gönderimi başarısız.");

                    return Json(result);

                }



            }


        }

        [HttpPost]
        public JsonResult ValidateCode(string code)
        {


            System.Threading.Thread.Sleep(1000);


            List<string> kodEmail = (List<string>)TempData["kodEmail"];
            TempData.Keep("kodEmail");

            List<string> result = new List<string>();

            if (kodEmail[0] == code)
            {
                result.Add("ok");
                result.Add("Doğrulama kodunuz başarılı.");
            }
            else
            {
                result.Add("error");
                result.Add("Girilen kod yanlış.");
            }

            return Json(result);
        }


        [HttpPost]
        public JsonResult NewPassword(string newPassword)
        {
            IMongoDatabase database = client.GetDatabase("LinkShortenerService");

            IMongoCollection<Users> users = database.GetCollection<Users>("Users");

            System.Threading.Thread.Sleep(1000);

            List<string> kodEmail = (List<string>)TempData["kodEmail"];


            string email = kodEmail[1];

            List<string> result = new List<string>();


            try
            {
                Users user = users.Find(x => x.Email == email).FirstOrDefault();

                user.Password = Crypto.HashPassword(newPassword);

                users.ReplaceOne(x => x.ID == user.ID, user);
            }
            catch (Exception)
            {
                result.Add("error");
                result.Add("Şifre güncelleme işlemi başarısız.");

                TempData.Keep("kodEmail");

            }

            result.Add("ok");
            result.Add("Şifreniz başarıyla güncellenmiştir. Yönlendiriliyorsunuz...");




            return Json(result);

        }




        [HttpPost]
        public PartialViewResult GetEmailForm()
        {
            return PartialView("_ForgotPasswordSendEmailFormPartialView");
        }

        [HttpPost]
        public PartialViewResult GetValidationForm()
        {
            return PartialView("_ForgotPasswordSendValidationCodeFormPartialView");
        }


        [HttpPost]
        public PartialViewResult GetNewPasswordForm()
        {
            return PartialView("_ForgotPasswordSendNewPasswordFormPartialView");
        }





    }





}