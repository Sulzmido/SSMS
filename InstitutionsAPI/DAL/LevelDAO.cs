using InstitutionsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InstitutionsAPI.DAL
{
    public class LevelDAO : BaseDAO<Level>
    {
        public LevelDAO(string tableName = "Level") : base(tableName)
        {
        }
        public new async Task<Level> InsertAsync(Level level)
        {
            var result = await base.InsertAsync(level);
            return result;
        }
        public new async Task<Level> FindAsync(int ID)
        {
            var result = await base.FindAsync(ID);
            return result;
        }
        public new IEnumerable<Level> GetAll()
        {
            var result = base.GetAll();
            return result;
        }
        public new async Task UpdateAsync(Level level)
        {
            var entityToUpdate = await FindAsync(level.ID);

            PropertyInfo[] props = typeof(Level).GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == "ID")
                {
                    continue;
                }
                var value = prop.GetValue(level);

                if (value != null)
                {
                    prop.SetValue(entityToUpdate, value);
                }

            }
            await base.UpdateAsync(level);

        }
        public new async Task DeleteAsync(Level level)
        {
            await base.DeleteAsync(level);
        }
    }
}
