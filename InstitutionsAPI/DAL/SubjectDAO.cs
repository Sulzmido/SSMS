using InstitutionsAPI.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            if (subject.Category != null)
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
            }

            if (subject.Level != null)
            {
                if (subject.Level.GetType() == typeof(string))
                {
                    subject.Level = Convert.ToString(Convert.ToInt32(subject.Level));
                }
                else
                {
                    var category = ((JObject)subject.Level).ToObject<SubjectCategory>();
                    subject.Level = Convert.ToString(category.ID);
                }
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
            if (subject.Category != null)
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
            }

            if (subject.Level != null)
            {
                if (subject.Level.GetType() == typeof(string))
                {
                    subject.Level = Convert.ToString(Convert.ToInt32(subject.Level));
                }
                else
                {
                    var category = ((JObject)subject.Level).ToObject<SubjectCategory>();
                    subject.Level = Convert.ToString(category.ID);
                }
            }

            var entityToUpdate = await this.FindAsync(subject.ID);
            
            PropertyInfo[] props = typeof(Subject).GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == "ID")
                {
                    continue;
                }

                var value = prop.GetValue(subject);

                if (value != null)
                {
                    prop.SetValue(entityToUpdate, value);
                }
                
            }
           
            await base.UpdateAsync(entityToUpdate);
        }

        public new async Task DeleteAsync(Subject subject)
        {
            await base.DeleteAsync(subject);
        }
    }
}
