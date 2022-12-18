using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using LinkShortenerService.Filters;
using LinkShortenerService.Models;
using LinkShortenerService.Models.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LinkShortenerService.Controllers
{
    public class UserController : Controller
    {
        MongoClient client = new MongoClient("mongodb+srv://localuser:4969@cluster0.lnlkhhr.mongodb.net/?retryWrites=true&w=majority");

        [AuthFilter]
        public ActionResult Links()
        {

            IMongoDatabase database = client.GetDatabase("LinkShortenerService");
            IMongoCollection<Users> UsersCollection = database.GetCollection<Users>("Users");

            //Users user = UsersCollection.Find(x => x.Email == "m.arda.yuksel@gmail.com").FirstOrDefault();

            Users user = (Users)Session["user"];


            IMongoCollection<Urls> UrlsCollection = database.GetCollection<Urls>("Urls");

            List<Urls> userUrls = UrlsCollection.Find(x => x.CreatedBy == user.ID && x.IsActive).ToList().OrderByDescending(x => x.CreatedAt).ToList();


            for (int i = 0; i < userUrls.Count; i++)
            {
                var decodedOriginalUrlBytes = Convert.FromBase64String(userUrls[i].OriginalUrl);
                var decodedShortenedUrlBytes = Convert.FromBase64String(userUrls[i].ShortenedUrl);

                var OriginalUrl = Encoding.UTF8.GetString(decodedOriginalUrlBytes);
                var ShortenedUrl = Encoding.UTF8.GetString(decodedShortenedUrlBytes);

                userUrls[i].OriginalUrl = OriginalUrl;
                userUrls[i].ShortenedUrl = ShortenedUrl;
            }

            int urlStartIndex = 0;

            if (TempData["updatedUrlId"] != null)
            {
                ObjectId objectId = new MongoDB.Bson.ObjectId((string)TempData["updatedUrlId"]);

                for (int i = 0; i < userUrls.Count; i++)
                {
                    if (userUrls[i].ID == objectId)
                    {
                        urlStartIndex = i;
                    }
                }
            }



            if (TempData["resultmessage"] != null)
            {
                ViewBag.result = TempData["resultmessage"];
                ViewBag.resultbg = TempData["resultbg"];
            }

            LinkDetailPartialViewModel linkDetailPartialViewModel = new LinkDetailPartialViewModel()
            {
                User = user,


            };

            if (userUrls.Count > 0)
            {
                linkDetailPartialViewModel.Url = userUrls[urlStartIndex];
            }
            else
            {
                linkDetailPartialViewModel.Url = new Urls();
            }







            List<string> tagNameList = new List<string> { "" };

            for (int i = 0; i < userUrls.Count; i++)
            {
                if (!tagNameList.Contains(userUrls[i].TagName))
                {
                    tagNameList.Add(userUrls[i].TagName);
                }

            }

            tagNameList.Remove("");

            tagNameList.Remove(null);
            tagNameList.Insert(0, "Tümü");



            List<SelectListItem> selectListItems = new List<SelectListItem>();

            for (int i = 0; i < tagNameList.Count; i++)
            {
                if (i == 0)
                {
                    SelectListItem selectListItem = new SelectListItem() { Text = tagNameList[i], Value = tagNameList[i], Selected = true };
                    selectListItems.Add(selectListItem);
                }
                else
                {
                    SelectListItem selectListItem = new SelectListItem() { Text = tagNameList[i], Value = tagNameList[i] };
                    selectListItems.Add(selectListItem);
                }


            }



            SelectList DdlFilterTagNames = new SelectList(selectListItems, "Value", "Text", "Tümü");



            UserLinksViewModel userLinksViewModel = new UserLinksViewModel()
            {
                User = user,
                Urls = userUrls,
                LinkDetailPartialViewModel = linkDetailPartialViewModel,
                DdlFilterTagNames = DdlFilterTagNames
            };

            ViewBag.editedStartUrlIndex = urlStartIndex;
            return View(userLinksViewModel);






        }


        [AuthFilter]
        public PartialViewResult FilterForTagNameGetLinks(string tagName)
        {

            IMongoDatabase database = client.GetDatabase("LinkShortenerService");
            IMongoCollection<Users> UsersCollection = database.GetCollection<Users>("Users");

            //Users user = UsersCollection.Find(x => x.Email == "m.arda.yuksel@gmail.com").FirstOrDefault();

            Users user = (Users)Session["user"];

            IMongoCollection<Urls> UrlsCollection = database.GetCollection<Urls>("Urls");

            List<Urls> userUrls;

            if (tagName != "Tümü")
            {
                userUrls = UrlsCollection.Find(x => x.CreatedBy == user.ID && x.IsActive && x.TagName == tagName).ToList().OrderByDescending(x => x.CreatedAt).ToList();

            }
            else
            {
                userUrls = UrlsCollection.Find(x => x.CreatedBy == user.ID && x.IsActive).ToList().OrderByDescending(x => x.CreatedAt).ToList();

            }




            for (int i = 0; i < userUrls.Count; i++)
            {
                var decodedOriginalUrlBytes = Convert.FromBase64String(userUrls[i].OriginalUrl);
                var decodedShortenedUrlBytes = Convert.FromBase64String(userUrls[i].ShortenedUrl);

                var OriginalUrl = Encoding.UTF8.GetString(decodedOriginalUrlBytes);
                var ShortenedUrl = Encoding.UTF8.GetString(decodedShortenedUrlBytes);

                userUrls[i].OriginalUrl = OriginalUrl;
                userUrls[i].ShortenedUrl = ShortenedUrl;
            }

            ViewBag.editedStartUrlIndex = 0;
            return PartialView("_UserLinksLinkListPartialView", userUrls);

        }



        [AuthFilter]
        public PartialViewResult GetLinkDetailPartialView(string urlID)
        {

            IMongoDatabase database = client.GetDatabase("LinkShortenerService");

            IMongoCollection<Urls> UrlsCollection = database.GetCollection<Urls>("Urls");

            IMongoCollection<Users> UsersCollection = database.GetCollection<Users>("Users");



            var urlId = new MongoDB.Bson.ObjectId(urlID);

            Urls url = UrlsCollection.Find(x => x.ID == urlId).FirstOrDefault();

            Users user = UsersCollection.Find(x => x.ID == url.CreatedBy).FirstOrDefault();

            var decodedOriginalUrlBytes = Convert.FromBase64String(url.OriginalUrl);
            var decodedShortenedUrlBytes = Convert.FromBase64String(url.ShortenedUrl);

            var OriginalUrl = Encoding.UTF8.GetString(decodedOriginalUrlBytes);
            var ShortenedUrl = Encoding.UTF8.GetString(decodedShortenedUrlBytes);

            url.OriginalUrl = OriginalUrl;
            url.ShortenedUrl = ShortenedUrl;

            LinkDetailPartialViewModel linkDetailPartialViewModel = new LinkDetailPartialViewModel()
            {
                Url = url,
                User = user

            };

            return PartialView("_UserLinksLinkDetailPartialView", linkDetailPartialViewModel);
        }




        [HttpPost]
        [AuthFilter]
        public ActionResult DeleteLink(string willDeleteLinkID)
        {
            var willDeleteUrlID = new MongoDB.Bson.ObjectId(willDeleteLinkID);

            IMongoDatabase database = client.GetDatabase("LinkShortenerService");

            IMongoCollection<Urls> UrlsCollection = database.GetCollection<Urls>("Urls");

            Urls willDeleteUrl = UrlsCollection.Find(x => x.ID == willDeleteUrlID).FirstOrDefault();

            willDeleteUrl.IsActive = false;

            UrlsCollection.ReplaceOne(x => x.ID == willDeleteUrl.ID, willDeleteUrl);


            TempData["resultmessage"] = "Silme işlemi başarıyla gerçekleştirildi.";
            TempData["resultbg"] = "bg-success";

            return RedirectToAction("Links");
        }




        [AuthFilter]
        public PartialViewResult GetLinkEditOffcanvasPartialView(string urlID)
        {

            IMongoDatabase database = client.GetDatabase("LinkShortenerService");
            IMongoCollection<Urls> UrlsCollection = database.GetCollection<Urls>("Urls");

            var urlId = new MongoDB.Bson.ObjectId(urlID);

            Urls url = UrlsCollection.Find(x => x.ID == urlId).FirstOrDefault();


            var decodedShortenedUrlBytes = Convert.FromBase64String(url.ShortenedUrl);
            var ShortenedUrl = Encoding.UTF8.GetString(decodedShortenedUrlBytes);


            url.ShortenedUrl = ShortenedUrl;



            LinkEditOffcanvasViewModel linkEditOffcanvasViewModel = new LinkEditOffcanvasViewModel()
            {
                HalfUrlAlias = url.ShortenedUrl,
                Url = url
            };


            return PartialView("_UserLinksEditLinkOffcanvasPartialView", linkEditOffcanvasViewModel);
        }

        [AuthFilter]
        public JsonResult CheckUrlAlias(string urlAlias, string urlId)
        {
            var result = "";

            var urlID = new MongoDB.Bson.ObjectId(urlId);

            IMongoDatabase database = client.GetDatabase("LinkShortenerService");

            IMongoCollection<Urls> UrlsCollection = database.GetCollection<Urls>("Urls");

            var shortUrlAliasBytes = Encoding.UTF8.GetBytes(urlAlias);
            var encodedShortUrlAlias = Convert.ToBase64String(shortUrlAliasBytes);

            var t = UrlsCollection.Find(x => x.ShortenedUrl == encodedShortUrlAlias && x.ID != urlID && x.IsActive).FirstOrDefault();



            if (t == null)
            {
                result = "ok";
            }
            else
            {
                result = "error";
            }

            return Json(result);
        }


        [HttpPost]
        [AuthFilter]
        public ActionResult EditLink(LinkEditOffcanvasViewModel model, string editUrlID)
        {
            IMongoDatabase database = client.GetDatabase("LinkShortenerService");
            IMongoCollection<Urls> UrlsCollection = database.GetCollection<Urls>("Urls");

            var urlID = new MongoDB.Bson.ObjectId(editUrlID);


            Urls willUpdateUrl = UrlsCollection.Find(x => x.ID == urlID).FirstOrDefault();

            string urlNewTitle = "";
            string urlNewTag = "";
            string urlNewAlias = "";
            //string urlNewValidityDate = "";




            urlNewTitle = model.Url.Title;
            willUpdateUrl.Title = urlNewTitle;




            urlNewTag = model.Url.TagName;
            willUpdateUrl.TagName = urlNewTag;


            var urlValidityDate = model.Url.ValidityDate;

            var yearMonthDay = urlValidityDate.Substring(0, urlValidityDate.IndexOf("T"));
            var hourMinute = urlValidityDate.Substring((urlValidityDate.IndexOf("T") + 1));

            var splitYMD = yearMonthDay.Split('-');
            var splitHm = hourMinute.Split(':');

            var validityDate = new DateTime(Convert.ToInt32(splitYMD[0]), Convert.ToInt32(splitYMD[1]), Convert.ToInt32(splitYMD[2]), Convert.ToInt32(splitHm[0]), Convert.ToInt32(splitHm[1]), 0);


            willUpdateUrl.ValidityDate = validityDate.ToString("yyyy-MM-ddTHH:mm");


            if (model.HalfUrlAlias != null)
            {
                urlNewAlias = model.HalfUrlAlias;
            }



            byte[] shortUrlNewAliasBytes;
            var encodedShortUrlNewAlias = "";

            if (urlNewAlias != "")
            {
                shortUrlNewAliasBytes = Encoding.UTF8.GetBytes(urlNewAlias);
                encodedShortUrlNewAlias = Convert.ToBase64String(shortUrlNewAliasBytes);

                willUpdateUrl.ShortenedUrl = encodedShortUrlNewAlias;


            }



            ReplaceOneResult result = UrlsCollection.ReplaceOne(x => x.ID == willUpdateUrl.ID, willUpdateUrl);

            if (result.ModifiedCount > 0)
            {
                TempData["resultmessage"] = "Link başarıyla güncellenmiştir.";
                TempData["resultbg"] = "bg-success";
            }

            TempData["updatedUrlId"] = editUrlID;
            return RedirectToAction("Links");

        }

        [AuthFilter]
        public ActionResult Dashboard()
        {

            IMongoDatabase database = client.GetDatabase("LinkShortenerService");
            var collection = database.GetCollection<Users>("Users");

            //Users user = (Users)collection.Find(x => x.Email == "m.arda.yuksel@gmail.com").FirstOrDefault();

            Users user = (Users)Session["user"];

            UserLinksViewModel userLinksViewModel = new UserLinksViewModel() { User = user };

            return View(userLinksViewModel);
        }


        [AuthFilter]
        public ActionResult UserProfile()
        {
            IMongoDatabase database = client.GetDatabase("LinkShortenerService");
            IMongoCollection<Users> UsersCollection = database.GetCollection<Users>("Users");

            //Users user = UsersCollection.Find(x => x.Email == "m.arda.yuksel@gmail.com").FirstOrDefault();

            Users user = (Users)Session["user"];



            if (TempData["resultmessage"] != null)
            {
                ViewBag.result = TempData["resultmessage"];
                ViewBag.resultbg = TempData["resultbg"];
            }

            UserProfileViewModel userProfileViewModel = new UserProfileViewModel()
            {
                User = user,

            };

            return View(userProfileViewModel);
        }



        [HttpPost]
        [AuthFilter]
        public ActionResult UserProfile(UserProfileViewModel model)
        {

            IMongoDatabase database = client.GetDatabase("LinkShortenerService");
            IMongoCollection<Users> UsersCollection = database.GetCollection<Users>("Users");

            Users user = (Users)Session["user"];


            Users willUpdateUser = UsersCollection.Find(x => x.ID == user.ID).FirstOrDefault();



            int PhotographCheck = 0;

            if (model.Photograph != null)
            {
                string contentType = model.Photograph.ContentType.Split('/')[1];
                if (contentType == "png" || contentType == "jpeg")
                {

                    var fileExtension = model.Photograph.ContentType.Split('/')[1];
                    var userName = model.User.Name.Replace(" ", "_");
                    var imageName = willUpdateUser.ID + "_" + userName.ToUpper().ToLower() + "_" + model.User.Surname.ToUpper().ToLower() + "." + fileExtension;

                    model.Photograph.SaveAs(Path.Combine(Server.MapPath("~/Uploads/Users/Images"), imageName));

                    model.User.Photograph = imageName;
                    willUpdateUser.Photograph = model.User.Photograph;
                    PhotographCheck = 1;
                }
                else
                {
                    ViewBag.result = "Lütfen geçerli bir fotoğraf seçiniz.";
                    ViewBag.resultbg = "bg-danger";

                    UserProfileViewModel userProfileViewModel = new UserProfileViewModel()
                    {
                        User = model.User,


                    };

                    userProfileViewModel.User.Photograph = willUpdateUser.Photograph;
                    userProfileViewModel.User.DateOfRegistration = willUpdateUser.DateOfRegistration;

                    return View(userProfileViewModel);

                }


            }


            ModelState.Remove("User.Password");



            var temporaryTcNo = "";
            if (model.User.TCNo == null)
            {
                temporaryTcNo = "";
            }
            else
            {
                temporaryTcNo = model.User.TCNo;
            }

            Users t = UsersCollection.Find(x => (x.Email == model.User.Email || x.TCNo == temporaryTcNo) && x.ID != willUpdateUser.ID && x.IsActive).FirstOrDefault();

            if (ModelState.IsValid && t == null)
            {

                if (model.Password != null)
                {
                    willUpdateUser.Password = Crypto.HashPassword(model.Password);
                }

                willUpdateUser.Name = model.User.Name;
                willUpdateUser.Surname = model.User.Surname;
                willUpdateUser.Email = model.User.Email;
                willUpdateUser.TCNo = model.User.TCNo;
                willUpdateUser.Tel = model.User.Tel;


                ReplaceOneResult result = UsersCollection.ReplaceOne(x => x.ID == willUpdateUser.ID, willUpdateUser);

                if (result.ModifiedCount >= 1 || PhotographCheck == 1)
                {
                    TempData["resultmessage"] = "Güncelleme işlemi başarılı.";
                    TempData["resultbg"] = "bg-success";

                    Session["user"] = willUpdateUser;

                    return RedirectToAction("UserProfile");
                }
                else
                {

                    RedirectToAction("UserProfile");
                }


            }
            else
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.result = "Profil güncelleme işlemi başarısız.";
                    ViewBag.resultbg = "bg-danger";


                    UserProfileViewModel userProfileViewModel = new UserProfileViewModel()
                    {
                        User = model.User,

                    };

                    userProfileViewModel.User.Photograph = willUpdateUser.Photograph;
                    userProfileViewModel.User.DateOfRegistration = willUpdateUser.DateOfRegistration;
                    return View(userProfileViewModel);

                }
                else if (t != null)
                {
                    ViewBag.result = "Girilen bilgilere sahip başka bir kullanıcı bulunmaktadır.";
                    ViewBag.resultbg = "bg-danger";

                    UserProfileViewModel userProfileViewModel = new UserProfileViewModel()
                    {
                        User = model.User,

                    };

                    userProfileViewModel.User.Photograph = willUpdateUser.Photograph;
                    userProfileViewModel.User.DateOfRegistration = willUpdateUser.DateOfRegistration;

                    return View(userProfileViewModel);
                }
            }


            return RedirectToAction("UserProfile");

        }



        [AuthFilter]
        public ActionResult CreateShortUrl()
        {


            IMongoDatabase database = client.GetDatabase("LinkShortenerService");
            var collection = database.GetCollection<Users>("Users");

            //Users user = (Users)collection.Find(x => x.Email == "m.arda.yuksel@gmail.com").FirstOrDefault();

            Users user = (Users)Session["user"];



            if (TempData["resultmessage"] != null)
            {
                ViewBag.result = TempData["resultmessage"];
                ViewBag.resultbg = TempData["resultbg"];
            }


            UserCreateShortUrlViewModel userCreateShortUrlViewModel = new UserCreateShortUrlViewModel()
            {
                User = user,

            };




            return View(userCreateShortUrlViewModel);
        }




        [HttpPost]
        [AuthFilter]
        public ActionResult CreateShortUrl(UserCreateShortUrlViewModel model)
        {
            DateTime now = DateTime.Now;

            IMongoDatabase database = client.GetDatabase("LinkShortenerService");
            var usersCollection = database.GetCollection<Users>("Users");

            // Users user = (Users)usersCollection.Find(x => x.Email == "m.arda.yuksel@gmail.com").FirstOrDefault();

            Users user = (Users)Session["user"];

            ModelState.Remove("NewUrl.ValidityDate");
            ModelState.Remove("NewUrl.CreatedAt");
            ModelState.Remove("NewUrl.CreatedBy");
            ModelState.Remove("NewUrl.ClickCount");

            string originalUrl = model.NewUrl.OriginalUrl;


            //Ascii 48-57 numbers - 65-90 uppercase 97-122 lowercase


            int t = 0;
            string shortUrl = "";

            byte[] shortUrlBytes;
            string encodedShortUrl;


            if (ModelState.IsValid)
            {

                var urlsCollection = database.GetCollection<Urls>("Urls");


                if (model.HalfUrlAlias == null)
                {
                    Random rnd = new Random();

                    do
                    {
                        for (int i = 0; i < 7; i++)
                        {

                            switch (rnd.Next(1, 4))
                            {
                                case 1:
                                    t = rnd.Next(97, 123); //lower case
                                    break;

                                case 2:
                                    t = rnd.Next(65, 91); //upper case
                                    break;

                                case 3:
                                    t = rnd.Next(48, 58); //number
                                    break;

                            }

                            shortUrl += Convert.ToChar(t).ToString();

                        }

                        shortUrlBytes = Encoding.UTF8.GetBytes(shortUrl);
                        encodedShortUrl = Convert.ToBase64String(shortUrlBytes);

                    } while (urlsCollection.Find(x => x.ShortenedUrl == encodedShortUrl).FirstOrDefault() != null);



                    model.NewUrl.ShortenedUrl = encodedShortUrl;

                }
                else
                {
                    var shortUrlAliasBytes = Encoding.UTF8.GetBytes(model.HalfUrlAlias);
                    var encodedShortUrlAlias = Convert.ToBase64String(shortUrlAliasBytes);

                    if (urlsCollection.Find(x => x.ShortenedUrl == encodedShortUrlAlias).FirstOrDefault() == null)
                    {

                        model.NewUrl.ShortenedUrl = encodedShortUrlAlias;

                    }
                    else
                    {
                        ModelState.AddModelError("HalfUrlAlias", "Daha önce kullanılmış bir özel url giremezsiniz.");

                        model.User = user;

                        return View(model);
                    }

                }


                Urls url = new Urls();


                var originalUrlBytes = Encoding.UTF8.GetBytes(model.NewUrl.OriginalUrl);
                var encodedOriginalUrl = Convert.ToBase64String(originalUrlBytes);

                url.OriginalUrl = encodedOriginalUrl;


                url.ShortenedUrl = model.NewUrl.ShortenedUrl;

                if (model.ValidityTime != null)
                {
                    if (model.ValidityTime.Value > now && model.ValidityTime.Value < now.AddYears(10))
                    {

                        url.ValidityDate = model.ValidityTime.Value.ToString("yyyy-MM-ddTHH:mm");

                        //url.ValidityDate = model.ValidityTime.Value.ToString("yyyyMMddHHmmssffff");
                    }
                    else
                    {
                        ModelState.AddModelError("ValidityTime", "Lütfen Geçerli bir tarih aralığı seçiniz.");

                        model.User = user;

                        return View(model);
                    }

                }
                else
                {
                    var temp = now;

                    var tempNow = new DateTime(temp.Year, temp.Month, temp.Day, 0, 0, 0);

                    url.ValidityDate = tempNow.AddDays(14).ToString("yyyy-MM-ddTHH:mm");
                    //url.ValidityDate = tempNow.AddDays(14).ToString("yyyyMMddHHmmssffff");
                }

                url.CreatedAt = now.AddHours(3);
                url.CreatedBy = user.ID;


                if (model.NewUrl.Title != null)
                {
                    url.Title = model.NewUrl.Title;
                }

                url.IsActive = true;


                urlsCollection.InsertOne(url);



                TempData["resultmessage"] = "Url başarıyla oluşturulmuştur.";
                TempData["resultbg"] = "bg-success";
                return RedirectToAction("CreateShortUrl");


            }
            else
            {

                model.User = user;

                return View(model);
            }





        }




        public ActionResult UrlRedirect(string shortenedUrl)
        {

            IMongoDatabase database = client.GetDatabase("LinkShortenerService");
            IMongoCollection<Urls> urlsCollection = database.GetCollection<Urls>("Urls");

            IMongoCollection<UrlsAccessDatesLocations> urlsAccessDatesLocationsCollection = database.GetCollection<UrlsAccessDatesLocations>("UrlsAccessDatesLocations");


            if (shortenedUrl == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var shortenedUrlBytes = Encoding.UTF8.GetBytes(shortenedUrl);
                var encodedShortenedUrl = Convert.ToBase64String(shortenedUrlBytes);

                Urls urlDocument = urlsCollection.Find(x => x.ShortenedUrl == encodedShortenedUrl).FirstOrDefault();

                if (urlDocument != null)
                {
                    if (urlDocument.IsActive)
                    {

                        var urlValidityDate = urlDocument.ValidityDate;

                        var yearMonthDay = urlValidityDate.Substring(0, urlValidityDate.IndexOf("T"));
                        var hourMinute = urlValidityDate.Substring((urlValidityDate.IndexOf("T") + 1));

                        var splitYMD = yearMonthDay.Split('-');
                        var splitHm = hourMinute.Split(':');

                        var validityDate = new DateTime(Convert.ToInt32(splitYMD[0]), Convert.ToInt32(splitYMD[1]), Convert.ToInt32(splitYMD[2]), Convert.ToInt32(splitHm[0]), Convert.ToInt32(splitHm[1]), 0);

                        if (DateTime.Now < validityDate)
                        {
                            urlDocument.ClickCount++;
                            urlsCollection.ReplaceOne(x => x.ID == urlDocument.ID, urlDocument);

                            var decodedOriginalUrlBytes = Convert.FromBase64String(urlDocument.OriginalUrl);
                            var OriginalUrl = Encoding.UTF8.GetString(decodedOriginalUrlBytes);


                            //UrlsAccessDatesLocations urlsAccessDatesLocations = new UrlsAccessDatesLocations();

                            //urlsAccessDatesLocations.UrlID = urlDocument.ID;


                            return Redirect(OriginalUrl);
                        }
                        else
                        {
                            urlDocument.IsActive = false;

                            urlsCollection.ReplaceOne(x => x.ID == urlDocument.ID, urlDocument);

                            TempData["resultmessage"] = "Linkin geçerlilik süresi dolmuştur.";
                            TempData["resultbg"] = "bg-warning";

                            return RedirectToAction("Index", "Home");
                        }



                    }
                    else
                    {
                        TempData["resultmessage"] = "Linkin geçerlilik süresi dolmuştur.";
                        TempData["resultbg"] = "bg-warning";
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }






        }


        [AuthFilter]
        public ActionResult Signout()
        {
            Session.Clear();

            return RedirectToAction("Login","Home");
        }


    }
}