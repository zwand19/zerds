using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Zerds.Menus
{
    public class MainMenu
    {
        private readonly MenuList _menu;

        public MainMenu(Func<bool> playGameFunc)
        {
            _menu = new MenuList(new List<MenuListItem> {new MenuListItem("Play Game", playGameFunc), new MenuListItem("Options", OptionsFunc)});
        }

        public void Update()
        {
            _menu.Update();
        }

        private bool OptionsFunc()
        {
            // TODO: options screen!
            return true;
        }

        public void Draw()
        {
            Globals.SpriteDrawer.Begin();
            _menu.Draw(new Vector2(150, 300), 30f, 100f, Color.White, new Color(0.6f, 0.6f, 0.9f));
            "ZERDS".Draw(new Vector2(Globals.ViewportBounds.Width / 2.0f, 150), 50f, color: Color.White);
            Globals.SpriteDrawer.End();
        }
    }
}
