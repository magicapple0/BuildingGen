namespace BuildingGen;

public class Model
{
    private int width;
    public int Width{ get; set; }
    public int Height{ get; set; }
    public Tile[] TileSet { get; set; }
    public Tile[,][] Field { get; set; }
    public List<Tile>[,]  TriedTiles { get; set; }

    private Tile[,] result;

    public Tile[,] Result()
    {
            var building = new Tile[Width, Height];
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++) 
                    building[i, j] = Field[i, j][0];
            return building;
    }

    public Model(int widht, int height, Tile[] tileSet)
    {
        Width = widht;
        Height = height;
        TileSet = tileSet;
        Field = new Tile[Width, Height][];
        TriedTiles = new List<Tile>[Width, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (i == 0 || i == Width - 1 || j == 0 || j == Height - 1)
                {
                    Field[i, j] = new [] {TileSet[0]};
                    continue;
                }
                Field[i, j] = TileSet.ToArray();
                TriedTiles[i, j] = new List<Tile>();
            }
        }
    }

    private Model() { }

    public Model Copy()
    {
        var copy = new Model();
        copy.Height = Height;
        copy.Width = Width;
        copy.TileSet = (Tile[])TileSet.Clone();
        copy.Field = (Tile[,][])Field.Clone();
        copy.TriedTiles = new List<Tile>[Width, Height];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                copy.TriedTiles[i, j] = new List<Tile>();
            }
        }
        //TriedTiles = new Dictionary<(int, int), List<Tile>>();
        return copy;
    }
}