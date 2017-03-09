using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
        public ItemRarities Rarity { get; set; }
        public ItemTypes Type { get; set; }

        protected Item(ItemRarities rarity, ItemTypes type, string folder)
        {
            Rarity = rarity;
            Type = type;
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
                case ItemRarities.Legendary:
                    iconName = "legend.png";
                    break;
                default:
                    throw new Exception("Unknown icon rarity");
            }
            Texture = TextureCacheFactory.GetOnce($"Items/{folder}/{iconName}");
        }

        public void Draw(Point p, float width, float height)
        {
            Globals.SpriteDrawer.Draw(Texture, new Rectangle((int)(p.X - width / 2), (int)(p.Y - height / 2), (int) width, (int) height), Color.White);
        }

        public override string ToString()
        {
            return $"{Rarity} {Type}";
        }

        public Color TextColor
            =>
                Rarity == ItemRarities.Novice
                    ? new Color(180, 180, 180)
                    : Rarity == ItemRarities.Apprentice
                        ? new Color(180, 255, 180)
                        : Rarity == ItemRarities.Adept ? new Color(180, 180, 255) : Rarity == ItemRarities.Master ? new Color(200, 180, 245) : new Color(255, 205, 180);

    }
}
