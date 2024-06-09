namespace BuildingGen;

public class TileCluster
{
    public int Id { get; set; }
    public String Name { get; set; }
    private int Size;
    private int[][] Neighbors;
    public Dictionary<Vector2, Tile> Tiles = new Dictionary<Vector2, Tile>();

    public TileCluster(int size, Dictionary<Vector2, Tile> map, Vector2 firstPoint)
    {
        Size = size;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Tiles.Add((i, j), map[(i + firstPoint.X, j + firstPoint.Y)]);
            }
        }
    }
    
    public override int GetHashCode()
    {
        return (int)Tiles.Sum(tile => (long)tile.Value.TileInfo.Name.GetHashCode());
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;
        if (!(obj is TileCluster))
            return false;
        if (obj.GetHashCode() != GetHashCode())
            return false;
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                if (!Tiles[(i, j)].TileInfo.Name.Equals(((TileCluster)obj).Tiles[(i, j)].TileInfo.Name))
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