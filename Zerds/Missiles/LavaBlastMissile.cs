using System;
using Microsoft.Xna.Framework;
using Zerds.GameObjects;
using Zerds.Entities;
using Zerds.Graphics;
using Zerds.Enums;
using Zerds.Constants;
using System.Collections.Generic;
using System.Linq;
using Zerds.Factories;
using Zerds.Buffs;

namespace Zerds.Missiles
{
    public class LavaBlastMissile : Missile
    {
        public List<Being> TargetsHit { get; set; }

        public LavaBlastMissile(Zerd zerd, DamageInstance damageInstance, Point p) : base("Missiles/lava_blast.png")
        {
            Damage = damageInstance;
            Width = 86;
            Height = 86;
            X = p.X;
            Y = p.Y;
            Creator = zerd;
            Origin = p;
            Distance = AbilityConstants.FireballDistance * (1 + zerd.Player.AbilityUpgrades[AbilityUpgradeType.LavaBlastDistance] / 100f);
            Speed = AbilityConstants.FireballSpeed;
            Velocity = Creator.Facing.Normalized();
            TargetsHit = new List<Being>();

            Animations = new AnimationList();
            var moveAnimation = new Animation(AnimationTypes.Move);
            moveAnimation.AddFrame(new Rectangle(120 * 0, 120 * 0, 120, 120), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(120 * 1, 120 * 0, 120, 120), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(120 * 2, 120 * 0, 120, 120), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(120 * 3, 120 * 0, 120, 120), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(120 * 0, 120 * 1, 120, 120), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(120 * 1, 120 * 1, 120, 120), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(120 * 2, 120 * 1, 120, 120), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(120 * 3, 120 * 1, 120, 120), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(120 * 0, 120 * 2, 120, 120), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(120 * 1, 120 * 2, 120, 120), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(120 * 2, 120 * 2, 120, 120), TimeSpan.FromSeconds(0.1));
            moveAnimation.AddFrame(new Rectangle(120 * 3, 120 * 2, 120, 120), TimeSpan.FromSeconds(0.1));
            Animations.Add(moveAnimation);

            var deathAnimation = new Animation(AnimationTypes.Death);
            deathAnimation.AddFrame(new Rectangle(120 * 0, 120 * 0, 120, 120), TimeSpan.FromSeconds(0.06));
            deathAnimation.AddFrame(new Rectangle(120 * 1, 120 * 0, 120, 120), TimeSpan.FromSeconds(0.06), () => { Opacity = 0.91f; return true; });
            deathAnimation.AddFrame(new Rectangle(120 * 2, 120 * 0, 120, 120), TimeSpan.FromSeconds(0.06), () => { Opacity = 0.82f; return true; });
            deathAnimation.AddFrame(new Rectangle(120 * 3, 120 * 0, 120, 120), TimeSpan.FromSeconds(0.06), () => { Opacity = 0.73f; return true; });
            deathAnimation.AddFrame(new Rectangle(120 * 0, 120 * 1, 120, 120), TimeSpan.FromSeconds(0.06), () => { Opacity = 0.64f; return true; });
            deathAnimation.AddFrame(new Rectangle(120 * 1, 120 * 1, 120, 120), TimeSpan.FromSeconds(0.06), () => { Opacity = 0.55f; return true; });
            deathAnimation.AddFrame(new Rectangle(120 * 2, 120 * 1, 120, 120), TimeSpan.FromSeconds(0.06), () => { Opacity = 0.46f; return true; });
            deathAnimation.AddFrame(new Rectangle(120 * 3, 120 * 1, 120, 120), TimeSpan.FromSeconds(0.06), () => { Opacity = 0.37f; return true; });
            deathAnimation.AddFrame(new Rectangle(120 * 0, 120 * 2, 120, 120), TimeSpan.FromSeconds(0.06), () => { Opacity = 0.28f; return true; });
            deathAnimation.AddFrame(new Rectangle(120 * 1, 120 * 2, 120, 120), TimeSpan.FromSeconds(0.06), () => { Opacity = 0.19f; return true; });
            deathAnimation.AddFrame(new Rectangle(120 * 2, 120 * 2, 120, 120), TimeSpan.FromSeconds(0.06), () => { Opacity = 0.1f; return true; });
            deathAnimation.AddFrame(new Rectangle(120 * 3, 120 * 2, 120, 120), TimeSpan.FromSeconds(0.06), DeathFunc);
            Animations.Add(deathAnimation);
        }

        public override Animation GetCurrentAnimation()
        {
            return Animations.Get(IsAlive ? AnimationTypes.Move : AnimationTypes.Death);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsAlive)
            {
                foreach (var enemy in Creator.Enemies().Where(e => e.IsAlive && !TargetsHit.Contains(e)))
                {
                    if (enemy.Hitbox().Any(hitbox => Hitbox().Any(hitbox.Intersects)))
                        OnHit(enemy);
                }
            }
            if (Origin.DistanceBetween(Position) > Distance && IsAlive)
            {
                Speed *= 0.75f;
                IsAlive = false;
                if (!TargetsHit.Any())
                    ((Zerd) Creator).Combo = 0;
                else ((Zerd)Creator).IncreaseCombo();
            }
            base.Update(gameTime);
        }

        public override float SpriteRotation()
        {
            return 3.0f * (float)Math.PI / 2;
        }

        public override List<Rectangle> Hitbox()
        {
            return new List<Rectangle> {
                new Rectangle((int)(X - Width * 0.38f), (int)(Y - Width * 0.38f), (int)(Width * 0.76f), (int)(Width * 0.76f))
            };
        }

        public override void OnHit(Being target)
        {
            if (TargetsHit.Contains(target))
                return;
            TargetsHit.Add(target);
            var oldDamage = Damage.Damage;
            Damage.Damage += Origin.DistanceBetween(Position) * ((Zerd)Creator).SkillValue(SkillType.Sniper, true) / 100f;
            Damage.DamageBeing(target);
            Speed *= 0.9f;
            target.AddBuff(new BurnBuff(Creator, target, TimeSpan.FromMilliseconds(AbilityConstants.LavaBlastBurnLength), Damage.Damage * AbilityConstants.LavaBlastBurnDamagePercentage));
            Damage.Damage = oldDamage;
        }
    }
}
