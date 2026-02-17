using Newtonsoft.Json;

namespace MinecraftModTranslator.Classes.JsonConfigs
{
    public class JsonMcmetaOld
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

            [JsonProperty("pack_format")]
            public int PackFormat { get; set; } = 1;

            [JsonProperty("supported_formats")]
            public int[] SupportFormats { get; set; } = { 1, 64 };
        }
    }
}
