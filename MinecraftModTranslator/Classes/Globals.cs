using MinecraftModTranslator.Classes.JsonConfigs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftModTranslator.Classes
{
    public static class Globals
    {
        public static string Version = "1.0.0-beta.1";//版本


        public static string? ModJarPath;//Jar路径
        public static string? ModRoot;//Mod根目录
        public static JsonFabricInfo.Index? ModFabricInfo;//Fabric信息
        public static List<string> ModLangDir = new List<string>();//语言文件目录列表
        public static string? ModName;//Mod名称
    }
}
