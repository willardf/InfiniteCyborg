using InfCy.Genetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.GameCore
{
    public class Component : Item
    {
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
        public readonly static int BitCount = WeightBits.End;

        public Component()
            : base(new BitField(BitCount))
        {
            BaseDamage = 0;
            Push = 0;
            Speed = 0;
        }

        public Component(BitField d)
            : base(d)
        {
        }
        
        public int BaseDamage { get { return (int)(data[DamageBits]); } set { data[DamageBits] = value; } }
        public int TotalDamage { get { return (int)(BaseDamage * Weapon.EleMod(Element)); } }
        public Elements Element { get { return (Elements)data[ElementBits]; } set { data[ElementBits] = (long)value; } }
        public bool Malfunctioning { get { return data[MalfunctionBits]; } set { data[MalfunctionBits] = value; } }
        public int Push { get { return (int)data[PushBits]; } set { data[PushBits] = value; } }
        public bool Melee { get { return data[MeleeBits]; } set { data[MeleeBits] = value; } }
        public byte Range { get { return (byte)data[RangeBits]; } set { data[RangeBits] = value; } }
        public byte CoolDown { get { return (byte)data[CoolDownBits]; } set { data[CoolDownBits] = value; } }
        public byte CoolDownLeft { get { return (byte)data[CoolDownLeftBits]; } set { data[CoolDownLeftBits] = value; } }
        public bool NeedsAmmo { get { return data[NeedsAmmoBits]; } set { data[NeedsAmmoBits] = value; } }
        public byte Ammo { get { return (byte)data[AmmoBits]; } set { data[AmmoBits] = value; } }
        public byte AmmoLeft { get { return (byte)data[AmmoLeftBits]; } set { data[AmmoLeftBits] = value; } }
        public int Speed { get { return (int)data[SpeedBits]; } set { data[SpeedBits] = value; } }
        public override int Weight { get { return (int)data[WeightBits]; } set { data[WeightBits] = value; } }
        public override string Name
        {
            get
            {
                return string.Format("{0}{1}-Type Component", this.Element.ToShortString(), this.Melee ? "M" : "R");
            }
        }

        public int Level
        {
            get
            {
                // One factor (0, 1) to a line makes counting easier
                const int MaxLevel = 100;
                const float NumFactors = 5;
                float factors =
                 Math.Abs(this.BaseDamage) * this.Speed / (float)-DamageBits.MinValue // We know that Damage is unsigned and -Min > Max
                 + Math.Abs(this.Push) / (float)-PushBits.MinValue
                 + (1 - this.CoolDown / (float)CoolDownBits.MaxValue)
                 + this.Speed / (float)SpeedBits.MaxValue;

                return (int)(factors / NumFactors * MaxLevel);
            }
        }

        public override void Draw(Camera root)
        {
            root.setChar(X, Y, '3');
        }

        public override void DrawInfo(Camera root, int y)
        {
            root.print(1, y, "{0}", this);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
