namespace BuildingGen;

public class Map2
{
    private TileManager _tileManager;
    public Vector2 Size { get; private set; }
    private bool XSymmetry { get; init; }
    private bool YSymmetry { get; init; }
    private bool XEven { get; init; }
    private bool YEven { get; init; }
    public Dictionary<Vector2, Tile[]> Field { get; private init; }

    private Map2() { }

    public Map2(Vector2 size, TileManager tileManager, bool xSymmetry, bool ySymmetry)
    {
        _tileManager = tileManager;
        XSymmetry = xSymmetry;
        YSymmetry = ySymmetry;
        XEven = size.X % 2 == 0;
        YEven = size.Y % 2 == 0;
        Size = CalculateFieldSize(size);

        Field = new Dictionary<Vector2, Tile[]>();
        InitializeSimpleField();
        InitializeFieldBounds(_tileManager.Bound, _tileManager.Ground);
    }

    private void InitializeFieldBounds(Tile bound, Tile ground)
    {
        var bounds = new Dictionary<Directions2, List<Vector2>>
        {
            { Directions2.Face, Field.Where(x => x.Key.Y == 0).Select(x => x.Key).ToList() },
            { Directions2.Back, Field.Where(x => x.Key.Y == Size.Y - 1).Select(x => x.Key).ToList() },
            { Directions2.Right, Field.Where(x => x.Key.X == Size.X - 1).Select(x => x.Key).ToList() },
            { Directions2.Left, Field.Where(x => x.Key.X == 0).Select(x => x.Key).ToList() }
        };
        var boundTileSet = new List<Tile>(new[] { bound });
        if (!XSymmetry && !YSymmetry)
        {
            foreach (var direction in DirectionConstants2.DirectionsVectors)
                SetBoundTiles(boundTileSet, bounds[direction.Key]);
        }
        if (XSymmetry && !YSymmetry)
        {
            var xSymmetryTiles = XEven ? _tileManager.GetEvenSymmetryTiles(Directions2.Right) : 
                _tileManager.GetOddSymmetryTiles(Directions2.Right);
            SetBoundTiles(xSymmetryTiles, bounds[Directions2.Right]);
            SetBoundTiles(boundTileSet, bounds[Directions2.Left]);
            SetBoundTiles(boundTileSet, bounds[Directions2.Face]);
            SetBoundTiles(boundTileSet, bounds[Directions2.Back]);
        }
        if (YSymmetry && !XSymmetry)
        {
            var ySymmetryTiles = YEven ? _tileManager.GetEvenSymmetryTiles(Directions2.Back) : 
                _tileManager.GetOddSymmetryTiles(Directions2.Back);
            SetBoundTiles(ySymmetryTiles, bounds[Directions2.Back]);
            SetBoundTiles(boundTileSet, bounds[Directions2.Left]);
            SetBoundTiles(boundTileSet, bounds[Directions2.Face]);
            SetBoundTiles(boundTileSet, bounds[Directions2.Right]);
        }
        if (YSymmetry && XSymmetry)
        {
            var xSymmetryTiles = XEven ? _tileManager.GetEvenSymmetryTiles(Directions2.Right) : 
                _tileManager.GetOddSymmetryTiles(Directions2.Right);
            var ySymmetryTiles = YEven ? _tileManager.GetEvenSymmetryTiles(Directions2.Back) : 
                _tileManager.GetOddSymmetryTiles(Directions2.Back);
            SetBoundTiles(ySymmetryTiles, bounds[Directions2.Back]);
            SetBoundTiles(xSymmetryTiles, bounds[Directions2.Right]);
            SetBoundTiles(boundTileSet, bounds[Directions2.Left]);
            SetBoundTiles(boundTileSet, bounds[Directions2.Face]);
        }
    }
    
    private void InitializeSimpleField()
    {
        for (var i = 0; i < Size.X; i++)
        {
            for (var j = 0; j < Size.Y; j++)
            {
                Field[(i, j)] = _tileManager.TileSet.ToArray();
            }
        }
    }

    private void SetBoundTiles(List<Tile> boundTiles, List<Vector2> boundCells)
    {
        foreach (var cell in boundCells)
            Field[cell] = boundTiles.ToArray();
    }

    private Vector2 CalculateFieldSize(Vector2 size)
    {
        return new Vector2(0, 0)
        {
            X = XSymmetry ? (int)Math.Ceiling(size.X / (decimal)2) + 1 : size.X + 2,
            Y = YSymmetry ? (int)Math.Ceiling(size.Y / (decimal)2) + 1 : size.Y + 2
        };
    }
    
    public Dictionary<Vector2, Tile> Result()
    {
        var field = GetFieldDictionaryVectorTile();
        field = DeleteBounds(field);
        return FromCluster(field);
        if (XSymmetry && XEven)
        {
            field = XEvenMirrorResult(field);
            Size = Size with { X = Size.X * 2 };
        }
        if (XSymmetry && !XEven)
        {
            field = XOddMirrorResult(field);
            Size = Size with { X = Size.X * 2 - 1};
        }
        if (YSymmetry && YEven)
        {
            field = YEvenMirrorResult(field);
            Size = Size with { Y = Size.Y * 2 };
        }
        if (YSymmetry && !YEven)
        {
            field = YOddMirrorResult(field);
            Size = Size with { Y = Size.Y * 2 - 1};
        }
        field = DeleteBounds(field);

        return field;
    }

    private Dictionary<Vector2, Tile> FromCluster(Dictionary<Vector2, Tile> field)
    {
        var result = new Dictionary<Vector2, Tile>();
        foreach (var cell in field)
        {
            var cluster = _tileManager.TileClusters[Int32.Parse(cell.Value.TileInfo.Name)].Tiles;
            foreach (var tile in cluster)
            {
                result.Add((cell.Key.X * 3 + tile.Key.X, cell.Key.Y * 3 + tile.Key.Y), tile.Value);
            }
        }

        return result;
    }
    
    private Dictionary<Vector2, Tile> DeleteBounds(Dictionary<Vector2, Tile> field)
    {
        var result = new Dictionary<Vector2, Tile>();
        foreach (var cell in field)
        {
            if (cell.Key.X == 0 || cell.Key.Y == 0 ||
                cell.Key.X == Size.X - 1 || cell.Key.Y == Size.Y - 1)
                continue;
            result.Add(new Vector2(cell.Key.X - 1, cell.Key.Y - 1), cell.Value);
        }
        return result;
    }

    private Dictionary<Vector2, Tile> XEvenMirrorResult(Dictionary<Vector2, Tile> field)
    {
        var result = new Dictionary<Vector2, Tile>();
        foreach (var cell in field)
        {
            result.Add(new Vector2(cell.Key.X, cell.Key.Y), cell.Value);
            var flippedTile = cell.Value.Copy();
            flippedTile.FlipX();
            result.Add(cell.Key with { X = 2 * Size.X - cell.Key.X - 1 }, flippedTile);
        }
        return result;
    }
    
    private Dictionary<Vector2, Tile> YOddMirrorResult(Dictionary<Vector2, Tile> field)
    {
        var result = new Dictionary<Vector2, Tile>();
        foreach (var cell in field)
        {
            if (cell.Key.Y == Size.Y - 1)
            {
                result.Add(new Vector2(cell.Key.X, cell.Key.Y), cell.Value);
                continue;
            }
            result.Add(new Vector2(cell.Key.X, cell.Key.Y), cell.Value);
            var flippedTile = cell.Value.Copy();
            flippedTile.FlipY();
            result.Add(cell.Key with { Y = 2 * (Size.Y - 1) - cell.Key.Y }, flippedTile);
        }
        return result;
    }
    
    private Dictionary<Vector2, Tile> YEvenMirrorResult(Dictionary<Vector2, Tile> field)
    {
        var result = new Dictionary<Vector2, Tile>();
        foreach (var cell in field)
        {
            result.Add(new Vector2(cell.Key.X, cell.Key.Y), cell.Value);
            var flippedTile = cell.Value.Copy();
            flippedTile.FlipY();
            result.Add(cell.Key with { Y = 2 * Size.Y - cell.Key.Y - 1 }, flippedTile);
        }
        return result;
    }
    
    private Dictionary<Vector2, Tile> XOddMirrorResult(Dictionary<Vector2, Tile> field)
    {
        var result = new Dictionary<Vector2, Tile>();
        foreach (var cell in field)
        {
            if (cell.Key.X == Size.X - 1)
            {
                result.Add(new Vector2(cell.Key.X, cell.Key.Y), cell.Value);
                continue;
            }
            result.Add(new Vector2(cell.Key.X, cell.Key.Y), cell.Value);
            var flippedTile = cell.Value.Copy();
            flippedTile.FlipX();
            result.Add(cell.Key with { X = 2 * (Size.X - 1) - cell.Key.X }, flippedTile);
        }
        return result;
    }

    private Dictionary<Vector2, Tile> GetFieldDictionaryVectorTile()
    {
        return Field.ToDictionary(x => x.Key, x => x.Value[0]);
    }

    public Map2 Copy()
    {
        var copy = new Map2
        {
            Size = Size,
            XSymmetry = XSymmetry,
            YSymmetry = YSymmetry,
            XEven = XEven,
            YEven = YEven,
            _tileManager = _tileManager,
            Field = new Dictionary<Vector2, Tile[]>()
        };
        foreach (var cell in Field)
        {
            copy.Field[cell.Key] = (Tile[])Field[cell.Key].Clone();
        }
        return copy;
    }
}