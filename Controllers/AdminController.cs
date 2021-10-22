using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TempoDigitalApex3.Models;
using System.Threading;
using System.IO;
using Tamir.SharpSsh;
using Tamir.Streams;
using System.Text;
using Renci.SshNet;
using Microsoft.Azure.Storage;
using Microsoft.Azure;
using Microsoft.Azure.Storage.Blob;
using Renci.SshNet.Sftp;
using Microsoft.Reporting.WebForms;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Globalization;
using PagedList;

namespace TempoDigitalApex3.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult UserManagementAdmin()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");
            try
            {

                ViewBag.UserList = db.Users.ToList().OrderByDescending(s => s.DateTimeCreated);
            }
            catch
            {
                ViewBag.UserList = null;
            }


            return View();
        }

        public ActionResult ConnectRequestAdmin()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            var ls = db.YoutubeConnect.ToList();
            return View(ls);           
        }

        public class PaymentAdmin
        {
            public string network { get; set; }
            public string user { get; set; }
            public string email { get; set; }
            public string channelName { get; set; }
            public string channellID { get; set; }
            public string share { get; set; }
            public string income { get; set; }
            public string paid { get; set; }
            public string datePaid { get; set; }
            public string total { get; set; }
        }

        public ActionResult YoutubePaymentAdmin(string year, string month)
        {
            if (Session["admin_session"] == null)
            {
                return RedirectToAction("Login");
            }
            try
            {
                //var finalList = new List<PaymentAdmin>();
                //var music_revenue = 0.0;
                //var music_share = 0.0;

                //var ent_music_revenue = 0.0;
                //var ent_music_share = 0.0;

                if(month != null && year != null)
                {
                    //var userList = db.Users.Where(a => a.share != "").ToList();
                    //var musicChannelList = db.YoutubeChannels.ToList();
                    //var musicList = db.YT_Reports_Music_2020.Where(a => a.Month.Equals(month) && a.Year.Equals(year)).ToList();

                    //var ent_musicChannelList = db.GetYoutubeChannelsENT.ToList();
                    //var ent_musicList = db.YT_Reports_Ent_2020.Where(a => a.Month.Equals(month) && a.Year.Equals(year)).ToList();

                    //foreach (var u in userList)
                    //{
                    //    var channels = musicChannelList.Where(a => a.UserId.Equals(u.Id)).ToList();
                    //    foreach (var c in channels)
                    //    {
                    //        var music = musicList.Where(a => a.Asset_Channel_ID.Equals(c.ChannelID)).ToList();
                    //        var i = Math.Round(music.Sum(a => a.Partner_Revenue), 2);
                    //        var s = Math.Round((i / 100) * Int32.Parse(u.share), 2);

                    //        music_revenue += (double)i;
                    //        music_share += (double)s;

                    //        var obj = new PaymentAdmin();
                    //        obj.network = "Music";
                    //        obj.user = u.FirstName + " " + u.LastName;
                    //        obj.email = u.Email;
                    //        obj.channelName = c.ChannelName;
                    //        obj.channellID = c.ChannelID;
                    //        obj.income = i.ToString();
                    //        obj.share = s.ToString();
                    //        obj.total = (i - s).ToString();
                    //        obj.paid = "";
                    //        obj.datePaid = "";

                    //        finalList.Add(obj);
                    //    }

                    //    var entchannels = ent_musicChannelList.Where(a => a.UserId.Equals(u.Id)).ToList();
                    //    foreach (var c in entchannels)
                    //    {
                    //        var music = ent_musicList.Where(a => a.Asset_Channel_ID.Equals(c.ChannelID)).ToList();
                    //        var i = Math.Round(music.Sum(a => a.Partner_Revenue), 2);
                    //        var s = Math.Round((i / 100) * Int32.Parse(u.share), 2);

                    //        ent_music_revenue += (double)i;
                    //        ent_music_share += (double)s;

                    //        var obj = new PaymentAdmin();
                    //        obj.network = "Music";
                    //        obj.user = u.FirstName + " " + u.LastName;
                    //        obj.email = u.Email;
                    //        obj.channelName = c.ChannelName;
                    //        obj.channellID = c.ChannelID;
                    //        obj.income = i.ToString();
                    //        obj.share = s.ToString();
                    //        obj.total = (i - s).ToString();
                    //        obj.paid = "";
                    //        obj.datePaid = "";

                    //        finalList.Add(obj);
                    //    }
                    //}

                    //ViewBag.FinalList = finalList;
                    //ViewBag.MusicIncome = Math.Round(music_revenue, 2);
                    //ViewBag.MusicOutcome = Math.Round(music_share, 2);

                    //ViewBag.EntMusicIncome = Math.Round(ent_music_revenue, 2);
                    //ViewBag.EntMusicOutcome = Math.Round(ent_music_share, 2);

                    var list = db.MonthlyPayments.Where(a=>a.Music_Month.Equals(month) && a.Music_Year.Equals(year) && a.Is_Paid == true).ToList();
                    ViewBag.FinalList = list.OrderByDescending(a => a.ID);

                    var music_total = 0.00;
                    var ent_total = 0.00;
                    foreach(var x in list)
                    {
                        music_total += Convert.ToDouble(x.MusicAmount);
                        if(x.Ent_Month != null && x.Ent_Year!= null)
                        {
                            ent_total += Convert.ToDouble(x.EntAmount);
                        }
                    }

                    ViewBag.MusicOutcome = Math.Round(music_total, 2);
                    ViewBag.EntMusicOutcome = Math.Round(ent_total, 2);

                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                else
                {
                    ViewBag.FinalList = null;
                    //ViewBag.MusicIncome = 0;
                    ViewBag.MusicOutcome = 0;
                    //ViewBag.EntMusicIncome = 0;
                    ViewBag.EntMusicOutcome = 0;

                    ViewBag.Month = 0;
                    ViewBag.Year = 0;
                }  
                

                
            }
            catch
            {
                ViewBag.FinalList = null;
                //ViewBag.MusicIncome = 0;
                ViewBag.MusicOutcome = 0;
                //ViewBag.EntMusicIncome = 0;
                ViewBag.EntMusicOutcome = 0;

                ViewBag.Month = 0;
                ViewBag.Year = 0;
            }

            ViewBag.Key = ConfigurationManager.AppSettings["APIKEY"];
            ViewBag.Token = GetAccessTokenHerokuAPI();

            return View();
        }

        public class channelsData
        {
            public string network { get; set; }
            public string user { get; set; }
            public string email { get; set; }
            public string channelName { get; set; }
            public string channellID { get; set; }
            public string share { get; set; }
        }
        public ActionResult YoutubeChannelsAdmin()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            
            var finalList = new List<channelsData>();
            var users = db.Users.ToList();
            var musicChannelList = db.YoutubeChannels.ToList();
            foreach(var ch in musicChannelList)
            {
                var u = users.Where(a => a.Id == ch.UserId).FirstOrDefault();
                if(u != null)
                {
                    var obj = new channelsData();
                    obj.network = "Music";
                    obj.user = u.FirstName + " " + u.LastName;
                    obj.email = u.Email;
                    obj.channelName = ch.ChannelName;
                    obj.channellID = ch.ChannelID;
                    obj.share = u.share;
                    finalList.Add(obj);
                } 
            }

            var entChannelList = db.GetYoutubeChannelsENT.ToList();
            foreach (var ch in entChannelList)
            {
                var u = users.Where(a => a.Id == ch.UserId).FirstOrDefault();
                if (u != null)
                {
                    var obj = new channelsData();
                    obj.network = "Entertainment";
                    obj.user = u.FirstName + " " + u.LastName;
                    obj.email = u.Email;
                    obj.channelName = ch.ChannelName;
                    obj.channellID = ch.ChannelID;
                    obj.share = u.share;
                    finalList.Add(obj);
                }

            }

            ViewBag.List = finalList;
            return View();
        }

        public ActionResult YoutubeConnectDetailsAdmin(string Id)
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            if (Id == null)
                return RedirectToAction("ConnectRequest");

            int ID = Int32.Parse(Id);
            var obj = db.YoutubeConnect.Where(a => a.Id == ID).FirstOrDefault();
            if (obj != null)
            {
                if (obj.CommunityGuidelinesGS.Equals("true"))
                {
                    obj.CommunityGuidelinesGS = "Good";
                }
                else
                {
                    obj.CommunityGuidelinesGS = "Bad";
                }

                if (obj.ContentIdClaimsGS.Equals("true"))
                {
                    obj.ContentIdClaimsGS = "Good";
                }
                else
                {
                    obj.ContentIdClaimsGS = "Bad";
                }

                if (obj.CopyrightStrikesGS.Equals("true"))
                {
                    obj.CopyrightStrikesGS = "Good";
                }
                else
                {
                    obj.CopyrightStrikesGS = "Bad";
                }
            }
            return View(obj);
        }

        public ActionResult YouTubeAnalyticsAdmin()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");
            ViewBag.Key = ConfigurationManager.AppSettings["APIKEY"];
            //ViewBag.Token = GetAccessToken();
            ViewBag.Token = GetAccessTokenHerokuAPI();
            
            return View();
        }
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult MonthlyPaidData()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            ViewBag.UserEmails = db.Users.ToList();

            var years = new List<String>();
            int year = DateTime.Now.Year;
            while (year > 2000)
            {
                years.Add(year.ToString());
                year = year - 1;
            }
            ViewBag.Years = years;
            var list = db.MonthlyPayments.ToList();
            ViewBag.sortedList = list.OrderByDescending(a => a.ID);
            return View(list);
        }

        public ActionResult DistributionReleaseDetails(int? id)
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            var songs = db.Songs.Where(a => a.MusicReleaseId == id).ToList();
            if (songs.Count == 0)
            {
                return RedirectToAction("UserDistributions");
            }
            ViewBag.songs = songs;

            var dis = db.MusicReleases.Where(a => a.MusicReleaseId == id).FirstOrDefault();
            ViewBag.User = db.Users.Where(a => a.Id.Equals(dis.UserId)).FirstOrDefault();
            ViewBag.Dis = dis;
            return View();
        }

        public ActionResult DistributionAnalyticsAdmin()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");
            var releaselist = db.MusicReleases.Where(x => x.Status != "Deleted").ToList();
            var labels = new List<string>();
            foreach (var x in releaselist)
            {
                if (x.LabelName != null && !labels.Contains(x.LabelName))
                {
                    labels.Add(x.LabelName);
                }
            }
            ViewBag.Labels = labels;
            return View();
        }

        public ActionResult MyAccountAdmin()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        public ActionResult saveMailTemplate()
        {
            var templates = db.mail_Templates.ToList();
            var AcceptChannel = templates.Where(a => a.type.Equals("AcceptChannel")).FirstOrDefault();
            if(AcceptChannel != null)
            {
                AcceptChannel.template = Request.Form["AcceptChannel"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["AcceptChannel"];
                obj.type = "AcceptChannel";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            var DeclineChannel = templates.Where(a => a.type.Equals("DeclineChannel")).FirstOrDefault();
            if (DeclineChannel != null)
            {
                DeclineChannel.template = Request.Form["DeclineChannel"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["DeclineChannel"];
                obj.type = "DeclineChannel";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            var DistributionCreated = templates.Where(a => a.type.Equals("DistributionCreated")).FirstOrDefault();
            if (DistributionCreated != null)
            {
                DistributionCreated.template = Request.Form["DistributionCreated"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["DistributionCreated"];
                obj.type = "DistributionCreated";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            var DistributionUpdated = templates.Where(a => a.type.Equals("DistributionUpdated")).FirstOrDefault();
            if (DistributionUpdated != null)
            {
                DistributionUpdated.template = Request.Form["DistributionUpdated"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["DistributionUpdated"];
                obj.type = "DistributionUpdated";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            var DistributionDeleted = templates.Where(a => a.type.Equals("DistributionDeleted")).FirstOrDefault();
            if (DistributionDeleted != null)
            {
                DistributionDeleted.template = Request.Form["DistributionDeleted"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["DistributionDeleted"];
                obj.type = "DistributionDeleted";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            var YoutubeReport = templates.Where(a => a.type.Equals("YoutubeReport")).FirstOrDefault();
            if (YoutubeReport != null)
            {
                YoutubeReport.template = Request.Form["YoutubeReport"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["YoutubeReport"];
                obj.type = "YoutubeReport";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            var DistributionReport = templates.Where(a => a.type.Equals("DistributionReport")).FirstOrDefault();
            if (DistributionReport != null)
            {
                DistributionReport.template = Request.Form["DistributionReport"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["DistributionReport"];
                obj.type = "DistributionReport";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            var NewRegistration = templates.Where(a => a.type.Equals("NewRegistration")).FirstOrDefault();
            if (NewRegistration != null)
            {
                NewRegistration.template = Request.Form["NewRegistration"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["NewRegistration"];
                obj.type = "NewRegistration";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            var ChangePassword = templates.Where(a => a.type.Equals("ChangePassword")).FirstOrDefault();
            if (ChangePassword != null)
            {
                ChangePassword.template = Request.Form["ChangePassword"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["ChangePassword"];
                obj.type = "ChangePassword";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            var AccountConfirmation = templates.Where(a => a.type.Equals("AccountConfirmation")).FirstOrDefault();
            if (AccountConfirmation != null)
            {
                AccountConfirmation.template = Request.Form["AccountConfirmation"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["AccountConfirmation"];
                obj.type = "AccountConfirmation";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            return RedirectToAction("SettingsAdmin");
        }

        [HttpPost]
        public ActionResult saveMailAddress()
        {
            var templates = db.mail_Templates.ToList();
            var YoutubeEmail = templates.Where(a => a.type.Equals("YoutubeEmail")).FirstOrDefault();
            if (YoutubeEmail != null)
            {
                YoutubeEmail.template = Request.Form["YoutubeEmail"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["YoutubeEmail"];
                obj.type = "YoutubeEmail";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            var DistEmail = templates.Where(a => a.type.Equals("DistEmail")).FirstOrDefault();
            if (DistEmail != null)
            {
                DistEmail.template = Request.Form["DistEmail"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["DistEmail"];
                obj.type = "DistEmail";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            var SupportEmail = templates.Where(a => a.type.Equals("SupportEmail")).FirstOrDefault();
            if (SupportEmail != null)
            {
                SupportEmail.template = Request.Form["SupportEmail"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["SupportEmail"];
                obj.type = "SupportEmail";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            var MinimumSubs = templates.Where(a => a.type.Equals("MinimumSubs")).FirstOrDefault();
            if (MinimumSubs != null)
            {
                MinimumSubs.template = Request.Form["MinimumSubs"];
                db.SaveChanges();
            }
            else
            {
                var obj = new mail_templates();
                obj.template = Request.Form["MinimumSubs"];
                obj.type = "MinimumSubs";
                db.mail_Templates.Add(obj);
                db.SaveChanges();
            }

            return RedirectToAction("SettingsAdmin");
        }

        public ActionResult SettingsAdmin()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");
            var templates = db.mail_Templates.ToList();
            ViewBag.AcceptChannel = templates.Where(a => a.type.Equals("AcceptChannel")).FirstOrDefault();
            ViewBag.DeclineChannel = templates.Where(a => a.type.Equals("DeclineChannel")).FirstOrDefault();
            ViewBag.DistributionCreated = templates.Where(a => a.type.Equals("DistributionCreated")).FirstOrDefault();
            ViewBag.DistributionUpdated = templates.Where(a => a.type.Equals("DistributionUpdated")).FirstOrDefault();
            ViewBag.DistributionDeleted = templates.Where(a => a.type.Equals("DistributionDeleted")).FirstOrDefault();
            ViewBag.YoutubeReport = templates.Where(a => a.type.Equals("YoutubeReport")).FirstOrDefault();
            ViewBag.DistributionReport = templates.Where(a => a.type.Equals("DistributionReport")).FirstOrDefault();
            ViewBag.NewRegistration = templates.Where(a => a.type.Equals("NewRegistration")).FirstOrDefault();
            ViewBag.ChangePassword = templates.Where(a => a.type.Equals("ChangePassword")).FirstOrDefault();
            ViewBag.AccountConfirmation = templates.Where(a => a.type.Equals("AccountConfirmation")).FirstOrDefault();
            ViewBag.YoutubeEmail = templates.Where(a => a.type.Equals("YoutubeEmail")).FirstOrDefault();
            ViewBag.DistEmail = templates.Where(a => a.type.Equals("DistEmail")).FirstOrDefault();
            ViewBag.SupportEmail = templates.Where(a => a.type.Equals("SupportEmail")).FirstOrDefault();
            ViewBag.MinimumSubs = templates.Where(a => a.type.Equals("MinimumSubs")).FirstOrDefault();
            return View();
        }

        public ActionResult ApprovedReleases()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            ViewBag.Users = db.Users.ToList();
            ViewBag.UserDistributions = db.MusicReleases.Where(a => a.Status == "Approved").ToList();
            return View();

        }

        public ActionResult MonthlyPaymentAdmin()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            ViewBag.UserEmails = db.Users.Where(a=>a.isActive == true).ToList();

            var years = new List<String>();
            int year = DateTime.Now.Year;
            while (year > 2000)
            {
                years.Add(year.ToString());
                year = year - 1;
            }
            ViewBag.Years = years;
            var list = db.MonthlyPayments.Where(a=>a.Is_Paid == false).ToList();
            ViewBag.sortedList = list.OrderByDescending(a => a.ID);
            return View(list);
        }

        public ActionResult DeletedReleases()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");
            ViewBag.DeleteReleaseList = db.MusicReleases.Where(a => a.Status.Equals("Deleted")).ToList();
            ViewBag.UserDistributions = db.MusicReleases.Where(a => a.Status == null)
            .OrderByDescending(b => b.Sales_Start_Date).ToList();
            return View();
        }

        public ActionResult UpdatedReleases()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");
            ViewBag.DeleteReleaseList = db.MusicReleases.Where(a => a.Update_Status.Equals("Updated") && !a.Status.Equals("Deleted")).ToList();
            ViewBag.UserDistributions = db.MusicReleases.Where(a => a.Status == null)
            .OrderByDescending(b => b.Sales_Start_Date).ToList();
            return View();
        }

        public ActionResult DistributionRequestsAdmin()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");
            ViewBag.DeleteReleaseList = db.MusicReleases.Where(a => a.Status.Equals("Deleted")).ToList();
            ViewBag.UserDistributions = db.MusicReleases.Where(a => a.Status == null)
            .OrderByDescending(b => b.Sales_Start_Date).ToList();
            return View();
        }

        public ActionResult SumarizedReportsAdmin()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");
            return View();
        }

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            //var users = db.Users.ToList();
            //foreach(var x in users)
            //{
            //    x.isActive = true;
            //    db.SaveChanges();
            //}

            //var user = db.Users.Where(a => a.Email.Equals("ather1rafiq@gmail.com")).FirstOrDefault();
            //if(user != null)
            //{
            //    user.isActive = false;
            //    db.SaveChanges();
            //}

            //var dates = db.MonthlyPayments.ToList();
            //foreach(var x in dates)
            //{
            //    var date = Convert.ToDateTime(x.Date_Time, CultureInfo.InvariantCulture);
            //    x.Date_Time = date.ToString("yyyy-MM-dd HH:mm:ss");
            //    db.SaveChanges();
            //}

            return View();
        }

        public ActionResult ApproveRelease(string id)
        {
            if(id == null)
            {
                return RedirectToAction("DistributionRequestsAdmin");
            }

            int ID = Int32.Parse(id);
            var res = db.MusicReleases.Where(a => a.MusicReleaseId == ID).FirstOrDefault();

            if(res != null)
            {
                res.Status = "Distributed";
                db.SaveChanges();
            }

            return RedirectToAction("DistributionReleaseDetails", new { id = id });
        }
        
        public ActionResult SystemLog()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            try
            {
                List<UserLogs> logList = db.UserLogs.OrderByDescending(b => b.Id).ToList();
                ViewBag.LogList = logList;
            }
            catch
            {
                ViewBag.LogList = null;
            }
            
            return View();
        }

        public ActionResult UserManagement()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            ViewBag.UserEmails = db.Users.ToList();
            return View();
        }

        public ActionResult UserDistributions()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");
            ViewBag.DeleteReleaseList = db.MusicReleases.Where(a => a.Status.Equals("Deleted")).ToList();
            ViewBag.UserDistributions = db.MusicReleases.Where(a => a.Status == null)
             .OrderByDescending(b => b.Sales_Start_Date).ToList();

            return View();
        }

        public ActionResult UserDistributionsApproved()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            ViewBag.Users = db.Users.ToList();
            ViewBag.UserDistributions = db.MusicReleases.Where(a => a.Status == "Approved").ToList();
            return View();
        }

        public ActionResult UserDistributionsSongs(int? id)
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            var songs = db.Songs.Where(a => a.MusicReleaseId == id).ToList();
            if (songs.Count == 0)
            {
                return RedirectToAction("UserDistributions");
            }
            ViewBag.songs = songs;

            var dis = db.MusicReleases.Where(a => a.MusicReleaseId == id).FirstOrDefault();
            ViewBag.User = db.Users.Where(a => a.Id.Equals(dis.UserId)).FirstOrDefault();
            ViewBag.Dis = dis;
            return View();
        }

        public ActionResult ReleaseDistributed(int id)
        {
            var dis = db.MusicReleases.Where(a => a.MusicReleaseId == id).FirstOrDefault();
            if (dis != null)
            {
                dis.Status = "Distributed";
                db.SaveChanges();
            }
            return RedirectToAction("ApprovedReleases");
        }

        public ActionResult ApproveDistribution(int? id)
        {
            var dis = db.MusicReleases.Where(a => a.MusicReleaseId == id).FirstOrDefault();
            if (dis != null)
            {
                dis.Status = "Approved";
                dis.currentDateTime = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("UserDistributions");
        }

        public ActionResult Reports()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            var users = db.Users.ToList();
            var channelList = db.YoutubeChannels.Where(a => a.Approved == false).ToList();
            var userName = new List<string>();
            foreach (var x in channelList)
            {
                var obj = db.Users.Where(a => a.Id.Equals(x.UserId)).FirstOrDefault();
                if (obj != null)
                    userName.Add(obj.FirstName + " " + obj.LastName);
                else
                    userName.Add("");
            }
            
            ViewBag.UserList = userName;

            var channelListEnt = db.GetYoutubeChannelsENT.Where(a => a.Approved == false).ToList();
            var userNameEnt = new List<string>();
            foreach (var x in channelListEnt)
            {
                var obj = db.Users.Where(a => a.Id.Equals(x.UserId)).FirstOrDefault();
                if (obj != null)
                    userNameEnt.Add(obj.FirstName + " " + obj.LastName);
                else
                    userNameEnt.Add("");
            }
            ViewBag.ApprovalEntChannelList = channelListEnt;
            ViewBag.EntUsers = userNameEnt;
            //ViewBag.ReleasesLog = db.UserLogs.Where(a => a.ActionPerformed.Equals("Release Created")).ToList();

            return View(channelList);
        }

        public ActionResult ViewReleaseAdmin(string search, string filter, int? i, string date)
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");


            if (date != null)
            {
                var list = db.MusicReleases.Where(x => x.PaymentStaus.Equals("Paid") && (x.Status == "Distributed" || x.Status == "Approved" || x.Status == null || x.Status != "Deleted")).ToList();
                //var finalList = new List<MusicRelease>();

                if (date.Equals("dsen"))
                {
                    var newList = new List<MusicRelease>();
                    for (int j = list.Count - 1; j > -1; j--)
                    {
                        newList.Add(list[j]);
                    }

                    return View(newList.ToPagedList(i ?? 1, 18));
                }
                else if (date.Equals("all"))
                {
                    return RedirectToAction("Index", "MusicReleases");
                }
                else
                {
                    //var sd = get_list_images(list.ToPagedList(i ?? 1, 18).ToList());
                    //return View(sd.ToPagedList(i ?? 1, 18));
                    //return View(get_list_images(list.ToPagedList(i ?? 1, 18).ToList()));
                    return View(list.ToPagedList(i ?? 1, 18));
                }
            }
            else if (search == null || filter == null)
            {
                var list = db.MusicReleases.Where(x => x.PaymentStaus.Equals("Paid") && (x.Status == "Distributed" || x.Status == "Approved" || x.Status == null || x.Status != "Deleted")).ToList();
                var newList = new List<MusicRelease>();
                for (int j = list.Count - 1; j > -1; j--)
                {
                    newList.Add(list[j]);
                }

                return View(newList.ToPagedList(i ?? 1, 18));
                //return View(get_list_images(newList.ToPagedList(i ?? 1, 18).ToList()));
            }
            else
            {
                var list = db.MusicReleases.Where(x => x.PaymentStaus.Equals("Paid") && (x.Status == "Distributed" || x.Status == "Approved" || x.Status == null || x.Status != "Deleted")).ToList();
                var finalList = new List<MusicRelease>();
                foreach (var x in list)
                {
                    if (filter == "Name")
                    {
                        if (x.ReleaseName != null)
                        {
                            if (x.ReleaseName.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) != -1)
                                finalList.Add(x);
                        }
                    }
                    else if (filter == "Artist")
                    {
                        if (x.MainArtist != null)
                        {
                            if (x.MainArtist.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) != -1)
                                finalList.Add(x);
                        }
                    }
                    else if (filter == "UPC")
                    {
                        if (x.UPCEAN != null)
                        {
                            if (x.UPCEAN.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) != -1)
                                finalList.Add(x);
                        }
                    }
                    else if (filter == "RecordingLabel")
                    {
                        if (x.LabelName != null)
                        {
                            if (x.LabelName.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) != -1)
                                finalList.Add(x);
                        }
                    }
                    else if (filter == "All")
                    {
                        finalList = list;
                    }
                }

                return View(finalList.ToPagedList(i ?? 1, 18));
            }
        }

        public ActionResult ReleaseDetailsAdmin(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "MusicReleases", new { id = 0 });
            }

            var list = db.Songs.Where(a => a.MusicReleaseId == id).ToList();
            var release = db.MusicReleases.Where(a => a.MusicReleaseId == id).FirstOrDefault();
            if (release != null)
            {
                ViewBag.Release = release;
                ViewBag.Part = db.Participants.Where(a => a.MusicReleaseId == release.MusicReleaseId).ToList();

                var cover = "";
                if (release.CoverImage != null)
                {
                    cover = release.CoverImage.Substring(0, release.CoverImage.Length - 3);
                    if (cover.EndsWith("_U."))
                    {
                        cover = cover.Replace("_U", "");
                    }
                    cover += "csv";
                }
                ViewBag.csv = cover;

                ViewBag.APIStatus = check_Tempo_API(release.UPCEAN);
                if (ViewBag.APIStatus)
                {
                    ViewBag.APIData = get_Tempo_aPI_response(release.UPCEAN);
                }
                else
                {
                    ViewBag.APIData = "";
                }
            }

            //var report = db.SpotifyReportFull.Where(a => a.UPC.Equals(release.UPCEAN)).ToList();

            return View(list);
        }


        public bool check_Tempo_API(string upc)
        {
            string url = @"https://api.audiosalad.com/fetch.php?k=19E1170F02FCED03938BE12B241E09034BDDD5E397902460B43F6197C40260A0&g_profile=tempodigital.audiosalad.com&upc=" + upc;
            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetAsync(url).Result;
            if (result.StatusCode.ToString() == "OK")
            {
                return true;
            }
            return false;
        }

        public string get_Tempo_aPI_response(string upc)
        {
            string url = @"https://api.audiosalad.com/fetch.php?k=19E1170F02FCED03938BE12B241E09034BDDD5E397902460B43F6197C40260A0&g_profile=tempodigital.audiosalad.com&upc=" + upc;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd().ToString();
            }
        }


        //public ActionResult YoutubeEnt()
        //{
        //    if (Session["admin_session"] == null)
        //        return RedirectToAction("Login");
        //    ViewBag.Key = ConfigurationManager.AppSettings["APIKEY"];
        //    ViewBag.Token = GetAccessToken();
        //    return View();
        //}

        public ActionResult YoutubeAnalytics()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");
            ViewBag.Key = ConfigurationManager.AppSettings["APIKEY"];
            //ViewBag.Token = GetAccessToken();
            ViewBag.Token = GetAccessTokenHerokuAPI();
            return View();
        }

        public string GetAccessTokenHerokuAPI()
        {
            string access_token;
            string url = @"https://tempo-digital.herokuapp.com/token?password=tempo_password";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                access_token = reader.ReadToEnd();
                return access_token;
            }
        }

        public string GetAccessToken()
        {
            HttpClient client = new HttpClient();
            string access_token;

            var values = new Dictionary<string, string>
            {
               { "client_id", ConfigurationManager.AppSettings["GoogleClientID"] },
               { "client_secret", ConfigurationManager.AppSettings["GoogleSecretID"] },
               { "refresh_token", ConfigurationManager.AppSettings["GoogleAPIRefreshToken"] },
               { "grant_type", "refresh_token" }
            };

            var content = new FormUrlEncodedContent(values);
            var response = client.PostAsync("https://www.googleapis.com/oauth2/v4/token", content);
            string json = response.Result.Content.ReadAsStringAsync().Result;
            dynamic obj = JObject.Parse(json);

            access_token = obj.access_token;
            return access_token;
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ConnectRequest()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            var ls = db.YoutubeConnect.ToList();
            return View(ls);
        }

        public ActionResult YoutubeConnectDetails(string Id)
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            if (Id == null)
                return RedirectToAction("ConnectRequest");

            int ID = Int32.Parse(Id);
            var obj = db.YoutubeConnect.Where(a => a.Id == ID).FirstOrDefault();
            if(obj != null)
            {
                if (obj.CommunityGuidelinesGS.Equals("true"))
                {
                    obj.CommunityGuidelinesGS = "Good";
                }
                else
                {
                    obj.CommunityGuidelinesGS = "Bad";
                }

                if (obj.ContentIdClaimsGS.Equals("true"))
                {
                    obj.ContentIdClaimsGS = "Good";
                }
                else
                {
                    obj.ContentIdClaimsGS = "Bad";
                }

                if (obj.CopyrightStrikesGS.Equals("true"))
                {
                    obj.CopyrightStrikesGS = "Good";
                }
                else
                {
                    obj.CopyrightStrikesGS = "Bad";
                }
            }
            return View(obj);
        }

        public ActionResult ApproveConnect(string Id)
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            if (Id == null)
                return RedirectToAction("ConnectRequestAdmin");

            int ID = Int32.Parse(Id);
            var obj = db.YoutubeConnect.Where(a => a.Id == ID).FirstOrDefault();
            if (obj != null)
            {
                sendEmail(obj.Email, "Hi "+ obj.FirstName + " " + obj.LastName + ",\n\n" +
                    "You have been accepted to join Tempo Digital MCN. There are still some steps required to complete your application. Please follow the link below to continue. Please follow the link below to continue your application:\n\n" +
                    "https://app.tempodigital.org/Account/Register" + "\n\n" +
                    "Thanks,\n" +
                    "\nSender,\nTempo Digital Team\n" +
                    "support@tempodigital.org" , "Thank you for your application") ;
                obj.Status = "Approved";
                db.SaveChanges();
            }
            return RedirectToAction("ConnectRequestAdmin");
        }

        public ActionResult RejectConnect(string Id)
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            if (Id == null)
                return RedirectToAction("ConnectRequestAdmin");

            int ID = Int32.Parse(Id);
            var obj = db.YoutubeConnect.Where(a => a.Id == ID).FirstOrDefault();
            if(obj != null)
            {
                sendEmail(obj.Email, "Thank you for applying. At this time we cannot partner with you, but we will keep your application on file and contact you if circumstances change.\n\nSender,\nTempo Digital Team.", "Your Application Was Not Accepted");
                obj.Status = "Rejected";
                db.SaveChanges();
            }
            
            return RedirectToAction("ConnectRequestAdmin");
        }

        public void sendEmail(string receiver, string body, string subject)
        {
            var senderMail = new MailAddress("support@tempodigital.org", "Tempo Digital Team");
            var receiverMial = new MailAddress(receiver, "Receiver");
            //var receiverMial = new MailAddress(email, "Receiver");

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderMail.Address, ConfigurationManager.AppSettings["GmailPasword"])
            };

            MailMessage message = new MailMessage(senderMail, receiverMial);
            message.Subject = subject;
            message.Body = body;

            smtp.Timeout = 2000000;
            smtp.Send(message);
        }

        public JsonResult CheckValidUser(string email)
        {
            var user = db.YoutubeConnect.Where(a => a.Email.Equals(email)).FirstOrDefault();
            if(user != null && user.Status == "Approved")
            {
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        public ActionResult Save(HttpPostedFileBase file)
        {
            string storageConnection = CloudConfigurationManager.GetSetting("BlobStorageConnectionString");
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

            //create a block blob 
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            //create a container 
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("musicdistribution");

            //create a container if it is not already exists

            cloudBlobContainer.CreateIfNotExists();
            cloudBlobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            //get Blob reference

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(file.FileName);
            cloudBlockBlob.Properties.ContentType = file.ContentType;

            //await cloudBlockBlob.UploadFromStreamAsync(file.InputStream);
            cloudBlockBlob.UploadFromStream(file.InputStream);

            //string host = "tempo-production.upload.ci-support.com";
            //string username = "tempo-production@upload.ci-support.com";
            //string password = "chohthae";

            //string destination = @"Distribution/";
            //int port = 22;  //Port 22 is defaulted for SFTP upload

            //using (BinaryReader b = new BinaryReader(file.InputStream))
            //{
            //    byte[] binData = b.ReadBytes(file.ContentLength);
            //    //result = System.Text.Encoding.UTF8.GetString(binData);
            //    var client = new SftpClient(host, port, username, password);
            //    client.Connect();
            //    client.ChangeDirectory(destination);
            //    var stream = new MemoryStream();
            //    stream.Write(binData, 0, binData.Length);
            //    stream.Position = 0;
            //    client.UploadFile(stream, Path.GetFileName(file.FileName));
            //}

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if(email.Equals(ConfigurationManager.AppSettings["AdminMail"]) && password.Equals(ConfigurationManager.AppSettings["AdminPasword"]))
            {
                Session["admin_session"] = ConfigurationManager.AppSettings["AdminMail"];
                return RedirectToAction("Index");
            }
            return View();
        }


        public ActionResult DisableUser(string id)
        {
            var user = db.Users.Where(a => a.Id.Equals(id)).FirstOrDefault();
            if(user != null)
            {
                user.isActive = false;
            }
            db.SaveChanges();
            return RedirectToAction("UserManagementAdmin", "Admin");
        }

        public ActionResult ActivateUser(string id)
        {
            var user = db.Users.Where(a => a.Id.Equals(id)).FirstOrDefault();
            if (user != null)
            {
                user.isActive = true;
            }
            db.SaveChanges();

            var senderMail = new MailAddress("distribution@tempodigital.org", "Tempo Digital Team");
            // send mail to distribution
            var receiverMial = new MailAddress(user.Email, "Receiver");
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(senderMail.Address, ConfigurationManager.AppSettings["GmailPasword"])
            };
            var message = new MailMessage(senderMail, receiverMial);
            message.Subject = "Account is Activated Successfully.";
            message.IsBodyHtml = true;
            message.Body = "" +
                @"<body><center><img src='https://tempodigital.org/wp-content/uploads/2020/11/Tempo-Logo-Color.png' style='width: 300px;'>
                </ center><center><h2>Thank you for the Registration</h2><p>After the review proces we are happy to approve your application for our Music Distribution Platform. Now you can login and use all tools. </p>
                <a href='https://app.tempodigital.org'><span> Go to Tempo Website</span></a>
                <br><br><label>Thank you.<br><br>Sender,<br>Tempo Digital Team.</label>
                </center>
                </body>";
            smtp.Send(message);

            return RedirectToAction("UserManagementAdmin", "Admin");
        }


        public ActionResult PaymentPaid(int id)
        {
            var user = db.MonthlyPayments.Where(a => a.ID.Equals(id)).FirstOrDefault();
            if (user != null)
            {
                user.Is_Paid = true;
            }
            db.SaveChanges();

            return RedirectToAction("MonthlyPaymentAdmin", "Admin");
        }

        public ActionResult PaymentNotPaid(int id)
        {
            var user = db.MonthlyPayments.Where(a => a.ID.Equals(id)).FirstOrDefault();
            if (user != null)
            {
                user.Is_Paid = false;
            }
            db.SaveChanges();

            return RedirectToAction("MonthlyPaymentAdmin", "Admin");
        }

        public ActionResult ApproveChannel(int id)
        {
            var channel = db.YoutubeChannels.Where(a => a.ID == id).FirstOrDefault();
            channel.Approved = true;
            DateTime dateTime = DateTime.UtcNow.Date;
            channel.ApprovelDate = dateTime.ToString("dd/MM/yyyy");
            db.Entry(channel).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ApproveEntChannel(int id)
        {
            var channel = db.GetYoutubeChannelsENT.Where(a => a.ID == id).FirstOrDefault();
            channel.Approved = true;
            DateTime dateTime = DateTime.UtcNow.Date;
            channel.ApprovelDate = dateTime.ToString("dd/MM/yyyy");
            db.Entry(channel).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteRelease(int id)
        {
            var release = db.MusicReleases.Where(a => a.MusicReleaseId == id).FirstOrDefault();
            var List = db.Songs.Where(a => a.MusicReleaseId == release.MusicReleaseId).ToList();
            foreach(var x in List)
            {
                db.Songs.Remove(x);
            }
            db.MusicReleases.Remove(release);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            Session["admin_session"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult HTMLReportAllCH(string M, string Y)
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            if (M == null || Y == null)
                return RedirectToAction("SumarizedReportsAdmin", "Admin");

            List<List<YT_Reports_Music_2020>> finalList = new List<List<YT_Reports_Music_2020>>();

            var users = db.Users.ToList();
            foreach(var x in users)
            {
                var userChannelList = db.YoutubeChannels.Where(a => a.UserId.Equals(x.Id) && a.Approved==true).ToList();
                var userChannelFinalList = new List<YT_Reports_Music_2020> ();
                foreach (var y in userChannelList)
                {
                    var list = new List<YT_Reports_Music_2020> ();
                    if(y.ChannelID != null)
                    {
                        y.ChannelID = y.ChannelID.Trim();
                        list = db.YT_Reports_Music_2020.Where(a => a.Asset_Channel_ID == y.ChannelID && a.Month == M && a.Year == Y).ToList();
                    }
                    else if(y.ChannelID == null && y.YT_Label_Name != null)
                    {
                        y.YT_Label_Name = y.YT_Label_Name.Trim();
                        list = db.YT_Reports_Music_2020.Where(a => a.Asset_Labels == y.YT_Label_Name && a.Month == M && a.Year == Y).ToList();
                    }
                    if (list.Count > 0)
                    {
                        var temp = new YT_Reports_Music_2020();
                        temp.Asset_Channel_ID = list[0].Asset_Channel_ID;
                        temp.Asset_Title = y.ChannelName;
                        temp.Owned_Views = list.Sum(i => i.Owned_Views);
                        temp.Partner_Revenue = list.Sum(i => i.Partner_Revenue);
                        temp.Custom_ID = x.FirstName + " " + x.LastName;
                        if (x.share == null || x.share == "NULL" || x.share == "")
                            temp.userShare = 0;
                        else
                            temp.userShare = Math.Round((temp.Partner_Revenue / 100) * Int32.Parse(x.share), 2);
                        userChannelFinalList.Add(temp);
                    }   
                }

                if (userChannelFinalList.Count > 0)
                    finalList.Add(userChannelFinalList);
            }

            List<List<YT_Reports_Ent_2020>> finalListEnt = new List<List<YT_Reports_Ent_2020>>();

            foreach (var x in users)
            {
                var userChannelListEnt = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(x.Id) && a.Approved == true).ToList();
                var userChannelFinalListEnt = new List<YT_Reports_Ent_2020>();
                foreach (var y in userChannelListEnt)
                {
                    var list = new List<YT_Reports_Ent_2020>();
                    if (y.ChannelID != null)
                    {
                        y.ChannelID = y.ChannelID.Trim();
                        list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Channel_ID == y.ChannelID && a.Month == M && a.Year == Y).ToList();
                    }
                    else if (y.ChannelID == null && y.YT_Label_Name != null)
                    {
                        y.YT_Label_Name = y.YT_Label_Name.Trim();
                        list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Labels == y.YT_Label_Name && a.Month == M && a.Year == Y).ToList();
                    }
                    
                    if (list.Count > 0)
                    {
                        var temp = new YT_Reports_Ent_2020();
                        temp.Asset_Channel_ID = list[0].Asset_Channel_ID;
                        temp.Asset_Title = y.ChannelName;
                        temp.Owned_Views = list.Sum(i => i.Owned_Views);
                        temp.Partner_Revenue = list.Sum(i => i.Partner_Revenue);
                        temp.Custom_ID = x.FirstName + " " + x.LastName;
                        if (x.share == null || x.share == "NULL" || x.share == "")
                            temp.userShare = 0;
                        else
                            temp.userShare = Math.Round((temp.Partner_Revenue / 100) * Int32.Parse(x.share), 2);
                        userChannelFinalListEnt.Add(temp);
                    }
                }

                if (userChannelFinalListEnt.Count > 0)
                    finalListEnt.Add(userChannelFinalListEnt);
                else
                    ViewBag.CheckListENT = 1;
            }

            ViewBag.finalListEnt = finalListEnt;

            return View(finalList);
        }

        public ActionResult PDFReportAllCH(string M, string Y)
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            if (M == null || Y == null)
            {
                return RedirectToAction("SumarizedReportsAdmin", "Admin");
            }
                //return File("NotFound.txt", "text/plain");

            /*Creating iTextSharp’s Document & Writer*/
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5, 5, 5, 5);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            /*End*/

            //Chunk chunk = new Chunk("Report of All Channels", FontFactory.GetFont("Arial", 20, Font.BOLDITALIC, BaseColor.MAGENTA));
            //pdfDoc.Add(chunk);
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);

            

            var users = db.Users.ToList();
            foreach (var x in users)
            {
                var userChannelList = db.YoutubeChannels.Where(a => a.UserId.Equals(x.Id) && a.Approved==true).ToList();
                var userChannelFinalList = new List<YT_Reports_Music_2020>();
                foreach (var y in userChannelList)
                {
                    //y.ChannelID = y.ChannelID.Trim();
                    //var list = db.YT_Reports_Music_Full.Where(a => a.Asset_Channel_ID == y.ChannelID && a.Month == M && a.Year == Y).ToList();
                    var list = new List<YT_Reports_Music_2020> ();
                    if(y.ChannelID != null)
                    {
                        y.ChannelID = y.ChannelID.Trim();
                        list = db.YT_Reports_Music_2020.Where(a => a.Asset_Channel_ID == y.ChannelID && a.Month == M && a.Year == Y).ToList();
                    }
                    else if(y.ChannelID == null && y.YT_Label_Name != null)
                    {
                        y.YT_Label_Name = y.YT_Label_Name.Trim();
                        list = db.YT_Reports_Music_2020.Where(a => a.Asset_Labels == y.YT_Label_Name && a.Month == M && a.Year == Y).ToList();
                    }
                    if (list.Count > 0)
                    {
                        var temp = new YT_Reports_Music_2020();
                        temp.Asset_Channel_ID = list[0].Asset_Channel_ID;
                        temp.Asset_Title = y.ChannelName;
                        temp.Owned_Views = list.Sum(i => i.Owned_Views);
                        temp.Partner_Revenue = list.Sum(i => i.Partner_Revenue);
                        temp.Custom_ID = x.FirstName + " " + x.LastName;
                        if (x.share == null || x.share == "NULL" || x.share == "")
                            temp.userShare = 0;
                        else
                            temp.userShare = Math.Round((temp.Partner_Revenue / 100) * Int32.Parse(x.share), 2);
                        userChannelFinalList.Add(temp);
                    }
                }


                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;
                PdfPCell myCell = new PdfPCell(new Phrase("Arial", new Font(Font.NORMAL, 4)));


                if (userChannelFinalList.Count > 0)
                {
                    Chunk chunk = new Chunk(x.FirstName + " " + x.LastName);
                    chunk.Font = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK);

                    pdfDoc.Add(chunk);

                    chunk = new Chunk( "            Share: $ " + userChannelFinalList.Sum(a => a.userShare).ToString() );
                    pdfDoc.Add(chunk);

                    table.AddCell("Channel Name");
                    table.AddCell("Channel ID");
                    table.AddCell("Channel Label");
                    table.AddCell("Owned Views");
                    table.AddCell("Revenue");
                    table.AddCell("Share");

                    foreach (var channel in userChannelFinalList)
                    {
                        table.AddCell(new Phrase(0, channel.Asset_Title, FontFactory.GetFont(FontFactory.COURIER, 11)));
                        table.AddCell(new Phrase(0, channel.Asset_Channel_ID, FontFactory.GetFont(FontFactory.COURIER, 11)));
                        table.AddCell(new Phrase(0, channel.Asset_Labels, FontFactory.GetFont(FontFactory.COURIER, 11)));
                        table.AddCell(new Phrase(0, channel.Owned_Views.ToString(), FontFactory.GetFont(FontFactory.COURIER, 11)));
                        table.AddCell(new Phrase(0, channel.Partner_Revenue.ToString(), FontFactory.GetFont(FontFactory.COURIER, 11)));
                        table.AddCell(new Phrase(0, channel.userShare.ToString(), FontFactory.GetFont(FontFactory.COURIER, 11)));
                    }

                    
                    table.AddCell("Total");
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell(userChannelFinalList.Sum(a => a.Owned_Views).ToString());
                    table.AddCell("$" + userChannelFinalList.Sum(a => a.Partner_Revenue).ToString());
                    table.AddCell("$" + userChannelFinalList.Sum(a => a.userShare).ToString());

                    pdfDoc.Add(table);

                    Paragraph para = new Paragraph();
                    para.Add("");
                    para.SpacingBefore = 20f;
                    para.SpacingAfter = 20f;
                    pdfDoc.Add(para);
                }
            }

            Chunk chunk1 = new Chunk("Youtube Entertainment Channel Report Data");
            chunk1.Font = FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK);
            pdfDoc.Add(chunk1);
            chunk1 = new Chunk("  \n");
            pdfDoc.Add(chunk1);

            foreach (var x in users)
            {
                var userChannelListEnt = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(x.Id) && a.Approved == true).ToList();
                var userChannelFinalListEnt = new List<YT_Reports_Ent_2020>();
                foreach (var y in userChannelListEnt)
                {
                    var list = new List<YT_Reports_Ent_2020>();
                    if (y.ChannelID != null)
                    {
                        y.ChannelID = y.ChannelID.Trim();
                        list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Channel_ID == y.ChannelID && a.Month == M && a.Year == Y).ToList();
                    }
                    else if (y.ChannelID == null && y.YT_Label_Name != null)
                    {
                        y.YT_Label_Name = y.YT_Label_Name.Trim();
                        list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Labels == y.YT_Label_Name && a.Month == M && a.Year == Y).ToList();
                    }
                    //y.ChannelID = y.ChannelID.Trim();
                    //var list = db.yT_Reports_Ents.Where(a => a.Asset_Channel_ID == y.ChannelID && a.Month == M && a.Year == Y).ToList();
                    if (list.Count > 0)
                    {
                        var temp = new YT_Reports_Ent_2020();
                        temp.Asset_Channel_ID = list[0].Asset_Channel_ID;
                        temp.Asset_Title = y.ChannelName;
                        temp.Owned_Views = list.Sum(i => i.Owned_Views);
                        temp.Partner_Revenue = list.Sum(i => i.Partner_Revenue);
                        temp.Custom_ID = x.FirstName + " " + x.LastName;
                        if (x.share == null || x.share == "NULL" || x.share == "")
                            temp.userShare = 0;
                        else
                            temp.userShare = Math.Round((temp.Partner_Revenue / 100) * Int32.Parse(x.share), 2);
                        userChannelFinalListEnt.Add(temp);
                    }
                }
                

                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;
                PdfPCell myCell = new PdfPCell(new Phrase("Arial", new Font(Font.NORMAL, 4)));

                if (userChannelFinalListEnt.Count > 0)
                {
                    Chunk chunk = new Chunk(x.FirstName + " " + x.LastName);
                    chunk.Font = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK);

                    pdfDoc.Add(chunk);

                    chunk = new Chunk("            Share: $ " + userChannelFinalListEnt.Sum(a => a.userShare).ToString());
                    pdfDoc.Add(chunk);

                    table.AddCell("Channel Name");
                    table.AddCell("Channel ID");
                    table.AddCell("Channel Label");
                    table.AddCell("Owned Views");
                    table.AddCell("Revenue");
                    table.AddCell("Share");

                    foreach (var channel in userChannelFinalListEnt)
                    {
                        table.AddCell(channel.Asset_Title);
                        table.AddCell(channel.Asset_Channel_ID);
                        table.AddCell(channel.Asset_Labels);
                        table.AddCell(channel.Owned_Views.ToString());
                        table.AddCell(channel.Partner_Revenue.ToString());
                        table.AddCell(channel.userShare.ToString());
                    }


                    table.AddCell("Total");
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell(userChannelFinalListEnt.Sum(a => a.Owned_Views).ToString());
                    table.AddCell("$" + userChannelFinalListEnt.Sum(a => a.Partner_Revenue).ToString());
                    table.AddCell("$" + userChannelFinalListEnt.Sum(a => a.userShare).ToString());

                    pdfDoc.Add(table);

                    Paragraph para = new Paragraph();
                    para.Add("");
                    para.SpacingBefore = 20f;
                    para.SpacingAfter = 20f;
                    pdfDoc.Add(para);
                }
            }


            /*Creating the PDF file*/
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=All-Channels-Report.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
            /*End*/

            return RedirectToAction("Index");
        }

        public JsonResult GetAllMY()
        {
            List<string> month = new List<string>();
            List<string> year = new List<string>();

            var allData = db.YT_Reports_Music_Full
                .Join(
                db.YoutubeChannels,
                d => d.Asset_Channel_ID,
                f => f.ChannelID,
                (d, f) => d).ToList();

            foreach (var x in allData)
            {
                if (!month.Contains(x.Month))
                    month.Add(x.Month);
                if (!year.Contains(x.Year))
                    year.Add(x.Year);
            }

            MY obj = new MY();
            obj.month = month;
            obj.year = year;


            return this.Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MonthlyPayments()
        {
            if (Session["admin_session"] == null)
                return RedirectToAction("Login");

            ViewBag.UserEmails = db.Users.ToList();

            var years = new List<String>();
            int year = DateTime.Now.Year;
            while (year > 2000)
            {
                years.Add(year.ToString());
                year = year - 1;
            }
            ViewBag.Years = years;
            var list = db.MonthlyPayments.ToList();
            ViewBag.sortedList = list.OrderByDescending(a => a.ID);
            return View(list);
        }

        [HttpPost]
        public ActionResult MonthlyPayments(string email, string yearMusic, string monthMusic, string yearEnt, string monthEnt, string platform)
        {
            if (platform.Equals("Youtube"))
            {
                Youtube_Ent_Monthly_Report_Self_Billing(email, yearMusic, monthMusic, yearEnt, monthEnt);
                TempData["Success"] = "Youtube/Entertainment Report + Self Billing Sent Successfully.";
            }
            else if (platform.Equals("Distributions"))
            {
                Distributions_Monthly_Report_Self_Billing(email, yearMusic, monthMusic);
                TempData["Success"] = "Distributions Report + Self Billing Sent Successfully.";
            }
            //else if (platform.Equals("Spotify"))
            //{
            //    Spotify_Monthly_Report_Self_Billing(email, yearMusic, monthMusic);
            //    TempData["Success"] = "Spotify Report + Self Billing Sent Successfully.";
            //}
            //else if (platform.Equals("Deezers"))
            //{
            //    Deezers_Ent_Monthly_Report_Self_Billing(email, yearMusic, monthMusic);
            //    TempData["Success"] = "Deezers Report + Self Billing Sent Successfully.";
            //}
            //else if (platform.Equals("GooglePlay")) 
            //{
            //    Google_Play_Ent_Monthly_Report_Self_Billing(email, yearMusic, monthMusic);
            //    TempData["Success"] = "Google Play Report + Self Billing Sent Successfully.";
            //}
            //else if (platform.Equals("SoundCloud")) 
            //{
            //    Sound_Cloud_Ent_Monthly_Report_Self_Billing(email, yearMusic, monthMusic);
            //    TempData["Success"] = "Sound Cloud Report + Self Billing Sent Successfully.";
            //}

            return RedirectToAction("MonthlyPaymentAdmin");
        }

        void Youtube_Ent_Monthly_Report_Self_Billing(string email, string yearMusic, string monthMusic, string yearEnt, string monthEnt)
        {
            var user = db.Users.Where(a => a.Email.Equals(email)).FirstOrDefault();

            int share = Int32.Parse(user.share);
            double musicTotal = 0.00, entTotal = 0.00;

            /*Creating iTextSharp’s Document & Writer*/
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
            pdfDoc.Open();


            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            float[] columnWidths = new float[] { 200f, 240f, 110f, 100f, 100f, 100f };
            table.SetWidthPercentage(columnWidths, PageSize.A4.Rotate());

            table.AddCell("Channel ID");
            table.AddCell("Asset Title");
            table.AddCell("Asset Type");
            table.AddCell("Owned Views");
            table.AddCell("Revenue");
            table.AddCell("Share");

            var pdfImage = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
            pdfImage.Alignment = Element.ALIGN_MIDDLE;


            pdfDoc.Add(pdfImage);
            var para12 = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Kaveldunsvägen 21, 80636\n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
            para12.Alignment = Element.ALIGN_MIDDLE;
            pdfDoc.Add(para12);

            Chunk chunk4 = new Chunk("Official Payment Report - Music Network:" + get_Month(monthMusic) + "-" + yearMusic + " |   Entertainment Network:" + get_Month(monthEnt) + "-" + yearEnt);
            chunk4.Font = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.BLACK);
            pdfDoc.Add(chunk4);

            int view = 0;
            double rev = 0;
            var channels = db.YoutubeChannels.Where(a => a.UserId.Equals(user.Id) && a.Approved == true).ToList();
            foreach (var x in channels)
            {
                var list = new List<YT_Reports_Music_2020>();
                if (x.ChannelName == null && x.YT_Label_Name != null)
                {
                    list = db.YT_Reports_Music_2020.Where(a => a.Asset_Labels.Equals(x.YT_Label_Name) && a.Month == monthMusic && a.Year == yearMusic && a.Asset_Type.Equals("Sound Recording")).ToList();
                }
                else
                {
                    x.ChannelID = x.ChannelID.Trim();
                    list = db.YT_Reports_Music_2020.Where(a => a.Asset_Channel_ID == x.ChannelID && a.Month == monthMusic && a.Year == yearMusic).ToList();
                }


                if (list.Count > 0)
                {
                    table.AddCell(new Phrase(0, list[0].Asset_Channel_ID, FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                    table.AddCell(new Phrase(0, list[0].Asset_Labels, FontFactory.GetFont(FontFactory.COURIER, 11)));
                    table.AddCell(new Phrase(0, list[0].Asset_Type, FontFactory.GetFont(FontFactory.COURIER, 11)));
                    table.AddCell(new Phrase(0, list.Sum(a => a.Owned_Views).ToString(), FontFactory.GetFont(FontFactory.COURIER, 11)));
                    table.AddCell(new Phrase(0, "$" + Math.Round(list.Sum(a => a.Partner_Revenue),2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 11)));
                    double asshare1 = (double)Math.Round(((list.Sum(a => a.Partner_Revenue) / 100) * share), 2);
                    table.AddCell(new Phrase(0, "$" + asshare1, FontFactory.GetFont(FontFactory.COURIER, 11)));

                    view = view + list.Sum(a => a.Owned_Views);
                    rev = rev + (double)list.Sum(a => a.Partner_Revenue);
                    musicTotal = musicTotal + (double)list.Sum(a => a.Partner_Revenue);
                }
            }

            var channelsEnt = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(user.Id) && a.Approved == true).ToList();
            foreach (var x in channelsEnt)
            {
                var list = new List<YT_Reports_Ent_2020>();
                if (x.ChannelName == null && x.YT_Label_Name != null)
                {
                    list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Labels.Equals(x.YT_Label_Name) && a.Month == monthEnt && a.Year == yearEnt).ToList();
                }
                else
                {
                    x.ChannelID = x.ChannelID.Trim();
                    list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Channel_ID == x.ChannelID && a.Month == monthEnt && a.Year == yearEnt).ToList();
                }


                if (list.Count > 0)
                {
                    table.AddCell(new Phrase(0, list[0].Asset_Channel_ID, FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                    table.AddCell(new Phrase(0, list[0].Asset_Labels, FontFactory.GetFont(FontFactory.COURIER, 11)));
                    table.AddCell(new Phrase(0, list[0].Asset_Type, FontFactory.GetFont(FontFactory.COURIER, 11)));
                    table.AddCell(new Phrase(0, list.Sum(a => a.Owned_Views).ToString(), FontFactory.GetFont(FontFactory.COURIER, 11)));
                    table.AddCell(new Phrase(0, "$" + Math.Round(list.Sum(a => a.Partner_Revenue),2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 11)));
                    double asshare1 = (double)Math.Round(((list.Sum(a => a.Partner_Revenue) / 100) * share), 2);
                    table.AddCell(new Phrase(0, "$" + asshare1, FontFactory.GetFont(FontFactory.COURIER, 11)));

                    view = view + list.Sum(a => a.Owned_Views);
                    rev = rev + (double)list.Sum(a => a.Partner_Revenue);
                    entTotal = entTotal + (double)list.Sum(a => a.Partner_Revenue);
                }
            }

            table.AddCell("Total");
            table.AddCell("");
            table.AddCell("");
            table.AddCell(view.ToString());
            table.AddCell("$" + Math.Round(rev,2).ToString());
            double asshare3 = (double)Math.Round(((rev / 100) * share), 2);
            table.AddCell("$" + asshare3);
            pdfDoc.Add(table);

            Paragraph line1 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line1);

            foreach (var x in channels)
            {
                var list = new List<YT_Reports_Music_2020>();
                if (x.ChannelName == null && x.YT_Label_Name != null)
                {
                    list = db.YT_Reports_Music_2020.Where(a => a.Asset_Labels.Equals(x.YT_Label_Name) && a.Month == monthMusic && a.Year == yearMusic && a.Asset_Type.Equals("Sound Recording")).ToList();
                }
                else
                {
                    list = db.YT_Reports_Music_2020.Where(a => a.Asset_Channel_ID == x.ChannelID && a.Month == monthMusic && a.Year == yearMusic).ToList();
                }

                PdfPTable table1 = new PdfPTable(6);
                table1.WidthPercentage = 100;
                table1.HorizontalAlignment = 0;
                table1.SpacingBefore = 20f;
                table1.SpacingAfter = 30f;

                table1.SetWidthPercentage(columnWidths, PageSize.A4.Rotate());

                if (list.Count > 0)
                {
                    Chunk chunk1 = new Chunk(x.ChannelName);
                    chunk1.Font = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);

                    pdfDoc.Add(chunk1);

                    table1.AddCell("Channel ID");
                    table1.AddCell("Asset Title");
                    table1.AddCell("Asset Type");
                    table1.AddCell("Owned Views");
                    table1.AddCell("Revenue");
                    table1.AddCell("Share");

                    foreach (var channel in list)
                    {
                        table1.AddCell(new Phrase(0, channel.Asset_Channel_ID, FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                        table1.AddCell(new Phrase(0, channel.Asset_Title, FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                        table1.AddCell(new Phrase(0, channel.Asset_Type, FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                        table1.AddCell(new Phrase(0, channel.Owned_Views.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                        table1.AddCell(new Phrase(0, "$" + Math.Round(channel.Partner_Revenue,2).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                        double asshare = (double)Math.Round(((channel.Partner_Revenue / 100) * share), 2);
                        table1.AddCell(new Phrase(0, "$" + asshare.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                    }

                    table1.AddCell("Total");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell(list.Sum(a => a.Owned_Views).ToString());
                    table1.AddCell("$" + Math.Round(list.Sum(a => a.Partner_Revenue),2).ToString());
                    double asshare1 = (double)Math.Round(((list.Sum(a => a.Partner_Revenue / 100)) * share), 2);
                    table1.AddCell("$" + asshare1);


                    pdfDoc.Add(table1);

                    Paragraph para = new Paragraph();
                    para.Add("");
                    para.SpacingBefore = 20f;
                    para.SpacingAfter = 20f;
                    pdfDoc.Add(para);
                }

            }

            foreach (var x in channelsEnt)
            {
                var list = new List<YT_Reports_Ent_2020>();
                if (x.ChannelName == null && x.YT_Label_Name != null)
                {
                    list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Labels.Equals(x.YT_Label_Name) && a.Month == monthEnt && a.Year == yearEnt).ToList();
                }
                else
                {
                    list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Channel_ID == x.ChannelID && a.Month == monthEnt && a.Year == yearEnt).ToList();
                }

                PdfPTable table1 = new PdfPTable(6);
                table1.WidthPercentage = 100;
                table1.HorizontalAlignment = 0;
                table1.SpacingBefore = 20f;
                table1.SpacingAfter = 30f;

                table1.SetWidthPercentage(columnWidths, PageSize.A4.Rotate());

                if (list.Count > 0)
                {
                    Chunk chunk1 = new Chunk(x.ChannelName);
                    chunk1.Font = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);

                    pdfDoc.Add(chunk1);

                    table1.AddCell("Channel ID");
                    table1.AddCell("Asset Title");
                    table1.AddCell("Asset Type");
                    table1.AddCell("Owned Views");
                    table1.AddCell("Revenue");
                    table1.AddCell("Share");

                    foreach (var channel in list)
                    {
                        table1.AddCell(new Phrase(0, channel.Asset_Channel_ID, FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                        table1.AddCell(new Phrase(0, channel.Asset_Title, FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                        table1.AddCell(new Phrase(0, channel.Asset_Type, FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                        table1.AddCell(new Phrase(0, channel.Owned_Views.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                        table1.AddCell(new Phrase(0, "$" + Math.Round(channel.Partner_Revenue,2).ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                        double asshare = (double)Math.Round(((channel.Partner_Revenue / 100) * share), 2);
                        table1.AddCell(new Phrase(0, "$" + asshare.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                    }

                    table1.AddCell("Total");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell(list.Sum(a => a.Owned_Views).ToString());
                    table1.AddCell("$" + Math.Round(list.Sum(a => a.Partner_Revenue),2).ToString());
                    double asshare1 = (double)Math.Round(((list.Sum(a => a.Partner_Revenue / 100)) * share), 2);
                    table1.AddCell("$" + asshare1);


                    pdfDoc.Add(table1);

                    Paragraph para = new Paragraph();
                    para.Add("");
                    para.SpacingBefore = 20f;
                    para.SpacingAfter = 20f;
                    pdfDoc.Add(para);
                }

            }

            pdfWriter.CloseStream = false;
            pdfDoc.Close();

            musicTotal = (double)Math.Round(((musicTotal / 100) * share), 2);
            entTotal = (double)Math.Round(((entTotal / 100) * share), 2);

            var payment = new MonthlyPayments();
            payment.User = user.Email;
            payment.MusicAmount = musicTotal.ToString();
            payment.EntAmount = entTotal.ToString();
            payment.Date_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            payment.Music_Month = monthMusic;
            payment.Music_Year = yearMusic;
            payment.Ent_Month = monthEnt;
            payment.Ent_Year = yearEnt;

            db.MonthlyPayments.Add(payment);
            db.SaveChanges();
            var count = db.MonthlyPayments.Count();

            /*Creating iTextSharp’s Document & Writer*/
            Document pdfDocSELF = new Document(PageSize.A4, 20, 10, 10, 20);
            MemoryStream memoryStreamSELF = new MemoryStream();
            PdfWriter pdfWriterSELF = PdfWriter.GetInstance(pdfDocSELF, memoryStreamSELF);
            pdfDocSELF.Open();
            /*End*/

            PdfPTable tableSELF = new PdfPTable(2);
            tableSELF.DefaultCell.Border = Rectangle.NO_BORDER;
            tableSELF.WidthPercentage = 100;
            tableSELF.HorizontalAlignment = 0;
            tableSELF.SpacingBefore = 20f;
            tableSELF.SpacingAfter = 30f;
            var para12SELF = new Paragraph("\n\n\n\n TP Digital AB Sweden \n Kaveldunsvägen 21, 80636 Gävle  \n VAT Number: SE559134761101.", FontFactory.GetFont("Georgia", 15, Font.NORMAL, BaseColor.BLACK));
            para12SELF.Alignment = Element.ALIGN_LEFT;
            tableSELF.AddCell(para12SELF);
            var pdfImageSELF = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
            pdfImageSELF.ScaleToFit(20, 20);
            pdfImageSELF.Alignment = Element.ALIGN_MIDDLE;
            tableSELF.AddCell(pdfImageSELF);
            pdfDocSELF.Add(tableSELF);


            PdfPTable table1SELF = new PdfPTable(2);
            table1SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            table1SELF.WidthPercentage = 100;
            var para1SELF = new Paragraph(DateTime.UtcNow.Date.ToString("d"), FontFactory.GetFont("Courier", 16, Font.BOLD, BaseColor.BLACK));
            para1SELF.Alignment = Element.ALIGN_LEFT;
            table1SELF.AddCell(para1SELF);
            var para2SELF = new Paragraph("  SELF-BILLING INVOICE NUMBER", FontFactory.GetFont("Courier", 16, Font.BOLD, BaseColor.BLACK));
            para1SELF.Alignment = Element.ALIGN_RIGHT;
            table1SELF.AddCell(para2SELF);
            pdfDocSELF.Add(table1SELF);

            var paraSELF = new Paragraph("INVOICE FORM: ", FontFactory.GetFont("Courier", 16, Font.BOLD, BaseColor.BLACK));
            paraSELF.Alignment = Element.ALIGN_LEFT;
            pdfDocSELF.Add(paraSELF);
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDocSELF.Add(line);
            var para5SELF = new Paragraph("TP-Digital -	000" + count.ToString(), FontFactory.GetFont("Calibri", 16, Font.BOLD, BaseColor.BLACK));
            para5SELF.Alignment = Element.ALIGN_RIGHT;
            pdfDocSELF.Add(para5SELF);

            var table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            var columnWidths1SELF = new float[] { 70f, 400f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;

            var para6SELF = new Paragraph("Name: ", FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;

            var para7SELF = new Paragraph(user.FirstName + " " + user.LastName, FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;

            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);


            table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            columnWidths1SELF = new float[] { 70f, 400f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;
            para6SELF = new Paragraph("Address: ", FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;
            para7SELF = new Paragraph(user.Email, FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;
            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);


            table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            columnWidths1SELF = new float[] { 100f, 370f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;
            para6SELF = new Paragraph("VAT Number: ", FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;
            para7SELF = new Paragraph("Not Applicable\n\n", FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;
            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);


            table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            columnWidths1SELF = new float[] { 400f, 70f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;
            para6SELF = new Paragraph("\n\n\nDESCRIPTIONS", FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;
            para7SELF = new Paragraph("\n\n\nAMOUNT", FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;
            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);

            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDocSELF.Add(line);

            table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            columnWidths1SELF = new float[] { 400f, 70f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;
            para6SELF = new Paragraph(monthMusic + "/" + yearMusic + "  Music Network Report", FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;
            para7SELF = new Paragraph("$" + musicTotal.ToString(), FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;
            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);

            table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            columnWidths1SELF = new float[] { 400f, 70f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;
            para6SELF = new Paragraph(monthEnt + "/" + yearEnt + "  Entertainment Network Report", FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;
            para7SELF = new Paragraph("$" + entTotal.ToString(), FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;
            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);


            para5SELF = new Paragraph("\n\nReverse charge in accordance with \nChapter 1. § 2 1st 2. The VAT Act\n\nPayment Terms:\nThis invoice has been paid to your nominated Bank account. \nPlease keep this self-bill invoice for your records.\nAmounts under $70 are not paid until the total has reached $70.", FontFactory.GetFont("Georgia", 12, Font.NORMAL, BaseColor.BLACK));
            para5SELF.Alignment = Element.ALIGN_LEFT;
            pdfDocSELF.Add(para5SELF);

            para5SELF = new Paragraph("\n\nSUB-TOTAL: $" + (musicTotal + entTotal).ToString() + "\nVAT: US $0.00\nTotal Amount: $" + (musicTotal + entTotal).ToString()+"\nBalance Due: $0.00", FontFactory.GetFont("Georgia", 12, Font.BOLD, BaseColor.BLACK));
            para5SELF.Alignment = Element.ALIGN_RIGHT;
            pdfDocSELF.Add(para5SELF);

            para5SELF = new Paragraph("Thank You for being our customer.", FontFactory.GetFont("Georgia", 12, Font.NORMAL, BaseColor.BLACK));
            para5SELF.Alignment = Element.ALIGN_RIGHT;
            pdfDocSELF.Add(para5SELF);

            /*Creating the PDF file*/
            pdfWriterSELF.CloseStream = false;
            pdfDocSELF.Close();




            var senderMail = new MailAddress("support@tempodigital.org", "Tempo Digital Team");
            var receiverMial = new MailAddress("munir@tempodigital.org", "Receiver");
            //var receiverMial = new MailAddress(email, "Receiver");

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderMail.Address, ConfigurationManager.AppSettings["GmailPasword"])                
            };

            MailMessage message = new MailMessage(senderMail, receiverMial);
            message.Subject = "Monthly Report - Self Billing";
            message.Body = "Hi,\nHere are the Reports for Month/Year" + monthMusic + "/" + yearMusic + " for Music Network & Month/Year" + monthEnt + " / " + yearEnt + " for Entertainment Network.\n Please find Monthly Report and Self Billing in Attached Files.\n\nSender,\nTempo Digital Team.";
            memoryStreamSELF.Position = 0;
            message.Attachments.Add(new Attachment(memoryStreamSELF, "Self-Billing.pdf"));
            memoryStream.Position = 0;
            message.Attachments.Add(new Attachment(memoryStream, "Monthly-Report.pdf"));
            smtp.Timeout = 2000000;
            smtp.Send(message);

        }

        void Distributions_Monthly_Report_Self_Billing(string email,string yearMusic,string monthMusic)
        {
            var user = db.Users.Where(a => a.Email.Equals(email)).FirstOrDefault();
            int share = Int32.Parse(user.share);

            double musicTotal = 0.0;

            //Spotify Report
            /*Creating iTextSharp’s Document & Writer*/
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
            pdfDoc.Open();


            PdfPTable table = new PdfPTable(8);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            float[] columnWidths = new float[] { 50f, 50f, 50f, 100f, 260f, 115f, 100f, 100f };
            table.SetWidthPercentage(columnWidths, PageSize.A4.Rotate());

            table.AddCell("Month");
            table.AddCell("Year");
            table.AddCell("Country");
            table.AddCell("UPC");
            table.AddCell("Artist Name");
            table.AddCell("Composer Name");
            table.AddCell("Revenue");
            table.AddCell("Share");

            var pdfImage = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
            pdfImage.Alignment = Element.ALIGN_MIDDLE;


            pdfDoc.Add(pdfImage);
            var para12 = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Kaveldunsvägen 21, 80636 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
            para12.Alignment = Element.ALIGN_MIDDLE;
            pdfDoc.Add(para12);

            Chunk chunk4 = new Chunk("Official Payment Report -Spotify Network:" + get_Month(monthMusic) + "-" + yearMusic);
            chunk4.Font = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.BLACK);
            pdfDoc.Add(chunk4);

            double rev = 0;
            var distributions = db.MusicReleases.Where(a => a.UserId.Equals(user.Id) && a.Status != "Deleted").ToList();
            foreach (var dist in distributions)
            {
                var list = db.SpotifyReports_2020.Where(a => a.UPC.Trim().Equals(dist.UPCEAN) && a.Month.Trim().Equals(monthMusic) && a.Year.Trim().Equals(yearMusic)).ToList();

                if (list.Count > 0)
                {
                    table.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    table.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, list[0].Country, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, list[0].UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, list[0].Artist_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, list[0].Composer_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, "$" + Math.Round(list.Sum(a => a.USD_Payable),2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                    double asshare1 = (double)Math.Round(((list.Sum(a => a.USD_Payable) / 100) * share), 2);
                    table.AddCell(new Phrase(0, "$" + asshare1, FontFactory.GetFont(FontFactory.COURIER, 10)));

                    rev = rev + (double)list.Sum(a => a.USD_Payable);
                    musicTotal = musicTotal + (double)list.Sum(a => a.USD_Payable);
                }
            }

            table.AddCell("Total");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("");
            table.AddCell("$" + Math.Round(rev,2).ToString());
            double asshare3 = (double)Math.Round(((rev / 100) * share), 2);
            table.AddCell("$" + asshare3);
            pdfDoc.Add(table);


            foreach (var x in distributions)
            {
                var list = db.SpotifyReports_2020.Where(a => a.UPC.Trim().Equals(x.UPCEAN) && a.Month.Trim().Equals(monthMusic) && a.Year.Trim().Equals(yearMusic)).ToList();

                PdfPTable table1 = new PdfPTable(8);
                table1.WidthPercentage = 100;
                table1.HorizontalAlignment = 0;
                table1.SpacingBefore = 20f;
                table1.SpacingAfter = 30f;
                float[] columnWidths1 = new float[] { 40f, 40f, 40f, 100f, 290f, 115f, 100f, 100f };
                table1.SetWidthPercentage(columnWidths1, PageSize.A4.Rotate());

                if (list.Count > 0)
                {
                    Chunk chunk1 = new Chunk("Relesse Name: " + x.ReleaseName);
                    chunk1.Font = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);

                    pdfDoc.Add(chunk1);

                    table1.AddCell("Month");
                    table1.AddCell("Year");
                    table1.AddCell("Country");
                    table1.AddCell("UPC");
                    table1.AddCell("Artist Name");
                    table1.AddCell("Composer Name");
                    table1.AddCell("Revenue");
                    table1.AddCell("Share");

                    foreach (var dist in list)
                    {
                        table1.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        table1.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.Country, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.Artist_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.Composer_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, "$" + Math.Round(dist.USD_Payable,2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                        double asshare2 = (double)Math.Round((((dist.USD_Payable) / 100) * share), 2);
                        table1.AddCell(new Phrase(0, "$" + asshare2, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    }

                    table1.AddCell("Total");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("$" + Math.Round(list.Sum(a => a.USD_Payable),2).ToString());
                    double asshare5 = (double)Math.Round(((list.Sum(a => a.USD_Payable / 100)) * share), 2);
                    table1.AddCell("$" + asshare5);


                    pdfDoc.Add(table1);

                    Paragraph para = new Paragraph();
                    para.Add("");
                    para.SpacingBefore = 20f;
                    para.SpacingAfter = 20f;
                    pdfDoc.Add(para);
                }

            }
            pdfWriter.CloseStream = false;
            pdfDoc.Close();

            var spotifyTotal = (double)Math.Round(((musicTotal / 100) * share), 2);
            musicTotal = 0.0;

            var payment = new MonthlyPayments();
            payment.User = user.Email;
            payment.MusicAmount = musicTotal.ToString();
            payment.EntAmount = "Spotify Report";
            payment.Date_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            payment.Music_Month = monthMusic;
            payment.Music_Year = yearMusic;

            db.MonthlyPayments.Add(payment);
            db.SaveChanges();


            // Deezer Report
            /*Creating iTextSharp’s Document & Writer*/
            Document pdfDocD = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            MemoryStream memoryStreamD = new MemoryStream();
            PdfWriter pdfWriterD = PdfWriter.GetInstance(pdfDocD, memoryStreamD);
            pdfDocD.Open();


            PdfPTable tableD = new PdfPTable(8);
            tableD.WidthPercentage = 100;
            tableD.HorizontalAlignment = 0;
            tableD.SpacingBefore = 20f;
            tableD.SpacingAfter = 30f;

            float[] columnWidthsD = new float[] { 50f, 50f, 100f, 100f, 250f, 110f, 90f, 80f };
            tableD.SetWidthPercentage(columnWidthsD, PageSize.A4.Rotate());

            tableD.AddCell("Month");
            tableD.AddCell("Year");
            tableD.AddCell("Label");
            tableD.AddCell("UPC");
            tableD.AddCell("Artist Name");
            tableD.AddCell("Album Name");
            tableD.AddCell("Revenue");
            tableD.AddCell("Share");

            var pdfImageD = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
            pdfImageD.Alignment = Element.ALIGN_MIDDLE;


            pdfDocD.Add(pdfImageD);
            var para12D = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Kaveldunsvägen 21, 80636 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
            para12D.Alignment = Element.ALIGN_MIDDLE;
            pdfDocD.Add(para12D);

            Chunk chunk4D = new Chunk("Official Payment Report - Deezer Network:" + get_Month(monthMusic) + "-" + yearMusic);
            chunk4D.Font = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.BLACK);
            pdfDocD.Add(chunk4D);

            double revD = 0;
            //var distributionsD = db.MusicReleases.Where(a => a.UserId.Equals(user.Id) && a.Status != "Deleted").ToList();
            foreach (var dist in distributions)
            {
                var list = db.DeezerReport_2020.Where(a => a.UPC.Trim().Equals(dist.UPCEAN) && a.Month.Trim().Equals(monthMusic) && a.Year.Trim().Equals(yearMusic)).ToList();

                if (list.Count > 0)
                {
                    tableD.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    tableD.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableD.AddCell(new Phrase(0, list[0].Label, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableD.AddCell(new Phrase(0, list[0].UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableD.AddCell(new Phrase(0, list[0].Artist, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableD.AddCell(new Phrase(0, list[0].Album, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableD.AddCell(new Phrase(0, "$" + Math.Round(list.Sum(a => a.Royalties),2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                    double asshare1 = (double)Math.Round(((list.Sum(a => a.Royalties) / 100) * share), 2);
                    tableD.AddCell(new Phrase(0, "$" + asshare1, FontFactory.GetFont(FontFactory.COURIER, 10)));

                    revD = revD + (double)list.Sum(a => a.Royalties);
                    musicTotal = musicTotal + (double)list.Sum(a => a.Royalties);
                }
            }

            tableD.AddCell("Total");
            tableD.AddCell("");
            tableD.AddCell("");
            tableD.AddCell("");
            tableD.AddCell("");
            tableD.AddCell("");
            tableD.AddCell("$" + Math.Round(revD,2).ToString());
            double asshare3D = (double)Math.Round(((revD / 100) * share), 2);
            tableD.AddCell("$" + asshare3D);
            pdfDocD.Add(tableD);


            foreach (var x in distributions)
            {
                var list = db.DeezerReport_2020.Where(a => a.UPC.Trim().Equals(x.UPCEAN) && a.Month.Trim().Equals(monthMusic) && a.Year.Trim().Equals(yearMusic)).ToList();

                PdfPTable table1 = new PdfPTable(8);
                table1.WidthPercentage = 100;
                table1.HorizontalAlignment = 0;
                table1.SpacingBefore = 20f;
                table1.SpacingAfter = 30f;
                float[] columnWidths1 = new float[] { 40f, 50f, 50f, 100f, 100f, 300f, 100f, 90f };
                table1.SetWidthPercentage(columnWidths1, PageSize.A4.Rotate());

                if (list.Count > 0)
                {
                    Chunk chunk1 = new Chunk("Relesse Name: " + x.ReleaseName);
                    chunk1.Font = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);

                    pdfDocD.Add(chunk1);

                    table1.AddCell("Month");
                    table1.AddCell("Year");
                    table1.AddCell("Country");
                    table1.AddCell("UPC");
                    table1.AddCell("ISRC");
                    table1.AddCell("Artist Name");
                    table1.AddCell("Revenue");
                    table1.AddCell("Share");

                    foreach (var dist in list)
                    {
                        table1.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        table1.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.Country, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.ISRC, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.Artist, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, "$" + Math.Round(dist.Royalties,2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                        double asshare2 = (double)Math.Round((((dist.Royalties) / 100) * share), 2);
                        table1.AddCell(new Phrase(0, "$" + asshare2, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    }

                    table1.AddCell("Total");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("$" + Math.Round(list.Sum(a => a.Royalties),2).ToString());
                    double asshare5 = (double)Math.Round(((list.Sum(a => a.Royalties / 100)) * share), 2);
                    table1.AddCell("$" + asshare5);


                    pdfDocD.Add(table1);

                    Paragraph para = new Paragraph();
                    para.Add("");
                    para.SpacingBefore = 20f;
                    para.SpacingAfter = 20f;
                    pdfDocD.Add(para);
                }

            }
            pdfWriterD.CloseStream = false;
            pdfDocD.Close();

            var deezersTotal = (double)Math.Round(((musicTotal / 100) * share), 2);
            musicTotal = 0.0;

            var paymentD = new MonthlyPayments();
            paymentD.User = user.Email;
            paymentD.MusicAmount = musicTotal.ToString();
            paymentD.EntAmount = "Deezer Report";
            paymentD.Date_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            paymentD.Music_Month = monthMusic;
            paymentD.Music_Year = yearMusic;

            db.MonthlyPayments.Add(paymentD);
            db.SaveChanges();


            // Google Play Report
            /*Creating iTextSharp’s Document & Writer*/
            Document pdfDocG = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            MemoryStream memoryStreamG = new MemoryStream();
            PdfWriter pdfWriterG = PdfWriter.GetInstance(pdfDocG, memoryStreamG);
            pdfDocG.Open();


            PdfPTable tableG = new PdfPTable(8);
            tableG.WidthPercentage = 100;
            tableG.HorizontalAlignment = 0;
            tableG.SpacingBefore = 20f;
            tableG.SpacingAfter = 30f;

            float[] columnWidthsG = new float[] { 50f, 50f, 100f, 100f, 250f, 110f, 90f, 80f };
            tableG.SetWidthPercentage(columnWidthsG, PageSize.A4.Rotate());

            tableG.AddCell("Month");
            tableG.AddCell("Year");
            tableG.AddCell("Label");
            tableG.AddCell("UPC");
            tableG.AddCell("Artist Name");
            tableG.AddCell("Container Title");
            tableG.AddCell("Revenue");
            tableG.AddCell("Share");

            var pdfImageG = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
            pdfImageG.Alignment = Element.ALIGN_MIDDLE;


            pdfDocG.Add(pdfImageG);
            var para12G = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Kaveldunsvägen 21, 80636 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
            para12G.Alignment = Element.ALIGN_MIDDLE;
            pdfDocG.Add(para12G);

            Chunk chunk4G = new Chunk("Official Payment Report - GooglePlay Network:" + get_Month(monthMusic) + "-" + yearMusic);
            chunk4G.Font = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.BLACK);
            pdfDocG.Add(chunk4G);

            double revG = 0;
            //var distributionsG = db.MusicReleases.Where(a => a.UserId.Equals(user.Id) && a.Status != "Deleted").ToList();
            foreach (var dist in distributions)
            {
                var list = db.GoogleMusicReport_2020.Where(a => a.UPC.Trim().Equals(dist.UPCEAN) && a.Month.Trim().Equals(monthMusic) && a.Year.Trim().Equals(yearMusic)).ToList();

                if (list.Count > 0)
                {
                    tableG.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    tableG.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableG.AddCell(new Phrase(0, list[0].Label, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableG.AddCell(new Phrase(0, list[0].UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableG.AddCell(new Phrase(0, list[0].Artist, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableG.AddCell(new Phrase(0, list[0].Container_Title, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableG.AddCell(new Phrase(0, "$" + Math.Round(list.Sum(a => a.EUR_Amount),2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                    double asshare1 = (double)Math.Round(((list.Sum(a => a.EUR_Amount) / 100) * share), 2);
                    tableG.AddCell(new Phrase(0, "$" + asshare1, FontFactory.GetFont(FontFactory.COURIER, 10)));

                    revG = revG + (double)list.Sum(a => a.EUR_Amount);
                    musicTotal = musicTotal + (double)list.Sum(a => a.EUR_Amount);
                }
            }

            tableG.AddCell("Total");
            tableG.AddCell("");
            tableG.AddCell("");
            tableG.AddCell("");
            tableG.AddCell("");
            tableG.AddCell("");
            tableG.AddCell("$" + Math.Round(revG,2).ToString());
            double asshare3G = (double)Math.Round(((revG / 100) * share), 2);
            tableG.AddCell("$" + asshare3G);
            pdfDocG.Add(tableG);


            foreach (var x in distributions)
            {
                var list = db.GoogleMusicReport_2020.Where(a => a.UPC.Trim().Equals(x.UPCEAN) && a.Month.Trim().Equals(monthMusic) && a.Year.Trim().Equals(yearMusic)).ToList();

                PdfPTable table1 = new PdfPTable(8);
                table1.WidthPercentage = 100;
                table1.HorizontalAlignment = 0;
                table1.SpacingBefore = 20f;
                table1.SpacingAfter = 30f;
                float[] columnWidths1 = new float[] { 40f, 50f, 50f, 100f, 100f, 300f, 100f, 90f };
                table1.SetWidthPercentage(columnWidths1, PageSize.A4.Rotate());

                if (list.Count > 0)
                {
                    Chunk chunk1 = new Chunk("Relesse Name: " + x.ReleaseName);
                    chunk1.Font = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);

                    pdfDocG.Add(chunk1);

                    table1.AddCell("Month");
                    table1.AddCell("Year");
                    table1.AddCell("Total Plays");
                    table1.AddCell("UPC");
                    table1.AddCell("ISRC");
                    table1.AddCell("Artist Name");
                    table1.AddCell("Revenue");
                    table1.AddCell("Share");

                    foreach (var dist in list)
                    {
                        table1.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        table1.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.Total_Plays.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.ISRC, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.Artist, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, "$" + Math.Round(dist.EUR_Amount,2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                        double asshare2 = (double)Math.Round((((dist.EUR_Amount) / 100) * share), 2);
                        table1.AddCell(new Phrase(0, "$" + asshare2, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    }

                    table1.AddCell("Total");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("$" + Math.Round(list.Sum(a => a.EUR_Amount),2).ToString());
                    double asshare5 = (double)Math.Round(((list.Sum(a => a.EUR_Amount / 100)) * share), 2);
                    table1.AddCell("$" + asshare5);


                    pdfDocG.Add(table1);

                    Paragraph para = new Paragraph();
                    para.Add("");
                    para.SpacingBefore = 20f;
                    para.SpacingAfter = 20f;
                    pdfDocG.Add(para);
                }

            }
            pdfWriterG.CloseStream = false;
            pdfDocG.Close();

            var googlePlayTotal = (double)Math.Round(((musicTotal / 100) * share), 2);
            musicTotal = 0.0;

            var paymentG = new MonthlyPayments();
            paymentG.User = user.Email;
            paymentG.MusicAmount = musicTotal.ToString();
            paymentG.EntAmount = "GooglePlay Report";
            paymentG.Date_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            paymentG.Music_Month = monthMusic;
            paymentG.Music_Year = yearMusic;

            db.MonthlyPayments.Add(paymentG);
            db.SaveChanges();


            // Sound Cloud  Report
            /*Creating iTextSharp’s Document & Writer*/
            Document pdfDocS = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            MemoryStream memoryStreamS = new MemoryStream();
            PdfWriter pdfWriterS = PdfWriter.GetInstance(pdfDocS, memoryStreamS);
            pdfDocS.Open();


            PdfPTable tableS = new PdfPTable(8);
            tableS.WidthPercentage = 100;
            tableS.HorizontalAlignment = 0;
            tableS.SpacingBefore = 20f;
            tableS.SpacingAfter = 30f;

            float[] columnWidthsS = new float[] { 50f, 50f, 100f, 100f, 250f, 110f, 90f, 80f };
            tableS.SetWidthPercentage(columnWidthsS, PageSize.A4.Rotate());

            tableS.AddCell("Month");
            tableS.AddCell("Year");
            tableS.AddCell("Label");
            tableS.AddCell("UPC");
            tableS.AddCell("Artist Name");
            tableS.AddCell("Album Name");
            tableS.AddCell("Revenue");
            tableS.AddCell("Share");

            var pdfImageS = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
            pdfImageS.Alignment = Element.ALIGN_MIDDLE;


            pdfDocS.Add(pdfImageS);
            var para12S = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Kaveldunsvägen 21, 80636 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
            para12S.Alignment = Element.ALIGN_MIDDLE;
            pdfDocS.Add(para12S);

            Chunk chunk4S = new Chunk("Official Payment Report - SoundCloud Network:" + get_Month(monthMusic) + "-" + yearMusic);
            chunk4S.Font = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.BLACK);
            pdfDocS.Add(chunk4S);

            double revS = 0;
            //var distributionsS = db.MusicReleases.Where(a => a.UserId.Equals(user.Id) && a.Status != "Deleted").ToList();
            foreach (var dist in distributions)
            {
                var list = db.SoundCloudReport_2020.Where(a => a.UPC.Trim().Equals(dist.UPCEAN) && a.Month.Trim().Equals(monthMusic) && a.Year.Trim().Equals(yearMusic)).ToList();

                if (list.Count > 0)
                {
                    tableS.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    tableS.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableS.AddCell(new Phrase(0, list[0].LabelName, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableS.AddCell(new Phrase(0, list[0].UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableS.AddCell(new Phrase(0, list[0].Artist_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableS.AddCell(new Phrase(0, list[0].Album_Title, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    tableS.AddCell(new Phrase(0, "$" + Math.Round(list.Sum(a => a.Total_Revenue),2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                    double asshare1 = (double)Math.Round(((list.Sum(a => a.Total_Revenue) / 100) * share), 2);
                    tableS.AddCell(new Phrase(0, "$" + asshare1, FontFactory.GetFont(FontFactory.COURIER, 10)));

                    revS = revS + (double)list.Sum(a => a.Total_Revenue);
                    musicTotal = musicTotal + (double)list.Sum(a => a.Total_Revenue);
                }
            }

            tableS.AddCell("Total");
            tableS.AddCell("");
            tableS.AddCell("");
            tableS.AddCell("");
            tableS.AddCell("");
            tableS.AddCell("");
            tableS.AddCell("$" + Math.Round(revS,2).ToString());
            double asshare3S = (double)Math.Round(((revS / 100) * share), 2);
            tableS.AddCell("$" + asshare3S);
            pdfDocS.Add(tableS);


            foreach (var x in distributions)
            {
                var list = db.SoundCloudReport_2020.Where(a => a.UPC.Trim().Equals(x.UPCEAN) && a.Month.Trim().Equals(monthMusic) && a.Year.Trim().Equals(yearMusic)).ToList();

                PdfPTable table1 = new PdfPTable(8);
                table1.WidthPercentage = 100;
                table1.HorizontalAlignment = 0;
                table1.SpacingBefore = 20f;
                table1.SpacingAfter = 30f;
                float[] columnWidths1 = new float[] { 40f, 50f, 50f, 100f, 100f, 300f, 100f, 90f };
                table1.SetWidthPercentage(columnWidths1, PageSize.A4.Rotate());

                if (list.Count > 0)
                {
                    Chunk chunk1 = new Chunk("Relesse Name: " + x.ReleaseName);
                    chunk1.Font = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);

                    pdfDocS.Add(chunk1);

                    table1.AddCell("Month");
                    table1.AddCell("Year");
                    table1.AddCell("Label");
                    table1.AddCell("UPC");
                    table1.AddCell("ISRC");
                    table1.AddCell("Artist Name");
                    table1.AddCell("Revenue");
                    table1.AddCell("Share");

                    foreach (var dist in list)
                    {
                        table1.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                        table1.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.LabelName, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.ISRC, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, dist.Artist_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
                        table1.AddCell(new Phrase(0, "$" + Math.Round(dist.Total_Revenue,2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                        double asshare2 = (double)Math.Round((((dist.Total_Revenue) / 100) * share), 2);
                        table1.AddCell(new Phrase(0, "$" + asshare2, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    }

                    table1.AddCell("Total");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell("$" + Math.Round(list.Sum(a => a.Total_Revenue),2).ToString());
                    double asshare5 = (double)Math.Round(((list.Sum(a => a.Total_Revenue / 100)) * share), 2);
                    table1.AddCell("$" + asshare5);


                    pdfDocS.Add(table1);

                    Paragraph para = new Paragraph();
                    para.Add("");
                    para.SpacingBefore = 20f;
                    para.SpacingAfter = 20f;
                    pdfDocS.Add(para);
                }

            }
            pdfWriterS.CloseStream = false;
            pdfDocS.Close();

            var soundCloudTotal = (double)Math.Round(((musicTotal / 100) * share), 2);

            var paymentS = new MonthlyPayments();
            paymentS.User = user.Email;
            paymentS.MusicAmount = musicTotal.ToString();
            paymentS.EntAmount = "SoundCloud Report";
            paymentS.Date_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            paymentS.Music_Month = monthMusic;
            paymentS.Music_Year = yearMusic;

            db.MonthlyPayments.Add(paymentS);
            db.SaveChanges();
            var count = db.MonthlyPayments.Count();


            /*Creating iTextSharp’s Document & Writer*/
            Document pdfDocSELF = new Document(PageSize.A4, 20, 10, 10, 20);
            MemoryStream memoryStreamSELF = new MemoryStream();
            PdfWriter pdfWriterSELF = PdfWriter.GetInstance(pdfDocSELF, memoryStreamSELF);
            pdfDocSELF.Open();
            /*End*/

            PdfPTable tableSELF = new PdfPTable(2);
            tableSELF.DefaultCell.Border = Rectangle.NO_BORDER;
            tableSELF.WidthPercentage = 100;
            tableSELF.HorizontalAlignment = 0;
            tableSELF.SpacingBefore = 20f;
            tableSELF.SpacingAfter = 30f;
            var para12SELF = new Paragraph("\n\n\n\n TP Digital AB Sweden \n Kaveldunsvägen 21, 80636 Gävle  \n VAT Number: SE559134761101.", FontFactory.GetFont("Georgia", 15, Font.NORMAL, BaseColor.BLACK));
            para12SELF.Alignment = Element.ALIGN_LEFT;
            tableSELF.AddCell(para12SELF);
            var pdfImageSELF = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
            pdfImageSELF.ScaleToFit(20, 20);
            pdfImageSELF.Alignment = Element.ALIGN_MIDDLE;
            tableSELF.AddCell(pdfImageSELF);
            pdfDocSELF.Add(tableSELF);


            PdfPTable table1SELF = new PdfPTable(2);
            table1SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            table1SELF.WidthPercentage = 100;
            var para1SELF = new Paragraph(DateTime.UtcNow.Date.ToString("d"), FontFactory.GetFont("Courier", 16, Font.BOLD, BaseColor.BLACK));
            para1SELF.Alignment = Element.ALIGN_LEFT;
            table1SELF.AddCell(para1SELF);
            var para2SELF = new Paragraph("  SELF-BILLING INVOICE NUMBER", FontFactory.GetFont("Courier", 16, Font.BOLD, BaseColor.BLACK));
            para1SELF.Alignment = Element.ALIGN_RIGHT;
            table1SELF.AddCell(para2SELF);
            pdfDocSELF.Add(table1SELF);

            var paraSELF = new Paragraph("INVOICE FORM: ", FontFactory.GetFont("Courier", 16, Font.BOLD, BaseColor.BLACK));
            paraSELF.Alignment = Element.ALIGN_LEFT;
            pdfDocSELF.Add(paraSELF);
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDocSELF.Add(line);
            var para5SELF = new Paragraph("TP-Digital -	000" + count.ToString(), FontFactory.GetFont("Calibri", 16, Font.BOLD, BaseColor.BLACK));
            para5SELF.Alignment = Element.ALIGN_RIGHT;
            pdfDocSELF.Add(para5SELF);

            var table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            var columnWidths1SELF = new float[] { 70f, 400f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;

            var para6SELF = new Paragraph("Name: ", FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;

            var para7SELF = new Paragraph(user.FirstName + " " + user.LastName, FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;

            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);


            table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            columnWidths1SELF = new float[] { 70f, 400f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;
            para6SELF = new Paragraph("Address: ", FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;
            para7SELF = new Paragraph(user.Email, FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;
            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);


            table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            columnWidths1SELF = new float[] { 100f, 370f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;
            para6SELF = new Paragraph("VAT Number: ", FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;
            para7SELF = new Paragraph("Not Applicable\n\n", FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;
            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);


            table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            columnWidths1SELF = new float[] { 400f, 70f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;
            para6SELF = new Paragraph("\n\n\nDESCRIPTIONS", FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;
            para7SELF = new Paragraph("\n\n\nAMOUNT", FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;
            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);

            line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDocSELF.Add(line);

            table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            columnWidths1SELF = new float[] { 400f, 70f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;
            para6SELF = new Paragraph(monthMusic + "/" + yearMusic + "  Spotify Network Report", FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;
            para7SELF = new Paragraph("$" + spotifyTotal.ToString(), FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;
            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);

            table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            columnWidths1SELF = new float[] { 400f, 70f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;
            para6SELF = new Paragraph(monthMusic + "/" + yearMusic + "  Deezer Network Report", FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;
            para7SELF = new Paragraph("$" + deezersTotal.ToString(), FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;
            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);

            table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            columnWidths1SELF = new float[] { 400f, 70f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;
            para6SELF = new Paragraph(monthMusic + "/" + yearMusic + "  GooglePlay Network Report", FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;
            para7SELF = new Paragraph("$" + googlePlayTotal.ToString(), FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;
            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);

            table3SELF = new PdfPTable(2);
            table3SELF.WidthPercentage = 100;
            table3SELF.DefaultCell.Border = Rectangle.NO_BORDER;
            columnWidths1SELF = new float[] { 400f, 70f };
            table3SELF.SetWidthPercentage(columnWidths1SELF, PageSize.A4);
            table3SELF.WidthPercentage = 100;
            para6SELF = new Paragraph(monthMusic + "/" + yearMusic + "  SoundCloud Network Report", FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para6SELF.Alignment = Element.ALIGN_LEFT;
            para7SELF = new Paragraph("$" + soundCloudTotal.ToString(), FontFactory.GetFont("Calibri", 14, Font.NORMAL, BaseColor.BLACK));
            para7SELF.Alignment = Element.ALIGN_RIGHT;
            table3SELF.AddCell(para6SELF);
            table3SELF.AddCell(para7SELF);
            pdfDocSELF.Add(table3SELF);


            para5SELF = new Paragraph("\n\nReverse charge in accordance with \nChapter 1. § 2 1st 2. The VAT Act\n\nPayment Terms:\nThis invoice has been paid to your nominated Bank account. \nPlease keep this self-bill invoice for your records.\nAmounts under $70 are not paid until the total has reached $70.", FontFactory.GetFont("Georgia", 12, Font.NORMAL, BaseColor.BLACK));
            para5SELF.Alignment = Element.ALIGN_LEFT;
            pdfDocSELF.Add(para5SELF);

            para5SELF = new Paragraph("\n\nSUB-TOTAL: $" + (spotifyTotal + deezersTotal + googlePlayTotal + soundCloudTotal).ToString() + "\nVAT: US $0.00\nTotal Amount: $" + (spotifyTotal + deezersTotal + googlePlayTotal + soundCloudTotal).ToString() + "\nBalance Due: $0.00", FontFactory.GetFont("Georgia", 12, Font.BOLD, BaseColor.BLACK));
            para5SELF.Alignment = Element.ALIGN_RIGHT;
            pdfDocSELF.Add(para5SELF);

            para5SELF = new Paragraph("Thank You for being our customer.", FontFactory.GetFont("Georgia", 12, Font.NORMAL, BaseColor.BLACK));
            para5SELF.Alignment = Element.ALIGN_RIGHT;
            pdfDocSELF.Add(para5SELF);

            /*Creating the PDF file*/
            pdfWriterSELF.CloseStream = false;
            pdfDocSELF.Close();



            var senderMail = new MailAddress("support@tempodigital.org", "Tempo Digital Team");
            var receiverMial = new MailAddress("munir@tempodigital.org", "Receiver");

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderMail.Address, ConfigurationManager.AppSettings["GmailPasword"])
            };

            MailMessage message = new MailMessage(senderMail, receiverMial);
            message.Subject = "Monthly Report - Self Billing";
            message.Body = "Hi,\nHere are the Reports for Month/Year" + monthMusic + "/" + yearMusic + ".\nPlease find Spotify, Deezer, GooglePlay and SoundCloud Monthly Report and Self Billing in Attached Files.\n\nSender,\nTempo Digital Team.";
            memoryStreamSELF.Position = 0;
            message.Attachments.Add(new Attachment(memoryStreamSELF, "Distributions-Self-Billing.pdf"));
            memoryStream.Position = 0;
            message.Attachments.Add(new Attachment(memoryStream, "Spotify-Monthly-Report.pdf"));
            memoryStreamD.Position = 0;
            message.Attachments.Add(new Attachment(memoryStreamD, "Deezer-Monthly-Report.pdf"));
            memoryStreamG.Position = 0;
            message.Attachments.Add(new Attachment(memoryStreamG, "GooglePlay-Monthly-Report.pdf"));
            memoryStreamS.Position = 0;
            message.Attachments.Add(new Attachment(memoryStreamS, "SoundCloud-Monthly-Report.pdf"));
            smtp.Send(message);
        }

        //void Spotify_Monthly_Report_Self_Billing(string email, string yearMusic, string monthMusic)
        //{
        //    var user = db.Users.Where(a => a.Email.Equals(email)).FirstOrDefault();
        //    int share = Int32.Parse(user.share);

        //    double musicTotal = 0.0;

        //    /*Creating iTextSharp’s Document & Writer*/
        //    Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
        //    MemoryStream memoryStream = new MemoryStream();
        //    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
        //    pdfDoc.Open();


        //    PdfPTable table = new PdfPTable(8);
        //    table.WidthPercentage = 100;
        //    table.HorizontalAlignment = 0;
        //    table.SpacingBefore = 20f;
        //    table.SpacingAfter = 30f;

        //    float[] columnWidths = new float[] { 50f, 50f, 50f, 100f, 260f, 115f, 100f, 100f };
        //    table.SetWidthPercentage(columnWidths, PageSize.A4.Rotate());

        //    table.AddCell("Month");
        //    table.AddCell("Year");
        //    table.AddCell("Country");
        //    table.AddCell("UPC");
        //    table.AddCell("Artist Name");
        //    table.AddCell("Composer Name");
        //    table.AddCell("Revenue");
        //    table.AddCell("Share");

        //    var pdfImage = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
        //    pdfImage.Alignment = Element.ALIGN_MIDDLE;


        //    pdfDoc.Add(pdfImage);
        //    var para12 = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Älgpasset 10 C 806 36 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
        //    para12.Alignment = Element.ALIGN_MIDDLE;
        //    pdfDoc.Add(para12);

        //    Chunk chunk4 = new Chunk("Official Payment Report -Spotify Network:" + get_Month(monthMusic) + "-" + yearMusic );
        //    chunk4.Font = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.BLACK);
        //    pdfDoc.Add(chunk4);
            
        //    double rev = 0;
        //    var distributions = db.MusicReleases.Where(a => a.UserId.Equals(user.Id)).ToList();
        //    foreach (var dist in distributions)
        //    {
        //        var list = db.SpotifyReportFull.Where(a => a.UPC.Equals(dist.UPCEAN)).ToList();

        //        if (list.Count > 0)
        //        {
        //            table.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
        //            table.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            table.AddCell(new Phrase(0, list[0].Country, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            table.AddCell(new Phrase(0, list[0].UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            table.AddCell(new Phrase(0, list[0].Artist_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            table.AddCell(new Phrase(0, list[0].Composer_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            table.AddCell(new Phrase(0, "$" + list.Sum(a => a.USD_Payable).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            double asshare1 = (double)Math.Round(((list.Sum(a => a.USD_Payable) / 100) * share), 2);
        //            table.AddCell(new Phrase(0, "$" + asshare1, FontFactory.GetFont(FontFactory.COURIER, 10)));

        //            rev = rev + (double)list.Sum(a => a.USD_Payable);
        //            musicTotal = musicTotal + (double)list.Sum(a => a.USD_Payable);
        //        }
        //    }

        //    table.AddCell("Total");
        //    table.AddCell("");
        //    table.AddCell("");
        //    table.AddCell("");
        //    table.AddCell("");
        //    table.AddCell("");
        //    table.AddCell("$" + rev.ToString());
        //    double asshare3 = (double)Math.Round(((rev / 100) * share), 2);
        //    table.AddCell("$" + asshare3);
        //    pdfDoc.Add(table);


        //    foreach (var x in distributions)
        //    {
        //        var list = db.SpotifyReportFull.Where(a => a.UPC.Equals(x.UPCEAN)).ToList();

        //        PdfPTable table1 = new PdfPTable(8);
        //        table1.WidthPercentage = 100;
        //        table1.HorizontalAlignment = 0;
        //        table1.SpacingBefore = 20f;
        //        table1.SpacingAfter = 30f;
        //        float[]columnWidths1 = new float[] { 40f, 40f, 40f, 100f, 290f, 115f, 100f, 100f };
        //        table1.SetWidthPercentage(columnWidths1, PageSize.A4.Rotate());
                
        //        if (list.Count > 0)
        //        {
        //            Chunk chunk1 = new Chunk("Relesse Name: "+x.ReleaseName);
        //            chunk1.Font = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);

        //            pdfDoc.Add(chunk1);

        //            table1.AddCell("Month");
        //            table1.AddCell("Year");
        //            table1.AddCell("Country");
        //            table1.AddCell("UPC");
        //            table1.AddCell("Artist Name");
        //            table1.AddCell("Composer Name");
        //            table1.AddCell("Revenue");
        //            table1.AddCell("Share");

        //            foreach (var dist in list)
        //            {
        //                table1.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
        //                table1.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.Country, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.Artist_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.Composer_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, "$" + dist.USD_Payable.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                double asshare2 = (double)Math.Round((((dist.USD_Payable) / 100) * share), 2);
        //                table1.AddCell(new Phrase(0, "$" + asshare2, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            }

        //            table1.AddCell("Total");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("$" + list.Sum(a =>a.USD_Payable).ToString());
        //            double asshare5 = (double)Math.Round(((list.Sum(a => a.USD_Payable / 100)) * share), 2);
        //            table1.AddCell("$" + asshare5);


        //            pdfDoc.Add(table1);

        //            Paragraph para = new Paragraph();
        //            para.Add("");
        //            para.SpacingBefore = 20f;
        //            para.SpacingAfter = 20f;
        //            pdfDoc.Add(para);
        //        }

        //    }
        //    pdfWriter.CloseStream = false;
        //    pdfDoc.Close();

        //    musicTotal = (double)Math.Round(((musicTotal / 100) * share), 2);

        //    var payment = new MonthlyPayments();
        //    payment.User = user.Email;
        //    payment.MusicAmount = musicTotal.ToString();
        //    payment.EntAmount = "Spotify Report";
        //    payment.Date_Time = DateTime.Now.ToString();
        //    payment.Music_Month = monthMusic;
        //    payment.Music_Year = yearMusic;

        //    db.MonthlyPayments.Add(payment);
        //    db.SaveChanges();
        //    var count = db.MonthlyPayments.Count();

        //    LocalReport lr = new LocalReport();
        //    string path = Path.Combine(Server.MapPath("~/PDF"), "Report5.rdlc");
        //    lr.ReportPath = path;

        //    ReportParameter[] parms = new ReportParameter[13];
        //    parms[0] = new ReportParameter("Name", user.FirstName + " " + user.LastName);
        //    parms[1] = new ReportParameter("Address", user.Email);
        //    parms[2] = new ReportParameter("InvoiceNumber", "0000" + count.ToString());
        //    parms[3] = new ReportParameter("VATNumber", "Not Applicable");
        //    parms[4] = new ReportParameter("Music", monthMusic + "/" + yearMusic + "  Spotify Network Report");
        //    parms[5] = new ReportParameter("Ent", " ---");
        //    parms[6] = new ReportParameter("MusicAmount", "$ " + musicTotal.ToString());
        //    parms[7] = new ReportParameter("EntAmount", "---" );
        //    parms[8] = new ReportParameter("SubTotal", "$ " + (musicTotal).ToString());
        //    parms[9] = new ReportParameter("VATUS", "$ 0.00");
        //    parms[10] = new ReportParameter("TotalAmount", "$ " + (musicTotal).ToString());
        //    parms[11] = new ReportParameter("BalanceDue", "$ 0.00");
        //    parms[12] = new ReportParameter("Date", DateTime.Now.ToString());
        //    lr.SetParameters(parms);


        //    string ReType = "PDF";
        //    string mimeTyoe;
        //    string encoding;
        //    string fileNameExtension;

        //    string deviceInfo =
        //        "<DeviceInfo>" +
        //        "<OutputFormat>" + "PDF" + "</OutputFormat>" +
        //        "<PageWidth> 9in </PageWidth>" +
        //        "<PageHeight>11in</PageHeight>" +
        //        "<MarginTop>0.5in</MarginTop>" +
        //        "<MarginLeft>0.5in</MarginLeft>" +
        //        "<MarginRight>0.5in</MarginRight>" +
        //        "<MarginBottom>0.5in</MarginBottom>" +
        //        "</DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    renderedBytes = lr.Render(
        //        ReType,
        //        deviceInfo,
        //        out mimeTyoe,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings
        //        );



        //    var senderMail = new MailAddress("support@tempodigital.org", "Sender");
        //    var receiverMial = new MailAddress("munir.hadrovic@gmail.com", "Receiver");

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(senderMail.Address, ConfigurationManager.AppSettings["GmailPasword"])
        //    };

        //    MailMessage message = new MailMessage(senderMail, receiverMial);
        //    message.Subject = "Monthly Report - Self Billing";
        //    message.Body = "Hi,\nHere are the Reports for Month/Year" + monthMusic + "/" + yearMusic +".\nPlease find Spotify Monthly Report and Self Billing in Attached Files.";
        //    message.Attachments.Add(new Attachment(new MemoryStream(renderedBytes), "Self-Billing.pdf"));
        //    memoryStream.Position = 0;
        //    message.Attachments.Add(new Attachment(memoryStream, "Spotify-Monthly-Report.pdf"));
        //    smtp.Send(message);

        //}

        //void Deezers_Ent_Monthly_Report_Self_Billing(string email, string yearMusic, string monthMusic)
        //{
        //    var user = db.Users.Where(a => a.Email.Equals(email)).FirstOrDefault();
        //    int share = Int32.Parse(user.share);

        //    double musicTotal = 0.0;

        //    /*Creating iTextSharp’s Document & Writer*/
        //    Document pdfDocD = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
        //    MemoryStream memoryStreamD = new MemoryStream();
        //    PdfWriter pdfWriterD = PdfWriter.GetInstance(pdfDocD, memoryStreamD);
        //    pdfDocD.Open();


        //    PdfPTable tableD = new PdfPTable(8);
        //    tableD.WidthPercentage = 100;
        //    tableD.HorizontalAlignment = 0;
        //    tableD.SpacingBefore = 20f;
        //    tableD.SpacingAfter = 30f;

        //    float[] columnWidthsD = new float[] { 50f, 50f, 100f, 100f, 250f, 110f, 90f, 80f };
        //    tableD.SetWidthPercentage(columnWidthsD, PageSize.A4.Rotate());

        //    tableD.AddCell("Month");
        //    tableD.AddCell("Year");
        //    tableD.AddCell("Label");
        //    tableD.AddCell("UPC");
        //    tableD.AddCell("Artist Name");
        //    tableD.AddCell("Album Name");
        //    tableD.AddCell("Revenue");
        //    tableD.AddCell("Share");

        //    var pdfImageD = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
        //    pdfImageD.Alignment = Element.ALIGN_MIDDLE;


        //    pdfDocD.Add(pdfImageD);
        //    var para12D = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Älgpasset 10 C 806 36 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
        //    para12D.Alignment = Element.ALIGN_MIDDLE;
        //    pdfDocD.Add(para12D);

        //    Chunk chunk4D = new Chunk("Official Payment Report - Deezers Network:" + get_Month(monthMusic) + "-" + yearMusic);
        //    chunk4D.Font = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.BLACK);
        //    pdfDocD.Add(chunk4D);

        //    double revD = 0;
        //    var distributionsD = db.MusicReleases.Where(a => a.UserId.Equals(user.Id)).ToList();
        //    foreach (var dist in distributionsD)
        //    {
        //        var list = db.Deezers.Where(a => a.UPC.Equals(dist.UPCEAN)).ToList();

        //        if (list.Count > 0)
        //        {
        //            tableD.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
        //            tableD.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableD.AddCell(new Phrase(0, list[0].Label, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableD.AddCell(new Phrase(0, list[0].UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableD.AddCell(new Phrase(0, list[0].Artist, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableD.AddCell(new Phrase(0, list[0].Album, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableD.AddCell(new Phrase(0, "$" + list.Sum(a => a.Royalties).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            double asshare1 = (double)Math.Round(((list.Sum(a => a.Royalties) / 100) * share), 2);
        //            tableD.AddCell(new Phrase(0, "$" + asshare1, FontFactory.GetFont(FontFactory.COURIER, 10)));

        //            revD = revD + (double)list.Sum(a => a.Royalties);
        //            musicTotal = musicTotal + (double)list.Sum(a => a.Royalties);
        //        }
        //    }

        //    tableD.AddCell("Total");
        //    tableD.AddCell("");
        //    tableD.AddCell("");
        //    tableD.AddCell("");
        //    tableD.AddCell("");
        //    tableD.AddCell("");
        //    tableD.AddCell("$" + revD.ToString());
        //    double asshare3 = (double)Math.Round(((revD / 100) * share), 2);
        //    tableD.AddCell("$" + asshare3);
        //    pdfDocD.Add(tableD);


        //    foreach (var x in distributionsD)
        //    {
        //        var list = db.Deezers.Where(a => a.UPC.Equals(x.UPCEAN)).ToList();

        //        PdfPTable table1 = new PdfPTable(8);
        //        table1.WidthPercentage = 100;
        //        table1.HorizontalAlignment = 0;
        //        table1.SpacingBefore = 20f;
        //        table1.SpacingAfter = 30f;
        //        float[] columnWidths1 = new float[] { 40f, 50f, 50f, 100f, 100f, 300f, 100f, 90f };
        //        table1.SetWidthPercentage(columnWidths1, PageSize.A4.Rotate());

        //        if (list.Count > 0)
        //        {
        //            Chunk chunk1 = new Chunk("Relesse Name: " + x.ReleaseName);
        //            chunk1.Font = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);

        //            pdfDocD.Add(chunk1);

        //            table1.AddCell("Month");
        //            table1.AddCell("Year");
        //            table1.AddCell("Country");
        //            table1.AddCell("UPC");
        //            table1.AddCell("ISRC");
        //            table1.AddCell("Artist Name");
        //            table1.AddCell("Revenue");
        //            table1.AddCell("Share");

        //            foreach (var dist in list)
        //            {
        //                table1.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
        //                table1.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.Country, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.ISRC, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.Artist, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, "$" + dist.Royalties.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                double asshare2 = (double)Math.Round((((dist.Royalties) / 100) * share), 2);
        //                table1.AddCell(new Phrase(0, "$" + asshare2, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            }

        //            table1.AddCell("Total");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("$" + list.Sum(a => a.Royalties).ToString());
        //            double asshare5 = (double)Math.Round(((list.Sum(a => a.Royalties / 100)) * share), 2);
        //            table1.AddCell("$" + asshare5);


        //            pdfDocD.Add(table1);

        //            Paragraph para = new Paragraph();
        //            para.Add("");
        //            para.SpacingBefore = 20f;
        //            para.SpacingAfter = 20f;
        //            pdfDocD.Add(para);
        //        }

        //    }
        //    pdfWriterD.CloseStream = false;
        //    pdfDocD.Close();

        //    musicTotal = (double)Math.Round(((musicTotal / 100) * share), 2);

        //    var paymentD = new MonthlyPayments();
        //    paymentD.User = user.Email;
        //    paymentD.MusicAmount = musicTotal.ToString();
        //    paymentD.EntAmount = "Deezers Report";
        //    paymentD.Date_Time = DateTime.Now.ToString();
        //    paymentD.Music_Month = monthMusic;
        //    paymentD.Music_Year = yearMusic;

        //    db.MonthlyPayments.Add(paymentD);
        //    db.SaveChanges();
        //    var count = db.MonthlyPayments.Count();

        //    LocalReport lr = new LocalReport();
        //    string path = Path.Combine(Server.MapPath("~/PDF"), "Report5.rdlc");
        //    lr.ReportPath = path;

        //    ReportParameter[] parms = new ReportParameter[13];
        //    parms[0] = new ReportParameter("Name", user.FirstName + " " + user.LastName);
        //    parms[1] = new ReportParameter("Address", user.Email);
        //    parms[2] = new ReportParameter("InvoiceNumber", "0000" + count.ToString());
        //    parms[3] = new ReportParameter("VATNumber", "Not Applicable");
        //    parms[4] = new ReportParameter("Music", monthMusic + "/" + yearMusic + "  Deezers Network Report");
        //    parms[5] = new ReportParameter("Ent", " ---");
        //    parms[6] = new ReportParameter("MusicAmount", "$ " + musicTotal.ToString());
        //    parms[7] = new ReportParameter("EntAmount", "---");
        //    parms[8] = new ReportParameter("SubTotal", "$ " + (musicTotal).ToString());
        //    parms[9] = new ReportParameter("VATUS", "$ 0.00");
        //    parms[10] = new ReportParameter("TotalAmount", "$ " + (musicTotal).ToString());
        //    parms[11] = new ReportParameter("BalanceDue", "$ 0.00");
        //    parms[12] = new ReportParameter("Date", DateTime.Now.ToString());
        //    lr.SetParameters(parms);


        //    string ReType = "PDF";
        //    string mimeTyoe;
        //    string encoding;
        //    string fileNameExtension;

        //    string deviceInfo =
        //        "<DeviceInfo>" +
        //        "<OutputFormat>" + "PDF" + "</OutputFormat>" +
        //        "<PageWidth> 9in </PageWidth>" +
        //        "<PageHeight>11in</PageHeight>" +
        //        "<MarginTop>0.5in</MarginTop>" +
        //        "<MarginLeft>0.5in</MarginLeft>" +
        //        "<MarginRight>0.5in</MarginRight>" +
        //        "<MarginBottom>0.5in</MarginBottom>" +
        //        "</DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    renderedBytes = lr.Render(
        //        ReType,
        //        deviceInfo,
        //        out mimeTyoe,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings
        //        );



        //    var senderMail = new MailAddress("support@tempodigital.org", "Sender");
        //    var receiverMial = new MailAddress("munir.hadrovic@gmail.com", "Receiver");

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(senderMail.Address, ConfigurationManager.AppSettings["GmailPasword"])
        //    };

        //    MailMessage message = new MailMessage(senderMail, receiverMial);
        //    message.Subject = "Monthly Report - Self Billing";
        //    message.Body = "Hi,\nHere are the Reports for Month/Year" + monthMusic + "/" + yearMusic + ".\nPlease find Deezers Monthly Report and Self Billing in Attached Files.";
        //    message.Attachments.Add(new Attachment(new MemoryStream(renderedBytes), "Self-Billing.pdf"));
        //    memoryStreamD.Position = 0;
        //    message.Attachments.Add(new Attachment(memoryStreamD, "Deezers-Monthly-Report.pdf"));
        //    smtp.Send(message);
        //}

        //void Google_Play_Ent_Monthly_Report_Self_Billing(string email, string yearMusic, string monthMusic)
        //{
        //    var user = db.Users.Where(a => a.Email.Equals(email)).FirstOrDefault();
        //    int share = Int32.Parse(user.share);

        //    double musicTotal = 0.0;

        //    /*Creating iTextSharp’s Document & Writer*/
        //    Document pdfDocG = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
        //    MemoryStream memoryStreamG = new MemoryStream();
        //    PdfWriter pdfWriterG = PdfWriter.GetInstance(pdfDocG, memoryStreamG);
        //    pdfDocG.Open();


        //    PdfPTable tableG = new PdfPTable(8);
        //    tableG.WidthPercentage = 100;
        //    tableG.HorizontalAlignment = 0;
        //    tableG.SpacingBefore = 20f;
        //    tableG.SpacingAfter = 30f;

        //    float[] columnWidthsG = new float[] { 50f, 50f, 100f, 100f, 250f, 110f, 90f, 80f };
        //    tableG.SetWidthPercentage(columnWidthsG, PageSize.A4.Rotate());

        //    tableG.AddCell("Month");
        //    tableG.AddCell("Year");
        //    tableG.AddCell("Label");
        //    tableG.AddCell("UPC");
        //    tableG.AddCell("Artist Name");
        //    tableG.AddCell("Container Title");
        //    tableG.AddCell("Revenue");
        //    tableG.AddCell("Share");

        //    var pdfImageG = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
        //    pdfImageG.Alignment = Element.ALIGN_MIDDLE;


        //    pdfDocG.Add(pdfImageG);
        //    var para12G = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Älgpasset 10 C 806 36 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
        //    para12G.Alignment = Element.ALIGN_MIDDLE;
        //    pdfDocG.Add(para12G);

        //    Chunk chunk4G = new Chunk("Official Payment Report - Google Play Network:" + get_Month(monthMusic) + "-" + yearMusic);
        //    chunk4G.Font = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.BLACK);
        //    pdfDocG.Add(chunk4G);

        //    double revG = 0;
        //    var distributionsG = db.MusicReleases.Where(a => a.UserId.Equals(user.Id)).ToList();
        //    foreach (var dist in distributionsG)
        //    {
        //        var list = db.Google_Play.Where(a => a.UPC.Equals(dist.UPCEAN)).ToList();

        //        if (list.Count > 0)
        //        {
        //            tableG.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
        //            tableG.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableG.AddCell(new Phrase(0, list[0].Label, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableG.AddCell(new Phrase(0, list[0].UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableG.AddCell(new Phrase(0, list[0].Artist, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableG.AddCell(new Phrase(0, list[0].Container_Title, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableG.AddCell(new Phrase(0, "$" + list.Sum(a => a.EUR_Amount).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            double asshare1 = (double)Math.Round(((list.Sum(a => a.EUR_Amount) / 100) * share), 2);
        //            tableG.AddCell(new Phrase(0, "$" + asshare1, FontFactory.GetFont(FontFactory.COURIER, 10)));

        //            revG = revG + (double)list.Sum(a => a.EUR_Amount);
        //            musicTotal = musicTotal + (double)list.Sum(a => a.EUR_Amount);
        //        }
        //    }

        //    tableG.AddCell("Total");
        //    tableG.AddCell("");
        //    tableG.AddCell("");
        //    tableG.AddCell("");
        //    tableG.AddCell("");
        //    tableG.AddCell("");
        //    tableG.AddCell("$" + revG.ToString());
        //    double asshare3G = (double)Math.Round(((revG / 100) * share), 2);
        //    tableG.AddCell("$" + asshare3G);
        //    pdfDocG.Add(tableG);


        //    foreach (var x in distributionsG)
        //    {
        //        var list = db.Google_Play.Where(a => a.UPC.Equals(x.UPCEAN)).ToList();

        //        PdfPTable table1 = new PdfPTable(8);
        //        table1.WidthPercentage = 100;
        //        table1.HorizontalAlignment = 0;
        //        table1.SpacingBefore = 20f;
        //        table1.SpacingAfter = 30f;
        //        float[] columnWidths1 = new float[] { 40f, 50f, 50f, 100f, 100f, 300f, 100f, 90f };
        //        table1.SetWidthPercentage(columnWidths1, PageSize.A4.Rotate());

        //        if (list.Count > 0)
        //        {
        //            Chunk chunk1 = new Chunk("Relesse Name: " + x.ReleaseName);
        //            chunk1.Font = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);

        //            pdfDocG.Add(chunk1);

        //            table1.AddCell("Month");
        //            table1.AddCell("Year");
        //            table1.AddCell("Total Plays");
        //            table1.AddCell("UPC");
        //            table1.AddCell("ISRC");
        //            table1.AddCell("Artist Name");
        //            table1.AddCell("Revenue");
        //            table1.AddCell("Share");

        //            foreach (var dist in list)
        //            {
        //                table1.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
        //                table1.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.Total_Plays.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.ISRC, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.Artist, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, "$" + dist.EUR_Amount.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                double asshare2 = (double)Math.Round((((dist.EUR_Amount) / 100) * share), 2);
        //                table1.AddCell(new Phrase(0, "$" + asshare2, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            }

        //            table1.AddCell("Total");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("$" + list.Sum(a => a.EUR_Amount).ToString());
        //            double asshare5 = (double)Math.Round(((list.Sum(a => a.EUR_Amount / 100)) * share), 2);
        //            table1.AddCell("$" + asshare5);


        //            pdfDocG.Add(table1);

        //            Paragraph para = new Paragraph();
        //            para.Add("");
        //            para.SpacingBefore = 20f;
        //            para.SpacingAfter = 20f;
        //            pdfDocG.Add(para);
        //        }

        //    }
        //    pdfWriterG.CloseStream = false;
        //    pdfDocG.Close();

        //    musicTotal = (double)Math.Round(((musicTotal / 100) * share), 2);

        //    var paymentG = new MonthlyPayments();
        //    paymentG.User = user.Email;
        //    paymentG.MusicAmount = musicTotal.ToString();
        //    paymentG.EntAmount = "Google Play Report";
        //    paymentG.Date_Time = DateTime.Now.ToString();
        //    paymentG.Music_Month = monthMusic;
        //    paymentG.Music_Year = yearMusic;

        //    db.MonthlyPayments.Add(paymentG);
        //    db.SaveChanges();
        //    var count = db.MonthlyPayments.Count();

        //    LocalReport lr = new LocalReport();
        //    string path = Path.Combine(Server.MapPath("~/PDF"), "Report5.rdlc");
        //    lr.ReportPath = path;

        //    ReportParameter[] parms = new ReportParameter[13];
        //    parms[0] = new ReportParameter("Name", user.FirstName + " " + user.LastName);
        //    parms[1] = new ReportParameter("Address", user.Email);
        //    parms[2] = new ReportParameter("InvoiceNumber", "0000" + count.ToString());
        //    parms[3] = new ReportParameter("VATNumber", "Not Applicable");
        //    parms[4] = new ReportParameter("Music", monthMusic + "/" + yearMusic + "  Google Play Network Report");
        //    parms[5] = new ReportParameter("Ent", " ---");
        //    parms[6] = new ReportParameter("MusicAmount", "$ " + musicTotal.ToString());
        //    parms[7] = new ReportParameter("EntAmount", "---");
        //    parms[8] = new ReportParameter("SubTotal", "$ " + (musicTotal).ToString());
        //    parms[9] = new ReportParameter("VATUS", "$ 0.00");
        //    parms[10] = new ReportParameter("TotalAmount", "$ " + (musicTotal).ToString());
        //    parms[11] = new ReportParameter("BalanceDue", "$ 0.00");
        //    parms[12] = new ReportParameter("Date", DateTime.Now.ToString());
        //    lr.SetParameters(parms);


        //    string ReType = "PDF";
        //    string mimeTyoe;
        //    string encoding;
        //    string fileNameExtension;

        //    string deviceInfo =
        //        "<DeviceInfo>" +
        //        "<OutputFormat>" + "PDF" + "</OutputFormat>" +
        //        "<PageWidth> 9in </PageWidth>" +
        //        "<PageHeight>11in</PageHeight>" +
        //        "<MarginTop>0.5in</MarginTop>" +
        //        "<MarginLeft>0.5in</MarginLeft>" +
        //        "<MarginRight>0.5in</MarginRight>" +
        //        "<MarginBottom>0.5in</MarginBottom>" +
        //        "</DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    renderedBytes = lr.Render(
        //        ReType,
        //        deviceInfo,
        //        out mimeTyoe,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings
        //        );



        //    var senderMail = new MailAddress("support@tempodigital.org", "Sender");
        //    var receiverMial = new MailAddress("munir.hadrovic@gmail.com", "Receiver");

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(senderMail.Address, ConfigurationManager.AppSettings["GmailPasword"])
        //    };

        //    MailMessage message = new MailMessage(senderMail, receiverMial);
        //    message.Subject = "Monthly Report - Self Billing";
        //    message.Body = "Hi,\nHere are the Reports for Month/Year" + monthMusic + "/" + yearMusic + ".\nPlease find Google Play Monthly Report and Self Billing in Attached Files.";
        //    message.Attachments.Add(new Attachment(new MemoryStream(renderedBytes), "Self-Billing.pdf"));
        //    memoryStreamG.Position = 0;
        //    message.Attachments.Add(new Attachment(memoryStreamG, "Google Play-Monthly-Report.pdf"));
        //    smtp.Send(message);
        //}

        //void Sound_Cloud_Ent_Monthly_Report_Self_Billing(string email, string yearMusic, string monthMusic)
        //{
        //    var user = db.Users.Where(a => a.Email.Equals(email)).FirstOrDefault();
        //    int share = Int32.Parse(user.share);

        //    double musicTotal = 0.0;

        //    /*Creating iTextSharp’s Document & Writer*/
        //    Document pdfDocS = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
        //    MemoryStream memoryStreamS = new MemoryStream();
        //    PdfWriter pdfWriterS = PdfWriter.GetInstance(pdfDocS, memoryStreamS);
        //    pdfDocS.Open();


        //    PdfPTable tableS = new PdfPTable(8);
        //    tableS.WidthPercentage = 100;
        //    tableS.HorizontalAlignment = 0;
        //    tableS.SpacingBefore = 20f;
        //    tableS.SpacingAfter = 30f;

        //    float[] columnWidthsS = new float[] { 50f, 50f, 100f, 100f, 250f, 110f, 90f, 80f };
        //    tableS.SetWidthPercentage(columnWidthsS, PageSize.A4.Rotate());

        //    tableS.AddCell("Month");
        //    tableS.AddCell("Year");
        //    tableS.AddCell("Label");
        //    tableS.AddCell("UPC");
        //    tableS.AddCell("Artist Name");
        //    tableS.AddCell("Album Name");
        //    tableS.AddCell("Revenue");
        //    tableS.AddCell("Share");

        //    var pdfImageS = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
        //    pdfImageS.Alignment = Element.ALIGN_MIDDLE;


        //    pdfDocS.Add(pdfImageS);
        //    var para12S = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Älgpasset 10 C 806 36 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
        //    para12S.Alignment = Element.ALIGN_MIDDLE;
        //    pdfDocS.Add(para12S);

        //    Chunk chunk4S = new Chunk("Official Payment Report - Sound Cloud Network:" + get_Month(monthMusic) + "-" + yearMusic);
        //    chunk4S.Font = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.BLACK);
        //    pdfDocS.Add(chunk4S);

        //    double revS = 0;
        //    var distributionsS = db.MusicReleases.Where(a => a.UserId.Equals(user.Id)).ToList();
        //    foreach (var dist in distributionsS)
        //    {
        //        var list = db.SoundCloud.Where(a => a.UPC.Equals(dist.UPCEAN)).ToList();

        //        if (list.Count > 0)
        //        {
        //            tableS.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
        //            tableS.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableS.AddCell(new Phrase(0, list[0].LabelName, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableS.AddCell(new Phrase(0, list[0].UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableS.AddCell(new Phrase(0, list[0].Artist_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableS.AddCell(new Phrase(0, list[0].Album_Title, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            tableS.AddCell(new Phrase(0, "$" + list.Sum(a => a.Total_Revenue).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            double asshare1 = (double)Math.Round(((list.Sum(a => a.Total_Revenue) / 100) * share), 2);
        //            tableS.AddCell(new Phrase(0, "$" + asshare1, FontFactory.GetFont(FontFactory.COURIER, 10)));

        //            revS = revS + (double)list.Sum(a => a.Total_Revenue);
        //            musicTotal = musicTotal + (double)list.Sum(a => a.Total_Revenue);
        //        }
        //    }

        //    tableS.AddCell("Total");
        //    tableS.AddCell("");
        //    tableS.AddCell("");
        //    tableS.AddCell("");
        //    tableS.AddCell("");
        //    tableS.AddCell("");
        //    tableS.AddCell("$" + revS.ToString());
        //    double asshare3S = (double)Math.Round(((revS / 100) * share), 2);
        //    tableS.AddCell("$" + asshare3S);
        //    pdfDocS.Add(tableS);


        //    foreach (var x in distributionsS)
        //    {
        //        var list = db.SoundCloud.Where(a => a.UPC.Equals(x.UPCEAN)).ToList();

        //        PdfPTable table1 = new PdfPTable(8);
        //        table1.WidthPercentage = 100;
        //        table1.HorizontalAlignment = 0;
        //        table1.SpacingBefore = 20f;
        //        table1.SpacingAfter = 30f;
        //        float[] columnWidths1 = new float[] { 40f, 50f, 50f, 100f, 100f, 300f, 100f, 90f };
        //        table1.SetWidthPercentage(columnWidths1, PageSize.A4.Rotate());

        //        if (list.Count > 0)
        //        {
        //            Chunk chunk1 = new Chunk("Relesse Name: " + x.ReleaseName);
        //            chunk1.Font = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);

        //            pdfDocS.Add(chunk1);

        //            table1.AddCell("Month");
        //            table1.AddCell("Year");
        //            table1.AddCell("Label");
        //            table1.AddCell("UPC");
        //            table1.AddCell("ISRC");
        //            table1.AddCell("Artist Name");
        //            table1.AddCell("Revenue");
        //            table1.AddCell("Share");

        //            foreach (var dist in list)
        //            {
        //                table1.AddCell(new Phrase(0, monthMusic, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
        //                table1.AddCell(new Phrase(0, yearMusic, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.LabelName, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.UPC, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.ISRC, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, dist.Artist_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                table1.AddCell(new Phrase(0, "$" + dist.Total_Revenue.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
        //                double asshare2 = (double)Math.Round((((dist.Total_Revenue) / 100) * share), 2);
        //                table1.AddCell(new Phrase(0, "$" + asshare2, FontFactory.GetFont(FontFactory.COURIER, 10)));
        //            }

        //            table1.AddCell("Total");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("");
        //            table1.AddCell("$" + list.Sum(a => a.Total_Revenue).ToString());
        //            double asshare5 = (double)Math.Round(((list.Sum(a => a.Total_Revenue / 100)) * share), 2);
        //            table1.AddCell("$" + asshare5);


        //            pdfDocS.Add(table1);

        //            Paragraph para = new Paragraph();
        //            para.Add("");
        //            para.SpacingBefore = 20f;
        //            para.SpacingAfter = 20f;
        //            pdfDocS.Add(para);
        //        }

        //    }
        //    pdfWriterS.CloseStream = false;
        //    pdfDocS.Close();

        //    musicTotal = (double)Math.Round(((musicTotal / 100) * share), 2);

        //    var paymentS = new MonthlyPayments();
        //    paymentS.User = user.Email;
        //    paymentS.MusicAmount = musicTotal.ToString();
        //    paymentS.EntAmount = "Sound Cloud Report";
        //    paymentS.Date_Time = DateTime.Now.ToString();
        //    paymentS.Music_Month = monthMusic;
        //    paymentS.Music_Year = yearMusic;

        //    db.MonthlyPayments.Add(paymentS);
        //    db.SaveChanges();
        //    var count = db.MonthlyPayments.Count();

        //    LocalReport lr = new LocalReport();
        //    string path = Path.Combine(Server.MapPath("~/PDF"), "Report5.rdlc");
        //    lr.ReportPath = path;

        //    ReportParameter[] parms = new ReportParameter[13];
        //    parms[0] = new ReportParameter("Name", user.FirstName + " " + user.LastName);
        //    parms[1] = new ReportParameter("Address", user.Email);
        //    parms[2] = new ReportParameter("InvoiceNumber", "0000" + count.ToString());
        //    parms[3] = new ReportParameter("VATNumber", "Not Applicable");
        //    parms[4] = new ReportParameter("Music", monthMusic + "/" + yearMusic + " Sound Cloud Network Report");
        //    parms[5] = new ReportParameter("Ent", " ---");
        //    parms[6] = new ReportParameter("MusicAmount", "$ " + musicTotal.ToString());
        //    parms[7] = new ReportParameter("EntAmount", "---");
        //    parms[8] = new ReportParameter("SubTotal", "$ " + (musicTotal).ToString());
        //    parms[9] = new ReportParameter("VATUS", "$ 0.00");
        //    parms[10] = new ReportParameter("TotalAmount", "$ " + (musicTotal).ToString());
        //    parms[11] = new ReportParameter("BalanceDue", "$ 0.00");
        //    parms[12] = new ReportParameter("Date", DateTime.Now.ToString());
        //    lr.SetParameters(parms);


        //    string ReType = "PDF";
        //    string mimeTyoe;
        //    string encoding;
        //    string fileNameExtension;

        //    string deviceInfo =
        //        "<DeviceInfo>" +
        //        "<OutputFormat>" + "PDF" + "</OutputFormat>" +
        //        "<PageWidth> 9in </PageWidth>" +
        //        "<PageHeight>11in</PageHeight>" +
        //        "<MarginTop>0.5in</MarginTop>" +
        //        "<MarginLeft>0.5in</MarginLeft>" +
        //        "<MarginRight>0.5in</MarginRight>" +
        //        "<MarginBottom>0.5in</MarginBottom>" +
        //        "</DeviceInfo>";

        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    renderedBytes = lr.Render(
        //        ReType,
        //        deviceInfo,
        //        out mimeTyoe,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings
        //        );



        //    var senderMail = new MailAddress("support@tempodigital.org", "Sender");
        //    var receiverMial = new MailAddress("munir.hadrovic@gmail.com", "Receiver");

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(senderMail.Address, ConfigurationManager.AppSettings["GmailPasword"])
        //    };

        //    MailMessage message = new MailMessage(senderMail, receiverMial);
        //    message.Subject = "Monthly Report - Self Billing";
        //    message.Body = "Hi,\nHere are the Reports for Month/Year" + monthMusic + "/" + yearMusic + ".\nPlease find Google Play Monthly Report and Self Billing in Attached Files.";
        //    message.Attachments.Add(new Attachment(new MemoryStream(renderedBytes), "Self-Billing.pdf"));
        //    memoryStreamS.Position = 0;
        //    message.Attachments.Add(new Attachment(memoryStreamS, "Google Play-Monthly-Report.pdf"));
        //    smtp.Send(message);
        //}

        string get_Month(string mon)
        {
            if (Int32.Parse(mon) == 1) { return "January"; }
            else if (Int32.Parse(mon) == 2) { return "February"; }
            else if (Int32.Parse(mon) == 3) { return "March"; }
            else if (Int32.Parse(mon) == 4) { return "April"; }
            else if (Int32.Parse(mon) == 5) { return "May"; }
            else if (Int32.Parse(mon) == 6) { return "June"; }
            else if (Int32.Parse(mon) == 7) { return "July"; }
            else if (Int32.Parse(mon) == 8) { return "August"; }
            else if (Int32.Parse(mon) == 9) { return "September"; }
            else if (Int32.Parse(mon) == 10) { return "October"; }
            else if (Int32.Parse(mon) == 11) { return "November"; }
            else if (Int32.Parse(mon) == 12) { return "December"; }

            return mon;
        }

        public JsonResult GetMusicChannelsList(string email)
        {
            var user = db.Users.Where(a => a.Email.Equals(email)).FirstOrDefault();
            var MuscList = db.YoutubeChannels.Where(a => a.UserId.Equals(user.Id)).ToList();
            return this.Json(MuscList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEntChannelsList(string email)
        {
            var user = db.Users.Where(a => a.Email.Equals(email)).FirstOrDefault();
            var MuscList = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(user.Id)).ToList();
            return this.Json(MuscList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserDetails(string email)
        {
            var user = db.Users.Where(a => a.Email.Equals(email)).FirstOrDefault();  
            return this.Json(user, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserFiles(string email)
        {
            var user = db.Users.Where(a => a.Email.Equals(email)).FirstOrDefault();
            var files = db.UserFiles.Where(a => a.UserID.Equals(user.Id)).ToList();
            return this.Json(files, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateUser(string Email)
        {
            var user = db.Users.Where(a => a.Email.Equals(Email)).FirstOrDefault();
            if(user != null)
            {
                user.BankName = Request.Form["BankName"];
                user.BankAddress = Request.Form["BankAddress"];
                user.BankCountry = Request.Form["BankCountry"];
                user.BankIBAN = Request.Form["BankIBAN"];
                user.CompanyName = Request.Form["CompanyName"];
                user.CompanyAddress = Request.Form["CompanyAddress"];
                user.Contact = Request.Form["ContactNumber"]; 
                user.Paypal = Request.Form["PaypalEmail"];
                user.share = Request.Form["UserShare"];
                db.SaveChanges();
            }
            
            return RedirectToAction("UserManagementAdmin");
        }

        public ActionResult UploadFile(HttpPostedFileBase file, string email)
        {
            var user = db.Users.Where(a => a.Email.Equals(email)).FirstOrDefault();
            if(user != null && file != null)
            {
                string storageConnection = CloudConfigurationManager.GetSetting("BlobStorageConnectionString");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

                //create a block blob 
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                //create a container 
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("userfiles");

                //create a container if it is not already exists
                cloudBlobContainer.CreateIfNotExists();
                cloudBlobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                var fileName = Regex.Replace(Request.Form["FileTitle"], @"\s+", "");

                var ImagePath = user.Email + "/" + fileName + ".pdf";

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(ImagePath);
                cloudBlockBlob.Properties.ContentType = file.ContentType;

                cloudBlockBlob.UploadFromStream(file.InputStream);

                var obj = new UserFiles();
                obj.UserID = user.Id;
                obj.FileName = Request.Form["FileTitle"];
                obj.DateTime = DateTime.Now.ToString();
                obj.FileLink = "https://tpdigital.blob.core.windows.net/userfiles/" + ImagePath;

                db.UserFiles.Add(obj);
                db.SaveChanges();
            }

            return RedirectToAction("UserManagementAdmin");
        }
        
        public ActionResult DeleteFile(string fileName, string email)
        {
            string storageConnection = CloudConfigurationManager.GetSetting("BlobStorageConnectionString");
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

            //create a block blob 
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            //create a container 
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("userfiles");

            //create a container if it is not already exists

            cloudBlobContainer.CreateIfNotExists();
            cloudBlobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            var file = Regex.Replace(fileName, @"\s+", "");
            var ImagePath = email + "/" + file + ".pdf";

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(ImagePath);
            cloudBlockBlob.DeleteIfExists();

            var user = db.Users.Where(a => a.Email.Equals(email)).FirstOrDefault();
            var obj = db.UserFiles.Where(a => a.UserID.Equals(user.Id)).FirstOrDefault();
            if (obj != null)
            {
                db.UserFiles.Remove(obj);
                db.SaveChanges();
            }

            return RedirectToAction("UserManagement");
        }

        public JsonResult GetDistSongs(string email)
        {
            var user_ID = db.Users.Where(a => a.Id.Equals(email)).Select(a=>a.Id).FirstOrDefault();
            var dis = db.MusicReleases.Where(a => a.UserId.Equals(user_ID)).Select(a=>a.MusicReleaseId).FirstOrDefault();
            //GetDistSongs(dis);
            var allData = db.Songs
                .Join(
                    db.MusicReleases.Where(a => a.MusicReleaseId.Equals(dis)),
                    d => d.MusicReleaseId,
                    f => f.MusicReleaseId,
                    (d, f) => d).ToList();
            var songs = db.Songs.Where(a => a.MusicReleaseId == dis).ToList();
            return this.Json(allData, JsonRequestBehavior.AllowGet);
        }

        void GetDistSongs(int id)
        {
            var songs = db.Songs.Where(a => a.MusicReleaseId == id).ToList();
            var sf = songs;
        }

        public JsonResult getStoreData(string store, string year, string month)
        {
            var releaselist = db.MusicReleases.Where(x => x.Status != "Deleted").ToList();
            if (store.Equals("Spotify"))
            {
                List<SpotifyReports_2020> finalList = new List<SpotifyReports_2020>();
                foreach (var y in releaselist)
                {
                    var spotifylist = db.SpotifyReports_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in spotifylist)
                    {
                        x.Composer_Name = y.ReleaseName;
                    }
                    finalList.AddRange(spotifylist);
                }

                var sortedlist = finalList.OrderByDescending(b => b.USD_Payable).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Quantity), revenue = sortedlist.Sum(a => a.USD_Payable) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Quantity), revenue = sortedlist.Sum(a => a.USD_Payable) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("Deezer"))
            {
                List<DeezerReport_2020> finalList = new List<DeezerReport_2020>();
                var list = db.DeezerReport_2020.Where(x => x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                foreach (var y in releaselist)
                {
                    var deezerlist = list.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in deezerlist)
                    {
                        x.Service = y.ReleaseName;
                    }
                    finalList.AddRange(deezerlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Royalties).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Nb_of_plays), revenue = sortedlist.Sum(a => a.Royalties) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Nb_of_plays), revenue = sortedlist.Sum(a => a.Royalties) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("GooglePlay"))
            {
                List<GoogleMusicReport_2020> finalList = new List<GoogleMusicReport_2020>();
                var list = db.GoogleMusicReport_2020.Where(x => x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                foreach (var y in releaselist)
                {
                    List<GoogleMusicReport_2020> Google_Playlist = list.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in Google_Playlist)
                    {
                        x.Interaction_Type = y.ReleaseName;
                    }
                    finalList.AddRange(Google_Playlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Partner_Revenue_Paid).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Count), revenue = sortedlist.Sum(a => a.Partner_Revenue_Paid) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Count), revenue = sortedlist.Sum(a => a.Partner_Revenue_Paid) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("SoundCloud"))
            {
                List<SoundCloudReport_2020> finalList = new List<SoundCloudReport_2020>();
                var list = db.SoundCloudReport_2020.Where(x => x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = list.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Usage_Type = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Total_Revenue).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Total_Plays), revenue = sortedlist.Sum(a => a.Total_Revenue) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Total_Plays), revenue = sortedlist.Sum(a => a.Total_Revenue) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            if (store.Equals("AppleMusic"))
            {
                List<Apple_Music_New> finalList = new List<Apple_Music_New>();
                var list = db.Apple_Music_New.Where(x => x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = list.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Configuration = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("Napster"))
            {
                List<Napster_Streaming> finalList = new List<Napster_Streaming>();
                var list = db.Napster_streaming.Where(x => x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = list.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Configuration = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("Tidal"))
            {
                List<Tidal> finalList = new List<Tidal>();
                var list = db.Tidal.Where(x => x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = list.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Configuration = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("Pandora"))
            {
                List<Pandora> finalList = new List<Pandora>();
                var list = db.Pandora.Where(x => x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = list.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Configuration = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }


            if (store.Equals("TikTok"))
            {

            }
            if (store.Equals("Facebook"))
            {

            }
            if (store.Equals("YouTubeMusic"))
            {

            }
            if (store.Equals("7Digital"))
            {

            }

            return this.Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStoreDataYearly(string store, string year)
        {
            var releaselist = db.MusicReleases.Where(x => x.Status != "Deleted").ToList();
            if (store.Equals("Spotify"))
            {
                List<SpotifyReports_2020> finalList = new List<SpotifyReports_2020>();
                foreach (var y in releaselist)
                {
                    var spotifylist = db.SpotifyReports_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year)).ToList();
                    foreach (var x in spotifylist)
                    {
                        x.Composer_Name = y.ReleaseName;
                    }
                    finalList.AddRange(spotifylist);
                }

                var sortedlist = finalList.OrderByDescending(b => b.USD_Payable).ToList();

                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Quantity), revenue = sortedlist.Sum(a => a.USD_Payable) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Quantity), revenue = sortedlist.Sum(a => a.USD_Payable) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            if (store.Equals("Deezer"))
            {
                List<DeezerReport_2020> finalList = new List<DeezerReport_2020>();
                foreach (var y in releaselist)
                {
                    var deezerlist = db.DeezerReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year)).ToList();
                    foreach (var x in deezerlist)
                    {
                        x.Service = y.ReleaseName;
                    }
                    finalList.AddRange(deezerlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Royalties).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Nb_of_plays), revenue = sortedlist.Sum(a => a.Royalties) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Nb_of_plays), revenue = sortedlist.Sum(a => a.Royalties) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("GooglePlay"))
            {
                List<GoogleMusicReport_2020> finalList = new List<GoogleMusicReport_2020>();
                foreach (var y in releaselist)
                {
                    List<GoogleMusicReport_2020> Google_Playlist = db.GoogleMusicReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year)).ToList();
                    foreach (var x in Google_Playlist)
                    {
                        x.Interaction_Type = y.ReleaseName;
                    }
                    finalList.AddRange(Google_Playlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Partner_Revenue_Paid).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Count), revenue = sortedlist.Sum(a => a.Partner_Revenue_Paid) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Count), revenue = sortedlist.Sum(a => a.Partner_Revenue_Paid) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("SoundCloud"))
            {
                List<SoundCloudReport_2020> finalList = new List<SoundCloudReport_2020>();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = db.SoundCloudReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Usage_Type = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Total_Revenue).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Total_Plays), revenue = sortedlist.Sum(a => a.Total_Revenue) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Total_Plays), revenue = sortedlist.Sum(a => a.Total_Revenue) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            if (store.Equals("AppleMusic"))
            {
                List<Apple_Music_New> finalList = new List<Apple_Music_New>();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = db.Apple_Music_New.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Configuration = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("Napster"))
            {
                List<Napster_Streaming> finalList = new List<Napster_Streaming>();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = db.Napster_streaming.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Configuration = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("Tidal"))
            {
                List<Tidal> finalList = new List<Tidal>();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = db.Tidal.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Configuration = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("Pandora"))
            {
                List<Pandora> finalList = new List<Pandora>();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = db.Pandora.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Configuration = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }


            if (store.Equals("TikTok"))
            {

            }
            if (store.Equals("Facebook"))
            {

            }
            if (store.Equals("YouTubeMusic"))
            {

            }
            if (store.Equals("7Digital"))
            {

            }

            return this.Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult getStoreDataLabel(string store, string year, string month, string label)
        {
            var releaselist = db.MusicReleases.Where(x => x.Status != "Deleted" && x.LabelName.Equals(label)).ToList();
            if (store.Equals("Spotify"))
            {
                List<SpotifyReports_2020> finalList = new List<SpotifyReports_2020>();
                foreach (var y in releaselist)
                {
                    var spotifylist = db.SpotifyReports_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in spotifylist)
                    {
                        x.Composer_Name = y.ReleaseName;
                    }
                    finalList.AddRange(spotifylist);
                }

                var sortedlist = finalList.OrderByDescending(b => b.USD_Payable).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Quantity), revenue = sortedlist.Sum(a => a.USD_Payable) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Quantity), revenue = sortedlist.Sum(a => a.USD_Payable) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("Deezer"))
            {
                List<DeezerReport_2020> finalList = new List<DeezerReport_2020>();
                foreach (var y in releaselist)
                {
                    var deezerlist = db.DeezerReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in deezerlist)
                    {
                        x.Service = y.ReleaseName;
                    }
                    finalList.AddRange(deezerlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Royalties).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Nb_of_plays), revenue = sortedlist.Sum(a => a.Royalties) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Nb_of_plays), revenue = sortedlist.Sum(a => a.Royalties) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("GooglePlay"))
            {
                List<GoogleMusicReport_2020> finalList = new List<GoogleMusicReport_2020>();
                foreach (var y in releaselist)
                {
                    List<GoogleMusicReport_2020> Google_Playlist = db.GoogleMusicReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in Google_Playlist)
                    {
                        x.Interaction_Type = y.ReleaseName;
                    }
                    finalList.AddRange(Google_Playlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Partner_Revenue_Paid).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Count), revenue = sortedlist.Sum(a => a.Partner_Revenue_Paid) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Count), revenue = sortedlist.Sum(a => a.Partner_Revenue_Paid) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("SoundCloud"))
            {
                List<SoundCloudReport_2020> finalList = new List<SoundCloudReport_2020>();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = db.SoundCloudReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Usage_Type = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Total_Revenue).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Total_Plays), revenue = sortedlist.Sum(a => a.Total_Revenue) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Total_Plays), revenue = sortedlist.Sum(a => a.Total_Revenue) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            if (store.Equals("AppleMusic"))
            {
                List<Apple_Music_New> finalList = new List<Apple_Music_New>();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = db.Apple_Music_New.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Configuration = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("Napster"))
            {
                List<Napster_Streaming> finalList = new List<Napster_Streaming>();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = db.Napster_streaming.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Configuration = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("Tidal"))
            {
                List<Tidal> finalList = new List<Tidal>();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = db.Tidal.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Configuration = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (store.Equals("Pandora"))
            {
                List<Pandora> finalList = new List<Pandora>();
                foreach (var y in releaselist)
                {
                    var SoundCloudlist = db.Pandora.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach (var x in SoundCloudlist)
                    {
                        x.Configuration = y.ReleaseName;
                    }
                    finalList.AddRange(SoundCloudlist);
                }
                var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                if (sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { list = sortedlist, views = sortedlist.Sum(a => a.Units), revenue = sortedlist.Sum(a => a.Net_Amount) };
                    return this.Json(result, JsonRequestBehavior.AllowGet);
                }
            }


            if (store.Equals("TikTok"))
            {

            }
            if (store.Equals("Facebook"))
            {

            }
            if (store.Equals("YouTubeMusic"))
            {

            }
            if (store.Equals("7Digital"))
            {

            }

            return this.Json("", JsonRequestBehavior.AllowGet);
        }
    }
} 