using InfCy.Genetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    /// <summary>
    /// Elec/Earth, Fire/Water, Air/Vacuum
    /// Elec + Fire = Plasma
    /// Water + Air = Vapor
    /// 6 Primary and 2 derivatives
    /// </summary>
    public class Elements : BitField
    {
        public const int FieldSize = 16;

        private static readonly Tribit ElectricBits = new Tribit(0);
        private static readonly Tribit FireBits = new Tribit(ElectricBits.End);
        private static readonly Tribit AirBits = new Tribit(FireBits.End);

        public bool Electric { get { return this[ElectricBits].True(); } }
        public bool Earth { get { return this[ElectricBits].False(); } }

        public bool Fire { get { return this[FireBits].True(); } }
        public bool Water { get { return this[FireBits].False(); } }

        public bool Air { get { return this[AirBits].True(); } }
        public bool Vacuum { get { return this[AirBits].False(); } }

        public bool Plasma { get { return Electric && Fire; } }
        public bool Vapor { get { return Water && Air; } }

        public Elements() : base(FieldSize) { }

        public Elements(long bits)
            : base(FieldSize)
        {
            this[0, FieldSize] = bits;
        }

        public Elements(BitField bits)
            : base(FieldSize)
        {
            this[0, FieldSize] = bits[0, FieldSize];
        }
        
        public static implicit operator Elements(long v)
        {
            return new Elements(v);
        }

        public static implicit operator long(Elements v)
        {
            return v[0, FieldSize];
        }

        public override string ToString()
        {
            HashSet<string> props = new HashSet<string>();

            if (this.Air)
            {
                if (this.Vapor) props.Add("Vapor");
                else props.Add("Air");
            }
            else if (this.Vacuum) props.Add("Vacuum");
            
            if (this.Fire)
            {
                if (this.Plasma) props.Add("Plasma");
                else props.Add("Fire");
            }
            else if (this.Water)
            {
                if (this.Vapor) props.Add("Vapor");
                else props.Add("Water");
            }

            if (this.Electric)
            {
                if (this.Plasma) props.Add("Plasma");
                else props.Add("Electric");
            } if (this.Earth) props.Add("Earth");

            return string.Join("/", props);
        }
    }
}
