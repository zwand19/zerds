using System;

namespace Zerds.Menus
{
    public class MenuListItem
    {
        public Func<bool> Callback { get; }
        public string Text { get; }

        public MenuListItem(string text, Func<bool> callback)
        {
            Text = text;
            Callback = callback;
        }
    }
}
