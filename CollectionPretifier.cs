using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NumericalTechnologyLtdTestTask
{
    enum format { Default};
    public class FormatAttribute : Attribute
    {
        private format format;

        public format Format 
        FormatAttribute(string? format)
        {
            if (format == null)
                this.format = NumericalTechnologyLtdTestTask.format.Default;
            switch(format)
            {
                case "":
                case "default":
                    this.format = NumericalTechnologyLtdTestTask.format.Default;
                    break;
                default:throw new ArgumentException("Invalid parametr format");
            }

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
            format format;
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
        collectionsInformation = sb.ToString();
        }
    }
}
