using InstitutionsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InstitutionsAPI.DAL
{
    public class SubjectCategoryDAO : BaseDAO<SubjectCategory>
    {        
        public SubjectCategoryDAO(string tableName = "SubjectCategories") : base(tableName)
        {
        }

        public new async Task<SubjectCategory> InsertAsync(SubjectCategory subjectCategory)
        {         
            var result = await base.InsertAsync(subjectCategory);
            return result;
        }

        public new async Task<SubjectCategory> FindAsync(int ID)
        {
            var result = await base.FindAsync(ID);
            return result;
        }

        public new IEnumerable<SubjectCategory> GetAll()
        {
            var result = base.GetAll();
            return result;
        }

        public new async Task UpdateAsync(SubjectCategory subjectCategory)
        {
            var entityToUpdate = await FindAsync(subjectCategory.ID);

            PropertyInfo[] props = typeof(SubjectCategory).GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == "ID")
                {
                    continue;
                }
                var value = prop.GetValue(subjectCategory);

                if (value != null)
                {
                    prop.SetValue(entityToUpdate, value);
                }

            }
                await base.UpdateAsync(subjectCategory);
        }

        public new async Task DeleteAsync(SubjectCategory subjectCategory)
        {
            await base.DeleteAsync(subjectCategory);
        }
    }
}
