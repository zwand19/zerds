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

        public static int BlazingSpeedSeconds = 4;
        public static int BleedFireHealthPercent = 8;
        public static int FrostAuraRange = 200;
        public static float IcicleKillPercent = 0.2f;

        public PlayerSkills(Player player)
        {
            FireSkillTree = new SkillTree("Fire", player);
            // Fire Row 1
            var improvedFireball = new SkillTreeItem(SkillType.ImprovedFireball, "Improved Fireball", $"Increase damage and size of fireball by {StatStr(SkillType.ImprovedFireball)}%.", 5, 0, 0, "improved_fireball.png");
            FireSkillTree.Items.Add(improvedFireball);
            var fireMastery = new SkillTreeItem(SkillType.FireMastery, "Fire Mastery", $"Increase all fire damage done by {StatStr(SkillType.FireMastery)}%.", 5, 0, 2, "fire_mastery.png");
            FireSkillTree.Items.Add(fireMastery);
            var devastation = new SkillTreeItem(SkillType.Devastation, "Devastation", $"Increase critical hit chance of fire spells by {StatStr(SkillType.Devastation)}%.", 5, 0, 4, "devastation.png");
            FireSkillTree.Items.Add(devastation);
            // Fire Row 2
            var deepBurn = new SkillTreeItem(SkillType.DeepBurn, "Deep Burn", $"Increases the damage and duration of burn effects by {StatStr(SkillType.DeepBurn)}.", 5, 1, 0, "heartburn.png");
            FireSkillTree.Items.Add(deepBurn);
            var lavaBlast = new SkillTreeItem(SkillType.LavaBlast, "Lava Blast", "Fuel up a large ball of lava that can blast through multiple enemies. Not learned until all 5 skill points are spent.", 5, 1, 2, "lava_blast.png", null, AbilityTypes.LavaBlast);
            FireSkillTree.Items.Add(lavaBlast);
            var fireballExplosion = new SkillTreeItem(SkillType.FireballExplosion, "Fireball Explosion", $"Cause fireballs to explode in a small radius, dealing {StatStr(SkillType.FireballExplosion)}% damage and burn effect to nearby enemies.", 5, 1, 4, "mine-explosion.png");
            FireSkillTree.Items.Add(fireballExplosion);
            // Fire Row 3
            var exposure = new SkillTreeItem(SkillType.Exposure, "Exposure", $"Enemies afflicted by a burn effect take {StatStr(SkillType.Exposure)}% extra damage from missiles.", 5, 2, 0, "sun.png", deepBurn);
            FireSkillTree.Items.Add(exposure);
            var sniper = new SkillTreeItem(SkillType.Sniper, "Sniper", $"Your fire missiles gain {StatStr(SkillType.Sniper)}% of their distance travelled in damage.", 5, 2, 2, "targeting.png");
            FireSkillTree.Items.Add(sniper);
            var blazingSpeed = new SkillTreeItem(SkillType.BlazingSpeed, "Blazing Speed", $"Each fire spell cast grants {StatStr(SkillType.BlazingSpeed)}% increased movement speed for {BlazingSpeedSeconds} seconds (stacks).", 5, 2, 4, "twister.png");
            FireSkillTree.Items.Add(blazingSpeed);
            // Fire Row 4
            var dragonsBreath = new SkillTreeItem(SkillType.DragonsBreath, "Dragon Breath", "Instantly project devastating flames from your mouth for the next 2 seconds. Not learned until all 5 skill points are spent.", 5, 3, 2, "dragon-breath.png", null, AbilityTypes.DragonsBreath);
            FireSkillTree.Items.Add(dragonsBreath);
            var bleedFire = new SkillTreeItem(SkillType.BleedFire, "Bleed Fire", $"Blazing speed buffs burn you for {BleedFireHealthPercent}% of your max health, but you gain {StatStr(SkillType.BleedFire)}% lifesteal from all fire spells.", 5, 3, 4, "life-support.png", blazingSpeed);
            FireSkillTree.Items.Add(bleedFire);
            //Fire Row 5
            var maniac = new SkillTreeItem(SkillType.Maniac, "Maniac", $"Gain {StatStr(SkillType.Maniac)}% of your missing health % in fire damage (e.g. having 50% health and full skill pts = 20% increase).", 5, 4, 4, "life-support.png", bleedFire);
            FireSkillTree.Items.Add(maniac);

            FrostSkillTree = new SkillTree("Frost", player);
            // Frost Row 1
            var improvedIceball = new SkillTreeItem(SkillType.ImprovedIceball, "Improved Iceball", $"Increase damage and slow effect of iceball by {StatStr(SkillType.ImprovedIceball)}%.", 5, 0, 0, "ice-bolt.png");
            FrostSkillTree.Items.Add(improvedIceball);
            var frozenSoul = new SkillTreeItem(SkillType.FrozenSoul, "Frozen Soul", $"Reduce mana cost of all frost spells by {StatStr(SkillType.FrozenSoul)}%.", 5, 0, 2, "cold-heart.png");
            FrostSkillTree.Items.Add(frozenSoul);
            var coldExplosion = new SkillTreeItem(SkillType.ColdExplosion, "Cold Explosion", $"Cause iceballs to explode in a small radius, dealing {StatStr(SkillType.ColdExplosion)}% damage and slow effect to nearby enemies.", 5, 0, 4, "cold-explosion.png");
            FrostSkillTree.Items.Add(coldExplosion);
            // Frost Row 2
            var bitterCold = new SkillTreeItem(SkillType.BitterCold, "Bitter Cold", $"Increase length of all slow effects by {StatStr(SkillType.BitterCold)}%.", 5, 1, 0, "thermometer-cold.png");
            FrostSkillTree.Items.Add(bitterCold);
            var frostPound = new SkillTreeItem(SkillType.FrostPound, "Frost Pound", "Slam the ground with a frozen fist, dealing damage and freezing all nearby enemies in place. Not learned until all 5 skill points are spent.", 5, 1, 2, "ice-punch.png", null, AbilityTypes.FrostPound);
            FrostSkillTree.Items.Add(frostPound);
            var coldWinds = new SkillTreeItem(SkillType.ColdWinds, "Cold Winds", $"Dashing through enemies freezes them in place for {StatStr(SkillType.ColdWinds)} seconds.", 5, 1, 4, "wind-slap.png");
            FrostSkillTree.Items.Add(coldWinds);
            // Frost Row 3
            var deepCold = new SkillTreeItem(SkillType.DeepCold, "Deep Cold", $"Deal an additional {StatStr(SkillType.DeepCold)}% frost damage to slowed targets.", 5, 2, 0, "brain-freeze.png", bitterCold);
            FrostSkillTree.Items.Add(deepCold);
            var shatter = new SkillTreeItem(SkillType.Shatter, "Shatter", $"Deal an additional {StatStr(SkillType.Shatter)}% damage to frozen targets.", 5, 2, 2, "shatter.png", frostPound);
            FrostSkillTree.Items.Add(shatter);
            var iceShield = new SkillTreeItem(SkillType.IceShield, "Ice Shield", $"Critical hits with frost spells grant you an invulnerability buff for {StatStr(SkillType.IceShield)}% seconds.", 5, 2, 4, "ice-shield.png");
            FrostSkillTree.Items.Add(iceShield);
            // Frost Row 4
            var frostAura = new SkillTreeItem(SkillType.FrostAura, "Frost Aura", $"Enemies within {FrostAuraRange} range take {StatStr(SkillType.FrostAura)}% more frost damage", 5, 3, 0, "ice-aura.png");
            FrostSkillTree.Items.Add(frostAura);
            var icicle = new SkillTreeItem(SkillType.Icicle, "Icicle", "Whip icicles in 8 directions around you which freeze, deal damage, and instantly killing enemies with <20% health.", 5, 3, 2, "icicle.png", null, AbilityTypes.Icicle);
            FrostSkillTree.Items.Add(icicle);

            ArcaneSkillTree = new SkillTree("Arcane", player);
            // Arcane Row 1
            var guzzler = new SkillTreeItem(SkillType.Guzzler, "Guzzler", $"Increase chance of potions dropping from enemies you kill by {StatStr(SkillType.Guzzler)}%.", 5, 0, 0, "potion-ball.png");
            ArcaneSkillTree.Items.Add(guzzler);
            var comboMaster = new SkillTreeItem(SkillType.ComboMaster, "Combo Master", $"Increase gold gained from your combo by {StatStr(SkillType.ComboMaster)}%.", 5, 0, 2, "ore.png");
            ArcaneSkillTree.Items.Add(comboMaster);
            var swift = new SkillTreeItem(SkillType.Swiftness, "Swiftness", $"Increases movement speed by {StatStr(SkillType.Swiftness)}%.", 5, 0, 4, "walking-boot.png");
            ArcaneSkillTree.Items.Add(swift);
            // Arcane Row 2
            var improvedWand = new SkillTreeItem(SkillType.ImprovedWand, "Improved Wand", $"Increase damage and range of wand attacks by {StatStr(SkillType.ImprovedWand)}%.", 5, 1, 0, "improved_wand.png");
            ArcaneSkillTree.Items.Add(improvedWand);
            var rage = new SkillTreeItem(SkillType.Rage, "Rage", $"After killing an enemy, your next wand attack, iceball, or fireball has a {StatStr(SkillType.Rage)}% increased crit chance.", 5, 1, 2, "enrage.png");
            ArcaneSkillTree.Items.Add(rage);
            var sprinter = new SkillTreeItem(SkillType.Sprinter, "Sprinter", $"Reduces mana cost of sprint by {StatStr(SkillType.Sprinter)}%.", 5, 1, 4, "rabbit.png", swift);
            ArcaneSkillTree.Items.Add(sprinter);
            // Arcane Row 3
            var replenish = new SkillTreeItem(SkillType.Replenish, "Replenish", $"Wand attacks restore {StatStr(SkillType.Replenish)}% of your mana.", 5, 2, 0, "glass-heart.png", improvedWand);
            ArcaneSkillTree.Items.Add(replenish);
            var rewind = new SkillTreeItem(SkillType.Rewind, "Rewind", $"Cast dash again within {StatStr(SkillType.Rewind)}s to teleport back to your original position", 5, 2, 2, "back-forth.png");
            ArcaneSkillTree.Items.Add(rewind);
            var dancer = new SkillTreeItem(SkillType.Dancer, "Dancer", $"Reduces cooldown of dash by {StatStr(SkillType.Dancer)} seconds.", 5, 2, 4, "charging-bull.png");
            ArcaneSkillTree.Items.Add(dancer);
            // Arcane Row 4
            var hardened = new SkillTreeItem(SkillType.Hardened, "Hardened", $"Take {StatStr(SkillType.Hardened)}% less damage and knockback from ranged attacks.", 5, 3, 0, "spiked-armor.png");
            ArcaneSkillTree.Items.Add(hardened);
            var charm = new SkillTreeItem(SkillType.Charm, "Charm", "Fire a powerful orb that charms the first enemy it hits into fighting for you. 30s cooldown. Charmed enemies degenerate health slowly.", 5, 3, 2, "chained-heart.png", null, AbilityTypes.Charm);
            ArcaneSkillTree.Items.Add(charm);
        }

        public int Pts(SkillType type)
        {
            return FireSkillTree.Items.FirstOrDefault(i => i.Type == type)?.PointsSpent ??
                   0 + FrostSkillTree.Items.FirstOrDefault(i => i.Type == type)?.PointsSpent ??
                   0 + ArcaneSkillTree.Items.FirstOrDefault(i => i.Type == type)?.PointsSpent ?? 0;
        }

        private static string StatStr(SkillType type)
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
