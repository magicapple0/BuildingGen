namespace BuildingGen;

public class TileCluster
{
    private int Id;
    private int Size;
    private int[][] Neighbors;
    public Dictionary<Vector2, Tile> Tiles;

    public TileCluster(int size, Dictionary<Vector2, Tile> map, Vector2 firstPoint)
    {
        Size = size;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Tiles[(i, j)] = map[(i + firstPoint.X, j + firstPoint.Y)];
            }
        }
    }
    
    public override int GetHashCode()
    {
        return (int)Tiles.Sum(tile => (long)tile.GetHashCode());
    }

    public override bool Equals(object? obj)
    {
        if (!(obj is TileCluster))
            return false;
        if (obj.GetHashCode() != GetHashCode())
            return false;
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (!Tiles[(i, j)].Equals(((TileCluster)obj).Tiles[(i, j)]))
                    return false;
            }
        }
        return true;
    }

    public override string ToString()
    {
        return Tiles.Aggregate("", (current, tile) => 
            current + ("( " + tile.Key.X + ", " + tile.Key.Y + "): " + tile.Value.TileInfo.Name + "\n"));
    }
}