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
        public bool FlipY { get; set; }
        public bool RotateZ { get; set; }
        public string? Color { get; set; }
        public string[]? Texture { get; set; }
        public TileInfo() { }
        public TileInfo(string name, string[][] edges, bool flipX, bool flipY, bool rotateZ, string? color, string[]? texture)
        {
            Name = name;
            Edges = edges;
            FlipX = flipX;
            FlipY = flipY;
            RotateZ = rotateZ;
            Color = color;
            Texture = texture;
            if (texture == null && color == null)
            {
                Texture = new [] { "Textures/img.png", "Textures/img.png", "Textures/img.png", 
                                   "Textures/img.png", "Textures/img.png", "Textures/img.png" };
            }
        }
    }
}
