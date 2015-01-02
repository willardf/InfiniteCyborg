using InfCy.Genetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfCy.GameCore
{
    enum ComponentTypes { Mod, Ability }

    class Component
    {
        BitField data;

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
        protected readonly static BitSet ComponentTypeBits = new BitSet(WeightBits.End, 4); // Up to 16 types
        public readonly static int BitCount = ComponentTypeBits.End;


        public Component()
        {
            data = new BitField(BitCount);
            Name = "Null";

            BaseDamage = 0;
            Push = 0;
            Speed = 0;
        }

        public string Name { get; set; }
        public int BaseDamage { get { return (int)(data[DamageBits]); } set { data[DamageBits] = value; } }
        public int TotalDamage { get { return (int)(BaseDamage * Weapon.EleMod(Element)); } }
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
        public ComponentTypes ComponentType { get { return (ComponentTypes)data[ComponentTypeBits]; } set { data[ComponentTypeBits] = (long)value; } }
        public byte Weight { get { return (byte)data[WeightBits]; } set { data[WeightBits] = value; } }
    }
}
