using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TempoDigitalApex3.Models;

namespace TempoDigitalApex3.Controllers
{
    [Authorize]
    public class YoutubeChannelController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: YoutubeChannel
        public ActionResult Index()
        {
            var userId =  User.Identity.GetUserId();
            var Channels = db.YoutubeChannels.Where(a => a.UserId.Equals(userId) && a.Approved == true).ToList();
            
            ViewBag.ChannelCount = Channels.Count;

            ViewBag.ApprovedChannelsList = Channels;

            var user = db.Users.Where(a => a.Id.Equals(userId)).FirstOrDefault();
            ViewBag.UserType = user.UserType;

            return View();

        }

        // GET: YoutubeChannel/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: YoutubeChannel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: YoutubeChannel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Channel,ChannelName,ChannelID,Description,YT_Label_Name")] YoutubeChannel youtubeChannel)
        {
            try
            {
                // TODO: Add insert logic here
                youtubeChannel.UserId = User.Identity.GetUserId();
                youtubeChannel.Approved = false;
                youtubeChannel.ApprovelDate = "";
                db.YoutubeChannels.Add(youtubeChannel);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: YoutubeChannel/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: YoutubeChannel/Edit/5
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

        // GET: YoutubeChannel/Delete/5
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
