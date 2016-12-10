using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.GameObjects;
using Zerds.Missiles;

namespace Zerds
{
    public class GameState
    {
        public int Level { get; set; }
        public List<Zerd> Zerds { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<InanimateObject> Objects { get; set; }
        public List<Missile> Missiles { get; set; }
        public List<DamageText> DamageTexts { get; set; }
        public Map Map { get; set; }
        public List<Player> Players { get; set; }
        public List<Being> Beings => Zerds.Select(z => (Being) z).Concat(Enemies).ToList();
        public List<Entity> Entities =>
            Beings.Select(b => (Entity)b)
                .Concat(Objects.Select(o => (Entity)o))
                .Concat(Missiles.Select(m => (Entity)m)).ToList();

        public GameState(GraphicsDevice graphicsDevice, MapTypes mapType, Rectangle clientBounds, List<Player> players)
        {
            Level = 1;
            Map = new Map(graphicsDevice, mapType, clientBounds);
            Zerds = new List<Zerd>();
            Enemies = new List<Enemy>();
            Objects = new List<InanimateObject>();
            Missiles = new List<Missile>();
            DamageTexts = new List<DamageText>();
            DamageFactory.AddText = text =>
            {
                DamageTexts.Add(text);
                return true;
            };
            Players = players;
        }

        public void Draw()
        {
            Map.Draw();
            Globals.SpriteDrawer.Begin();
            Entities.ForEach(b => b.Draw());
            Enemies.ForEach(b => b.DrawHealthbar());
            DamageTexts.ForEach(d => d.Draw());
            Globals.SpriteDrawer.End();
        }

        public void Update(GameTime gameTime)
        {
            Beings.ForEach(b => b.Update(gameTime));
            Enemies = Enemies.Where(e => e.IsActive).ToList();
            Enemies.ForEach(e => e.RunAI());
            Missiles = Missiles.Where(m => m.IsActive).ToList();
            Missiles.ForEach(m => m.Update(gameTime));
            DamageTexts = DamageTexts.Where(d => d.IsActive).ToList();
            DamageTexts.ForEach(d => d.Update(gameTime));
            EnemyCreatorFactory.Update(gameTime);
        }
    }
}
