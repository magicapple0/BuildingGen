using System.Diagnostics;

namespace BuildingGen;

public static class Program
{
    private const string InputTiles = "TileSetups/input.json";
    private const int Width = 5;
    private const int  Depth = 5;
    private const int  Height = 1;
    private const int Seed = 4;
    
    public static void Main()
    {
        
        var confLoader = new ConfigLoader(InputTiles);
        var tiles = confLoader.Tiles;
        //PrintTileSet(tiles);
        
        var n = 100000;

        var function = new WaveFunction((Width, Depth, Height), tiles.ToArray(), Seed, confLoader.XSymmetry, confLoader.YSymmetry);
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

    public static void TimeTest(Vector3 size, int seed, List<Tile> tiles, int n)
    {
        var test = new int[n];
        Console.WriteLine("Time for width = " + size.X + ", depth = " + size.Y + ", height = " + size.Z + ", seed = " + seed);

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine(i);
            var sw = new Stopwatch();
            GC.Collect();
            sw.Start();
            var function = new WaveFunction(size, tiles.ToArray(), seed, true, false);
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

    public static Dictionary<Vector3, Tile> Build(Vector3 size, int seed, string config)
    {
        var confLoader = new ConfigLoader(config);
        var tiles = confLoader.Tiles;

        var function = new WaveFunction(size, tiles.ToArray(), seed, confLoader.XSymmetry, confLoader.YSymmetry);
        function.Run();
        return function.CurrModel.Result();
    }

    public static Dictionary<Vector3, Tile> BuildTestTile()
    {
        var cornerInfo = new TileInfo("corner",
            new[]
            {
                new[] { "roof", "corner" }, new[] { "house" }, new[] { "corner", "ground" },
                new[] { "house" }, new[] { "air" }, new[] { "air" }
            },
            false, false, false, null,
            new[]
            {
                "Textures/roof.png", "Textures/bottom.png", "Textures/bottom.png", "Textures/bottom.png",
                "Textures/corner.png", "Textures/corner_flip.png"
            });
        var tile = new Tile(cornerInfo);
        var tile1 = new Tile(cornerInfo);
        tile1.FlipX();
        tile1.FlipY();
        tile1.FlipX();
        tile1.FlipY();
        return new Dictionary<Vector3, Tile>{{new Vector3(0,0,0), tile}, {new Vector3(1,1,1), tile1}} ;
    }
}