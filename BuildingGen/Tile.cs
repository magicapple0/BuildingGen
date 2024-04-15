using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingGen
{
    public class Tile
    {
        public TileInfo TileInfo { get; set; }
        public string[][] ModifiedEdges { get; set; }
        public List<TileModifiers> TileModifiers { get; set; }
         
        public Tile(TileInfo tileInfo, string[][] modifiedEdges, List<TileModifiers> tileModifiers) {
            TileInfo = tileInfo;
            ModifiedEdges = modifiedEdges;
            TileModifiers = tileModifiers;
        }
    }
}
