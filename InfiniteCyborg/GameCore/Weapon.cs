using InfCy.Anim;
using InfCy.Genetics;
using InfCy.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfCy.GameCore
{
    public class Weapon : Item
    {
        private List<Component> SetComponents { get; set; }

        protected readonly static BitSet ComponentBits = new BitSet(0, 3); // 0 - 8

        public readonly static int BitCount = ComponentBits.End;

        public Weapon()
            : this("Fist", new BitField(1))
        {
            Name = "Fist";
        }

        public Weapon(string name, BitField d) : base(d)
        {
            SetComponents = new List<Component>();
            Name = name;
            data = d;
        }

        public Animation Use()
        {
            if (NeedsAmmo)
            {
                AmmoLeft--;
            }

            this.SetComponents.ForEach(c => c.CoolDownLeft = c.CoolDown);

            if (this.Melee)
            {
                return new Animation(0);
            }
            else
            {
                // Create projectile and stuff
                return new Animation(1);
            }
        }

        public bool Usable()
        {
            if (CoolDownLeft == 0 && (!NeedsAmmo || AmmoLeft > 0))
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
            if (e.Air)
            {
                return .5f; // Air and children converts 50% damage into push
            }

            return 1;
        }

        public int Level { get { return this.SetComponents.Sum(c => c.Level); } }

        public int BaseDamage { get { return 1; } }
        public int TotalDamage { get { return (int)(BaseDamage * EleMod(Element)) + SetComponents.Sum(c => c.TotalDamage); } }
        public Elements Element { get { return new Elements(this.SetComponents.Aggregate<Component, BitField>(new BitField(Elements.FieldSize), (a, b) => a | b.Element)); } }
        public bool Malfunctioning { get { return this.SetComponents.Any(c => c.Malfunctioning); } }
        public bool Melee { get { return Range <= 1; } }
        public int Push { get { return this.SetComponents.Sum(c => c.Push); } }
        public byte Range { get { return this.SetComponents.Count == 0 ? (byte)1 : this.SetComponents.Max(c => c.Range); } }
        public byte CoolDown { get { return this.SetComponents.Count == 0 ? (byte)0 : this.SetComponents.Max(c => c.CoolDown); } }
        public byte CoolDownLeft { get { return this.SetComponents.Count == 0 ? (byte)0 : this.SetComponents.Max(c => c.CoolDownLeft); } }
        public bool NeedsAmmo { get { return this.SetComponents.Any(c => c.NeedsAmmo); } }
        public int Ammo { get { return this.SetComponents.Sum(c => c.Ammo); } }
        public int AmmoLeft
        {
            get { return this.SetComponents.Sum(c => c.AmmoLeft); }
            set {
                var split = this.SetComponents.Count(c => c.Ammo > 0);
                var perComp = value / (float)split;
                var cnt = value;
                foreach (var comp in this.SetComponents)
                {
                    if (comp.Ammo > 0)
                    {
                        var val = Math.Min(perComp, cnt);
                        comp.AmmoLeft = (byte)Math.Min(comp.Ammo, val);
                        cnt -= comp.AmmoLeft;
                    }

                    if (cnt <= 0) break;
                }
            }
        }
        public int Speed { get { return this.SetComponents.Min(c => c.Speed) + 64; } }
        public override int Weight { get { return 1 + SetComponents.Sum(c => c.Weight); } set { throw new NotImplementedException(); } }
        public byte MaxComponents { get { return (byte)data[ComponentBits]; } set { data[ComponentBits] = value; } }

        public override string ToString()
        {
            return string.Format("{0} - {1} Dmg ({2})", this.Name, this.TotalDamage, this.Element);
        }

        public static readonly Weapon Fists = new Weapon();

        public override void Draw(Camera root)
        {
            root.setChar(X, Y, '2');
        }

        public override void DrawInfo(Camera root, int y)
        {
            
        }
    }
}
