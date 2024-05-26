using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using BuildingGen;
using FontStashSharp;
using Visualize.UI;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace Visualize
{
    public class Core : Game
    {
        private readonly Dictionary<BuildingGen.Vector3, Tile> _tiles;
        public static FontSystem FontSystem;
        
        private readonly GraphicsDeviceManager _graphics;
        public Matrix ProjectionMatrix;
        public Matrix ViewMatrix;
        public Matrix WorldMatrix;

        private World _world;
        private Camera _camera;
        private UserInterface _userInterface;
        
        private SpriteBatch _spriteBatch;

        public Core(Dictionary<BuildingGen.Vector3, Tile> tiles)
        {
            _tiles = tiles;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            KeyboardInput.Initialize(this, 500f, 20);
            SetGraphicsSettings();
            _world = new World(this);
            _world.LoadTiles(_tiles);
            _camera = new Camera(this, _world);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            FontSystem = new FontSystem();
            FontSystem.AddFont(File.ReadAllBytes(@"Fonts/OpenSans-Regular.ttf"));
            _userInterface = new UserInterface(_spriteBatch, this, _camera);
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardInput.Update();
            _userInterface.Update();
            _camera.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _world.Draw();
            _userInterface.Draw();

            base.Draw(gameTime);
        }

        private void SetGraphicsSettings()
        {
            var windowMultiplier = 2;
            var screenWidth = 540;
            var screenHeight = 380;
            Window.Position = new Point(
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - (screenWidth),
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (screenHeight)
            );
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = screenWidth * windowMultiplier;
            _graphics.PreferredBackBufferHeight = screenHeight * windowMultiplier;
            _graphics.ApplyChanges();
        }
    }
}