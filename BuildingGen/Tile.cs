namespace BuildingGen
{
    public class Tile
    {
        private static int _idCounter;
        
        private string[][] _modifiedEdges;
        public string StringRep { get; private set; }
        public TileInfo TileInfo { get; set; }
        public int Id { get; set; }

        public string[][] ModifiedEdges
        {
            get => _modifiedEdges;
            set
            {
                _modifiedEdges = value;
                StringRep = GetStringRep();
            }
        }

        public string[]? ModifiedTextures { get; set; }
        public List<TileModifiers> Modifiers { get; set; }

        public Tile() { }

        public Tile(TileInfo tileInfo, int id = -1) {
            Id = id >= 0 ? id : _idCounter++;
            TileInfo = tileInfo;
            ModifiedEdges = (string[][])tileInfo.Edges?.Clone();
            Modifiers = new List<TileModifiers>();
            if (tileInfo.Texture != null) ModifiedTextures = (string[])tileInfo.Texture.Clone();
            
        }
        
        public Tile(String name, String[] textures, string color, int id = -1)
        {
            Id = id >= 0 ? id : _idCounter++;
            TileInfo = new TileInfo(name, textures, color);
            ModifiedEdges = null;
            Modifiers = new List<TileModifiers>();
            if (TileInfo.Texture != null) ModifiedTextures = (string[])TileInfo.Texture.Clone();
        }

        public void FlipX()
        {
            ModifiedEdges = new [] { ModifiedEdges[0], ModifiedEdges[1], ModifiedEdges[2], ModifiedEdges[4], ModifiedEdges[3], ModifiedEdges[5] };
            if (ModifiedTextures != null) ModifiedTextures = new [] { ModifiedTextures[0], ModifiedTextures[1], ModifiedTextures[2], ModifiedTextures[4], ModifiedTextures[3], ModifiedTextures[5] };
            Modifiers.Add(TileModifiers.FlipX);
        }
        
        public void FlipY()
        {
            ModifiedEdges = new [] { ModifiedEdges[0], ModifiedEdges[5], ModifiedEdges[2], ModifiedEdges[3], ModifiedEdges[4], ModifiedEdges[1] };
            if (ModifiedTextures != null) ModifiedTextures = new [] { ModifiedTextures[0], ModifiedTextures[5], ModifiedTextures[2], ModifiedTextures[3], ModifiedTextures[4], ModifiedTextures[1] };
            Modifiers.Add(TileModifiers.FlipY);
        }
        
        public void RotateZTile()
        {
            if (ModifiedEdges != null) ModifiedEdges = new [] { ModifiedEdges[0], ModifiedEdges[3], ModifiedEdges[2], ModifiedEdges[5], ModifiedEdges[1], ModifiedEdges[4] };
            if (ModifiedTextures != null) ModifiedTextures = new [] { ModifiedTextures[0], ModifiedTextures[3], ModifiedTextures[2], ModifiedTextures[5], ModifiedTextures[1], ModifiedTextures[4] };
            Modifiers.Add(TileModifiers.RotateZ);
        }

        public Tile Copy()
        {
            var copy = new Tile(TileInfo, Id)
            {
                ModifiedEdges = (string[][])ModifiedEdges.Clone(),
                Modifiers = new List<TileModifiers>(Modifiers.ToArray()),
                ModifiedTextures = (string[]?)ModifiedTextures?.Clone()
            };
            return copy;
        }

        public override int GetHashCode()
        {
            return (int)(TileInfo.Name.GetHashCode() + ModifiedEdges.Sum(edge => (long)edge[0].GetHashCode()));
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Tile tile))
                return false;
            return StringRep == tile.StringRep;
            /*if (obj.GetHashCode() != GetHashCode())
                return false;
            if (!((Tile) obj).TileInfo.Name.Equals(TileInfo.Name))
                return false;
            for (var i = 0; i < 6; i++)
            {
                if (ModifiedEdges[i].Length != (tile).ModifiedEdges[i].Length)
                    return false;
                for (var j = 0; j < ModifiedEdges[i].Length; j++)
                    if (!ModifiedEdges[i][j].Equals(tile.ModifiedEdges[i][j]))
                        return false;
            }*/

            return true;
        }

        public string GetStringRep()
        {
            if (ModifiedEdges == null)
                return $"{TileInfo.Name}:null";
            return $"{TileInfo.Name}:{string.Join('/', ModifiedEdges.Select(x => string.Join(',', x)))}";
        }
    }
}
