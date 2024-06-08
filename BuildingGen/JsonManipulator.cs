using System.Text.Json;

namespace BuildingGen
{
    public static class JsonManipulator
    {
        public static TileSetDescription GetTileSetDescriptionFromJson(string fileName)
        {
            return JsonSerializer.Deserialize<TileSetDescription>(File.ReadAllText(fileName));
        }

        public static Dictionary<Vector3, Tile> GetTilesSetFromFieldJson(string fileName)
        {
            var inputJson = JsonSerializer.Deserialize<List<FieldCellJson>>(File.ReadAllText(fileName));
            var dic = new Dictionary<Vector3, Tile>();
            foreach (var cell in inputJson)
                dic[cell.Key] = cell.Value;

            return dic;
        }
        
        public static Dictionary<Vector2, Tile> GetTilesSetFromFieldJson2D(string fileName)
        {
            var inputJson = JsonSerializer.Deserialize<List<FieldCellJson2D>>(File.ReadAllText(fileName));
            var dic = new Dictionary<Vector2, Tile>();
            foreach (var cell in inputJson)
                dic[cell.Key] = cell.Value;

            return dic;
        }
        
        public static void SaveJsonResult(Dictionary<Vector3, Tile> result, string path)
        {
            var json = JsonSerializer.Serialize(result.ToArray());
            File.WriteAllText(path, json);
        }

    }
}
