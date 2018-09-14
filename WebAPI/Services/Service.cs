using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WebAPI.Model;

namespace WebAPI.Services
{
    /// <summary>
    /// This is an abstract class provides common functionality among the derived services.
    /// It implements IDisposable interface to be able to explicitly call .Dispose() on Its DbContext.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Service<T> : IDisposable where T : Entity
    {
        protected DbContext Context;
        protected DbSet<T> Set;

        /// <summary>
        /// Service constructor needs a DbContext instance to execute queries in its DbSets.
        /// This is designed to be injected and not instatiated in the constructor (dependency injection)
        /// in order to be able to be mocked for testing.
        /// All derived classes will have to follow the same dependency injection schema in order for them to be
        /// able to have theis own unit tests with mocked DbContext.
        /// </summary>
        /// <param name="context">The injected DbContext instance</param>
        public Service(DbContext context)
        {
            Context = context;
            Set = context.Set<T>();
        }

        /// <summary>
        /// This is a generic method for a <T> entity.
        /// It retrieves a <T> entity from the DB.
        /// </summary>
        /// <param name="id">The Id of the <T> entity</param>
        /// <returns>The <T> entity with the matching id</returns>
        public virtual T Get(int id)
        {
            return Set.Find(id);
        }

        /// <summary>
        /// This is a generic method for a <T> entity.
        /// It retrieves a list of <T> entities from the DB.
        /// </summary>
        /// <returns>A list of <T> entities</returns>
        public virtual IEnumerable<T> GetAll()
        {
            // we are using .ToList() instead of .AsEnumerable() to force execute the sql select query (eager loading).
            // this is done in order to fetch the data in memory and be able to dispose the DbContext after that.
            return Set.ToList();
        }

        /// <summary>
        /// This is a generic method for a <T> entity.
        /// It persists a new <T> entity in the DB.
        /// </summary>
        /// <param name="entity">The <T> entity to be persisted</param>
        public virtual void Add(T entity)
        {
            Set.Add(entity);
            Context.SaveChanges();
        }

        /// <summary>
        /// This is a generic method for a <T> entity.
        /// It updates an old <T> entity in the DB.
        /// By default it throws a NotImplementedException.
        /// The reason for that is because you cannot update a generic entity.
        /// Appart from that, it is dangerous to have generic functionality for updates.
        /// The reason this function exists as virtual (and not abstract) is to avoid copy-pasting in all derived classes.
        /// </summary>
        /// <param name="id">The id of the <T> existing entity</param>
        /// <param name="entity">The new <T> entity for the update</param>
        public virtual void Update(int id, T entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function explicitly calls the .Dispose() method on its DbContext if that is not disposed.
        /// </summary>
        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }
    }
}