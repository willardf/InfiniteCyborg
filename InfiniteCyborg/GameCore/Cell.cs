using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.GameCore
{
    class Cell
    {
        public enum BlockType { None, Half, Full }

        public char DrawChar { get; set; }
        public bool Transparent { get; set; }
        public BlockType Blocking { get; set; }

        public Cell()
        {
            DrawChar = '#';
            Blocking = BlockType.Full;
        }
    }
}
