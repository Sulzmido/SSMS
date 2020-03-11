using InstitutionsAPI.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InstitutionsAPI.DAL
{
    public class BaseDAO<T>
    {
        private readonly string _tableName;

        public string ConnectionString { get; set; }

        public BaseDAO(string tableName)
        {
            _tableName = tableName;
        }

        protected async Task<T> InsertAsync(T entity)
        {
            var tableExists = DatabaseHelper.ExecuteTableCheck(ConnectionString, _tableName);

            if (!tableExists)
            {
                DatabaseHelper.CreateTable(ConnectionString, _tableName);
            }

            var serializerSettings = SerializationHelper.GetIgnoreIDSerializerSetting(typeof(T));
            var serializedEntity = JsonConvert.SerializeObject(entity, serializerSettings);

            int newEntryID = await DatabaseHelper.ExecuteInsertQueryAsync(ConnectionString, $@"INSERT INTO [dbo].[{_tableName}]
                                                                    ([SerializedEntity]) output INSERTED.ID VALUES ('{serializedEntity}')");
            PropertyInfo idProperty = typeof(T).GetProperty("ID");
            idProperty.SetValue(entity, newEntryID);
            return entity;
        }

        protected IEnumerable<T> GetAll()
        {
            var data = DatabaseHelper.ExecuteSelectQuery(ConnectionString, $"Select * from [dbo].[{_tableName}]");
            return data.Select(s => SerializationHelper.UnboxEntity<T>(s));
        }

        protected async Task<T> FindAsync(int ID)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data = await DatabaseHelper.ExecuteSelectFindQueryAsync(ConnectionString, $"Select * from [dbo].[{_tableName}] where ID={ID}");
            var entity = SerializationHelper.UnboxEntity<T>(data);
            return entity;

        }

        protected async Task UpdateAsync(T entity)
        {
            var serializerSettings = SerializationHelper.GetIgnoreIDSerializerSetting(typeof(T));
            var serializedEntity = JsonConvert.SerializeObject(entity, serializerSettings);

            PropertyInfo idProperty = typeof(T).GetProperty("ID");
            int id = Convert.ToInt32(idProperty.GetValue(entity));

            await DatabaseHelper.ExecuteQueryAsync(ConnectionString, $@"Update [dbo].[{_tableName}] set [SerializedEntity] = '{serializedEntity}' where ID = {id}");
        }

        protected async Task DeleteAsync(T entity)
        {
            PropertyInfo idProperty = typeof(T).GetProperty("ID");
            int id = Convert.ToInt32(idProperty.GetValue(entity));

            await DatabaseHelper.ExecuteQueryAsync(ConnectionString, $@"DELETE FROM [dbo].[{_tableName}] WHERE ID='{id}'");
        }

    }
}