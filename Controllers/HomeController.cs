using Google.Apis.Auth.OAuth2;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TempoDigitalApex3.Models;
//using ASPSnippets.GoogleAPI;
using System.Web.Script.Serialization;

namespace TempoDigitalApex3.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        [Authorize]
        public ActionResult Analytics_Releases ()
        {
            //User Authentication  - get user
            var userId = User.Identity.GetUserId();
            var user = db.Users.Where(a => a.Id == userId).FirstOrDefault();
            ViewBag.Share = user.share;

            //get all releases from the user
            var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
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

        [Authorize]
        public ActionResult Spotify_Page(int? id, string year, string month, string full)
        {
            var userId = User.Identity.GetUserId();
            if (full != null)
            {   
                try
                {
                    var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
                    List<SpotifyReports_2020> finalList = new List<SpotifyReports_2020>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.SpotifyReports_2020.Where(x => x.Year.Equals(year) && x.Month.Equals(month) && x.UPC.Equals(y.UPCEAN)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.USD_Payable).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }
            else
            {
                try
                { 
                    var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted" && x.MusicReleaseId == id).ToList();

                    List<SpotifyReports_2020> finalList = new List<SpotifyReports_2020>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.SpotifyReports_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.USD_Payable).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }
           

            var releaselist1 = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
            ViewBag.Releases = releaselist1;
            return View();
        }

        [Authorize]
        public ActionResult Deezer_Page(int? id, string year, string month, string full)
        {
            var userId = User.Identity.GetUserId();

            if(full != null)
            {
                try
                {
                    var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();

                    List<DeezerReport_2020> finalList = new List<DeezerReport_2020>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.DeezerReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.Royalties).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }
            else
            {
                try
                {
                    var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted" && x.MusicReleaseId == id).ToList();

                    List<DeezerReport_2020> finalList = new List<DeezerReport_2020>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.DeezerReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.Royalties).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }

            


            var releaselist1 = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
            ViewBag.Releases = releaselist1;
            return View();
        }

        [Authorize]
        public ActionResult Apple_Page(int? id, string year, string month, string full)
        {
            var userId = User.Identity.GetUserId();

            if (full != null)
            {
                var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
                try
                {
                    List<Apple_Music_New> finalList = new List<Apple_Music_New>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.Apple_Music_New.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }
            else
            {
                var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted" && x.MusicReleaseId == id).ToList();
                try
                {
                    List<Apple_Music_New> finalList = new List<Apple_Music_New>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.Apple_Music_New.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }

            


            var releaselist1 = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
            ViewBag.Releases = releaselist1;
            return View();
        }

        [Authorize]
        public ActionResult Napster_Page(int? id, string year, string month, string full)
        {
            var userId = User.Identity.GetUserId();

            if (full != null)
            {
                var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
                try
                {
                    List<Napster_Streaming> finalList = new List<Napster_Streaming>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.Napster_streaming.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }
            else
            {
                var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted" && x.MusicReleaseId == id).ToList();
                try
                {
                    List<Napster_Streaming> finalList = new List<Napster_Streaming>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.Napster_streaming.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }

            


            var releaselist1 = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
            ViewBag.Releases = releaselist1;
            return View();
        }

        [Authorize]
        public ActionResult Tidal_Page(int? id, string year, string month, string full)
        {
            var userId = User.Identity.GetUserId();
            if (full != null)
            {
                var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
                try
                {
                    List<Tidal> finalList = new List<Tidal>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.Tidal.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }
            else
            {
                var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted" && x.MusicReleaseId == id).ToList();
                try
                {
                    List<Tidal> finalList = new List<Tidal>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.Tidal.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }
            


            var releaselist1 = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
            ViewBag.Releases = releaselist1;
            return View();
        }

        [Authorize]
        public ActionResult Pandora_Page(int? id, string year, string month, string full)
        {
            var userId = User.Identity.GetUserId();
            if (full != null)
            {
                var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
                try
                {
                    List<Pandora> finalList = new List<Pandora>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.Pandora.Where(x => x.Barcode.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }
            else
            {
                var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted" && x.MusicReleaseId == id).ToList();
                try
                {
                    List<Pandora> finalList = new List<Pandora>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.Pandora.Where(x => x.Barcode.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.Net_Amount).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }
            

            var releaselist1 = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
            ViewBag.Releases = releaselist1;
            return View();
        }

        [Authorize]
        public ActionResult GoogleMusic_Page(int? id, string year, string month, string full)
        {
            var userId = User.Identity.GetUserId();
            if (full != null)
            {
                var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
                try
                {
                    List<GoogleMusicReport_2020> finalList = new List<GoogleMusicReport_2020>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.GoogleMusicReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.EUR_Amount).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }
            else
            {
                var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted" && x.MusicReleaseId == id).ToList();
                try
                {
                    List<GoogleMusicReport_2020> finalList = new List<GoogleMusicReport_2020>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.GoogleMusicReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.EUR_Amount).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }
            


            var releaselist1 = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
            ViewBag.Releases = releaselist1;
            return View();
        }

        [Authorize]
        public ActionResult SoundCloud_Page(int? id, string year, string month, string full)
        {
            var userId = User.Identity.GetUserId();
            if (full != null)
            {
                var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
                try
                {
                    List<SoundCloudReport_2020> finalList = new List<SoundCloudReport_2020>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.SoundCloudReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.Total_Revenue).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }
            else
            {
                var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted" && x.MusicReleaseId == id).ToList();
                try
                {
                    List<SoundCloudReport_2020> finalList = new List<SoundCloudReport_2020>();
                    foreach (var y in releaselist)
                    {
                        var spotifylist = db.SoundCloudReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                        finalList.AddRange(spotifylist);
                    }
                    var sortedlist = finalList.OrderByDescending(b => b.Total_Revenue).ToList();
                    ViewBag.Spotify = sortedlist;
                    ViewBag.Name = releaselist[0].ReleaseName;
                    ViewBag.Count = sortedlist.Count;
                    ViewBag.Month = month;
                    ViewBag.Year = year;
                }
                catch
                {
                    ViewBag.Spotify = null;
                    ViewBag.Name = null;
                    ViewBag.Month = null;
                    ViewBag.Year = null;
                }
            }
            


            var releaselist1 = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();
            ViewBag.Releases = releaselist1;
            return View();
        }

        [Authorize]
        public ActionResult TikTok_Page(int? id, string year, string month)
        {
            return View();
        }

        [Authorize]
        public ActionResult Facebook_Page(int? id, string year, string month)
        {
            return View();
        }

        [Authorize]
        public ActionResult YouTubeMusic_Page(int? id, string year, string month)
        {
            return View();
        }

        [Authorize]
        public ActionResult SevenDigital_Page(int? id, string year, string month)
        {
            return View();
        }

        [Authorize]
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            var channel = db.YoutubeChannels.Where(a => a.UserId.Equals(user) && a.YT_Label_Name == null).ToList();
            ViewBag.Channels = channel.Select(a => a.ChannelID);
            ViewBag.ChannelsName = channel.Select(a => a.ChannelName);
            ViewBag.ChannelCount = channel.Count;

            var channelEnt = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(user) && a.Approved == true && a.YT_Label_Name == null).ToList();
            ViewBag.ChannelsEnt = channelEnt.Select(a => a.ChannelID);
            ViewBag.ChannelsNameEnt = channelEnt.Select(a => a.ChannelName);
            ViewBag.ChannelCountEnt = channelEnt.Count;


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

        public ActionResult FAQ()
        {
            return View();
        }
        public ActionResult HelpDistribution()
        {
            return View();
        }
        //public ActionResult Index1()
        //{
        //    return View();
        //}

        //public ActionResult MyOwnership()
        //{
        //    ViewBag.YoutubeEarningLastMonth = Math.Round(getYoutubeData(), 2);
        //    ViewBag.YoutubeEntEarningLastMonth = Math.Round(getYoutubeEntData(), 2);
        //    return View();
        //}

        public ActionResult Analytics()
        {
            var userId = User.Identity.GetUserId();
            var releaselist = db.MusicReleases.Where(x => x.UserId.Equals(userId) && x.Status != "Deleted").ToList();

            int month = Int32.Parse(DateTime.Now.Month.ToString());
            int year = Int32.Parse(DateTime.Now.Year.ToString());

            if(month == 1)
            {
                month = 12;
                year -= 1;
            }
            else
            {
                month -= 1;
            }


            ViewBag.SoundCloudEarningLastMonth = Math.Round(getSoundCloudData(releaselist, ref year, ref month), 2);
            ViewBag.SpotifyEarningLastMonth = Math.Round(getSpotifyData(releaselist, ref year, ref month), 2);
            ViewBag.DeezersEarningLastMonth = Math.Round(getDeezersData(releaselist, ref year, ref month), 2);
            ViewBag.GooglePlayEarningLastMonth = Math.Round(getGooglePlayData(releaselist, ref year, ref month), 2);

            // new stores
            ViewBag.AppleEarningLastMonth = Math.Round(getAppleData(releaselist, ref year, ref month), 2);
            ViewBag.TidalEarningLastMonth = Math.Round(getTidalData(releaselist, ref year, ref month), 2);
            ViewBag.PandoraEarningLastMonth = Math.Round(getPandoraData(releaselist, ref year, ref month), 2);
            ViewBag.NapsterEarningLastMonth = Math.Round(getNapsterData(releaselist, ref year, ref month), 2);

            //ViewBag.SoundCloudEarningLastMonth = 0;
            //ViewBag.SpotifyEarningLastMonth = 0;
            //ViewBag.DeezersEarningLastMonth = 0;
            //ViewBag.GooglePlayEarningLastMonth = 0;

            int totalSongs = 0;
            var labels = new List<string>();
            ViewBag.TotalReleases = totalReleases(releaselist, ref totalSongs, ref labels);
            ViewBag.TotalSongs = totalSongs;
            ViewBag.Labels = labels;
            return View();
        }

        int totalReleases(List<MusicRelease> releaselist, ref int totalSongs, ref List<string> labels)
        {
            var all_songs = db.Songs.ToList();
            foreach (var x in releaselist)
            {
                var songs = all_songs.Where(a => a.MusicReleaseId.Equals(x.MusicReleaseId)).ToList();
                totalSongs = totalSongs + songs.Count;

                if (!labels.Contains(x.LabelName))
                {
                    labels.Add(x.LabelName);
                }
            }

            return releaselist.Count;
        }
        double getSpotifyData(List<MusicRelease> releaselist, ref int year, ref int month)
        {
            double earning = 0;

            List<SpotifyReportFull> finalList = new List<SpotifyReportFull>();
            var yr = year.ToString();
            var m = month.ToString();

            var all_spotify = db.SpotifyReportFull.Where(a => a.Month == m && a.Year == yr).ToList();
            foreach (var y in releaselist)
            {
                var spotifylist = all_spotify.Where(x => x.UPC.Equals(y.UPCEAN)).ToList();
                finalList.AddRange(spotifylist);
            }

            earning = (double)finalList.Sum(a=>a.USD_Payable);
        
            return earning;
        }
        double getDeezersData(List<MusicRelease> releaselist, ref int year, ref int month)
        {
            double earning = 0;
            var yr = year.ToString();
            var m = month.ToString();

            List<Deezer> finalList = new List<Deezer>();
            var all_dezzers = db.Deezers.Where(a => a.Month == m && a.Year == yr).ToList();
            foreach (var y in releaselist)
            {
                var deezerlist = all_dezzers.Where(x => x.UPC.Equals(y.UPCEAN)).ToList();
                finalList.AddRange(deezerlist);
            }


            earning = (double)finalList.Sum(a => a.Royalties);

            return earning;
        }
        double getGooglePlayData(List<MusicRelease> releaselist, ref int year, ref int month)
        {
            double earning = 0;
            var yr = year.ToString();
            var m = month.ToString();

            List<Google_Play> finalList = new List<Google_Play>();
            var all_google_play = db.Google_Play.Where(a => a.Month == m && a.Year == yr).ToList();
            foreach (var y in releaselist)
            {
                List<Google_Play> Google_Playlist = all_google_play.Where(x => x.UPC.Equals(y.UPCEAN)).ToList();
                finalList.AddRange(Google_Playlist);
            }
            earning = (double)finalList.Sum(a => a.EUR_Amount);

            return earning;
        }
        double getSoundCloudData(List<MusicRelease> releaselist, ref int year, ref int month)
        {
            double earning = 0;
            var yr = year.ToString();
            var m = month.ToString();

            List<SoundCloud> finalList = new List<SoundCloud>();
            var all_sound_cloud = db.SoundCloud.Where(a=>a.Month == m && a.Year == yr).ToList();
            
            foreach (var y in releaselist)
            {
                var SoundCloudlist = all_sound_cloud.Where(x => x.UPC.Equals(y.UPCEAN)).ToList();
                finalList.AddRange(SoundCloudlist);
            }

            earning = (double)finalList.Sum(a => a.Total_Revenue);


            //foreach (var x in finalList)
            //{
            //    if (Int32.Parse(x.Year) > year)
            //        year = Int32.Parse(x.Year);
            //}

            //foreach (var x in finalList)
            //{
            //    if (Int32.Parse(x.Year) == year)
            //    {
            //        if (Int32.Parse(x.Month) > month)
            //            month = Int32.Parse(x.Month);
            //    }
            //}

            //foreach (var x in finalList)
            //{
            //    if (Int32.Parse(x.Month) == month && Int32.Parse(x.Year) == year)
            //        earning = earning + (double)x.Total_Revenue;
            //}

            return earning;
        }

        // new stores
        double getAppleData(List<MusicRelease> releaselist, ref int year, ref int month)
        {
            double earning = 0;

            List<Apple_Music> finalList = new List<Apple_Music>();
            var yr = year.ToString();
            var m = month.ToString();

            var all_spotify = db.Apple_Music.Where(a => a.Month == m && a.Year == yr).ToList();
            foreach (var y in releaselist)
            {
                var spotifylist = all_spotify.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId)).ToList();
                finalList.AddRange(spotifylist);
            }

            earning = (double)finalList.Sum(a => a.Net_Amount);

            return earning;
        }
        double getTidalData(List<MusicRelease> releaselist, ref int year, ref int month)
        {
            double earning = 0;

            List<Tidal> finalList = new List<Tidal>();
            var yr = year.ToString();
            var m = month.ToString();

            var all_spotify = db.Tidal.Where(a => a.Month == m && a.Year == yr).ToList();
            foreach (var y in releaselist)
            {
                var spotifylist = all_spotify.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId)).ToList();
                finalList.AddRange(spotifylist);
            }

            earning = (double)finalList.Sum(a => a.Net_Amount);

            return earning;
        }
        double getPandoraData(List<MusicRelease> releaselist, ref int year, ref int month)
        {
            double earning = 0;

            List<Pandora> finalList = new List<Pandora>();
            var yr = year.ToString();
            var m = month.ToString();

            var all_spotify = db.Pandora.Where(a => a.Month == m && a.Year == yr).ToList();
            foreach (var y in releaselist)
            {
                var spotifylist = all_spotify.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId)).ToList();
                finalList.AddRange(spotifylist);
            }

            earning = (double)finalList.Sum(a => a.Net_Amount);

            return earning;
        }
        double getNapsterData(List<MusicRelease> releaselist, ref int year, ref int month)
        {
            double earning = 0;

            List<Napster_Streaming> finalList = new List<Napster_Streaming>();
            var yr = year.ToString();
            var m = month.ToString();

            var all_spotify = db.Napster_streaming.Where(a => a.Month == m && a.Year == yr).ToList();
            foreach (var y in releaselist)
            {
                var spotifylist = all_spotify.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId)).ToList();
                finalList.AddRange(spotifylist);
            }

            earning = (double)finalList.Sum(a => a.Net_Amount);

            return earning;
        }


        double getYoutubeData()
        {
            double earning = 0;

            var userId = User.Identity.GetUserId();
            var releaselist = db.YoutubeChannels.Where(x => x.UserId.Equals(userId) && x.Approved == true).ToList();

            List<YT_Reports_Music_Full> finalList = new List<YT_Reports_Music_Full>();

            foreach (var y in releaselist)
            {
                var Youtubelist = db.YT_Reports_Music_Full.Where(x => x.Asset_Channel_ID.Equals(y.ChannelID)).ToList();
                finalList.AddRange(Youtubelist);
            }

            int month = 0, year = 0;

            foreach (var x in finalList)
            {
                if (Int32.Parse(x.Year) > year)
                    year = Int32.Parse(x.Year);
            }

            foreach (var x in finalList)
            {
                if (Int32.Parse(x.Year) == year)
                {
                    if (Int32.Parse(x.Month) > month)
                        month = Int32.Parse(x.Month);
                }
            }

            foreach (var x in finalList)
            {
                if (Int32.Parse(x.Month) == month && Int32.Parse(x.Year) == year)
                    earning = earning + (double)x.Partner_Revenue;
            }

            return earning;
        }
        double getYoutubeEntData()
        {
            double earning = 0;

            var userId = User.Identity.GetUserId();
            var releaselist = db.GetYoutubeChannelsENT.Where(x => x.UserId.Equals(userId) && x.Approved == true).ToList();

            List<YT_Reports_Ent> finalList = new List<YT_Reports_Ent>();

            foreach (var y in releaselist)
            {
                var Youtubelist = db.yT_Reports_Ents.Where(x => x.Asset_Channel_ID.Equals(y.ChannelID)).ToList();
                finalList.AddRange(Youtubelist);
            }

            int month = 0, year = 0;

            foreach (var x in finalList)
            {
                if (Int32.Parse(x.Year) > year)
                    year = Int32.Parse(x.Year);
            }

            foreach (var x in finalList)
            {
                if (Int32.Parse(x.Year) == year)
                {
                    if (Int32.Parse(x.Month) > month)
                        month = Int32.Parse(x.Month);
                }
            }

            foreach (var x in finalList)
            {
                if (Int32.Parse(x.Month) == month && Int32.Parse(x.Year) == year)
                    earning = earning + (double)x.Partner_Revenue;
            }

            return earning;
        }

        [AllowAnonymous]
        public ActionResult Privacy_Policy()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Terms_Conditions()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Distributions_Terms_Conditions()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Youtube_Terms_Conditions()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Both_Terms_Conditions()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public JsonResult NewChart()
        {
            List<string> years = new List<string>();
            List<string> months = new List<string>();

            var userId = User.Identity.GetUserId();

            var finalList = db.YT_Reports_Music_Full
                .Join(
                db.YoutubeChannels.Where(a => a.UserId.Equals(userId) && a.Approved == true),
                d => d.Asset_Channel_ID,
                f => f.ChannelID,
                (d, f) => d).ToList();

            var finalList1 = db.yT_Reports_Ents
                .Join(
                db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(userId) && a.Approved == true),
                d => d.Asset_Channel_ID,
                f => f.ChannelID,
                (d, f) => d).ToList();

            //var youtubeList = db.YoutubeChannels.Where(a => a.UserId.Equals(userId) && a.Approved == true).ToList();

            //List<YT_Reports_Music_Full> finalList = new List<YT_Reports_Music_Full>();
            //foreach (var x in youtubeList)
            //{
            //    var chList = db.YT_Reports_Music_Full.Where(a => a.Asset_Channel_ID.Equals(x.ChannelID)).ToList();
            //    finalList.AddRange(chList);
            //}

            if (finalList.Count > 0)
            {
                string month = finalList[0].Month;
                if (!months.Contains(month))
                    months.Add(month);
                foreach (var x in finalList)
                {
                    if (!x.Month.Equals(month))
                    {
                        if (!months.Contains(x.Month))
                            months.Add(x.Month);
                        month = x.Month;
                    }
                }

                string year = finalList[0].Year;
                if (!years.Contains(year))
                    years.Add(year);
                foreach (var x in finalList)
                {
                    if (!x.Year.Equals(year))
                    {
                        if (!years.Contains(x.Year))
                            years.Add(x.Year);
                        year = x.Year;
                    }
                }
            }

            if (finalList1.Count > 0)
            {
                string month = finalList1[0].Month;
                if (!months.Contains(month))
                    months.Add(month);
                foreach (var x in finalList1)
                {
                    if (!x.Month.Equals(month))
                    {
                        if (!months.Contains(x.Month))
                            months.Add(x.Month);
                        month = x.Month;
                    }
                }

                string year = finalList1[0].Year;
                if (!years.Contains(year))
                    years.Add(year);
                foreach (var x in finalList1)
                {
                    if (!x.Year.Equals(year))
                    {
                        if (!years.Contains(x.Year))
                            years.Add(x.Year);
                        year = x.Year;
                    }
                }
            }

            List<object> iData = new List<object>();
            //Creating sample data  
            DataTable dt = new DataTable();
            dt.Columns.Add("Year/Month", System.Type.GetType("System.String"));
            dt.Columns.Add("RevenueYoutube", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("RevenueEnt", System.Type.GetType("System.Decimal"));

            years = years.OrderBy(a => Int32.Parse(a)).ToList();
            months = months.OrderBy(a => Int32.Parse(a)).ToList();

            foreach (var y in years)
            {
                foreach (var m in months)
                {
                    var revenueList = finalList.Where(a => a.Month.Equals(m) && a.Year.Equals(y)).ToList();
                    var revenueList1 = finalList1.Where(a => a.Month.Equals(m) && a.Year.Equals(y)).ToList();

                    DataRow dr = dt.NewRow();
                    dr["Year/Month"] = y + " / " + m;
                    dr["RevenueYoutube"] = Math.Round(revenueList.Sum(a => a.Partner_Revenue), 1);
                    dr["RevenueEnt"] = Math.Round(revenueList1.Sum(a => a.Partner_Revenue), 1);
                    dt.Rows.Add(dr);
                    
                }
            }

            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult NewChart1()
        {
            var userId = User.Identity.GetUserId();

            var releaselist = db.MusicReleases.Where(x => x.UserId == userId && x.Status != "Deleted").ToList();

            List<SpotifyReportFull> finalList = new List<SpotifyReportFull>();
            var all_spotify = db.SpotifyReportFull.ToList();

            List<Deezer> finalList1 = new List<Deezer>();
            var all_deezers = db.Deezers.ToList();

            List<Google_Play> finalList2 = new List<Google_Play>();
            var all_googleplay = db.Google_Play.ToList();

            List<SoundCloud> finalList3 = new List<SoundCloud>();
            var all_sound_cloud = db.SoundCloud.ToList();


            foreach (var y in releaselist)
            {
                var spotifylist = all_spotify.Where(x => x.UPC.Equals(y.UPCEAN)).ToList();
                finalList.AddRange(spotifylist);

                var deezerlist = all_deezers.Where(x => x.UPC.Equals(y.UPCEAN)).ToList();
                finalList1.AddRange(deezerlist);

                var Google_Playlist = all_googleplay.Where(x => x.UPC.Equals(y.UPCEAN)).ToList();
                finalList2.AddRange(Google_Playlist);

                var SoundCloudlist = all_sound_cloud.Where(x => x.UPC.Equals(y.UPCEAN)).ToList();
                finalList3.AddRange(SoundCloudlist);
            }

           


            //List<string> YM = new List<string>();
            //foreach (var x in finalList)
            //{
            //    if (x.Year != null && x.Month != null)
            //    {
            //        if (!YM.Contains(x.Year + "%" + x.Month))
            //        {
            //            YM.Add(x.Year + "%" + x.Month);
            //        }
            //    }
            //}

            List<string> years = new List<string>();
            List<string> months = new List<string>();

            if (finalList.Count > 0)
            {
                string month = finalList[0].Month;
                months.Add(month);
                foreach (var x in finalList)
                {
                    if (!x.Month.Equals(month))
                    {
                        if (!months.Contains(x.Month))
                            months.Add(x.Month);
                        month = x.Month;
                    }
                }

                string year = finalList[0].Year;
                years.Add(year);
                foreach (var x in finalList)
                {
                    if (!x.Year.Equals(year))
                    {
                        if (!years.Contains(x.Year))
                            years.Add(x.Year);
                        year = x.Year;
                    }
                }
            }


            List<int> ys = new List<int>();
            List<int> ms = new List<int>();

            foreach(var x in months)
            {
                ms.Add(Int32.Parse(x));
            }

            foreach (var x in years)
            {
                ys.Add(Int32.Parse(x));
            }

            var Y1 = ys.OrderBy(x => x).ToArray();
            var M1 = ms.OrderBy(x => x).ToArray();

            List<string> Y = new List<string>();
            List<string> M = new List<string>();

            foreach (var x in M1)
            {
                M.Add(x.ToString());
            }

            foreach (var x in Y1)
            {
                Y.Add(x.ToString());
            }


            List<object> iData = new List<object>();
            //Creating sample data  
            DataTable dt = new DataTable();
            dt.Columns.Add("Year/Month", System.Type.GetType("System.String"));
            dt.Columns.Add("RevenueSpotify", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("RevenueDeezers", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("RevenueGooglePlay", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("RevenueSoundCloud", System.Type.GetType("System.Decimal"));


            foreach (var y in Y)
            {
                foreach (var m in M)
                {
                    var revenueList = finalList.Where(a => a.Month.Equals(m) && a.Year.Equals(y)).ToList();
                    var revenueList1 = finalList1.Where(a => a.Month.Equals(m) && a.Year.Equals(y)).ToList();
                    var revenueList2 = finalList2.Where(a => a.Month.Equals(m) && a.Year.Equals(y)).ToList();
                    var revenueList3 = finalList3.Where(a => a.Month.Equals(m) && a.Year.Equals(y)).ToList();

                    DataRow dr = dt.NewRow();
                    dr["Year/Month"] = y + " / " + m;
                    dr["RevenueSpotify"] = Math.Round(revenueList.Sum(a => a.USD_Payable), 1);
                    dr["RevenueDeezers"] = Math.Round(revenueList1.Sum(a => a.Royalties), 1);
                    dr["RevenueGooglePlay"] = Math.Round(revenueList2.Sum(a => a.EUR_Amount), 1);
                    dr["RevenueSoundCloud"] = Math.Round(revenueList3.Sum(a => a.Total_Revenue), 1);
                    dt.Rows.Add(dr);
                }
            }

            //foreach (var ym in YM)
            //{
            //    var revenueList = finalList.Where(a => a.Month.Equals(ym.Split('%')[1]) && a.Year.Equals(ym.Split('%')[0])).ToList();
            //    var revenueList1 = finalList1.Where(a => a.Month.Equals(ym.Split('%')[1]) && a.Year.Equals(ym.Split('%')[0])).ToList();
            //    var revenueList2 = finalList2.Where(a => a.Month.Equals(ym.Split('%')[1]) && a.Year.Equals(ym.Split('%')[0])).ToList();
            //    var revenueList3 = finalList3.Where(a=> a.Month.Equals(ym.Split('%')[1]) && a.Year.Equals(ym.Split('%')[0])).ToList();

            //    var sf = revenueList.Sum(a => a.USD_Payable);
            //    DataRow dr = dt.NewRow();
            //    dr["Year/Month"] = ym.Split('%')[0] + " / " + ym.Split('%')[1];
            //    dr["RevenueSpotify"] = Math.Round(revenueList.Sum(a => a.USD_Payable), 1);
            //    dr["RevenueDeezers"] = Math.Round(revenueList1.Sum(a => a.Royalties), 1);
            //    dr["RevenueGooglePlay"] = Math.Round(revenueList2.Sum(a => a.EUR_Amount), 1);
            //    dr["RevenueSoundCloud"] = Math.Round(revenueList3.Sum(a => a.Total_Revenue), 1);
            //    dt.Rows.Add(dr);

            //    var sd = Math.Round(revenueList.Sum(a => a.USD_Payable), 1);
            //    sd = Math.Round(revenueList1.Sum(a => a.Royalties), 1);
            //    sd = Math.Round(revenueList2.Sum(a => a.EUR_Amount), 1);
            //    sd = Math.Round(revenueList3.Sum(a => a.Total_Revenue), 1);
            //}

            //Looping and extracting each DataColumn to List<Object>  
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }
            //Source data returned as JSON  
            return Json(iData, JsonRequestBehavior.AllowGet);

        }


        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();

            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/Home/PaymentWithPayPal?";

                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {

                    // This function exectues after receving all parameters for the payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }

            int id = Int32.Parse(TempData["ReleaseID"].ToString());
            var release = db.MusicReleases.Where(a => a.MusicReleaseId == id).FirstOrDefault();
            release.PaymentStaus = "Paid";
            db.SaveChanges();

            TempData["ReleaseID"] = null;
            TempData["ChargePrice"] = null;

            // email sending
            string userId = User.Identity.GetUserId();
            var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
            var senderMail = new MailAddress("support@tempodigital.org", "Tempo Digital Team");
            //var receiverMial = new MailAddress("munir.hadrovic@gmail.com", "Receiver");
            var receiverMial = new MailAddress(user.Email, "Receiver");

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
            message.Subject = "Your delivery order TP Digital AB has completed.";
            message.Body = "Hi,\n The release successfully delivered in this order are now visible in the View Discograpgy section of distribution dashboard.\n\nFor any further queries please email support@tempodigital.org\n\nThank you.\n\nSender,\nTempo Digital Team.";
            
            smtp.Send(message);
            //on successful payment, show success page to user.
            return RedirectToAction("Index", "MusicReleases", new { id = 0 });
            //on successful payment, show success page to user.
            //return View("SuccessView");

            //on successful payment, show success page to user.
            //return View("SuccessView");
        }

        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        public Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            //Adding Item Details like name, currency, price etc
            itemList.items.Add(new Item()
            {
                name = "Distribution Price",
                currency = "USD",
                price = TempData["ChargePrice"].ToString(),
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };

            // Adding Tax, shipping and Subtotal details
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "4",
                shipping_discount = "-1"
            };

            //Final amount with details
            var amount = new Amount()
            {
                currency = "USD",
                total = TempData["ChargePrice"].ToString() // Total must be equal to sum of tax, shipping and subtotal.
                //details = details
            };

            var transactionList = new List<Transaction>();
            // Adding description about the transaction
            transactionList.Add(new Transaction()
            {
                description = "Tempo Digital Muisc Release Payment",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = itemList
            });


            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

    }


}