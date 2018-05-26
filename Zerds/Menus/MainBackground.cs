using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Zerds.Factories;


namespace Zerds.Menus
{
    public class MainBackground
    {
        private readonly Texture2D _background;
        private readonly Texture2D _backgroundRed;
        private readonly Texture2D _backgroundGreen;
        private float _elapsedMilliseconds;

        public MainBackground()
        {
            _background = TextureCacheFactory.GetOnce("Backgrounds/sky.png");
            _backgroundGreen = TextureCacheFactory.GetOnce("Backgrounds/sky-green.png");
            _backgroundRed = TextureCacheFactory.GetOnce("Backgrounds/sky-red.png");
        }

        public void Update(GameTime elapsedGameTime)
        {
            _elapsedMilliseconds += elapsedGameTime.ElapsedGameTime.Milliseconds;
        }

        public void Draw()
        {
            Globals.SpriteDrawer.Begin();
            var thousandth = _elapsedMilliseconds % 14000;
            double opacity = 0;
            if (thousandth < 4000)
            {
                var factor = thousandth < 2000 ? thousandth / 2000 : (4000 - thousandth) / 2000;
                opacity = Math.Pow(factor, 0.5) * 0.5;
            }
            else if (thousandth > 8000 && thousandth < 12000)
            {
                var factor = thousandth < 10000 ? (thousandth - 8000) / 2000 : (12000 - thousandth) / 2000;
                opacity = Math.Pow(factor, 0.5) * 0.5;
            }

            Globals.SpriteDrawer.Draw(_background, Globals.ViewportBounds.Scale(1.2f), Color.White);
            if (opacity > 0 && thousandth < 4000)
                Globals.SpriteDrawer.Draw(_backgroundGreen, Globals.ViewportBounds.Scale(1.2f), Color.White * (float)opacity);
            if (opacity > 0 && thousandth > 8000 && thousandth < 12000)
                Globals.SpriteDrawer.Draw(_backgroundRed, Globals.ViewportBounds.Scale(1.2f), Color.White * (float)opacity);

            Globals.SpriteDrawer.Draw(Globals.WhiteTexture, Globals.ViewportBounds, new Color(0, 0, 0, 0.75f));
            Globals.SpriteDrawer.End();
        }
    }
}
