using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;
using Zerds.Items;

namespace Zerds.Consumables
{
    public class TreasureChest : PickupItem
    {
        public List<PotentialItem> PotentialItems { get; set; }

        public TreasureChest(Enemy dropper) : base("Items/treasure-chest.png", dropper)
        {
            const float scale = 0.45f;
            Width = 119 * scale;
            Height = 108 * scale;

            PotentialItems = new List<PotentialItem>();
            AddItem(dropper, 1);
            AddItem(dropper, GameplayConstants.SecondItemChanceFactor);
            AddItem(dropper, GameplayConstants.ThirdItemChanceFactor);
            PotentialItems = PotentialItems.OrderByDescending(t => t.Item.Rarity).ToList();

            Animations = new AnimationList();
            var anim = new Animation(AnimationTypes.Stand);
            var x = (int) PotentialItems.First().Item.Rarity % 2 == 0 ? 119 : 0;
            var y = 108 * (int) Math.Floor(((int) PotentialItems.First().Item.Rarity - 1) / 2f);
            anim.AddFrame(new Rectangle(x, y, 119, 108), TimeSpan.FromMilliseconds(100));
            Animations.Add(anim);
        }

        /// <summary>
        /// Get the chances of each item, WRT them being processed one at a time.
        /// </summary>
        /// <returns></returns>
        public List<double> Chances()
        {
            var chances = new List<double> {PotentialItems[0].Chance};
            var chance = 1 - PotentialItems[0].Chance;
            chances.Add(PotentialItems[1].Chance * chance);
            chance *= 1 - PotentialItems[1].Chance;
            chances.Add(PotentialItems[2].Chance * chance);
            chance *= 1 - PotentialItems[2].Chance;
            chances.Add(chance);
            return chances;
        }

        public override void OnPickup(Zerd zerd)
        {
            IsActive = false;
            zerd.TreasureChests.Add(this);
        }

        private void AddItem(Enemy dropper, double baseChanceFactor)
        {
            ItemRarities rarity;
            var chance = new Random().NextDouble();
            if (dropper == null)
                rarity = ItemRarities.Novice; // base treasure chest always available to open
            else if (chance < GameplayConstants.ItemRarityChances[ItemRarities.Novice])
                rarity = ItemRarities.Novice;
            else if (chance < GameplayConstants.ItemRarityChances[ItemRarities.Apprentice])
                rarity = ItemRarities.Apprentice;
            else if (chance < GameplayConstants.ItemRarityChances[ItemRarities.Adept])
                rarity = ItemRarities.Adept;
            else if (chance < GameplayConstants.ItemRarityChances[ItemRarities.Master])
                rarity = ItemRarities.Master;
            else rarity = ItemRarities.Legendary;

            chance = new Random().NextDouble();
            if (dropper == null)
                PotentialItems.Add(null);
            else if (chance < 1 / 5f)
                PotentialItems.Add(new PotentialItem(new BootItem(rarity), baseChanceFactor));
            else if (chance < 2 / 5f)
                PotentialItems.Add(new PotentialItem(new HoodItem(rarity), baseChanceFactor));
            else if (chance < 3 / 5f)
                PotentialItems.Add(new PotentialItem(new RobeItem(rarity), baseChanceFactor));
            else if (chance < 4 / 5f)
                PotentialItems.Add(new PotentialItem(new WandItem(rarity), baseChanceFactor));
            else
                PotentialItems.Add(new PotentialItem(new RingItem(rarity), baseChanceFactor));
        }
    }
}
