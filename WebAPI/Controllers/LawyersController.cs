using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Services;
using System.Web;
using WebAPI.Data;
using WebAPI.Model;

namespace WebAPI.Controllers
{
    public class LawyersController : ApiController
    {
        // by convention routing: GET/api.lawyers/{id}
        public Lawyer Get(int id)
        {
            using (LawyerService lawyerService = new LawyerService(new DataContext()))
            {
                return lawyerService.Get(id);
            }
        }

        // by convention routing: GET/api.lawyers?includeGender=<bool>&includeTitle=<bool>&includeInactive=<bool>  automagically!
        public IEnumerable<Lawyer> GetAll([FromUri] LawyerSearchParameters lawyerSearchParameters)
        {
            using (LawyerService lawyerService = new LawyerService(new DataContext()))
            {
                return lawyerService.GetAll(lawyerSearchParameters);
            }
        }

        // by convention routing: POST/api.lawyers
        public HttpResponseMessage Post(Lawyer body)
        {
            using (LawyerService lawyerService = new LawyerService(new DataContext()))
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                lawyerService.Add(body);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        // by convention routing: PUT/api.lawyers
        public HttpResponseMessage Put(int id, Lawyer body)
        {
            using (LawyerService lawyerService = new LawyerService(new DataContext()))
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                try
                {
                    lawyerService.Update(id, body);
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                catch (NullReferenceException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
                }
                catch (HttpRequestValidationException ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, ex.Message);
                }
            }
        }
    }
}
