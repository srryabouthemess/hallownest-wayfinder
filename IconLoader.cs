using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace HallownestWayfinder
{
    internal static class IconLoader
    {
        private static readonly Dictionary<string, Texture2D> Cache = new Dictionary<string, Texture2D>();

        public static Texture2D Get(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            if (Cache.TryGetValue(fileName, out Texture2D cached)) return cached;

            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"HallownestWayfinder.Assets.{fileName}";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return null;
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);

                Texture2D texture = new Texture2D(2, 2);
                texture.name = "HallownestWayfinder " + fileName;
                ImageConversion.LoadImage(texture, data, false);
                Cache[fileName] = texture;
                return texture;
            }
        }

        public static void Unload()
        {
            foreach (Texture2D texture in Cache.Values)
            {
                if (texture != null) Object.Destroy(texture);
            }
            Cache.Clear();
        }
    }
}

