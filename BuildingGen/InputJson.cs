using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingGen
{
    public class InputJson
    {
        public int[][] Base { get; set; }
        public string[] UsingTiles { get; set; }
        public TileInfo[] TilesInfo { get; set; }
    }
}
