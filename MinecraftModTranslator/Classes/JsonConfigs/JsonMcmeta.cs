using Newtonsoft.Json;

namespace MinecraftModTranslator.Classes.JsonConfigs
{
    public class JsonMcmeta
    {
        public static string MadeInfo = "§8§n§o= Minecraft Mod Translator =§r§r§r";

        public class Index
        {
            [JsonProperty("pack")]
            public Pack Pack { get; set; } = new Pack();
        }

        public class Pack
        {
            [JsonProperty("description")]
            public string Description { get; set; } = MadeInfo;

            [JsonProperty("min_format")]
            public int[] MinFormat { get; set; } = { 0, 0 };

            [JsonProperty("max_format")]
            public int[] MaxFormat { get; set; } = { 9999, 0 };

            [JsonProperty("pack_format")]
            public int PackFormat { get; set; } = 0;

            [JsonProperty("supported_formats")]
            public int[] SupportFormats { get; set; } = { 0, 9999 };
        }
    }
}
