using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BuildingGen;
using FontStashSharp;
using Visualize.UI;

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
        public TileInfo[] Tileset;
        
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
            KeyboardInput.Initialize(this, 250f, 20);
            SetGraphicsSettings();
            _world = new World(this);
            _world.LoadTiles(_tiles);
            _camera = new Camera(this, _world);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            FontSystem = new FontSystem();
            FontSystem.AddFont(File.ReadAllBytes(@"Fonts/OpenSans-Regular.ttf"));
            LoadTileSet();
            _userInterface = new UserInterface(_spriteBatch, this, _camera, _world);
            base.Initialize();
        }

        private void LoadTileSet()
        {
            var tilesJson = File.ReadAllText("tiles.json");
            var loaded = JsonSerializer.Deserialize<TileInfo[]>(tilesJson);
            Tileset = loaded;
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