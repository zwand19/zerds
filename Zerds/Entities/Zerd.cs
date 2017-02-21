using Zerds.Graphics;
using System;
using Microsoft.Xna.Framework;
using Zerds.Constants;
using Zerds.Enums;
using System.Collections.Generic;
using Zerds.Abilities;
using System.Linq;
using Zerds.Buffs;
using Zerds.GameObjects;
using Zerds.Items;
using Microsoft.Xna.Framework.Graphics;

namespace Zerds.Entities
{
    public class Zerd : Being
    {
        public string Name { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<Item> Inventory { get; set; }
        public int Combo { get; set; }
        public int MaxCombo { get; set; }
        public int MaxLevelCombo { get; set; }
        public int EnemiesKilled { get; set; }
        public int LevelEnemiesKilled { get; set; }
        public Player Player { get; set; }
        public ZerdAnimations ZerdAnimations { get; set; }
        public Texture2D ChestTexture { get; set; }
        public Texture2D HandTexture { get; set; }
        public Texture2D HeadTexture { get; set; }
        public Texture2D FeetTexture { get; set; }
        
        private static Vector2 _scaleVector = new Vector2(0.55f);

        public Zerd(Player player, Texture2D chestTexture, Texture2D handTexture, Texture2D headTexture, Texture2D feetTexture) : base(null, false)
        {
            Player = player;
            ChestTexture = chestTexture;
            HandTexture = handTexture;
            HeadTexture = headTexture;
            FeetTexture = feetTexture;

            X = Globals.ViewportBounds.Width / 2;
            Y = Globals.ViewportBounds.Height / 2;
            X += (int)player.PlayerIndex % 2 == 0 ? 85 : -85;
            Y += (int)player.PlayerIndex < 2 ? -60 : 60;
            Health = GameplayConstants.ZerdStartingHealth;
            MaxHealth = Health;
            Mana = GameplayConstants.ZerdStartingMana;
            MaxMana = Mana;
            HealthRegen = GameplayConstants.ZerdStartingHealthRegen;
            ManaRegen = GameplayConstants.ZerdStartingManaRegen;
            Width = 64;
            Height = 64;
            HitboxSize = 0.7f;
            BaseSpeed = GameplayConstants.MaxZerdSpeed;
            CriticalChance = GameplayConstants.ZerdCritChance;

            ZerdAnimations = ZerdAnimationHelpers.GetAnimations();

            Abilities = new List<Ability>
            {
                new Dash(this),
                new Fireball(this),
                new Wand(this),
                new Iceball(this)
            };
            Inventory = new List<Item>();
        }

        public void AddCastingAnimation(string name, TimeSpan castTime, TimeSpan followThroughTime, Func<bool> executeFunc, Func<bool> castedFunc)
        {
            ZerdAnimations.AddAnimation(ZerdAnimationHelpers.GetCastingAnimation(name, castTime, followThroughTime, executeFunc, castedFunc));
        }

        public string GetCurrentAnimationType()
        {
            if (Knockback != null)
                return AnimationTypes.Damaged;
            if (Abilities.First(a => a.Type == AbilityTypes.Wand).Active)
                return AnimationTypes.Attack;
            if (Abilities.First(a => a.Type == AbilityTypes.Iceball).Active)
                return AnimationTypes.FrostAttack;
            if (Abilities.First(a => a.Type == AbilityTypes.Fireball).Active)
                return AnimationTypes.FireAttack;
            if (Abilities.FirstOrDefault(a => a.Type == AbilityTypes.LavaBlast)?.Active == true)
                return AnimationTypes.LavaBlastAttack;
            if (Abilities.FirstOrDefault(a => a.Type == AbilityTypes.FrostPound)?.Active == true)
                return AnimationTypes.FrostPoundAttack;
            if (Abilities.FirstOrDefault(a => a.Type == AbilityTypes.DragonsBreath)?.Active == true)
                return AnimationTypes.FireBreath;
            if (Abilities.FirstOrDefault(a => a.Type == AbilityTypes.Charm)?.Active == true)
                return AnimationTypes.Charm;
            if (Velocity.Length() > Vector2.Zero.Length())
                return AnimationTypes.Move;
            return AnimationTypes.Stand;
        }

        public override Animation GetCurrentAnimation()
        {
            return new Animation(GetCurrentAnimationType());
        }

        public override void Draw()
        {
            if (IsAlive) Buffs.ForEach(b => b.Draw());
            var angle = -(float)Math.Atan2(Facing.Y, Facing.X) + SpriteRotation();
            var animation = ZerdAnimations.Animations[GetCurrentAnimationType()];
            if (animation.Keys.Contains(ZerdBodyPartTypes.Feet))
                Globals.SpriteDrawer.Draw(texture: FeetTexture, sourceRectangle: animation[ZerdBodyPartTypes.Feet].CurrentRectangle,
                    color: Color.White, position: new Vector2(X, Y), rotation: angle, origin:
                    new Vector2(ZerdAnimationHelpers.Feet.Width / 2, ZerdAnimationHelpers.Feet.Height / 2), scale: _scaleVector);
            if (animation.Keys.Contains(ZerdBodyPartTypes.Hands))
                Globals.SpriteDrawer.Draw(texture: HandTexture, sourceRectangle: animation[ZerdBodyPartTypes.Hands].CurrentRectangle,
                    color: Color.White, position: new Vector2(X, Y), rotation: angle, origin:
                    new Vector2(ZerdAnimationHelpers.Hands.Width / 2, ZerdAnimationHelpers.Hands.Height / 2), scale: _scaleVector);
            if (animation.Keys.Contains(ZerdBodyPartTypes.Chest))
                Globals.SpriteDrawer.Draw(texture: ChestTexture, sourceRectangle: animation[ZerdBodyPartTypes.Chest].CurrentRectangle,
                    color: Color.White, position: new Vector2(X, Y), rotation: angle, origin:
                    new Vector2(ZerdAnimationHelpers.Chest.Width / 2, ZerdAnimationHelpers.Chest.Height / 2), scale: _scaleVector);
            if (animation.Keys.Contains(ZerdBodyPartTypes.Head))
                Globals.SpriteDrawer.Draw(texture: HeadTexture, sourceRectangle: animation[ZerdBodyPartTypes.Head].CurrentRectangle,
                    color: Color.White, position: new Vector2(X, Y), rotation: angle, origin:
                    new Vector2(ZerdAnimationHelpers.Head.Width / 2, ZerdAnimationHelpers.Head.Height / 2), scale: _scaleVector);
            if (Globals.ShowHitboxes && IsAlive)
            {
                Hitbox().ForEach(r => Globals.SpriteDrawer.Draw(Globals.WhiteTexture, r, Color.Green));
            }
        }

        public override bool IsCritical(DamageTypes type, AbilityTypes ability)
        {
            var chance = CriticalChance;
            if (type == DamageTypes.Fire)
                chance += this.SkillValue(SkillType.Devastation, false) / 100;
            if (ability == AbilityTypes.Iceball)
                chance += this.AbilityValue(AbilityUpgradeType.IceballCrit) / 100;
            if ((ability == AbilityTypes.Iceball || ability == AbilityTypes.Fireball || ability == AbilityTypes.Wand) && Buffs.Any(b => b is RageBuff))
            {
                var buff = Buffs.First(b => b is RageBuff);
                Buffs.Remove(buff);
                chance += buff.CritChanceFactor;
            }
            return new Random().NextDouble() < chance;
        }

        public void ControllerUpdate(float leftTrigger, float rightTrigger, Vector2 leftStickDirection, Vector2 rightStickDirection)
        {
            if (Stunned)
                return;

            if (Math.Abs(leftStickDirection.Length()) > CodingConstants.JoystickTolerance)
            {
                Facing = leftStickDirection;
            }
            if (leftTrigger > CodingConstants.TriggerPress && Velocity.Length() > 0)
            {
                Facing = Facing.Rotate(180);
            }
            Velocity = leftStickDirection;
        }

        public override void Update(GameTime gameTime)
        {
            if (Knockback != null)
            {
                Abilities.ForEach(a => a.Cancel());
            }
            Abilities.ForEach(a => a.Update(gameTime));
            var animation = ZerdAnimations.Animations[GetCurrentAnimationType()];
            foreach (var anim in animation.Values)
                anim.Update(gameTime);
            base.Update(gameTime);
        }

        public void IncreaseCombo()
        {
            Combo++;
            MaxCombo = Combo > MaxCombo ? Combo : MaxCombo;
            MaxLevelCombo = Combo > MaxLevelCombo ? Combo : MaxLevelCombo;
        }

        public void EnemyKilled(Enemy enemy)
        {
            EnemiesKilled++;
            LevelEnemiesKilled++;
        }
        
        public override float SpriteRotation()
        {
            return 1f * (float)Math.PI / 2f;
        }
    }
}
