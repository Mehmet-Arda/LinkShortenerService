using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LinkShortenerService
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
              name: "UrlRedirect",
              url: "{shortenedUrl}",
              defaults: new { controller = "User", action = "UrlRedirect", shortenedUrl = UrlParameter.Optional }
          );


            routes.MapRoute(
             name: "hosgeldiniz",
             url: "hosgeldiniz/anasayfa",
             defaults: new { controller = "Home", action = "Index" }
         );


            routes.MapRoute(
            name: "girisyap",
            url: "hosgeldiniz/girisyap",
            defaults: new { controller = "Home", action = "Login" }
        );


            routes.MapRoute(
             name: "kaydol",
             url: "hosgeldiniz/kaydol",
             defaults: new { controller = "Home", action = "Signin" }
         );


            routes.MapRoute(
                 name: "sifremi-unuttum",
                 url: "hosgeldiniz/sifremi-unuttum",
                 defaults: new { controller = "Home", action = "ForgotPassword" }
             );



            routes.MapRoute(
                 name: "anasayfa",
                 url: "kullanici/linklerim",
                 defaults: new { controller = "User", action = "Links" }
             );



            routes.MapRoute(
                 name: "yeniurl",
                 url: "kullanici/yeni-url-olustur",
                 defaults: new { controller = "User", action = "CreateShortUrl" }
             );



            routes.MapRoute(
                 name: "dashboard",
                 url: "kullanici/dashboard",
                 defaults: new { controller = "User", action = "Dashboard" }
             );


            routes.MapRoute(
              name: "profilim",
              url: "kullanici/profilim",
              defaults: new { controller = "User", action = "UserProfile" }
          );




            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );






        }
    }
}
