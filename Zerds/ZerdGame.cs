﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Zerds.Enums;
using Zerds.Factories;
using Zerds.GameObjects;
using Zerds.Input;
using Zerds.Menus;

namespace Zerds
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ZerdGame : Game
    {
        private List<Player> _players;
        private GameStates _state;
        private MainMenu _mainMenu;
        private GameSetup _gameSetup;
        private IntermissionScreen _intermissionScreen;

        public ZerdGame()
        {
            _state = GameStates.MainMenu;
            // ReSharper disable once ObjectCreationAsStatement
            new GraphicsDeviceManager(this);
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
            ControllerService.Initialize();

            Globals.Map = new Map(GraphicsDevice, MapTypes.Dungeon, Globals.ViewportBounds);
            _mainMenu = new MainMenu(SetupGameFunc);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Globals.ContentManager = Content;
            Globals.SpriteDrawer = new SpriteBatch(GraphicsDevice);
            Globals.ViewportBounds = GraphicsDevice.Viewport.Bounds;
            _players = new List<Player>
            {
                new Player("Player One", PlayerIndex.One),
                new Player("Player Two", PlayerIndex.Two),
                new Player("Player Three", PlayerIndex.Three),
                new Player("Player Four", PlayerIndex.Four)
            };
            Globals.GameState = new GameState(_players);
            Globals.Initialize();
            Globals.LoadFont("Pericles", FontTypes.Pericles);
            HUD.Initialize(GraphicsDevice);
        }
        
        private bool SetupGameFunc()
        {
            _state = GameStates.GameSetup;
            _mainMenu = null;
            _gameSetup = new GameSetup(PlayGameFunc);
            return true;
        }

        private bool PlayGameFunc(List<bool> players)
        {
            _gameSetup = null;
            _state = GameStates.Game;
            for (var i=0; i < players.Count; i++)
                if (players[i]) _players[i].JoinGame();

            return true;
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
            ControllerService.Update(gameTime);
            switch (_state)
            {
                case GameStates.MainMenu:
                    _mainMenu.Update();
                    break;
                case GameStates.GameSetup:
                    _gameSetup.Update();
                    break;
                case GameStates.Game:
                    _players.ForEach(p => p.Update(gameTime));
                    Globals.GameState.Update(gameTime);
                    if (Globals.GameState.LevelTimeRemaining <= TimeSpan.Zero && !Globals.GameState.Enemies.Any())
                    {
                        _state = GameStates.Intermission;
                        _intermissionScreen = new IntermissionScreen();
                    }
                    break;
                case GameStates.Intermission:
                    _intermissionScreen.Update();
                    if (_intermissionScreen.Ready)
                    {
                        _intermissionScreen = null;
                        _state = GameStates.Game; 
                        Globals.GameState.StartLevel();
                    }
                    return;
            }
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

            switch (_state)
            {
                case GameStates.MainMenu:
                    _mainMenu.Draw();
                    break;
                case GameStates.GameSetup:
                    _gameSetup.Draw();
                    break;
                case GameStates.Game:
                    Globals.GameState.Draw();
                    HUD.Draw();
                    break;
                case GameStates.Intermission:
                    Globals.GameState.Draw();
                    _intermissionScreen.Draw();
                    break;
            }
        }
    }
}
