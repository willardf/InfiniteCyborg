using InfCy.Genetics;
using InfCy.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    public enum Elements { None = 0, Electric, Fire, Water, Air }

    class Weapon
    {
        BitField data;

        private List<Component> SetComponents { get; set; }

        protected readonly static BitSet DamageBits = new BitSet(0, 6, true); // -32 - 31
        protected readonly static Bit MalfunctionBits = new Bit(DamageBits.End);
        protected readonly static BitSet ElementBits = new BitSet(MalfunctionBits.End, 4); // Up to 16 types
        protected readonly static BitSet PushBits = new BitSet(ElementBits.End, 5, true); // -16 - 15
        protected readonly static Bit MeleeBits = new Bit(PushBits.End);
        protected readonly static BitSet RangeBits = new BitSet(MeleeBits.End, 5); // 0 - 31
        protected readonly static BitSet CoolDownBits = new BitSet(RangeBits.End, 5); // 0 - 31
        protected readonly static BitSet CoolDownLeftBits = new BitSet(RangeBits.End, 5); // 0 - 31
        protected readonly static Bit NeedsAmmoBits = new Bit(CoolDownLeftBits.End);
        protected readonly static BitSet AmmoBits = new BitSet(NeedsAmmoBits.End, 8); // 0 - 255
        protected readonly static BitSet AmmoLeftBits = new BitSet(AmmoBits.End, 8); // 0 - 255
        protected readonly static BitSet SpeedBits = new BitSet(AmmoLeftBits.End, 7); // 0 - 128
        protected readonly static BitSet WeightBits = new BitSet(SpeedBits.End, 8); // 0 - 255
        protected readonly static BitSet ComponentBits = new BitSet(WeightBits.End, 4); // 0 - 16

        public readonly static int BitCount = ComponentBits.End;

        public Weapon()
            : this("First", new BitField(BitCount))
        {
            Name = "Fist";
            Melee = true;
            BaseDamage = 1;
            Push = 0;
            Speed = 1;
        }

        public Weapon(string name, BitField d)
        {
            SetComponents = new List<Component>();
            Name = name;
            data = d;
        }

        public void Use()
        {
            if (NeedsAmmo)
            {
                AmmoLeft--;
            }

            CoolDownLeft = CoolDown;
        }

        public bool Usable()
        {
            if (CoolDownLeft == 0 && AmmoLeft > 0)
            {
                return true;
            }

            return false;
        }

        internal bool CheckRange(int srcX, int srcY, int destX, int destY)
        {
            return (IntVector.Distance(srcX, srcY, destX, destY) <= Range + .01);
        }

        public static float EleMod(Elements e)
        {
            if (e == Elements.Air)
            {
                return .5f; // Air converts 50% damage into push
            }

            return 1;
        }

        public static int WeaponLevel (BitField b)
        {
            // One factor (0, 1) to a line makes counting easier
            const int MaxLevel = 100;
            const float NumFactors = 5;
            float factors =
             Math.Abs(b[DamageBits]) * b[SpeedBits] / (float)-DamageBits.MinValue // We know that Damage is unsigned and -Min > Max
             + Math.Abs(b[PushBits]) / (float)-PushBits.MinValue
             + (1 - b[CoolDownBits] / (float)CoolDownBits.MaxValue)
             + b[SpeedBits] / (float)SpeedBits.MaxValue;

            return (int)(factors / NumFactors * MaxLevel);
        }

        public string Name { get; set; }
        public int BaseDamage { get { return (int)(data[DamageBits]); } set { data[DamageBits] = value; } }
        public int TotalDamage { get { return (int)(BaseDamage * EleMod(Element)) + SetComponents.Sum(c => c.TotalDamage); } }
        public Elements Element { get { return (Elements)data[ElementBits]; } set { data[ElementBits] = (long)value; } }
        public bool Malfunctioning { get { return data[MalfunctionBits.Start]; } set { data[MalfunctionBits.Start] = value; } }
        public bool Melee { get { return data[MeleeBits.Start]; } set { data[MeleeBits.Start] = value; } }
        public int Push { get { return (int)data[PushBits]; } set { data[PushBits] = value; } }
        public byte Range { get { return (byte)data[RangeBits]; } set { data[RangeBits] = value; } }
        public byte CoolDown { get { return (byte)data[CoolDownBits]; } set { data[CoolDownBits] = value; } }
        public byte CoolDownLeft { get { return (byte)data[CoolDownLeftBits]; } set { data[CoolDownLeftBits] = value; } }
        public bool NeedsAmmo { get { return data[NeedsAmmoBits.Start]; } set { data[NeedsAmmoBits.Start] = value; } }
        public byte Ammo { get { return (byte)data[AmmoBits]; } set { data[AmmoBits] = value; } }
        public byte AmmoLeft { get { return (byte)data[AmmoLeftBits]; } set { data[AmmoLeftBits] = value; } }
        public int Speed { get { return (int)data[SpeedBits]; } set { data[SpeedBits] = value; } }
        public byte Weight { get { return (byte)data[WeightBits]; } set { data[WeightBits] = value; } }
        public int TotalWeight { get { return Weight + SetComponents.Sum(c => c.Weight); } }
        public byte MaxComponents { get { return (byte)data[ComponentBits]; } set { data[ComponentBits] = value; } }

        public override string ToString()
        {
            return string.Format("{0} - {1} Dmg", this.Name, TotalDamage);
        }

        public static readonly Weapon Fists = new Weapon();
    }
}
