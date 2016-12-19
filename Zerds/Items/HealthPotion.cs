using System;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;

namespace Zerds.Items
{
    public class HealthPotion : Potion
    {
        public HealthPotion(Enemy dropper) : base("Items/potions.png", dropper)
        {
            Animations = new AnimationList();
            var anim = new Animation(AnimationTypes.Stand);
            anim.AddFrame(new Rectangle(0, 537, 146, 155), TimeSpan.FromMilliseconds(100));
            Animations.Add(anim);
        }

        public override void OnPickup(Being being)
        {
            if (!(being is Zerd)) return;
            IsActive = false;
            being.Health += being.MaxHealth*GameplayConstants.HealthPotionBonus;
        }
    }
}
