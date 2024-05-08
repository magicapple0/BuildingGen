using System.Diagnostics;

namespace BuildingGen;

public static class Program
{
    private const string InputTiles = "TileSetups/pileHouse.json";
    private const int Width = 4;
    private const int  Depth = 4;
    private const int  Height = 4;
    private const int Seed = 1;
    
    public static void Main()
    {
        
        var confLoader = new ConfigLoader(InputTiles);
        var tiles = confLoader.Tiles;
        //PrintTileSet(tiles);
        
        var n = 100000;

        var function = new WaveFunction(Width, Depth, Height, tiles.ToArray(), Seed);
        var b = function.Run();
        Console.WriteLine(b);

        /*for (int i = 0; i < n; i++)
        {
            if (b)
                Console.WriteLine(i + "\t");
            function = new WaveFunction(width, depth, height, tiles.ToArray(), i);
            b = function.Run();
        }*/
        
        //function.Run().Last();
        //Console.WriteLine(function.Run());
        /*for (int i = 0; i < n; i++)
        {
            Console.WriteLine(i);
            function = new WaveFunction(width, height, tiles.ToArray(), i);
            if (!function.Run())
            {
                break;
            }
        }*/
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

    private static void PrintField(Dictionary<Vector3, Tile[]> field)
    {
        foreach (var cell in field)
        {
            Console.Write(cell.Key.X + "\t" + cell.Key.Y + "\t" + cell.Key.Z + "\t");
            foreach (var tile in cell.Value)
                Console.Write(tile.TileInfo.Name + "\t");
            Console.WriteLine();
        }
    }

    public static Tile[,,] Build(Vector3 size, int seed, string config)
    {
        var confLoader = new ConfigLoader(config);
        var tiles = confLoader.Tiles;

        var function = new WaveFunction(size.X, size.Y, size.Z, tiles.ToArray(), seed);
        function.Run();
        return function.CurrModel.Result();
    }

    public static Tile[,,] BuildTestTile()
    {
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
        return a;
    }
}