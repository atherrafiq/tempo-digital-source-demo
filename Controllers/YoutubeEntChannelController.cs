using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TempoDigitalApex3.Models;

namespace TempoDigitalApex3.Controllers
{
    [Authorize]
    public class YoutubeEntChannelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: YoutubeEntChannel
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var entChannels = db.GetYoutubeChannelsENT.Where(a => a.UserId.Equals(userId) && a.Approved == true).ToList();
            ViewBag.EntChannelCount = entChannels.Count;
            ViewBag.ApprovedChannelsList = entChannels;

            var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
            ViewBag.UserType = user.UserType;

            return View();

        }

        // GET: YoutubeEntChannel/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: YoutubeEntChannel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: YoutubeEntChannel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Channel,ChannelName,ChannelID,Description,YT_Label_Name")] YoutubeChannelsENT youtubeChannel)
        {
            try
            {
                // TODO: Add insert logic here
                youtubeChannel.UserId = User.Identity.GetUserId();
                db.GetYoutubeChannelsENT.Add(youtubeChannel);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: YoutubeEntChannel/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: YoutubeEntChannel/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: YoutubeEntChannel/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: YoutubeChannel/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}