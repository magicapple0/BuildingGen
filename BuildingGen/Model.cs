namespace BuildingGen;

public class Model
{
    public int Width{ get; set; }
    public int Height{ get; set; }
    public int Depth{ get; set; }
    public Tile[] TileSet { get; set; }
    public Dictionary<(int, int, int), Tile[]> Field { get; set; }
    public List<Tile>[,,]  VisitedTiles { get; set; }
    public List<(int, int, int)> VisitedCells { get; set; }

    private Tile[,] result;

    public Tile[,,] Result()
    {
            var building = new Tile[Width, Depth, Height];
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Depth; j++)
                    for (int k = 0; k < Height; k++)
                    building[i, j, k] = Field[(i, j, k)][0];
            return building;
    }

    public Model(int widht, int depth, int height, Tile[] tileSet)
    {
        Width = widht;
        Depth = depth;
        Height = height;
        TileSet = tileSet;
        Field = new Dictionary<(int, int, int), Tile[]>();
        VisitedCells = new List<(int, int, int)>();
        VisitedTiles = new List<Tile>[Width, Depth, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                for (int k = 0; k < Height; k++)
                {
                    if (k == 0)
                    {
                        Field[(i, j, k)] = new [] {TileSet[^2]};
                        VisitedCells.Add((i, j, k));
                        continue;
                    }
                    if (i == 0 || i == Width - 1 || j == 0 || j == Depth - 1 || k == Height - 1 )
                    {
                        Field[(i, j, k)] = new [] {TileSet[^1]};
                        VisitedCells.Add((i, j, k));
                        continue;
                    }
                    Field[(i, j, k)] = TileSet.ToArray();
                    VisitedTiles[i, j, k] = new List<Tile>();
                }
            }
        }
    }

    private Model() { }

    public Model Copy()
    {
        var copy = new Model();
        copy.Depth = Depth;
        copy.Width = Width;
        copy.Height = Height;
        copy.TileSet = (Tile[])TileSet.Clone();
        copy.Field = new Dictionary<(int, int, int), Tile[]>();
        copy.VisitedTiles = new List<Tile>[Width, Depth, Height];
        copy.VisitedCells = new List<(int, int, int)>(VisitedCells);
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                for (int k = 0; k < Height; k++)
                {
                    copy.Field[(i, j, k)] = (Tile[])Field[(i, j, k)].Clone();
                    if (VisitedTiles[i, j, k] != null)
                        copy.VisitedTiles[i, j, k] = new List<Tile>(VisitedTiles[i, j, k]);
                }
            }
        }
        return copy;
    } 
}