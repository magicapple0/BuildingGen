using System.Diagnostics;

namespace BuildingGen;

public static class Program
{
    public static void Main()
    {
        var sw = Stopwatch.StartNew();
        GenerateFromTestField((15, 15, 15), 1);
        sw.Stop();
        Console.Write(sw.Elapsed);
        /*var inputField = CreateExampleField3D();
        var tileManager = new TileManager(inputField);
        Console.WriteLine(inputField.Count + " " + tileManager.TileSet.Count());
        inputField = CreateExampleField3D2();
        tileManager = new TileManager(inputField);
        Console.WriteLine(inputField.Count + " " + tileManager.TileSet.Count());*/
    }
    
    public static Dictionary<Vector3, Tile> CreateExampleField3D3()
    {
        int tileId = 0;
        var exampleField = new Dictionary<Vector3, Tile>();
        var houseTexture = new[]
        {
            "Textures/roof.png", "Textures/wall.png", "Textures/bottom.png",
            "Textures/wall.png", "Textures/wall.png", "Textures/wall.png"
        };
        var roofTexture = new[]
        {
            "Textures/roof.png", "Textures/roof_side.png", "Textures/bottom.png",
            "Textures/roof_side.png", "Textures/roof_side.png", "Textures/roof_side.png"
        };
        int n = 3;
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                exampleField[(i, j, 0)] = new Tile("grass", null, "3C896D");
        for (int j = 0; j < 3; j++)
            for (int i = 0; i < 9; i++)
                exampleField[(i / n, i % n, j + 1)] = new Tile("house", houseTexture, null);
        
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < n; j++)
            {
                var h = i + 4;
                if (i > 0)
                {
                    h += 1;
                }
                exampleField[(j, 0 + i, h)] = new Tile("roof",roofTexture, null);
                exampleField[(j, n - i - 1, h)] = new Tile("roof", roofTexture, null);
                exampleField[(j, 2, 5)] = new Tile("house", houseTexture, null);
            }
        }
        return exampleField;
    }
    
    public static Dictionary<Vector3, Tile> CreateExampleField3D4()
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
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 5; j++)
                exampleField[(i, j, 0)] = new Tile("grass", null, "3C896D");
        for (int j = 0; j < 4; j++)
            for (int i = 0; i < 25; i++)
                exampleField[(i / n, i % n, j + 1)] = new Tile("house", houseTexture, null);
                    
            for (int i = 0; i < 4; i++)
            {
                exampleField[(0, 0 + n + 2, i + 1)] = new Tile("corner",  cornerTexture, null);
                exampleField[(0, 0 + n + 2, i + 1)].RotateZTile();
                exampleField[(0, n - 1 + n + 2, i + 1)] = new Tile("corner",  cornerTexture, null);
                exampleField[(0, n - 1 + n + 2, i + 1)].RotateZTile();
                exampleField[(0, n - 1 + n + 2, i + 1)].RotateZTile();
                exampleField[(n - 1, 0 + n + 2, i + 1)] = new Tile("corner", cornerTexture, null);
                exampleField[(n - 1, n - 1 + n + 2, i + 1)] = new Tile("corner",  cornerTexture, null);
                exampleField[(n - 1, n - 1 + n + 2, i + 1)].RotateZTile(); 
                exampleField[(n - 1, n - 1 + n + 2, i + 1)].RotateZTile();
                exampleField[(n - 1, n - 1 + n + 2, i + 1)].RotateZTile();
                
                exampleField[(0, 0, i + 1)] = new Tile("corner", cornerTexture, null);
                exampleField[(0, 0, i + 1)].RotateZTile();
                exampleField[(0, n - 1, i + 1)] = new Tile("corner",  cornerTexture, null);
                exampleField[(0, n - 1, i + 1)].RotateZTile();
                exampleField[(0, n - 1, i + 1)].RotateZTile();
                exampleField[(n - 1, 0, i + 1)] = new Tile("corner",  cornerTexture, null);
                exampleField[(n - 1, n - 1, i + 1)] = new Tile("corner", cornerTexture, null);
                exampleField[(n - 1, n - 1, i + 1)].RotateZTile(); 
                exampleField[(n - 1, n - 1, i + 1)].RotateZTile();
                exampleField[(n - 1, n - 1, i + 1)].RotateZTile();
            }


        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < n; j++)
            {
                var h = i + 4;
                if (i > 0)
                {
                    h += 1;
                }
                exampleField[(j, 0 + i, h)] = new Tile("roof",  roofTexture, null);
                exampleField[(j, n - i - 1, h)] = new Tile("roof",  roofTexture, null);
                exampleField[(j, 2, 5)] = new Tile("house",  houseTexture, null);
                /*exampleField[(j, 1, 5)] = new Tile("house", tileId++, houseTexture, null);
                exampleField[(j, 3, 5)] = new Tile("house", tileId++, houseTexture, null);
                exampleField[(j, 2, 6)] = new Tile("house", tileId++, houseTexture, null);*/
            }
        }
        return exampleField;
    }
    
    public static Dictionary<Vector3, Tile> CreateExampleField3D2()
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
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < 5; j++)
                exampleField[(i, j, 0)] = new Tile("grass",  null, "3C896D");
        for (int j = 0; j < 4; j++)
            for (int i = 0; i < 25; i++)
                exampleField[(i / n, i % n, j + 1)] = new Tile("house",  houseTexture, null);
                    
            /*for (int i = 0; i < 4; i++)
            {
                exampleField[(0, 0 + n + 2, i + 1)] = new Tile("corner", tileId++, cornerTexture, null);
                exampleField[(0, 0 + n + 2, i + 1)].RotateZTile();
                exampleField[(0, n - 1 + n + 2, i + 1)] = new Tile("corner", tileId++, cornerTexture, null);
                exampleField[(0, n - 1 + n + 2, i + 1)].RotateZTile();
                exampleField[(0, n - 1 + n + 2, i + 1)].RotateZTile();
                exampleField[(n - 1, 0 + n + 2, i + 1)] = new Tile("corner", tileId++, cornerTexture, null);
                exampleField[(n - 1, n - 1 + n + 2, i + 1)] = new Tile("corner", tileId++, cornerTexture, null);
                exampleField[(n - 1, n - 1 + n + 2, i + 1)].RotateZTile(); 
                exampleField[(n - 1, n - 1 + n + 2, i + 1)].RotateZTile();
                exampleField[(n - 1, n - 1 + n + 2, i + 1)].RotateZTile();
                
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
            }*/


        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < n; j++)
            {
                var h = i + 4;
                exampleField[(j, 0 + i, h)] = new Tile("roof", roofTexture, null);
                exampleField[(j, n - i - 1, h)] = new Tile("roof",  roofTexture, null);
                exampleField[(j, 2, 5)] = new Tile("house", houseTexture, null);
            }
        }
        return exampleField;
    }
    
    public static Dictionary<Vector3, Tile> CreateExampleField3D()
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
        int n = 7;
        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 16; j++)
                exampleField[(i, j, 0)] = new Tile("grass", null, "3C896D");
        for (int k = 0; k < 2; k++)
        {
            for (int j = 0; j < 5; j++)
                for (int i = 0; i < 49; i++)
                {
                    exampleField[(i / n, i % n, j + 1)] = new Tile("house", houseTexture, null);
                    exampleField[(i / n, i % n + n + 2, j + 1)] = new Tile("house", houseTexture, null);
                    if (i / n + i % n == 0 || i / n + i % n == n - 1 || i / n + i % n == 2 * n - 2)
                        exampleField[(i / n, i % n, j + 1)] = new Tile("corner", cornerTexture, null);
                    if ((i / n == 0 && i % n + n + 2 == 9) || (i / n == 0 && i % n + n + 2 == 15) ||  
                        (i / n == 6 && i % n + n + 2 == 15) || (i / n == 6 && i % n + n + 2 == 9))
                        exampleField[(i / n, i % n + n + 2, j + 1)] = new Tile("corner", cornerTexture, null);
                }
        }


        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < n; j++)
            {
                var h = i + 4;
                if (i > 1)
                {
                    h += 1;
                }
                exampleField[(j, 0 + i, h)] = new Tile("roof", roofTexture, null);
                exampleField[(j, n - i - 1, h)] = new Tile("roof", roofTexture, null);
                exampleField[(j, 3, 8)] = new Tile("roof", roofTexture, null);
                exampleField.Remove((j, 0, 5));
                exampleField.Remove((j, 6, 5));
                exampleField[(j, 3, 6)] = new Tile("house",  houseTexture, null);
                exampleField[(j, 2, 6)] = new Tile("house",  houseTexture, null);
                exampleField[(j, 4, 6)] = new Tile("house",  houseTexture, null);
                exampleField[(j, 3, 7)] = new Tile("house",  houseTexture, null);
            }
                
        }
        
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < n; j++)
            {
                var h = j + 2 + n;
                
                exampleField.Remove((0, h, 5));
                exampleField.Remove((6, h, 5));
                exampleField[(0, h, 4)] = new Tile("roof", roofTexture, null);
                exampleField[(6, h, 4)] = new Tile("roof",  roofTexture, null);
                exampleField[(1, h, 5)] = new Tile("roof", roofTexture, null);
                exampleField[(5, h, 5)] = new Tile("roof",  roofTexture, null);
                exampleField[(2, h, 7)] = new Tile("roof",  roofTexture, null);
                exampleField[(3, h, 8)] = new Tile("roof",  roofTexture, null);
                exampleField[(4, h, 7)] = new Tile("roof",  roofTexture, null);
                exampleField[(2, h, 6)] = new Tile("house",  houseTexture, null);
                exampleField[(3, h, 7)] = new Tile("house", houseTexture, null);
                exampleField[(3, h, 6)] = new Tile("house",  houseTexture, null);
                exampleField[(4, h, 6)] = new Tile("house", houseTexture, null);
                
            }
                
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
            exampleField[(i / n, i % n)] = new Tile("grass",  null, "3C8950");
        }

        for (int i = 0; i < n; i++)
        {
            exampleField[(n / 2, i)] = new Tile("road", null, "1c1c1c");
            exampleField[(i, n / 2)] = new Tile("road",  null, "1c1c1c");
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                exampleField[(n / 2 + i - 1, n / 2 + j - 1)] = new Tile("roundabout", null, "8c188a");
            }
        }
        exampleField[(k + 1, n / 2)] = new Tile("cross",  null, "FFFFFF");
        for (int i = n / 2 + 1; i < n - k - 1; i++)
        {
            exampleField[(k + 1, i)] = new Tile("road", null, "1c1c1c");
        }
        for (int i = 0; i < k + 1; i++)
        {
            exampleField[(i, n - k - 2)] = new Tile("road",  null, "1c1c1c");
        }
        exampleField[(k + 1, n - k - 2)] = new Tile("turn", null, "FFF000");
        
        exampleField[(n / 2, k + 1)] = new Tile("cross",  null, "FFFFFF");
        for (int i = n / 2 - 1; i >= k + 1; i--)
        {
            exampleField[(i, k + 1)] = new Tile("road", null, "1c1c1c");
        }
        exampleField[(k + 1, k + 1)] = new Tile("turn",  null, "FFF000");
        for (int i = 0; i < k + 1; i++)
        {
            exampleField[(k + 1, i)] = new Tile("road",  null, "1c1c1c");
        }
        
        for (int i = n - 1; i >= n - k - 1; i--)
        {
            exampleField[(i, k + 1)] = new Tile("road",  null, "1c1c1c");
        }
        for (int i = n / 2 - 1; i >= k + 1; i--)
        {
            exampleField[(n - k - 2, i)] = new Tile("road",  null, "1c1c1c");
        }
        exampleField[(n - k - 2, k + 1)] = new Tile("turn",  null, "FFF000");
        exampleField[(n - k - 2, n / 2)] = new Tile("cross",  null, "FFFFFF");
        
        for (int i = n - k - 1; i < n; i++)
        {
            exampleField[(n - k - 2, i)] = new Tile("road",  null, "1c1c1c");
        }
        for (int i = n / 2; i < n - k - 1; i++)
        {
            exampleField[(i, n - k - 2)] = new Tile("road",  null, "1c1c1c");
        }
        exampleField[(n - k - 2, n - k - 2)] = new Tile("turn",  null, "FFF000");
        exampleField[(n / 2, n - k - 2)] = new Tile("cross", null, "FFFFFF");
        return exampleField;
    }
    
    public static Dictionary<Vector2, Tile> CreateExampleField2D2()
    {
        int tileId = 0;
        var exampleField = new Dictionary<Vector2, Tile>();
        
        int n = 42;
        int k = 10;
        for (int i = 0; i < n * n; i++)
        {
            //if ((i / n + i % n) % 2 == 0)
                exampleField[(i / n, i % n)] = new Tile("grass", null, "3C8950");
            /*else
                exampleField[(i / n, i % n)] = new Tile("grass", tileId++, null, "3C8990");*/
        }

        for (int i = 0; i < n; i++)
        {
            exampleField[(n / 2, i)] = new Tile("road",  null, "1c1c1c");
            exampleField[(i, n / 2)] = new Tile("road",  null, "1c1c1c");
            exampleField[(n / 2 - 1, i)] = new Tile("road",  null, "1c1c1c");
            exampleField[(i, n / 2 - 1)] = new Tile("road",  null, "1c1c1c");
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                exampleField[(n / 2 + i - 2, n / 2 + j - 2)] = new Tile("square",  null, "8c188a");
            }
        }
        exampleField[(15 + 8, 12 + 8)] = new Tile("square",  null, "8c188a");
        exampleField[(15 + 8, 13 + 8)] = new Tile("square",  null, "8c188a");
        exampleField[(13 + 8, 10 + 8)] = new Tile("square",  null, "8c188a");
        exampleField[(12 + 8, 10 + 8)] = new Tile("square", null, "8c188a");
        exampleField[(13 + 8, 15 + 8)] = new Tile("square",  null, "8c188a");
        exampleField[(12 + 8, 15 + 8)] = new Tile("square", null, "8c188a");
        exampleField[(10 + 8, 12 + 8)] = new Tile("square",  null, "8c188a");
        exampleField[(10 + 8, 13 + 8)] = new Tile("square", null, "8c188a");
        
        for (int i = n / 2 + 1; i < n - k - 1; i++)
        {
            exampleField[(k + 1, i)] = new Tile("road", null, "1c1c1c");
            exampleField[(k, i)] = new Tile("road",  null, "1c1c1c");
        }
        for (int i = 0; i < k + 1; i++)
        {
            exampleField[(i, n - k - 2)] = new Tile("road",  null, "1c1c1c");
            exampleField[(i, n - k - 1)] = new Tile("road",  null, "1c1c1c");
        }
        exampleField[(k + 1, n - k - 2)] = new Tile("turn",  null, "FFF000");
        exampleField[(k, n - k - 2)] = new Tile("turn",  null, "FFF000");
        exampleField[(k + 1, n - k - 1)] = new Tile("turn",  null, "FFF000");
        exampleField[(k, n - k - 1)] = new Tile("turn",  null, "FFF000");
        
        exampleField[(k + 1, n / 2)] = new Tile("cross",  null, "FFFFFF");
        exampleField[(k, n / 2)] = new Tile("cross",  null, "FFFFFF");
        exampleField[(k + 1, n / 2 - 1)] = new Tile("cross",  null, "FFFFFF");
        exampleField[(k, n / 2 - 1)] = new Tile("cross",  null, "FFFFFF");
        

        for (int i = n / 2 - 1; i >= k + 1; i--)
        {
            exampleField[(i, k + 1)] = new Tile("road", null, "1c1c1c");
            exampleField[(i, k)] = new Tile("road",  null, "1c1c1c");
        }
        for (int i = 0; i < k + 1; i++)
        {
            exampleField[(k + 1, i)] = new Tile("road",  null, "1c1c1c");
            exampleField[(k, i)] = new Tile("road", null, "1c1c1c");
        }
        exampleField[(k + 1, k + 1)] = new Tile("turn",  null, "FFF000");
        exampleField[(k, k + 1)] = new Tile("turn",  null, "FFF000");
        exampleField[(k + 1, k)] = new Tile("turn",  null, "FFF000");
        exampleField[(k, k)] = new Tile("turn",  null, "FFF000");
        
        exampleField[(n / 2, k + 1)] = new Tile("cross",null, "FFFFFF");
        exampleField[(n / 2 - 1, k + 1)] = new Tile("cross", null, "FFFFFF");
        exampleField[(n / 2, k)] = new Tile("cross",  null, "FFFFFF");
        exampleField[(n / 2 - 1, k)] = new Tile("cross",  null, "FFFFFF");
        
        for (int i = n - 1; i >= n - k - 1; i--)
        {
            exampleField[(i, k + 1)] = new Tile("road",  null, "1c1c1c");
            exampleField[(i, k)] = new Tile("road",  null, "1c1c1c");
        }
        for (int i = n / 2 - 1; i >= k + 1; i--)
        {
            exampleField[(n - k - 2, i)] = new Tile("road",  null, "1c1c1c");
            exampleField[(n - k - 1, i)] = new Tile("road",  null, "1c1c1c");
        }
        exampleField[(n - k - 2, k + 1)] = new Tile("turn",  null, "FFF000");
        exampleField[(n - k - 1, k + 1)] = new Tile("turn",  null, "FFF000");
        exampleField[(n - k - 2, k)] = new Tile("turn",  null, "FFF000");
        exampleField[(n - k - 1, k)] = new Tile("turn", null, "FFF000");
        
        exampleField[(n - k - 2, n / 2)] = new Tile("cross",  null, "FFFFFF");
        exampleField[(n - k - 1, n / 2)] = new Tile("cross",  null, "FFFFFF");
        exampleField[(n - k - 2, n / 2 - 1)] = new Tile("cross",  null, "FFFFFF");
        exampleField[(n - k - 1, n / 2 - 1)] = new Tile("cross",  null, "FFFFFF");
        
        for (int i = n - k - 1; i < n; i++)
        {
            exampleField[(n - k - 2, i)] = new Tile("road",  null, "1c1c1c");
            exampleField[(n - k - 1, i)] = new Tile("road",  null, "1c1c1c");
        }
        for (int i = n / 2; i < n - k - 1; i++)
        {
            exampleField[(i, n - k - 2)] = new Tile("road", null, "1c1c1c");
            exampleField[(i, n - k - 1)] = new Tile("road",  null, "1c1c1c");
        }
        exampleField[(n - k - 2, n - k - 2)] = new Tile("turn", null, "FFF000");
        exampleField[(n - k - 1, n - k - 2)] = new Tile("turn",  null, "FFF000");
        exampleField[(n - k - 2, n - k - 1)] = new Tile("turn", null, "FFF000");
        exampleField[(n - k - 1, n - k - 1)] = new Tile("turn",  null, "FFF000");
        
        exampleField[(n / 2, n - k - 2)] = new Tile("cross",  null, "FFFFFF");
        exampleField[(n / 2 - 1, n - k - 2)] = new Tile("cross",  null, "FFFFFF");
        exampleField[(n / 2, n - k - 1)] = new Tile("cross", null, "FFFFFF");
        exampleField[(n / 2 - 1, n - k - 1)] = new Tile("cross",  null, "FFFFFF");
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
    
    public static Dictionary<Vector3, Tile> GenerateFromInputField(Vector3 size, int seed, string inputFile)
    {
        var inputField = JsonManipulator.GetTilesSetFromFieldJson(inputFile);
        var tileManager = new TileManager(inputField);
        var function = new WaveFunction(size, tileManager, seed, false, false);
        function.Run();
        //JsonManipulator.SaveJsonResult(function.CurrState.Map.Result(), "generatedSimpleHouse.json");
        return function.CurrState.Map.Result();
    }
    
    public static Dictionary<Vector3, Tile> GenerateFromTestField(Vector3 size, int seed)
    {
        var inputField = CreateExampleField3D();
        var tileManager = new TileManager(inputField);
        var function = new WaveFunction(size, tileManager, seed, false, false);
        function.Run();
        //JsonManipulator.SaveJsonResult(function.CurrState.Map.Result(), "generatedSimpleHouse.json");
        return function.CurrState.Map.Result();
    }

    public static Dictionary<Vector3, Tile> BuildTest3DField()
    {
        var result = CreateExampleField3D();
        Save(result, "inputField3D.json");
        return result;
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
        var field = CreateExampleField2D2();
        var tileClusterSet = new TileManager(field, 2);
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
    
    public static Dictionary<Vector3, Tile> BuildJsonFile(String inputFile)
    {
        var inputField = JsonManipulator.GetTilesSetFromFieldJson(inputFile);
        return inputField;
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
        var house = BuildFromInputTileSet(new Vector3(size.X, size.Y, 10), seed, inputHouseFile);
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

    public static Dictionary<Vector3, Tile> GenerateStreetFromInputFieldFile(string inputStreetFile,
        string inputHouseFile, Vector2 min, Vector2 max, int seed)
    {
        Random random = new Random(seed);
        var inputField = JsonManipulator.GetTilesSetFromFieldJson2D(inputStreetFile);
        var foundations = BSP.GetFoundations(inputField, min, max, random);
        var street = From2Dto3D(inputField);
        foreach (var foundation in foundations)
        {
            GenerateHousesOnTheStreet2(street, foundation.Value, random.Next(), inputHouseFile);
        }
        Save(street, "street.json");
        return street;
    }
    
    public static void GenerateHousesOnTheStreet2(Dictionary<Vector3, Tile> map, List<Vector2> foundation, int seed, String inputHouseFile)
    {
        var maxPoint = foundation.Aggregate((0, 0),
            (max, pair) => (Math.Max(max.Item1, pair.X), Math.Max(max.Item2, pair.Y)));
        var minPoint = foundation.Aggregate((9999999999, 99999999999),
            (min, pair) => (Math.Min(min.Item1, pair.X), Math.Min(min.Item2, pair.Y)));
        var size = new Vector2((int)(maxPoint.Item1 - minPoint.Item1 + 1), (int)(maxPoint.Item2 - minPoint.Item2 + 1));
        var house = GenerateFromInputField(new Vector3(size.X, size.Y, 10), seed, inputHouseFile);
        var f = false;
        foreach (var cell in house)
        {
            if (!cell.Value.TileInfo.Name.Equals("air"))
                f = true;
            
            if (cell.Key.Z > 0 &&  !cell.Value.TileInfo.Name.Equals("air"))
                map[(cell.Key.X + (int)minPoint.Item1, cell.Key.Y + (int)minPoint.Item2, cell.Key.Z)] = cell.Value;
        }

        if (f == false)
            GenerateHousesOnTheStreet2(map, foundation, seed + 1, inputHouseFile);
    }
}