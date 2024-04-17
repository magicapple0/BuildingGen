using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualize
{
    public class Cube
    {
        Vector3 position { get; set; }
        Game1 game;
        BasicEffect effect;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        ushort[] indices;
        VertexPositionColor[] vertices { set; get; }
        Quad[] faces;

        public Cube(Game1 game, Vector3 position) 
        {
            this.game = game;
            effect = new BasicEffect(game.GraphicsDevice);
            this.position = position;
            initializeVertexCoordinates(position);
            InitializeEffect();
            InitializeIndices();
            InitializeFaces();
        }

        private void InitializeFaces()
        {
            faces = new Quad[6];
            faces[0] = new Quad(game, "wall.png",
                new Vector3[] { position + new Vector3(0, 1, 1), position + new Vector3(1, 1, 1), position + new Vector3(1, 0, 1), position + new Vector3(0, 0, 1) }); //front
            faces[1] = new Quad(game, "wall.png",
                new Vector3[] { position + new Vector3(1, 1, 1), position + new Vector3(1, 1, 0), position + new Vector3(1, 0, 0), position + new Vector3(1, 0, 1) }); //right
            faces[2] = new Quad(game, "wall.png",
                new Vector3[] { position + new Vector3(0, 1, 0), position + new Vector3(0, 1, 1), position + new Vector3(0, 0, 1), position + new Vector3(0, 0, 0) }); //left
            faces[3] = new Quad(game, "wall.png",
                new Vector3[] { position + new Vector3(1, 1, 0), position + new Vector3(0, 1, 0), position + new Vector3(0, 0, 0), position + new Vector3(1, 0, 0) }); //back
            faces[4] = new Quad(game, "roof.png",
                new Vector3[] { position + new Vector3(0, 1, 1), position + new Vector3(0, 1, 0), position + new Vector3(1, 1, 0), position + new Vector3(1, 1, 1) }); //up
            faces[5] = new Quad(game, "bottom.png",
                new Vector3[] { position + new Vector3(1, 0, 0), position + new Vector3(0, 0, 0), position + new Vector3(0, 0, 1), position + new Vector3(1, 0, 1) }); //down
        }

        public void InitializeIndices()
        {
            indices = new ushort[] {
                0,1,// передние линии
                1,2,
                2,3,
                3,0,

                4,5, // задние линии
                5,6,
                6,7,
                7,4,

                0,4,// боковые линии
                3,7,
                1,5,
                2,6
            };

            indexBuffer = new IndexBuffer(game.GraphicsDevice, typeof(ushort), 24, BufferUsage.WriteOnly);
            indexBuffer.SetData<ushort>(indices);
        }

        public void InitializeEffect()
        {
            effect = new BasicEffect(game.GraphicsDevice);
            effect.VertexColorEnabled = true;
        }

        private void initializeVertexCoordinates(Vector3 position)
        {
            vertices = new VertexPositionColor[8];
            vertices[0] = new VertexPositionColor(new Vector3(position.X, position.Y + 1, position.Z + 1), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(position.X + 1, position.Y + 1, position.Z + 1), Color.Green);
            vertices[2] = new VertexPositionColor(new Vector3(position.X + 1, position.Y, position.Z + 1), Color.Yellow);
            vertices[3] = new VertexPositionColor(new Vector3(position.X, position.Y, position.Z + 1), Color.Blue);
            vertices[4] = new VertexPositionColor(new Vector3(position.X, position.Y + 1, position.Z), Color.Red);
            vertices[5] = new VertexPositionColor(new Vector3(position.X + 1, position.Y + 1, position.Z), Color.Green);
            vertices[6] = new VertexPositionColor(new Vector3(position.X + 1, position.Y, position.Z), Color.Yellow);
            vertices[7] = new VertexPositionColor(new Vector3(position.X, position.Y, position.Z), Color.Blue);

            vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColor), 8, BufferUsage.None);
            vertexBuffer.SetData(vertices.ToArray());
        }

        public void Draw()
        {
            effect.World = game.worldMatrix;
            effect.View = game.viewMatrix;
            effect.Projection = game.projectionMatrix;

            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            game.GraphicsDevice.Indices = indexBuffer;
            //drawing the edges
            /*foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, 12);
            }*/
            foreach (var face in faces)
            {
                face.Draw();
            }
        }
    }
}
