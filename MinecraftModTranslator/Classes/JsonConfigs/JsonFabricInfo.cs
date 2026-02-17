using Newtonsoft.Json;

namespace MinecraftModTranslator.Classes.JsonConfigs
{
    public class JsonFabricInfo
    {
        public class Index
        {
            [JsonProperty("name")]
            public string? Name { get; set; }

            [JsonProperty("description")]
            public string? Description { get; set; }

            [JsonProperty("icon")]
            public string? Icon { get; set; }
        }
    }
}
