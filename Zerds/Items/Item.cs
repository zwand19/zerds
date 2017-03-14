using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zerds.Data;
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
            SkillUpgrades = new List<SkillUpgrade>();
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

        public void Draw(float x, float y, float width, float height)
        {
            Globals.SpriteDrawer.Draw(Texture, new Rectangle((int)(x - width / 2), (int)(y - height / 2), (int) width, (int) height), Color.White);
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

        public static Item FromSaveData(SavedItem savedItem)
        {
            Item item;
            switch ((ItemTypes)savedItem.Type)
            {
                case ItemTypes.Hood:
                    item = new HoodItem((ItemRarities)savedItem.Rarity);
                    break;
                case ItemTypes.Robe:
                    item = new RobeItem((ItemRarities)savedItem.Rarity);
                    break;
                case ItemTypes.Wand:
                    item = new WandItem((ItemRarities)savedItem.Rarity);
                    break;
                case ItemTypes.Boots:
                    item = new BootItem((ItemRarities)savedItem.Rarity);
                    break;
                case ItemTypes.Ring:
                    item = new RingItem((ItemRarities)savedItem.Rarity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown saved item type");
            }
            item.AbilityUpgrades = savedItem.AbilityUpgrades.Select(u => new AbilityUpgrade(u)).ToList();
            item.SkillUpgrades = savedItem.SkillUpgrades.Select(u => new SkillUpgrade(u)).ToList();
            return item;
        }
    }
}
