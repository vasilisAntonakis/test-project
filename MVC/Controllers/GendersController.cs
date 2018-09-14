using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebAPI.Model;

namespace MVC.Controllers
{
    public class GendersController : Controller
    {

        // GET: Genders
        public ActionResult Index()
        {
            return View(ApiConsumer.Get<List<Gender>>("genders"));
        }

        public ActionResult Create()
        {
            Gender model = new Gender
            {
                DateRecStarted = DateTime.Now,
                LoginRecStarted = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Gender body)
        {
            if (ModelState.IsValid && ApiConsumer.Post("/genders", body).IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View(body);
        }
    }
}