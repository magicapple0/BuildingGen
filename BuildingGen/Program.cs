namespace BuildingGen;

public static class Program
{
    public static void Main()
    {
        var field = CreateExampleField2D();
        var tileClusterSet = new TileManager(field, 3);
        int i = 0;
        //PrintTileSet(tileClusterSet.TileSet);
        PrintField(From2Dto3D(tileClusterSet.Field));
        foreach (var cluster in tileClusterSet.TileClusters)
        {
            Console.WriteLine("==========" + i++ + "=========");
            Console.Write(cluster.ToString());
        }
        
        //PrintField(BuildFromInput2DField((5,5), 0, "InputFields/inputField2D.json"));
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
        n = 3;
        for (int j = 0; j < 3; j++)
            for (int i = 0; i < 9; i++)
                exampleField[(1 + i / n, 1 + (i % n), j + 1)] = new Tile("house", tileId++, houseTexture, null);

        
        for (int j = 0; j < n; j++)
        {
            exampleField[(1, 1 + j, 3)] = new Tile("roof", tileId++, roofTexture, null);
            exampleField[(2, 1 + j, 4)] = new Tile("roof", tileId++, roofTexture, null);
            exampleField[(3, 1 + j, 3)] = new Tile("roof", tileId++, roofTexture, null);
            exampleField[(2, 1 + j, 3)] = new Tile("house", tileId++, houseTexture, null);
        }
        return exampleField;
    }

    public static Dictionary<Vector2, Tile> CreateExampleField2D()
    {
        int tileId = 0;
        var exampleField = new Dictionary<Vector2, Tile>();
        
        int n = 39;
        int k = 6;
        for (int i = 0; i < n * n; i++)
        {
            exampleField[(i / n, i % n)] = new Tile("grass", tileId++, null, "3C8950");
        }

        for (int i = 0; i < n; i++)
        {
            exampleField[(n / 2, i)] = new Tile("road", tileId++, null, "1c1c1c");
            exampleField[(i, n / 2)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                exampleField[(n / 2 + i - 1, n / 2 + j - 1)] = new Tile("roundabout", tileId++, null, "8c188a");
            }
        }
        exampleField[(k + 1, n / 2)] = new Tile("cross", tileId++, null, "FFFFFF");
        for (int i = n / 2 + 1; i < n - k - 1; i++)
        {
            exampleField[(k + 1, i)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        for (int i = 0; i < k + 1; i++)
        {
            exampleField[(i, n - k - 2)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        exampleField[(k + 1, n - k - 2)] = new Tile("turn", tileId++, null, "FFF000");
        
        exampleField[(n / 2, k + 1)] = new Tile("cross", tileId++, null, "FFFFFF");
        for (int i = n / 2 - 1; i >= k + 1; i--)
        {
            exampleField[(i, k + 1)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        exampleField[(k + 1, k + 1)] = new Tile("turn", tileId++, null, "FFF000");
        for (int i = 0; i < k + 1; i++)
        {
            exampleField[(k + 1, i)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        
        for (int i = n - 1; i >= n - k - 1; i--)
        {
            exampleField[(i, k + 1)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        for (int i = n / 2 - 1; i >= k + 1; i--)
        {
            exampleField[(n - k - 2, i)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        exampleField[(n - k - 2, k + 1)] = new Tile("turn", tileId++, null, "FFF000");
        exampleField[(n - k - 2, n / 2)] = new Tile("cross", tileId++, null, "FFFFFF");
        
        for (int i = n - k - 1; i < n; i++)
        {
            exampleField[(n - k - 2, i)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        for (int i = n / 2; i < n - k - 1; i++)
        {
            exampleField[(i, n - k - 2)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        exampleField[(n - k - 2, n - k - 2)] = new Tile("turn", tileId++, null, "FFF000");
        exampleField[(n / 2, n - k - 2)] = new Tile("cross", tileId++, null, "FFFFFF");
        return exampleField;
    }
    
    public static Dictionary<Vector2, Tile> CreateExampleField2D2()
    {
        int tileId = 0;
        var exampleField = new Dictionary<Vector2, Tile>();
        
        int n = 26;
        int k = 4;
        for (int i = 0; i < n * n; i++)
        {
            if ((i / n + i % n) % 2 == 0)
                exampleField[(i / n, i % n)] = new Tile("grass", tileId++, null, "3C8950");
            else
                exampleField[(i / n, i % n)] = new Tile("grass", tileId++, null, "3C8990");
        }

        for (int i = 0; i < n; i++)
        {
            exampleField[(n / 2, i)] = new Tile("road", tileId++, null, "1c1c1c");
            exampleField[(i, n / 2)] = new Tile("road", tileId++, null, "1c1c1c");
            exampleField[(n / 2 - 1, i)] = new Tile("road", tileId++, null, "1c1c1c");
            exampleField[(i, n / 2 - 1)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                exampleField[(n / 2 + i - 2, n / 2 + j - 2)] = new Tile("square", tileId++, null, "8c188a");
            }
        }

        exampleField[(k + 1, n / 2)] = new Tile("cross", tileId++, null, "FFFFFF");
        for (int i = n / 2 + 1; i < n - k - 1; i++)
        {
            exampleField[(k + 1, i)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        for (int i = 0; i < k + 1; i++)
        {
            exampleField[(i, n - k - 2)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        exampleField[(k + 1, n - k - 2)] = new Tile("turn", tileId++, null, "FFF000");
        
        exampleField[(n / 2, k + 1)] = new Tile("cross", tileId++, null, "FFFFFF");
        for (int i = n / 2 - 1; i >= k + 1; i--)
        {
            exampleField[(i, k + 1)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        exampleField[(k + 1, k + 1)] = new Tile("turn", tileId++, null, "FFF000");
        for (int i = 0; i < k + 1; i++)
        {
            exampleField[(k + 1, i)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        
        for (int i = n - 1; i >= n - k - 1; i--)
        {
            exampleField[(i, k + 1)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        for (int i = n / 2 - 1; i >= k + 1; i--)
        {
            exampleField[(n - k - 2, i)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        exampleField[(n - k - 2, k + 1)] = new Tile("turn", tileId++, null, "FFF000");
        exampleField[(n - k - 2, n / 2)] = new Tile("cross", tileId++, null, "FFFFFF");
        
        for (int i = n - k - 1; i < n; i++)
        {
            exampleField[(n - k - 2, i)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        for (int i = n / 2; i < n - k - 1; i++)
        {
            exampleField[(i, n - k - 2)] = new Tile("road", tileId++, null, "1c1c1c");
        }
        exampleField[(n - k - 2, n - k - 2)] = new Tile("turn", tileId++, null, "FFF000");
        exampleField[(n / 2, n - k - 2)] = new Tile("cross", tileId++, null, "FFFFFF");
        return exampleField;
    }
    
    private static Dictionary<Vector3, Tile> From2Dto3D(Dictionary<Vector2, Tile> map)
    {
        return map.ToDictionary(x => x.Key.ToVector3(), x => x.Value);
    }

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
    
    private static void PrintField(Dictionary<Vector3, Tile> field)
    {
        foreach (var cell in field)
            Console.WriteLine(cell.Key.X + "\t" + cell.Key.Y + "\t" + cell.Key.Z + "\t" + cell.Value.TileInfo.Name);
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
        //var inputField = JsonManipulator.GetTilesSetFromFieldJson(inputFile);
        var inputField = CreateExampleField();
        var tileManager = new TileManager(inputField);
        /*var exampleField = CreateExampleField();
        var tileManager = new TileManager(exampleField);*/
        var function = new WaveFunction(size, tileManager, seed, false, false);
        function.Run();
        //JsonManipulator.SaveJsonResult(function.CurrState.Map.Result(), "generatedSimpleHouse.json");
        return function.CurrState.Map.Result();
    }
    
    public static Dictionary<Vector3, Tile> BuildFromInput2DField(Vector2 size, int seed, string inputFile)
    {
        var inputField = JsonManipulator.GetTilesSetFromFieldJson2D(inputFile);
        var tileManager = new TileManager(inputField);
        /*var exampleField = CreateExampleField();
        var tileManager = new TileManager(exampleField);*/
        var function = new WaveFunction2(size, tileManager, seed, false, false);
        function.Run();
        var result = From2Dto3D(function.CurrState.Map.Result());
        Save(result, "generatedField2D.json");
        return result;
    }

    public static Dictionary<Vector3, Tile> BuildTest2DField()
    {
        var result = From2Dto3D(CreateExampleField2D2());
        Save(result, "inputField2D.json");
        return result;
    }
    
    public static Dictionary<Vector3, Tile> GenerateFromTest2DField(Vector2 size, int seed)
    {
        var field = CreateExampleField2D();
        var tileClusterSet = new TileManager(field, 3);
        var function = new WaveFunction2(size, tileClusterSet, seed, false, false);
        function.Run();
        var result = From2Dto3D(function.CurrState.Map.Result());
        Save(result, "generatedField2D.json");
        return result;
    }
    
    public static Dictionary<Vector3, Tile> DivideGeneratedField(string inputFile, Vector2 min, Vector2 max, int seed)
    {
        var inputField = JsonManipulator.GetTilesSetFromFieldJson2D(inputFile);
        BSP.GetFoundations(inputField, min, max, new Random(seed));
        return From2Dto3D(inputField);
    }
    
    public static Dictionary<Vector3, Tile> BuildTestTile()
    {
        return CreateExampleField();
    }

    private static void Save(Dictionary<Vector3, Tile> result, String fileName)
    {
        JsonManipulator.SaveJsonResult(result, fileName);
    }

    public static Dictionary<Vector3, Tile> GenerateStreet(string inputStreetFile, string inputHouseFile, Vector2 min, Vector2 max, int seed)
    {
        Random random = new Random(seed);
        var inputField = JsonManipulator.GetTilesSetFromFieldJson2D(inputStreetFile);
        var foundations = BSP.GetFoundations(inputField, min, max, random);
        var street = From2Dto3D(inputField);
        foreach (var foundation in foundations)
        {
            GenerateHousesOnTheStreet(street, foundation.Value, random.Next(), inputHouseFile);
        }
        Save(street, "street.json");
        return street;
    }

    public static void GenerateHousesOnTheStreet(Dictionary<Vector3, Tile> map, List<Vector2> foundation, int seed, String inputHouseFile)
    {
        var maxPoint = foundation.Aggregate((0, 0),
            (max, pair) => (Math.Max(max.Item1, pair.X), Math.Max(max.Item2, pair.Y)));
        var minPoint = foundation.Aggregate((9999999999, 99999999999),
            (min, pair) => (Math.Min(min.Item1, pair.X), Math.Min(min.Item2, pair.Y)));
        var size = new Vector2((int)(maxPoint.Item1 - minPoint.Item1 + 1), (int)(maxPoint.Item2 - minPoint.Item2 + 1));
        var house = BuildFromInputTileSet(new Vector3(size.X + 1, size.Y + 1, 10), seed, inputHouseFile);
        //var house = BuildFromInputField(new Vector3(size.X, size.Y, 15), seed, inputHouseFile);
        var f = false;
        foreach (var cell in house)
        {
            if (!cell.Value.TileInfo.Name.Equals("air"))
                f = true;
            
            if (cell.Key.Z > 0 &&  !cell.Value.TileInfo.Name.Equals("air"))
                map[(cell.Key.X + (int)minPoint.Item1, cell.Key.Y + (int)minPoint.Item2, cell.Key.Z)] = cell.Value;
        }

        if (f == false)
            GenerateHousesOnTheStreet(map, foundation, seed + 1, inputHouseFile);
    }
}