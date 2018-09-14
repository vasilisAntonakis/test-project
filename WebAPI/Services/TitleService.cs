using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using WebAPI.Model;

namespace WebAPI.Services
{
    public class TitleService : Service<Title>
    {
        public TitleService(DbContext context) : base(context) { }

        public IEnumerable<TitleSelect> GetSelect()
        {
            return Set.Select(g => new TitleSelect { TitleRefId = g.Id, Description = g.Description }).ToList();
        }
    }
}