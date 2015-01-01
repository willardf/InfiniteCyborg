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

        // TODO: Weapons have component hardpoints (Which might be "weapons")

        private const int DmgShift = 10;
        private readonly static BitSet DamageBits = new BitSet(0, 5, true); // -6 - 25
        private readonly static Bit MalfunctionBits = new Bit(DamageBits.End);
        private readonly static BitSet ElementBits = new BitSet(MalfunctionBits.End, 4); // Up to 16 types
        private readonly static BitSet PushBits = new BitSet(ElementBits.End, 5, true); // -16 - 15
        private readonly static Bit MeleeBits = new Bit(PushBits.End);
        private readonly static BitSet RangeBits = new BitSet(MeleeBits.End, 5); // 0 - 31
        private readonly static BitSet CoolDownBits = new BitSet(RangeBits.End, 5); // 0 - 31
        private readonly static BitSet CoolDownLeftBits = new BitSet(RangeBits.End, 5); // 0 - 31
        private readonly static Bit NeedsAmmoBits = new Bit(CoolDownLeftBits.End);
        private readonly static BitSet AmmoBits = new BitSet(NeedsAmmoBits.End, 8); // 0 - 255
        private readonly static BitSet AmmoLeftBits = new BitSet(AmmoBits.End, 8); // 0 - 255
        private readonly static BitSet SpeedBits = new BitSet(AmmoLeftBits.End, 8, true); // -128 - 127

        public Weapon()
        {
            data = new BitField(AmmoLeftBits.End);
            Name = "Unnamed";
        }

        public Weapon(string name, BitField d)
        {
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

        private static float EleMod(Elements e)
        {
            if (e == Elements.Air)
            {
                return .5f; // Air converts 50% damage into push
            }

            return 1;
        }

        public string Name { get; set; }
        public int BaseDamage { get { return (int)(data[DamageBits] + DmgShift); } set { data[DamageBits] = value - DmgShift; } }
        public int Damage { get { return (int)(BaseDamage * EleMod(Element)); } }
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
        public int Speed { get { return (byte)data[SpeedBits]; } set { data[SpeedBits] = value; } }

        public override string ToString()
        {
            return string.Format("{0} - {1} Dmg", this.Name, Damage);
        }

        public static readonly Weapon Fists = new Weapon() { Name = "Fists", BaseDamage = 1, Melee = true };

        internal bool checkRange(int srcX, int srcY, int destX, int destY)
        {
            return (IntVector.Distance(srcX, srcY, destX, destY) <= Range + .01);
        }

        internal void attack(Mover attacker, Mover defender)
        {
            Logger.Log("{0} dealt {1} damage to {2}", attacker.Name, Damage, defender.Name);
            defender.TakeDamage(this.Damage, attacker);
            if (Push != 0)
            {
                var vec = (defender.Position - attacker.Position);
                vec.Norm();
                defender.Push(vec, Push, attacker);
            }
        }
    }
}
