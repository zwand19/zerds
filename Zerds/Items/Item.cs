using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Zerds.Factories;
using Zerds.GameObjects;

namespace Zerds.Items
{
    public abstract class Item
    {
        public List<AbilityUpgrade> AbilityUpgrades { get; set; }
        public List<SkillUpgrade> SkillUpgrades { get; set; }
        public Texture2D Texture { get; set; }

        protected Item(ItemRarities rarity, string folder)
        {
            AbilityUpgrades = new List<AbilityUpgrade>();
            for (var i = 0; i < (int) rarity; i++)
                AbilityUpgrades.Add(AbilityUpgradeHelper.GetRandomUpgrade());
            string iconName;
            switch (rarity)
            {
                case ItemRarities.Novice:
                    iconName = "novice.png";
                    break;
                case ItemRarities.Apprentice:
                    iconName = "apprentice.png";
                    break;
                case ItemRarities.Adept:
                    iconName = "adept.png";
                    break;
                case ItemRarities.Master:
                    iconName = "master.png";
                    break;
                case ItemRarities.Legend:
                    iconName = "legend.png";
                    break;
                default:
                    throw new Exception("Unknown icon rarity");
            }
            Texture = TextureCacheFactory.GetOnce($"Items/{folder}/{iconName}");
        }
    }
}
