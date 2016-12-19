using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zerds.Constants;
using Zerds.Input;

namespace Zerds.Menus
{
    public class MenuList
    {
        public List<MenuListItem> Items { get; set; }
        public MenuListItem Selected => Items[_selectedIndex];
        public List<PlayerIndex> Players { get; set; }

        private int _selectedIndex;

        public MenuList(List<MenuListItem> items)
        {
            Items = items;
        }

        public void Update()
        {
            CheckController(ControllerService.Controllers[PlayerIndex.One]);
            CheckController(ControllerService.Controllers[PlayerIndex.Two]);
            CheckController(ControllerService.Controllers[PlayerIndex.Three]);
            CheckController(ControllerService.Controllers[PlayerIndex.Four]);
        }

        public void CheckController(Controller controller)
        {
            var buttonsPressed = controller.ButtonsPressed;
            if (buttonsPressed.Contains(Buttons.A))
                Selected.Callback();
            if (_selectedIndex < Items.Count - 1 && buttonsPressed.Contains(Buttons.LeftThumbstickDown))
                _selectedIndex++;
            if (_selectedIndex > 0 && buttonsPressed.Contains(Buttons.LeftThumbstickUp))
                _selectedIndex--;
        }

        public void Draw(Vector2 position, float fontSize, float spacing, Color fontColor, Color selectedColor)
        {
            for (var i = 0; i < Items.Count; i++)
            {
                var text = _selectedIndex == i ? $"> {Items[i].Text}" : Items[i].Text;
                Globals.SpriteDrawer.DrawTextLeftAlign(text, position, fontSize, color: _selectedIndex == i ? selectedColor : fontColor);
                position.Y += spacing;
            }
        }
    }
}
