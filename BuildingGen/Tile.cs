namespace BuildingGen
{
    public class Tile
    {
        public TileInfo TileInfo { get; set; }
        public string[][] ModifiedEdges { get; set; }
        public string[]? ModifiedTextures { get; set; }
        public List<TileModifiers> Modifiers { get; set; }

        public Tile(TileInfo tileInfo) {
            TileInfo = tileInfo;
            ModifiedEdges = (string[][])tileInfo.Edges.Clone();
            Modifiers = new List<TileModifiers>();
            if (tileInfo.Texture != null) ModifiedTextures = (string[])tileInfo.Texture.Clone();
        }

        public void FlipX()
        {
            ModifiedEdges = new [] { ModifiedEdges[0], ModifiedEdges[1], ModifiedEdges[2], ModifiedEdges[4], ModifiedEdges[3], ModifiedEdges[5] };
            if (ModifiedTextures != null) ModifiedTextures = new [] { ModifiedTextures[0], ModifiedTextures[1], ModifiedTextures[2], ModifiedTextures[4], ModifiedTextures[3], ModifiedTextures[5] };
            Modifiers.Add(TileModifiers.FlipX);
        }
        
        public void RotateZTile()
        {
            ModifiedEdges = new [] { ModifiedEdges[0], ModifiedEdges[3], ModifiedEdges[2], ModifiedEdges[5], ModifiedEdges[1], ModifiedEdges[4] };
            if (ModifiedTextures != null) ModifiedTextures = new [] { ModifiedTextures[0], ModifiedTextures[3], ModifiedTextures[2], ModifiedTextures[5], ModifiedTextures[1], ModifiedTextures[4] };
            Modifiers.Add(TileModifiers.RotateZ);
        }

        public Tile Copy()
        {
            var copy = new Tile(TileInfo)
            {
                ModifiedEdges = (string[][])ModifiedEdges.Clone(),
                Modifiers = new List<TileModifiers>(Modifiers.ToArray()),
                ModifiedTextures = (string[]?)ModifiedTextures?.Clone()
            };
            return copy;
        }
    }
}
