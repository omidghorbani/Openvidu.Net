using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Openvidu.Net.Core
{
    public class JsonDateTimeConverter : JsonConverter<object>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.Name == nameof(DateTime))
            {
                return true;
            }

            return false;
        }

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {

                return new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(reader.GetInt64());

            }
            catch (Exception e)
            {
                return new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(0);

            }
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            if (value.GetType().Name == nameof(DateTime))
                if (value != null)
                    writer.WriteStringValue("");
        }

    }
}
