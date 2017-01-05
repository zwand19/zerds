using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zerds.Enums;
using Zerds.Factories;

namespace Zerds.Menus
{
    public class SkillTreeItem
    {
        public int MaxPoints { get; set; }
        public int PointsSpent { get; set; }
        public SkillTreeItem Parent { get; set; }
        public List<SkillTreeItem> Children { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Texture2D Texture { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public AbilityTypes Ability { get; set; }

        private const int Padding = 22;
        private const int NumRows = 6;
        private const int NumCols = 5;
        private const int Size = 48;
        private const int Border = 6;
        private const int LineWidth = 5;
        private const int InfoHeight = 320;

        public SkillTreeItem(string title, string description, int points, int row, int col, string file, SkillTreeItem parent = null, AbilityTypes ability = AbilityTypes.None)
        {
            Row = row;
            Col = col;
            Title = title;
            MaxPoints = points;
            Description = description;
            Texture = TextureCacheFactory.GetOnce(file);
            Children = new List<SkillTreeItem>();
            Parent = parent;
            parent?.Children.Add(this);
            Ability = ability;
        }

        public void DrawDependencies(Rectangle bounds)
        {
            var pos = GetPosition(bounds) + new Point(Size / 2, Size / 2);
            Children.ForEach(c =>
            {
                var cPos = GetPosition(bounds, c.Row, c.Col) + new Point(Size / 2, Size / 2);
                Globals.SpriteDrawer.DrawLine(new Vector2(pos.X + LineWidth / 2, pos.Y), new Vector2(cPos.X + LineWidth / 2, cPos.Y), LineWidth, new Color(80, 80, 80));
            });
        }

        public void Draw(Rectangle bounds, bool selected)
        {
            var pos = GetPosition(bounds);
            if (selected)
                Globals.SpriteDrawer.DrawRect(new Rectangle(pos.X - Border, pos.Y - Border, Size + Border * 2, Size + Border * 2), Color.White);
            Globals.SpriteDrawer.Draw(Texture, new Rectangle(pos.X, pos.Y, Size, Size), Color.White);
        }

        private Point GetPosition(Rectangle bounds, int? row = null, int? col = null)
        {
            var rowVal = row ?? Row;
            var colVal = col ?? Col;
            var x = bounds.X + Padding + (bounds.Width - Padding * 2) * colVal / NumCols;
            var y = bounds.Y + Padding + (bounds.Height - InfoHeight - Padding * 2) * rowVal / NumRows;
            return new Point(x, y);
        }
    }
}
