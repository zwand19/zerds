using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Zerds.Entities;
using Zerds.Factories;
using Zerds.GameObjects;
using Zerds.Consumables;
using Zerds.Missiles;

namespace Zerds
{
    public class GameState
    {
        public float GameSpeed { get; set; }
        public List<Zerd> Zerds { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<Enemy> Allies { get; set; }
        public List<InanimateObject> Objects { get; set; }
        public List<Missile> Missiles { get; set; }
        public List<DamageText> DamageTexts { get; set; }
        public List<Player> Players { get; set; }
        public List<PickupItem> Items { get; set; }
        public List<Being> Beings => Zerds.Select(z => (Being) z).Concat(Enemies).Concat(Allies).ToList();
        public List<Being> Friendlies => Zerds.Select(z => (Being) z).Concat(Allies).ToList();
        public List<Entity> Entities =>
            Beings.Select(b => (Entity)b)
                .Concat(Objects.Select(o => (Entity)o))
                .Concat(Items.Select(i => (Entity)i))
                .Concat(Missiles.Select(m => (Entity)m)).ToList();

        public GameState(List<Player> players)
        {
            Zerds = new List<Zerd>();
            Enemies = new List<Enemy>();
            Allies = new List<Enemy>();
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
            Level.Initialize(players);
        }

        public void Draw()
        {
            Globals.SpriteDrawer.Begin();
            Globals.Map.Draw();
            Entities.ForEach(b => b.Draw());
            Enemies.ForEach(b => b.DrawHealthbar());
            Allies.ForEach(b => b.DrawHealthbar());
            DamageTexts.ForEach(d => d.Draw());
            Globals.SpriteDrawer.End();
        }

        public void Update(GameTime gameTime)
        {
            Level.Update(gameTime);
            Globals.Camera.Update(gameTime);
            Beings.ForEach(b => b.Update(gameTime));
            Enemies = Enemies.Where(e => e.IsActive).ToList();
            Enemies.ForEach(e => e.GetAI().Run(gameTime));
            Allies = Allies.Where(e => e.IsActive).ToList();
            Allies.ForEach(e => e.GetAI().Run(gameTime));
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
