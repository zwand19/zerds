using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Zerds.Buffs;
using Zerds.Constants;
using Zerds.Data;
using Zerds.Enums;
using Zerds.Input;
using Zerds.Factories;
using Zerds.Items;

namespace Zerds.GameObjects
{
    public class Player
    {
        public string Name { get; set; }
        public bool IsPlaying { get; set; }
        public PlayerIndex PlayerIndex { get; set; }
        public Entities.Zerd Zerd { get; set; }
        public int FloatingSkillPoints { get; set; }
        public PlayerSkills Skills { get; set; }
        public Dictionary<AbilityUpgradeType, float> AbilityUpgrades { get; set; }
        public List<Item> Items { get; set; }
        public int Gold { get; set; }

        public Player(string name, PlayerIndex playerIndex)
        {
            Name = name;
            PlayerIndex = playerIndex;
            Skills = new PlayerSkills(this);
            AbilityUpgrades = new Dictionary<AbilityUpgradeType, float>
            {
                {AbilityUpgradeType.DamageTaken, 0},
                {AbilityUpgradeType.DashDistance, 0},
                {AbilityUpgradeType.FireballDamage, 0},
                {AbilityUpgradeType.FireballMana, 0},
                {AbilityUpgradeType.HealthRegen, 0},
                {AbilityUpgradeType.IceballCrit, 0},
                {AbilityUpgradeType.IceballMana, 0},
                {AbilityUpgradeType.LavaBlastDistance, 0},
                {AbilityUpgradeType.ManaRegen, 0},
                {AbilityUpgradeType.MovementSpeed, 0},
                {AbilityUpgradeType.SprintSpeed, 0},
                {AbilityUpgradeType.MaxHealth, 0},
                {AbilityUpgradeType.MaxMana, 0}
            };
            Gold = GameplayConstants.StartingGold;
            Items = new List<Item>();
        }

        public void Update(GameTime gameTime)
        {
            if (Zerd == null || !Zerd.IsAlive) return;
            var controller = ControllerService.Controllers[PlayerIndex];
            Zerd.ControllerUpdate(controller.LeftTrigger, controller.RightTrigger, controller.LeftStickDirection, controller.RightStickDirection);
            var buttonsPressed = ControllerService.Controllers[PlayerIndex].ButtonsPressed;
            if (Zerd.GetCurrentAnimationType() == AnimationTypes.Stand ||
                Zerd.GetCurrentAnimationType() == AnimationTypes.Move)
            {
                if (buttonsPressed.Contains(Buttons.RightShoulder))
                    Zerd.Abilities.First(a => a.Type == AbilityTypes.Dash).Cast();
                if (buttonsPressed.Contains(Buttons.A))
                    Zerd.Abilities.First(a => a.Type == AbilityTypes.Wand).Cast();
                if (buttonsPressed.Contains(Buttons.B))
                    Zerd.Abilities.First(a => a.Type == AbilityTypes.Iceball).Cast();
                if (buttonsPressed.Contains(Buttons.Y))
                    Zerd.Abilities.First(a => a.Type == AbilityTypes.Fireball).Cast();
                if (buttonsPressed.Contains(Buttons.X))
                    Zerd.Abilities.FirstOrDefault(a => a.Type == AbilityTypes.LavaBlast)?.Cast();
                if (buttonsPressed.Contains(Buttons.RightStick))
                    Zerd.Abilities.FirstOrDefault(a => a.Type == AbilityTypes.FrostPound)?.Cast();
                if (buttonsPressed.Contains(Buttons.LeftStick))
                    Zerd.Abilities.FirstOrDefault(a => a.Type == AbilityTypes.DragonsBreath)?.Cast();
                if (buttonsPressed.Contains(Buttons.DPadDown))
                    Zerd.Abilities.FirstOrDefault(a => a.Type == AbilityTypes.Icicle)?.Cast();
                if (buttonsPressed.Contains(Buttons.DPadUp))
                    Zerd.Abilities.FirstOrDefault(a => a.Type == AbilityTypes.Charm)?.Cast();
            }
            if (ControllerService.Controllers[PlayerIndex].RightTrigger > CodingConstants.TriggerPress && Zerd.Mana > 1)
            {
                Zerd.Mana -= AbilityConstants.SprintManaPerSecond * (float) gameTime.ElapsedGameTime.TotalSeconds * Globals.GameState.GameSpeed *
                             (1 - Zerd.SkillValue(SkillType.Sprinter, false) / 100);
                if (!Zerd.Buffs.Any(b => b is SprintBuff))
                    Zerd.Buffs.Add(new SprintBuff(Zerd));
            }
            else
            {
                if (Zerd.Buffs.Any(b => b is SprintBuff))
                    Zerd.Buffs.Remove(Zerd.Buffs.First(b => b is SprintBuff));
            }
        }

        public void JoinGame()
        {
            if (Zerd != null) return;

            IsPlaying = true;
            var chestTexture = TextureCacheFactory.GetOnce("Zerds/red-robe.png");
            var feetTexture = TextureCacheFactory.GetOnce("Zerds/bare-feet.png");
            var handsTexture = TextureCacheFactory.GetOnce("Zerds/bare-hands.png");
            var headTexture = TextureCacheFactory.GetOnce("Zerds/bare-head.png");
            Zerd = new Entities.Zerd(this, chestTexture, handsTexture, headTexture, feetTexture);
            Globals.GameState.Zerds.Add(Zerd);
        }

        public void SaveSyncronous()
        {
            Task.Run(async () =>
                { await XmlStorage.SavePlayer(this); });
        }
    }
}
