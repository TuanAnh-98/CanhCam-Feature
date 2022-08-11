using Newtonsoft.Json.Converters;
using System;
using Newtonsoft.Json;

namespace CanhCam.Web.ProductUI
{
    public class TickDateTimeConverter : DateTimeConverterBase
    {
        private static DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            long tick = (long)reader.Value;

            return unixEpoch.AddMilliseconds(tick).ToLocalTime();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return base.CanConvert(objectType);
        }
    }
}
