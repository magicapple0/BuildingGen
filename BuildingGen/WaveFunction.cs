namespace BuildingGen;

public class WaveFunction
{
    private int Seed { get; set; }
    private Random Rand { get; set; }
    public Model CurrModel { get; set; }
    private List<Model> PreviouceFields;
    public int n = 0;
    private Dictionary<(int, int, int), string> directions = new Dictionary<(int, int, int), string>{ 
        {(0, 1, 0), "forward"},
        {(0, -1, 0), "backward"},
        {(1, 0, 0), "right"},
        {(-1, 0, 0), "left"},
        {(0, 0, 1), "up"},
        {(0, 0, -1), "down"},
    };
    Dictionary<string, (int, int)> neighborCellDirections = new Dictionary<string, (int, int)>()
    {
        { "forward", (1, 5) },
        { "backward", (5, 1) },
        { "right", (3, 4) },
        { "left", (4, 3) },
        { "up", (2, 0) },
        { "down", (0, 2) },
    };

    public WaveFunction(int widht, int height, int depth, Tile[] tileSet, int seed)
    {
        CurrModel = new Model(widht, height, depth, tileSet);
        Seed = seed;
        Rand = new Random(Seed);
        Wave();
        PreviouceFields = new List<Model>();
        PreviouceFields.Add(CurrModel);
        CurrModel = CurrModel.Copy();
    }

    public bool Run()
    {
        if (IsCollapse())
            return true;
        var currCell = ChooseCell();
        while (true)
        {
            var choosenTile = ChooseTile(currCell);
            CurrModel.VisitedTiles[currCell.Item1, currCell.Item2, currCell.Item3].Add(choosenTile);
            Wave();
            if (IsCollapse())
                return true;
            //если у одной из клеток нет возможных состояний - вернемнся к 1 состоянию и попробуем еще раз
            if (IsBroken())
            {
                while (CurrModel.VisitedTiles[currCell.Item1, currCell.Item2, currCell.Item3].Count ==
                       PreviouceFields[^1].Field[currCell].Length ||
                       PreviouceFields[^1].Field[currCell].Length == 0)
                {
                    //если не попробовали все тайлы на этой клетке - попробуем
                    //если эту клетку мы никак не можем поставить правильно - делаем шаг назад и пробуем другой тайл в предыдущей клетке
                    /*if (IsAllTilesVisited())
                        return false;*/
                    PreviouceFields.RemoveAt(PreviouceFields.Count - 1);
                    CurrModel = PreviouceFields[^1];
                    currCell = ChooseCell();
                    Console.WriteLine("Шаг назад");
                }
                PreviouceFields[^1].VisitedTiles[currCell.Item1, currCell.Item2, currCell.Item3].Add(choosenTile);
                CurrModel = PreviouceFields[^1].Copy();
                Console.WriteLine("Другой тайл");
            }
            else
            {
                CurrModel.VisitedCells.Add(currCell);
                PreviouceFields.Add(CurrModel);
                CurrModel = CurrModel.Copy();
                currCell = ChooseCell();
                Console.WriteLine(currCell.Item1 + "\t" + currCell.Item2 + "\tДальше");
            }
        }
    }

    private bool IsAllTilesVisited()
    {
        foreach (var tile in CurrModel.Field)
        {
            if (CurrModel.VisitedTiles[tile.Key.Item1, tile.Key.Item2, tile.Key.Item3] != null && 
                CurrModel.VisitedTiles[tile.Key.Item1, tile.Key.Item2, tile.Key.Item3].Count != tile.Value.Length)
                return false;
        }
        return true;
    }

    private bool IsBroken()
    {
        foreach (var tile in CurrModel.Field)
        {
            if (tile.Value.Length == 0)
                return true;
        }
        return false;
    }

    public bool IsCollapse()
    {
        foreach (var tile in CurrModel.Field)
        {
            if (tile.Value.Length != 1)
                return false;
        }
        return true;
    }

    private void Wave()
    {
        //bfs
        Queue<(int, int, int)> queue = new Queue<(int, int, int)>(CurrModel.VisitedCells.ToArray());
        List<(int, int, int)> visited = new List<(int, int, int)>(CurrModel.VisitedCells.ToArray());
        
        while (queue.Count != 0)
        {
            var currCell = queue.Dequeue();
            foreach (var direction in directions)
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
            if (!visited.Contains(currCell))
                visited.Add(currCell);
        }
    }

    private void UpdateCellTiles((int, int, int) currCell, (int, int, int) changingTile, string direction)
    {
        var neighborCellDirection = neighborCellDirections[direction].Item1;
        var currCellDirection = neighborCellDirections[direction].Item2;
        
        var newNeighborTiles = new List<Tile>();
        var oldNeighborTiles = CurrModel.Field[changingTile];
        var currCellTiles = CurrModel.Field[currCell];
        foreach (var currCellTile in currCellTiles)
        {
            var currCellNeighborsTilesNames = currCellTile.ModifiedEdges[currCellDirection];
            foreach (var currCellTileName in currCellNeighborsTilesNames)
            {
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
        }
        CurrModel.Field[changingTile] = newNeighborTiles.ToArray();
    }

    private bool IsAble((int, int, int) currCell, (int, int, int) offset)
    {
        var newCell = (currCell.Item1 + offset.Item1, currCell.Item2 + offset.Item2, currCell.Item3 + offset.Item3);
        if (newCell.Item1 < 0 || newCell.Item1 >= CurrModel.Width || newCell.Item2 < 0 || newCell.Item2 >= CurrModel.Depth ||
            newCell.Item3 < 0 || newCell.Item3 >= CurrModel.Height)
            return false;
        return true;
    }
    
    private Tile ChooseTile((int, int, int) currCell)
    {
        var availableTiles = CurrModel.Field[(currCell.Item1, currCell.Item2, currCell.Item3)];
        var choosenTile = availableTiles[Rand.Next() % availableTiles.Length];
        var i = 1;
        while (CurrModel.VisitedTiles[currCell.Item1, currCell.Item2, currCell.Item3].Contains(choosenTile))
        {
            choosenTile = availableTiles[(Rand.Next() % availableTiles.Length + i) % availableTiles.Length];
            i++;
        }
        CurrModel.Field[(currCell.Item1, currCell.Item2, currCell.Item3)] = new []{choosenTile};
        return choosenTile;
    }

    private (int, int, int) ChooseCell8()
    {
        //выбираем все ячейки с минимальной энтропией, исключая коллапсированные
        return CurrModel.Field.Where(x => x.Value.Length > 1).MinBy(x => (x.Value.Length, Rand.Next())).Key;
    }
    private (int, int, int) ChooseCell()
    {
        var cell = (Rand.Next() % CurrModel.Width, Rand.Next() % CurrModel.Depth, Rand.Next() % CurrModel.Height);
        while (CurrModel.Field[(cell.Item1, cell.Item2, cell.Item3)].Length <= 1)
        {
            if (cell.Item1 + 1 != CurrModel.Width)
            {
                cell.Item1 += 1;
                continue;
            }
            cell.Item1 = 0;
            if (cell.Item2 + 1 != CurrModel.Depth)
            {
                cell.Item2 += 1;
                continue;
            }
            cell.Item2 = 0;
            if (cell.Item3 + 1 != CurrModel.Height)
            {
                cell.Item3 += 1;
                continue;
            }
            cell.Item3 = 0;
        }
        return cell;
    } 
}