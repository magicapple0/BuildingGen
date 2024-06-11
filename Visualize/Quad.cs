using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Visualize
{
    public class Quad : IDisposable
    {
        private VertexPositionTexture[] _vertices;
        private short[] _indices;
        private readonly Core _game;
        private BasicEffect _effect;
        private Texture2D _texture;

        public Quad(Core game, Texture2D texture, Vector3[] positions)
        {
            _game = game;
            InitializeVertices(positions);
            InitializeIndices();
            InitializeEffect(texture);
        }

        private void InitializeVertices(Vector3[] position)
        {
            _vertices = new VertexPositionTexture[4];

            // Define vertex 0 (top left)
            _vertices[0].Position = position[0];
            _vertices[0].TextureCoordinate = new Vector2(0, -1);
            // Define vertex 1 (top right)
            _vertices[1].Position = position[1];
            _vertices[1].TextureCoordinate = new Vector2(1, -1);
            // define vertex 2 (bottom right)
            _vertices[2].Position = position[2];
            _vertices[2].TextureCoordinate = new Vector2(1, 0);
            // define vertex 3 (bottom left) 
            _vertices[3].Position = position[3];
            _vertices[3].TextureCoordinate = new Vector2(0, 0);
            
        }

        private void InitializeIndices()
        {
            _indices = new short[6];
            // Define triangle 0 
            _indices[3] = 0;
            _indices[4] = 1;
            _indices[5] = 2;
            // define triangle 1
            _indices[0] = 2;
            _indices[1] = 3;
            _indices[2] = 0;
        }

        private void InitializeEffect(Texture2D texture)
        {
            _effect = new BasicEffect(_game.GraphicsDevice);
            _effect.TextureEnabled = true;
            _effect.Texture = texture;
        }

        public void Draw()
        {
            _effect.World = _game.WorldMatrix;
            _effect.Projection = _game.ProjectionMatrix;
            _effect.View = _game.ViewMatrix;
            _effect.CurrentTechnique.Passes[0].Apply();
            _game.GraphicsDevice.DrawUserIndexedPrimitives(
                PrimitiveType.TriangleList,
                _vertices,   // The vertex collection
                0,          // The starting index in the vertex array
                4,          // The number of indices in the shape
                _indices,    // The index collection
                0,          // The starting index in the index array
                2           // The number of triangles to draw
                );
        }

        public void Dispose()
        {
            _effect?.Dispose();
            _texture?.Dispose();
        }
    }
}
