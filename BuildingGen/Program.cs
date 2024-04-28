namespace BuildingGen;

public static class Program
{
    public static void Main()
    {
        var confLoader = new ConfigLoader("input.json");
        var tiles = confLoader.Tiles;
        var width = 7;
        var height = 7;
        //PrintTileSet(tiles);

        var function = new WaveFunction(width, height, tiles.ToArray(), 2);

        Console.WriteLine(function.Run());
        PrintField(function.CurrModel.Field);
        //Console.WriteLine(function.Run());
        //Console.WriteLine(function.Run());
        //Console.WriteLine(function.Run());
    }

    public static void PrintTileSet(List<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            var edges = "";
            Console.Write(tile.TileInfo.Name + "\t");
            foreach (var edge in tile.ModifiedEdges)
            {
                edges += "(";
                foreach (var neighbor in edge)
                {
                    edges += neighbor + " ";
                }
                edges += ") ";
            }
            Console.Write(edges + "\t");
            var modifiers = "";
            if (tile.TileModifiers.Count == 0)
                modifiers = "None";
            foreach (var modifier in tile.TileModifiers)
                modifiers += modifier + " ";
            Console.Write(modifiers + "\t");
            Console.WriteLine();
        }
    }

    public static void PrintField(Tile[,][] field)
    {
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int j = 0; j < field.GetLength(1); j++)
            {
                Console.Write(i + "\t" + j + "\t");
                foreach (var tile in field[i,j])
                {
                    Console.Write(tile.TileInfo.Name + "\t");
                }
                Console.WriteLine();
            }
        }
    }

    public static Tile[,] Build(int width, int height, int seed)
    {
        var confLoader = new ConfigLoader("input.json");
        var tiles = confLoader.Tiles;

        var function = new WaveFunction(width, height, tiles.ToArray(), seed);
        function.Run();
        
        return function.CurrModel.Result();
    }
}