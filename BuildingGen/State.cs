namespace BuildingGen;


public class State
{
    private readonly Dictionary<string, Vector3> _directions = new()
    {
        { "forward", (0, 1, 0) },
        { "backward", (0, -1, 0) },
        { "right", (1, 0, 0) },
        { "left", (-1, 0, 0) },
        { "up", (0, 0, 1) },
        { "down", (0, 0, -1) },
    };
    private readonly Dictionary<string, (int, int)> _neighborCellDirections = new()
    {
        { "forward", (5, 1) },
        { "backward", (1, 5) },
        { "right", (3, 4) },
        { "left", (4, 3) },
        { "up", (2, 0) },
        { "down", (0, 2) },
    };
    private Vector3 Size { get; set; }
    private bool XSymmetry { get; init; }
    private bool YSymmetry { get; init; }
    private bool XEven { get; init; }
    private bool YEven { get; init; }
    private Tile[] TileSet { get; init; }
    private Dictionary<Vector3, Tile[]> Field { get; init; }
    private Dictionary<string, List<Vector3>> _bounds;
    private List<Vector3> VisitedCells => Field.Where(x => x.Value.Length == 1).Select(x => x.Key).ToList();

    public Queue<(Vector3, Tile)>? PossibleMoves;
    public HashSet<Vector3> Neighbors = new ();

    public State(Vector3 size, Tile[] tileSet, bool xSymmetry, bool ySymmetry)
    {
        XSymmetry = xSymmetry;
        YSymmetry = ySymmetry;
        XEven = size.X % 2 == 0;
        YEven = size.Y % 2 == 0;
        Size = CalculateFieldSize(size);

        Field = new Dictionary<Vector3, Tile[]>();
        TileSet = (Tile[])tileSet.Clone();
        var newTileSet = new List<Tile>(TileSet);
        var ground = TileSet[^2];
        var bound = TileSet[^1];
        newTileSet.RemoveAt(newTileSet.Count - 2);
        newTileSet.RemoveAt(newTileSet.Count - 1);
        TileSet = newTileSet.ToArray();
        InitializeField();
        _bounds = new()
        {
            { "forward", Field.Where(x => x.Key.Y == 0).Select(x => x.Key).ToList() },
            { "backward", Field.Where(x => x.Key.Y == Size.Y - 1).Select(x => x.Key).ToList() },
            { "right", Field.Where(x => x.Key.X == Size.X - 1).Select(x => x.Key).ToList() },
            { "left", Field.Where(x => x.Key.X == 0).Select(x => x.Key).ToList() },
            { "up", Field.Where(x => x.Key.Z == Size.Z - 1).Select(x => x.Key).ToList() },
            { "down", Field.Where(x => x.Key.Z == 0).Select(x => x.Key).ToList() },
        };
        var boundTileSet = new List<Tile>(new[] { bound });
        var groundTileSet = new List<Tile>(new[] { ground });
        if (!xSymmetry && !ySymmetry)
        {
            foreach (var direction in _directions)
                SetBoundTiles(boundTileSet, direction.Key);
            SetBoundTiles(groundTileSet, "down");
        }
        if (xSymmetry && !ySymmetry)
        {
            var xSymmetryTiles = XEven ? GetEvenSymmetryTiles("right") : GetOddSymmetryTiles("right");
            SetBoundTiles(xSymmetryTiles, "right");
            SetBoundTiles(groundTileSet, "down");
            SetBoundTiles(boundTileSet, "up");
            SetBoundTiles(boundTileSet, "left");
            SetBoundTiles(boundTileSet, "forward");
            SetBoundTiles(boundTileSet, "backward");
        }
        if (ySymmetry && !xSymmetry)
        {
            var ySymmetryTiles = YEven ? GetEvenSymmetryTiles("backward") : GetOddSymmetryTiles("backward");
            SetBoundTiles(ySymmetryTiles, "backward");
            SetBoundTiles(groundTileSet, "down");
            SetBoundTiles(boundTileSet, "up");
            SetBoundTiles(boundTileSet, "left");
            SetBoundTiles(boundTileSet, "forward");
            SetBoundTiles(boundTileSet, "right");
        }
        if (ySymmetry && xSymmetry)
        {
            var xSymmetryTiles = XEven ? GetEvenSymmetryTiles("right") : GetOddSymmetryTiles("right");
            var ySymmetryTiles = YEven ? GetEvenSymmetryTiles("backward") : GetOddSymmetryTiles("backward");
            SetBoundTiles(ySymmetryTiles, "backward");
            SetBoundTiles(xSymmetryTiles, "right");
            SetBoundTiles(groundTileSet, "down");
            SetBoundTiles(boundTileSet, "up");
            SetBoundTiles(boundTileSet, "left");
            SetBoundTiles(boundTileSet, "forward");
        }
    }

    private void SetBoundTiles(List<Tile> boundTiles, string direction)
    {
        var boundCells = _bounds[direction];
        foreach (var cell in Field.Where(cell => boundCells.Contains(cell.Key)))
            Field[cell.Key] = boundTiles.ToArray();
    }

    private Vector3 CalculateFieldSize(Vector3 size)
    {
        var newSize = new Vector3(0, 0, size.Z + 2);
        newSize.X = XSymmetry ? (int)Math.Ceiling(size.X / (decimal)2) + 1 : size.X + 2;
        newSize.Y = YSymmetry ? (int)Math.Ceiling(size.Y / (decimal)2) + 1 : size.Y + 2;
        return newSize;
    }

    private List<Tile> GetOddSymmetryTiles(string direction)
    {
        var symmetryTiles = new List<Tile>();
        var directions = _neighborCellDirections[direction];
        foreach (var tile in TileSet)
        {
            var symmetryTile = tile.Copy();
            var modifiedEdges = new List<string>();
            foreach (var neighbor in tile.ModifiedEdges[directions.Item1])
            {
                if (tile.ModifiedEdges[directions.Item2].Contains(neighbor))
                {
                    modifiedEdges.Add(neighbor);
                }
            }
            if (modifiedEdges.Count == 0)
                continue;
            symmetryTile.ModifiedEdges[directions.Item1] = modifiedEdges.ToArray();
            symmetryTile.ModifiedEdges[directions.Item2] = modifiedEdges.ToArray();
            symmetryTiles.Add(symmetryTile);
        }
        return symmetryTiles;
    }

    private List<Tile> GetEvenSymmetryTiles(string direction)
    {
        var symmetryTiles = new List<Tile>();
        var directions = _neighborCellDirections[direction];
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

    private void InitializeField()
    {
        for (var i = 0; i < Size.X; i++)
        {
            for (var j = 0; j < Size.Y; j++)
            {
                for (var k = 0; k < Size.Z; k++)
                {
                    Field[(i, j, k)] = TileSet.ToArray();
                }
            }
        }
    }

    private State() { }
    
    public void CalculateMoves(Random random)
    {
        if (!Neighbors.Any(x => Field[x].Length > 1))
        {
            PossibleMoves = new Queue<(Vector3, Tile)>();
            return;
        }
        var neighbor = Neighbors.Where(x => Field[x].Length > 1).MinBy(x => Field[x].Length);
        PossibleMoves = new Queue<(Vector3, Tile)>(Field[neighbor].OrderBy(_ => random.Next()).Select(x => (neighbor, x)));
    }

    public void Wave()
    {
        //bfs
        var queue = new Queue<Vector3>(VisitedCells.ToArray());
        var visited = new HashSet<Vector3>(VisitedCells.ToArray());

        while (queue.Count != 0)
        {
            var currCell = queue.Dequeue();
            foreach (var neighbor in GetNotVisitedNeighbors(currCell, visited))
            {
                UpdateCellTiles(currCell, neighbor.Item1, neighbor.Item2);
                queue.Enqueue(neighbor.Item1);
            }
            visited.Add(currCell);
        }
    }

    private List<(Vector3, string)> GetNotVisitedNeighbors(Vector3 currCell, HashSet<Vector3> visited)
    {
        var neighbors = new List<(Vector3, string)>();
        foreach (var direction in _directions)
        {
            var newCell = (currCell.X + direction.Value.X,
                currCell.Y + direction.Value.Y, currCell.Z + direction.Value.Z);
            if (IsAble(newCell) && !visited.Contains(newCell))
                neighbors.Add((newCell, direction.Key));
        }
        return neighbors;
    }
    
    public void SetTile(Vector3 cell, Tile tile)
    {
        Field[cell] = new[] { tile };
        foreach (var neighbor in GetNotVisitedNeighbors(cell, Neighbors))
            Neighbors.Add(neighbor.Item1);
        Neighbors.Remove(cell);
    }
    
    private bool IsAble((int, int, int) cell)
    {
        return !(cell.Item1 < 0 || cell.Item1 >= Size.X || cell.Item2 < 0 || cell.Item2 >= Size.Y ||
                cell.Item3 < 0 || cell.Item3 >= Size.Z);
    }

    private void UpdateCellTiles((int, int, int) currCell, (int, int, int) changingCell, string direction)
    {
        var neighborCellDirection = _neighborCellDirections[direction].Item1;
        var currCellDirection = _neighborCellDirections[direction].Item2;

        var newNeighborTiles = new List<Tile>();
        var newCurrCellTiles = new List<Tile>();
        //если текущая клетка может соседствовать с выбранным соседом (в этом направлении) и наоборот
        foreach (var currCellTile in Field[currCell])
            foreach (var oldNeighborTile in Field[changingCell])
                if (currCellTile.ModifiedEdges[currCellDirection].Contains(oldNeighborTile.TileInfo.Name) &&
                    oldNeighborTile.ModifiedEdges[neighborCellDirection].Contains(currCellTile.TileInfo.Name))
                {
                    if (!newNeighborTiles.Contains(oldNeighborTile))
                        newNeighborTiles.Add(oldNeighborTile);
                    if (!newCurrCellTiles.Contains(currCellTile))
                        newCurrCellTiles.Add(currCellTile);
                }
        Field[changingCell] = newNeighborTiles.ToArray();
        Field[currCell] = newCurrCellTiles.ToArray();
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
            result.Add(new Vector3(2 * Size.X - cell.Key.X - 1, cell.Key.Y, cell.Key.Z), flippedTile);
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
            result.Add(new Vector3(cell.Key.X, 2 * (Size.Y - 1) - cell.Key.Y, cell.Key.Z), flippedTile);
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
            result.Add(new Vector3(cell.Key.X, 2 * Size.Y - cell.Key.Y - 1, cell.Key.Z), flippedTile);
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
            result.Add(new Vector3(2 * (Size.X - 1) - cell.Key.X, cell.Key.Y, cell.Key.Z), flippedTile);
        }
        return result;
    }

    private Dictionary<Vector3, Tile> GetFieldDictionaryVectorTile()
    {
        return Field.ToDictionary(x => x.Key, x => x.Value[0]);
    }
    
    public bool IsCollapse()
    {
        return Field.Where(x => x.Value.Length != 1).Select(x => x.Key).ToList().Count == 0;
    }
    
    public bool IsBroken()
    {
        return Field.Where(x => x.Value.Length == 0).Select(x => x.Key).ToList().Count != 0;
    }

    public State Copy()
    {
        var copy = new State
        {
            Size = Size,
            XSymmetry = XSymmetry,
            YSymmetry = YSymmetry,
            XEven = XEven,
            YEven = YEven,
            TileSet = (Tile[])TileSet.Clone(),
            Field = new Dictionary<Vector3, Tile[]>(),
            Neighbors = new HashSet<Vector3>(Neighbors)
        };
        foreach (var cell in Field)
        {
            copy.Field[cell.Key] = (Tile[])Field[cell.Key].Clone();
        }
        return copy;
    }
}