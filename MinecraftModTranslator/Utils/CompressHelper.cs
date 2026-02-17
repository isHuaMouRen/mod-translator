using System.IO;
using System.IO.Compression;

namespace MinecraftModTranslator.Utils
{
    public static class CompressHelper
    {
        /// <summary>
        /// 解压压缩文档
        /// </summary>
        /// <param name="archivePath">文档位置</param>
        /// <param name="destinationFolder">解压到的文件夹</param>
        /// <returns></returns>
        public static async Task Extract(string archivePath, string destinationFolder)
        {
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            await ZipFile.ExtractToDirectoryAsync(archivePath, destinationFolder);
        }

        /// <summary>
        /// 压缩文件夹到压缩文档
        /// </summary>
        /// <param name="sourceDirectory">文件夹</param>
        /// <param name="zipFilePath">要压缩到的文件</param>
        /// <param name="compressionLevel">压缩等级</param>
        /// <param name="includeBaseDirectory">包含基础文件夹</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void Compress(string sourceDirectory,string zipFilePath,CompressionLevel compressionLevel = CompressionLevel.Optimal,bool includeBaseDirectory = false)
        {
            if (!Directory.Exists(sourceDirectory))
                throw new DirectoryNotFoundException($"源文件夹不存在：{sourceDirectory}");

            if (!Directory.Exists(Path.GetDirectoryName(zipFilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(zipFilePath)!);

            ZipFile.CreateFromDirectoryAsync(sourceDirectory,zipFilePath,compressionLevel,includeBaseDirectory);
        }
    }
}
