namespace BuildingGen;

public class TileManager
{
    public Tile[] TileSet { get; set; }
    public Tile Ground { get; set; }
    public Tile Bound { get; set; }

    public TileManager(Tile[] tileSet)
    {
        var newTileSet = new List<Tile>(tileSet);
        Ground = tileSet[^2];
        Bound = tileSet[^1];
        newTileSet.RemoveAt(newTileSet.Count - 2);
        newTileSet.RemoveAt(newTileSet.Count - 1);
        TileSet = newTileSet.ToArray();
    }
    
    public List<Tile> GetOddSymmetryTiles(Directions direction)
    {
        var symmetryTiles = new List<Tile>();
        var directions = DirectionConstants.CellOppositeSides[direction];
        foreach (var tile in TileSet)
        {
            var symmetryTile = tile.Copy();
            var modifiedEdges = tile.ModifiedEdges[directions.Item1].Where(neighbor => tile.ModifiedEdges[directions.Item2].Contains(neighbor)).ToList();
            if (modifiedEdges.Count == 0)
                continue;
            symmetryTile.ModifiedEdges[directions.Item1] = modifiedEdges.ToArray();
            symmetryTile.ModifiedEdges[directions.Item2] = modifiedEdges.ToArray();
            symmetryTiles.Add(symmetryTile);
        }
        return symmetryTiles;
    }

    public List<Tile> GetEvenSymmetryTiles(Directions direction)
    {
        var symmetryTiles = new List<Tile>();
        var directions = DirectionConstants.CellOppositeSides[direction];
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
}