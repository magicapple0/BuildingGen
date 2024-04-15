using Microsoft.Xna.Framework;
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
        public Vector3 Position { get; set; }
        public ushort[] LineCubeIndices { get; }
        public VertexPositionColor[] triangleVertices { get; }

        public Cube(Vector3 position) 
        {
            LineCubeIndices = new ushort[] {
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
            Position = position;

            triangleVertices = setVertexCoordinates(position);
        }

        private VertexPositionColor[] setVertexCoordinates(Vector3 position)
        {
            var vertices = new VertexPositionColor[8];
            vertices[0] = new VertexPositionColor(new Vector3(position.X, position.Y + 1, position.Z + 1), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(position.X + 1, position.Y + 1, position.Z + 1), Color.Green);
            vertices[2] = new VertexPositionColor(new Vector3(position.X + 1, position.Y, position.Z + 1), Color.Yellow);
            vertices[3] = new VertexPositionColor(new Vector3(position.X, position.Y, position.Z + 1), Color.Blue);

            vertices[4] = new VertexPositionColor(new Vector3(position.X, position.Y + 1, position.Z), Color.Red);
            vertices[5] = new VertexPositionColor(new Vector3(position.X + 1, position.Y + 1, position.Z), Color.Green);
            vertices[6] = new VertexPositionColor(new Vector3(position.X + 1, position.Y, position.Z), Color.Yellow);
            vertices[7] = new VertexPositionColor(new Vector3(position.X, position.Y, position.Z), Color.Blue);
            /*vertices[0] = new VertexPositionColor(new Vector3(-1, 1, 1), Color.Red);
            vertices[1] = new VertexPositionColor(new Vector3(1, 1, 1), Color.Green);
            vertices[2] = new VertexPositionColor(new Vector3(1, -1, 1), Color.Yellow);
            vertices[3] = new VertexPositionColor(new Vector3(-1, -1, 1), Color.Blue);

            vertices[4] = new VertexPositionColor(new Vector3(-1, 1, -1), Color.Red);
            vertices[5] = new VertexPositionColor(new Vector3(1, 1, -1), Color.Green);
            vertices[6] = new VertexPositionColor(new Vector3(1, -1, -1), Color.Yellow);
            vertices[7] = new VertexPositionColor(new Vector3(-1, -1, -1), Color.Blue);*/
            return vertices;
        }
    }
}
