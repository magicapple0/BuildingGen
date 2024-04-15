using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualize
{
    public class LineManager
    {
        public ushort[] LineIndices { get; set; }
        ushort indexOffset = 0;

        public LineManager()
        {
            LineIndices = new ushort[0];
        }

        public void addLines(ushort[] lineIndeces)
        {
            ushort maxIndex = 0;
            var newIndeces = new List<ushort>();
            foreach (ushort i in lineIndeces)
            {
                newIndeces.Add((ushort)(i + indexOffset));
                maxIndex = Math.Max(maxIndex, i);
            }
            indexOffset += (ushort)(maxIndex + 1);
            LineIndices = LineIndices.Concat(newIndeces).ToArray();
        }
    }
}
