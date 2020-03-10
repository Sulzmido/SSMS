using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionsAPI.Utilities
{
    public class SerializationHelper
    {
        public static JsonSerializerSettings GetIgnnoreIDSerializerSetting(Type type)
        {
            var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();

            jsonResolver.IgnoreProperty(type, "ID");

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = jsonResolver;

            return serializerSettings;
        }
    }
}
