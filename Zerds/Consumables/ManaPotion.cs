using System;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;

namespace Zerds.Consumables
{
    public class ManaPotion : Potion
    {
        public ManaPotion(Enemy dropper) : base("Consumables/potions.png", dropper)
        {
            Animations = new AnimationList();
            var anim = new Animation(AnimationTypes.Stand);
            anim.AddFrame(new Rectangle(0, 692, 146, 155), TimeSpan.FromMilliseconds(100));
            Animations.Add(anim);
        }

        public override void OnPickup(Zerd zerd)
        {
            IsActive = false;
            zerd.Mana += zerd.MaxMana * GameplayConstants.ManaPotionBonus;
        }
    }
}
