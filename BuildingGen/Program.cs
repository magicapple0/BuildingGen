using System.Diagnostics;

namespace BuildingGen;

public static class Program
{
    public static void Main()
    {
        var confLoader = new ConfigLoader("input.json");
        var tiles = confLoader.Tiles;
        const int width = 6;
        const int  depth = 6;
        const int  height = 6;
        const int  seed = 0;
        //PrintTileSet(tiles);
        var n = 100000;

        var function = new WaveFunction(width, depth, height, tiles.ToArray(), seed);
        Console.WriteLine(function.Run());
        /*for (int i = 0; i < n; i++)
        {
            Console.WriteLine(i);
            function = new WaveFunction(width, height, tiles.ToArray(), i);
            if (!function.Run())
            {
                break;
            }
        }*/
        PrintField(function.CurrModel.Field);
        //TimeTest(width, height, seed, tiles, n);
    }

    public static void TimeTest(int width, int depth, int height, int seed, List<Tile> tiles, int n)
    {
        var test = new int[n];
        Console.WriteLine("Time for width = " + width + ", depth = " + depth + ", height = " + height + ", seed = " + seed);

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine(i);
            var sw = new Stopwatch();
            GC.Collect();
            sw.Start();
            var function = new WaveFunction(width, depth, height, tiles.ToArray(), seed);
            function.Run();
            test[i] = (int)sw.Elapsed.TotalMilliseconds;
        }

        var sum = 0;
        for (int i = 0; i < n; i++)
        {
            Console.Write(test[i] + "\t");
            sum += test[i];
        }
        Console.WriteLine();
        Console.WriteLine("Mean: " + sum/n);
        
    }

    private static void PrintTileSet(List<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            var edges = "";
            Console.Write(tile.TileInfo.Name + "\t");
            foreach (var edge in tile.ModifiedEdges)
            {
                edges += "(";
                edges = edge.Aggregate(edges, (current, neighbor) => current + (neighbor + " "));
                edges += ") ";
            }
            Console.Write(edges + "\t");
            var modifiers = "";
            if (tile.Modifiers.Count == 0)
                modifiers = "None";
            modifiers = tile.Modifiers.Aggregate(modifiers, (current, modifier) => current + (modifier + " "));
            Console.Write(modifiers + "\t");
            Console.WriteLine();
        }
    }

    private static void PrintField(Dictionary<(int, int, int), Tile[]> field)
    {
        foreach (var cell in field)
        {
            Console.Write(cell.Key.Item1 + "\t" + cell.Key.Item2 + "\t" + cell.Key.Item3 + "\t");
            foreach (var tile in cell.Value)
            {
                Console.Write(tile.TileInfo.Name + "\t");
            }
            Console.WriteLine();
        }
    }

    public static Tile[,,] Build(int width, int depth, int height, int seed)
    {
        var confLoader = new ConfigLoader("input.json");
        var tiles = confLoader.Tiles;

        var function = new WaveFunction(width, depth, height, tiles.ToArray(), seed);
        function.Run();

        /*
        var tile = new Tile(new TileInfo("corner",
            new[]
            {
                new[] { "roof", "corner" }, new[] { "house" }, new[] { "corner", "ground" },
                new[] { "house" }, new[] { "air" }, new[] { "air" }
            },
            false, true, null, //null));
            new[] { "roof.png", "bottom.png", "bottom.png", "bottom.png", "corner.png", "corner_flip.png" }));
        var a = new Tile[1, 1, 1];
        tile.RotateZTile();
        tile.RotateZTile();
        a[0, 0, 0] = tile;
        return a;*/
        return function.CurrModel.Result();
    }
}