using Newtonsoft.Json;

namespace MinecraftModTranslator.Classes.JsonConfigs
{
    public class JsonFabricInfo
    {
        public class Index
        {
            [JsonProperty("name")]
            public string Name { get; set; } = "Name";

            [JsonProperty("description")]
            public string Description { get; set; } = "Description";

            [JsonProperty("icon")]
            public string Icon { get; set; } = "icon.png";
        }
    }
}
