using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Visualize;

public class Chamber
{
    private Vector3 size;
    Game1 game;
    BasicEffect effect;
    VertexBuffer vertexBuffer;
    IndexBuffer indexBuffer;
    ushort[] indices;
    VertexPositionColor[] vertices { set; get; }
    
    public Chamber(Game1 _game, Vector3 _size)
    {
        size = _size;
        game = _game;
        effect = new BasicEffect(game.GraphicsDevice);
        InitializeVertexCoordinates(size);
        InitializeEffect();
        InitializeIndices();
    }

    private void InitializeEffect()
    {
        effect = new BasicEffect(game.GraphicsDevice);
        effect.VertexColorEnabled = true;
    }

    private void InitializeVertexCoordinates(Vector3 size)
    {
        vertices = new VertexPositionColor[8];
        vertices[0] = new VertexPositionColor(new Vector3(0, size.Y, size.Z), Color.Red);
        vertices[1] = new VertexPositionColor(new Vector3(size.X, size.Y, size.Z), Color.Red);
        vertices[2] = new VertexPositionColor(new Vector3(size.X, 0, size.Z), Color.Red);
        vertices[3] = new VertexPositionColor(new Vector3(0, 0, size.Z), Color.Red);
 
        vertices[4] = new VertexPositionColor(new Vector3(0, size.Y, 0), Color.Red);
        vertices[5] = new VertexPositionColor(new Vector3(size.X, size.Y, 0), Color.Red);
        vertices[6] = new VertexPositionColor(new Vector3(size.X, 0, 0), Color.Red);
        vertices[7] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Red);
        
        vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColor), 8, BufferUsage.None);
        vertexBuffer.SetData(vertices.ToArray());
    }

    private void InitializeIndices()
    {
        indices = new ushort[]
        {
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
        indexBuffer.SetData(indices);
    }
    
    public void Draw()
    {
        effect.World = game.worldMatrix;
        effect.View = game.viewMatrix;
        effect.Projection = game.projectionMatrix;

        game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
        game.GraphicsDevice.Indices = indexBuffer;
        //drawing the edges
        foreach (EffectPass pass in effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, 12);
        }
    }
}