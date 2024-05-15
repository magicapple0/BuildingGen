using System.Text.Json;

namespace BuildingGen
{
    public class JsonManager
    {
        private InputJson? InputJson { get; set; }
        public TileInfo[] TilesInfos { get; private set; }
        public bool XSymmetry { get; private set; }
        public bool YSymmetry { get; private set; }

        public JsonManager(string fileName)
        {
            InputJson = JsonSerializer.Deserialize<InputJson>(File.ReadAllText(fileName));
            XSymmetry = InputJson.XSymmetry;
            YSymmetry = InputJson.YSymmetry;
            TilesInfos = InputJson.TilesInfo.ToArray();
        }

        public static void SaveJsonResult(Dictionary<Vector3, Tile> result, string path)
        {
            var json = JsonSerializer.Serialize(result.ToArray());
            File.WriteAllText(path, json);
        }

    }
}
