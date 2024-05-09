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
        public TileInfo[] TilesInfo { get; set; }
        public bool XSymmetry { get; set; }
        public bool YSymmetry { get; set; }
    }
}
