using System.Text.Json;

namespace BuildingGen
{
    public class ConfigLoader 
    {
        private InputJson? InputJson { get; set; }
        public List<Tile> Tiles { get; private set; }
        private Tile _ground;
        private Tile _air;
        private Tile _bound;

        public ConfigLoader(string fileName) {

            InputJson = JsonSerializer.Deserialize<InputJson>(File.ReadAllText(fileName));
            Tiles = new List<Tile>();
            foreach (var tileInfo in InputJson.TilesInfo)
            {
                AppendBoundsNeighbors(tileInfo);
                var tiles = GetModifiedTiles(tileInfo);
                Tiles.AddRange(tiles);
            }
            
            InitializeAir();
            InitializeGround();
            InitializeBound();
        }

        private static void AppendBoundsNeighbors(TileInfo tileInfo)
        {
            for (var i = 0; i < 6; i++)
                if (tileInfo.Edges[i].Contains("air"))
                    tileInfo.Edges[i] = new List<string>(tileInfo.Edges[i]).Append("bound").ToArray();
        }

        private static List<Tile> GetModifiedTiles(TileInfo tileInfo)
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

            return tiles;
        }

        private void InitializeAir()
        {
            _air = new Tile(new TileInfo(
                "air",new []{
                    new []{"air", "bound"},
                    new []{"air", "bound"},
                    new []{"air", "ground"},
                    new []{"air", "bound"},
                    new []{"air", "bound"},
                    new []{"air", "bound"}
                }, false, false, null, null));
            AddUsingTiles(_air);
            _air.ModifiedEdges = _air.TileInfo.Edges;
            Tiles.Add(_air);
        }
        
        private void InitializeBound()
        {
            _bound = new Tile(new TileInfo(
                "bound",new []{
                    new []{"bound"},
                    new []{"air", "bound"},
                    new []{"air", "ground", "bound"},
                    new []{"air", "bound"},
                    new []{"air", "bound"},
                    new []{"air", "bound"}
                }, false, false, null, null));
            AddUsingTiles(_bound); 
            _bound.ModifiedEdges = _bound.TileInfo.Edges;
            Tiles.Add(_bound);
        }

        private void InitializeGround()
        {
            _ground = new Tile(new TileInfo(
                "ground", new [] {
                    new []{"air", "bound"},
                    new []{"ground"},
                    new []{""},
                    new []{"ground"},
                    new []{"ground"},
                    new []{"ground"}
                },false, false, null, null));
            var groundNeighbors = new List<string>(InputJson.UsingTiles);
            groundNeighbors.AddRange(new List<string>(_ground.TileInfo.Edges[0]));
            _ground.TileInfo.Edges[0] = groundNeighbors.ToArray();
            _ground.ModifiedEdges = _ground.TileInfo.Edges;
            Tiles.Add(_ground);
        }

        private void AddUsingTiles(Tile tile)
        {
            for (var i = 0; i < 6; i++)
            {
                var neighbors = new List<string>(InputJson.UsingTiles);
                neighbors.AddRange(new List<string>(tile.TileInfo.Edges[i]));
                tile.TileInfo.Edges[i] = neighbors.ToArray();
            }
        }
    }
}
