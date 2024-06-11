using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Visualize
{
    public class Cube : IDisposable
    {
        public Vector3 Position { get; }
        private readonly Core _game;
        private BasicEffect _effect;
        private Quad[] _faces;
        private readonly Texture2D[] _textures;

        public Cube(Core game, Vector3 position, Texture2D[] textures) 
        {
            _game = game;
            _textures = textures;
            _effect = new BasicEffect(game.GraphicsDevice);
            Position = position;
            InitializeEffect();
            InitializeFaces();
        }

        private void InitializeFaces()
        {
            _faces = new Quad[6];
            _faces[0] = new Quad(_game, _textures[1],
                new [] { Position + new Vector3(0, 1, 1), Position + new Vector3(1, 1, 1), Position + new Vector3(1, 0, 1), Position + new Vector3(0, 0, 1) }); //front
            _faces[1] = new Quad(_game, _textures[4],
                new [] { Position + new Vector3(1, 1, 1), Position + new Vector3(1, 1, 0), Position + new Vector3(1, 0, 0), Position + new Vector3(1, 0, 1) }); //right
            _faces[2] = new Quad(_game, _textures[3],
                new [] { Position + new Vector3(0, 1, 0), Position + new Vector3(0, 1, 1), Position + new Vector3(0, 0, 1), Position + new Vector3(0, 0, 0) }); //left
            _faces[3] = new Quad(_game, _textures[5],
                new [] { Position + new Vector3(1, 1, 0), Position + new Vector3(0, 1, 0), Position + new Vector3(0, 0, 0), Position + new Vector3(1, 0, 0) }); //back
            _faces[4] = new Quad(_game, _textures[0],
                new [] { Position + new Vector3(0, 1, 1), Position + new Vector3(0, 1, 0), Position + new Vector3(1, 1, 0), Position + new Vector3(1, 1, 1) }); //up
            _faces[5] = new Quad(_game, _textures[2],
                new [] { Position + new Vector3(1, 0, 0), Position + new Vector3(0, 0, 0), Position + new Vector3(0, 0, 1), Position + new Vector3(1, 0, 1) }); //down
        }

        private void InitializeEffect()
        {
            _effect = new BasicEffect(_game.GraphicsDevice);
            _effect.VertexColorEnabled = true;
        }
        
        public void Draw()
        {
            _effect.World = _game.WorldMatrix;
            _effect.View = _game.ViewMatrix;
            _effect.Projection = _game.ProjectionMatrix;

            foreach (var face in _faces)
            {
                face.Draw();
            }
        }

        public void Dispose()
        {
            _effect?.Dispose();
            foreach (var face in _faces)
            {
                face.Dispose();
            }
        }
    }
}
