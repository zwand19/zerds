using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Zerds.Constants;
using Zerds.Entities;
using Zerds.Factories;
using Zerds.GameObjects;
using Zerds.Items;
using Zerds.Missiles;

namespace Zerds
{
    public class GameState
    {
        public int Level { get; set; }
        public float GameSpeed { get; set; }
        public TimeSpan LevelTimeRemaining { get; set; }
        public TimeSpan TimeIntoLevel => GameplayConstants.LevelLength - LevelTimeRemaining;
        public List<Zerd> Zerds { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<InanimateObject> Objects { get; set; }
        public List<Missile> Missiles { get; set; }
        public List<DamageText> DamageTexts { get; set; }
        public List<Player> Players { get; set; }
        public List<PickupItem> Items { get; set; }
        public List<Being> Beings => Zerds.Select(z => (Being) z).Concat(Enemies).ToList();
        public List<Entity> Entities =>
            Beings.Select(b => (Entity)b)
                .Concat(Objects.Select(o => (Entity)o))
                .Concat(Items.Select(i => (Entity)i))
                .Concat(Missiles.Select(m => (Entity)m)).ToList();

        public GameState(List<Player> players)
        {
            Zerds = new List<Zerd>();
            Enemies = new List<Enemy>();
            Objects = new List<InanimateObject>();
            Missiles = new List<Missile>();
            DamageTexts = new List<DamageText>();
            Items = new List<PickupItem>();
            GameSpeed = 0.9f;
            DamageFactory.AddText = text =>
            {
                DamageTexts.Add(text);
                return true;
            };
            Players = players;
            StartLevel();
        }

        public void StartLevel()
        {
            Level++;
            LevelTimeRemaining = GameplayConstants.LevelLength;
            Players.ForEach(p =>
            {
                p.FloatingSkillPoints += GameplayConstants.FloatingPointsPerLevel;
                p.Skills.ArcaneSkillTree.PointsAvailable += GameplayConstants.SkillPointsPerLevel;
                p.Skills.FireSkillTree.PointsAvailable += GameplayConstants.SkillPointsPerLevel;
                p.Skills.FrostSkillTree.PointsAvailable += GameplayConstants.SkillPointsPerLevel;
            });
        }

        public void Draw()
        {
            Globals.Map.Draw();
            Globals.SpriteDrawer.Begin();
            Entities.ForEach(b => b.Draw());
            Enemies.ForEach(b => b.DrawHealthbar());
            DamageTexts.ForEach(d => d.Draw());
            Globals.SpriteDrawer.End();
        }

        public void Update(GameTime gameTime)
        {
            LevelTimeRemaining -= gameTime.ElapsedGameTime;
            if (LevelTimeRemaining < TimeSpan.Zero)
                LevelTimeRemaining = TimeSpan.Zero;
            Beings.ForEach(b => b.Update(gameTime));
            Enemies = Enemies.Where(e => e.IsActive).ToList();
            Enemies.ForEach(e => e.GetAI().Run());
            Missiles = Missiles.Where(m => m.IsActive).ToList();
            Missiles.ForEach(m => m.Update(gameTime));
            Items = Items.Where(i => i.IsActive).ToList();
            Items.ForEach(i => i.Update(gameTime));
            DamageTexts = DamageTexts.Where(d => d.IsActive).ToList();
            DamageTexts.ForEach(d => d.Update(gameTime));
            EnemyCreatorFactory.Update(gameTime);
        }
    }
}
