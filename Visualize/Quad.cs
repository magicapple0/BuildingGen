using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace Visualize
{
    public class Quad
    {
        VertexPositionTexture[] vertices;
        short[] indices;
        Game1 game;
        BasicEffect effect;
        Texture2D texture;

        public Quad(Game1 game, string texturePath, Vector3[] positions)
        {
            this.game = game;
            InitializeVertices(positions);
            InitializeIndices();
            InitializeEffect(texturePath);
        }

        public void InitializeVertices(Vector3[] position)
        {
            vertices = new VertexPositionTexture[4];

            // Define vertex 0 (top left)
            vertices[0].Position = position[0];
            vertices[0].TextureCoordinate = new Vector2(0, -1);
            // Define vertex 1 (top right)
            vertices[1].Position = position[1];
            vertices[1].TextureCoordinate = new Vector2(1, -1);
            // define vertex 2 (bottom right)
            vertices[2].Position = position[2];
            vertices[2].TextureCoordinate = new Vector2(1, 0);
            // define vertex 3 (bottom left) 
            vertices[3].Position = position[3];
            vertices[3].TextureCoordinate = new Vector2(0, 0);
            
        }

        public void InitializeIndices()
        {
            indices = new short[6];
            // Define triangle 0 
            indices[3] = 0;
            indices[4] = 1;
            indices[5] = 2;
            // define triangle 1
            indices[0] = 2;
            indices[1] = 3;
            indices[2] = 0;
        }

        public void InitializeEffect(string texturePath)
        {
            effect = new BasicEffect(game.GraphicsDevice);
            using (var stream = TitleContainer.OpenStream(texturePath))
            {
                texture = Texture2D.FromStream(game.GraphicsDevice, stream);
            }
            effect.TextureEnabled = true;
            effect.Texture = texture;
        }

        public void Draw()
        {
            effect.World = game.worldMatrix;
            effect.Projection = game.projectionMatrix;
            effect.View = game.viewMatrix;
            effect.CurrentTechnique.Passes[0].Apply();
            game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                PrimitiveType.TriangleList,
                vertices,   // The vertex collection
                0,          // The starting index in the vertex array
                4,          // The number of indices in the shape
                indices,    // The index collection
                0,          // The starting index in the index array
                2           // The number of triangles to draw
                );
        }

    }
}
