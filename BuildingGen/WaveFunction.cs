namespace BuildingGen;

public class WaveFunction
{
    //private Tile[] TileSet { get; set; }
    private int Seed { get; set; }
    private Random Rand { get; set; }
    public Model CurrModel { get; set; }
    private List<Model> PreviouceFields;
    public List<(int, int)> Visited { get; set; }

    public WaveFunction(int widht, int height, Tile[] tileSet, int seed)
    {
        CurrModel = new Model(widht, height, tileSet);
        Seed = seed;
        //TileSet = tileSet.ToArray();
        Visited = new List<(int, int)>();
        Rand = new Random(Seed);
        Wave((0,0));
        PreviouceFields = new List<Model>();
        PreviouceFields.Add(CurrModel);
        CurrModel = CurrModel.Copy();
    }

    public bool Run()
    {
        while (!isCollapse())
        {
            var currCell = ChooseCell();
            if (!ChooseTile(currCell))
            {
                return false;
            }
            Wave(currCell);
            //если у одной из клеток нет возможных состояний - вернемнся к 1 состоянию и попробуем еще раз
            if (isBroken())
            {
                Visited = new List<(int, int)>();
                CurrModel = PreviouceFields[0].Copy();
                PreviouceFields = new List<Model>{PreviouceFields[0]};
            }
            else
            {
                Visited.Add(currCell);
                PreviouceFields.Add(CurrModel);
                CurrModel = CurrModel.Copy();
            }
        }
        return true;
    }

    private bool isBroken()
    {
        foreach (var tile in CurrModel.Field)
        {
            if (tile.Length == 0)
                return true;
        }
        return false;
    }

    public bool isCollapse()
    {
        foreach (var tile in CurrModel.Field)
        {
            if (tile.Length != 1)
                return false;
        }
        return true;
    }

    private void Wave((int, int) startCell)
    {
        //bfs
        Queue<(int, int)> queue = new Queue<(int, int)>(new []{startCell});
        List<(int, int)> visited = new List<(int, int)>(Visited.ToArray());

        while (queue.Count != 0)
        {
            var currCell = queue.Dequeue();
            
            //up
            if (IsAble(currCell, (0, 1)))
            {
                var up = (currCell.Item1, currCell.Item2 + 1);
                if (!visited.Contains(up))
                {
                    UpdateCellTiles(currCell, up, "up");
                    queue.Enqueue(up);
                }
            }
            //down
            if (IsAble(currCell, (0, -1)))
            {
                var down = (currCell.Item1, currCell.Item2 - 1);
                if (!visited.Contains(down))
                {
                    UpdateCellTiles(currCell, down, "down");
                    queue.Enqueue(down);
                }
            }
            //right
            if (IsAble(currCell, (1, 0)))
            {
                var right = (currCell.Item1 + 1, currCell.Item2);
                if (!visited.Contains(right))
                {
                    UpdateCellTiles(currCell, right, "right");
                    queue.Enqueue(right);
                }
            }
            //left
            if (IsAble(currCell, (-1, 0)))
            {
                var left = (currCell.Item1 - 1, currCell.Item2);
                if (!visited.Contains(left))
                {
                    UpdateCellTiles(currCell, left, "left");
                    queue.Enqueue(left);
                }
            }
            visited.Add(currCell);
        }
    }

    private void UpdateCellTiles((int, int) currCell, (int, int) changingTile, string direction)
    {
        var currCellDirection = -1;
        var neighborCellDirection = -1;
        switch (direction)
        {
            case "up":
                neighborCellDirection = 1;
                currCellDirection = 5;
                break;
            case "down":
                neighborCellDirection = 5;
                currCellDirection = 1;
                break;
            case "right":
                neighborCellDirection = 3;
                currCellDirection = 4;
                break;
            case "left":
                neighborCellDirection = 4;
                currCellDirection = 3;
                break;
            default:
                neighborCellDirection = -1;
                currCellDirection = -1;
                break;
        }
        var newNeighborTiles = new List<Tile>();
        var oldNeighborTiles = CurrModel.Field[changingTile.Item1, changingTile.Item2];
        var currCellTiles = CurrModel.Field[currCell.Item1, currCell.Item2];
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
        CurrModel.Field[changingTile.Item1, changingTile.Item2] = newNeighborTiles.ToArray();
    }

    private bool IsAble((int, int) currCell, (int, int) ofset)
    {
        var newCell = (currCell.Item1 + ofset.Item1, currCell.Item2 + ofset.Item2);
        if (newCell.Item1 < 0 || newCell.Item1 >= CurrModel.Width || newCell.Item2 < 0 || newCell.Item2 >= CurrModel.Height)
            return false;
        return true;
    }
    
    private bool ChooseTile((int, int) currCell)
    {
        var availableTiles = CurrModel.Field[currCell.Item1, currCell.Item2];
        if (availableTiles.Length == 0)
            return false;
        var choosenTile = availableTiles[Rand.Next() % availableTiles.Length];
        var i = 1;
        while (CurrModel.TriedTiles[currCell.Item1, currCell.Item2].Contains(choosenTile))
        {
            choosenTile = availableTiles[(Rand.Next() % availableTiles.Length + i) % availableTiles.Length];
            i++;
        }
        CurrModel.TriedTiles[currCell.Item1, currCell.Item2].Add(choosenTile);
        CurrModel.Field[currCell.Item1, currCell.Item2] = new []{choosenTile};
        return true;
        /*foreach (var tile in availableTiles)
        {
            if (!CurrModel.TriedTiles[currCell.Item1, currCell.Item2].Contains(tile))
            {
                CurrModel.TriedTiles[currCell.Item1, currCell.Item2].Add(tile);
                CurrModel.Field[currCell.Item1, currCell.Item2] = new Tile[]{tile};
                return true;
            }
        }
        return false;*/
    }
    
    private (int, int) ChooseCell()
    {
        var cell = (Rand.Next() % CurrModel.Width, Rand.Next() % CurrModel.Height);
        //var cell = (Seed % CurrModel.Width, Seed % CurrModel.Height);
        while (CurrModel.Field[cell.Item1, cell.Item2].Length <= 1)
        {
            if (cell.Item1 + 1 != CurrModel.Width)
            {
                cell.Item1 += 1;
                continue;
            }
            cell.Item1 = 0;
            if (cell.Item2 + 1 != CurrModel.Height)
            {
                cell.Item2 += 1;
                continue;
            }
            cell.Item2 = 0;
        }
        return cell;
    } 
}