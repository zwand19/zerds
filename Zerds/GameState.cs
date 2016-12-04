using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Zerds.Entities;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.GameObjects;

namespace Zerds
{
    public class GameState
    {
        public int Level { get; set; }
        public List<Zerd> Zerds { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<InanimateObject> Objects { get; set; }
        public List<Missile> Missiles { get; set; }
        public List<Being> Beings => Zerds.Select(z => (Being)z).Concat(Enemies).ToList();
        public List<Entity> Entities =>
            Beings.Select(b => (Entity)b)
                .Concat(Objects.Select(o => (Entity)o))
                .Concat(Missiles.Select(m => (Entity)m)).ToList();
        public Map Map { get; set; }
        public List<Player> Players { get; set; }

        public GameState(GraphicsDevice graphicsDevice, MapTypes mapType, Rectangle clientBounds, List<Player> players)
        {
            Level = 1;
            Map = new Map(graphicsDevice, mapType, clientBounds);
            Zerds = new List<Zerd>();
            Enemies = new List<Enemy>();
            Objects = new List<InanimateObject>();
            Missiles = new List<Missile>();
            Players = players;
            Players.ForEach(p => p.GameCreated(this));
        }

        public void Draw()
        {
            Map.Draw();
            Globals.SpriteDrawer.Begin();
            Entities.ForEach(b => b.Draw());
            Globals.SpriteDrawer.End();
        }

        public void Update(GameTime gameTime)
        {
            Beings.ForEach(b => b.Update(gameTime));
            Enemies.ForEach(e => e.RunAI());
            Enemies = Enemies.Where(e => e.IsActive).ToList();
            Missiles.ForEach(m => m.Update(gameTime));
            Missiles = Missiles.Where(m => m.IsActive).ToList();
            EnemyCreatorFactory.Update(gameTime);
        }
    }
}
