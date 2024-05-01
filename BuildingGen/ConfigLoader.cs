using System.Text.Json;

namespace BuildingGen
{
    public class ConfigLoader 
    {
        public InputJson? InputJson { get; private set; }
        public List<Tile> Tiles { get; private set; }
        private Tile Ground = new (new TileInfo(
            "ground", new [] {
                    new []{"air"},
                    new []{"ground"},
                    new []{""},
                    new []{"ground"},
                    new []{"ground"},
                    new []{"ground"}
                },false, false, null, null));
        private Tile Air = new (new TileInfo(
            "air",new []{
                    new []{"air"},
                    new []{"air"},
                    new []{"air", "ground"},
                    new []{"air"},
                    new []{"air"},
                    new []{"air"}
                }, false, false, null, null));

        public ConfigLoader(string fileName) {

            InputJson = JsonSerializer.Deserialize<InputJson>(File.ReadAllText(fileName));
            Tiles = new List<Tile>();
            foreach (var tileInfo in InputJson.TilesInfo)
            {
                var tiles = new List<Tile>
                {
                    new (tileInfo)
                };
                if (tileInfo.FlipX)
                {
                    tiles.Add(tiles[0].Copy());
                    tiles[^1].FlipX();
                }
                if (tileInfo.RotateZ)
                {
                    tiles.Add(tiles[0].Copy());
                    tiles[^1].RotateZTile();
                    tiles.Add(tiles[^1].Copy());
                    tiles[^1].RotateZTile();
                    tiles.Add(tiles[^1].Copy());
                    tiles[^1].RotateZTile();
                }
                if (tileInfo.RotateZ && tileInfo.FlipX)
                {
                    tiles.Add(tiles[1].Copy());
                    tiles[^1].RotateZTile();
                    tiles.Add(tiles[^1].Copy());
                    tiles[^1].RotateZTile();
                    tiles.Add(tiles[^1].Copy());
                    tiles[^1].RotateZTile();
                }
                Tiles.AddRange(tiles);
            }
            
            var groundNeighbors = new List<string>(InputJson.UsingTiles);
            groundNeighbors.AddRange(new List<string>(Ground.TileInfo.Edges[0]));
            Ground.TileInfo.Edges[0] = groundNeighbors.ToArray();
            for (var i = 0; i < 6; i++)
            {
                var airNeighbors = new List<string>(InputJson.UsingTiles);
                airNeighbors.AddRange(new List<string>(Air.TileInfo.Edges[i]));
                Air.TileInfo.Edges[i] = airNeighbors.ToArray();
            }
            Ground.ModifiedEdges = Ground.TileInfo.Edges;
            Air.ModifiedEdges = Air.TileInfo.Edges;
            Tiles.Add(Ground);
            Tiles.Add(Air);
        }
    }
}
