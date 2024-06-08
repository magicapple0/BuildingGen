namespace BuildingGen;


public class State2
{
    public Map2 Map { get; private init; }
    private List<Vector2> VisitedCells => Map.Field.Where(x => x.Value.Length == 1).Select(x => x.Key).ToList();
    public Queue<(Vector2, Tile)>? PossibleMoves;
    public HashSet<Vector2> Neighbors = new ();

    public State2(Vector2 size, TileManager tileManager, bool xSymmetry, bool ySymmetry)
    {
        Map = new Map2(size, tileManager, xSymmetry, ySymmetry);
    }

    private State2() { }
    
    public void CalculateMoves(Random random)
    {
        if (!Neighbors.Any(x => Map.Field[x].Length > 1))
        {
            //PossibleMoves = new Queue<(Vector3, Tile)>();
            var neighbor1 = Map.Field.Keys.Where(x => Map.Field[x].Length > 1).MinBy(x => Map.Field[x].Length);
            PossibleMoves = new Queue<(Vector2, Tile)>(Map.Field[neighbor1].OrderBy(_ => random.Next()).Select(x => (neighbor1, x)));
            return;
        }
        var neighbor = Neighbors.Where(x => Map.Field[x].Length > 1).MinBy(x => Map.Field[x].Length);
        PossibleMoves = new Queue<(Vector2, Tile)>(Map.Field[neighbor].OrderBy(_ => random.Next()).Select(x => (neighbor, x)));
    }

    public void Wave()
    {
        var queue = new Queue<Vector2>(VisitedCells.ToArray());
        var visited = new HashSet<Vector2>(VisitedCells.ToArray());
        
        while (queue.Count != 0)
        {
            queue = new Queue<Vector2>(queue.OrderBy(x => Map.Field[x].Length));
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

    private List<(Vector2, Directions)> GetNotVisitedNeighbors(Vector2 currCell, HashSet<Vector2> visited)
    {
        var neighbors = new List<(Vector2, Directions)>();
        foreach (var direction in DirectionConstants.DirectionsVectors)
        {
            var newCell = (currCell.X + direction.Value.X,
                currCell.Y + direction.Value.Y);
            if (IsCellInBounds(newCell) && !visited.Contains(newCell))
                neighbors.Add((newCell, direction.Key));
        }
        return neighbors;
    }
    
    public void SetTile(Vector2 cell, Tile tile)
    {
        Map.Field[cell] = new[] { tile };
        foreach (var neighbor in GetNotVisitedNeighbors(cell, Neighbors))
            Neighbors.Add(neighbor.Item1);
        Neighbors.Remove(cell);
    }
    
    private void UpdateCellTiles(Vector2 currCell, Vector2 changingCell, Directions direction)
    {
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
    }
    
    public bool IsCollapse()
    {
        return Map.Field.Where(x => x.Value.Length != 1).Select(x => x.Key).ToList().Count == 0;
    }
    
    public bool IsBroken()
    {
        return Map.Field.Where(x => x.Value.Length == 0).Select(x => x.Key).ToList().Count != 0;
    }
    
    private bool IsCellInBounds(Vector2 cell)
    {
        return !(cell.X < 0 || cell.X >= Map.Size.X || cell.Y < 0 || cell.Y >= Map.Size.Y);
    }
    
    public State2 Copy()
    {
        var copy = new State2
        {
            Map = Map.Copy(),
            Neighbors = new HashSet<Vector2>(Neighbors)
        };
        return copy;
    }
}