using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ERPNext_PowerPlay.Helpers
{
    public class JsonHelper
    {
        public static JsonElement GetJsonElement(JsonElement jsonElement, string path)
        {
            if (jsonElement.ValueKind == JsonValueKind.Null ||
                jsonElement.ValueKind == JsonValueKind.Undefined)
            {
                return default;
            }

            string[] segments =
                path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            for (int n = 0; n < segments.Length; n++)
            {
                jsonElement = jsonElement.TryGetProperty(segments[n], out JsonElement value) ? value : default;

                if (jsonElement.ValueKind == JsonValueKind.Null ||
                    jsonElement.ValueKind == JsonValueKind.Undefined)
                {
                    return default;
                }
            }

            return jsonElement;
        }
        public static string GetJsonElementValue(JsonElement jsonElement)
        {
            return
                jsonElement.ValueKind != JsonValueKind.Null &&
                jsonElement.ValueKind != JsonValueKind.Undefined ?
                jsonElement.ToString() :
                default;
        }
    }
}