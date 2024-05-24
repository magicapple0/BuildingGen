using System.Diagnostics;

namespace BuildingGen;

public static class Program
{
    private const string InputTiles = "TileSetups/triangleRoof.json";
    private const int Width = 15;
    private const int  Depth = 15;
    private const int  Height = 9;
    private const int Seed = 0;
    
    public static void Main()
    {
        //JsonManipulator.SaveJsonResult(CreateExampleField(), "simpleHouse.json");
        /*var confLoader = JsonManipulator.GetTileSetDescriptionFromJson(InputTiles);
        var tileManager = new TileManager(confLoader.TilesInfo);
        var function = new WaveFunction((Width, Depth, Height), tileManager , Seed, confLoader.XSymmetry, confLoader.YSymmetry);
        function.Run();
        JsonManipulator.SaveJsonResult(function.CurrState.Map.Result(), "triangleRoof.json");*/

        /*var inputField = JsonManipulator.GetTilesSetFromFieldJson("InputFields/triangleRoof.json");
        var tileManager = new TileManager(inputField);
        PrintTileSet(tileManager.TileSet);
        Console.WriteLine(inputField.Count);
        Console.WriteLine(tileManager.TileSet.Length);
        var function = new WaveFunction((Width, Depth, Height), tileManager , Seed, false, false);
        function.Run();*/
    }

    public static Dictionary<Vector3, Tile> CreateExampleField()
    {
        int tileId = 0;
        var exampleField = new Dictionary<Vector3, Tile>();
        var houseTexture = new[]
        {
            "Textures/roof.png", "Textures/wall.png", "Textures/bottom.png",
            "Textures/wall.png", "Textures/wall.png", "Textures/wall.png"
        };
        var cornerTexture = new[]
        {
            "Textures/roof.png", "Textures/bottom.png", "Textures/bottom.png",
            "Textures/bottom.png", "Textures/corner.png", "Textures/corner_flip.png"
        };
        var roofTexture = new[]
        {
            "Textures/roof.png", "Textures/roof_side.png", "Textures/bottom.png",
            "Textures/roof_side.png", "Textures/roof_side.png", "Textures/roof_side.png"
        };
        int n = 5;
        for (int i = 0; i < 25; i++)
            exampleField[(i / n, i % n, 0)] = new Tile("grass", tileId++, null, "3C896D");
        for (int j = 0; j < 4; j++)
            for (int i = 0; i < 25; i++)
                exampleField[(i / n, i % n, j + 1)] = new Tile("house", tileId++, houseTexture, null);
        for (int i = 0; i < 4; i++)
        {
            exampleField[(0, 0, i + 1)] = new Tile("corner", tileId++, cornerTexture, null);
            exampleField[(0, 0, i + 1)].RotateZTile();
            exampleField[(0, n - 1, i + 1)] = new Tile("corner", tileId++, cornerTexture, null);
            exampleField[(0, n - 1, i + 1)].RotateZTile();
            exampleField[(0, n - 1, i + 1)].RotateZTile();
            exampleField[(n - 1, 0, i + 1)] = new Tile("corner", tileId++, cornerTexture, null);
            exampleField[(n - 1, n - 1, i + 1)] = new Tile("corner", tileId++, cornerTexture, null);
            exampleField[(n - 1, n - 1, i + 1)].RotateZTile(); 
            exampleField[(n - 1, n - 1, i + 1)].RotateZTile();
            exampleField[(n - 1, n - 1, i + 1)].RotateZTile();
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < n; j++)
            {
                exampleField[(j, 0 + i, i + 4)] = new Tile("roof", tileId++, roofTexture, null);
                exampleField[(j, n - i - 1, i + 4)] = new Tile("roof", tileId++, roofTexture, null);
                exampleField[(j, 2, 5)] = new Tile("house", tileId++, houseTexture, null);
            }
                
        }

        /*
        exampleField[(0, 0, 1)] = new Tile("air", 9, null, null);
        exampleField[(0, 1, 1)] = new Tile("air", 10, null, null);
        exampleField[(0, 2, 1)] = new Tile("air", 11, null, null);
        exampleField[(1, 0, 1)] = new Tile("air", 12, null, null);
        exampleField[(1, 1, 1)] = new Tile("house", 13, new []
        {
            "Textures/roof.png", "Textures/wall.png", "Textures/bottom.png", 
            "Textures/wall.png", "Textures/wall.png", "Textures/wall.png"
        }, null);
        exampleField[(1, 2, 1)] = new Tile("air", 14, null, null);
        exampleField[(2, 0, 1)] = new Tile("air", 15, null, null);
        exampleField[(2, 1, 1)] = new Tile("air", 16, null, null);
        exampleField[(2, 2, 1)] = new Tile("air", 17, null, null);
        
        exampleField[(0, 0, 2)] = new Tile("air", 18, null, null);
        exampleField[(0, 1, 2)] = new Tile("air", 19, null, null);
        exampleField[(0, 2, 2)] = new Tile("air", 20, null, null);
        exampleField[(1, 0, 2)] = new Tile("air", 21, null, null);
        exampleField[(1, 1, 2)] = new Tile("roof", 22, new []
        {
            "Textures/roof.png", "Textures/roof_side.png", "Textures/bottom.png", 
            "Textures/roof_side.png", "Textures/roof_side.png", "Textures/roof_side.png"
        }, null);
        exampleField[(1, 2, 2)] = new Tile("air", 23, null, null);
        exampleField[(2, 0, 2)] = new Tile("air", 24, null, null);
        exampleField[(2, 1, 2)] = new Tile("air", 25, null, null);
        exampleField[(2, 2, 2)] = new Tile("air", 26, null, null);*/

        return exampleField;
    }

    /*public static void TimeTest(Vector3 size, int seed, TileInfo[] tilesInfos, int n)
    {
        var test = new int[n];
        Console.WriteLine("Time for width = " + size.X + ", depth = " + size.Y + ", height = " + size.Z + ", seed = " + seed);

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine(i);
            var sw = new Stopwatch();
            GC.Collect();
            sw.Start();
            var function = new WaveFunction(size, tilesInfos, seed, true, false);
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
        
    }*/

    private static void PrintTileSet(Tile[] tiles)
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
            Console.Write(modifiers + "\t" + tile.Id);
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

    public static Dictionary<Vector3, Tile> BuildFromInputTileSet(Vector3 size, int seed, string inputFile)
    {
        var confLoader = JsonManipulator.GetTileSetDescriptionFromJson(inputFile);
        
        var tileManager = new TileManager(confLoader.TilesInfo);
        
        var function = new WaveFunction(size, tileManager, seed, confLoader.XSymmetry, confLoader.YSymmetry);
        function.Run();
        return function.CurrState.Map.Result();
    }
    
    public static Dictionary<Vector3, Tile> BuildFromInputField(Vector3 size, int seed, string inputFile)
    {
        var inputField = JsonManipulator.GetTilesSetFromFieldJson(inputFile);
        var tileManager = new TileManager(inputField);
        /*var exampleField = CreateExampleField();
        var tileManager = new TileManager(exampleField);*/
        var function = new WaveFunction(size, tileManager, seed, false, false);
        function.Run();
        return function.CurrState.Map.Result();
    }

    public static Dictionary<Vector3, Tile> BuildTestTile()
    {
        return CreateExampleField();
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
        var tile = new Tile(cornerInfo, 1);
        var tile1 = new Tile(cornerInfo, 2);
        tile1.FlipX();
        tile1.FlipY();
        tile1.FlipX();
        tile1.FlipY();
        return new Dictionary<Vector3, Tile>{{new Vector3(0,0,0), tile}, {new Vector3(1,1,1), tile1}} ;
    }
}