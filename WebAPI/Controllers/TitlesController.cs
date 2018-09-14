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
    public class TitlesController : ApiController
    {
        // GET: api/Titles
        public IEnumerable<Title> GetAll()
        {
            using (TitleService titleService = new TitleService(new DataContext()))
            {
                return titleService.GetAll();
            }
        }

        [Route("api/titles/select")]
        public IEnumerable<TitleSelect> GetSelect()
        {
            using (TitleService titleService = new TitleService(new DataContext()))
            {
                return titleService.GetSelect();
            }
        }

        // GET: api/Titles/5
        public Title Get(int id)
        {
            using (TitleService titleService = new TitleService(new DataContext()))
            {
                return titleService.Get(id);
            }
        }

        // POST: api/Titles
        public HttpResponseMessage Post(Title body)
        {
            using (TitleService titleService = new TitleService(new DataContext()))
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                titleService.Add(body);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        // PUT: api/Titles/5
        public HttpResponseMessage Put(int id, Title body)
        {
            using (TitleService titleService = new TitleService(new DataContext()))
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                titleService.Update(id, body);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }
    }
}
