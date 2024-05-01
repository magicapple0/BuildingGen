using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using BuildingGen;

namespace Visualize
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;

        public Matrix ProjectionMatrix;
        public Matrix ViewMatrix;
        public Matrix WorldMatrix;
        private Vector3 _centerOfBuilding;
        private Vector3 _cameraPosition;
        private Vector3 _cameraTarget;
        private readonly Tile[,,] _tiles;
        private readonly string[] _houseTexture = { "roof.png", "wall.png", "bottom.png", "wall.png", "wall.png", "wall.png"};
        private readonly string[] _cornerTexture = { "roof.png", "corner.png", "bottom.png", "corner.png", "corner.png", "corner.png"};
        private readonly string[] _roofTexture = { "roof.png", "roof_side.png", "bottom.png", "roof_side.png", "roof_side.png", "roof_side.png"};
        
        private Chamber _chamber;
        private readonly List<Cube> _cubes = new ();

        public Game1(Tile[,,] tiles)
        {
            this._tiles = tiles;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //screen settings
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

            //cubes init
            Vector3 min = new Vector3(0, 0, 0);
            Vector3 max = new Vector3(_tiles.GetLength(0), _tiles.GetLength(2), _tiles.GetLength(1));

            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _tiles.GetLength(1); j++)
                {
                    for (int k = 0; k < _tiles.GetLength(2); k++)
                    {
                        switch(_tiles[i, j, k].TileInfo.Name)
                        {
                            case "house":
                                _cubes.Add(new Cube(this, new Vector3(i, k, j), _houseTexture));
                                break;
                            case "roof":
                                _cubes.Add(new Cube(this, new Vector3(i, k, j), _roofTexture));
                                break;
                            case "corner":
                                _cubes.Add(new Cube(this, new Vector3(i, k, j), _cornerTexture));
                                break;
                        }
                    }
                }
            }

            _chamber = new Chamber(this, new Vector3(_tiles.GetLength(0), _tiles.GetLength(2), _tiles.GetLength(1)));
            
            _centerOfBuilding = new Vector3((max.X - min.X) / 2, (max.Y - min.Y) / 2, (max.Z - min.Z) / 2);
            var zoom = MathHelper.Max(max.X, MathHelper.Max(max.Y, max.Z)) * 1.2f + 4;
            _cameraPosition = new Vector3(0, -_centerOfBuilding.Z + 2 , zoom);
            _cameraTarget = new Vector3(0, _centerOfBuilding.Z - 2, -zoom);
            ViewMatrix = Matrix.CreateLookAt(_cameraPosition, new Vector3(0, _centerOfBuilding.Z - 0.7f, -zoom), Vector3.Up);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                (float)Window.ClientBounds.Width / Window.ClientBounds.Height,
                1, 100);
            WorldMatrix = Matrix.CreateWorld(new Vector3(-_centerOfBuilding.X - 0.5f, -_centerOfBuilding.Z + 0.5f, -_centerOfBuilding.Y - 0.5f), new Vector3(0, 0, -1), Vector3.Up);

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                WorldMatrix *= Matrix.CreateRotationX(MathHelper.ToRadians(1));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                WorldMatrix *= Matrix.CreateRotationX(-1 * MathHelper.ToRadians(1));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                WorldMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(1));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                WorldMatrix *= Matrix.CreateRotationY(-1 * MathHelper.ToRadians(1));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                WorldMatrix = Matrix.CreateWorld(new Vector3(-_centerOfBuilding.X - 0.5f, -_centerOfBuilding.Z + 0.5f, -_centerOfBuilding.Y - 0.5f), new Vector3(0, 0, -1), Vector3.Up);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                _cameraPosition = new Vector3(_cameraPosition.X, _cameraPosition.Y, _cameraPosition.Z - 0.1f);
                ViewMatrix = Matrix.CreateLookAt(_cameraPosition, _cameraTarget, Vector3.Up);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                _cameraPosition = new Vector3(_cameraPosition.X, _cameraPosition.Y, _cameraPosition.Z + 0.1f);
                ViewMatrix = Matrix.CreateLookAt(_cameraPosition, _cameraTarget, Vector3.Up);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (var cube in _cubes)
            {
                cube.Draw();
            }
            _chamber.Draw();
            base.Draw(gameTime);
        }
    }
}