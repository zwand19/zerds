using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Zerds.Abilities;
using Zerds.Enums;
using Zerds.GameObjects;
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

        private readonly Player _player;
        private const int NumRows = 6;
        private const int NumCols = 5;
        private const int InfoHeight = 320;
        private const int Padding = 30;

        public SkillTree(string name, Player player)
        {
            Name = name;
            _player = player;
            Items = new List<SkillTreeItem>();
        }

        public void Draw(Rectangle bounds)
        {
            Selected = Selected ?? Items.First();
            Items.ForEach(i => i.DrawDependencies(bounds));
            Items.ForEach(i =>
            {
                var available = PointsSpent >= i.Row * 5 && (i.Parent == null || i.Parent.PointsSpent == i.Parent.MaxPoints);
                i.Draw(bounds, Selected == i, available);
            });
            new Rectangle(bounds.X, bounds.Y + bounds.Height - InfoHeight, bounds.Width, InfoHeight).Draw(new Color(new Color(180, 180, 180), 0.75f));
            Selected.Title.Draw(new Vector2(bounds.X + bounds.Width / 2, bounds.Bottom - InfoHeight + 30), 24f, Color.Black);
            $"{Selected.PointsSpent} / {Selected.MaxPoints}".Draw(new Vector2(bounds.X + bounds.Width / 2, bounds.Bottom - InfoHeight + 80f), 20f);
            Selected.Description.Wrap(bounds.Width - Padding * 2, 16f).Draw(new Vector2(bounds.X + bounds.Width / 2, bounds.Bottom - InfoHeight + 180f), 16f);
            var skillPts = (Name == "Fire" ? _player.Skills.FireSkillTree.PointsAvailable : Name == "Frost" ? _player.Skills.FrostSkillTree.PointsAvailable : _player.Skills.ArcaneSkillTree.PointsAvailable);
            $"Pts Available: {skillPts} + {_player.FloatingSkillPoints} Floating".Draw(new Vector2(bounds.Center.X, bounds.Y + 30f), 15f, Color.White);
        }

        public void Update()
        {
            Selected = Selected ?? Items.First();

            if (InputService.InputDevices[_player.PlayerIndex].IsPressed(Buttons.A))
            {
                if ((PointsAvailable > 0 || _player.FloatingSkillPoints > 0) && Selected.PointsSpent < Selected.MaxPoints && PointsSpent >= Selected.Row * 5 &&
                    (Selected.Parent == null || Selected.Parent.PointsSpent == Selected.Parent.MaxPoints))
                {
                    _player.Zerd.Stats.SkillPointsSpent++;
                    Selected.PointsSpent++;
                    if (PointsAvailable > 0)
                        PointsAvailable--;
                    else
                        _player.FloatingSkillPoints--;
                    if (Selected.PointsSpent == Selected.MaxPoints)
                    {
                        switch (Selected.Ability)
                        {
                            case AbilityTypes.LavaBlast:
                                _player.Zerd.Abilities.Add(new LavaBlast(_player.Zerd));
                                break;
                            case AbilityTypes.FrostPound:
                                _player.Zerd.Abilities.Add(new FrostPound(_player.Zerd));
                                break;
                            case AbilityTypes.DragonsBreath:
                                _player.Zerd.Abilities.Add(new DragonBreath(_player.Zerd));
                                break;
                            case AbilityTypes.Icicle:
                                _player.Zerd.Abilities.Add(new Icicle(_player.Zerd));
                                break;
                            case AbilityTypes.Charm:
                                _player.Zerd.Abilities.Add(new Charm(_player.Zerd));
                                break;
                            case AbilityTypes.None:
                                break;
                            default:
                                throw new Exception("missing ability type");
                        }
                    }
                }
            }

            var origSelection = Selected;
            if (InputService.InputDevices[_player.PlayerIndex].IsPressed(Buttons.LeftThumbstickRight))
                Selected = Items.Where(i => i.Row == Selected.Row && i.Col > Selected.Col).OrderBy(i => i.Col).FirstOrDefault() ?? Selected;
            else if (InputService.InputDevices[_player.PlayerIndex].IsPressed(Buttons.LeftThumbstickLeft))
                Selected = Items.Where(i => i.Row == Selected.Row && i.Col < Selected.Col).OrderByDescending(i => i.Col).FirstOrDefault() ?? Selected;
            else if (InputService.InputDevices[_player.PlayerIndex].IsPressed(Buttons.LeftThumbstickDown))
            {
                Selected = Items.Where(i => i.Col == Selected.Col && i.Row > Selected.Row).OrderBy(i => i.Row).FirstOrDefault();
                if (Selected == null)
                    Selected = Items.Where(i => i.Col == origSelection.Col - 1 && i.Row > origSelection.Row).OrderBy(i => i.Row).FirstOrDefault();
                if (Selected == null)
                    Selected = Items.Where(i => i.Col == origSelection.Col + 1 && i.Row > origSelection.Row).OrderBy(i => i.Row).FirstOrDefault();
            }
            else if (InputService.InputDevices[_player.PlayerIndex].IsPressed(Buttons.LeftThumbstickUp))
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
