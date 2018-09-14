using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Model;

namespace MVC.Controllers
{
    public class LawyersController : Controller
    {

        public ActionResult Index()
        {
            ViewData.Add("parameters", new LawyerSearchParameters());
            return View(ApiConsumer.Get<List<Lawyer>>(string.Format("Lawyers")));
        }

        [HttpPost]
        public ActionResult Index(LawyerSearchParameters searchParameters)
        {
            if (searchParameters == null)
                searchParameters = new LawyerSearchParameters();
            ViewData.Add("parameters", searchParameters);
            return View(ApiConsumer.Get<List<Lawyer>>(string.Format("Lawyers{0}", searchParameters.ToString())));
        }


        private void prepareSelects()
        {
            List<GenderSelect> genders = ApiConsumer.Get<List<GenderSelect>>("genders/select");
            List<TitleSelect> titles = ApiConsumer.Get<List<TitleSelect>>("titles/select");

            SelectList genderOptions = new SelectList(genders, "GenderRefId", "Description");
            SelectList titleOptions = new SelectList(titles, "TitleRefId", "Description");

            ViewData.Add("genders", genderOptions);
            ViewData.Add("titles", titleOptions);
        }

        public ActionResult Create()
        {
            prepareSelects();
            return View(new Lawyer());
        }

        [HttpPost]
        public ActionResult Create(Lawyer body)
        {
            if (ModelState.IsValid && ApiConsumer.Post("/lawyers", body).IsSuccessStatusCode)
                return RedirectToAction("Index");

            prepareSelects();
            return View(body);
        }

        public ActionResult Edit(int Id)
        {
            Lawyer lawyer = ApiConsumer.Get<Lawyer>(string.Format("/lawyers/{0}", Id));

            if (lawyer == null)
            {
                return RedirectToAction("NotFound", "Errors");
            }

            if (lawyer.Active == false)
            {
                return RedirectToAction("AccessDenied", "Errors");
            }

            prepareSelects();
            return View(lawyer);
        }

        [HttpPut]
        public ActionResult Edit(int Id, Lawyer body)
        {
            if (ModelState.IsValid && ApiConsumer.Put(string.Format("/lawyers/{0}", Id), body).IsSuccessStatusCode)
                return RedirectToAction("Index");

            prepareSelects();
            return View(body);
        }

    }
}