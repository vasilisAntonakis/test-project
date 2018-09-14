using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Data;
using WebAPI.Model;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    public class GendersController : ApiController
    {
        // GET: api/Genders
        public IEnumerable<Gender> GetAll()
        {
            using (GenderService genderService = new GenderService(new DataContext()))
            {
                return genderService.GetAll();
            }
        }

        [Route("api/genders/select")]
        public IEnumerable<GenderSelect> GetSelect()
        {
            using (GenderService genderService = new GenderService(new DataContext()))
            {
                return genderService.GetSelect();
            }
        }

        // GET: api/Genders/5
        public Gender Get(int id)
        {
            using (GenderService genderService = new GenderService(new DataContext()))
            {
                return genderService.Get(id);
            }
        }

        // POST: api/Genders
        public HttpResponseMessage Post(Gender body)
        {
            using (GenderService genderService = new GenderService(new DataContext()))
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                genderService.Add(body);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        // PUT: api/Genders/5
        public HttpResponseMessage Put(int id, Gender body)
        {
            using (GenderService genderService = new GenderService(new DataContext()))
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                genderService.Update(id, body);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }
    }
}
