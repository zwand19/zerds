using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Menus;

namespace Zerds.GameObjects
{
    public class PlayerSkills
    {
        public SkillTree FireSkillTree { get; set; }
        public SkillTree ArcaneSkillTree { get; set; }
        public SkillTree FrostSkillTree { get; set; }


        public PlayerSkills(PlayerIndex playerIndex)
        {
            FireSkillTree = new SkillTree("Fire", playerIndex);
            var improvedFireball = new SkillTreeItem(SkillConstants.ImprovedFireballName, $"Increase damage and size of fireball by {StatStr(SkillConstants.ImprovedFireballStat, 5, 0)}%.", 5, 0, 0, "Skills/improved_fireball.png");
            FireSkillTree.Items.Add(improvedFireball);
            var fireMastery = new SkillTreeItem(SkillConstants.FireMasteryName, $"Increase all fire damage done by {StatStr(SkillConstants.FireMasteryStat, 5, 0)}%.", 5, 0, 2, "Skills/fire_mastery.png");
            FireSkillTree.Items.Add(fireMastery);
            var devastationSkill = new SkillTreeItem(SkillConstants.DevastationName, $"Increase critical hit chance of fire spells by {StatStr(SkillConstants.DevastationStat, 5, 1)}%.", 5, 0, 4, "Skills/devastation.png");
            FireSkillTree.Items.Add(devastationSkill);
            //var lavaBlastSkill = new SkillTreeItem("Lava Blast", "Fuel up a large ", 5, 0, 4, "Skills/devastation.png");
            //FireSkillTree.Items.Add(lavaBlastSkill);

            FrostSkillTree = new SkillTree("Frost", playerIndex);
            var improvedIceball = new SkillTreeItem(SkillConstants.ImprovedIceballName, $"Increase damage and slow effect of iceball by {StatStr(SkillConstants.ImprovedIceballStat, 5, 0)}%.", 5, 0, 0, "Skills/improved_iceball.png");
            FrostSkillTree.Items.Add(improvedIceball);

            ArcaneSkillTree = new SkillTree("Arcane", playerIndex);
            var improvedWand = new SkillTreeItem(SkillConstants.ImprovedWandName, $"Increase damage and range of wand attacks by {StatStr(SkillConstants.ImprovedWandStat, 5, 0)}%.", 5, 0, 0, "Skills/improved_wand.png");
            ArcaneSkillTree.Items.Add(improvedWand);
            var dancer = new SkillTreeItem(SkillConstants.DancerName, $"Reduces cooldown of dash by {StatStr(SkillConstants.DancerStat, 5, 1)} seconds.", 5, 0, 2, "Skills/charging-bull.png");
            ArcaneSkillTree.Items.Add(dancer);
            var swift = new SkillTreeItem(SkillConstants.SwiftName, $"Increases movement speed by {StatStr(SkillConstants.SwiftStat, 5, 1)}%.", 5, 0, 4, "Skills/walking-boot.png");
            ArcaneSkillTree.Items.Add(swift);
            var replenish = new SkillTreeItem(SkillConstants.ReplenishName, $"Wand attacks restore {StatStr(SkillConstants.ReplenishStat, 5, 1)}% of your mana.", 5, 1, 0, "Skills/potion-ball.png", improvedWand);
            ArcaneSkillTree.Items.Add(replenish);
        }

        public int ImprovedFireball => FireSkill(SkillConstants.ImprovedFireballName);
        public int FireMastery => FireSkill(SkillConstants.FireMasteryName);
        public int Devastation => FireSkill(SkillConstants.DevastationName);

        public int ImprovedIceball => FrostSkill(SkillConstants.ImprovedIceballName);

        public int ImprovedWand => ArcaneSkill(SkillConstants.ImprovedWandName);
        public int Dancer => ArcaneSkill(SkillConstants.DancerName);
        public int Swift => ArcaneSkill(SkillConstants.SwiftName);
        public int Replenish => ArcaneSkill(SkillConstants.ReplenishName);

        public int FireSkill(string name) => FireSkillTree.Items.First(i => i.Title == name).PointsSpent;
        public int FrostSkill(string name) => FrostSkillTree.Items.First(i => i.Title == name).PointsSpent;
        public int ArcaneSkill(string name) => ArcaneSkillTree.Items.First(i => i.Title == name).PointsSpent;

        private string StatStr(float amt, int num, int decimalPlaces)
        {
            var str = "";
            for (var i = 1; i <= num; i++)
            {
                var slash = i == num ? "" : "/";
                str += Math.Round(amt * i, decimalPlaces) + slash;
            }
            return str;
        }
    }
}
