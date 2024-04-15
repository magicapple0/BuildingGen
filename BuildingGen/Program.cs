using BuildingGen;
using System;
using System.IO;
using System.Text.Json;

class Program
{
    public static void Main()
    {
        //var tiles = loadConfig(true);
        //var cubes = new List<List<int>>() { new List<int>{ 1 } };
        //var window = new WindowCore(cubes);
        //window.Run();
    }

    private static List<Tile> loadConfig(bool write)
    {
        var confLoader = new ConfigLoader("1.json");
        var tiles = confLoader.Tiles;
        if (write)
            writeTiles(tiles);
        return tiles;
    }

    private static void writeTiles(List<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            var edges = "";
            Console.Write(tile.TileInfo.Name + "\t");
            foreach (var edge in tile.ModifiedEdges)
                edges += edge[0] + " ";
            Console.Write(edges + "\t");
            var modifiers = "";
            if (tile.TileModifiers.Count == 0)
                modifiers = "None";
            foreach (var modifier in tile.TileModifiers)
                modifiers += modifier.ToString() + " ";
            Console.Write("||\t" + modifiers + "\t");
            Console.WriteLine();
        }
    }
}