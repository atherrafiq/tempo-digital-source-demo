using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TempoDigitalApex3.Models;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net;

namespace TempoDigitalApex3.Controllers
{
    public struct MY
    {
        public List<string> month;
        public List<string> year;
    }
    [Authorize]
    public class YoutubeAnalyticsController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: YoutubeAnalytics
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
                ViewBag.Share = user.share;
                var Channels = db.YoutubeChannels.Where(a => a.UserId.Equals(userId) && a.Approved == true).ToList();
                ViewBag.ChannelCount = Channels.Count;

                return View();
            }

            else 
            {
                return RedirectToAction("Index","Home");
            }
        
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

        //public ActionResult Test()
        //{
        //    string[] scopes1 = { "https://www.googleapis.com/auth/youtubepartner",
        //    "https://www.googleapis.com/auth/youtube",
        //    "https://www.googleapis.com/auth/youtube.readonly",
        //    "https://www.googleapis.com/auth/youtubepartner",
        //    "https://www.googleapis.com/auth/yt-analytics-monetary.readonly",
        //    "https://www.googleapis.com/auth/yt-analytics.readonly" };



        //    //string[] scopes1 = { "https://www.googleapis.com/auth/youtubepartner",
        //    //"https://www.googleapis.com/auth/youtube",
        //    //"https://www.googleapis.com/auth/youtube.readonly",
        //    //"https://www.googleapis.com/auth/youtubepartner",
        //    //"https://www.googleapis.com/auth/yt-analytics-monetary.readonly",
        //    //"https://www.googleapis.com/auth/yt-analytics.readonly" };
        //    //string path = Server.MapPath("~/assets/contentid-api-internal-e65f4f6da0f9.json");
        //    //using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        //    //{
        //    //    var cred = GoogleCredential
        //    //        .FromStream(stream) // Loads key file  
        //    //        .CreateScoped(scopes1) // Gathers scopes requested  
        //    //        .UnderlyingCredential // Gets the credentials  
        //    //        .GetAccessTokenForRequestAsync(); // Gets the Access Token  

        //    //    ViewBag.Token = cred.Result;
        //    //}

        //    var user = User.Identity.GetUserId();
        //    var channel = db.YoutubeChannels.Where(a => a.UserId.Equals(user) && a.YT_Label_Name == null && a.Approved == true).ToList();
        //    ViewBag.Channels = channel.Select(a => a.ChannelID);
        //    ViewBag.ChannelsName = channel.Select(a => a.ChannelName);

        //    return View();
        //}


        public ActionResult YoutubeAnalytics()
        {
            var user = User.Identity.GetUserId();
            var channel = db.YoutubeChannels.Where(a => a.UserId.Equals(user) && a.YT_Label_Name == null && a.Approved == true).ToList();
            ViewBag.Channels = channel.Select(a => a.ChannelID);
            ViewBag.ChannelsName = channel.Select(a => a.ChannelName);
            ViewBag.ChannelCount = channel.Count;

            //ViewBag.Token = GetAccessToken();
            ViewBag.Token = GetAccessTokenHerokuAPI();

            return View();
        }

        public ActionResult AnalyticsChannelDetails(string channelID, string name)
        {
            if (channelID == null)
                return RedirectToAction("YoutubeAnalytics");

            ViewBag.ChannelId = channelID;
            ViewBag.Name = name;

            ViewBag.Key = ConfigurationManager.AppSettings["APIKEY"];
            //ViewBag.Token = GetAccessToken();
            ViewBag.Token = GetAccessTokenHerokuAPI();

            return View();
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

        [HttpGet]
        public ActionResult HTMLReportAllCH(string M, string Y)
        {
            if (M == null || Y == null)
                return RedirectToAction("Index");

            List<List<YT_Reports_Music_2020>> finalList = new List<List<YT_Reports_Music_2020>>();

            var userId = User.Identity.GetUserId();
            var channels = db.YoutubeChannels.Where(a => a.UserId.Equals(userId) && a.Approved == true).ToList();

            var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
            ViewBag.Share = user.share;

            int check = 0;

            foreach (var x in channels)
            {
                var list = new List<YT_Reports_Music_2020>();
                if (x.ChannelName == null && x.YT_Label_Name!=null)
                {
                    list = db.YT_Reports_Music_2020.Where(a => a.Asset_Labels.Equals(x.YT_Label_Name) && a.Month == M && a.Year == Y && a.Asset_Type.Equals("Sound Recording")).ToList();
                    //var checkviews = list.Sum(a=>a.)
                }
                else
                {
                    x.ChannelID = x.ChannelID.Trim();
                    list = db.YT_Reports_Music_2020.Where(a => a.Asset_Channel_ID.Equals(x.ChannelID) && a.Month == M && a.Year == Y).ToList();
                    if (list.Count != 0)
                        check = 1;
                }
                
                finalList.Add(list);
            }

            if (Int32.Parse(M) == 1) { ViewBag.Month = "January"; } else if (Int32.Parse(M) == 2) { ViewBag.Month = "February"; } else if (Int32.Parse(M) == 3) { ViewBag.Month = "March"; }
            else if (Int32.Parse(M) == 4) { ViewBag.Month = "April"; } else if (Int32.Parse(M) == 5) { ViewBag.Month = "May"; } else if (Int32.Parse(M) == 6) { ViewBag.Month = "June"; }
            else if (Int32.Parse(M) == 7) { ViewBag.Month = "July"; } else if (Int32.Parse(M) == 8) { ViewBag.Month = "August"; } else if (Int32.Parse(M) == 9) { ViewBag.Month = "September"; }
            else if (Int32.Parse(M) == 10) { ViewBag.Month = "October"; } else if (Int32.Parse(M) == 11) { ViewBag.Month = "November"; } else if (Int32.Parse(M) == 12) { ViewBag.Month = "December"; }

            ViewBag.Year = Y;
            ViewBag.checkCount = check;

            return View(finalList);
        }

        public ActionResult PDFReportAllCH(string M, string Y)
        {
            if (M == null || Y == null)
                return File("NotFound.txt", "text/plain");

            var userId = User.Identity.GetUserId();
            var channels = db.YoutubeChannels.Where(a => a.UserId.Equals(userId) && a.Approved == true).ToList();
            var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();

            int share = Int32.Parse(user.share);

            /*Creating iTextSharp’s Document & Writer*/
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            /*End*/

            var pdfImage = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
            pdfImage.Alignment = Element.ALIGN_MIDDLE;


            pdfDoc.Add(pdfImage);
            var para12 = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Kaveldunsvägen 21, 80636 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
            para12.Alignment = Element.ALIGN_MIDDLE;
            pdfDoc.Add(para12);

            //Chunk chunk = new Chunk("Report of All Channels", FontFactory.GetFont("Arial", 20, Font.BOLDITALIC, BaseColor.MAGENTA));
            //pdfDoc.Add(chunk);
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            pdfDoc.Add(line);


            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
            //PdfPCell myCell = new PdfPCell(new Phrase("Arial", new Font(Font.NORMAL, 8)));

            float[] columnWidths = new float[] { 200f, 240f, 110f, 100f, 100f, 100f };
            table.SetWidthPercentage(columnWidths, PageSize.A4.Rotate());


            table.AddCell("Channel ID");
            table.AddCell("Asset Title");
            table.AddCell("Asset Type");
            table.AddCell("Owned Views");
            table.AddCell("Revenue");
            table.AddCell("Share");

            if (Int32.Parse(M) == 1) { ViewBag.M = "January"; }
            else if (Int32.Parse(M) == 2) { ViewBag.M = "February"; }
            else if (Int32.Parse(M) == 3) { ViewBag.M = "March"; }
            else if (Int32.Parse(M) == 4) { ViewBag.M = "April"; }
            else if (Int32.Parse(M) == 5) { ViewBag.M = "May"; }
            else if (Int32.Parse(M) == 6) { ViewBag.M = "June"; }
            else if (Int32.Parse(M) == 7) { ViewBag.M = "July"; }
            else if (Int32.Parse(M) == 8) { ViewBag.M = "August"; }
            else if (Int32.Parse(M) == 9) { ViewBag.M = "September"; }
            else if (Int32.Parse(M) == 10) { ViewBag.M = "October"; }
            else if (Int32.Parse(M) == 11) { ViewBag.M = "November"; }
            else if (Int32.Parse(M) == 12) { ViewBag.M = "December"; }

            var i = ViewBag.M + "-" + Y + " Summary";
            Chunk chunk4 = new Chunk(i);
            chunk4.Font = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);
            pdfDoc.Add(chunk4);

            int view = 0;
            double rev = 0;
            foreach (var x in channels)
            {
                var list = new List<YT_Reports_Music_2020>();
                if (x.ChannelName == null && x.YT_Label_Name != null)
                {
                    list = db.YT_Reports_Music_2020.Where(a => a.Asset_Labels.Equals(x.YT_Label_Name) && a.Month == M && a.Year == Y && a.Asset_Type.Equals("Sound Recording")).ToList();
                }
                else
                {
                    x.ChannelID = x.ChannelID.Trim();
                    list = db.YT_Reports_Music_2020.Where(a => a.Asset_Channel_ID == x.ChannelID && a.Month == M && a.Year == Y).ToList();
                }


                if (list.Count > 0)
                {
                    table.AddCell(new Phrase(0, list[0].Asset_Channel_ID,FontFactory.GetFont(FontFactory.COURIER, 11)));
                    table.AddCell(new Phrase(0, list[0].Asset_Labels, FontFactory.GetFont(FontFactory.COURIER, 11)));
                    table.AddCell(new Phrase(0, list[0].Asset_Type, FontFactory.GetFont(FontFactory.COURIER, 11)));
                    table.AddCell(new Phrase(0, list.Sum(a => a.Owned_Views).ToString(), FontFactory.GetFont(FontFactory.COURIER, 11)));
                    table.AddCell(new Phrase(0, "$" + Math.Round(list.Sum(a => a.Partner_Revenue),2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 11)));
                    double asshare1 = (double)Math.Round(((list.Sum(a => a.Partner_Revenue) / 100) * share), 2);
                    table.AddCell(new Phrase(0, "$" + asshare1, FontFactory.GetFont(FontFactory.COURIER, 11)));

                    view = view + list.Sum(a => a.Owned_Views);
                    rev = rev + (double)list.Sum(a => a.Partner_Revenue);
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
            pdfDoc.Add(line);

            foreach (var x in channels)
            {
                x.ChannelID = x.ChannelID.Trim();
                var list = db.YT_Reports_Music_2020.Where(a => a.Asset_Channel_ID == x.ChannelID && a.Month == M && a.Year == Y).ToList();

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
                        table1.AddCell(channel.Asset_Channel_ID);
                        table1.AddCell(channel.Asset_Title);
                        table1.AddCell(channel.Asset_Type);
                        table1.AddCell(channel.Owned_Views.ToString());
                        table1.AddCell("$" + Math.Round(channel.Partner_Revenue,2).ToString());
                        double asshare = (double)Math.Round(((channel.Partner_Revenue / 100) * share), 2);
                        table1.AddCell("$" + asshare.ToString());
                    }

                    //table.AddCell(myCell);
                    table1.AddCell("Total");
                    table1.AddCell("");
                    table1.AddCell("");
                    table1.AddCell(list.Sum(a => a.Owned_Views).ToString());
                    table1.AddCell("$" + Math.Round(list.Sum(a=>a.Partner_Revenue),2).ToString());
                    double asshare1 = (double)Math.Round(((list.Sum(a => a.Partner_Revenue / 100)) * share), 2);
                    table1.AddCell("$"+asshare1);

                    
                    pdfDoc.Add(table1);

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

        public ActionResult DownloadReportHTML(string MN, string ID)
        {
            if (MN == null || ID == null)
                return RedirectToAction("Index");

            string mon = MN.Split('/')[0];
            string year = MN.Split('/')[1];

            var userId = User.Identity.GetUserId();
            var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
            ViewBag.Share = user.share;

            List<YT_Reports_Music_2020> finalList = new List<YT_Reports_Music_2020>();
            if (ID.Contains("LABEL"))
            {
                var label = ID.Substring(5);
                finalList = db.YT_Reports_Music_2020.Where(a => a.Asset_Labels.Equals(label) && a.Asset_Type.Equals("Sound Recording") && a.Month == mon && a.Year == year).ToList();
            }
            else
                finalList = db.YT_Reports_Music_2020.Where(a => a.Asset_Channel_ID.Equals(ID) && a.Month == mon && a.Year == year).ToList();

            return View(finalList);
        }

        public ActionResult DownloadReport(string MN, string ID)
        {
            if(MN == null || ID == null)
                return File("NotFound.txt", "text/plain");

            string mon = MN.Split('/')[0];
            string year = MN.Split('/')[1];

            List<YT_Reports_Music_2020> finalList = new List<YT_Reports_Music_2020>();
            if (ID.Contains("LABEL"))
            {
                var label = ID.Substring(5);
                finalList = db.YT_Reports_Music_2020.Where(a => a.Asset_Labels.Equals(label) && a.Asset_Type.Equals("Sound Recording") && a.Month == mon && a.Year == year).ToList();
            }
            else
                finalList = db.YT_Reports_Music_2020.Where(a => a.Asset_Channel_ID.Equals(ID) && a.Month == mon && a.Year == year).ToList();

            var userId = User.Identity.GetUserId();
            var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();

            /*Creating iTextSharp’s Document & Writer*/
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();


            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            float[] columnWidths = new float[] { 140f, 360f, 110f, 80f, 80f, 80f };
            table.SetWidthPercentage(columnWidths, PageSize.A4.Rotate());

            table.AddCell("Asset ID");
            table.AddCell("Asset Title");
            table.AddCell("Asset Label");
            table.AddCell("Owned Views");
            table.AddCell("Revenue");
            table.AddCell("Share");

            var pdfImage = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
            pdfImage.Alignment = Element.ALIGN_MIDDLE;


            pdfDoc.Add(pdfImage);
            var para12 = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Kaveldunsvägen 21, 80636 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
            para12.Alignment = Element.ALIGN_MIDDLE;
            pdfDoc.Add(para12);

            Chunk chunk4 = new Chunk("Channel ID:   " + ID + "          Month/Year:  " + mon + "/" + year);
            chunk4.Font = FontFactory.GetFont("Verdana", 12, Font.BOLD, BaseColor.BLACK);
            pdfDoc.Add(chunk4);

            foreach (var x in finalList)
            {
                table.AddCell(new Phrase(0, x.Asset_ID, FontFactory.GetFont(FontFactory.HELVETICA, 11)));
                table.AddCell(new Phrase(0, x.Asset_Title, FontFactory.GetFont(FontFactory.COURIER, 11)));
                table.AddCell(new Phrase(0, x.Asset_Labels, FontFactory.GetFont(FontFactory.COURIER, 11)));
                table.AddCell(new Phrase(0, x.Owned_Views.ToString(), FontFactory.GetFont(FontFactory.COURIER, 11)));
                table.AddCell(new Phrase(0, "$" + Math.Round(x.Partner_Revenue, 2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 11)));
                table.AddCell(new Phrase(0, "$" + Math.Round((x.Partner_Revenue / 100) * Int32.Parse(user.share), 2), FontFactory.GetFont(FontFactory.COURIER, 11)));
            }


            table.AddCell("Total");
            table.AddCell("");
            table.AddCell("");
            table.AddCell(finalList.Sum(a=>a.Owned_Views).ToString());
            table.AddCell("$" + Math.Round(finalList.Sum(a => a.Partner_Revenue), 2).ToString());
            table.AddCell("$" + Math.Round((finalList.Sum(a => a.Partner_Revenue) / 100) * Int32.Parse(user.share), 2));
            pdfDoc.Add(table);

            /*Creating the PDF file*/
            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Youtube-Channel-Report.pdf");
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

            var userId = User.Identity.GetUserId();

            var allData = db.YT_Reports_Music_2020
                .Join(
                db.YoutubeChannels.Where(a => a.UserId.Equals(userId) && a.Approved == true),
                d => d.Asset_Channel_ID,
                f => f.ChannelID,
                (d, f) => d).ToList();

            var labeldata = db.YT_Reports_Music_2020.Where(a=> a.Asset_Type.Equals("Sound Recording"))
                .Join(
                    db.YoutubeChannels.Where(a => a.UserId.Equals(userId) && a.Approved == true),
                    d => d.Asset_Labels,
                    f => f.YT_Label_Name,
                    (d, f) => d).ToList();

            allData.AddRange(labeldata);

            foreach (var x in allData)
            {
                if (!month.Contains(x.Month))
                    month.Add(x.Month);
                if (!year.Contains(x.Year))
                    year.Add(x.Year);
            }

            year.Sort();
            month.Sort();

            MY obj = new MY();
            obj.month = month;
            obj.year = year;
            
            
            return this.Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChannelNames()
        {
            var userId = User.Identity.GetUserId();
            List<YoutubeChannel> userData = db.YoutubeChannels.Where(a => a.UserId.Equals(userId) && a.ChannelName!=null && a.Approved==true).ToList();
            var obj1 = new YoutubeChannel();
            obj1.ChannelID = "";
            obj1.ChannelName = "-Label Name-";
            userData.Add(obj1);

            var Data = db.YoutubeChannels.Where(a => a.UserId.Equals(userId) && a.Approved == true).ToList();

            foreach (var x in Data)
            {
                if (x.YT_Label_Name != null)
                {
                    var obj = new YoutubeChannel();
                    obj.ChannelID = "LABEL" + x.YT_Label_Name;
                    obj.ChannelName = x.YT_Label_Name;
                    userData.Add(obj);
                }
            }

            return this.Json(userData, JsonRequestBehavior.AllowGet);
        }
      
        //public JsonResult GetYears(string channelId)
        //{
        //    List<YT_Reports> userData = db.YT_Reports.Where(a => a.ChannelID.Equals(channelId)).ToList();
        //    List<string> years = new List<string>();
        //    if (userData != null)
        //    {
        //        string year = userData.ElementAt(0).Year;
        //        years.Add(year);
        //        foreach (var x in userData)
        //        {
        //            if (!x.Year.Equals(year))
        //            {
        //                years.Add(x.Year);
        //                year = x.Year;
        //            }
        //        }
        //    }
        //    return this.Json(years, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetYears(string channelId)
        {
            //var sd = db.YT_Reports_Entertainment_Full_new.ToList();
            List<YT_Reports_Music_2020> userData = new List<YT_Reports_Music_2020>();
            if (channelId.Contains("LABEL"))
            {
                var label = channelId.Substring(5);
                userData = db.YT_Reports_Music_2020.Where(a => a.Asset_Labels.Equals(label) && a.Asset_Type.Equals("Sound Recording")).ToList();
            }
            else
                userData = db.YT_Reports_Music_2020.Where(a => a.Asset_Channel_ID.Equals(channelId)).ToList();
            //List<YT_Reports_Music_Full> userData = db.YT_Reports_Music_Full.Where(a => a.Asset_Channel_ID.Equals(channelId)).ToList();
            List<string> years = new List<string>();
            if (userData.Count > 0)
            {
                string year = userData.ElementAt(0).Year;
                years.Add(year);
                foreach (var x in userData)
                {
                    if (!x.Year.Equals(year))
                    {
                        years.Add(x.Year);
                        year = x.Year;
                    }
                }
            }
            return this.Json(years, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetMonths(string channelId)
        //{
        //    List<YT_Reports> userData = db.YT_Reports.Where(a => a.ChannelID.Equals(channelId)).ToList();
        //    List<string> months = new List<string>();
        //    if (userData != null)
        //    {
        //        string month = userData.ElementAt(0).Month;
        //        months.Add(month);
        //        foreach (var x in userData)
        //        {
        //            if (!x.Month.Equals(month))
        //            {
        //                months.Add(x.Month);
        //                month = x.Month;
        //            }
        //        }
        //    }
        //    return this.Json(months, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetMonths(string channelId)
        {
            List<YT_Reports_Music_2020> userData = new List<YT_Reports_Music_2020>();

            if (channelId.Contains("LABEL"))
            {
                var label = channelId.Substring(5);
                userData = db.YT_Reports_Music_2020.Where(a => a.Asset_Labels.Equals(label) && a.Asset_Type.Equals("Sound Recording")).ToList();
            }
            else
                userData = db.YT_Reports_Music_2020.Where(a => a.Asset_Channel_ID.Equals(channelId)).ToList();

            List<string> months = new List<string>();
            
            foreach (var x in userData)
            {
                if (!months.Contains(x.Month))
                {
                    months.Add(x.Month);
                }
            }
            
            return this.Json(months, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult GetChannelData(string channelId, string year, string month)
        //{ 
        //    List<YT_Reports> list = db.YT_Reports.Where(a => a.ChannelID.Equals(channelId) && a.Month.Equals(month) && a.Year.Equals(year)).ToList();

        //    int views = 0;
        //    double rev = 0;
        //    foreach (var x in list)
        //    {
        //        views = views + x.Views;
        //        rev = rev + decimal.ToDouble(x.Revenue);
        //    }

        //    ChannelData obj = new ChannelData();
        //    obj.ChannelId1 = list.ElementAt(0).ChannelID.ToString();
        //    obj.ChannelName1 = list.ElementAt(0).ChannelName.ToString();

        //    var sortedlist = list.OrderByDescending(b=>b.Views);

        //    obj.Month = Int32.Parse(month);
        //    obj.Year = Int32.Parse(year);

        //    obj.Views = views;
        //    obj.Revenue = Math.Round(rev, 2);
        //    obj.Top1 = list.ElementAt(0).AssetTitle;
        //    obj.Top2 = list.ElementAt(1).AssetTitle;
        //    obj.Top3 = list.ElementAt(2).AssetTitle;
        //    obj.Top4 = list.ElementAt(3).AssetTitle;
        //    obj.Top5 = list.ElementAt(4).AssetTitle;

        //    obj.Top1V = list.ElementAt(0).Views;
        //    obj.Top2V = list.ElementAt(1).Views;
        //    obj.Top3V = list.ElementAt(2).Views;
        //    obj.Top4V = list.ElementAt(3).Views;
        //    obj.Top5V = list.ElementAt(4).Views;

        //    return this.Json(obj, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetChannelData(string channelId, string year, string month)
        {
            List<YT_Reports_Music_2020> list = new List<YT_Reports_Music_2020>();

            if (channelId.Contains("LABEL"))
            {
                var label = channelId.Substring(5);
                list = db.YT_Reports_Music_2020.Where(a => a.Asset_Labels.Equals(label) && a.Asset_Type.Equals("Sound Recording") && a.Month.Equals(month) && a.Year.Equals(year)).ToList();
            }
            else
                list = db.YT_Reports_Music_2020.Where(a => a.Asset_Channel_ID.Equals(channelId) && a.Month.Equals(month) && a.Year.Equals(year)).ToList();

            //List<YT_Reports_Music_Full> list = db.YT_Reports_Music_Full.Where(a => a.Asset_Channel_ID.Equals(channelId) && a.Month.Equals(month) && a.Year.Equals(year)).ToList();

            var sortedlist = list.OrderByDescending(b => b.Partner_Revenue).ToList();


            //int views = 0;
            //double rev = 0;
            //foreach (var x in list)
            //{
            //    views = views + x.Owned_Views;
            //    rev = rev + decimal.ToDouble(x.Partner_Revenue);
            //}

            //ChannelData obj = new ChannelData();


          
            //if (list.Count > 0)
            //{
                
            //    obj.ChannelId1 = list[0].Asset_Channel_ID.ToString();
            //    obj.ChannelName1 = list[0].Asset_Labels.ToString();

            //    var sortedlist = list.OrderByDescending(b => b.Owned_Views).ToList();

            //    obj.Month = Int32.Parse(month);
            //    obj.Year = Int32.Parse(year);

            //    obj.Views = views;
            //    obj.Revenue = Math.Round(rev, 2);
            //    obj.Top1 = sortedlist[0].Asset_Title;
            //    obj.Top1V = sortedlist[0].Owned_Views;

            //    if (list.Count > 1)
            //    {
            //        obj.Top2 = sortedlist[1].Asset_Title;
            //        obj.Top2V = sortedlist[1].Owned_Views;

            //        if(list.Count > 2)
            //        {
            //            obj.Top3 = sortedlist[2].Asset_Title;
            //            obj.Top3V = sortedlist[2].Owned_Views;

            //            if(list.Count > 3)
            //            {
            //                obj.Top4 = sortedlist[3].Asset_Title;
            //                obj.Top4V = sortedlist[3].Owned_Views;

            //                if(list.Count > 4)
            //                {
            //                    obj.Top5 = sortedlist[4].Asset_Title;
            //                    obj.Top5V = sortedlist[4].Owned_Views;
            //                }
            //            }
            //        }
            //    }
            //}

            return this.Json(sortedlist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult NewChart()
        {
            List<string> years = new List<string>();
            List<string> months = new List<string>();

            var userId = User.Identity.GetUserId();
            var youtubeList = db.YoutubeChannels.Where(a => a.UserId.Equals(userId) && a.Approved == true).ToList();

            List<YT_Reports_Music_2020> finalList = new List<YT_Reports_Music_2020>();
            foreach (var x in youtubeList)
            {
                if(x.ChannelID != null)
                {
                    x.ChannelID = x.ChannelID.Trim();
                    var chList = db.YT_Reports_Music_2020.Where(a => a.Asset_Channel_ID.Equals(x.ChannelID)).ToList();
                    finalList.AddRange(chList);
                }
                else
                {
                    var chList = db.YT_Reports_Music_2020.Where(a => a.Asset_Labels.Equals(x.YT_Label_Name)).ToList();
                    finalList.AddRange(chList);
                }
            }

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

            List<object> iData = new List<object>();
            //Creating sample data  
            DataTable dt = new DataTable();
            dt.Columns.Add("Year/Month", System.Type.GetType("System.String"));
            dt.Columns.Add("Revenue", System.Type.GetType("System.Decimal"));

            years = years.OrderBy(a => Int32.Parse(a)).ToList();
            months = months.OrderBy(a => Int32.Parse(a)).ToList();

            foreach (var y in years)
            {
                foreach (var m in months)
                {
                    var revenueList = finalList.Where(a => a.Month.Equals(m) && a.Year.Equals(y)).ToList();
                    if (revenueList.Count > 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Year/Month"] = y + " / " + m;
                        decimal rev = revenueList.Sum(a => a.Partner_Revenue);
                        dr["Revenue"] = Math.Round(rev, 2);
                        dt.Rows.Add(dr);
                    }
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


        public JsonResult GetStoreYears(string store)
        {
            var userId = User.Identity.GetUserId();
            var releaselist = db.MusicReleases.Where(x => x.UserId == userId).ToList();

            if (store.Equals("Spotify"))
            {
                var finalList = new List<string>();
                foreach (var y in releaselist)
                {
                    var spotifylist = db.SpotifyReportFull.Where(x => x.UPC.Equals(y.UPCEAN)).Select(a => a.Year).ToList();
                    finalList.AddRange(spotifylist);
                }

                var years = new List<string>();
                foreach(var x in finalList)
                {
                    if (!years.Contains(x))
                    {
                        years.Add(x);
                    }
                }

                List<int> ys = new List<int>();

                foreach (var x in years)
                {
                    ys.Add(Int32.Parse(x));
                }

                var Y1 = ys.OrderBy(x => x).ToArray();

                List<string> Y = new List<string>();

                foreach (var x in Y1)
                {
                    Y.Add(x.ToString());
                }

                return this.Json(Y, JsonRequestBehavior.AllowGet);
            }

            if (store.Equals("Deezer"))
            {
                var finalList = new List<string>();
                foreach (var y in releaselist)
                {
                    var deezerlist = db.Deezers.Where(x => x.UPC.Equals(y.UPCEAN)).Select(a => a.Year).ToList();
                    finalList.AddRange(deezerlist);
                }

                var years = new List<string>();
                foreach (var x in finalList)
                {
                    if (!years.Contains(x))
                    {
                        years.Add(x);
                    }
                }

                List<int> ys = new List<int>();

                foreach (var x in years)
                {
                    ys.Add(Int32.Parse(x));
                }

                var Y1 = ys.OrderBy(x => x).ToArray();

                List<string> Y = new List<string>();

                foreach (var x in Y1)
                {
                    Y.Add(x.ToString());
                }

                return this.Json(Y, JsonRequestBehavior.AllowGet);
            }

            if (store.Equals("GooglePlay"))
            {
                var finalList = new List<string>();
                foreach (var y in releaselist)
                {
                    var googlePlaylist = db.Google_Play.Where(x => x.UPC.Equals(y.UPCEAN)).Select(a => a.Year).ToList();
                    finalList.AddRange(googlePlaylist);
                }

                var years = new List<string>();
                foreach (var x in finalList)
                {
                    if (!years.Contains(x))
                    {
                        years.Add(x);
                    }
                }

                List<int> ys = new List<int>();

                foreach (var x in years)
                {
                    ys.Add(Int32.Parse(x));
                }

                var Y1 = ys.OrderBy(x => x).ToArray();

                List<string> Y = new List<string>();

                foreach (var x in Y1)
                {
                    Y.Add(x.ToString());
                }

                return this.Json(Y, JsonRequestBehavior.AllowGet);
            }

            if (store.Equals("SoundCloud"))
            {
                var finalList = new List<string>();
                foreach (var y in releaselist)
                {
                    var soundCloudlist = db.SoundCloud.Where(x => x.UPC.Equals(y.UPCEAN)).Select(a => a.Year).ToList();
                    finalList.AddRange(soundCloudlist);
                }

                var years = new List<string>();
                foreach (var x in finalList)
                {
                    if (!years.Contains(x))
                    {
                        years.Add(x);
                    }
                }

                List<int> ys = new List<int>();

                foreach (var x in years)
                {
                    ys.Add(Int32.Parse(x));
                }

                var Y1 = ys.OrderBy(x => x).ToArray();

                List<string> Y = new List<string>();

                foreach (var x in Y1)
                {
                    Y.Add(x.ToString());
                }

                return this.Json(Y, JsonRequestBehavior.AllowGet);
            }

            return this.Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStoreMonths(string store, string year)
        {
            var userId = User.Identity.GetUserId();
            var releaselist = db.MusicReleases.Where(x => x.UserId == userId).ToList();

            if (store.Equals("Spotify"))
            {
                var finalList = new List<string>();
                foreach (var y in releaselist)
                {
                    var spotifylist = db.SpotifyReportFull.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year)).Select(a=>a.Month).ToList();
                    finalList.AddRange(spotifylist);
                }

                var months = new List<string>();
                foreach (var x in finalList)
                {
                    if (!months.Contains(x))
                    {
                        months.Add(x);
                    }
                }

                List<int> ms = new List<int>();

                foreach (var x in months)
                {
                    ms.Add(Int32.Parse(x));
                }

                var M1 = ms.OrderBy(x => x).ToArray();

                List<string> M = new List<string>();

                foreach (var x in M1)
                {
                    M.Add(x.ToString());
                }

                return this.Json(M, JsonRequestBehavior.AllowGet);
            }

            if (store.Equals("Deezer"))
            {
                var finalList = new List<string>();
                foreach (var y in releaselist)
                {
                    var spotifylist = db.Deezers.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year)).Select(a => a.Month).ToList();
                    finalList.AddRange(spotifylist);
                }

                var months = new List<string>();
                foreach (var x in finalList)
                {
                    if (!months.Contains(x))
                    {
                        months.Add(x);
                    }
                }

                List<int> ms = new List<int>();

                foreach (var x in months)
                {
                    ms.Add(Int32.Parse(x));
                }

                var M1 = ms.OrderBy(x => x).ToArray();

                List<string> M = new List<string>();

                foreach (var x in M1)
                {
                    M.Add(x.ToString());
                }

                return this.Json(M, JsonRequestBehavior.AllowGet);
            }

            if (store.Equals("GooglePlay"))
            {
                var finalList = new List<string>();
                foreach (var y in releaselist)
                {
                    var spotifylist = db.Google_Play.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year)).Select(a => a.Month).ToList();
                    finalList.AddRange(spotifylist);
                }

                var months = new List<string>();
                foreach (var x in finalList)
                {
                    if (!months.Contains(x))
                    {
                        months.Add(x);
                    }
                }

                List<int> ms = new List<int>();

                foreach (var x in months)
                {
                    ms.Add(Int32.Parse(x));
                }

                var M1 = ms.OrderBy(x => x).ToArray();

                List<string> M = new List<string>();

                foreach (var x in M1)
                {
                    M.Add(x.ToString());
                }

                return this.Json(M, JsonRequestBehavior.AllowGet);
            }

            if (store.Equals("SoundCloud"))
            {
                var finalList = new List<string>();
                foreach (var y in releaselist)
                {
                    var spotifylist = db.SoundCloud.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year)).Select(a => a.Month).ToList();
                    finalList.AddRange(spotifylist);
                }

                var months = new List<string>();
                foreach (var x in finalList)
                {
                    if (!months.Contains(x))
                    {
                        months.Add(x);
                    }
                }

                List<int> ms = new List<int>();

                foreach (var x in months)
                {
                    ms.Add(Int32.Parse(x));
                }

                var M1 = ms.OrderBy(x => x).ToArray();

                List<string> M = new List<string>();

                foreach (var x in M1)
                {
                    M.Add(x.ToString());
                }

                return this.Json(M, JsonRequestBehavior.AllowGet);
            }

            return this.Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult getStoreData(string store, string year, string month)
        {
            var userId = User.Identity.GetUserId();
            var releaselist = db.MusicReleases.Where(x => x.UserId == userId && x.Status != "Deleted").ToList();
            if (store.Equals("Spotify"))
            {
                List<SpotifyReports_2020> finalList = new List<SpotifyReports_2020>();
                foreach (var y in releaselist)
                {
                    var spotifylist = db.SpotifyReports_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                    foreach(var x in spotifylist)
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
                List<Napster_Streaming > finalList = new List<Napster_Streaming>();
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

        public JsonResult GetStoreDataYearly(string store, string year)
        {
            var userId = User.Identity.GetUserId();
            var releaselist = db.MusicReleases.Where(x => x.UserId == userId && x.Status != "Deleted").ToList();
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

                if(sortedlist.Count > 50)
                {
                    var newsortedlist = sortedlist.Take(50);
                    var result = new { list = newsortedlist, views = sortedlist.Sum(a=>a.Quantity), revenue = sortedlist.Sum(a => a.USD_Payable) };
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
            //var userId = User.Identity.GetUserId();
            //var releaselist = db.MusicReleases.Where(x => x.UserId == userId && x.Status != "Deleted" && x.LabelName.Equals(label)).ToList();
            if (store.Equals("Spotify"))
            {
                //List<SpotifyReports_2020> finalList = new List<SpotifyReports_2020>();
                //foreach (var y in releaselist)
                //{
                //    //var spotifylist = db.SpotifyReports_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                //    var spotifylist = db.SpotifyReports_2020.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                //    foreach (var x in spotifylist)
                //    {
                //        x.Composer_Name = y.ReleaseName;
                //    }
                //    finalList.AddRange(spotifylist);
                //}

                List<SpotifyReports_2020> finalList = db.SpotifyReports_2020.Where(x => x.Label.Equals(label) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();

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
                //List<DeezerReport_2020> finalList = new List<DeezerReport_2020>();
                //foreach (var y in releaselist)
                //{
                //    //var deezerlist = db.DeezerReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                //    var deezerlist = db.DeezerReport_2020.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                //    foreach (var x in deezerlist)
                //    {
                //        x.Service = y.ReleaseName;
                //    }
                //    finalList.AddRange(deezerlist);
                //}
                List<DeezerReport_2020> finalList = db.DeezerReport_2020.Where(x => x.Label.Equals(label) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
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
                //List<GoogleMusicReport_2020> finalList = new List<GoogleMusicReport_2020>();
                //foreach (var y in releaselist)
                //{
                //    //List<GoogleMusicReport_2020> Google_Playlist = db.GoogleMusicReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                //    List<GoogleMusicReport_2020> Google_Playlist = db.GoogleMusicReport_2020.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                //    foreach (var x in Google_Playlist)
                //    {
                //        x.Interaction_Type = y.ReleaseName;
                //    }
                //    finalList.AddRange(Google_Playlist);
                //}

                List<GoogleMusicReport_2020> finalList = db.GoogleMusicReport_2020.Where(x => x.Label.Equals(label) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();

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
                //List<SoundCloudReport_2020> finalList = new List<SoundCloudReport_2020>();
                //foreach (var y in releaselist)
                //{
                //    //var SoundCloudlist = db.SoundCloudReport_2020.Where(x => x.UPC.Equals(y.UPCEAN) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                //    var SoundCloudlist = db.SoundCloudReport_2020.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                //    foreach (var x in SoundCloudlist)
                //    {
                //        x.Usage_Type = y.ReleaseName;
                //    }
                //    finalList.AddRange(SoundCloudlist);
                //}

                List<SoundCloudReport_2020> finalList = db.SoundCloudReport_2020.Where(x => x.LabelName.Equals(label) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();

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
                //List<Apple_Music_New> finalList = new List<Apple_Music_New>();
                //foreach (var y in releaselist)
                //{
                //    var SoundCloudlist = db.Apple_Music_New.Where(x => x.MusicReleaseId.Equals(y.MusicReleaseId) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();
                //    foreach (var x in SoundCloudlist)
                //    {
                //        x.Configuration = y.ReleaseName;
                //    }
                //    finalList.AddRange(SoundCloudlist);
                //}
                List<Apple_Music_New> finalList = db.Apple_Music_New.Where(x => x.Release_Label.Equals(label) && x.Year.Equals(year) && x.Month.Equals(month)).ToList();

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
                var userId = User.Identity.GetUserId();
                var releaselist = db.MusicReleases.Where(x => x.UserId == userId && x.Status != "Deleted" && x.LabelName.Equals(label)).ToList();
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
                var userId = User.Identity.GetUserId();
                var releaselist = db.MusicReleases.Where(x => x.UserId == userId && x.Status != "Deleted" && x.LabelName.Equals(label)).ToList();
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
                var userId = User.Identity.GetUserId();
                var releaselist = db.MusicReleases.Where(x => x.UserId == userId && x.Status != "Deleted" && x.LabelName.Equals(label)).ToList();
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