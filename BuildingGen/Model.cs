namespace BuildingGen;



public class Model
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
    
    private int Width { get; init; }
    private int Height { get; init; }
    private int Depth { get; init; }
    private Tile[] TileSet { get; init; }
    public Dictionary<Vector3, Tile[]> Field { get; private init; }
    public List<Vector3> VisitedCells => Field.Where(x => x.Value.Length == 1).Select(x => x.Key).ToList();

    public Queue<(Vector3, Tile)>? PossibleMoves;
    public HashSet<Vector3> Neighbors = new ();

    public Model(int width, int depth, int height, Tile[] tileSet)
    {
        Width = width;
        Depth = depth;
        Height = height;
        TileSet = tileSet;
        Field = new Dictionary<Vector3, Tile[]>();
        var ground = TileSet[^2];
        var bound = TileSet[^1];
        TileSet = (Tile[])TileSet.Clone();
        var newTileSet = new List<Tile>(TileSet);
        newTileSet.RemoveAt(newTileSet.Count - 2);
        newTileSet.RemoveAt(newTileSet.Count - 1);
        TileSet = newTileSet.ToArray();
        for (var i = 0; i < Width; i++)
        {
            for (var j = 0; j < Depth; j++)
            {
                for (var k = 0; k < Height; k++)
                {
                    if (k == 0)
                    {
                        Field[(i, j, k)] = new[] { ground };
                        VisitedCells.Add((i, j, k));
                        continue;
                    }

                    if (i == 0 || i == Width - 1 || j == 0 || j == Depth - 1 || k == Height - 1)
                    {
                        Field[(i, j, k)] = new[] { bound };
                        VisitedCells.Add((i, j, k));
                        continue;
                    }

                    Field[(i, j, k)] = TileSet.ToArray();
                }
            }
        }
    }

    private Model() { }
    
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
        return !(cell.Item1 < 0 || cell.Item1 >= Width || cell.Item2 < 0 || cell.Item2 >= Depth ||
                cell.Item3 < 0 || cell.Item3 >= Height);
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
    
    public Tile[,,] Result()
    {
        var building = new Tile[Width - 2, Depth - 2, Height - 2];
        for (int i = 0; i < Width - 1; i++)
            for (int j = 0; j < Depth - 1; j++)
            for (int k = 0; k < Height - 1; k++)
            {
                if (Field[(i, j, k)].Length != 1)
                {
                    building[i - 1, j - 1, k - 1] = TileSet[^1];
                    continue;
                }
                if (i == 0 || j == 0 || k == 0)
                    continue;
                building[i - 1, j - 1, k - 1] = Field[(i, j, k)][0];
            }
        
        return building;
    }
    
    public bool IsCollapse()
    {
        return Field.Where(x => x.Value.Length != 1).Select(x => x.Key).ToList().Count == 0;
    }
    
    public bool IsBroken()
    {
        return Field.Where(x => x.Value.Length == 0).Select(x => x.Key).ToList().Count != 0;
    }

    public Model Copy()
    {
        var copy = new Model
        {
            Depth = Depth,
            Width = Width,
            Height = Height,
            TileSet = (Tile[])TileSet.Clone(),
            Field = new Dictionary<Vector3, Tile[]>(),
            Neighbors = new HashSet<Vector3>(Neighbors)
        };
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                for (int k = 0; k < Height; k++)
                {
                    copy.Field[(i, j, k)] = (Tile[])Field[(i, j, k)].Clone();
                }
            }
        }

        return copy;
    }
}