using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace BuildingGen
{
    public class ConfigLoader 
    {
        public InputJson InputJson { get; private set; }
        public List<Tile> Tiles { get; private set; }
        private Tile Ground = new Tile(new TileInfo("ground",
            new []
                {
                    new []{"house","air"},
                    new []{"ground"},
                    new []{"house","air"},
                    new []{"ground"},
                    new []{"ground"},
                    new []{"ground"}
                },
            false, false), 
            new []
            {
                new []{"house","air"},
                new []{"ground"},
                new []{"house","air"},
                new []{"ground"},
                new []{"ground"},
                new []{"ground"}
            },
            new List<TileModifiers>());
        private Tile Air = new Tile(new TileInfo("air",
                new []
                {
                    new []{"house", "air", "roof"},
                    new []{"house", "air", "roof"},
                    new []{"house", "air", "ground", "roof"},
                    new []{"house", "air", "roof"},
                    new []{"house", "air", "roof"},
                    new []{"house", "air", "roof"}
                },
                false, false), 
            new []
            {
                new []{"house", "air", "ground", "roof"},
                new []{"house", "air", "ground", "roof"},
                new []{"house", "air", "ground", "roof"},
                new []{"house", "air", "ground", "roof"},
                new []{"house", "air", "ground", "roof"},
                new []{"house", "air", "ground", "roof"}
            },
            new List<TileModifiers>());

        private readonly string fileName;
        public ConfigLoader(string fileName) {
            this.fileName = fileName;
            InputJson = JsonSerializer.Deserialize<InputJson>(File.ReadAllText(fileName));
            Tiles = new List<Tile>();
            //var currTile = TileInfoToTile(InputJson.Tiles.FirstOrDefault());
            //var modifiers = new[] { None, FlipXTile, FlipYTile, FlipZTile, RotateXTile };
            //var modifiers = new[] {None, FlipXTile, FlipYTile, FlipZTile, RotateXTile, RotateXTile, RotateXTile, RotateYTile, RotateYTile, RotateYTile, RotateZTile, RotateZTile, RotateZTile };
            //var automat = new Automatoe(modifiers);
            foreach (var tile in InputJson.Tiles)
            {
                var tiles = new List<Tile>
                {
                    TileInfoToTile(tile)
                };
                if (tile.FlipX)
                {
                    tiles.Add(FlipXTile(tiles[0]));
                }
                if (tile.RotateZ)
                {
                    tiles.Add(RotateZTile(tiles[0]));
                    tiles.Add(RotateZTile(tiles[^1]));
                    tiles.Add(RotateZTile(tiles[^1]));
                }
                if (tile.RotateZ && tile.FlipX)
                {
                    tiles.Add(RotateZTile(tiles[1]));
                    tiles.Add(RotateZTile(tiles[^1]));
                    tiles.Add(RotateZTile(tiles[^1]));
                }
                Tiles.AddRange(tiles);
            }
            Tiles.Add(Ground);
            Tiles.Add(Air);
        }


        public string GetJsonString()
        {
            return File.ReadAllText(fileName);
        }

        private Tile TileInfoToTile(TileInfo tileInfo)
        {
            return new Tile(tileInfo, tileInfo.Edges.ToArray(), new List<TileModifiers>());
        }

        private Tile None(Tile tile)
        {
            var newEdges = new string[][] { tile.ModifiedEdges[0], tile.ModifiedEdges[1], tile.ModifiedEdges[2], tile.ModifiedEdges[3], tile.ModifiedEdges[4], tile.ModifiedEdges[5] };
            var newModifiers = tile.TileModifiers.ToList();
            newModifiers.Add(TileModifiers.None);
            return new Tile(tile.TileInfo, newEdges, newModifiers);
        }

        private Tile FlipXTile(Tile tile)
        {
            var newEdges = new string[][] { tile.ModifiedEdges[0], tile.ModifiedEdges[1], tile.ModifiedEdges[2], tile.ModifiedEdges[4], tile.ModifiedEdges[3], tile.ModifiedEdges[5] };
            var newModifiers = tile.TileModifiers.ToList();
            newModifiers.Add(TileModifiers.FlipX);
            return new Tile(tile.TileInfo, newEdges, newModifiers);
        }


        private Tile FlipYTile(Tile tile)
        {
            var newEdges = new string[][] { tile.ModifiedEdges[0], tile.ModifiedEdges[5], tile.ModifiedEdges[2], tile.ModifiedEdges[3], tile.ModifiedEdges[4], tile.ModifiedEdges[1] };
            var newModifiers = tile.TileModifiers.ToList();
            newModifiers.Add(TileModifiers.FlipY);
            return new Tile(tile.TileInfo, newEdges, newModifiers);
        }

        private Tile FlipZTile(Tile tile)
        {
            var newEdges = new string[][] { tile.ModifiedEdges[2], tile.ModifiedEdges[1], tile.ModifiedEdges[0], tile.ModifiedEdges[3], tile.ModifiedEdges[4], tile.ModifiedEdges[5] };
            var newModifiers = tile.TileModifiers.ToList();
            newModifiers.Add(TileModifiers.FlipZ);
            return new Tile(tile.TileInfo, newEdges, newModifiers);
        }

        private Tile RotateXTile(Tile tile)
        {
            var newEdges = new string[][] { tile.ModifiedEdges[4], tile.ModifiedEdges[1], tile.ModifiedEdges[3], tile.ModifiedEdges[0], tile.ModifiedEdges[2], tile.ModifiedEdges[5] };
            var newModifiers = tile.TileModifiers.ToList();
            newModifiers.Add(TileModifiers.RotateX);
            return new Tile(tile.TileInfo, newEdges, newModifiers);
        }

        private Tile RotateYTile(Tile tile)
        {
            var newEdges = new string[][] { tile.ModifiedEdges[5], tile.ModifiedEdges[0], tile.ModifiedEdges[1], tile.ModifiedEdges[3], tile.ModifiedEdges[4], tile.ModifiedEdges[2] };
            var newModifiers = tile.TileModifiers.ToList();
            newModifiers.Add(TileModifiers.RotateY);
            return new Tile(tile.TileInfo, newEdges, newModifiers);
        }

        private Tile RotateZTile(Tile tile)
        {
            var newEdges = new string[][] { tile.ModifiedEdges[0], tile.ModifiedEdges[3], tile.ModifiedEdges[2], tile.ModifiedEdges[5], tile.ModifiedEdges[1], tile.ModifiedEdges[4] };
            var newModifiers = tile.TileModifiers.ToList();
            newModifiers.Add(TileModifiers.RotateZ);
            return new Tile(tile.TileInfo, newEdges, newModifiers);
        }
    }
}
