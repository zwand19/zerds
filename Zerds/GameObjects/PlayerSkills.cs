using System;
using System.Linq;
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
            var improvedFireball = new SkillTreeItem(SkillType.ImprovedFireball, "Improved Fireball", $"Increase damage and size of fireball by {StatStr(SkillType.ImprovedFireball)}%.", 5, 0, 0, "Icons/improved_fireball.png");
            FireSkillTree.Items.Add(improvedFireball);
            var fireMastery = new SkillTreeItem(SkillType.FireMastery, "Fire Mastery", $"Increase all fire damage done by {StatStr(SkillType.FireMastery)}%.", 5, 0, 2, "Icons/fire_mastery.png");
            FireSkillTree.Items.Add(fireMastery);
            var devastationSkill = new SkillTreeItem(SkillType.Devastation, "Devastation", $"Increase critical hit chance of fire spells by {StatStr(SkillType.Devastation)}%.", 5, 0, 4, "Icons/devastation.png");
            FireSkillTree.Items.Add(devastationSkill);
            // Fire Row 2
            var lavaBlastSkill = new SkillTreeItem(SkillType.LavaBlast, "Lava Blast", "Fuel up a large ball of lava that can blast through multiple enemies. Not learned until all 5 skill points are spent.", 5, 1, 2, "Icons/lava_blast.png", null, AbilityTypes.LavaBlast);
            FireSkillTree.Items.Add(lavaBlastSkill);

            FrostSkillTree = new SkillTree("Frost", player);
            // Frost Row 1
            var improvedIceball = new SkillTreeItem(SkillType.ImprovedIceball, "Improved Iceball", $"Increase damage and slow effect of iceball by {StatStr(SkillType.ImprovedIceball)}%.", 5, 0, 0, "Icons/ice-bolt.png");
            FrostSkillTree.Items.Add(improvedIceball);
            var frozenSoul = new SkillTreeItem(SkillType.FrozenSoul, "Frozen Soul", $"Reduce mana cost of all frost spells by {StatStr(SkillType.FrozenSoul)}%.", 5, 0, 2, "Icons/cold-heart.png");
            FrostSkillTree.Items.Add(frozenSoul);
            var coldExplosion = new SkillTreeItem(SkillType.ColdExplosion, "Cold Explosion", $"Cause iceballs to explode in a small radius, dealing {StatStr(SkillType.ColdExplosion)}% damage and slow effect to nearby enemies.", 5, 0, 4, "Icons/cold-explosion.png");
            FrostSkillTree.Items.Add(coldExplosion);
            // Frost Row 2
            var bitterCold = new SkillTreeItem(SkillType.BitterCold, "Bitter Cold", $"Increase length of all slow effects by {StatStr(SkillType.BitterCold)}%.", 5, 1, 0, "Icons/thermometer-cold.png");
            FrostSkillTree.Items.Add(bitterCold);
            var frostPound = new SkillTreeItem(SkillType.FrostPound, "Frost Pound", "Slam the ground with a frozen fist, dealing damage and freezing all nearby enemies in place. Not learned until all 5 skill points are spent.", 5, 1, 2, "Icons/ice-punch.png", null, AbilityTypes.FrostPound);
            FrostSkillTree.Items.Add(frostPound);

            ArcaneSkillTree = new SkillTree("Arcane", player);
            // Arcane Row 1
            var improvedWand = new SkillTreeItem(SkillType.ImprovedWand, "Improved Wand", $"Increase damage and range of wand attacks by {StatStr(SkillType.ImprovedWand)}%.", 5, 0, 0, "Icons/improved_wand.png");
            ArcaneSkillTree.Items.Add(improvedWand);
            var dancer = new SkillTreeItem(SkillType.Dancer, "Dancer", $"Reduces cooldown of dash by {StatStr(SkillType.Dancer)} seconds.", 5, 0, 2, "Icons/charging-bull.png");
            ArcaneSkillTree.Items.Add(dancer);
            var swift = new SkillTreeItem(SkillType.Swiftness, "Swiftness", $"Increases movement speed by {StatStr(SkillType.Swiftness)}%.", 5, 0, 4, "Icons/walking-boot.png");
            ArcaneSkillTree.Items.Add(swift);
            // Arcane Row 2
            var replenish = new SkillTreeItem(SkillType.Replenish, "Replenish", $"Wand attacks restore {StatStr(SkillType.Replenish)}% of your mana.", 5, 1, 0, "Icons/glass-heart.png", improvedWand);
            ArcaneSkillTree.Items.Add(replenish);
            var sprinter = new SkillTreeItem(SkillType.Sprinter, "Sprinter", $"Reduces mana cost of sprint by {StatStr(SkillType.Sprinter)}%.", 5, 1, 4, "Icons/rabbit.png", swift);
            ArcaneSkillTree.Items.Add(sprinter);
            var guzzler = new SkillTreeItem(SkillType.Guzzler, "Guzzler", $"Increase chance of potions dropping from enemies you kill by {StatStr(SkillType.Guzzler)}%.", 5, 1, 2, "Icons/potion-ball.png");
            ArcaneSkillTree.Items.Add(guzzler);
            // Arcane Row 3
            var rewind = new SkillTreeItem(SkillType.Rewind, "Rewind", $"Cast dash again within {StatStr(SkillType.Replenish)}s to teleport back to your original position", 5, 2, 0, "Icons/back-forth.png");
            ArcaneSkillTree.Items.Add(rewind);
        }

        public int Pts(SkillType type)
        {
            return FireSkillTree.Items.FirstOrDefault(i => i.Type == type)?.PointsSpent ??
                   0 + FrostSkillTree.Items.FirstOrDefault(i => i.Type == type)?.PointsSpent ??
                   0 + ArcaneSkillTree.Items.FirstOrDefault(i => i.Type == type)?.PointsSpent ?? 0;
        }

        private string StatStr(SkillType type)
        {
            var skillInfo = SkillConstants.Values[type];
            var str = "";
            for (var i = 1; i <= skillInfo.MaxPoints; i++)
            {
                var slash = i == skillInfo.MaxPoints ? "" : "/";
                str += Math.Round(skillInfo.Stat * i, skillInfo.DecimalPlaces) + slash;
            }
            return str;
        }
    }
}
