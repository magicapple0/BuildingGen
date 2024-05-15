namespace BuildingGen;

public class Map
{
    private TileManager _tileManager;
    public Vector3 Size { get; private set; }
    private bool XSymmetry { get; init; }
    private bool YSymmetry { get; init; }
    private bool XEven { get; init; }
    private bool YEven { get; init; }
    public Dictionary<Vector3, Tile[]> Field { get; private init; }

    private Map() { }

    public Map(Vector3 size, TileInfo[] tilesInfos, bool xSymmetry, bool ySymmetry)
    {
        _tileManager = new TileManager(tilesInfos);
        XSymmetry = xSymmetry;
        YSymmetry = ySymmetry;
        XEven = size.X % 2 == 0;
        YEven = size.Y % 2 == 0;
        Size = CalculateFieldSize(size);

        Field = new Dictionary<Vector3, Tile[]>();
        InitializeSimpleField();
        InitializeFieldBounds(_tileManager.Bound, _tileManager.Ground);
    }

    private void InitializeFieldBounds(Tile bound, Tile ground)
    {
        var bounds = new Dictionary<Directions, List<Vector3>>
        {
            { Directions.Face, Field.Where(x => x.Key.Y == 0).Select(x => x.Key).ToList() },
            { Directions.Back, Field.Where(x => x.Key.Y == Size.Y - 1).Select(x => x.Key).ToList() },
            { Directions.Right, Field.Where(x => x.Key.X == Size.X - 1).Select(x => x.Key).ToList() },
            { Directions.Left, Field.Where(x => x.Key.X == 0).Select(x => x.Key).ToList() },
            { Directions.Up, Field.Where(x => x.Key.Z == Size.Z - 1).Select(x => x.Key).ToList() },
            { Directions.Down, Field.Where(x => x.Key.Z == 0).Select(x => x.Key).ToList() },
        };
        var boundTileSet = new List<Tile>(new[] { bound });
        var groundTileSet = new List<Tile>(new[] { ground });
        if (!XSymmetry && !YSymmetry)
        {
            foreach (var direction in DirectionConstants.DirectionsVectors)
                SetBoundTiles(boundTileSet, bounds[direction.Key]);
            SetBoundTiles(groundTileSet, bounds[Directions.Down]);
        }
        if (XSymmetry && !YSymmetry)
        {
            var xSymmetryTiles = XEven ? _tileManager.GetEvenSymmetryTiles(Directions.Right) : 
                _tileManager.GetOddSymmetryTiles(Directions.Right);
            SetBoundTiles(xSymmetryTiles, bounds[Directions.Right]);
            SetBoundTiles(groundTileSet, bounds[Directions.Down]);
            SetBoundTiles(boundTileSet, bounds[Directions.Up]);
            SetBoundTiles(boundTileSet, bounds[Directions.Left]);
            SetBoundTiles(boundTileSet, bounds[Directions.Face]);
            SetBoundTiles(boundTileSet, bounds[Directions.Back]);
        }
        if (YSymmetry && !XSymmetry)
        {
            var ySymmetryTiles = YEven ? _tileManager.GetEvenSymmetryTiles(Directions.Back) : 
                _tileManager.GetOddSymmetryTiles(Directions.Back);
            SetBoundTiles(ySymmetryTiles, bounds[Directions.Back]);
            SetBoundTiles(groundTileSet, bounds[Directions.Down]);
            SetBoundTiles(boundTileSet, bounds[Directions.Up]);
            SetBoundTiles(boundTileSet, bounds[Directions.Left]);
            SetBoundTiles(boundTileSet, bounds[Directions.Face]);
            SetBoundTiles(boundTileSet, bounds[Directions.Right]);
        }
        if (YSymmetry && XSymmetry)
        {
            var xSymmetryTiles = XEven ? _tileManager.GetEvenSymmetryTiles(Directions.Right) : 
                _tileManager.GetOddSymmetryTiles(Directions.Right);
            var ySymmetryTiles = YEven ? _tileManager.GetEvenSymmetryTiles(Directions.Back) : 
                _tileManager.GetOddSymmetryTiles(Directions.Back);
            SetBoundTiles(ySymmetryTiles, bounds[Directions.Back]);
            SetBoundTiles(xSymmetryTiles, bounds[Directions.Right]);
            SetBoundTiles(groundTileSet, bounds[Directions.Down]);
            SetBoundTiles(boundTileSet, bounds[Directions.Up]);
            SetBoundTiles(boundTileSet, bounds[Directions.Left]);
            SetBoundTiles(boundTileSet, bounds[Directions.Face]);
        }
    }
    
    private void InitializeSimpleField()
    {
        for (var i = 0; i < Size.X; i++)
        {
            for (var j = 0; j < Size.Y; j++)
            {
                for (var k = 0; k < Size.Z; k++)
                {
                    Field[(i, j, k)] = _tileManager.TileSet.ToArray();
                }
            }
        }
    }

    private void SetBoundTiles(List<Tile> boundTiles, List<Vector3> boundCells)
    {
        foreach (var cell in boundCells)
            Field[cell] = boundTiles.ToArray();
    }

    private Vector3 CalculateFieldSize(Vector3 size)
    {
        return new Vector3(0, 0, size.Z + 2)
        {
            X = XSymmetry ? (int)Math.Ceiling(size.X / (decimal)2) + 1 : size.X + 2,
            Y = YSymmetry ? (int)Math.Ceiling(size.Y / (decimal)2) + 1 : size.Y + 2
        };
    }
    
    public Dictionary<Vector3, Tile> Result()
    {
        var field = GetFieldDictionaryVectorTile();
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

    private Dictionary<Vector3, Tile> DeleteBounds(Dictionary<Vector3, Tile> field)
    {
        var result = new Dictionary<Vector3, Tile>();
        foreach (var cell in field)
        {
            if (cell.Key.X == 0 || cell.Key.Y == 0 || cell.Key.Z == 0 || 
                cell.Key.X == Size.X - 1 || cell.Key.Y == Size.Y - 1 || cell.Key.Z == Size.Z - 1)
                continue;
            result.Add(new Vector3(cell.Key.X - 1, cell.Key.Y - 1, cell.Key.Z - 1), cell.Value);
        }
        return result;
    }

    private Dictionary<Vector3, Tile> XEvenMirrorResult(Dictionary<Vector3, Tile> field)
    {
        var result = new Dictionary<Vector3, Tile>();
        foreach (var cell in field)
        {
            result.Add(new Vector3(cell.Key.X, cell.Key.Y, cell.Key.Z), cell.Value);
            var flippedTile = cell.Value.Copy();
            flippedTile.FlipX();
            result.Add(cell.Key with { X = 2 * Size.X - cell.Key.X - 1 }, flippedTile);
        }
        return result;
    }
    
    private Dictionary<Vector3, Tile> YOddMirrorResult(Dictionary<Vector3, Tile> field)
    {
        var result = new Dictionary<Vector3, Tile>();
        foreach (var cell in field)
        {
            if (cell.Key.Y == Size.Y - 1)
            {
                result.Add(new Vector3(cell.Key.X, cell.Key.Y, cell.Key.Z), cell.Value);
                continue;
            }
            result.Add(new Vector3(cell.Key.X, cell.Key.Y, cell.Key.Z), cell.Value);
            var flippedTile = cell.Value.Copy();
            flippedTile.FlipY();
            result.Add(cell.Key with { Y = 2 * (Size.Y - 1) - cell.Key.Y }, flippedTile);
        }
        return result;
    }
    
    private Dictionary<Vector3, Tile> YEvenMirrorResult(Dictionary<Vector3, Tile> field)
    {
        var result = new Dictionary<Vector3, Tile>();
        foreach (var cell in field)
        {
            result.Add(new Vector3(cell.Key.X, cell.Key.Y, cell.Key.Z), cell.Value);
            var flippedTile = cell.Value.Copy();
            flippedTile.FlipY();
            result.Add(cell.Key with { Y = 2 * Size.Y - cell.Key.Y - 1 }, flippedTile);
        }
        return result;
    }
    
    private Dictionary<Vector3, Tile> XOddMirrorResult(Dictionary<Vector3, Tile> field)
    {
        var result = new Dictionary<Vector3, Tile>();
        foreach (var cell in field)
        {
            if (cell.Key.X == Size.X - 1)
            {
                result.Add(new Vector3(cell.Key.X, cell.Key.Y, cell.Key.Z), cell.Value);
                continue;
            }
            result.Add(new Vector3(cell.Key.X, cell.Key.Y, cell.Key.Z), cell.Value);
            var flippedTile = cell.Value.Copy();
            flippedTile.FlipX();
            result.Add(cell.Key with { X = 2 * (Size.X - 1) - cell.Key.X }, flippedTile);
        }
        return result;
    }

    private Dictionary<Vector3, Tile> GetFieldDictionaryVectorTile()
    {
        return Field.ToDictionary(x => x.Key, x => x.Value[0]);
    }

    public Map Copy()
    {
        var copy = new Map
        {
            Size = Size,
            XSymmetry = XSymmetry,
            YSymmetry = YSymmetry,
            XEven = XEven,
            YEven = YEven,
            _tileManager = _tileManager,
            Field = new Dictionary<Vector3, Tile[]>()
        };
        foreach (var cell in Field)
        {
            copy.Field[cell.Key] = (Tile[])Field[cell.Key].Clone();
        }
        return copy;
    }
}