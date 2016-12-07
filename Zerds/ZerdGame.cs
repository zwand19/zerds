using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.ViewManagement;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.GameObjects;

namespace Zerds
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ZerdGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private List<Player> _players;

        public ZerdGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            TextureCacheFactory.Initialize(GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Globals.SpriteDrawer = new SpriteBatch(GraphicsDevice);
            Globals.ViewportBounds = GraphicsDevice.Viewport.Bounds;
            _players = new List<Player>
            {
                new Player("Player One", true, PlayerIndex.One),
                new Player("Player Two", false, PlayerIndex.Two),
                new Player("Player Three", false, PlayerIndex.Three),
                new Player("Player Four", false, PlayerIndex.Four)
            };
            Globals.GameState = new GameState(GraphicsDevice, MapTypes.Dungeon, Window.ClientBounds, _players);
            Globals.Initialize();
            HUD.Initialize(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _players.Where(p => p.IsPlaying).ToList().ForEach(p => p.Update(gameTime));
            Globals.GameState.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Globals.ViewportBounds = GraphicsDevice.Viewport.Bounds;
            GraphicsDevice.Clear(Color.Black);

            Globals.GameState.Draw();
            HUD.Draw();
        }
    }
}
