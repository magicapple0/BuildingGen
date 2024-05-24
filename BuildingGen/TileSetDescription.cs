namespace BuildingGen
{
    public class TileSetDescription
    {
        public int[][] Base { get; set; }
        public TileInfo[] TilesInfo { get; set; }
        public bool XSymmetry { get; set; }
        public bool YSymmetry { get; set; }
    }
}
