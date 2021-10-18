using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SKPLager.Shared.Converters
{

    public class PagedListJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(PagedList<>);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert.IsGenericType &&
                typeToConvert.GetGenericTypeDefinition() == typeof(PagedList<>));

            Type elementType = typeToConvert.GetGenericArguments()[0];

            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(PagedListJsonConverter<>)
                    .MakeGenericType(new Type[] { elementType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null)!;

            return converter;
        }
    }



    public class PagedListJsonConverter<T> : JsonConverter<PagedList<T>>
    {
        public override PagedList<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            reader.Read();
            var list = new PagedList<T>();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }
            string propertyName = reader.GetString();
            if (propertyName.ToLower() != "paging")
            {
                throw new JsonException();
            }
            reader.Read();
            list.Paging = JsonSerializer.Deserialize<Pagination>(ref reader, options);
            reader.Read();
            propertyName = reader.GetString();
            if (propertyName.ToLower() != "items")
            {
                throw new JsonException();
            }
            reader.Read();
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                list.Add(JsonSerializer.Deserialize<T>(ref reader, options));

                reader.Read();
            }
            reader.Read();
            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }
            return list;
        }

        public override void Write(Utf8JsonWriter writer, PagedList<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Paging");
            JsonSerializer.Serialize(writer, value.Paging, options);
            writer.WriteStartArray("items");
            foreach (T item in value)
            {
                JsonSerializer.Serialize(writer, item, options);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
