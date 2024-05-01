namespace BuildingGen;

public class Model
{
    private readonly Dictionary<(int, int, int), string> _directions = new()
    {
        { (0, 1, 0), "forward" },
        { (0, -1, 0), "backward" },
        { (1, 0, 0), "right" },
        { (-1, 0, 0), "left" },
        { (0, 0, 1), "up" },
        { (0, 0, -1), "down" },
    };
    private readonly Dictionary<string, (int, int)> _neighborCellDirections = new()
    {
        { "forward", (1, 5) },
        { "backward", (5, 1) },
        { "right", (3, 4) },
        { "left", (4, 3) },
        { "up", (2, 0) },
        { "down", (0, 2) },
    };
    
    private int Width { get; set; }
    private int Height { get; set; }
    private int Depth { get; set; }
    private Tile[] TileSet { get; set; }
    public Dictionary<(int, int, int), Tile[]> Field { get; set; }
    private Dictionary<(int, int, int), List<Tile>> VisitedTiles { get; set; }
    private List<(int, int, int)> VisitedCells => Field.Where(x => x.Value.Length == 1).Select(x => x.Key).ToList();

    public Queue<((int, int, int), Tile)>? PossibleMoves;

    public Model(int width, int depth, int height, Tile[] tileSet)
    {
        Width = width;
        Depth = depth;
        Height = height;
        TileSet = tileSet;
        Field = new Dictionary<(int, int, int), Tile[]>();
        VisitedTiles = new Dictionary<(int, int, int), List<Tile>>();
        var ground = TileSet[^2];
        var air = TileSet[^1];
        TileSet = (Tile[])TileSet.Clone();
        var newTileSet = new List<Tile>(TileSet);
        newTileSet.RemoveAt(newTileSet.Count - 2);
        TileSet = newTileSet.ToArray();
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                for (int k = 0; k < Height; k++)
                {
                    VisitedTiles.Add((i, j, k), new List<Tile>());
                    if (k == 0)
                    {
                        Field[(i, j, k)] = new[] { ground };
                        VisitedCells.Add((i, j, k));
                        VisitedTiles[(i, j, k)].Add(ground);
                        continue;
                    }

                    if (i == 0 || i == Width - 1 || j == 0 || j == Depth - 1 || k == Height - 1)
                    {
                        Field[(i, j, k)] = new[] { air };
                        VisitedCells.Add((i, j, k));
                        VisitedTiles[(i, j, k)].Add(air);
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
        var minTiles = Field.Where(x => x.Value.Length > 1).MinBy(x => x.Value.Length).Value.Length;
        PossibleMoves = new Queue<((int, int, int), Tile)>(
            Field.Where(x => x.Value.Length == minTiles)
                .SelectMany(x => x.Value.Select(tile => (x.Key, tile)))
                .OrderBy(x => random.Next()));
    }

    public void Wave()
    {
        //bfs
        var queue = new Queue<(int, int, int)>(VisitedCells.ToArray());
        var visited = new HashSet<(int, int, int)>(VisitedCells.ToArray());

        while (queue.Count != 0)
        {
            var currCell = queue.Dequeue();
            foreach (var direction in _directions)
            {
                var newCell = (currCell.Item1 + direction.Key.Item1,
                    currCell.Item2 + direction.Key.Item2, currCell.Item3 + direction.Key.Item3);
                if (IsAble(currCell, direction.Key) && !visited.Contains(newCell))
                {
                    if (newCell == (1, 1, 1))
                        newCell = (1, 1, 1);
                    UpdateCellTiles(currCell, newCell, direction.Value);
                    queue.Enqueue(newCell);
                }
            }
            visited.Add(currCell);
        }
    }

    private bool IsAble((int, int, int) currCell, (int, int, int) offset)
    {
        var newCell = (currCell.Item1 + offset.Item1, currCell.Item2 + offset.Item2, currCell.Item3 + offset.Item3);
        return !(newCell.Item1 < 0 || newCell.Item1 >= Width || newCell.Item2 < 0 || newCell.Item2 >= Depth ||
                newCell.Item3 < 0 || newCell.Item3 >= Height);
    }

    private void UpdateCellTiles((int, int, int) currCell, (int, int, int) changingTile, string direction)
    {
        var neighborCellDirection = _neighborCellDirections[direction].Item1;
        var currCellDirection = _neighborCellDirections[direction].Item2;

        var newNeighborTiles = new List<Tile>();
        var oldNeighborTiles = Field[changingTile];
        var currCellTiles = Field[currCell];
        foreach (var currCellTile in currCellTiles)
        {
            var currCellNeighborsTilesNames = currCellTile.ModifiedEdges[currCellDirection];
            foreach (var tile in oldNeighborTiles)
            {
                //если текущая клетка может соседствовать с выбранным соседом (в этом направлении)
                if (currCellNeighborsTilesNames.Contains(tile.TileInfo.Name))
                    //если соседняя клетка может соседствовать с текущей (в этом направлении)
                    if (!newNeighborTiles.Contains(tile) &&
                        tile.ModifiedEdges[neighborCellDirection].Contains(currCellTile.TileInfo.Name))
                        newNeighborTiles.Add(tile);
            }
        }

        Field[changingTile] = newNeighborTiles.ToArray();
    }
    
    public Tile[,,] Result()
    {
        var building = new Tile[Width, Depth, Height];
        for (int i = 0; i < Width; i++)
        for (int j = 0; j < Depth; j++)
        for (int k = 0; k < Height; k++)
            building[i, j, k] = Field[(i, j, k)][0];
        return building;
    }
    
    public bool IsCollapse()
    {
        foreach (var tile in Field)
        {
            if (tile.Value.Length != 1)
                return false;
        }
        return true;
    }
    
    public bool IsBroken()
    {
        foreach (var tile in Field)
        {
            if (tile.Value.Length == 0)
                return true;
        }
        return false;
    }

    public Model Copy()
    {
        var copy = new Model
        {
            Depth = Depth,
            Width = Width,
            Height = Height,
            TileSet = (Tile[])TileSet.Clone(),
            Field = new Dictionary<(int, int, int), Tile[]>(),
            VisitedTiles = new Dictionary<(int, int, int), List<Tile>>()
        };
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                for (int k = 0; k < Height; k++)
                {
                    copy.Field[(i, j, k)] = (Tile[])Field[(i, j, k)].Clone();
                    copy.VisitedTiles[(i, j, k)] = new List<Tile>(VisitedTiles[(i, j, k)]);
                }
            }
        }

        return copy;
    }
}