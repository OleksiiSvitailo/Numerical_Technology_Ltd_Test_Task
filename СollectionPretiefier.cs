using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace NumericalTechnologyLtdTestTask
{

    public enum CustomSerializationType
    {
        Default,
        Json,
        Full
    }

    public class CustomSerializeAttribute : Attribute
    {
        public CustomSerializationType Type { get; set; }
        public CustomSerializeAttribute(CustomSerializationType type = CustomSerializationType.Default)
        {
            Type = type;
        }
    }

    public class CollectionSerializer
    {
        public static IEnumerable<IEnumerable<string>> ToStrings<T>(IEnumerable<T> src)
        {
            var props = typeof(T).GetProperties()
                .ToList();

            return src.Select(itm => SerializeFields(itm, props));
        }

        private static IEnumerable<string> SerializeFields<T>(T src, IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.Select(itm =>
            {
                var fieldName = itm.Name;
                var fieldValue = itm.GetValue(src)?.ToString();
                var attr =
                    itm.GetCustomAttributes(true).FirstOrDefault(itm => itm is CustomSerializeAttribute) as
                        CustomSerializeAttribute;

                return attr == null ? Serialize(fieldName, fieldValue, CustomSerializationType.Default)
                    : Serialize(fieldName, fieldValue, attr.Type);
            });
        }

        private static string Serialize(string key, string value, CustomSerializationType type)
        {
            switch (type)
            {
                case CustomSerializationType.Json:
                    return $"\"{key}\": \"{value}\"";
                case CustomSerializationType.Full:
                    return $"{key}={value}";
                default:
                    return $"{key}:{value}";
            }
        }
    }
    public class CollectionPretifier<T>
    {
        private string collectionsInfo;

        public string CollectionsInfo { get; }

        public CollectionPretifier(IEnumerable<T> src)
        {
            collectionsInfo = CollectionSerializer.ToStrings(src).Select(itm => string.Join(";", itm)).ToString();
        }
    }
}