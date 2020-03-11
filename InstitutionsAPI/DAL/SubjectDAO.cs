using InstitutionsAPI.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionsAPI.DAL
{
    public class SubjectDAO : BaseDAO<Subject>
    {        
        public SubjectDAO(string tableName = "Subjects") : base(tableName)
        {
        }

        public new async Task<Subject> InsertAsync(Subject subject)
        {
            if (subject.Category.GetType() == typeof(string))
            {
                subject.Category = Convert.ToString(Convert.ToInt32(subject.Category));
            }
            else
            {
                var category = ((JObject)subject.Category).ToObject<SubjectCategory>();
                subject.Category = Convert.ToString(category.ID);
            }

            var result = await base.InsertAsync(subject);
            return result;
        }

        public new async Task<Subject> FindAsync(int ID)
        {
            var result = await base.FindAsync(ID);
            return result;
        }

        public new IEnumerable<Subject> GetAll()
        {
            var result = base.GetAll();
            return result;
        }

        public new async Task UpdateAsync(Subject subject)
        {
            // edit will be a little tricky  

            // a retrieval will happen first
            Subject entityToUpdate = await this.FindAsync(subject.ID);

            // we iterate through the values provided in the Subject parameter.
            // and set values for the entityToUpdate.

            // we take it one by one for now.
            if(subject.Name != null)
            {
                entityToUpdate.Name = subject.Name;
            }

            if (subject.Category.GetType() == typeof(string))
            {
                entityToUpdate.Category = Convert.ToString(Convert.ToInt32(subject.Category));
            }
            else
            {
                var category = ((JObject)subject.Category).ToObject<SubjectCategory>();
                entityToUpdate.Category = Convert.ToString(category.ID);
            }

            await base.UpdateAsync(entityToUpdate);
        }

        public new async Task DeleteAsync(Subject subject)
        {
            await base.DeleteAsync(subject);
        }
    }
}
