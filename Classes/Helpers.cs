using System.Diagnostics;
using System.IO;
using Pfim;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace HgmViewer.Classes
{
    public static class Helpers
    {
        public static (bool isSubPath, string relPath) GetRelativePathOf(string basePath, string subPath)
        {
            if (subPath == null) return (false, null);

            var rel = Path.GetRelativePath(basePath.Replace('\\', '/'), subPath.Replace('\\', '/'));

            var isSubPath = rel != "."
                && rel != ".."
                && !rel.StartsWith("..\\")
                && !rel.StartsWith("../")
                && !Path.IsPathRooted(rel);

            return (isSubPath, rel);
        }

        public static bool IsSubPathOf(string basePath, string subPath)
        {
            return GetRelativePathOf(basePath, subPath).isSubPath;
        }

        public static void ConvertDDS(string inputPath, string outputPath)
        {
            var img = Pfimage.FromFile(inputPath);
            if (img.Compressed) img.Decompress();

            if (img.Format == ImageFormat.Rgba32)
                ConvertDDS_Save<Bgra32>(img, outputPath);
            else if (img.Format == ImageFormat.Rgb24)
                ConvertDDS_Save<Bgr24>(img, outputPath);
            else if (img.Format == ImageFormat.Rgb8)
                ConvertDDS_Save<L8>(img, outputPath);
            else
                Debug.WriteLine($"Unsupported pixel format: {img.Format}");
        }

        private static void ConvertDDS_Save<TPixel>(Pfim.IImage img, string outputPath)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            try
            {
                var image = Image.LoadPixelData<TPixel>(
                img.Data, img.Width, img.Height);
                image.Save(outputPath);
            }
            catch (IOException ex) when (ex.HResult == -2147024864)
            {
                // file open in another process
                Debug.WriteLine($"ConvertDDS_Save exception (\"{outputPath}\"): {ex.Message}");
            }
        }
    }
}