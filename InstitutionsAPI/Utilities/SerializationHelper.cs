using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InstitutionsAPI.Utilities
{
    public class SerializationHelper
    {
        public static JsonSerializerSettings GetIgnoreIDSerializerSetting(Type type)
        {
            var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();

            jsonResolver.IgnoreProperty(type, "ID");

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = jsonResolver;

            return serializerSettings;
        }

        public static T UnboxEntity<T>(IDictionary<string, object> boxedEntity)
        {
            var serializedData = Convert.ToString(boxedEntity["SerializedEntity"]);

            int ID = Convert.ToInt32(boxedEntity["ID"]);

            var data = JsonConvert.DeserializeObject<T>(serializedData);
            
            PropertyInfo idProperty = data.GetType().GetProperty("ID");
            idProperty.SetValue(data, ID);

            return data;
        }
    }
}
