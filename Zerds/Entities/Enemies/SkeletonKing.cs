using System;
using Zerds.Graphics;
using Microsoft.Xna.Framework;
using Zerds.Abilities;
using Zerds.Enums;
using Zerds.AI;
using Zerds.Constants;
using Zerds.Buffs;

namespace Zerds.Entities.Enemies
{
    public class SkeletonKing : Enemy
    {
        private readonly SkeletonKingAI _ai;
        
        public SkeletonKing() : base(EnemyTypes.SkeletonKing, EnemyConstants.GetSkeletonKingProperties(), "Entities/Zomb-King.png", true)
        {
            _ai = new SkeletonKingAI(this, new Melee(this, EnemyConstants.SkeletonKingMinDamage, EnemyConstants.SkeletonKingMaxDamage, null, EnemyConstants.SkeletonKingKnockback, EnemyConstants.SkeletonKingKnockbackMillis), new SkeletonKingBlast(this));

            HitboxSize = 0.8f;
            Width = 312;
            Height = 312;
            AttackRange = 130;
            Spawned = true;

            Animations = new AnimationList();
            var walkAnimation = new Animation(AnimationTypes.Move, new BodyPart(BodyPartType.Custom, 470, 370));
            walkAnimation.AddFrame(0, 0, TimeSpan.FromSeconds(0.3));
            walkAnimation.AddFrame(1, 0, TimeSpan.FromSeconds(0.3));
            walkAnimation.AddFrame(2, 0, TimeSpan.FromSeconds(0.3));
            walkAnimation.AddFrame(1, 0, TimeSpan.FromSeconds(0.3));
            walkAnimation.AddFrame(0, 0, TimeSpan.FromSeconds(0.3));
            walkAnimation.AddFrame(3, 0, TimeSpan.FromSeconds(0.3));
            walkAnimation.AddFrame(4, 0, TimeSpan.FromSeconds(0.3));
            walkAnimation.AddFrame(3, 0, TimeSpan.FromSeconds(0.3));
            Animations.Add(walkAnimation);

            var attackAnimation = new Animation(AnimationTypes.Attack, new BodyPart(BodyPartType.Custom, 470, 370));
            attackAnimation.AddFrame(1, 1, TimeSpan.FromSeconds(0.05));
            attackAnimation.AddFrame(2, 1, TimeSpan.FromSeconds(0.05));
            attackAnimation.AddFrame(3, 1, TimeSpan.FromSeconds(0.05));
            attackAnimation.AddFrame(4, 1, TimeSpan.FromSeconds(0.05));
            attackAnimation.AddFrame(5, 1, TimeSpan.FromSeconds(0.05), Rotate);
            attackAnimation.AddFrame(6, 1, TimeSpan.FromSeconds(0.05), Rotate);
            attackAnimation.AddFrame(7, 1, TimeSpan.FromSeconds(0.05), Rotate);
            attackAnimation.AddFrame(0, 2, TimeSpan.FromSeconds(0.05), Rotate);
            attackAnimation.AddFrame(1, 2, TimeSpan.FromSeconds(0.05), AttackedFunc);
            attackAnimation.AddFrame(2, 2, TimeSpan.FromSeconds(0.05), Unrotate);
            attackAnimation.AddFrame(3, 2, TimeSpan.FromSeconds(0.05), Unrotate);
            attackAnimation.AddFrame(4, 2, TimeSpan.FromSeconds(0.05), Unrotate);
            attackAnimation.AddFrame(5, 2, TimeSpan.FromSeconds(0.05), Unrotate);
            attackAnimation.AddFrame(5, 2, TimeSpan.FromSeconds(0.05), DoneAttackingFunc);
            Animations.Add(attackAnimation);

            var castingAnimation = new Animation(AnimationTypes.FireAttack, new BodyPart(BodyPartType.Custom, 470, 370));
            castingAnimation.AddFrame(7, 0, TimeSpan.FromSeconds(0.15), BecomeInvulnerable);
            castingAnimation.AddFrame(0, 1, TimeSpan.FromSeconds(0.65));
            castingAnimation.AddFrame(7, 0, TimeSpan.FromSeconds(0.15));
            castingAnimation.AddFrame(5, 0, TimeSpan.FromSeconds(0.1), CastedFunc);
            castingAnimation.AddFrame(6, 0, TimeSpan.FromSeconds(0.3));
            castingAnimation.AddFrame(5, 0, TimeSpan.FromSeconds(0.15));
            castingAnimation.AddFrame(5, 0, TimeSpan.FromSeconds(0.05), DoneAttackingFunc);
            Animations.Add(castingAnimation);

            var dieAnimation = new Animation(AnimationTypes.Death, new BodyPart(BodyPartType.Custom, 470, 370));
            dieAnimation.AddFrame(6, 2, TimeSpan.FromSeconds(0.1), OnDeath);
            dieAnimation.AddFrame(7, 2, TimeSpan.FromSeconds(0.1), OnDeathFinished);
            Animations.Add(dieAnimation);

            var walkEnragedAnimation = new Animation(AnimationTypes.MoveEnraged, new BodyPart(BodyPartType.Custom, 470, 370));
            walkEnragedAnimation.AddFrame(0, 3, TimeSpan.FromSeconds(0.22));
            walkEnragedAnimation.AddFrame(1, 3, TimeSpan.FromSeconds(0.22));
            walkEnragedAnimation.AddFrame(2, 3, TimeSpan.FromSeconds(0.22));
            walkEnragedAnimation.AddFrame(1, 3, TimeSpan.FromSeconds(0.22));
            walkEnragedAnimation.AddFrame(0, 3, TimeSpan.FromSeconds(0.22));
            walkEnragedAnimation.AddFrame(3, 3, TimeSpan.FromSeconds(0.22));
            walkEnragedAnimation.AddFrame(4, 3, TimeSpan.FromSeconds(0.22));
            walkEnragedAnimation.AddFrame(3, 3, TimeSpan.FromSeconds(0.22));
            Animations.Add(walkEnragedAnimation);

            var attackEnragedAnimation = new Animation(AnimationTypes.AttackEnraged, new BodyPart(BodyPartType.Custom, 470, 370));
            attackEnragedAnimation.AddFrame(1, 4, TimeSpan.FromSeconds(0.04));
            attackEnragedAnimation.AddFrame(2, 4, TimeSpan.FromSeconds(0.04));
            attackEnragedAnimation.AddFrame(3, 4, TimeSpan.FromSeconds(0.04));
            attackEnragedAnimation.AddFrame(4, 4, TimeSpan.FromSeconds(0.04));
            attackEnragedAnimation.AddFrame(5, 4, TimeSpan.FromSeconds(0.04), Rotate);
            attackEnragedAnimation.AddFrame(6, 4, TimeSpan.FromSeconds(0.04), Rotate);
            attackEnragedAnimation.AddFrame(7, 4, TimeSpan.FromSeconds(0.04), Rotate);
            attackEnragedAnimation.AddFrame(0, 5, TimeSpan.FromSeconds(0.04), Rotate);
            attackEnragedAnimation.AddFrame(1, 5, TimeSpan.FromSeconds(0.04), AttackedFunc);
            attackEnragedAnimation.AddFrame(2, 5, TimeSpan.FromSeconds(0.04), Unrotate);
            attackEnragedAnimation.AddFrame(3, 5, TimeSpan.FromSeconds(0.04), Unrotate);
            attackEnragedAnimation.AddFrame(4, 5, TimeSpan.FromSeconds(0.04), Unrotate);
            attackEnragedAnimation.AddFrame(5, 5, TimeSpan.FromSeconds(0.04), Unrotate);
            attackEnragedAnimation.AddFrame(5, 5, TimeSpan.FromSeconds(0.04), DoneAttackingFunc);
            Animations.Add(attackEnragedAnimation);

            var castingEnragedAnimation = new Animation(AnimationTypes.FireAttackEnraged, new BodyPart(BodyPartType.Custom, 470, 370));
            castingEnragedAnimation.AddFrame(7, 3, TimeSpan.FromSeconds(0.12), BecomeInvulnerable);
            castingEnragedAnimation.AddFrame(0, 4, TimeSpan.FromSeconds(0.25));
            castingEnragedAnimation.AddFrame(7, 3, TimeSpan.FromSeconds(0.1));
            castingEnragedAnimation.AddFrame(5, 3, TimeSpan.FromSeconds(0.1), CastedFunc);
            castingEnragedAnimation.AddFrame(6, 3, TimeSpan.FromSeconds(0.2));
            castingEnragedAnimation.AddFrame(5, 3, TimeSpan.FromSeconds(0.12));
            castingEnragedAnimation.AddFrame(5, 3, TimeSpan.FromSeconds(0.05), DoneAttackingFunc);
            Animations.Add(castingEnragedAnimation);

            var dieEnragedAnimation = new Animation(AnimationTypes.DeathEnraged, new BodyPart(BodyPartType.Custom, 470, 370));
            dieEnragedAnimation.AddFrame(6, 5, TimeSpan.FromSeconds(0.1), OnDeath);
            dieEnragedAnimation.AddFrame(7, 5, TimeSpan.FromSeconds(0.1), OnDeathFinished);
            Animations.Add(dieEnragedAnimation);
        }

        private bool Rotate()
        {
            Facing = Facing.Rotate(1.5f);
            return true;
        }

        private bool Unrotate()
        {
            Facing = Facing.Rotate(-1.5f);
            return true;
        }

        public override AI.AI GetAI() => _ai;

        private bool AttackedFunc()
        {
            return _ai.Attacked();
        }

        private bool DoneAttackingFunc()
        {
            Buffs.RemoveAll(b => b is InvulnerableBuff);
            return _ai.FinishedAttacking();
        }

        private bool BecomeInvulnerable()
        {
            Buffs.Add(new InvulnerableBuff(this, TimeSpan.FromSeconds(20)));
            return true;
        }

        private bool CastedFunc()
        {
            foreach (var zerd in Globals.GameState.Zerds)
            {
                var z1 = new Zombie();
                var v = new Vector2(50, 0).Rotate(Globals.Random.Next(360));
                z1.X = zerd.X + v.X;
                z1.Y = zerd.Y + v.Y;
                z1.GetAI().Target = zerd;
                var z2 = new Zombie();
                v = new Vector2(50, 0).Rotate(Globals.Random.Next(360));
                z2.X = zerd.X + v.X;
                z2.Y = zerd.Y + v.Y;
                z2.GetAI().Target = zerd;
                var d1 = new Dog {X = X, Y = Y};
                d1.GetAI().Target = zerd;
                var d2 = new Dog {X = X, Y = Y};
                d2.GetAI().Target = zerd;
                Globals.GameState.Enemies.Add(d1);
                Globals.GameState.Enemies.Add(d2);
                Globals.GameState.Enemies.Add(z1);
                Globals.GameState.Enemies.Add(z2);
            }
            _ai.Casted();
            return true;
        }

        public override void LevelEnded()
        {
            BaseSpeed *= EnemyConstants.SkeletonKingEnrageSpeedFactor;
            _ai.Enrage();
        }

        public override float SpriteRotation()
        {
            return 3f * (float)Math.PI / 2f;
        }
    }
}
