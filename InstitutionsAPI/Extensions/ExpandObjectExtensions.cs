using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace InstitutionsAPI.Extensions
{
    public static class ExpandObjectExtensions
    {
        public static TObject ToObject<TObject>(this IDictionary<string, object> source, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
                where TObject : class, new()
        {
            Contract.Requires(source != null);
            TObject targetObject = new TObject();
            Type targetObjectType = typeof(TObject);

            foreach (PropertyInfo property in
                    targetObjectType.GetProperties(bindingFlags))
            {
                if (source.ContainsKey(property.Name)
                    && property.PropertyType == source[property.Name].GetType())
                {
                    property.SetValue(targetObject, source[property.Name]);
                }
            }

            return targetObject;
        }
    }
}
