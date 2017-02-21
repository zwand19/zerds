using System;
using Zerds.Graphics;
using Microsoft.Xna.Framework;
using Zerds.Abilities;
using Zerds.Enums;
using Zerds.AI;
using Zerds.Constants;

namespace Zerds.Entities.Enemies
{
    public class SkeletonKing : Enemy
    {
        private MeleeAI _ai;

        public SkeletonKing() : base(EnemyConstants.GetSkeletonKingProperties(), "Entities/Zomb-King.png", true)
        {
            _ai = new MeleeAI(this, new Melee(this, 20, 30, null, 750, 750));

            HitboxSize = 0.85f;
            Width = 128;
            Height = 128;
            AttackRange = 100;
            Spawned = true;

            Animations = new AnimationList();
            var walkAnimation = new Animation(AnimationTypes.Move);
            walkAnimation.AddFrame(new Rectangle(0, 0, 273, 195), TimeSpan.FromSeconds(0.3), origin: new Vector2(139, 86));
            walkAnimation.AddFrame(new Rectangle(273, 1656, 273, 195), TimeSpan.FromSeconds(0.3), origin: new Vector2(411 - 273, 1743 - 1656));
            walkAnimation.AddFrame(new Rectangle(0, 1656, 273, 195), TimeSpan.FromSeconds(0.3), origin: new Vector2(139, 1743 - 1656));
            walkAnimation.AddFrame(new Rectangle(273, 1656, 273, 195), TimeSpan.FromSeconds(0.3), origin: new Vector2(411 - 273, 1743 - 1656));
            walkAnimation.AddFrame(new Rectangle(0, 0, 273, 195), TimeSpan.FromSeconds(0.3), origin: new Vector2(139, 86));
            walkAnimation.AddFrame(new Rectangle(0, 851, 273, 195), TimeSpan.FromSeconds(0.3), origin: new Vector2(139, 86));
            walkAnimation.AddFrame(new Rectangle(0, 1656, 273, 195), TimeSpan.FromSeconds(0.3), origin: new Vector2(139, 86));
            walkAnimation.AddFrame(new Rectangle(0, 851, 273, 195), TimeSpan.FromSeconds(0.3), origin: new Vector2(139, 86));
            Animations.Add(walkAnimation);

            var attackAnimation = new Animation(AnimationTypes.Attack);
            attackAnimation.AddFrame(new Rectangle(270, 200, 306, 193), TimeSpan.FromSeconds(0.05), origin: new Vector2(441 - 270, 286 - 200));
            attackAnimation.AddFrame(new Rectangle(576, 200, 327, 190), TimeSpan.FromSeconds(0.05), origin: new Vector2(768 - 576, 286 - 200));
            attackAnimation.AddFrame(new Rectangle(0, 398, 345, 191), TimeSpan.FromSeconds(0.05), origin: new Vector2(211 - 0, 484 - 398));
            attackAnimation.AddFrame(new Rectangle(345, 398, 345, 197), TimeSpan.FromSeconds(0.05), origin: new Vector2(554 - 345, 484 - 398));
            attackAnimation.AddFrame(new Rectangle(7, 595, 335, 196), TimeSpan.FromSeconds(0.05), origin: new Vector2(208 - 7, 680 - 595));
            attackAnimation.AddFrame(new Rectangle(363, 595, 308, 222), TimeSpan.FromSeconds(0.05), origin: new Vector2(527 - 363, 682 - 595));
            attackAnimation.AddFrame(new Rectangle(677, 595, 273, 256), TimeSpan.FromSeconds(0.05), origin: new Vector2(790 - 677, 680 - 595));
            attackAnimation.AddFrame(new Rectangle(273, 851, 271, 263), TimeSpan.FromSeconds(0.05), origin: new Vector2(376 - 273, 938 - 851));
            attackAnimation.AddFrame(new Rectangle(544, 851, 267, 257), TimeSpan.FromSeconds(0.05), AttackedFunc, new Vector2(639 - 544, 937 - 851));
            attackAnimation.AddFrame(new Rectangle(0, 1119, 265, 248), TimeSpan.FromSeconds(0.05), origin: new Vector2(94 - 0, 1206 - 1119));
            attackAnimation.AddFrame(new Rectangle(265, 1119, 269, 251), TimeSpan.FromSeconds(0.05), origin: new Vector2(369 - 265, 1205 - 1119));
            attackAnimation.AddFrame(new Rectangle(534, 1119, 276, 237), TimeSpan.FromSeconds(0.05), origin: new Vector2(635 - 534, 1205 - 1119));
            attackAnimation.AddFrame(new Rectangle(0, 1385, 270, 213), TimeSpan.FromSeconds(0.05), origin: new Vector2(131 - 0, 1471 - 1385));
            attackAnimation.AddFrame(new Rectangle(0, 0, 273, 195), TimeSpan.FromSeconds(0.05), DoneAttackingFunc, new Vector2(139, 86));
            Animations.Add(attackAnimation);

            var dieAnimation = new Animation(AnimationTypes.Death);
            dieAnimation.AddFrame(new Rectangle(0, 0, 9, 9), TimeSpan.FromSeconds(0.1), OnDeath);
            dieAnimation.AddFrame(new Rectangle(6, 6, 9, 9), TimeSpan.FromSeconds(0.1), OnDeathFinished);
            Animations.Add(dieAnimation);
        }

        public override AI.AI GetAI() => _ai;

        private bool AttackedFunc()
        {
            return _ai.Attacked();
        }

        private bool DoneAttackingFunc()
        {
            return _ai.FinishedAttacking();
        }

        public override float SpriteRotation()
        {
            return 3f * (float)Math.PI / 2f;
        }
    }
}
