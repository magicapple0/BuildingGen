using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using BuildingGen;

namespace Visualize
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        public Matrix projectionMatrix;
        public Matrix viewMatrix;
        public Matrix worldMatrix;
        public BasicEffect effect { get; set; }
        Vector3 centerOfBuilding;
        Vector3 cameraPosition;
        Vector3 cameraTarget;
        private Tile[,] tiles;
        private Chamber chamber;

        List<Cube> cubes = new List<Cube>();
        List<VertexPositionColor> vertices = new List<VertexPositionColor>();
        int[][][] tallBuilding = new int[][][]
        {
                          new int[][] {
                          new int[] { 1, 1, 1 },
                          new int[] { 1, 1, 1 },
                          new int[] { 1, 1, 1 }}, //first floor
                          new int[][] {
                          new int[] { 1, 1, 1 }, 
                          new int[] { 1, 1, 1 }, 
                          new int[] { 0, 1, 1 }},
                          new int[][] {
                          new int[] { 1, 1, 1 },
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},
        };

        int[][][] building = new int[][][]
        {
                          new int[][] {
                          new int[] { 1, 1, 1 },
                          new int[] { 1, 1, 1 },
                          new int[] { 1, 1, 1 }}, //first floor
                          new int[][] {
                          new int[] { 1, 1, 1 },
                          new int[] { 1, 1, 1 },
                          new int[] { 0, 1, 1 }},
                          new int[][] {
                          new int[] { 1, 1, 1 },
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},

        };

        int[][][] wideBuilding = new int[][][]
        {
                          new int[][] {
                          new int[] { 1, 1, 1 ,  1, 1, 1 ,  1, 1, 1 ,  1, 1, 1 },
                          new int[] { 1, 1, 1 ,  1, 1, 1 ,  1, 1, 1 ,  1, 1, 1 },
                          new int[] { 1, 1, 1 ,  1, 1, 1 ,  1, 1, 1 ,  1, 1, 1 },}, //first floor
                          new int[][] {
                          new int[] { 1, 1, 1 },
                          new int[] { 1, 1, 1 , 0, 0, 1, 1 },
                          new int[] { 0, 1, 1 }},
                          new int[][] {
                          new int[] { 1, 1, 1 },
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 }},
                          new int[][] {
                          new int[] { 0, 1, 0 },
                          new int[] { 0, 0, 0 },
                          new int[] { 0, 0, 0 }},

        };

        public Game1(Tile[,] tiles)
        {
            this.tiles = tiles;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //screen settings
            var _windowMultiplier = 2;
            var _screenWidth = 540;
            var _screenHeight = 380;
            Window.Position = new Point(
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - (_screenWidth),
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (_screenHeight)
                );
            var _nativeRenderTarget = new RenderTarget2D(GraphicsDevice, _screenWidth, _screenHeight);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = _screenWidth * _windowMultiplier;
            graphics.PreferredBackBufferHeight = _screenHeight * _windowMultiplier;
            graphics.ApplyChanges();

            //cubes init
            Vector3 min = new Vector3(666, 666, 666);
            Vector3 max = new Vector3(-666, -666, -666);

            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    if (tiles[i,j].TileInfo.Name.Equals("house"))
                    {
                        cubes.Add(new Cube(this, new Vector3(i, 0, j)));
                        //min.X = MathHelper.Min(min.X, i);
                        min.Z = MathHelper.Min(min.Z, 1);
                        max.Z = MathHelper.Max(max.Z, 1);
                        min.Y = MathHelper.Min(min.Y, i);
                        max.Y = MathHelper.Max(max.Y, i);
                        min.X = MathHelper.Min(min.X, j);
                        max.X = MathHelper.Max(max.X, j);
                    }
                }
            }

            chamber = new Chamber(this, new Vector3(tiles.GetLength(0), 1, tiles.GetLength(1)));

            /*//var b = building;
            var b = tallBuilding;
            //var b = wideBuilding;
            
            for (var i = 0; i < b.Length; i++)
            {
                for (var j = 0; j < b[i].Length; j++)
                {
                    for (var k = 0; k < b[i][j].Length; k++)
                    {
                        if (b[i][j][k] == 1)
                        {
                            cubes.Add(new Cube(this, new Vector3(k, i, j)));
                            //min.X = MathHelper.Min(min.X, i);
                            min.Z = MathHelper.Min(min.Z, i);
                            max.Z = MathHelper.Max(max.Z, i);
                            min.Y = MathHelper.Min(min.Y, j);
                            max.Y = MathHelper.Max(max.Y, j);
                            min.X = MathHelper.Min(min.X, k);
                            max.X = MathHelper.Max(max.X, k);
                        }
                    }
                }
            }*/

            //camera settings
            centerOfBuilding = new Vector3((max.X - min.X) / 2, (max.Y - min.Y) / 2, (max.Z - min.Z) / 2);
            var zoom = MathHelper.Max(max.X, MathHelper.Max(max.Y, max.Z)) * 1.2f + 4;
            cameraPosition = new Vector3(0, -centerOfBuilding.Z + 2 , zoom);
            cameraTarget = new Vector3(0, centerOfBuilding.Z - 2, -zoom);
            viewMatrix = Matrix.CreateLookAt(cameraPosition, new Vector3(0, centerOfBuilding.Z - 0.7f, -zoom), Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                (float)Window.ClientBounds.Width / (float)Window.ClientBounds.Height,
                1, 100);
            worldMatrix = Matrix.CreateWorld(new Vector3(-centerOfBuilding.X - 0.5f, -centerOfBuilding.Z + 0.5f, -centerOfBuilding.Y - 0.5f), new Vector3(0, 0, -1), Vector3.Up);

            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        { }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                worldMatrix *= Matrix.CreateRotationX(MathHelper.ToRadians(1));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                worldMatrix *= Matrix.CreateRotationX(-1 * MathHelper.ToRadians(1));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                worldMatrix *= Matrix.CreateRotationY(MathHelper.ToRadians(1));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                worldMatrix *= Matrix.CreateRotationY(-1 * MathHelper.ToRadians(1));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                worldMatrix = Matrix.CreateWorld(new Vector3(-centerOfBuilding.X - 0.5f, -centerOfBuilding.Z + 0.5f, -centerOfBuilding.Y - 0.5f), new Vector3(0, 0, -1), Vector3.Up);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                cameraPosition = new Vector3(cameraPosition.X, cameraPosition.Y, cameraPosition.Z - 0.1f);
                viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                cameraPosition = new Vector3(cameraPosition.X, cameraPosition.Y, cameraPosition.Z + 0.1f);
                viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (var cube in cubes)
            {
                cube.Draw();
            }
            chamber.Draw();
            base.Draw(gameTime);
        }
    }
}