using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SchoolManager.Extensions
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
                var propertyName = property.Name.ToLower();               

                if (source.ContainsKey(propertyName))// && property.PropertyType == source[propertyName].GetType())
                {
                    property.SetValue(targetObject, source[propertyName]);
                }
            }

            return targetObject;
        }
    }
}
