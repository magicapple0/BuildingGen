namespace BuildingGen;


public class State
{
    public Map Map { get; private init; }
    private List<Vector3> VisitedCells => Map.Field.Where(x => x.Value.Length == 1).Select(x => x.Key).ToList();
    public Queue<(Vector3, Tile)>? PossibleMoves;
    public HashSet<Vector3> Neighbors = new ();

    public State(Vector3 size, TileManager tileManager, bool xSymmetry, bool ySymmetry)
    {
        Map = new Map(size, tileManager, xSymmetry, ySymmetry);
    }

    private State() { }
    
    public void CalculateMoves(Random random)
    {
        if (!Neighbors.Any(x => Map.Field[x].Length > 1))
        {
            //PossibleMoves = new Queue<(Vector3, Tile)>();
            var neighbor1 = Map.Field.Keys.Where(x => Map.Field[x].Length > 1).MinBy(x => Map.Field[x].Length);
            PossibleMoves = new Queue<(Vector3, Tile)>(Map.Field[neighbor1].OrderBy(_ => random.Next()).Select(x => (neighbor1, x)));
            return;
        }
        var neighbor = Neighbors.Where(x => Map.Field[x].Length > 1).MinBy(x => Map.Field[x].Length);
        PossibleMoves = new Queue<(Vector3, Tile)>(Map.Field[neighbor].OrderBy(_ => random.Next()).Select(x => (neighbor, x)));
    }

    public void Wave()
    {
        var queue = new Queue<Vector3>(VisitedCells.ToArray());
        var visited = new HashSet<Vector3>(VisitedCells.ToArray());
        
        while (queue.Count != 0)
        {
            queue = new Queue<Vector3>(queue.OrderBy(x => Map.Field[x].Length));
            var currCell = queue.Dequeue();
            foreach (var neighbor in GetNotVisitedNeighbors(currCell, visited))
            {
                UpdateCellTiles(currCell, neighbor.Item1, neighbor.Item2);
                if (!queue.Contains(neighbor.Item1))
                    queue.Enqueue(neighbor.Item1);
            }
            visited.Add(currCell);
            
            //Console.WriteLine(queue.Count);
        }
    }

    private List<(Vector3, Directions)> GetNotVisitedNeighbors(Vector3 currCell, HashSet<Vector3> visited)
    {
        var neighbors = new List<(Vector3, Directions)>();
        foreach (var direction in DirectionConstants.DirectionsVectors)
        {
            var newCell = (currCell.X + direction.Value.X,
                currCell.Y + direction.Value.Y, currCell.Z + direction.Value.Z);
            if (IsCellInBounds(newCell) && !visited.Contains(newCell))
                neighbors.Add((newCell, direction.Key));
        }
        return neighbors;
    }
    
    public void SetTile(Vector3 cell, Tile tile)
    {
        Map.Field[cell] = new[] { tile };
        foreach (var neighbor in GetNotVisitedNeighbors(cell, Neighbors))
            Neighbors.Add(neighbor.Item1);
        Neighbors.Remove(cell);
    }

    private static Dictionary<string, (Tile[] newNeighborTiles, Tile[] newCurrCellTiles)> UpdateCellCache = new();

    private void UpdateCellTiles(Vector3 currCell, Vector3 changingCell, Directions direction)
    {
        var cacheKey =
            $"{direction}|{string.Join(';', Map.Field[currCell].Select(x => x.StringRep))}|{string.Join(';', Map.Field[changingCell].Select(x => x.StringRep))}";
        if (UpdateCellCache.TryGetValue(cacheKey, out var value))
        {
            var (newNeighborT, newCurrCellT) = value;
            Map.Field[changingCell] = (Tile[])newNeighborT.Clone();
            Map.Field[currCell] = (Tile[])newCurrCellT.Clone();
            return;
        }
        var neighborCellDirection = DirectionConstants.CellOppositeSides[direction].Item1;
        var currCellDirection = DirectionConstants.CellOppositeSides[direction].Item2;
        var newNeighborTiles = new List<Tile>();
        var newCurrCellTiles = new List<Tile>();
        //если текущая клетка может соседствовать с выбранным соседом (в этом направлении) и наоборот
        foreach (var currCellTile in Map.Field[currCell])
            foreach (var oldNeighborTile in Map.Field[changingCell])
                if (currCellTile.ModifiedEdges[currCellDirection].Contains(oldNeighborTile.TileInfo.Name) &&
                    oldNeighborTile.ModifiedEdges[neighborCellDirection].Contains(currCellTile.TileInfo.Name))
                {
                    if (!newNeighborTiles.Contains(oldNeighborTile))
                        newNeighborTiles.Add(oldNeighborTile);
                    if (!newCurrCellTiles.Contains(currCellTile))
                        newCurrCellTiles.Add(currCellTile);
                }
        Map.Field[changingCell] = newNeighborTiles.ToArray();
        Map.Field[currCell] = newCurrCellTiles.ToArray();
        UpdateCellCache[cacheKey] = (Map.Field[changingCell], Map.Field[currCell]);
    }
    
    public bool IsCollapse()
    {
        return Map.Field.Where(x => x.Value.Length != 1).Select(x => x.Key).ToList().Count == 0;
    }
    
    public bool IsBroken()
    {
        return Map.Field.Where(x => x.Value.Length == 0).Select(x => x.Key).ToList().Count != 0;
    }
    
    private bool IsCellInBounds(Vector3 cell)
    {
        return !(cell.X < 0 || cell.X >= Map.Size.X || cell.Y < 0 || cell.Y >= Map.Size.Y ||
                 cell.Z < 0 || cell.Z >= Map.Size.Z);
    }
    
    public State Copy()
    {
        var copy = new State
        {
            Map = Map.Copy(),
            Neighbors = new HashSet<Vector3>(Neighbors)
        };
        return copy;
    }
}