using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TempoDigitalApex3.Models;

namespace TempoDigitalApex3.Controllers
{
    public class ConsentScreenController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ConsentScreen
        public ActionResult Index()
        {
            ViewBag.ClientID = ConfigurationManager.AppSettings["GooglesSigninClientID"];

            //var dates = db.YoutubeConnect.ToList();
            //foreach (var x in dates)
            //{
            //    if (x.Created.Contains("/"))
            //    {
            //        var date = DateTime.ParseExact(x.Created, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //        //var date = Convert.ToDateTime(x.Created, CultureInfo.InvariantCulture);
            //        x.Created = date.ToString("yyyy-MM-dd HH:mm:ss");
            //        db.SaveChanges();
            //    }

                
            //}

            return View();
        }


        public ActionResult SaveConnectFormData()
        {
            var obj = new YoutubeConnect();
            obj.FirstName = Request.Form["FirstName"];
            obj.LastName = Request.Form["LastName"];
            obj.Email = Request.Form["Email"];
            obj.Country = Request.Form["Country"];
            obj.State = Request.Form["State"];
            obj.DOB = Request.Form["DOB"]; ;
            obj.CurrentNetwork = Request.Form["Network"];
            obj.ChannelID = Request.Form["ID"];
            obj.AccountTitle = Request.Form["AT"];
            obj.SubscribersCount = Request.Form["SC"];
            obj.CommentCount = Request.Form["CC"];
            obj.VideoCount = Request.Form["VDC"];
            obj.ViewCount = Request.Form["VWC"];
            obj.PublishedTime = Request.Form["PBAT"];
            obj.Descriptions = Request.Form["DES"];
            obj.CommunityGuidelinesGS = Request.Form["CGGS"];
            obj.ContentIdClaimsGS = Request.Form["CCGS"];
            obj.CopyrightStrikesGS = Request.Form["CRGS"];
            obj.ImageURL = Request.Form["URL"];
            obj.Status = "Applied";
            //obj.Created = DateTime.Now.Date.ToString("dd/MM/yyyy");
            obj.Created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            obj.AccountStatus = "Not Created";

            db.YoutubeConnect.Add(obj);
            db.SaveChanges();

            return RedirectToAction("ConnectSuccess");
        }

        public ActionResult ConnectSuccess()
        {
            return View();
        }
    }
}