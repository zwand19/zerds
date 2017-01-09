using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Enums;
using Zerds.Menus;

namespace Zerds.GameObjects
{
    public class PlayerSkills
    {
        public SkillTree FireSkillTree { get; set; }
        public SkillTree ArcaneSkillTree { get; set; }
        public SkillTree FrostSkillTree { get; set; }
        
        public PlayerSkills(Player player)
        {
            FireSkillTree = new SkillTree("Fire", player);
            // Fire Row 1
            var improvedFireball = new SkillTreeItem(SkillConstants.ImprovedFireballName, $"Increase damage and size of fireball by {StatStr(SkillConstants.ImprovedFireballStat, 5, 0)}%.", 5, 0, 0, "Icons/improved_fireball.png");
            FireSkillTree.Items.Add(improvedFireball);
            var fireMastery = new SkillTreeItem(SkillConstants.FireMasteryName, $"Increase all fire damage done by {StatStr(SkillConstants.FireMasteryStat, 5, 0)}%.", 5, 0, 2, "Icons/fire_mastery.png");
            FireSkillTree.Items.Add(fireMastery);
            var devastationSkill = new SkillTreeItem(SkillConstants.DevastationName, $"Increase critical hit chance of fire spells by {StatStr(SkillConstants.DevastationStat, 5, 1)}%.", 5, 0, 4, "Icons/devastation.png");
            FireSkillTree.Items.Add(devastationSkill);
            // Fire Row 2
            var lavaBlastSkill = new SkillTreeItem("Lava Blast", "Fuel up a large ball of lava that can blast through multiple enemies. Not learned until all 5 skill points are spent.", 5, 1, 2, "Icons/lava_blast.png", null, AbilityTypes.LavaBlast);
            FireSkillTree.Items.Add(lavaBlastSkill);

            FrostSkillTree = new SkillTree("Frost", player);
            // Frost Row 1
            var improvedIceball = new SkillTreeItem(SkillConstants.ImprovedIceballName, $"Increase damage and slow effect of iceball by {StatStr(SkillConstants.ImprovedIceballStat, 5, 0)}%.", 5, 0, 0, "Icons/ice-bolt.png");
            FrostSkillTree.Items.Add(improvedIceball);
            var frozenSoul = new SkillTreeItem(SkillConstants.FrozenSoulName, $"Reduce mana cost of all frost spells by {StatStr(SkillConstants.FrozenSoulStat, 5, 1)}%.", 5, 0, 2, "Icons/cold-heart.png");
            FrostSkillTree.Items.Add(frozenSoul);
            var coldExplosion = new SkillTreeItem(SkillConstants.ColdExplosionName, $"Causes iceballs to explode in a small radius, dealing {StatStr(SkillConstants.ColdExplosionStat, 5, 0)}% damage and slow effect to nearby enemies.", 5, 0, 4, "Icons/cold-explosion.png");
            FrostSkillTree.Items.Add(coldExplosion);

            ArcaneSkillTree = new SkillTree("Arcane", player);
            // Arcane Row 1
            var improvedWand = new SkillTreeItem(SkillConstants.ImprovedWandName, $"Increase damage and range of wand attacks by {StatStr(SkillConstants.ImprovedWandStat, 5, 0)}%.", 5, 0, 0, "Icons/improved_wand.png");
            ArcaneSkillTree.Items.Add(improvedWand);
            var dancer = new SkillTreeItem(SkillConstants.DancerName, $"Reduces cooldown of dash by {StatStr(SkillConstants.DancerStat, 5, 1)} seconds.", 5, 0, 2, "Icons/charging-bull.png");
            ArcaneSkillTree.Items.Add(dancer);
            var swift = new SkillTreeItem(SkillConstants.SwiftName, $"Increases movement speed by {StatStr(SkillConstants.SwiftStat, 5, 1)}%.", 5, 0, 4, "Icons/walking-boot.png");
            ArcaneSkillTree.Items.Add(swift);
            // Arcane Row 2
            var replenish = new SkillTreeItem(SkillConstants.ReplenishName, $"Wand attacks restore {StatStr(SkillConstants.ReplenishStat, 5, 1)}% of your mana.", 5, 1, 0, "Icons/glass-heart.png", improvedWand);
            ArcaneSkillTree.Items.Add(replenish);
            var sprinter = new SkillTreeItem(SkillConstants.SprinterName, $"Reduces mana cost of sprint by {StatStr(SkillConstants.SprintStat, 5, 0)}%.", 5, 1, 4, "Icons/rabbit.png", swift);
            ArcaneSkillTree.Items.Add(sprinter);
            var guzzler = new SkillTreeItem(SkillConstants.GuzzlerName, $"Increase chance of potions dropping from enemies you kill by {StatStr(SkillConstants.GuzzlerStat, 5, 1)}%.", 5, 1, 2, "Icons/potion-ball.png");
            ArcaneSkillTree.Items.Add(guzzler);
        }

        public int ImprovedFireball => FireSkill(SkillConstants.ImprovedFireballName);
        public int FireMastery => FireSkill(SkillConstants.FireMasteryName);
        public int Devastation => FireSkill(SkillConstants.DevastationName);

        public int ImprovedIceball => FrostSkill(SkillConstants.ImprovedIceballName);
        public int FrozenSoul => FrostSkill(SkillConstants.FrozenSoulName);
        public int ColdExplosion => FrostSkill(SkillConstants.ColdExplosionName);

        public int ImprovedWand => ArcaneSkill(SkillConstants.ImprovedWandName);
        public int Dancer => ArcaneSkill(SkillConstants.DancerName);
        public int Swift => ArcaneSkill(SkillConstants.SwiftName);
        public int Replenish => ArcaneSkill(SkillConstants.ReplenishName);
        public int Guzzler => ArcaneSkill(SkillConstants.GuzzlerName);

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
