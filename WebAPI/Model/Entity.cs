using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Model
{
    /// <summary>
    /// This is an abstract class inherited by all concrete entities.
    /// It is used to expose the common attribute Id for unit testing and service abstraction.
    /// </summary>
    public abstract class Entity
    {
        public abstract int Id { get; set; }
    }
}