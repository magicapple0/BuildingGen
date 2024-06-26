﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Visualize;

public class Border : IDisposable
{
    private Vector3 _position;
    private Vector3 _size;
    private Core _game;
    private BasicEffect _effect;
    private VertexBuffer _vertexBuffer;
    private IndexBuffer _indexBuffer;
    private ushort[] _indices;
    private VertexPositionColor[] _vertices;
    
    public Border(Core game, Vector3 pos, Vector3 size)
    {
        _position = pos;
        _size = pos + size;
        _game = game;
        _effect = new BasicEffect(_game.GraphicsDevice);
        InitializeVertexCoordinates(_size);
        InitializeEffect();
        InitializeIndices();
    }
    
    public Border(Core game, Vector3 size)
    {
        _position = new Vector3(0, 0, 0);
        _size = size;
        _game = game;
        _effect = new BasicEffect(_game.GraphicsDevice);
        InitializeVertexCoordinates(_size);
        InitializeEffect();
        InitializeIndices();
    }

    private void InitializeEffect()
    {
        _effect = new BasicEffect(_game.GraphicsDevice);
        _effect.VertexColorEnabled = true;
    }

    private void InitializeVertexCoordinates(Vector3 size)
    {
        _vertices = new VertexPositionColor[8];
        _vertices[0] = new VertexPositionColor(new Vector3(_position.X, size.Y, size.Z), Color.Red);
        _vertices[1] = new VertexPositionColor(new Vector3(size.X, size.Y, size.Z), Color.Red);
        _vertices[2] = new VertexPositionColor(new Vector3(size.X, _position.Y, size.Z), Color.Red);
        _vertices[3] = new VertexPositionColor(new Vector3(_position.X, _position.Y, size.Z), Color.Red);
 
        _vertices[4] = new VertexPositionColor(new Vector3(_position.X, size.Y, _position.Z), Color.Red);
        _vertices[5] = new VertexPositionColor(new Vector3(size.X, size.Y, _position.Z), Color.Red);
        _vertices[6] = new VertexPositionColor(new Vector3(size.X, _position.Y, _position.Z), Color.Red);
        _vertices[7] = new VertexPositionColor(new Vector3(_position.X, _position.Y, _position.Z), Color.Red);
        
        _vertexBuffer = new VertexBuffer(_game.GraphicsDevice, typeof(VertexPositionColor), 8, BufferUsage.None);
        _vertexBuffer.SetData(_vertices.ToArray());
    }

    private void InitializeIndices()
    {
        _indices = new ushort[]
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
        
        _indexBuffer = new IndexBuffer(_game.GraphicsDevice, typeof(ushort), 24, BufferUsage.WriteOnly);
        _indexBuffer.SetData(_indices);
    }
    
    public void Draw()
    {
        _effect.World = _game.WorldMatrix;
        _effect.View = _game.ViewMatrix;
        _effect.Projection = _game.ProjectionMatrix;

        _game.GraphicsDevice.SetVertexBuffer(_vertexBuffer);
        _game.GraphicsDevice.Indices = _indexBuffer;
        //drawing the edges
        foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
        {
            pass.Apply();
            _game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, 12);
        }
    }

    public void Dispose()
    {
        _effect?.Dispose();
        _vertexBuffer?.Dispose();
        _indexBuffer?.Dispose();
    }
}