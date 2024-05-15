namespace BuildingGen;

public class TileManager
{
    public Tile[] TileSet { get; set; }
    public Tile Ground { get; private set; }
    public Tile Bound { get; private set; }
    private Tile Air { get; set; }
    private List<string> UsingTiles { get; set; }

    public TileManager(TileInfo[] tilesInfos)
    {
        var newTiles = new List<Tile>();
        UsingTiles = new List<string>();
        foreach (var tileInfo in tilesInfos)
        {
            if (!UsingTiles.Contains(tileInfo.Name))
                UsingTiles.Add(tileInfo.Name);
            AppendBoundsNeighbors(tileInfo);
            var tiles = GetModifiedTiles(tileInfo);
            newTiles.AddRange(tiles);
        }
        InitializeAir();
        InitializeGround();
        InitializeBound();
        newTiles.Add(Air!);

        TileSet = newTiles.ToArray();
    }
    
    public List<Tile> GetOddSymmetryTiles(Directions direction)
    {
        var symmetryTiles = new List<Tile>();
        var directions = DirectionConstants.CellOppositeSides[direction];
        foreach (var tile in TileSet)
        {
            var symmetryTile = tile.Copy();
            var modifiedEdges = tile.ModifiedEdges[directions.Item1].Where(neighbor => tile.ModifiedEdges[directions.Item2].Contains(neighbor)).ToList();
            if (modifiedEdges.Count == 0)
                continue;
            symmetryTile.ModifiedEdges[directions.Item1] = modifiedEdges.ToArray();
            symmetryTile.ModifiedEdges[directions.Item2] = modifiedEdges.ToArray();
            symmetryTiles.Add(symmetryTile);
        }
        return symmetryTiles;
    }

    public List<Tile> GetEvenSymmetryTiles(Directions direction)
    {
        var symmetryTiles = new List<Tile>();
        var directions = DirectionConstants.CellOppositeSides[direction];
        foreach (var tile1 in TileSet)
        {
            foreach (var tile2 in TileSet)
                if (tile1.TileInfo.Name == tile2.TileInfo.Name && tile1.ModifiedEdges[directions.Item1].Contains(tile2.TileInfo.Name) &&
                    tile2.ModifiedEdges[directions.Item2].Contains(tile1.TileInfo.Name))
                {
                    if (!symmetryTiles.Contains(tile1))
                    {
                        var symmetryTile = tile1.Copy();
                        symmetryTile.ModifiedEdges[directions.Item1] = new []{tile2.TileInfo.Name};
                        symmetryTiles.Add(symmetryTile);
                    }   
                }
            if (tile1.ModifiedEdges[directions.Item1].Contains(tile1.TileInfo.Name) && 
                tile1.ModifiedEdges[directions.Item2].Contains(tile1.TileInfo.Name) && !symmetryTiles.Contains(tile1))
                symmetryTiles.Add(tile1);
        }
        return symmetryTiles;
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
        Air = new Tile(new TileInfo(
            "air",new []{
                new []{"air", "bound"},
                new []{"air", "bound"},
                new []{"air", "ground"},
                new []{"air", "bound"},
                new []{"air", "bound"},
                new []{"air", "bound"}
            }, false, false, false, null, null));
        AddUsingTiles(Air);
        Air.ModifiedEdges = Air.TileInfo.Edges;
    }
    
    private void InitializeBound()
    {
        Bound = new Tile(new TileInfo(
            "bound",new []{
                new []{"bound"},
                new []{"air", "bound"},
                new []{"air", "ground", "bound"},
                new []{"air", "bound"},
                new []{"air", "bound"},
                new []{"air", "bound"}
            }, false, false, false, null, null));
        AddUsingTiles(Bound); 
        Bound.ModifiedEdges = Bound.TileInfo.Edges;
    }

    private void InitializeGround()
    {
        Ground = new Tile(new TileInfo(
            "ground", new [] {
                new []{"air", "bound"},
                new []{"ground"},
                new []{""},
                new []{"ground"},
                new []{"ground"},
                new []{"ground"}
            },false, false, false, null, null));
        var groundNeighbors = new List<string>(UsingTiles);
        groundNeighbors.AddRange(new List<string>(Ground.TileInfo.Edges[0]));
        Ground.TileInfo.Edges[0] = groundNeighbors.ToArray();
        Ground.ModifiedEdges = Ground.TileInfo.Edges;
    }

    private void AddUsingTiles(Tile tile)
    {
        for (var i = 0; i < 6; i++)
        {
            var neighbors = new List<string>(UsingTiles);
            neighbors.AddRange(new List<string>(tile.TileInfo.Edges[i]));
            tile.TileInfo.Edges[i] = neighbors.ToArray();
        }
    }
}