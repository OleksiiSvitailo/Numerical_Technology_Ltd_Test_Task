using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NumericalTechnologyLtdTestTask
{
    public class FormatAttribute : Attribute
    {
        enum format;

        FormatAttribute(string format)
        {
            this.format = format;
        }
    }

    public class CollectionPretifier
    {
        private string collectionsInformation;

        public string CollectionsInformation { get; }

        public CollectionPretifier(IEnumerable collection)
        {
            var fields = collection.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            var properties = collection.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            StringBuilder sb = new StringBuilder();
            foreach (var field in fields)
            {
                var value = field.GetValue(collection);
                sb.Append(field.GetType().Name + " " + field.Name + ":" + value);
            }
            foreach (var property in properties)
            {
                var value = property.GetValue(collection);
                sb.Append(property.GetType().Name + " " + property.Name + ":" + value);
            }
        }
    }
}
