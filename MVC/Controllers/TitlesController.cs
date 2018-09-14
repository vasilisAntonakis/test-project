using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Model;

namespace MVC.Controllers
{
    public class TitlesController : Controller
    {
        // GET: Titles
        public ActionResult Index()
        {
            return View(ApiConsumer.Get<List<Title>>("titles"));
        }

        public ActionResult Create()
        {
            Title model = new Title
            {
                DateRecStarted = DateTime.Now,
                LoginRecStarted = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Title body)
        {
            if (ModelState.IsValid && ApiConsumer.Post("/titles", body).IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View(body);
        }
    }
}