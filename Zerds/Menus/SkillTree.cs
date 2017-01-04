using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zerds.Input;

namespace Zerds.Menus
{
    public class SkillTree
    {
        public string Name { get; set; }
        public int PointsSpent => Items.Sum(i => i.PointsSpent);
        public int PointsAvailable { get; set; }
        public List<SkillTreeItem> Items { get; set; }
        public SkillTreeItem Selected { get; set; }

        private readonly PlayerIndex _playerIndex;
        private const int NumRows = 6;
        private const int NumCols = 5;
        private const int InfoHeight = 320;
        private const int Padding = 30;

        public SkillTree(string name, PlayerIndex playerIndex)
        {
            Name = name;
            _playerIndex = playerIndex;
            Items = new List<SkillTreeItem>();
        }

        public void Draw(Rectangle bounds)
        {
            Selected = Selected ?? Items.First();
            Items.ForEach(i => i.DrawDependencies(bounds));
            Items.ForEach(i => i.Draw(bounds, Selected == i));
            Globals.SpriteDrawer.DrawRect(new Rectangle(bounds.X, bounds.Y + bounds.Height - InfoHeight, bounds.Width, InfoHeight), new Color(new Color(180, 180, 180), 0.75f));
            Globals.SpriteDrawer.DrawText(Selected.Title, new Vector2(bounds.X + bounds.Width / 2, bounds.Bottom - InfoHeight + 30), 24f, Color.Black);
            Globals.SpriteDrawer.DrawText($"{Selected.PointsSpent} / {Selected.MaxPoints}", new Vector2(bounds.X + bounds.Width / 2, bounds.Bottom - InfoHeight + 80f), 20f);
            Globals.SpriteDrawer.DrawText(Selected.Description.Wrap(bounds.Width - Padding * 2, 16f), new Vector2(bounds.X + bounds.Width / 2, bounds.Bottom - InfoHeight + 150f), 16f);
        }

        public void Update()
        {
            Selected = Selected ?? Items.First();

            if (ControllerService.Controllers[_playerIndex].IsPressed(Buttons.A))
            {
                if (PointsAvailable > 0 && Selected.PointsSpent < Selected.MaxPoints && PointsSpent >= Selected.Row * 5 &&
                    (Selected.Parent == null || Selected.Parent.PointsSpent == Selected.MaxPoints))
                {
                    Selected.PointsSpent++;
                    PointsAvailable--;
                }
            }

            var origSelection = Selected;
            if (ControllerService.Controllers[_playerIndex].IsPressed(Buttons.LeftThumbstickRight))
                Selected = Items.Where(i => i.Row == Selected.Row && i.Col > Selected.Col).OrderBy(i => i.Col).FirstOrDefault() ?? Selected;
            else if (ControllerService.Controllers[_playerIndex].IsPressed(Buttons.LeftThumbstickLeft))
                Selected = Items.Where(i => i.Row == Selected.Row && i.Col < Selected.Col).OrderByDescending(i => i.Col).FirstOrDefault() ?? Selected;
            else if (ControllerService.Controllers[_playerIndex].IsPressed(Buttons.LeftThumbstickDown))
            {
                Selected = Items.Where(i => i.Col == Selected.Col && i.Row > Selected.Row).OrderBy(i => i.Row).FirstOrDefault();
                if (Selected == null)
                    Selected = Items.Where(i => i.Col == origSelection.Col - 1 && i.Row > origSelection.Row).OrderBy(i => i.Row).FirstOrDefault();
                if (Selected == null)
                    Selected = Items.Where(i => i.Col == origSelection.Col + 1 && i.Row > origSelection.Row).OrderBy(i => i.Row).FirstOrDefault();
            }
            else if (ControllerService.Controllers[_playerIndex].IsPressed(Buttons.LeftThumbstickUp))
            {
                Selected = Items.Where(i => i.Col == Selected.Col && i.Row < Selected.Row).OrderByDescending(i => i.Row).FirstOrDefault();
                if (Selected == null)
                    Selected = Items.Where(i => i.Col == origSelection.Col - 1 && i.Row < origSelection.Row).OrderByDescending(i => i.Row).FirstOrDefault();
                if (Selected == null)
                    Selected = Items.Where(i => i.Col == origSelection.Col + 1 && i.Row < origSelection.Row).OrderByDescending(i => i.Row).FirstOrDefault();
            }
        }
    }
}
