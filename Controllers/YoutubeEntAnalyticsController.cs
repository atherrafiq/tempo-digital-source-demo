using Microsoft.AspNet.Identity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TempoDigitalApex3.Models;
using System.IO;
using System.Data;
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;

namespace TempoDigitalApex3.Controllers
{
    public struct MY1
    {
        public List<string> month;
        public List<string> year;
    }

    public class YoutubeEntAnalyticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: YoutubeEntAnalytics
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
                ViewBag.Share = user.share;

                var entChannels = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(userId) && a.Approved==true).ToList();
                ViewBag.EntChannelCount = entChannels.Count;
                return View();
            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public PartialViewResult NoEntChannelPartial()
        {
            return PartialView("_NoEntChannelPartial");
        }

        public ActionResult YoutubeEntAnalytics()
        {
            var user = User.Identity.GetUserId();
            var channel = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(user) && a.Approved == true && a.YT_Label_Name == null).ToList();

            var channelsID = new List<string>();
            var channelName = new List<string>();

            channelsID.AddRange(channel.Select(a => a.ChannelID));
            channelName.AddRange(channel.Select(a => a.ChannelName));

            var channelLabels = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(user) && a.Approved == true && a.YT_Label_Name != null).ToList();

            channelsID.AddRange(channelLabels.Select(a => a.YT_Label_Name));
            channelName.AddRange(channelLabels.Select(a => a.YT_Label_Name));

            ViewBag.Channels = channel.Select(a => a.ChannelID);
            ViewBag.ChannelsName = channel.Select(a => a.ChannelName);

            ViewBag.EntChannelCount = channel.Count;

            //ViewBag.Channels = channelsID;
            //ViewBag.ChannelsName = channelName;
            //ViewBag.EntChannelCount = channelsID.Count;

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

        public ActionResult AnalyticsEntChannelDetails(string channelID, string name)
        {
            if (channelID == null)
                return RedirectToAction("YoutubeEntAnalytics");

            ViewBag.ChannelId = channelID;
            ViewBag.Name = name;

            ViewBag.Key = ConfigurationManager.AppSettings["APIKEY"];
            //ViewBag.Token = GetAccessToken();
            ViewBag.Token = GetAccessTokenHerokuAPI();

            return View();
        }

        public ActionResult HTMLReportAllCH(string M, string Y)
        {
            if (M == null || Y == null)
                return RedirectToAction("Index");

            List<List<YT_Reports_Ent_2020>> finalList = new List<List<YT_Reports_Ent_2020>>();

            var userId = User.Identity.GetUserId();
            var channels = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(userId) && a.Approved == true).ToList();

            var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
            ViewBag.Share = user.share;

            int check = 0;

            foreach (var x in channels)
            {
                var list = new List<YT_Reports_Ent_2020>();
                if (x.ChannelName == null && x.YT_Label_Name != null)
                {
                    list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Labels.Equals(x.YT_Label_Name) && a.Month == M && a.Year == Y ).ToList();
                }
                else
                {
                    x.ChannelID = x.ChannelID.Trim();
                    list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Channel_ID == x.ChannelID && a.Month == M && a.Year == Y).ToList();
                    if (list.Count != 0)
                        check = 1;
                }

                finalList.Add(list);
            }

            if (Int32.Parse(M) == 1) { ViewBag.Month = "January"; }
            else if (Int32.Parse(M) == 2) { ViewBag.Month = "February"; }
            else if (Int32.Parse(M) == 3) { ViewBag.Month = "March"; }
            else if (Int32.Parse(M) == 4) { ViewBag.Month = "April"; }
            else if (Int32.Parse(M) == 5) { ViewBag.Month = "May"; }
            else if (Int32.Parse(M) == 6) { ViewBag.Month = "June"; }
            else if (Int32.Parse(M) == 7) { ViewBag.Month = "July"; }
            else if (Int32.Parse(M) == 8) { ViewBag.Month = "August"; }
            else if (Int32.Parse(M) == 9) { ViewBag.Month = "September"; }
            else if (Int32.Parse(M) == 10) { ViewBag.Month = "October"; } else if (Int32.Parse(M) == 11) { ViewBag.Month = "November"; } else if (Int32.Parse(M) == 12) { ViewBag.Month = "December"; }

            ViewBag.Year = Y;
            ViewBag.checkCount = check;

            return View(finalList);
        }

        public ActionResult PDFReportAllCH(string M, string Y)
        {
            var UId = User.Identity.GetUserId();
            var channels = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(UId) && a.Approved == true).ToList();
            var user = db.Users.Where(a => a.Id.Equals(UId)).FirstOrDefault();

            if (M == null || Y == null)
            {
                return File("NotFound.txt", "text/plain");
            }

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
                var list = new List<YT_Reports_Ent_2020>();
                if (x.ChannelName == null && x.YT_Label_Name != null)
                {
                    list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Labels.Equals(x.YT_Label_Name) && a.Month == M && a.Year == Y ).ToList();
                }
                else
                {
                    list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Channel_ID == x.ChannelID && a.Month == M && a.Year == Y).ToList();
                }


                if (list.Count > 0)
                {
                    table.AddCell(new Phrase(0, list[0].Asset_Channel_ID, FontFactory.GetFont(FontFactory.COURIER, 11)));
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
                var list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Channel_ID == x.ChannelID && a.Month == M && a.Year == Y).ToList();

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

            List<YT_Reports_Ent_2020> finalList = new List<YT_Reports_Ent_2020>();
            if (ID.Contains("LABEL"))
            {
                var label = ID.Substring(5);
                finalList = db.YT_Reports_Ent_2020.Where(a => a.Asset_Labels.Equals(label) &&  a.Month == mon && a.Year == year).ToList();
            }
            else
                finalList = db.YT_Reports_Ent_2020.Where(a => a.Asset_Channel_ID.Equals(ID) && a.Month == mon && a.Year == year).ToList();

            return View(finalList);
        }

        public ActionResult DownloadReport(string MN, string ID)
        {
            if (MN == null || ID == null)
                return File("NotFound.txt", "text/plain");

            string mon = MN.Split('/')[0];
            string year = MN.Split('/')[1];

            List<YT_Reports_Ent_2020> finalList = new List<YT_Reports_Ent_2020>();
            if (ID.Contains("LABEL"))
            {
                var label = ID.Substring(5);
                finalList = db.YT_Reports_Ent_2020.Where(a => a.Asset_Labels.Equals(label) && a.Month == mon && a.Year == year).ToList();
            }
            else
                finalList = db.YT_Reports_Ent_2020.Where(a => a.Asset_Channel_ID.Equals(ID) && a.Month == mon && a.Year == year).ToList();

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
            table.AddCell(finalList.Sum(a => a.Owned_Views).ToString());
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

            var allData = db.YT_Reports_Ent_2020
                .Join(
                db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(userId) && a.Approved == true),
                d => d.Asset_Channel_ID,
                f => f.ChannelID,
                (d, f) => d).ToList();

            var labeldata = db.YT_Reports_Ent_2020
                .Join(
                db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(userId) && a.Approved == true),
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
            List<YoutubeChannelsENT> userData = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(userId) && a.ChannelName != null && a.Approved == true).ToList();
            var obj1 = new YoutubeChannelsENT();
            obj1.ChannelID = "";
            obj1.ChannelName = "-Label Name-";
            userData.Add(obj1);

            var Data = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(userId) && a.Approved == true).ToList();

            foreach (var x in Data)
            {
                if (x.YT_Label_Name != null)
                {
                    var obj = new YoutubeChannelsENT();
                    obj.ChannelID = "LABEL" + x.YT_Label_Name;
                    obj.ChannelName = x.YT_Label_Name;
                    userData.Add(obj);
                }
            }

            return this.Json(userData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetYears(string channelId)
        {
            //db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            //var sd = db.yT_Reports_Ents.Where(a => a.Asset_Channel_ID.Equals(channelId)).ToList();
            List<YT_Reports_Ent_2020> userData = new List<YT_Reports_Ent_2020>();
            if (channelId.Contains("LABEL"))
            {
                var label = channelId.Substring(5);
                userData = db.YT_Reports_Ent_2020.Where(a => a.Asset_Labels.Equals(label)).ToList();
            }
            else
                userData = db.YT_Reports_Ent_2020.Where(a => a.Asset_Channel_ID.Equals(channelId)).ToList();
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

        public JsonResult GetMonths(string channelId)
        {
            List<YT_Reports_Ent_2020> userData = new List<YT_Reports_Ent_2020>();

            if (channelId.Contains("LABEL"))
            {
                var label = channelId.Substring(5);
                userData = db.YT_Reports_Ent_2020.Where(a => a.Asset_Labels.Equals(label)).ToList();
            }
            else
                userData = db.YT_Reports_Ent_2020.Where(a => a.Asset_Channel_ID.Equals(channelId)).ToList();

            List<string> months = new List<string>();
            if (userData.Count > 0)
            {
                string month = userData.ElementAt(0).Month;
                if (!months.Contains(month))
                    months.Add(month);
                foreach (var x in userData)
                {
                    if (!x.Month.Equals(month))
                    {
                        if (!months.Contains(x.Month)) { 
                            months.Add(x.Month);
                            month = x.Month;
                        }
                    }
                }
            }
            return this.Json(months, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChannelData(string channelId, string year, string month)
        {
            List<YT_Reports_Ent_2020> list = new List<YT_Reports_Ent_2020>();

            if (channelId.Contains("LABEL"))
            {
                var label = channelId.Substring(5);
                list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Labels.Equals(label) &&  a.Month.Equals(month) && a.Year.Equals(year)).ToList();
            }
            else
                list = db.YT_Reports_Ent_2020.Where(a => a.Asset_Channel_ID.Equals(channelId) && a.Month.Equals(month) && a.Year.Equals(year)).ToList();

            //List<YT_Reports_Music_Full> list = db.YT_Reports_Music_Full.Where(a => a.Asset_Channel_ID.Equals(channelId) && a.Month.Equals(month) && a.Year.Equals(year)).ToList();

            int views = 0;
            double rev = 0;
            foreach (var x in list)
            {
                views = views + x.Owned_Views;
                rev = rev + decimal.ToDouble(x.Partner_Revenue);
            }

            var sortedlist = list.OrderByDescending(b => b.Partner_Revenue);

            //ChannelData obj = new ChannelData();


            //if (list.Count > 0)
            //{

            //    obj.ChannelId1 = list[0].Asset_Channel_ID.ToString();
            //    obj.ChannelName1 = list[0].Asset_Labels.ToString();

            //    var sortedlist = list.OrderByDescending(b => b.Owned_Views);

            //    obj.Month = Int32.Parse(month);
            //    obj.Year = Int32.Parse(year);

            //    obj.Views = views;
            //    obj.Revenue = Math.Round(rev, 2);
            //    obj.Top1 = list[0].Asset_Title;
            //    obj.Top1V = list[0].Owned_Views;

            //    if (list.Count > 1)
            //    {
            //        obj.Top2 = list[1].Asset_Title;
            //        obj.Top2V = list[1].Owned_Views;

            //        if (list.Count > 2)
            //        {
            //            obj.Top3 = list[2].Asset_Title;
            //            obj.Top3V = list[2].Owned_Views;

            //            if (list.Count > 3)
            //            {
            //                obj.Top4 = list[3].Asset_Title;
            //                obj.Top4V = list[3].Owned_Views;

            //                if (list.Count > 4)
            //                {
            //                    obj.Top5 = list[4].Asset_Title;
            //                    obj.Top5V = list[4].Owned_Views;
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
            var youtubeList = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(userId) && a.Approved == true).ToList();

            List<YT_Reports_Ent_2020> finalList = new List<YT_Reports_Ent_2020>();
            foreach (var x in youtubeList)
            {
                if(x.ChannelID != null)
                {
                    x.ChannelID = x.ChannelID.Trim();
                    var chList = db.YT_Reports_Ent_2020.Where(a => a.Asset_Channel_ID.Equals(x.ChannelID)).ToList();
                    finalList.AddRange(chList);
                }
                else
                {
                    var chList = db.YT_Reports_Ent_2020.Where(a => a.Asset_Labels.Equals(x.YT_Label_Name)).ToList();
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
                        double rev = (double)revenueList.Sum(a => a.Partner_Revenue);
                        dr["Revenue"] = Math.Round(rev, 1);
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
    }
}