using Microsoft.AspNet.Identity;
using Microsoft.Reporting.WebForms;
using Stripe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TempoDigitalApex3.Models;
using PagedList.Mvc;
using PagedList;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure;
using Microsoft.Azure.Storage;
using System.Configuration;
using Microsoft.Azure.Storage.RetryPolicies;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Xml;
using Amazon.S3;
using Amazon.S3.Model;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace TempoDigitalApex3.Controllers
{
    [Authorize]
    public class MusicReleasesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ReleaseDetails(int? id)
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
                //ViewBag.SpotifyReport = SpotifyReport(release.UPCEAN);
                //ViewBag.DeezersReport = DeezersReport(release.UPCEAN);
                //ViewBag.GooglePlayReport = GooglePlayReport(release.UPCEAN);
                //ViewBag.SoundCloudReport = SoundCloudReport(release.UPCEAN);

                var cover = "";
                if (release.CoverImage != null)
                {
                    cover = release.CoverImage.Substring(0, release.CoverImage.Length - 3);
                    if (cover.EndsWith("_U."))
                    {
                        cover = cover.Replace("_U","");
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
        
        // GET: MusicReleases
        public ActionResult Index(string search, string filter, int? i, string date)
        {
            //var re = db.MusicReleases.ToList();
            //foreach(var x in re)
            //{
            //    if(x.DeletionDate != null && x.DeletionDate!="44119")
            //    {
            //        var date_ = Convert.ToDateTime(x.DeletionDate, CultureInfo.InvariantCulture);
            //        x.DeletionDate = date_.ToString("yyyy-MM-dd HH:mm:ss");
            //        db.SaveChanges();
            //    }
            //}

            //var resd = db.MusicReleases.Where(a => a.UserId.Equals("921e5b8c-8749-4411-931b-decef3d4d359")).ToList();
            //foreach (var x in resd)
            //{
            //    if (x.UPCEAN.Substring(0,4).Equals("0650"))
            //    {
            //        var img = get_image_from_API(x.UPCEAN);
            //        if (img != null)
            //        {
            //            x.CoverImage = img;
            //            db.SaveChanges();
            //        }
            //    }                
            //

            //var musicrelease = db.MusicReleases.Where(a => a.MusicReleaseId == 2168).FirstOrDefault();
            //var xml = EP(musicrelease, "Test");


            //string targetFolder = Server.MapPath("~/assets");
            //string targetPath = Path.Combine(targetFolder, "file_test.xml");
            //xml.Save(targetPath);


            var userId = User.Identity.GetUserId();
           
            if(date != null)
            {
                var list = db.MusicReleases.Where(x => x.UserId == userId && x.PaymentStaus.Equals("Paid") && (x.Status == "Distributed" || x.Status == "Approved" || x.Status == null || x.Status != "Deleted")).ToList();
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
                    return RedirectToAction("Index","MusicReleases");
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
                var list = db.MusicReleases.Where(x => x.UserId == userId && x.PaymentStaus.Equals("Paid") && (x.Status == "Distributed" || x.Status == "Approved" || x.Status == null || x.Status != "Deleted")).ToList();
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
                var list = db.MusicReleases.Where(x => x.UserId == userId && x.PaymentStaus.Equals("Paid") && (x.Status == "Distributed" || x.Status == "Approved" || x.Status == null || x.Status != "Deleted")).ToList();
                var finalList = new List<MusicRelease>();
                foreach(var x in list)
                {
                    if(filter== "Name")
                    {
                        if (x.ReleaseName != null)
                        {
                            if (x.ReleaseName.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) != -1)
                                finalList.Add(x);
                        }
                    }
                    else if(filter== "Artist")
                    {
                        if (x.MainArtist != null)
                        {
                            if (x.MainArtist.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) != -1)
                                finalList.Add(x);
                        }
                    }
                    else if (filter == "UPC")
                    {
                        if(x.UPCEAN!=null)
                        {
                            if (x.UPCEAN.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) != -1)
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

        public List<MusicRelease> get_list_images(List<MusicRelease> list)
        {
            foreach(var x in list)
            {
                var image = get_image_from_API(x.UPCEAN.Trim());
                if(image != null)
                {
                    x.CoverImage = image;
                }
            }
            return list;
        }

        public string get_image_from_API(string upc)
        {
            string url = @"https://api.audiosalad.com/fetch.php?k=19E1170F02FCED03938BE12B241E09034BDDD5E397902460B43F6197C40260A0&g_profile=tempodigital.audiosalad.com&upc=" + upc;

            HttpClient httpClient = new HttpClient();
            var result = httpClient.GetAsync(url).Result;
            if (result.StatusCode.ToString() == "OK")
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    var html = reader.ReadToEnd().ToString();
                    var objects = JArray.Parse(html); // parse as array  
                    foreach (JObject root in objects)
                    {
                        foreach (KeyValuePair<String, JToken> app in root)
                        {
                            var appName = app.Key;

                            if (appName.Equals("images"))
                            {
                                var value = (String)app.Value[0]["urlMedium"];
                                return value;
                            }
                        }
                    }
                }
            }

            return null;
        }

        // GET: MusicReleases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "MusicReleases", new { id = 0 });
            }

            var list = db.Songs.Where(a => a.MusicReleaseId == id ).ToList();
            var release = db.MusicReleases.Where(a => a.MusicReleaseId == id).FirstOrDefault();
            if (release != null)
            {
                ViewBag.Release = release;
                ViewBag.SpotifyReport = SpotifyReport(release.UPCEAN);
                ViewBag.DeezersReport = DeezersReport(release.UPCEAN);
                ViewBag.GooglePlayReport = GooglePlayReport(release.UPCEAN);
                ViewBag.SoundCloudReport = SoundCloudReport(release.UPCEAN);

                var cover = "";
                if(release.CoverImage != null)
                {
                    cover = release.CoverImage.Substring(0, release.CoverImage.Length - 3);
                    cover += "csv";
                }
                ViewBag.csv = cover;

            }

            //var report = db.SpotifyReportFull.Where(a => a.UPC.Equals(release.UPCEAN)).ToList();

            var userId = User.Identity.GetUserId();
            return View(list);
        }

        List<SpotifyReports_2020> SpotifyReport(string upc)
        {
            var report = db.SpotifyReports_2020.Where(a => a.UPC.Equals(upc)).ToList();
            List<string> yearsMonths = new List<string>();
            foreach (var x in report)
            {
                if (!yearsMonths.Contains(x.Year + "$" + x.Month))
                {
                    yearsMonths.Add(x.Year + "$" + x.Month);
                }
            }

            List<SpotifyReports_2020> listNew = new List<SpotifyReports_2020>();
            foreach (var x in yearsMonths)
            {
                List<String> s = new List<String>(x.Split("$".ToCharArray()));

                var tmp = report.Where(a => a.Year.Equals(s[0]) && a.Month.Equals(s[1])).ToList();
                SpotifyReports_2020 obj = new SpotifyReports_2020();
                obj.Month = s[1].ToString();
                obj.Year = s[0].ToString();
                obj.UPC = report[0].UPC;

                double payable = 0;
                foreach (var y in tmp)
                {
                    payable = payable + (double)y.USD_Payable;
                }

                obj.USD_Payable = (double)payable;

                listNew.Add(obj);
            }

            return listNew;
        }
        List<DeezerReport_2020> DeezersReport(string upc)
        {
            var report = db.DeezerReport_2020.Where(a => a.UPC.Equals(upc)).ToList();
            List<string> yearsMonths = new List<string>();
            foreach (var x in report)
            {
                if (!yearsMonths.Contains(x.Year + "$" + x.Month))
                {
                    yearsMonths.Add(x.Year + "$" + x.Month);
                }
            }

            List<DeezerReport_2020> listNew = new List<DeezerReport_2020>();
            foreach (var x in yearsMonths)
            {
                List<String> s = new List<String>(x.Split("$".ToCharArray()));

                var tmp = report.Where(a => a.Year.Equals(s[0]) && a.Month.Equals(s[1])).ToList();
                DeezerReport_2020 obj = new DeezerReport_2020();
                obj.Month = s[1].ToString();
                obj.Year = s[0].ToString();
                obj.UPC = report[0].UPC;

                double payable = 0;
                foreach (var y in tmp)
                {
                    payable = payable + (double)y.Royalties;
                }

                obj.Royalties = (decimal)payable;

                listNew.Add(obj);
            }

            return listNew;
        }
        List<GoogleMusicReport_2020> GooglePlayReport(string upc)
        {
            var report = db.GoogleMusicReport_2020.Where(a => a.UPC.Equals(upc)).ToList();
            List<string> yearsMonths = new List<string>();
            foreach (var x in report)
            {
                if (!yearsMonths.Contains(x.Year + "$" + x.Month))
                {
                    yearsMonths.Add(x.Year + "$" + x.Month);
                }
            }

            List<GoogleMusicReport_2020> listNew = new List<GoogleMusicReport_2020>();
            foreach (var x in yearsMonths)
            {
                List<String> s = new List<String>(x.Split("$".ToCharArray()));

                var tmp = report.Where(a => a.Year.Equals(s[0]) && a.Month.Equals(s[1])).ToList();
                GoogleMusicReport_2020 obj = new GoogleMusicReport_2020();
                obj.Month = s[1].ToString();
                obj.Year = s[0].ToString();
                obj.UPC = report[0].UPC;

                double payable = 0;
                foreach (var y in tmp)
                {
                    payable = payable + (double)y.EUR_Amount;
                }

                obj.EUR_Amount = (decimal)payable;

                listNew.Add(obj);
            }

            return listNew;
        }
        List<SoundCloudReport_2020> SoundCloudReport(string upc)
        {
            var report = db.SoundCloud.Where(a => a.UPC.Equals(upc)).ToList();
            List<string> yearsMonths = new List<string>();
            foreach (var x in report)
            {
                if (!yearsMonths.Contains(x.Year + "$" + x.Month))
                {
                    yearsMonths.Add(x.Year + "$" + x.Month);
                }
            }

            List<SoundCloudReport_2020> listNew = new List<SoundCloudReport_2020>();
            foreach (var x in yearsMonths)
            {
                List<String> s = new List<String>(x.Split("$".ToCharArray()));

                var tmp = report.Where(a => a.Year.Equals(s[0]) && a.Month.Equals(s[1])).ToList();
                SoundCloudReport_2020 obj = new SoundCloudReport_2020();
                obj.Month = s[1].ToString();
                obj.Year = s[0].ToString();
                obj.UPC = report[0].UPC;

                double payable = 0;
                foreach (var y in tmp)
                {
                    payable = payable + (double)y.Total_Revenue;
                }

                obj.Total_Revenue = (decimal)payable;

                listNew.Add(obj);
            }

            return listNew;
        }

        public ActionResult ReportDetails(string month, string year, string upc, string reportName)
        {
            if(reportName == null || month == null || year==null || upc == null)
                return RedirectToAction("Index", "MusicReleases", new { id = 0 });

            if (reportName.Equals("Spotify"))
            {
                var finalList = db.SpotifyReportFull.Where(a => a.Month.Equals(month) && a.Year.Equals(year) && a.UPC.Equals(upc)).ToList();
                ViewBag.Month = finalList[0].Month;
                ViewBag.Year = finalList[0].Year;
                ViewBag.UPC = finalList[0].UPC;
                ViewBag.reportCheck = "Spotify";
                ViewBag.report = finalList;
            }
            else if (reportName.Equals("Deezers"))
            {
                var finalList = db.Deezers.Where(a => a.Month.Equals(month) && a.Year.Equals(year) && a.UPC.Equals(upc)).ToList();
                ViewBag.reportCheck = "Deezers";
                ViewBag.Month = finalList[0].Month;
                ViewBag.Year = finalList[0].Year;
                ViewBag.UPC = finalList[0].UPC;
                ViewBag.report = finalList;
            }
            else if (reportName.Equals("GooglePlay"))
            {
                var finalList = db.Google_Play.Where(a => a.Month.Equals(month) && a.Year.Equals(year) && a.UPC.Equals(upc)).ToList();
                ViewBag.reportCheck = "GooglePlay";
                ViewBag.Month = finalList[0].Month;
                ViewBag.Year = finalList[0].Year;
                ViewBag.UPC = finalList[0].UPC;
                ViewBag.report = finalList;
            }
            else if (reportName.Equals("SoundCloud"))
            {
                var finalList = db.SoundCloud.Where(a => a.Month.Equals(month) && a.Year.Equals(year) && a.UPC.Equals(upc)).ToList();
                ViewBag.reportCheck = "SoundCloud";
                ViewBag.Month = finalList[0].Month;
                ViewBag.Year = finalList[0].Year;
                ViewBag.UPC = finalList[0].UPC;
                ViewBag.report = finalList;
            }
            var userId = User.Identity.GetUserId();
            var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
            ViewBag.UserShare = user.share;
            return View();
        }

        public ActionResult DownloadReport(string month, string year, string upc, string reportName)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
            if (reportName == "Spotify")
            {
                var finalList = db.SpotifyReportFull.Where(a => a.Month.Equals(month) && a.Year.Equals(year) && a.UPC.Equals(upc)).ToList();

                /*Creating iTextSharp’s Document & Writer*/
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();


                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                float[] columnWidths = new float[] { 120f, 270f, 110f, 120f, 50f, 90f, 90f };
                table.SetWidthPercentage(columnWidths, PageSize.A4.Rotate());

                table.AddCell("ISRC");
                table.AddCell("URI");
                table.AddCell("Country");
                table.AddCell("Track Name");
                table.AddCell("Quantity");
                table.AddCell("Revenue");
                table.AddCell("Share");

                var pdfImage = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
                pdfImage.Alignment = Element.ALIGN_MIDDLE;


                pdfDoc.Add(pdfImage);
                var para12 = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Kaveldunsvägen 21, 80636 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
                para12.Alignment = Element.ALIGN_MIDDLE;
                pdfDoc.Add(para12);

                if(finalList.Count > 0)
                {
                    Chunk chunk4 = new Chunk("Spotify Report\n\nUPC:                          "+upc+"\nArtist Name:             " + finalList[0].Artist_Name + "\nComposer Name:     " + finalList[0].Composer_Name + "\nAlbum Name:           " + finalList[0].Album_Name +"\nLabel:                       "+ finalList[0].Label + "\nMonth/Year:              " + month+"/"+year);
                    chunk4.Font = FontFactory.GetFont("Verdana", 11, Font.BOLD, BaseColor.BLACK);
                    pdfDoc.Add(chunk4);
                }

                foreach (var x in finalList)
                {
                    table.AddCell(new Phrase(0, x.ISRC, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    table.AddCell(new Phrase(0, x.URI, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, x.Country, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, x.TrackName, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, x.Quantity.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, "$" + Math.Round(x.USD_Payable, 2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, "$" + Math.Round((x.USD_Payable / 100) * Int32.Parse(user.share), 2), FontFactory.GetFont(FontFactory.COURIER, 10)));
                }


                table.AddCell("Total");
                table.AddCell("");
                table.AddCell("");
                table.AddCell("");
                table.AddCell("");
                table.AddCell("$" + Math.Round(finalList.Sum(a => a.USD_Payable), 2).ToString());
                table.AddCell("$" + Math.Round((finalList.Sum(a => a.USD_Payable) / 100) * Int32.Parse(user.share), 2));
                pdfDoc.Add(table);

                /*Creating the PDF file*/
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Spotify-Report.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
                /*End*/

                return RedirectToAction("Index", "MusicReleases", new { id = 0 });
            }
            else if (reportName == "Deezers")
            {
                var finalList = db.Deezers.Where(a => a.Month.Equals(month) && a.Year.Equals(year) && a.UPC.Equals(upc)).ToList();

                /*Creating iTextSharp’s Document & Writer*/
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();


                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                float[] columnWidths = new float[] { 120f, 270f, 110f, 120f, 50f, 90f, 90f };
                table.SetWidthPercentage(columnWidths, PageSize.A4.Rotate());

                table.AddCell("ISRC");
                table.AddCell("Artist");
                table.AddCell("Country");
                table.AddCell("Title");
                table.AddCell("Quantity");
                table.AddCell("Revenue");
                table.AddCell("Share");

                var pdfImage = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
                pdfImage.Alignment = Element.ALIGN_MIDDLE;


                pdfDoc.Add(pdfImage);
                var para12 = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Kaveldunsvägen 21, 80636 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
                para12.Alignment = Element.ALIGN_MIDDLE;
                pdfDoc.Add(para12);

                if (finalList.Count > 0)
                {
                    Chunk chunk4 = new Chunk("Deezer Report\n\nUPC:                          " + upc + "\nAlbum Name:           " + finalList[0].Album + "\nLabel:                       " + finalList[0].Label + "\nMonth/Year:              " + month + "/" + year);
                    chunk4.Font = FontFactory.GetFont("Verdana", 11, Font.BOLD, BaseColor.BLACK);
                    pdfDoc.Add(chunk4);
                }

                foreach (var x in finalList)
                {
                    table.AddCell(new Phrase(0, x.ISRC, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    table.AddCell(new Phrase(0, x.Artist, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, x.Country, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, x.Title, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, x.Nb_of_plays.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, "$" + Math.Round(x.Royalties, 2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, "$" + Math.Round((x.Royalties / 100) * Int32.Parse(user.share), 2), FontFactory.GetFont(FontFactory.COURIER, 10)));
                }


                table.AddCell("Total");
                table.AddCell("");
                table.AddCell("");
                table.AddCell("");
                table.AddCell("");
                table.AddCell("$" + Math.Round(finalList.Sum(a => a.Royalties), 2).ToString());
                table.AddCell("$" + Math.Round((finalList.Sum(a => a.Royalties) / 100) * Int32.Parse(user.share), 2));
                pdfDoc.Add(table);

                /*Creating the PDF file*/
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=Deezer-Report.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
                /*End*/

                return RedirectToAction("Index", "MusicReleases", new { id = 0 });
            }
            else if (reportName == "GooglePlay")
            {
                var finalList = db.Google_Play.Where(a => a.Month.Equals(month) && a.Year.Equals(year) && a.UPC.Equals(upc)).ToList();

                /*Creating iTextSharp’s Document & Writer*/
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();


                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                float[] columnWidths = new float[] { 120f, 270f, 110f, 120f, 50f, 90f, 90f };
                table.SetWidthPercentage(columnWidths, PageSize.A4.Rotate());

                table.AddCell("ISRC");
                table.AddCell("Product Title");
                table.AddCell("Country");
                table.AddCell("Container Title");
                table.AddCell("Quantity");
                table.AddCell("Revenue");
                table.AddCell("Share");

                var pdfImage = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
                pdfImage.Alignment = Element.ALIGN_MIDDLE;


                pdfDoc.Add(pdfImage);
                var para12 = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Kaveldunsvägen 21, 80636 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
                para12.Alignment = Element.ALIGN_MIDDLE;
                pdfDoc.Add(para12);

                if (finalList.Count > 0)
                {
                    Chunk chunk4 = new Chunk("Google Play Report\n\nUPC:                          " + upc + "\nLabel:                       " + finalList[0].Label + "\nMonth/Year:              " + month + "/" + year);
                    chunk4.Font = FontFactory.GetFont("Verdana", 11, Font.BOLD, BaseColor.BLACK);
                    pdfDoc.Add(chunk4);
                }

                foreach (var x in finalList)
                {
                    table.AddCell(new Phrase(0, x.ISRC, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    table.AddCell(new Phrase(0, x.Product_Title, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, x.Consumer_Country, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, x.Container_Title, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, x.Total_Plays.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, "$" + Math.Round(x.EUR_Amount, 2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, "$" + Math.Round((x.EUR_Amount / 100) * Int32.Parse(user.share), 2), FontFactory.GetFont(FontFactory.COURIER, 10)));
                }


                table.AddCell("Total");
                table.AddCell("");
                table.AddCell("");
                table.AddCell("");
                table.AddCell("");
                table.AddCell("$" + Math.Round(finalList.Sum(a => a.EUR_Amount), 2).ToString());
                table.AddCell("$" + Math.Round((finalList.Sum(a => a.EUR_Amount) / 100) * Int32.Parse(user.share), 2));
                pdfDoc.Add(table);

                /*Creating the PDF file*/
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=GooglePlay-Report.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
                /*End*/

                return RedirectToAction("Index", "MusicReleases", new { id = 0 });
            }
            else if (reportName == "SoundCloud")
            {
                var finalList = db.SoundCloud.Where(a => a.Month.Equals(month) && a.Year.Equals(year) && a.UPC.Equals(upc)).ToList();
                /*Creating iTextSharp’s Document & Writer*/
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();


                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;

                float[] columnWidths = new float[] { 120f, 300f, 110f, 140f, 90f, 90f };
                table.SetWidthPercentage(columnWidths, PageSize.A4.Rotate());

                table.AddCell("ISRC");
                table.AddCell("Artist Name");
                table.AddCell("Country");
                table.AddCell("Track Name");
                table.AddCell("Quantity");
                table.AddCell("Revenue");
                table.AddCell("Share");

                var pdfImage = Image.GetInstance("https://tpdigital.blob.core.windows.net/logo/Logo.JPG");
                pdfImage.Alignment = Element.ALIGN_MIDDLE;


                pdfDoc.Add(pdfImage);
                var para12 = new Paragraph("+46(0)73 - 546 64 61 \n http://www.tempodigital.org \n TP Digital AB \n c/o TEMPO DIGITAL  \n Kaveldunsvägen 21, 80636 \n Gävle SWEDEN \n \n \n", FontFactory.GetFont("Verdana", 10, Font.BOLD, BaseColor.BLACK));
                para12.Alignment = Element.ALIGN_MIDDLE;
                pdfDoc.Add(para12);

                if (finalList.Count > 0)
                {
                    Chunk chunk4 = new Chunk("Sound Cloud Report\n\nUPC:                          " + upc + "\nLabel:                       " + finalList[0].LabelName + "\nMonth/Year:              " + month + "/" + year);
                    chunk4.Font = FontFactory.GetFont("Verdana", 11, Font.BOLD, BaseColor.BLACK);
                    pdfDoc.Add(chunk4);
                }

                foreach (var x in finalList)
                {
                    table.AddCell(new Phrase(0, x.ISRC, FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                    table.AddCell(new Phrase(0, x.Artist_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, x.Territory, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, x.Track_Name, FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, x.Total_Plays.ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, "$" + Math.Round(x.Total_Revenue, 2).ToString(), FontFactory.GetFont(FontFactory.COURIER, 10)));
                    table.AddCell(new Phrase(0, "$" + Math.Round((x.Total_Revenue / 100) * Int32.Parse(user.share), 2), FontFactory.GetFont(FontFactory.COURIER, 10)));
                }


                table.AddCell("Total");
                table.AddCell("");
                table.AddCell("");
                table.AddCell("");
                table.AddCell("");
                table.AddCell("$" + Math.Round(finalList.Sum(a => a.Total_Revenue), 2).ToString());
                table.AddCell("$" + Math.Round((finalList.Sum(a => a.Total_Revenue) / 100) * Int32.Parse(user.share), 2));
                pdfDoc.Add(table);

                /*Creating the PDF file*/
                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=SoundCloud-Report.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
                /*End*/

                return RedirectToAction("Index", "MusicReleases", new { id = 0 });
            }

            return File("NotFound.txt", "text/plain");
        }

        public JsonResult UploadFiles(string labelName, string UPC)
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


            string ServerSavePath = null;
            string dateToday = DateTime.Now.ToString("yyyy-MM-dd");

            if (labelName != "")
            {
                ServerSavePath = "musicdistribution/" + dateToday + "/" + labelName + "/" + UPC + "/";
            }
            else
            {
                ServerSavePath = "musicdistribution/" + dateToday + "/" + labelName + "/";
            }

            string FileName = "";
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";    
                //string filename = Path.GetFileName(Request.Files[i].FileName);    

                HttpPostedFileBase file = files[i];
                string fname;

                // Checking for Internet Explorer    
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                    FileName = file.FileName;
                }

                string SongPath = null;
                int sIndex = i + 1;
                if (sIndex < 10)
                {
                    if (labelName != "")
                        SongPath = ServerSavePath + UPC + "_01_0" + sIndex + ".wav";
                    else
                        SongPath = ServerSavePath + UPC + "_01_" + sIndex + ".wav";
                }
                else
                {
                    if (labelName != "")
                        SongPath = ServerSavePath + UPC + "_01_0" + sIndex + ".wav";
                    else
                        SongPath = ServerSavePath + UPC + "_01_" + sIndex + ".wav";
                }

                // Get the complete folder path and store the file inside it.    
                //fname = Path.Combine(Server.MapPath("~/assets/"), fname);
                //file.SaveAs(fname);
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(SongPath);
                cloudBlockBlob.Properties.ContentType = file.ContentType;

                //await cloudBlockBlob.UploadFromStreamAsync(file.InputStream);
                cloudBlockBlob.UploadFromStream(file.InputStream);
            }
            return Json(FileName, JsonRequestBehavior.AllowGet);
        }

        // GET: MusicReleases/Create
        public ActionResult Create()
        {
            string userId = User.Identity.GetUserId();
            var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
            if (user.LabelName != null)
                TempData["LabelName"] = user.LabelName;
            else
                TempData["LabelName"] = "";

            //var isrc = db.UPC_ISRCs.ToList();
            //foreach(var x in isrc)
            //{
            //    if (x.ISRC_status != null && x.ISRC_status.Equals("InProgress"))
            //        x.ISRC_status = null;

            //    if (x.UPC_status != null && x.UPC_status.Equals("InProgress"))
            //        x.UPC_status = null;
            //}
            //db.SaveChanges();

            var UPC = db.UPC_Codes.ToList();
            foreach (var x in UPC)
            {
                if (x.UPC_status != null && x.UPC_status.Equals("InProgress"))
                    x.UPC_status = null;
            }
            db.SaveChanges();


            var ISRC = db.ISRC_Codes.ToList();
            foreach (var x in ISRC)
            {
                if (x.ISRC_status != null && x.ISRC_status.Equals("InProgress"))
                    x.ISRC_status = null;
            }
            db.SaveChanges();



            if (user.LabelName == "")
                ViewBag.checkLabel = "Artist";
            else
                ViewBag.checkLabel = "Label";

            var freeEMail = new List<string>();
            freeEMail.Add(ConfigurationManager.AppSettings["Email1"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email2"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email3"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email4"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email5"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email6"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email7"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email8"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email9"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email10"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email11"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email12"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email13"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email14"]);
            freeEMail.Add(ConfigurationManager.AppSettings["Email15"]);

            ViewBag.FreeEmails = freeEMail;
            ViewBag.UserEmail = user.Email;
           
            return View();
        }

        public JsonResult GetAvailableUPC()
        {
            var availableUPC = db.UPC_Codes.Where(a => a.UPC_status == null).FirstOrDefault();
            availableUPC.UPC_status = "InProgress";
            db.SaveChanges();
            return this.Json(availableUPC.UPC, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAvailableISRC()
        {
            var availableUPC = db.ISRC_Codes.Where(a => a.ISRC_status == null).FirstOrDefault();
            availableUPC.ISRC_status = "InProgress";
            db.SaveChanges();
            return this.Json(availableUPC.ISRC, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Status(string upc)
        {
            if(upc == null)
            {
                return RedirectToAction("index");
            }

            try
            {
                string url = String.Format("https://api.audiosalad.com/fetch.php?k=19E1170F02FCED03938BE12B241E09034BDDD5E397902460B43F6197C40260A0&g_profile=tempodigital.audiosalad.com&upc={0}&releaseDeliveries", upc);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    ViewBag.StatusDetails = reader.ReadToEnd().ToString();
                }

                ViewBag.Status = true;
            }
            catch
            {
                ViewBag.Status = false;
            }
            

            return View();
        }
        
        // POST: MusicReleases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MusicReleaseId,ReleaseName,MainArtist,ReleaseVersion,RecordingLocation,Language,CopyrightYear,ReleaseDate,OriginalReleaseDate,UPCEAN,LabelName,CInfo,CYear,PInfo,PYear,CopyryghtHolder,Publisher")] MusicRelease musicRelease,
            string ReleaseFormat, string releaseName, string mainArtist, string releaseVersion, string Advisory, string Category, string Territory, string stripeEmail,string IS_VAR, string[] VAR, string [] composer_song, string[] writer_song, string [] arranger_song, string [] iswc_song, string paymentMethod, string priceCharge, string stripeToken, 
            string[] FeaturedArtist, string[] Title, string[] ISrc, string Explicit, string [] PartName__New, string [] PartRole__New, string[] PartType__New, string[] PartInstrument__New, string[] PartIPICode__New, string[] PartIPNCode__New,
            string[] Store, HttpPostedFileBase coverImage, string country, string needUPC, string [] artist_song, string [] trackversion_song, string [] cinfo_song, string [] cyear_song, string [] pinfo_song, string [] pyear_song, string [] copyright_song, string [] participent_song)
        {
            //try
            //{
                var UPC = db.UPC_Codes.Where(x=>x.UPC_status != null && x.UPC_status.Equals("InProgress")).ToList();
                foreach (var x in UPC)
                {
                    if (x.UPC.Equals(musicRelease.UPCEAN))
                        x.UPC_status = "Used";
                }
                db.SaveChanges();

                var ISRC = db.ISRC_Codes.Where(x => x.ISRC_status != null && x.ISRC_status.Equals("InProgress")).ToList();

                musicRelease.ReleaseName = releaseName;
                musicRelease.MainArtist = mainArtist;
                musicRelease.ReleaseVersion = releaseVersion;
                musicRelease.ReleaseFormat = ReleaseFormat;
                musicRelease.ReleaseAdvisory = Advisory;
                musicRelease.ReleaseCategory = Category;
                musicRelease.TerritoryAvailability = Territory;
                musicRelease.RecordingLocation = country;
                musicRelease.Itune_Track_Price = Request.Form["itunes_track_price"];
                musicRelease.Itune_Release_Price = Request.Form["itunes_release_price"];

                if (Request.Form.AllKeys.Contains("artist_to_all_songs"))
                {
                    musicRelease.artistToAllSongs = true;
                }

                if (Request.Form.AllKeys.Contains("SetToAllTracks"))
                {
                    musicRelease.copyRightToAllSongs = true;
                }

                string dateToday = DateTime.Now.ToString("yyyy-MM-dd");

                if (paymentMethod == "Stripe")
                {
                    var customers = new CustomerService();
                    var charges = new ChargeService();

                    var customer = customers.Create(new CustomerCreateOptions
                    {
                        Email = stripeEmail,
                        SourceToken = stripeToken
                    });

                    var price = float.Parse(priceCharge, CultureInfo.InvariantCulture.NumberFormat) * 100;
                    int finalPrice = (int)price;

                    var charge = charges.Create(new ChargeCreateOptions
                    {
                        Amount = finalPrice,
                        Description = "Music Release",
                        Currency = "USD",
                        CustomerId = customer.Id
                    });
                }

                
                if (Request.Form["needUPC"] != null)
                    musicRelease.needUPC = true;
                else
                    musicRelease.needUPC = false;

                if (IS_VAR == "on")
                {
                    musicRelease.isVA = true;
                }
                else { 
                    musicRelease.isVA = false;
                }
                
                musicRelease.currentDateTime = DateTime.Now;
                musicRelease.Sales_Start_Date = DateTime.Now;
                musicRelease.UserId = User.Identity.GetUserId();
                db.MusicReleases.Add(musicRelease);
                db.SaveChanges();

                var mainArtist1 = new Artist();
                mainArtist1.ArtistName = musicRelease.MainArtist;
                mainArtist1.MusicReleaseId = musicRelease.MusicReleaseId;
                db.Artists.Add(mainArtist1);
                db.SaveChanges();

                if (PartName__New != null && PartType__New != null && PartRole__New != null && PartInstrument__New != null && PartIPICode__New != null && PartIPNCode__New != null)
                {
                    for (int i = 0; i < PartName__New.Length; i++)
                    {
                        var participent = new Participants();
                        participent.MusicReleaseId = musicRelease.MusicReleaseId;
                        participent.ParticipantName = PartName__New[i];
                        participent.RoleType = PartType__New[i];
                        participent.ParticipantRole = PartRole__New[i];
                        participent.Instrument = PartInstrument__New[i];
                        participent.IPICode = PartIPICode__New[i];
                        participent.IPNCode = PartIPNCode__New[i];

                        db.Participants.Add(participent);
                        db.SaveChanges();
                    }
                }

                if(Store != null)
                {
                    foreach(var x in Store)
                    {
                        var store = new Stores();
                        store.MusicReleaseId = musicRelease.MusicReleaseId;
                        store.StoreName = x;
                        store.isSelected = true;

                        db.Stores.Add(store);
                        db.SaveChanges();
                    }
                }

                //string ServerSavePath = "https://ddex-distribution.s3.eu-north-1.amazonaws.com/" + "musicdistribution/" + dateToday + "/" + musicRelease.UPCEAN ;
                string ServerSavePath = "https://ddex-distribution.s3.eu-north-1.amazonaws.com/" + musicRelease.UPCEAN ;
                string userId = User.Identity.GetUserId();
                var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
                if (Title.Length > 0)
                {

                    StringBuilder content = new StringBuilder();
                    
                    string head = "Release Name, Main Artist, Release Verison, Country, Release Format, Advisory, Language, Year, Release Date," +
                        "Original Release Date, UPC, Label Name, Category/Genre, Territory, (C)Info, (C)Year, (P)Info, (P)Year, Copyright Holder, Publisher," +
                        "Main Artist Name, Type, Role Type, Instrument, IPI Code, IPN Code, Track Title, ISRC, Composer Name, Writer Name, Arranger Name, ISWC, Stores";
                    content.AppendLine(head);
                    for (int i =0; i < Title.Length; i++)
                    {
                        if (Title[i] != null)
                        {
                            var sIndex = i + 1;
                            var song = new Song();
                            

                            foreach (var x in ISRC)
                            {
                                if (x.ISRC.Equals(ISrc[i]))
                                    x.ISRC_status = "Used";
                            }
                            db.SaveChanges();
                            
                            song.SongPath =  ServerSavePath + "/" + musicRelease.UPCEAN.ToString() + "_01_" + sIndex + ".wav";
                            song.SongTitle = Title[i];
                            song.isrc = ISrc[i];
                            song.MusicReleaseId = musicRelease.MusicReleaseId;
                            song.ArtistName = artist_song[i];
                            song.ISWC = iswc_song[i];
                            song.PInfo = pinfo_song[i];
                            song.PYear = pyear_song[i];
                            song.CInfo = cinfo_song[i];
                            song.CYear = cyear_song[i];
                            song.CopyRightHolder = copyright_song[i];
                            song.version = trackversion_song[i];

                            if (musicRelease.isVA)
                            {
                                song.ArtistName = VAR[i];
                            }

                            db.Songs.Add(song);
                            db.SaveChanges();

                            var last_song = db.Songs.ToList().LastOrDefault();
                            if(last_song != null)
                            {
                                if(participent_song != null)
                                {
                                    if(participent_song.Length > 0)
                                    {
                                        var part = participent_song[i];
                                        if (part.Contains('$'))
                                        {
                                            var participents = part.Split('$');
                                            for (int p = 0; p < participents.Length - 1; p++)
                                            {
                                                var part_data = participents[p].Split('|');
                                                var new_song_participent = new SongParticipent();
                                                new_song_participent.SongId = last_song.SongId;
                                                new_song_participent.ParticipantName = part_data[0];
                                                new_song_participent.RoleType = part_data[1];
                                                new_song_participent.ParticipantRole = part_data[2];
                                                new_song_participent.Instrument = part_data[3];
                                                new_song_participent.IPICode = part_data[4];
                                                new_song_participent.IPNCode = part_data[5];

                                                db.SongParticipent.Add(new_song_participent);
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                }
                                
                            }

                            
                            string upcean = musicRelease.UPCEAN.ToString();

                            head = "Release Name, Main Artist, Release Verison, Country, Release Format, Advisory, Language, Year, Release Date," +
                            "Original Release Date, UPC, Label Name, Category/Genre, Territory, (C)Info, (C)Year, (P)Info, (P)Year, Copyright Holder, Publisher," +
                            "Main Artist Name, Type, Role Type, Instrument, IPI Code, IPN Code, Track Title, ISRC, Artist Name, ISWC, " +
                            "PInfo, PYear, CInfo, CYear, Copy Right Holder, Version, Song Participents ";

                            
                            string line = "";
                            //if (i == 0 && PartName_New != null && PartType_New != null && PartRole_New != null && PartInstrument_New != null && PartIPICode_New != null && PartIPNCode_New != null && Store != null)
                            //{
                            //    line = "" + musicRelease.ReleaseName + "," + musicRelease.MainArtist + "," + musicRelease.ReleaseVersion + "," +
                            //    "" + musicRelease.RecordingLocation + "," +
                            //    "" + musicRelease.ReleaseFormat + "," + musicRelease.ReleaseAdvisory + "," + musicRelease.Language + "," +
                            //    "" + musicRelease.CopyrightYear + "," + musicRelease.ReleaseDate + "," + musicRelease.OriginalReleaseDate + "," +
                            //    "" + musicRelease.UPCEAN + "," + musicRelease.LabelName + "," + musicRelease.ReleaseCategory + "," +
                            //    "" + musicRelease.TerritoryAvailability + "," + musicRelease.CInfo + "," + musicRelease.CYear + "," +
                            //    "" + musicRelease.PInfo + "," + musicRelease.PYear + "," + musicRelease.CopyryghtHolder + "," +
                            //    "" + musicRelease.Publisher + "," + string.Join("| ", PartName_New) + "," + string.Join("| ", PartType_New) + "," + 
                            //    string.Join("| ", PartRole_New) + "," + string.Join("| ", PartInstrument_New) + "," + string.Join("| ", PartIPICode_New) + "," + 
                            //    string.Join("| ", PartIPNCode_New) + "," + song.SongTitle + "," + song.isrc + "," + song.ArtistName + "," +
                            //    "" + song.ISWC + "," + song.PInfo + "," + song.PYear + "," + song.CYear + "," + song.CYear + "," + 
                            //    song.CopyRightHolder + "," + song.version + "," + String.Join(" || ", participent_song[i].Split('$')) + "," + string.Join("| ", Store);
                            //}
                            //else if (i == 0 && PartName_New != null && PartType_New != null && PartRole_New != null && PartInstrument_New != null && PartIPICode_New != null && PartIPNCode_New != null && Store == null)
                            //{
                            //    line = "" + musicRelease.ReleaseName + "," + musicRelease.MainArtist + "," + musicRelease.ReleaseVersion + "," +
                            //    "" + musicRelease.RecordingLocation + "," +
                            //    "" + musicRelease.ReleaseFormat + "," + musicRelease.ReleaseAdvisory + "," + musicRelease.Language + "," +
                            //    "" + musicRelease.CopyrightYear + "," + musicRelease.ReleaseDate + "," + musicRelease.OriginalReleaseDate + "," +
                            //    "" + musicRelease.UPCEAN + "," + musicRelease.LabelName + "," + musicRelease.ReleaseCategory + "," +
                            //    "" + musicRelease.TerritoryAvailability + "," + musicRelease.CInfo + "," + musicRelease.CYear + "," +
                            //    "" + musicRelease.PInfo + "," + musicRelease.PYear + "," + musicRelease.CopyryghtHolder + "," +
                            //    "" + musicRelease.Publisher + "," + string.Join("| ", PartName_New) + "," + string.Join("| ", PartType_New) + "," +
                            //    string.Join("| ", PartRole_New) + "," + string.Join("| ", PartInstrument_New) + "," + string.Join("| ", PartIPICode_New) + "," +
                            //    string.Join("| ", PartIPNCode_New) + "," + song.SongTitle + "," + song.isrc + "," + song.ArtistName + "," +
                            //    "" + song.ISWC + "," + song.PInfo + "," + song.PYear + "," + song.CYear + "," + song.CYear + "," +
                            //    song.CopyRightHolder + "," + song.version + "," + String.Join(" || ", participent_song[i].Split('$')) + ",";
                            //}
                            //else if(i == 0 && Store !=  null && PartName_New == null && PartType_New == null && PartRole_New == null && PartInstrument_New == null && PartIPICode_New == null && PartIPNCode_New == null)
                            //{
                            //    line = "" + musicRelease.ReleaseName + "," + musicRelease.MainArtist + "," + musicRelease.ReleaseVersion + "," +
                            //    "" + musicRelease.RecordingLocation + "," +
                            //    "" + musicRelease.ReleaseFormat + "," + musicRelease.ReleaseAdvisory + "," + musicRelease.Language + "," +
                            //    "" + musicRelease.CopyrightYear + "," + musicRelease.ReleaseDate + "," + musicRelease.OriginalReleaseDate + "," +
                            //    "" + musicRelease.UPCEAN + "," + musicRelease.LabelName + "," + musicRelease.ReleaseCategory + "," +
                            //    "" + musicRelease.TerritoryAvailability + "," + musicRelease.CInfo + "," + musicRelease.CYear + "," +
                            //    "" + musicRelease.PInfo + "," + musicRelease.PYear + "," + musicRelease.CopyryghtHolder + "," +
                            //    "" + musicRelease.Publisher + ",,,,,,," + song.SongTitle + "," + song.isrc + "," + song.ArtistName + "," +
                            //    "" + song.ISWC + "," + song.PInfo + "," + song.PYear + "," + song.CYear + "," + song.CYear + "," +
                            //    song.CopyRightHolder + "," + song.version + "," + String.Join(" || ", participent_song[i].Split('$')) + "," + string.Join("|", Store);
                            //}
                            //else
                            //{
                            //    line = "" + musicRelease.ReleaseName + "," + musicRelease.MainArtist + "," + musicRelease.ReleaseVersion + "," +
                            //    "" + musicRelease.RecordingLocation + "," +
                            //    "" + musicRelease.ReleaseFormat + "," + musicRelease.ReleaseAdvisory + "," + musicRelease.Language + "," +
                            //    "" + musicRelease.CopyrightYear + "," + musicRelease.ReleaseDate + "," + musicRelease.OriginalReleaseDate + "," +
                            //    "" + musicRelease.UPCEAN + "," + musicRelease.LabelName + "," + musicRelease.ReleaseCategory + "," +
                            //    "" + musicRelease.TerritoryAvailability + "," + musicRelease.CInfo + "," + musicRelease.CYear + "," +
                            //    "" + musicRelease.PInfo + "," + musicRelease.PYear + "," + musicRelease.CopyryghtHolder + "," +
                            //    "" + musicRelease.Publisher + ",,,,,,," + song.SongTitle + "," + song.isrc + "," + song.ArtistName + "," +
                            //    "" + song.ISWC + "," + song.PInfo + "," + song.PYear + "," + song.CYear + "," + song.CYear + "," +
                            //    song.CopyRightHolder + "," + song.version + String.Join(" || ", participent_song[i].Split('$')) + "";
                            //}

                            content.AppendLine(line); 
          
                        }
                    }
                    
                    //byte[] data = new ASCIIEncoding().GetBytes(content.ToString());
                    //MemoryStream stream = new MemoryStream(data);
                    //uploadToS3Bucket(stream, "musicdistribution/" + dateToday + "/" + musicRelease.UPCEAN + "/" + musicRelease.UPCEAN + ".csv", "application/vnd.ms-excel");


                    StringBuilder template = new StringBuilder();
                    template.AppendLine("PRODUCT BARCODE,SERVICES");
                    template.AppendFormat(""+musicRelease.UPCEAN.ToString()+",YAT|AWM|AWM|SPO|KKB|AKA|DEE|SAA|YCI|FBK|MXM|FBL|DIG|GGL|GNT|PDX|AAP|NEE|SCD|UMA");
                    
                    byte[] data1 = new ASCIIEncoding().GetBytes(template.ToString());
                    MemoryStream stream1 = new MemoryStream(data1);
                    //uploadToS3Bucket(stream1, "musicdistribution/" + dateToday + "/" + musicRelease.UPCEAN + "/" + "CI-Delivery Template.csv", "application/vnd.ms-excel");
                    uploadToS3Bucket(stream1, musicRelease.UPCEAN + "/" + "CI-Delivery Template.csv", "application/vnd.ms-excel");

                }
                if (coverImage != null)
                {
                    byte[] imageByte = null;
                    BinaryReader rdr = new BinaryReader(coverImage.InputStream);
                    imageByte = rdr.ReadBytes((int)coverImage.ContentLength);
                    MemoryStream stream1 = new MemoryStream(imageByte);


                    //uploadToS3Bucket(stream1, "musicdistribution/" + dateToday + "/" + musicRelease.UPCEAN + "/" + musicRelease.UPCEAN + ".jpg", coverImage.ContentType);
                    uploadToS3Bucket(stream1, musicRelease.UPCEAN + "/" + musicRelease.UPCEAN + ".jpg", coverImage.ContentType);
                    musicRelease.CoverImage = ServerSavePath + "/" + musicRelease.UPCEAN + ".jpg";
      

                    var ReleaseLog = new UserLogs();
                    ReleaseLog.DateTime = DateTime.Now.Date.ToString("MM/dd/yyyy");
                    ReleaseLog.UserEmail = user.Email;
                    ReleaseLog.UserID = user.Id;
                    ReleaseLog.UserName = user.FirstName + " " + user.LastName;
                    ReleaseLog.ActionPerformed = "Release Created";
                    ReleaseLog.ClientIP = GetIPAddress();
                    db.UserLogs.Add(ReleaseLog);

                    db.SaveChanges();


                    if (musicRelease.ReleaseFormat.Equals("Single"))
                    {  
                        var doc = Single(musicRelease, "add");
                        //byte[] data4 = new ASCIIEncoding().GetBytes(doc.OuterXml);
                        //MemoryStream stream4 = new MemoryStream(data4);
                        //uploadToS3Bucket(stream4, musicRelease.UPCEAN + "/" + musicRelease.UPCEAN + ".xml", "application/xml");

                        //string targetFolder = Server.MapPath("~/assets");
                        //string targetPath = Path.Combine(targetFolder, musicRelease.UPCEAN + ".xml");
                        string targetPath = Path.Combine(Server.MapPath("~/assets"), musicRelease.UPCEAN + ".xml");
                        doc.Save(targetPath);

                        sendFileToS3_folder(targetPath, musicRelease.UPCEAN + "/" + musicRelease.UPCEAN + ".xml");

                        string fullPath = targetPath;
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    else
                    {
                        var doc = EP(musicRelease, "add");
                        //byte[] data4 = new ASCIIEncoding().GetBytes(doc.OuterXml);
                        //MemoryStream stream4 = new MemoryStream(data4);
                        //uploadToS3Bucket(stream4, musicRelease.UPCEAN + "/" + musicRelease.UPCEAN + ".xml", "application/xml");

                        
                        string targetPath = Path.Combine(Server.MapPath("~/assets"), musicRelease.UPCEAN + ".xml");
                        doc.Save(targetPath);

                        sendFileToS3_folder(targetPath, musicRelease.UPCEAN + "/" + musicRelease.UPCEAN + ".xml");

                        string fullPath = targetPath;
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }


                    // send mail to distribution
                    var senderMail1 = new MailAddress("support@tempodigital.org", "Tempo Digital Team");
                    var receiverMial1 = new MailAddress("distribution@tempodigital.org", "Receiver");
                    var smtp1 = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderMail1.Address, ConfigurationManager.AppSettings["GmailPasword"])
                    };

                    var message1 = new MailMessage(senderMail1, receiverMial1);
                    message1.Subject = "Music Release is Created.";
                    message1.Body = "Hi,\nRelease was successfully Created.\n" +
                    "\nID:" + musicRelease.MusicReleaseId.ToString() + "\n" +
                    "Details are Here: " + ServerSavePath + "/" + musicRelease.UPCEAN + ".xml" + "\n" +
                    "\nThank you.\n\nSender,\nTempo Digital Team.";
                    
                    smtp1.Send(message1);


                    if (paymentMethod == "Stripe" || paymentMethod == "Free")
                    {
                        musicRelease.PaymentStaus = "Paid";
                        db.SaveChanges();
                        // email sending
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
                        message.Body = "Hi,\nProducts successfully delivered in this order are now visible in the View Discograpghy section of distribution dashboard.\n\nFor any further queries please email support@tempodigital.org\n\nThank you.\n\nSender,\nTempo Digital Team.";

                        smtp.Send(message);
                        //Here I have to insert method to insert
                        //data into Stores table in my database

                        return RedirectToAction("Index", "MusicReleases", new { id = 0 });
                    }
                    else
                    {
                        TempData["ReleaseID"] = musicRelease.MusicReleaseId;
                        TempData["ChargePrice"] = priceCharge;
                        return RedirectToAction("PaymentWithPaypal", "Home");
                    }   
                }

                return RedirectToAction("Index", "MusicReleases", new { id = 0 });
            //}
            //catch (Exception ex)
            //{
            //    throw new ApplicationException(ex.Message);
            //}
        }


        public static void sendFileToS3_folder(string fileName, string FileDestPath)
        {
            try
            {
                AmazonS3Client client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
                PutObjectRequest request = new PutObjectRequest();
                request.BucketName = "ddex-distribution";
                request.FilePath = fileName;
                request.Key = FileDestPath;
                request.ContentType = MimeMapping.GetMimeMapping(fileName);
                PutObjectResponse response = client.PutObject(request);
            }
            catch (Exception ex)
            {
                // use a logger and handle it
            }
        }

        public void uploadToS3Bucket(MemoryStream File, string FileDestPath, string FileType)
        {
            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            string bucketName = "ddex-distribution";
            try
            {
                PutObjectRequest putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = FileDestPath,
                    InputStream = File,
                    ContentType = FileType
                };

                PutObjectResponse response = client.PutObject(putRequest);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + amazonS3Exception.Message);
                }
            }
        }

       

        public static void SetAttributeValue(XmlNode xmlne, string name, string value)
        {
            XmlAttribute node = xmlne.OwnerDocument.CreateAttribute(name);
            xmlne.Attributes.Append(node);
            node.Value = value;
        }

        public XmlDocument Single(MusicRelease music, string action)
        {
            var songs = db.Songs.Where(a => a.MusicReleaseId == music.MusicReleaseId).ToList();
            var participents = db.Participants.Where(a => a.MusicReleaseId == music.MusicReleaseId).ToList();

            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", "");
            doc.AppendChild(docNode);

            XmlNode release = doc.CreateElement("release");
            doc.AppendChild(release);

            //XmlNode node1 = doc.CreateNode(XmlNodeType.Element, "html", null);
            //release.AppendChild(node1);
            //SetAttributeValue(node1, "lang", "en");
            //SetAttributeValue(node1, "prefix", "op: http://media.facebook.com/op#");

            XmlNode node2 = doc.CreateNode(XmlNodeType.Element, "meta", null);
            release.AppendChild(node2);
            SetAttributeValue(node2, "charset", "utf-8");

            XmlNode Node = doc.CreateElement("schema_id");
            Node.AppendChild(doc.CreateTextNode("schema_id"));
            release.AppendChild(Node);

            Node = doc.CreateElement("distributor_name");
            Node.AppendChild(doc.CreateTextNode("Tempo Production"));
            release.AppendChild(Node);

            Node = doc.CreateElement("export_id");
            Node.AppendChild(doc.CreateTextNode(music.MusicReleaseId.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("export_time");
            Node.AppendChild(doc.CreateTextNode(music.currentDateTime.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("action");
            Node.AppendChild(doc.CreateTextNode(action));
            release.AppendChild(Node);

            Node = doc.CreateElement("upc_ean");
            Node.AppendChild(doc.CreateTextNode(music.UPCEAN.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("vendor_release_id");
            Node.AppendChild(doc.CreateTextNode(""));
            release.AppendChild(Node);

            Node = doc.CreateElement("catalog_id");
            Node.AppendChild(doc.CreateTextNode(""));
            release.AppendChild(Node);

            Node = doc.CreateElement("title");
            if (music.ReleaseName != null)
            {
                Node.AppendChild(doc.CreateTextNode(music.ReleaseName));
            }
            else
            {
                Node.AppendChild(doc.CreateTextNode(""));
            }
            release.AppendChild(Node);


            Node = doc.CreateElement("advisory");
            Node.AppendChild(doc.CreateTextNode(music.ReleaseAdvisory.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("metadata_language");
            Node.AppendChild(doc.CreateTextNode("English"));
            release.AppendChild(Node);

            Node = doc.CreateElement("audio_language");
            Node.AppendChild(doc.CreateTextNode("English"));
            release.AppendChild(Node);

            Node = doc.CreateElement("display_artist");
            //Node.AppendChild(doc.CreateTextNode(music.MainArtist.ToString()));
            Node.AppendChild(doc.CreateTextNode(music.MainArtist));
            release.AppendChild(Node);

            Node = doc.CreateElement("participant");
            var insideNode = doc.CreateElement("Role");
            insideNode.AppendChild(doc.CreateTextNode("Primary Artist"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("Name");
            insideNode.AppendChild(doc.CreateTextNode(music.MainArtist.ToString()));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("Primary");
            insideNode.AppendChild(doc.CreateTextNode("True"));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);

            foreach (var x in participents)
            {
                Node = doc.CreateElement("participant");
                insideNode = doc.CreateElement("Role");
                insideNode.AppendChild(doc.CreateTextNode(x.RoleType));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("Name");
                //insideNode.AppendChild(doc.CreateTextNode(x.ParticipantName.ToString()));
                insideNode.AppendChild(doc.CreateTextNode(x.ParticipantName));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("Primary");
                insideNode.AppendChild(doc.CreateTextNode("False"));
                Node.AppendChild(insideNode);
                release.AppendChild(Node);
            }

            Node = doc.CreateElement("compilation");
            Node.AppendChild(doc.CreateTextNode("false"));
            release.AppendChild(Node);

            Node = doc.CreateElement("original_release_date");
            Node.AppendChild(doc.CreateTextNode(music.OriginalReleaseDate.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("release_date");
            Node.AppendChild(doc.CreateTextNode(music.ReleaseDate.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("release_format");
            Node.AppendChild(doc.CreateTextNode(music.ReleaseFormat.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("recording_location");
            Node.AppendChild(doc.CreateTextNode(music.RecordingLocation.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("genre");
            insideNode = doc.CreateElement("primary");
            insideNode.AppendChild(doc.CreateTextNode(music.ReleaseCategory.ToString()));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);

            Node = doc.CreateElement("genre");
            insideNode = doc.CreateElement("primary");
            insideNode.AppendChild(doc.CreateTextNode(music.ReleaseCategory.ToString()));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);


            Node = doc.CreateElement("c_info");
            Node.AppendChild(doc.CreateTextNode(music.CInfo.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("c_year");
            Node.AppendChild(doc.CreateTextNode(music.CYear.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("p_info");
            Node.AppendChild(doc.CreateTextNode(music.PInfo.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("p_year");
            Node.AppendChild(doc.CreateTextNode(music.PYear.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("rights_holders");
            Node.AppendChild(doc.CreateTextNode(music.CopyryghtHolder.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("label");
            insideNode = doc.CreateElement("vendor_label_id");
            insideNode.AppendChild(doc.CreateTextNode(""));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("name");
            insideNode.AppendChild(doc.CreateTextNode(music.LabelName.ToString()));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);


            Node = doc.CreateElement("price_tier");
            insideNode = doc.CreateElement("type");
            insideNode.AppendChild(doc.CreateTextNode("generic"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("name");
            if (music.Itune_Track_Price != null)
            {
                insideNode.AppendChild(doc.CreateTextNode(music.Itune_Track_Price));
            }
            else
            {
                insideNode.AppendChild(doc.CreateTextNode(""));
            }
            Node.AppendChild(insideNode);
            release.AppendChild(Node);


            Node = doc.CreateElement("price_tier");
            insideNode = doc.CreateElement("type");
            insideNode.AppendChild(doc.CreateTextNode("itunes"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("name");
            if (music.Itune_Release_Price != null)
            {
                insideNode.AppendChild(doc.CreateTextNode(music.Itune_Release_Price));
            }
            else
            {
                insideNode.AppendChild(doc.CreateTextNode(""));
            }
            Node.AppendChild(insideNode);
            release.AppendChild(Node);


            Node = doc.CreateElement("territory");
            insideNode = doc.CreateElement("country_code");
            insideNode.AppendChild(doc.CreateTextNode(music.RecordingLocation.ToString()));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("release_date");
            insideNode.AppendChild(doc.CreateTextNode(music.ReleaseDate.ToString()));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);


            Node = doc.CreateElement("asset");
            insideNode = doc.CreateElement("type");
            insideNode.AppendChild(doc.CreateTextNode("Image"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("sub_type");
            insideNode.AppendChild(doc.CreateTextNode("Front"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("format");
            insideNode.AppendChild(doc.CreateTextNode("JPEG"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("md5_checksum");
            insideNode.AppendChild(doc.CreateTextNode(""));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("file_name");
            insideNode.AppendChild(doc.CreateTextNode(music.CoverImage.ToString()));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);

            foreach (var x in songs)
            {
                Node = doc.CreateElement("track");
                insideNode = doc.CreateElement("vendor_track_id");
                insideNode.AppendChild(doc.CreateTextNode(x.SongId.ToString()));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("isrc");
                insideNode.AppendChild(doc.CreateTextNode(x.isrc.ToString()));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("disc_number");
                insideNode.AppendChild(doc.CreateTextNode(""));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("track_number");
                insideNode.AppendChild(doc.CreateTextNode(x.SongId.ToString()));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("title");
                insideNode.AppendChild(doc.CreateTextNode(x.SongTitle.ToString()));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("track_version");
                insideNode.AppendChild(doc.CreateTextNode(""));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("track_length");
                insideNode.AppendChild(doc.CreateTextNode(""));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("advisory");
                insideNode.AppendChild(doc.CreateTextNode(""));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("audio_language");
                insideNode.AppendChild(doc.CreateTextNode(""));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("display_artist");
                insideNode.AppendChild(doc.CreateTextNode(x.ArtistName.ToString()));
                Node.AppendChild(insideNode);

                insideNode = doc.CreateElement("participent");
                var in_insideNode = doc.CreateElement("role");
                in_insideNode.AppendChild(doc.CreateTextNode("Primary"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("name");
                in_insideNode.AppendChild(doc.CreateTextNode(x.ArtistName.ToString()));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("primary");
                in_insideNode.AppendChild(doc.CreateTextNode("true"));
                insideNode.AppendChild(in_insideNode);
                Node.AppendChild(insideNode);

                var song_part = db.SongParticipent.Where(a => a.SongId == x.SongId).ToList();

                foreach (var y in song_part)
                {
                    insideNode = doc.CreateElement("participent");
                    in_insideNode = doc.CreateElement("role");
                    in_insideNode.AppendChild(doc.CreateTextNode(y.RoleType));
                    insideNode.AppendChild(in_insideNode);
                    in_insideNode = doc.CreateElement("name");
                    in_insideNode.AppendChild(doc.CreateTextNode(y.ParticipantName.ToString()));
                    insideNode.AppendChild(in_insideNode);
                    in_insideNode = doc.CreateElement("primary");
                    in_insideNode.AppendChild(doc.CreateTextNode("false"));
                    insideNode.AppendChild(in_insideNode);
                    Node.AppendChild(insideNode);
                }

                in_insideNode = doc.CreateElement("c_info");
                in_insideNode.AppendChild(doc.CreateTextNode(x.CInfo.ToString()));
                Node.AppendChild(in_insideNode);

                in_insideNode = doc.CreateElement("c_year");
                in_insideNode.AppendChild(doc.CreateTextNode(x.CYear.ToString()));
                Node.AppendChild(in_insideNode);

                in_insideNode = doc.CreateElement("p_info");
                in_insideNode.AppendChild(doc.CreateTextNode(x.PInfo.ToString()));
                Node.AppendChild(in_insideNode);

                in_insideNode = doc.CreateElement("p_year");
                in_insideNode.AppendChild(doc.CreateTextNode(x.PYear.ToString()));
                Node.AppendChild(in_insideNode);

                in_insideNode = doc.CreateElement("rights_holders");
                in_insideNode.AppendChild(doc.CreateTextNode(x.CopyRightHolder.ToString()));
                Node.AppendChild(in_insideNode);

                insideNode = doc.CreateElement("permission");
                in_insideNode = doc.CreateElement("type");
                in_insideNode.AppendChild(doc.CreateTextNode("track_sale"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("enabled");
                in_insideNode.AppendChild(doc.CreateTextNode("true"));
                insideNode.AppendChild(in_insideNode);
                Node.AppendChild(insideNode);

                insideNode = doc.CreateElement("permission");
                in_insideNode = doc.CreateElement("type");
                in_insideNode.AppendChild(doc.CreateTextNode("stream"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("enabled");
                in_insideNode.AppendChild(doc.CreateTextNode("true"));
                insideNode.AppendChild(in_insideNode);
                Node.AppendChild(insideNode);

                insideNode = doc.CreateElement("permission");
                in_insideNode = doc.CreateElement("type");
                in_insideNode.AppendChild(doc.CreateTextNode("download"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("enabled");
                in_insideNode.AppendChild(doc.CreateTextNode("true"));
                insideNode.AppendChild(in_insideNode);
                Node.AppendChild(insideNode);

                insideNode = doc.CreateElement("permission");
                in_insideNode = doc.CreateElement("type");
                in_insideNode.AppendChild(doc.CreateTextNode("subscription"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("enabled");
                in_insideNode.AppendChild(doc.CreateTextNode("true"));
                insideNode.AppendChild(in_insideNode);
                Node.AppendChild(insideNode);


                insideNode = doc.CreateElement("asset");
                in_insideNode = doc.CreateElement("type");
                in_insideNode.AppendChild(doc.CreateTextNode("audio"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("sub_type");
                in_insideNode.AppendChild(doc.CreateTextNode("flac"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("format");
                in_insideNode.AppendChild(doc.CreateTextNode("flac"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("md5_checksum");
                in_insideNode.AppendChild(doc.CreateTextNode(""));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("file_name");
                in_insideNode.AppendChild(doc.CreateTextNode(x.SongPath.ToString()));
                insideNode.AppendChild(in_insideNode);
                Node.AppendChild(insideNode);


                release.AppendChild(Node);
            }



            return doc;

            //doc.Save("foo1.xml");
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

        public XmlDocument EP(MusicRelease music, string action)
        {
            var songs = db.Songs.Where(a => a.MusicReleaseId == music.MusicReleaseId).ToList();
            var participents = db.Participants.Where(a => a.MusicReleaseId == music.MusicReleaseId).ToList();

            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode release = doc.CreateElement("release");
            doc.AppendChild(release);

            XmlNode node2 = doc.CreateNode(XmlNodeType.Element, "meta", null);
            release.AppendChild(node2);
            SetAttributeValue(node2, "charset", "utf-8");

            XmlNode Node = doc.CreateElement("schema_id");
            Node.AppendChild(doc.CreateTextNode("schema_id"));
            release.AppendChild(Node);

            Node = doc.CreateElement("distributor_name");
            Node.AppendChild(doc.CreateTextNode("Tempo Production"));
            release.AppendChild(Node);

            Node = doc.CreateElement("export_id");
            Node.AppendChild(doc.CreateTextNode(music.MusicReleaseId.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("export_time");
            Node.AppendChild(doc.CreateTextNode(music.currentDateTime.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("action");
            Node.AppendChild(doc.CreateTextNode(action));
            release.AppendChild(Node);

            Node = doc.CreateElement("upc_ean");
            Node.AppendChild(doc.CreateTextNode(music.UPCEAN.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("vendor_release_id");
            Node.AppendChild(doc.CreateTextNode(""));
            release.AppendChild(Node);

            Node = doc.CreateElement("catalog_id");
            Node.AppendChild(doc.CreateTextNode(""));
            release.AppendChild(Node);

            var Node1 = doc.CreateElement("title");
            if (music.ReleaseName != null)
            {
                Node1.AppendChild(doc.CreateTextNode(music.ReleaseName));
            }
            else
            {
                Node1.AppendChild(doc.CreateTextNode(""));
            }
            release.AppendChild(Node1);

            Node = doc.CreateElement("title_version");
            if(music.ReleaseVersion != null)
            {
                Node.AppendChild(doc.CreateTextNode(music.ReleaseVersion.ToString()));
            }
            else
            {
                Node.AppendChild(doc.CreateTextNode(""));
            }
            release.AppendChild(Node);

            Node = doc.CreateElement("advisory");
            Node.AppendChild(doc.CreateTextNode(music.ReleaseAdvisory.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("metadata_language");
            Node.AppendChild(doc.CreateTextNode("English"));
            release.AppendChild(Node);

            Node = doc.CreateElement("audio_language");
            Node.AppendChild(doc.CreateTextNode("English"));
            release.AppendChild(Node);

            Node = doc.CreateElement("display_artist");
            //Node.AppendChild(doc.CreateTextNode(music.MainArtist.ToString()));
            Node.AppendChild(doc.CreateTextNode(music.MainArtist));
            release.AppendChild(Node);

            Node = doc.CreateElement("participant");
            var insideNode = doc.CreateElement("Role");
            insideNode.AppendChild(doc.CreateTextNode("Primary Artist"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("Name");
            //insideNode.AppendChild(doc.CreateTextNode(music.MainArtist.ToString()));
            insideNode.AppendChild(doc.CreateTextNode(music.MainArtist));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("Primary");
            insideNode.AppendChild(doc.CreateTextNode("True"));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);

            foreach(var x in participents)
            {
                Node = doc.CreateElement("participant");
                insideNode = doc.CreateElement("Role");
                insideNode.AppendChild(doc.CreateTextNode(x.RoleType));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("Name");
                //insideNode.AppendChild(doc.CreateTextNode(x.ParticipantName.ToString()));
                insideNode.AppendChild(doc.CreateTextNode(x.ParticipantName));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("Primary");
                insideNode.AppendChild(doc.CreateTextNode("False"));
                Node.AppendChild(insideNode);
                release.AppendChild(Node);
            }

            Node = doc.CreateElement("compilation");
            Node.AppendChild(doc.CreateTextNode("false"));
            release.AppendChild(Node);

            Node = doc.CreateElement("original_release_date");
            Node.AppendChild(doc.CreateTextNode(music.OriginalReleaseDate.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("release_date");
            Node.AppendChild(doc.CreateTextNode(music.ReleaseDate.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("release_format");
            Node.AppendChild(doc.CreateTextNode(music.ReleaseFormat.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("recording_location");
            Node.AppendChild(doc.CreateTextNode(music.RecordingLocation.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("genre");
            insideNode = doc.CreateElement("primary");
            insideNode.AppendChild(doc.CreateTextNode(music.ReleaseCategory.ToString()));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);

            Node = doc.CreateElement("genre");
            insideNode = doc.CreateElement("primary");
            insideNode.AppendChild(doc.CreateTextNode(music.ReleaseCategory.ToString()));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);


            Node = doc.CreateElement("c_info");
            Node.AppendChild(doc.CreateTextNode(music.CInfo.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("c_year");
            Node.AppendChild(doc.CreateTextNode(music.CYear.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("p_info");
            Node.AppendChild(doc.CreateTextNode(music.PInfo.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("p_year");
            Node.AppendChild(doc.CreateTextNode(music.PYear.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("rights_holders");
            Node.AppendChild(doc.CreateTextNode(music.CopyryghtHolder.ToString()));
            release.AppendChild(Node);

            Node = doc.CreateElement("label");
            insideNode = doc.CreateElement("vendor_label_id");
            insideNode.AppendChild(doc.CreateTextNode(""));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("name");
            insideNode.AppendChild(doc.CreateTextNode(music.LabelName.ToString()));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);


            Node = doc.CreateElement("price_tier");
            insideNode = doc.CreateElement("type");
            insideNode.AppendChild(doc.CreateTextNode("generic"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("name");
            if (music.Itune_Track_Price != null)
            {
                insideNode.AppendChild(doc.CreateTextNode(music.Itune_Track_Price));
            }
            else
            {
                insideNode.AppendChild(doc.CreateTextNode(""));
            }
            Node.AppendChild(insideNode);
            release.AppendChild(Node);


            Node = doc.CreateElement("price_tier");
            insideNode = doc.CreateElement("type");
            insideNode.AppendChild(doc.CreateTextNode("itunes"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("name");
            if (music.Itune_Release_Price != null)
            {
                insideNode.AppendChild(doc.CreateTextNode(music.Itune_Release_Price));
            }
            else
            {
                insideNode.AppendChild(doc.CreateTextNode(""));
            }
            Node.AppendChild(insideNode);
            release.AppendChild(Node);



            Node = doc.CreateElement("permission");
            insideNode = doc.CreateElement("type");
            insideNode.AppendChild(doc.CreateTextNode("preorder"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("enabled");
            insideNode.AppendChild(doc.CreateTextNode("true"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("start_date");
            insideNode.AppendChild(doc.CreateTextNode("start_date_value"));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);



            Node = doc.CreateElement("territory");
            insideNode = doc.CreateElement("country_code");
            insideNode.AppendChild(doc.CreateTextNode(music.RecordingLocation.ToString()));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("release_date");
            insideNode.AppendChild(doc.CreateTextNode(music.ReleaseDate.ToString()));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);


            Node = doc.CreateElement("asset");
            insideNode = doc.CreateElement("type");
            insideNode.AppendChild(doc.CreateTextNode("Image"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("sub_type");
            insideNode.AppendChild(doc.CreateTextNode("Front"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("format");
            insideNode.AppendChild(doc.CreateTextNode("JPEG"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("md5_checksum");
            insideNode.AppendChild(doc.CreateTextNode(""));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("file_name");
            insideNode.AppendChild(doc.CreateTextNode(music.CoverImage.ToString()));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);



            Node = doc.CreateElement("asset");
            insideNode = doc.CreateElement("type");
            insideNode.AppendChild(doc.CreateTextNode("asset"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("sub_type");
            insideNode.AppendChild(doc.CreateTextNode("document"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("name");
            insideNode.AppendChild(doc.CreateTextNode(""));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("notes");
            insideNode.AppendChild(doc.CreateTextNode(""));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("mime_type");
            insideNode.AppendChild(doc.CreateTextNode("application/pdf"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("md5_checksum");
            insideNode.AppendChild(doc.CreateTextNode("md5_checksum_value"));
            Node.AppendChild(insideNode);
            insideNode = doc.CreateElement("file_name");
            insideNode.AppendChild(doc.CreateTextNode(""));
            Node.AppendChild(insideNode);
            release.AppendChild(Node);

            foreach(var x in songs)
            {
                Node = doc.CreateElement("track");
                insideNode = doc.CreateElement("vendor_track_id");
                insideNode.AppendChild(doc.CreateTextNode(x.SongId.ToString()));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("isrc");
                insideNode.AppendChild(doc.CreateTextNode(x.isrc.ToString()));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("disc_number");
                insideNode.AppendChild(doc.CreateTextNode(""));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("track_number");
                insideNode.AppendChild(doc.CreateTextNode(x.SongId.ToString()));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("title");
                insideNode.AppendChild(doc.CreateTextNode(x.SongTitle.ToString()));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("track_version");
                insideNode.AppendChild(doc.CreateTextNode(""));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("track_length");
                insideNode.AppendChild(doc.CreateTextNode(""));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("advisory");
                insideNode.AppendChild(doc.CreateTextNode(""));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("audio_language");
                insideNode.AppendChild(doc.CreateTextNode(""));
                Node.AppendChild(insideNode);
                insideNode = doc.CreateElement("display_artist");
                insideNode.AppendChild(doc.CreateTextNode(x.ArtistName.ToString()));
                Node.AppendChild(insideNode);

                insideNode = doc.CreateElement("participent");
                var in_insideNode = doc.CreateElement("role");
                in_insideNode.AppendChild(doc.CreateTextNode("Primary"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("name");
                in_insideNode.AppendChild(doc.CreateTextNode(x.ArtistName.ToString()));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("primary");
                in_insideNode.AppendChild(doc.CreateTextNode("true"));
                insideNode.AppendChild(in_insideNode);
                Node.AppendChild(insideNode);

                var song_part = db.SongParticipent.Where(a => a.SongId == x.SongId).ToList();

                foreach(var y in song_part)
                {
                    insideNode = doc.CreateElement("participent");
                    in_insideNode = doc.CreateElement("role");
                    in_insideNode.AppendChild(doc.CreateTextNode(y.RoleType.ToString()));
                    insideNode.AppendChild(in_insideNode);
                    in_insideNode = doc.CreateElement("name");
                    in_insideNode.AppendChild(doc.CreateTextNode(y.ParticipantName.ToString()));
                    insideNode.AppendChild(in_insideNode);
                    in_insideNode = doc.CreateElement("primary");
                    in_insideNode.AppendChild(doc.CreateTextNode("false"));
                    insideNode.AppendChild(in_insideNode);
                    Node.AppendChild(insideNode);
                }

                in_insideNode = doc.CreateElement("c_info");
                in_insideNode.AppendChild(doc.CreateTextNode(x.CInfo.ToString()));
                Node.AppendChild(in_insideNode);

                in_insideNode = doc.CreateElement("c_year");
                in_insideNode.AppendChild(doc.CreateTextNode(x.CYear.ToString()));
                Node.AppendChild(in_insideNode);

                in_insideNode = doc.CreateElement("p_info");
                in_insideNode.AppendChild(doc.CreateTextNode(x.PInfo.ToString()));
                Node.AppendChild(in_insideNode);

                in_insideNode = doc.CreateElement("p_year");
                in_insideNode.AppendChild(doc.CreateTextNode(x.PYear.ToString()));
                Node.AppendChild(in_insideNode);

                in_insideNode = doc.CreateElement("rights_holders");
                in_insideNode.AppendChild(doc.CreateTextNode(x.CopyRightHolder.ToString()));
                Node.AppendChild(in_insideNode);

                insideNode = doc.CreateElement("permission");
                in_insideNode = doc.CreateElement("type");
                in_insideNode.AppendChild(doc.CreateTextNode("track_sale"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("enabled");
                in_insideNode.AppendChild(doc.CreateTextNode("true"));
                insideNode.AppendChild(in_insideNode);
                Node.AppendChild(insideNode);

                insideNode = doc.CreateElement("permission");
                in_insideNode = doc.CreateElement("type");
                in_insideNode.AppendChild(doc.CreateTextNode("stream"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("enabled");
                in_insideNode.AppendChild(doc.CreateTextNode("true"));
                insideNode.AppendChild(in_insideNode);
                Node.AppendChild(insideNode);

                insideNode = doc.CreateElement("permission");
                in_insideNode = doc.CreateElement("type");
                in_insideNode.AppendChild(doc.CreateTextNode("download"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("enabled");
                in_insideNode.AppendChild(doc.CreateTextNode("true"));
                insideNode.AppendChild(in_insideNode);
                Node.AppendChild(insideNode);

                insideNode = doc.CreateElement("permission");
                in_insideNode = doc.CreateElement("type");
                in_insideNode.AppendChild(doc.CreateTextNode("subscription"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("enabled");
                in_insideNode.AppendChild(doc.CreateTextNode("true"));
                insideNode.AppendChild(in_insideNode);
                Node.AppendChild(insideNode);

                insideNode = doc.CreateElement("permission");
                in_insideNode = doc.CreateElement("type");
                in_insideNode.AppendChild(doc.CreateTextNode("preorder"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("enabled");
                in_insideNode.AppendChild(doc.CreateTextNode("true"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("start_date");
                in_insideNode.AppendChild(doc.CreateTextNode(""));
                insideNode.AppendChild(in_insideNode);
                Node.AppendChild(insideNode);

                insideNode = doc.CreateElement("asset");
                in_insideNode = doc.CreateElement("type");
                in_insideNode.AppendChild(doc.CreateTextNode("audio"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("sub_type");
                in_insideNode.AppendChild(doc.CreateTextNode("flac"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("format");
                in_insideNode.AppendChild(doc.CreateTextNode("flac"));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("md5_checksum");
                in_insideNode.AppendChild(doc.CreateTextNode(""));
                insideNode.AppendChild(in_insideNode);
                in_insideNode = doc.CreateElement("file_name");
                in_insideNode.AppendChild(doc.CreateTextNode(x.SongPath.ToString()));
                insideNode.AppendChild(in_insideNode);
                Node.AppendChild(insideNode);


                release.AppendChild(Node);
            }

            

            return doc;

            //doc.Save("foo1.xml");
        }

        public void DajMiUPC()
        {

        }

        public ActionResult ConvertToXML()
        {
            //bla bla bla
            //daj mi ovo sve u xml prije pucanja u sql server
            //moram i metodu za validaciju oba mehanizma da sredim prije upisa

            return View();
        }

        // GET: MusicReleases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index");
            }
            MusicRelease musicRelease = db.MusicReleases
                .Include(x => x.Artists)
                .Include(x => x.FeaturedArtists)
                .Include(x => x.Songs)
                .FirstOrDefault(x => x.MusicReleaseId == id);
            if (musicRelease == null)
            {
                return HttpNotFound();
            }

            var songs = musicRelease.Songs.Select(a=>a.SongId);
            ViewBag.SongsParticipants = db.SongParticipent.Where(a => songs.Contains(a.SongId)).ToList();

            string[] release_format = { "Digital", "Single", "EP", "Album", "DoubleAlbum", "BoxSet", "LivePerformance", "ClassicalAlbum", "Video" };
            string[] genre = { "Altrernative", "Blues", "ChristianAndGospel", "Classical", "Country", "Children", "Dance", "Dance", "BreakBeat",
                "ElectroHouse", "House", "Techno", "Electronic", "Ambient", "NewAge", "Chill", "Electro", "Electronica", "Experimental", "Relax",
                "Folk", "HipHopRap", "AlternativeRap", "Holiday", "Christmas", "IndieRock", "Jazz", "Latin", "Salsa", "Pop", "AdultContemporary", 
                "KPop", "PopRock", "SoftRock", "RnBSoul", "Funk", "Reggae", "Rock", "Metal", "Soundtrack", "World", "AfroBeat" };
            string[] advisory = { "None", "Clean", "Explicit" };
            string[] countries = { "Albania", "Algeria", "American Samoa", "Andorra", "Angola", "Anguilla", "Antarctica", "Antigua and Barbuda",
                    "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh",
                    "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia and Herzegovina",
                    "Botswana", "Bouvet Island", "Brazil", "British Indian Ocean Territory", "Brunei Darussalam", "Bulgaria",
                    "Burkina Faso","Burundi","Cambodia","Cameroon","Canada","Cape Verde","Cayman Islands","Central African Republic",
                    "Chad", "Chile", "China", "Christmas Island", "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo", "Congo, The Democratic Republic of The",
                    "Cook Islands", "Costa Rica", "Cote D'ivoire", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica",
                    "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia",
                    "Falkland Islands (Malvinas)", "Faroe Islands", "Fiji", "Finland", "France", "French Guiana", "French Polynesia",
                    "French Southern Territories", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland",
                    "Grenada", "Guadeloupe", "Guam", "Guatemala", "Guernsey", "Guinea", "Guinea-bissau", "Guyana", "Haiti", "Heard Island and Mcdonald Islands",
                    "Holy See (Vatican City State)", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran, Islamic Republic of",
                    "Iraq", "Ireland", "Isle of Man", "Israel", "Italy", "Jamaica", "Japan", "Jersey", "Jordan", "Kazakhstan", "Kenya", "Kiribati",
                    "Korea, Democratic People's Republic of", "Korea, Republic of", "Kuwait", "Kyrgyzstan", "Lao People's Democratic Republic",
                    "Latvia", "Lebanon", "Lesotho", "Liberia", "Libyan Arab Jamahiriya", "Liechtenstein", "Lithuania", "Luxembourg", "Macao",
                    "Macedonia, The Former Yugoslav Republic of", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands",
                    "Martinique", "Mauritania", "Mauritius", "Mayotte", "Mexico", "Micronesia, Federated States of", "Moldova, Republic of",
                    "Monaco", "Mongolia", "Montenegro", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands",
                    "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Niue", "Norfolk Island", "Northern Mariana Islands",
                    "Norway", "Oman", "Pakistan", "Palau", "Palestinian Territory, Occupied", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines",
                    "Pitcairn", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russian Federation", "Rwanda", "Saint Helena",
                    "Saint Kitts and Nevis", "Saint Lucia", "Saint Pierre and Miquelon", "Saint Vincent and The Grenadines", "Samoa",
                    "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia",
                    "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Georgia and The South Sandwich Islands", "Spain",
                    "Sri Lanka", "Sudan", "Suriname", "Svalbard and Jan Mayen", "Swaziland", "Sweden", "Switzerland", "Syrian Arab Republic",
                    "Taiwan, Province of China", "Tajikistan", "Tanzania, United Republic of", "Thailand", "Timor-leste", "Togo", "Tokelau", "Tonga",
                    "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu", "Uganda", "Ukraine",
                    "United Arab Emirates", "United Kingdom", "United States", "United States Minor Outlying Islands", "Uruguay", "Uzbekistan",
                    "Vanuatu", "Venezuela", "Viet Nam", "Virgin Islands, British", "Virgin Islands, U.S.", "Wallis and Futuna", "Western Sahara",
                    "Yemen", "Zambia", "Zimbabwe" };

            ViewBag.Release_Format = release_format;
            ViewBag.Genre = genre;
            ViewBag.Advisory = advisory;
            ViewBag.Countries = countries;

            return View(musicRelease);
        }

        // POST: MusicReleases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MusicReleaseId,ReleaseName,MainArtist,ReleaseVersion,RecordingLocation,Language,CopyrightYear,ReleaseDate,OriginalReleaseDate,UPCEAN,LabelName,CInfo,CYear,PInfo,PYear,CopyryghtHolder,Publisher")] MusicRelease musicRelease,
            string[] MainArtist, string[] FeaturedArtist, string[] Title, string[] ISrc, int[] SongId, HttpPostedFileBase[] songs, HttpPostedFileBase coverImage,
            string[] PartName_New, string[] PartRole_New, string[] PartType_New, string[] PartInstrument_New, string[] PartIPICode_New, string[] PartIPNCode_New,
            string[] Store, string[] composer_song, string[] writer_song, string[] arranger_song, string[] iswc_song, string[] artist_song, string[] trackversion_song, string[] cinfo_song, string[] cyear_song, string[] pinfo_song, string[] pyear_song, string[] copyright_song, string[] participent_song)
        {
            try
            {
                var existedRelease = db.MusicReleases.FirstOrDefault(x => x.MusicReleaseId == musicRelease.MusicReleaseId);
                //existedRelease.ReleaseName = musicRelease.ReleaseName;
                existedRelease.MainArtist = musicRelease.MainArtist;
                existedRelease.ReleaseVersion = musicRelease.ReleaseVersion;
                existedRelease.Language = musicRelease.Language;
                existedRelease.CopyrightYear = musicRelease.CopyrightYear;
                existedRelease.ReleaseDate = musicRelease.ReleaseDate;
                existedRelease.OriginalReleaseDate = musicRelease.OriginalReleaseDate;
                existedRelease.LabelName = musicRelease.LabelName;
                //existedRelease.UPCEAN = musicRelease.UPCEAN;
                existedRelease.ReleaseCategory = musicRelease.ReleaseCategory;
                existedRelease.CInfo = musicRelease.CInfo;
                existedRelease.CYear = musicRelease.CYear;
                existedRelease.PInfo = musicRelease.PInfo;
                existedRelease.PYear = musicRelease.PYear;
                existedRelease.ReleaseFormat = Request.Form["ReleaseFormat"];
                existedRelease.ReleaseCategory = Request.Form["Genre"];
                existedRelease.ReleaseAdvisory = Request.Form["ReleaseAdvisory"];
                existedRelease.TerritoryAvailability = Request.Form["TerritoryAvailability"];
                existedRelease.RecordingLocation = Request.Form["Country"];
                existedRelease.CopyryghtHolder = musicRelease.CopyryghtHolder;
                existedRelease.Publisher = musicRelease.Publisher;
                existedRelease.Update_Status = "Updated";
                musicRelease.DeletionDate = DateTime.Now.ToString();
                db.SaveChanges();

                var artists = db.Artists.Where(x => x.MusicReleaseId == musicRelease.MusicReleaseId);
                db.Artists.RemoveRange(artists);
                db.SaveChanges();

                var mainArtist = new Artist();
                mainArtist.ArtistName = musicRelease.MainArtist;
                mainArtist.MusicReleaseId = musicRelease.MusicReleaseId;
                db.Artists.Add(mainArtist);
                db.SaveChanges();

                var fArtists = db.Participants.Where(x => x.MusicReleaseId == musicRelease.MusicReleaseId);
                db.Participants.RemoveRange(fArtists);
                db.SaveChanges();

                if (PartName_New != null && PartType_New != null && PartRole_New != null && PartInstrument_New != null && PartIPICode_New != null && PartIPNCode_New != null)
                {
                    for (int i = 0; i < PartName_New.Length; i++)
                    {
                        var participent = new Participants();
                        participent.MusicReleaseId = musicRelease.MusicReleaseId;
                        participent.ParticipantName = PartName_New[i];
                        participent.RoleType = PartType_New[i];
                        participent.ParticipantRole = PartRole_New[i];
                        participent.Instrument = PartInstrument_New[i];
                        participent.IPICode = PartIPICode_New[i];
                        participent.IPNCode = PartIPNCode_New[i];

                        db.Participants.Add(participent);
                        db.SaveChanges();
                    }
                }

                var stores = db.Stores.Where(x => x.MusicReleaseId == musicRelease.MusicReleaseId);
                db.Stores.RemoveRange(stores);
                db.SaveChanges();

                if (Store != null)
                {
                    foreach (var x in Store)
                    {
                        var store = new Stores();
                        store.MusicReleaseId = musicRelease.MusicReleaseId;
                        store.StoreName = x;
                        store.isSelected = true;

                        db.Stores.Add(store);
                        db.SaveChanges();
                    }
                }


                var rSongs = db.Songs.Where(x => x.MusicReleaseId == musicRelease.MusicReleaseId).ToList();
                var ncSongs = rSongs.Where(x => !SongId.Any(y => y == x.SongId)).ToList();
                if (ncSongs.Count != 0)
                {
                    db.Songs.RemoveRange(ncSongs);
                    db.SaveChanges();
                }

                string userId = User.Identity.GetUserId();
                var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
                
                StringBuilder content = new StringBuilder();
                string head = "Release Name, Main Artist, Release Verison, Country, Release Format, Advisory, Language, Year, Release Date," +
                       "Original Release Date, UPC, Label Name, Category/Genre, Territory, (C)Info, (C)Year, (P)Info, (P)Year, Copyright Holder, Publisher," +
                       "Main Artist Name, Type, Role Type, Instrument, IPI Code, IPN Code, Track Title, ISRC, Composer Name, Writer Name, Arranger Name, ISWC, Stores";
                content.AppendLine(head);
                if (songs != null)
                {
                    bool csv_check = false;
                    for (int i = 0; i < songs.Length; i++)
                    {
                        var songID = SongId[i];
                        var existSong = db.Songs.FirstOrDefault(x => x.SongId == songID);
                        if (existSong != null)
                        {
                            existSong.SongTitle = Title[i];
                            existSong.isrc = ISrc[i];
                            existSong.ISWC = iswc_song[i];
                            existSong.ArtistName = artist_song[i];
                            existSong.ISWC = iswc_song[i];
                            existSong.PInfo = pinfo_song[i];
                            existSong.PYear = pyear_song[i];
                            existSong.CInfo = cinfo_song[i];
                            existSong.CYear = cyear_song[i];
                            existSong.CopyRightHolder = copyright_song[i];
                            existSong.version = trackversion_song[i];
                            db.SaveChanges();

                            var old_song_part = db.SongParticipent.Where(a => a.SongId == existSong.SongId).ToList();
                            db.SongParticipent.RemoveRange(old_song_part);

                            var part = participent_song[i];
                            if (part.Contains('$'))
                            {
                                var participents = part.Split('$');
                                for (int p = 0; p < participents.Length - 1; p++)
                                {
                                    var part_data = participents[p].Split('|');
                                    var new_song_participent = new SongParticipent();
                                    new_song_participent.SongId = existSong.SongId;
                                    new_song_participent.ParticipantName = part_data[0];
                                    new_song_participent.RoleType = part_data[1];
                                    new_song_participent.ParticipantRole = part_data[2];
                                    new_song_participent.Instrument = part_data[3];
                                    new_song_participent.IPICode = part_data[4];
                                    new_song_participent.IPNCode = part_data[5];

                                    db.SongParticipent.Add(new_song_participent);
                                    db.SaveChanges();
                                }
                            }

                            string upcean = existedRelease.UPCEAN.ToString();
                            string line = "";
                            if (i==0 && PartName_New != null && PartType_New != null && PartRole_New != null && PartInstrument_New != null && PartIPICode_New != null && PartIPNCode_New != null && Store != null)
                            {
                                line = "" + existedRelease.ReleaseName + "," + existedRelease.MainArtist + "," + existedRelease.ReleaseVersion + "," +
                                "" + existedRelease.RecordingLocation + "," +
                                "" + existedRelease.ReleaseFormat + "," + existedRelease.ReleaseAdvisory + "," + existedRelease.Language + "," +
                                "" + existedRelease.CopyrightYear + "," + existedRelease.ReleaseDate + "," + existedRelease.OriginalReleaseDate + "," +
                                "" + existedRelease.UPCEAN + "," + existedRelease.LabelName + "," + existedRelease.ReleaseCategory + "," +
                                "" + existedRelease.TerritoryAvailability + "," + existedRelease.CInfo + "," + existedRelease.CYear + "," +
                                "" + existedRelease.PInfo + "," + existedRelease.PYear + "," + existedRelease.CopyryghtHolder + "," +
                                "" + existedRelease.Publisher + "," + string.Join("| ", PartName_New) + "," + string.Join("| ", PartType_New) + "," +
                                string.Join("| ", PartRole_New) + "," + string.Join("| ", PartInstrument_New) + "," + string.Join("| ", PartIPICode_New) + "," +
                                string.Join("| ", PartIPNCode_New) + ",,,," +
                                ",,," + string.Join("| ", Store);
                                content.AppendLine(line);
                                csv_check = true;
                            }
                            else if (i==0 && PartName_New != null && PartType_New != null && PartRole_New != null && PartInstrument_New != null && PartIPICode_New != null && PartIPNCode_New != null && Store == null)
                            {
                                line = "" + existedRelease.ReleaseName + "," + existedRelease.MainArtist + "," + existedRelease.ReleaseVersion + "," +
                                "" + existedRelease.RecordingLocation + "," +
                                "" + existedRelease.ReleaseFormat + "," + existedRelease.ReleaseAdvisory + "," + existedRelease.Language + "," +
                                "" + existedRelease.CopyrightYear + "," + existedRelease.ReleaseDate + "," + existedRelease.OriginalReleaseDate + "," +
                                "" + existedRelease.UPCEAN + "," + existedRelease.LabelName + "," + existedRelease.ReleaseCategory + "," +
                                "" + existedRelease.TerritoryAvailability + "," + existedRelease.CInfo + "," + existedRelease.CYear + "," +
                                "" + existedRelease.PInfo + "," + existedRelease.PYear + "," + existedRelease.CopyryghtHolder + "," +
                                "" + existedRelease.Publisher + "," + string.Join("| ", PartName_New) + "," + string.Join("| ", PartType_New) + "," +
                                string.Join("| ", PartRole_New) + "," + string.Join("| ", PartInstrument_New) + "," + string.Join("| ", PartIPICode_New) + "," +
                                string.Join("| ", PartIPNCode_New) + ",,,," +
                                ",,,";
                                content.AppendLine(line);
                                csv_check = true;
                            }
                            else if (i==0 && Store != null && PartName_New == null && PartType_New == null && PartRole_New == null && PartInstrument_New == null && PartIPICode_New == null && PartIPNCode_New == null)
                            {
                                line = "" + existedRelease.ReleaseName + "," + existedRelease.MainArtist + "," + existedRelease.ReleaseVersion + "," +
                                "" + existedRelease.RecordingLocation + "," +
                                "" + existedRelease.ReleaseFormat + "," + existedRelease.ReleaseAdvisory + "," + existedRelease.Language + "," +
                                "" + existedRelease.CopyrightYear + "," + existedRelease.ReleaseDate + "," + existedRelease.OriginalReleaseDate + "," +
                                "" + existedRelease.UPCEAN + "," + existedRelease.LabelName + "," + existedRelease.ReleaseCategory + "," +
                                "" + existedRelease.TerritoryAvailability + "," + existedRelease.CInfo + "," + existedRelease.CYear + "," +
                                "" + existedRelease.PInfo + "," + existedRelease.PYear + "," + existedRelease.CopyryghtHolder + "," +
                                "" + existedRelease.Publisher + ",,,,,,,,,," +
                                ",,," + string.Join("|", Store);
                                content.AppendLine(line);
                                csv_check = true;
                            }
                            else
                            {
                                line = "" + existedRelease.ReleaseName + "," + existedRelease.MainArtist + "," + existedRelease.ReleaseVersion + "," +
                                "" + existedRelease.RecordingLocation + "," +
                                "" + existedRelease.ReleaseFormat + "," + existedRelease.ReleaseAdvisory + "," + existedRelease.Language + "," +
                                "" + existedRelease.CopyrightYear + "," + existedRelease.ReleaseDate + "," + existedRelease.OriginalReleaseDate + "," +
                                "" + existedRelease.UPCEAN + "," + existedRelease.LabelName + "," + existedRelease.ReleaseCategory + "," +
                                "" + existedRelease.TerritoryAvailability + "," + existedRelease.CInfo + "," + existedRelease.CYear + "," +
                                "" + existedRelease.PInfo + "," + existedRelease.PYear + "," + existedRelease.CopyryghtHolder + "," +
                                "" + existedRelease.Publisher + ",,,,,,,,,," +
                                ",,,";
                                content.AppendLine(line);
                                csv_check = true;
                            }
                        }
                        else
                        {
                            if (songs[i] != null)
                            {
                                Random rnd = new Random();
                                int rand = rnd.Next(1000, 9999);
                                var sIndex = i + 1;

                                //string ServerSavePath = "https://ddex-distribution.s3.eu-north-1.amazonaws.com/" + "musicdistribution/" + existedRelease.currentDateTime.ToString("yyyy-MM-dd") + "/" + musicRelease.UPCEAN;
                                string ServerSavePath = "https://ddex-distribution.s3.eu-north-1.amazonaws.com/" + musicRelease.UPCEAN;

                                var song = new Song();
                             
                                byte[] imageByte = null;
                                BinaryReader rdr = new BinaryReader(songs[i].InputStream);
                                imageByte = rdr.ReadBytes((int)songs[i].ContentLength);
                                MemoryStream stream1 = new MemoryStream(imageByte);
                                //uploadToS3Bucket(stream1, "musicdistribution/" + existedRelease.currentDateTime.ToString("yyyy-MM-dd") + "/" + musicRelease.UPCEAN + "/" + musicRelease.UPCEAN.ToString() + rand.ToString() + "_01_" + sIndex + ".wav", songs[i].ContentType);
                                uploadToS3Bucket(stream1, musicRelease.UPCEAN + "/" + musicRelease.UPCEAN.ToString() + rand.ToString() + "_01_" + sIndex + ".wav", songs[i].ContentType);

                                song.SongPath = ServerSavePath + "/" + musicRelease.UPCEAN.ToString() + rand.ToString() + "_01_" + sIndex + ".wav";
                                song.SongTitle = Title[i];
                                song.isrc = ISrc[i];
                                song.MusicReleaseId = musicRelease.MusicReleaseId;
                                song.ArtistName = artist_song[i];
                                song.ISWC = iswc_song[i];
                                song.PInfo = pinfo_song[i];
                                song.PYear = pyear_song[i];
                                song.CInfo = cinfo_song[i];
                                song.CYear = cyear_song[i];
                                song.CopyRightHolder = copyright_song[i];
                                song.version = trackversion_song[i];

                                db.Songs.Add(song);
                                db.SaveChanges();

                                var last_song = db.Songs.ToList().LastOrDefault();

                                var part = participent_song[i];
                                if (part.Contains('$'))
                                {
                                    var participents = part.Split('$');
                                    for (int p = 0; p < participents.Length - 1; p++)
                                    {
                                        var part_data = participents[p].Split('|');
                                        var new_song_participent = new SongParticipent();
                                        new_song_participent.SongId = last_song.SongId;
                                        new_song_participent.ParticipantName = part_data[0];
                                        new_song_participent.RoleType = part_data[1];
                                        new_song_participent.ParticipantRole = part_data[2];
                                        new_song_participent.Instrument = part_data[3];
                                        new_song_participent.IPICode = part_data[4];
                                        new_song_participent.IPNCode = part_data[5];

                                        db.SongParticipent.Add(new_song_participent);
                                        db.SaveChanges();
                                    }
                                }

                                string upcean = existedRelease.UPCEAN.ToString();
                                string line = "";
                                if (i == 0 && csv_check == false && PartName_New != null && PartType_New != null && PartRole_New != null && PartInstrument_New != null && PartIPICode_New != null && PartIPNCode_New != null && Store != null)
                                {
                                    line = "" + existedRelease.ReleaseName + "," + existedRelease.MainArtist + "," + existedRelease.ReleaseVersion + "," +
                                    "" + existedRelease.RecordingLocation + "," +
                                    "" + existedRelease.ReleaseFormat + "," + existedRelease.ReleaseAdvisory + "," + existedRelease.Language + "," +
                                    "" + existedRelease.CopyrightYear + "," + existedRelease.ReleaseDate + "," + existedRelease.OriginalReleaseDate + "," +
                                    "" + existedRelease.UPCEAN + "," + existedRelease.LabelName + "," + existedRelease.ReleaseCategory + "," +
                                    "" + existedRelease.TerritoryAvailability + "," + existedRelease.CInfo + "," + existedRelease.CYear + "," +
                                    "" + existedRelease.PInfo + "," + existedRelease.PYear + "," + existedRelease.CopyryghtHolder + "," +
                                    "" + existedRelease.Publisher + "," + string.Join("| ", PartName_New) + "," + string.Join("| ", PartType_New) + "," +
                                    string.Join("| ", PartRole_New) + "," + string.Join("| ", PartInstrument_New) + "," + string.Join("| ", PartIPICode_New) + "," +
                                    string.Join("| ", PartIPNCode_New) + ",,,," +
                                    ",,," + string.Join("| ", Store);
                                    content.AppendLine(line);
                                    
                                }
                                else if (i == 0 && csv_check == false && PartName_New != null && PartType_New != null && PartRole_New != null && PartInstrument_New != null && PartIPICode_New != null && PartIPNCode_New != null && Store == null)
                                {
                                    line = "" + existedRelease.ReleaseName + "," + existedRelease.MainArtist + "," + existedRelease.ReleaseVersion + "," +
                                    "" + existedRelease.RecordingLocation + "," +
                                    "" + existedRelease.ReleaseFormat + "," + existedRelease.ReleaseAdvisory + "," + existedRelease.Language + "," +
                                    "" + existedRelease.CopyrightYear + "," + existedRelease.ReleaseDate + "," + existedRelease.OriginalReleaseDate + "," +
                                    "" + existedRelease.UPCEAN + "," + existedRelease.LabelName + "," + existedRelease.ReleaseCategory + "," +
                                    "" + existedRelease.TerritoryAvailability + "," + existedRelease.CInfo + "," + existedRelease.CYear + "," +
                                    "" + existedRelease.PInfo + "," + existedRelease.PYear + "," + existedRelease.CopyryghtHolder + "," +
                                    "" + existedRelease.Publisher + "," + string.Join("| ", PartName_New) + "," + string.Join("| ", PartType_New) + "," +
                                    string.Join("| ", PartRole_New) + "," + string.Join("| ", PartInstrument_New) + "," + string.Join("| ", PartIPICode_New) + "," +
                                    string.Join("| ", PartIPNCode_New) + ",,,," +
                                    ",,,";
                                    content.AppendLine(line);
                                    
                                }
                                else if (i == 0 && csv_check == false && Store != null && PartName_New == null && PartType_New == null && PartRole_New == null && PartInstrument_New == null && PartIPICode_New == null && PartIPNCode_New == null)
                                {
                                    line = "" + existedRelease.ReleaseName + "," + existedRelease.MainArtist + "," + existedRelease.ReleaseVersion + "," +
                                    "" + existedRelease.RecordingLocation + "," +
                                    "" + existedRelease.ReleaseFormat + "," + existedRelease.ReleaseAdvisory + "," + existedRelease.Language + "," +
                                    "" + existedRelease.CopyrightYear + "," + existedRelease.ReleaseDate + "," + existedRelease.OriginalReleaseDate + "," +
                                    "" + existedRelease.UPCEAN + "," + existedRelease.LabelName + "," + existedRelease.ReleaseCategory + "," +
                                    "" + existedRelease.TerritoryAvailability + "," + existedRelease.CInfo + "," + existedRelease.CYear + "," +
                                    "" + existedRelease.PInfo + "," + existedRelease.PYear + "," + existedRelease.CopyryghtHolder + "," +
                                    "" + existedRelease.Publisher + ",,,,,,,,,," +
                                    ",,," + string.Join("|", Store);
                                    content.AppendLine(line);
                                    
                                }
                                else
                                {
                                    line = "" + existedRelease.ReleaseName + "," + existedRelease.MainArtist + "," + existedRelease.ReleaseVersion + "," +
                                    "" + existedRelease.RecordingLocation + "," +
                                    "" + existedRelease.ReleaseFormat + "," + existedRelease.ReleaseAdvisory + "," + existedRelease.Language + "," +
                                    "" + existedRelease.CopyrightYear + "," + existedRelease.ReleaseDate + "," + existedRelease.OriginalReleaseDate + "," +
                                    "" + existedRelease.UPCEAN + "," + existedRelease.LabelName + "," + existedRelease.ReleaseCategory + "," +
                                    "" + existedRelease.TerritoryAvailability + "," + existedRelease.CInfo + "," + existedRelease.CYear + "," +
                                    "" + existedRelease.PInfo + "," + existedRelease.PYear + "," + existedRelease.CopyryghtHolder + "," +
                                    "" + existedRelease.Publisher + ",,,,,,,,,," +
                                    ",,,";
                                    content.AppendLine(line);
                                    
                                }

                            }
                        }
                    }
                }
                else {
                    string upcean = existedRelease.UPCEAN.ToString();
                    string line = "";
                    if (PartName_New != null && PartType_New != null && PartRole_New != null && PartInstrument_New != null && PartIPICode_New != null && PartIPNCode_New != null && Store != null)
                    {
                        line = "" + existedRelease.ReleaseName + "," + existedRelease.MainArtist + "," + existedRelease.ReleaseVersion + "," +
                        "" + existedRelease.RecordingLocation + "," +
                        "" + existedRelease.ReleaseFormat + "," + existedRelease.ReleaseAdvisory + "," + existedRelease.Language + "," +
                        "" + existedRelease.CopyrightYear + "," + existedRelease.ReleaseDate + "," + existedRelease.OriginalReleaseDate + "," +
                        "" + existedRelease.UPCEAN + "," + existedRelease.LabelName + "," + existedRelease.ReleaseCategory + "," +
                        "" + existedRelease.TerritoryAvailability + "," + existedRelease.CInfo + "," + existedRelease.CYear + "," +
                        "" + existedRelease.PInfo + "," + existedRelease.PYear + "," + existedRelease.CopyryghtHolder + "," +
                        "" + existedRelease.Publisher + "," + string.Join("| ", PartName_New) + "," + string.Join("| ", PartType_New) + "," +
                        string.Join("| ", PartRole_New) + "," + string.Join("| ", PartInstrument_New) + "," + string.Join("| ", PartIPICode_New) + "," +
                        string.Join("| ", PartIPNCode_New) + ",,,," +
                        ",,," + string.Join("| ", Store);
                        content.AppendLine(line);
                    }
                    else if (PartName_New != null && PartType_New != null && PartRole_New != null && PartInstrument_New != null && PartIPICode_New != null && PartIPNCode_New != null && Store == null)
                    {
                        line = "" + existedRelease.ReleaseName + "," + existedRelease.MainArtist + "," + existedRelease.ReleaseVersion + "," +
                        "" + existedRelease.RecordingLocation + "," +
                        "" + existedRelease.ReleaseFormat + "," + existedRelease.ReleaseAdvisory + "," + existedRelease.Language + "," +
                        "" + existedRelease.CopyrightYear + "," + existedRelease.ReleaseDate + "," + existedRelease.OriginalReleaseDate + "," +
                        "" + existedRelease.UPCEAN + "," + existedRelease.LabelName + "," + existedRelease.ReleaseCategory + "," +
                        "" + existedRelease.TerritoryAvailability + "," + existedRelease.CInfo + "," + existedRelease.CYear + "," +
                        "" + existedRelease.PInfo + "," + existedRelease.PYear + "," + existedRelease.CopyryghtHolder + "," +
                        "" + existedRelease.Publisher + "," + string.Join("| ", PartName_New) + "," + string.Join("| ", PartType_New) + "," +
                        string.Join("| ", PartRole_New) + "," + string.Join("| ", PartInstrument_New) + "," + string.Join("| ", PartIPICode_New) + "," +
                        string.Join("| ", PartIPNCode_New) + ",,,," +
                        ",,,";
                        content.AppendLine(line);
                    }
                    else if (Store != null && PartName_New == null && PartType_New == null && PartRole_New == null && PartInstrument_New == null && PartIPICode_New == null && PartIPNCode_New == null)
                    {
                        line = "" + existedRelease.ReleaseName + "," + existedRelease.MainArtist + "," + existedRelease.ReleaseVersion + "," +
                        "" + existedRelease.RecordingLocation + "," +
                        "" + existedRelease.ReleaseFormat + "," + existedRelease.ReleaseAdvisory + "," + existedRelease.Language + "," +
                        "" + existedRelease.CopyrightYear + "," + existedRelease.ReleaseDate + "," + existedRelease.OriginalReleaseDate + "," +
                        "" + existedRelease.UPCEAN + "," + existedRelease.LabelName + "," + existedRelease.ReleaseCategory + "," +
                        "" + existedRelease.TerritoryAvailability + "," + existedRelease.CInfo + "," + existedRelease.CYear + "," +
                        "" + existedRelease.PInfo + "," + existedRelease.PYear + "," + existedRelease.CopyryghtHolder + "," +
                        "" + existedRelease.Publisher + ",,,,,,,,,," +
                        ",,," + string.Join("|", Store);
                        content.AppendLine(line);
                    }
                    else
                    {
                        line = "" + existedRelease.ReleaseName + "," + existedRelease.MainArtist + "," + existedRelease.ReleaseVersion + "," +
                        "" + existedRelease.RecordingLocation + "," +
                        "" + existedRelease.ReleaseFormat + "," + existedRelease.ReleaseAdvisory + "," + existedRelease.Language + "," +
                        "" + existedRelease.CopyrightYear + "," + existedRelease.ReleaseDate + "," + existedRelease.OriginalReleaseDate + "," +
                        "" + existedRelease.UPCEAN + "," + existedRelease.LabelName + "," + existedRelease.ReleaseCategory + "," +
                        "" + existedRelease.TerritoryAvailability + "," + existedRelease.CInfo + "," + existedRelease.CYear + "," +
                        "" + existedRelease.PInfo + "," + existedRelease.PYear + "," + existedRelease.CopyryghtHolder + "," +
                        "" + existedRelease.Publisher + ",,,,,,,,,," +
                        ",,," ;
                        content.AppendLine(line);
                    }

                }

                //var s = existedRelease.CoverImage.Split('/');
                //var sa = "";
                //for (int i = 0; i < s.Length - 1; i++)
                //{
                //    sa += s[i] + "/";
                //}
                //var toRemove1 = "https://tpdigital.blob.core.windows.net/musicdistribution/";
                //var img1 = sa.Remove(sa.IndexOf(toRemove1), toRemove1.Length);
                //img1 += "Updated_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "_" + existedRelease.UPCEAN + ".csv";

                //byte[] data = new ASCIIEncoding().GetBytes(content.ToString());
                //MemoryStream stream = new MemoryStream(data);
                //CloudBlockBlob cloudBlockBlob1 = cloudBlobContainer.GetBlockBlobReference(img1);
                //cloudBlockBlob1.Properties.ContentType = "application/vnd.ms-excel";
                //cloudBlockBlob1.UploadFromStream(stream);


                if (coverImage != null)
                {
                    if (existedRelease.CoverImage.Contains("https://tpdigital.blob.core.windows.net"))
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

                        string ServerSavePath = null;
                        if (user.LabelName != "")
                        {
                            ServerSavePath = "musicdistribution/" + existedRelease.currentDateTime.ToString("yyyy-MM-dd") + "/" + existedRelease.LabelName + "/" + existedRelease.UPCEAN + "/";
                        }
                        else
                        {
                            ServerSavePath = "musicdistribution/" + existedRelease.currentDateTime.ToString("yyyy-MM-dd") + "/" + existedRelease.LabelName + "/";
                        }

                        var ImagePath = ServerSavePath + musicRelease.UPCEAN + ".jpg";
                        var toRemove = "https://tpdigital.blob.core.windows.net/musicdistribution/";
                        var img = existedRelease.CoverImage.Remove(existedRelease.CoverImage.IndexOf(toRemove), toRemove.Length);
                        CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(img);
                        cloudBlockBlob.DeleteIfExists();

                        var newimg = img.Remove(img.IndexOf(".jpg"), ".jpg".Length);
                        cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(newimg + "_U.jpg");
                        cloudBlockBlob.Properties.ContentType = coverImage.ContentType;
                        cloudBlockBlob.UploadFromStream(coverImage.InputStream);
                        existedRelease.CoverImage = "https://tpdigital.blob.core.windows.net/musicdistribution/" + newimg + "_U.jpg";

                        db.SaveChanges();
                    }
                    else
                    {
                        //var path = existedRelease.CoverImage;
                        //var toRemoveFrom = "https://ddex-distribution.s3.eu-north-1.amazonaws.com/";
                        //var updatedPath = path.Remove(path.IndexOf(toRemoveFrom), toRemoveFrom.Length);

                        byte[] imageByte = null;
                        BinaryReader rdr = new BinaryReader(coverImage.InputStream);
                        imageByte = rdr.ReadBytes((int)coverImage.ContentLength);
                        MemoryStream stream1 = new MemoryStream(imageByte);

                        //uploadToS3Bucket(stream1, updatedPath, coverImage.ContentType);
                        uploadToS3Bucket(stream1, existedRelease.UPCEAN + "/" + existedRelease.UPCEAN + ".jpg", coverImage.ContentType);
                    }

                    existedRelease = db.MusicReleases.FirstOrDefault(x => x.MusicReleaseId == musicRelease.MusicReleaseId);
                    if (existedRelease.ReleaseFormat.Equals("Single"))
                    {
                        var doc = Single(existedRelease, "full update");
                        byte[] data4 = new ASCIIEncoding().GetBytes(doc.OuterXml);
                        MemoryStream stream4 = new MemoryStream(data4);
                        uploadToS3Bucket(stream4, musicRelease.UPCEAN + ".xml", "application/xml");
                    }
                    else
                    {
                        var doc = EP(existedRelease, "full update");
                        byte[] data4 = new ASCIIEncoding().GetBytes(doc.OuterXml);
                        MemoryStream stream4 = new MemoryStream(data4);
                         uploadToS3Bucket(stream4, musicRelease.UPCEAN + ".xml", "application/xml");
                    }
                }
                else
                {
                    existedRelease = db.MusicReleases.FirstOrDefault(x => x.MusicReleaseId == musicRelease.MusicReleaseId);
                    if (existedRelease.ReleaseFormat.Equals("Single"))
                    {
                        var doc = Single(existedRelease, "meta update");
                        byte[] data4 = new ASCIIEncoding().GetBytes(doc.OuterXml);
                        MemoryStream stream4 = new MemoryStream(data4);
                        uploadToS3Bucket(stream4, musicRelease.UPCEAN + ".xml", "application/xml");
                    }
                    else
                    {
                        var doc = EP(existedRelease, "meta update");
                        byte[] data4 = new ASCIIEncoding().GetBytes(doc.OuterXml);
                        MemoryStream stream4 = new MemoryStream(data4);
                        uploadToS3Bucket(stream4, musicRelease.UPCEAN + ".xml", "application/xml");
                    }
                }

                var senderMail = new MailAddress("support@tempodigital.org", "Tempo Digital Team");
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

                string ServerSavePath1 = "https://ddex-distribution.s3.eu-north-1.amazonaws.com/" + musicRelease.UPCEAN;

                MailMessage message = new MailMessage(senderMail, receiverMial);
                message.Subject = "Music Release is Updated.";
                message.Body = "Hi,\nRelease successfully updated and is now visible in the View Discograpghy section of distribution dashboard.\n" +
                    
                    "For any further queries please email support@tempodigital.org\n\nThank you.\n\nSender,\nTempo Digital Team.";

                smtp.Send(message);

                // send mail to distribution
                receiverMial = new MailAddress("distribution@tempodigital.org", "Receiver");
                smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderMail.Address, ConfigurationManager.AppSettings["GmailPasword"])
                };
                message = new MailMessage(senderMail, receiverMial);
                message.Subject = "Music Release is Updated.";
                message.Body = "Hi,\nRelease was successfully updated.\n" +
                    "\nID:" + musicRelease.MusicReleaseId.ToString() + "\n" +
                    "Details are Here: " + ServerSavePath1 + "/" + musicRelease.UPCEAN + ".xml" + "\n" +
                    "\nThank you.\n\nSender,\nTempo Digital Team.";
                smtp.Send(message);


                return RedirectToAction("ReleaseDetails", "MusicReleases", new { id = existedRelease.MusicReleaseId });
        }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.Message);
    }
}


        // GET: MusicReleases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index");
            }
            MusicRelease musicRelease = db.MusicReleases.Find(id);
            if (musicRelease == null)
            {
                return HttpNotFound();
            }
            return View(musicRelease);
        }

        // POST: MusicReleases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MusicRelease musicRelease = db.MusicReleases.Find(id);
            musicRelease.Status = "Deleted";
            musicRelease.DeletionDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            db.SaveChanges();

            string userId = User.Identity.GetUserId();
            var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();


            if (musicRelease.ReleaseFormat.Equals("Single"))
            {
                var doc = Single(musicRelease, "delete");
                byte[] data4 = new ASCIIEncoding().GetBytes(doc.OuterXml);
                MemoryStream stream4 = new MemoryStream(data4);
                uploadToS3Bucket(stream4, musicRelease.UPCEAN + ".xml", "application/xml");
            }
            else
            {
                var doc = EP(musicRelease, "delete");
                byte[] data4 = new ASCIIEncoding().GetBytes(doc.OuterXml);
                MemoryStream stream4 = new MemoryStream(data4);
                uploadToS3Bucket(stream4, musicRelease.UPCEAN + ".xml", "application/xml");
            }


            // send mail to distribution
            var senderMail1 = new MailAddress("support@tempodigital.org", "Tempo Digital Team");
            var receiverMial1 = new MailAddress("distribution@tempodigital.org", "Receiver");
            var smtp1 = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderMail1.Address, ConfigurationManager.AppSettings["GmailPasword"])
            };

            string ServerSavePath1 = "https://ddex-distribution.s3.eu-north-1.amazonaws.com/" + musicRelease.UPCEAN;
            var message1 = new MailMessage(senderMail1, receiverMial1);
            message1.Subject = "Music Release was Deleted.";
            message1.IsBodyHtml = true;
                       
                message1.Body = "Hi,<br>Release was successfully Deleted.<br>" +
                "Music Release ID:  " + id + "<br>" +
                "Details are Here: " + ServerSavePath1 + "/" + musicRelease.UPCEAN + ".xml" + "<br>" +
                "<br>Thank you.<br><br>Sender,<br>Tempo Digital Team.";

            smtp1.Send(message1);

            return RedirectToAction("Index","MusicReleases", new { id=0 });
        }

        [HttpPost]
        public ActionResult SetMetadata(int blocksCount, string fileName, long fileSize, int fileIndex, string labelName, string UPC)
        {
            var container = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["BlobStorageConnectionString"]).CreateCloudBlobClient()
                .GetContainerReference("musicdistribution");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            string userId = User.Identity.GetUserId();
            var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
            string ServerSavePath = null;
            string dateToday = DateTime.Now.ToString("yyyy-MM-dd");
            if (user.LabelName != "")
            {
                //ServerSavePath = "musicdistribution/" + labelName + "/" + UPC + "/";
                ServerSavePath = "musicdistribution/" + dateToday + "/" + labelName + "/" + UPC + "/";
            }
            else
            {
                //ServerSavePath = "musicdistribution/" + labelName + "/";
                ServerSavePath = "musicdistribution/" + dateToday + "/" + labelName + "/";
            }

            string SongPath = null;
            int sIndex = fileIndex + 1;
            if (sIndex < 10)
            {
                if (labelName != "")
                    SongPath = ServerSavePath + UPC + "_01_0" + sIndex + ".wav";
                else
                    SongPath = ServerSavePath + UPC + "_01_" + sIndex + ".wav";
            }
            else
            {
                if (labelName != "")
                    SongPath = ServerSavePath + UPC + "_01_0" + sIndex + ".wav";
                else
                    SongPath = ServerSavePath + UPC + "_01_" + sIndex + ".wav";
            }


            var fileToUpload = new CloudFile()
            {
                BlockCount = blocksCount,
                FileName = fileName,
                Size = fileSize,
                BlockBlob = container.GetBlockBlobReference(SongPath),
                StartTime = DateTime.Now,
                IsUploadCompleted = false,
                UploadStatusMessage = string.Empty,
                FileKey = "CurrentFile" + fileIndex.ToString(),
                FileIndex = fileIndex
            };
            Session.Add(fileToUpload.FileKey, fileToUpload);
            return Json(new { success = true, index = fileIndex });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadChunk(int id, int fileIndex)
        {
            HttpPostedFileBase request = Request.Files["Slice"];
            byte[] chunk = new byte[request.ContentLength];
            request.InputStream.Read(chunk, 0, Convert.ToInt32(request.ContentLength));
            JsonResult returnData = null;
            string fileSession = "CurrentFile" + fileIndex.ToString();
            if (Session[fileSession] != null)
            {
                CloudFile model = (CloudFile)Session[fileSession];
                returnData = UploadCurrentChunk(model, chunk, id);
                if (returnData != null)
                {
                    return returnData;
                }
                if (id == model.BlockCount)
                {
                    return CommitAllChunks(model);
                }
            }
            else
            {
                returnData = Json(new
                {
                    error = true,
                    isLastBlock = false,
                    message = string.Format(CultureInfo.CurrentCulture,
                        "Failed to Upload file.", "Session Timed out")
                });
                return returnData;
            }

            return Json(new { error = false, isLastBlock = false, message = string.Empty, index = fileIndex });
        }

        private ActionResult CommitAllChunks(CloudFile model)
        {
            model.IsUploadCompleted = true;
            bool errorInOperation = false;
            try
            {
                var blockList = Enumerable.Range(1, (int)model.BlockCount).ToList<int>().ConvertAll(rangeElement =>
                            Convert.ToBase64String(Encoding.UTF8.GetBytes(
                                string.Format(CultureInfo.InvariantCulture, "{0:D4}", rangeElement))));
                model.BlockBlob.PutBlockList(blockList);
                var duration = DateTime.Now - model.StartTime;
                float fileSizeInKb = model.Size / 1024;
                string fileSizeMessage = fileSizeInKb > 1024 ?
                    string.Concat((fileSizeInKb / 1024).ToString(CultureInfo.CurrentCulture), " MB") :
                    string.Concat(fileSizeInKb.ToString(CultureInfo.CurrentCulture), " KB");
                model.UploadStatusMessage = string.Format(CultureInfo.CurrentCulture,
                    "File uploaded successfully. {0} took {1} seconds to upload",
                    fileSizeMessage, duration.TotalSeconds);
            }
            catch (StorageException e)
            {
                model.UploadStatusMessage = "Failed to Upload file. Exception - " + e.Message;
                errorInOperation = true;
            }
            finally
            {
                Session.Remove(model.FileKey);
            }
            return Json(new
            {
                error = errorInOperation,
                isLastBlock = model.IsUploadCompleted,
                message = model.UploadStatusMessage,
                index = model.FileIndex
            });
        }

        private JsonResult UploadCurrentChunk(CloudFile model, byte[] chunk, int id)
        {
            using (var chunkStream = new MemoryStream(chunk))
            {
                var blockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                        string.Format(CultureInfo.InvariantCulture, "{0:D4}", id)));
                try
                {
                    model.BlockBlob.PutBlock(
                        blockId,
                        chunkStream, null, null,
                        new BlobRequestOptions()
                        {
                            RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(10), 3)
                        },
                        null);
                    return null;
                }
                catch (StorageException e)
                {
                    Session.Remove(model.FileKey);
                    model.IsUploadCompleted = true;
                    model.UploadStatusMessage = "Failed to Upload file. Exception - " + e.Message;
                    return Json(new { error = true, isLastBlock = false, message = model.UploadStatusMessage, index = model.FileIndex });
                }
            }
        }

        public string GetIPAddress()
        {
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            string IPAddress = null;
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
    }
}
