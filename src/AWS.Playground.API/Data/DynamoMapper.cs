using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AWS.Playground.API.Data
{
    public static class DynamoMapperExtensions
    {
        public static Document ToDocument<T>(this T item)
        {
            var document = new Document();

            foreach (var property in typeof(T).GetProperties())
            {
                var value = property.GetValue(item);

                if (value != null)
                {
                    document[property.Name] = (DynamoDBEntry)value;
                }
            }

            return document;
        }

        public static T FromDocument<T>(this Document document) where T : new()
        {
            var item = new T();

            foreach (var property in typeof(T).GetProperties())
            {
                if (document.TryGetValue(property.Name, out var value))
                {
                    property.SetValue(item, value);
                }
            }

            return item;
        }
    }
}
