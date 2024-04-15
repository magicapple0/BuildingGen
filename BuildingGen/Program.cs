using BuildingGen;
using System;
using System.IO;
using System.Text.Json;

class Program
{
    public static void Main()
    {
        var confLoader = new ConfigLoader("1.json");
        var tiles = confLoader.Tiles;
        var dict = new Dictionary<string, List<string>>();
        foreach (var tile in confLoader.Tiles)
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
            if (!dict.ContainsKey(edges))
            {
                dict[edges] = new List<string> { modifiers };
            }
            else
                dict[edges].Add(modifiers);
            Console.Write(modifiers + "\t");
            Console.WriteLine();
        }
    }
}