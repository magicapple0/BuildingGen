namespace BuildingGen;

public class Model
{
    private int width;
    public int Width{ get; set; }
    public int Depth{ get; set; }
    public Tile[] TileSet { get; set; }
    public Dictionary<(int, int), Tile[]> Field { get; set; }
    public List<Tile>[,]  VisitedTiles { get; set; }
    public List<(int, int)> VisitedCells { get; set; }
    public (int, int) CurrCell;

    private Tile[,] result;

    public Tile[,] Result()
    {
            var building = new Tile[Width, Depth];
            for (int i = 0; i < Width; i++)
            for (int j = 0; j < Depth; j++)
                building[i, j] = Field[(i, j)][0];
            return building;
    }

    public Model(int widht, int depth, Tile[] tileSet)
    {
        Width = widht;
        Depth = depth;
        TileSet = tileSet;
        CurrCell = (0, 0);
        Field = new Dictionary<(int, int), Tile[]>();
        VisitedCells = new List<(int, int)>();
        VisitedTiles = new List<Tile>[Width, Depth];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                if (i == 0 || i == Width - 1 || j == 0 || j == Depth - 1)
                {
                    Field[(i, j)] = new [] {TileSet[^1]};
                    VisitedCells.Add((i, j));
                    continue;
                }
                Field[(i, j)] = TileSet.ToArray();
                VisitedTiles[i, j] = new List<Tile>();
            }
        }
    }

    private Model() { }

    public Model Copy()
    {
        var copy = new Model();
        copy.Depth = Depth;
        copy.CurrCell = CurrCell;
        copy.Width = Width;
        copy.TileSet = (Tile[])TileSet.Clone();
        copy.Field = new Dictionary<(int, int), Tile[]>();
        copy.VisitedTiles = new List<Tile>[Width, Depth];
        copy.VisitedCells = new List<(int, int)>(VisitedCells);
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                copy.Field[(i, j)] = (Tile[])Field[(i, j)].Clone();
                if (VisitedTiles[i, j] != null)
                    copy.VisitedTiles[i, j] = new List<Tile>(VisitedTiles[i, j]);
            }
        }
        return copy;
    }
    
    
}