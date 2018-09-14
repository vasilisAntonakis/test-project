using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using WebAPI.Model;

namespace WebAPI.Services
{
    public class GenderService : Service<Gender>
    {
        public GenderService(DbContext context) : base(context) { }

        public IEnumerable<GenderSelect> GetSelect()
        {
            return Set.Select(g => new GenderSelect { GenderRefId = g.Id, Description = g.Description }).ToList();
        }
    }
}