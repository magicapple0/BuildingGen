using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingGen
{
    public class TileInfo
    {
        public string Name { get; set; }
        public string[][] Edges { get; set; }
        public bool FlipX { get; set; }
        public bool RotateZ { get; set; }
        public string? Color { get; set; }
        public string[]? Texture { get; set; }
        public TileInfo() { }
        public TileInfo(string name, string[][] edges, bool flipX, bool rotateZ, string? color, string[]? texture)
        {
            Name = name;
            Edges = edges;
            FlipX = flipX;
            RotateZ = rotateZ;
            Color = color;
            Texture = texture;
            if (texture == null && color == null)
            {
                Texture = new [] { "img.png", "img.png", "img.png", "img.png", "img.png", "img.png" };
            }
        }
    }
}
