using InfCy.Genetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.GameCore
{
    class Component : Weapon
    {
        BitField data;

        public readonly static int BitCount = 1;//ComponentBits.End;


        public Component()
        {
            data = new BitField(BitCount);
            Name = "Null";

            BaseDamage = 0;
            Push = 0;
            Speed = 0;
        }
    }
}
