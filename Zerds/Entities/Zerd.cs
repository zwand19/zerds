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
using Zerds.Consumables;
using Zerds.Factories;

namespace Zerds.Entities
{
    public class Zerd : Being
    {
        public string Name { get; set; }
        public List<Ability> Abilities { get; set; }
        public List<Item> Inventory { get; set; }
        public List<Key> Keys { get; set; }
        public Stats Stats { get; set; }
        public List<TreasureChest> TreasureChests { get; set; }
        public Player Player { get; set; }
        public ZerdAnimations ZerdAnimations { get; set; }
        public Texture2D ChestTexture { get; set; }
        public Texture2D HandTexture { get; set; }
        public Texture2D HeadTexture { get; set; }
        public Texture2D FeetTexture { get; set; }
        public int Deaths { get; set; }
        public int TeammateDeaths { get; set; }

        private static readonly Vector2 BodyScaleVector = new Vector2(0.55f);

        public Zerd(Player player, List<Item> gear) : base(null, false)
        {
            Player = player;
            ChestTexture = TextureCacheFactory.GetOnce(gear.First(g => g.Type == ItemTypes.Robe).AnimationFile);
            FeetTexture = TextureCacheFactory.GetOnce(gear.First(g => g.Type == ItemTypes.Boots).AnimationFile);
            HandTexture = TextureCacheFactory.GetOnce(gear.First(g => g.Type == ItemTypes.Glove).AnimationFile);
            HeadTexture = TextureCacheFactory.GetOnce(gear.First(g => g.Type == ItemTypes.Hood).AnimationFile);
            gear.ForEach(i =>
            {
                i.AbilityUpgrades.ForEach(a => player.AbilityUpgrades[a.Type] += a.Amount);
                i.SkillUpgrades.ForEach(s =>
                {
                    var skill = player.Skills.AllSkillTrees.SelectMany(t => t.Items).FirstOrDefault(item => item.Type == s.Type);
                    if (skill != null)
                    {
                        skill.PointsSpent = Math.Min(skill.PointsSpent + s.UpgradeAmount, skill.MaxPoints);
                    }
                });
            });
            Inventory = gear;

            X = Globals.ViewportBounds.Width / 2;
            Y = Globals.ViewportBounds.Height / 2;
            X += (int)player.PlayerIndex % 2 == 0 ? 85 : -85;
            Y += (int)player.PlayerIndex < 2 ? -60 : 60;
            Health = GameplayConstants.ZerdStartingHealth;
            Health *= 1 + Inventory.SelectMany(i => i.AbilityUpgrades).Where(i => i.Type == AbilityUpgradeType.MaxHealth).Sum(i => i.Amount)/ 100f;
            MaxHealth = Health;
            Mana = GameplayConstants.ZerdStartingMana;
            Mana *= 1 + Inventory.SelectMany(i => i.AbilityUpgrades).Where(i => i.Type == AbilityUpgradeType.MaxMana).Sum(i => i.Amount) / 100f;
            MaxMana = Mana;
            HealthRegen = GameplayConstants.ZerdStartingHealthRegen;
            ManaRegen = GameplayConstants.ZerdStartingManaRegen;
            Width = 64;
            Height = 64;
            HitboxSize = 0.7f;
            BaseSpeed = BootItem.Speed;
            CriticalChance = GameplayConstants.ZerdCritChance;
            TreasureChests = new List<TreasureChest>();
            Keys = new List<Key>();
            ZerdAnimations = ZerdAnimationHelpers.GetAnimations();
            Stats = new Stats();

            Abilities = new List<Ability>
            {
                new Dash(this),
                new Fireball(this),
                new Wand(this),
                new Iceball(this)
            };
        }

        public void AddCastingAnimation(string name, TimeSpan castTime, TimeSpan followThroughTime, Func<bool> executeFunc, Func<bool> castedFunc)
        {
            ZerdAnimations.AddAnimation(ZerdAnimationHelpers.GetCastingAnimation(name, castTime, followThroughTime, executeFunc, castedFunc));
        }

        public string GetCurrentAnimationType()
        {
            if (Knockback != null || !IsAlive)
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
            if (animation.Keys.Contains(BodyPartType.Feet))
                Globals.SpriteDrawer.Draw(FeetTexture, sourceRectangle: animation[BodyPartType.Feet].CurrentRectangle,
                    color: Color.White, position: new Vector2(X, Y), rotation: angle, origin:
                    new Vector2(ZerdAnimationHelpers.Feet.Width / 2, ZerdAnimationHelpers.Feet.Height / 2), scale: BodyScaleVector);
            if (animation.Keys.Contains(BodyPartType.Hands))
                Globals.SpriteDrawer.Draw(HandTexture, sourceRectangle: animation[BodyPartType.Hands].CurrentRectangle,
                    color: Color.White, position: new Vector2(X, Y), rotation: angle, origin:
                    new Vector2(ZerdAnimationHelpers.Hands.Width / 2, ZerdAnimationHelpers.Hands.Height / 2), scale: BodyScaleVector);
            if (animation.Keys.Contains(BodyPartType.Chest))
                Globals.SpriteDrawer.Draw(ChestTexture, sourceRectangle: animation[BodyPartType.Chest].CurrentRectangle,
                    color: Color.White, position: new Vector2(X, Y), rotation: angle, origin:
                    new Vector2(ZerdAnimationHelpers.Chest.Width / 2, ZerdAnimationHelpers.Chest.Height / 2), scale: BodyScaleVector);
            if (animation.Keys.Contains(BodyPartType.Head))
                Globals.SpriteDrawer.Draw(HeadTexture, sourceRectangle: animation[BodyPartType.Head].CurrentRectangle,
                    color: Color.White, position: new Vector2(X, Y), rotation: angle, origin:
                    new Vector2(ZerdAnimationHelpers.Head.Width / 2, ZerdAnimationHelpers.Head.Height / 2), scale: BodyScaleVector);
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
            return Globals.Random.NextDouble() < chance;
        }

        public void ControllerUpdate(float leftTrigger, float rightTrigger, Vector2 leftStickDirection, Vector2 rightStickDirection)
        {
            if (Stunned || !IsAlive)
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
            if (!IsAlive)
            {
                Opacity -= (float)(gameTime.ElapsedGameTime.TotalSeconds / DisplayConstants.ZerdDeathTime.TotalSeconds);
                if (Opacity < 0)
                    Opacity = 0;
                return;
            }
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
        
        public override float SpriteRotation()
        {
            return 1f * (float)Math.PI / 2f;
        }

        public BootItem BootItem => Inventory.First(i => i is BootItem) as BootItem;
        public RobeItem RobeItem => Inventory.First(i => i is RobeItem) as RobeItem;
        public WandItem WandItem => Inventory.First(i => i is WandItem) as WandItem;
        public HoodItem HoodItem => Inventory.First(i => i is HoodItem) as HoodItem;
        public GloveItem GloveItem => Inventory.First(i => i is GloveItem) as GloveItem;
    }
}
