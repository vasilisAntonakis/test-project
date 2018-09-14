using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebAPI.Model;

namespace WebAPI.Services
{
    public class LawyerService : Service<Lawyer>
    {
        public LawyerService(DbContext context) : base(context) { }

        /// <summary>
        /// retrieves the default list of all active lawyers.
        /// </summary>
        /// <returns>all active lawyers</returns>
        public override IEnumerable<Lawyer> GetAll()
        {
            return Set.Where(l => l.Active == true).ToList();
        }

        /// <summary>
        /// retrieves a list of lawyers with the specified parameters.
        /// </summary>
        /// <param name="IncludeGender">if true lawyer will include Gender entity</param>
        /// <param name="IncludeTitle">if true lawyer will include Title entity</param>
        /// <param name="IncludeInactive">if true the list will contain all records from the DB. Not just the active.</param>
        /// <param name="Name">if a name is provided the list will be filtered with the lawyers with that name</param>
        /// <param name="Surname">if a surname is provided the list will be filtered with the lawyers with that surname</param>
        /// <returns>A list with all Lawyer records that are active</returns>
        public IEnumerable<Lawyer> GetAll(LawyerSearchParameters lawyerSearchParameters)
        {
            // IQueryable will create the sql query but it will not execute it. It queries the DB lazily when needed.
            // DbSet from EF is smart enough to know how to convert the set to a queryable. Both share the same lazy principal.
            IQueryable<Lawyer> query = Set;

            // if caller needs only the active records, we include a where clause in the query.
            if (!lawyerSearchParameters.IncludeInactive)
                query = Set.Where(l => l.Active == true);

            // if caller needs gender included, query will include it by joining the tables with the gender FK.
            if (lawyerSearchParameters.IncludeGender)
                query = query.Include(s => s.Gender);

            // same goes for the title.
            if (lawyerSearchParameters.IncludeTitle)
                query = query.Include(s => s.Title);

            // if both name and surname are provided, query will include all records matching with either of them.
            if (!string.IsNullOrWhiteSpace(lawyerSearchParameters.Name) && !string.IsNullOrWhiteSpace(lawyerSearchParameters.Surname))
            {
                lawyerSearchParameters.Name = lawyerSearchParameters.Name.ToLower();
                lawyerSearchParameters.Surname = lawyerSearchParameters.Surname.ToLower();
                query = query.Where(l => l.Name.ToLower().Contains(lawyerSearchParameters.Name) || l.Surname.ToLower().Contains(lawyerSearchParameters.Surname));
            }

            else if (!string.IsNullOrWhiteSpace(lawyerSearchParameters.Name))
            {
                lawyerSearchParameters.Name = lawyerSearchParameters.Name.ToLower();
                query = query.Where(l => l.Name.ToLower().Contains(lawyerSearchParameters.Name));
            }

            else if (!string.IsNullOrWhiteSpace(lawyerSearchParameters.Surname))
            {
                lawyerSearchParameters.Surname = lawyerSearchParameters.Surname.ToLower();
                query = query.Where(l => l.Surname.ToLower().Contains(lawyerSearchParameters.Surname));
            }

            // finally we execute the query with ToList(). This method executes the IQueryable immediately and converts the result to a List(IEnumerable).
            return query.ToList();
        }

        /// <summary>
        /// It makes sure the new lawyer's state is active and persists him/her in the DB.
        /// </summary>
        /// <param name="entity"></param>
        public override void Add(Lawyer entity)
        {
            entity.Active = true;
            base.Add(entity);
        }

        /// <summary>
        /// It updates an existing Lawyer entity in the DB.
        /// For history recording it actually finds the entity by Id and changes its status to inactive.
        /// Then, It persists a new active record that represents the updated version of the old one
        /// </summary>
        /// <param name="id">The Id of the existing Lawyer entity to be updated</param>
        /// <param name="modifiedLawyer">The modified Lawyer entity to be stored</param>
        public override void Update(int id, Lawyer modifiedLawyer)
        {
            Lawyer currentRecord = Set.Find(id);

            if (currentRecord == null)
            {
                throw new NullReferenceException("Lawyer not found.");
            }

            if (currentRecord.Active == false)
            {
                throw new HttpRequestValidationException(string.Format("the lawyer record with id: '{0}' is archived.", id));
            }

            currentRecord.Active = false;
            Add(modifiedLawyer);
        }
    }
}