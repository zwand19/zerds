using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Zerds.Factories
{
    public static class TextureCacheFactory
    {
        private static GraphicsDevice _graphicsDevice;
        private static Dictionary<string, Texture2D> _textures;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _textures = new Dictionary<string, Texture2D>();
        }
        
        public static Texture2D Get(string file)
        {
            if (_textures.Keys.Any(k => k == file))
                return _textures[file];
            using (var stream = TitleContainer.OpenStream($"Content/{file}"))
            {
                _textures[file] = Texture2D.FromStream(_graphicsDevice, stream);
                return _textures[file];
            }
        }

        public static Texture2D GetOnce(string file)
        {
            using (var stream = TitleContainer.OpenStream($"Content/{file}"))
            {
                return Texture2D.FromStream(_graphicsDevice, stream); ;
            }
        }

        public static void Clear(string file)
        {
            _textures.Remove(file);
        }
    }
}
