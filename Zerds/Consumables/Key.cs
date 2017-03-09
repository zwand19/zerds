using System;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Graphics;

namespace Zerds.Consumables
{
    public class Key : PickupItem
    {
        public Key(Enemy dropper) : base("Consumables/key.png", dropper)
        {
            const float scale = 0.2f;
            Width = 134 * scale;
            Height = 218 * scale;
            HitboxSize = 2;
            Animations = new AnimationList();
            var anim = new Animation(AnimationTypes.Stand, 134, 218);
            anim.AddFrame(0, 0, TimeSpan.FromMilliseconds(300));
            anim.AddFrame(1, 0, TimeSpan.FromMilliseconds(200));
            anim.AddFrame(2, 0, TimeSpan.FromMilliseconds(200));
            anim.AddFrame(1, 0, TimeSpan.FromMilliseconds(300));
            Animations.Add(anim);
        }

        public override void OnPickup(Zerd zerd)
        {
            IsActive = false;
            zerd.Keys.Add(this);
        }
    }
}
