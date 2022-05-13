using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace IERG3080PartII.Model
{
    // By HUI, Sze Ho Forster

    public class PokemonFactory // A list of all Pokemons Available
    {
        protected static List<PokemonTemplate> PokemonDatabase = new List<PokemonTemplate>();

        public static void InitDatabase()
        {
            _ = new Pikachu();
            _ = new IERG_Student();
            _ = new FireDragon();
            _ = new Elsa();
            _ = new Killer();
        }

        public static List<PokemonTemplate> accessDatabase
        {
            get
            {
                return PokemonDatabase;
            }
        }
    }

    public abstract class PokemonTemplate : PokemonFactory, ICloneable
    {
        protected enum PokemonType { Electric, Ice, Fire, CUHK }

        protected int ID;
        protected string Name;
        protected int HP; 
        protected int MP;
        protected int MaxHP;
        protected int MaxMP;
        protected PokemonType type;
        protected User owner;

        public delegate void AttackAction(PokemonTemplate p1);
        public delegate void IdleAction(PokemonTemplate p1);

        protected List<AttackAction> attackActions = new List<AttackAction>();
        protected List<IdleAction> idleActions = new List<IdleAction>();

        protected PokemonTemplate() {
            attackActions.Add(Bite);
            attackActions.Add(Punch);

            this.AddToDatabase();
        }

        public string getID {
            get {
                return ID.ToString();
            }
        }
        public string getName {
            get {
                return Name;
            }
            set {
                Name = value;
            }
        }

        public int GetHP {
            get { return HP; }
            set {
                if (value < HP)
                    HP = value;
            }
        }

        public int GetMP {
            get { return MP; }
            set {
                if (value < HP)
                    MP = value;
            }
        }
        public int GetMaxHP { get { return MaxHP; } }
        public int GetMaxMP { get { return MaxMP; } }
        public string getType { get { return type.ToString(); } }

        public List<AttackAction> getAttacks
        {
            get
            {
                return attackActions;
            }
        }

        public List<IdleAction> getIdles
        {
            get
            {
                return idleActions;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public static void CapturePokemon(User owner, PokemonTemplate p1)
        {
            p1.owner = owner;
        }
        public static void ReleasePokemon(PokemonTemplate p1)
        {
            p1.owner = null;
        }

        public virtual void Bite(PokemonTemplate enemy)
        {
            enemy.HP -= 50;
        }

        public virtual void Punch(PokemonTemplate enemy)
        {
            enemy.HP -= 20;
        }

        public virtual void RecoverHP(int value)
        {
            // Pokemon can recover health
            if (this.HP + value < this.MaxHP)
                this.HP += value;
            else
                this.HP = this.MaxHP;
        }

        public virtual void RecoverMP(int value)
        {
            // Pokemon can recover MP
            if (this.MP + value < this.MaxMP)
                this.MP += value;
            else
                this.MP = this.MaxMP;
        }

        public virtual void PowerUp()
        {
            this.HP = this.MaxHP + 200;
            this.MP = this.MaxMP + 200;
        }

        public void Evolve()
        {
            this.Name = "Super " + this.ToString();
            this.HP *= 2;
            this.MaxHP = this.HP;
            this.MP *= 2;
            this.MaxMP = this.MP;
        }
        public void Rename(string newName)
        {
            this.Name = newName;
        }

        protected void AddToDatabase()
        {
            if (!PokemonDatabase.Exists(obj => obj.GetType().Name == this.GetType().Name))
                PokemonDatabase.Add(this);
        }
        public abstract object Clone();
    }

    public class Pikachu : PokemonTemplate
    {
        public Pikachu()
        {
            this.ID = 1;
            this.Name = "Pikachu";
            this.HP = 200;
            this.MP = 100;
            this.MaxHP = this.HP;
            this.MaxMP = this.MP;
            this.type = PokemonType.Electric;
            this.owner = null;

            attackActions.Add(HundredThousandVolts);
        }

        private static Pikachu newPikachu = new Pikachu();

        public void HundredThousandVolts(PokemonTemplate enemy)
        {
            if (this.MP >= 50)
            {
                enemy.GetHP -= 100;
                this.MP -= 50;
            }
            // attack specific for Pikachu
        }

        public override object Clone()
        {
            Pikachu newPikachuCopy = newPikachu.MemberwiseClone() as Pikachu;
            return newPikachuCopy;
        }
    }

    public class IERG_Student : PokemonTemplate
    {
        public IERG_Student()
        {
            this.ID = 2;
            this.Name = "IERG Student";
            this.HP = 400;
            this.MP = 300;
            this.MaxHP = this.HP;
            this.MaxMP = this.MP;
            this.type = PokemonType.CUHK;
            this.owner = null;

            attackActions.Add(Coding);
            idleActions.Add(Studying);
        }

        private static IERG_Student newIERG_Student = new IERG_Student();

        public void Coding(PokemonTemplate enemy)
        {
            if (this.MP >= 80)
            {
                enemy.GetHP -= 60;
                this.MP -= 80;
            }
        }

        public void Studying(PokemonTemplate _)
        {
            if (this.MP + 100 <= this.MaxMP)
                this.MP += 100;
            else
                this.MP = this.MaxMP;
            // a boosting method
        }

        public override object Clone()
        {
            IERG_Student newIERG_StudentCopy = newIERG_Student.MemberwiseClone() as IERG_Student;
            return newIERG_StudentCopy;
        }
    }

    public class FireDragon : PokemonTemplate
    {
        public FireDragon()
        {
            this.ID = 3;
            this.Name = "Fire Dragon";
            this.HP = 600;
            this.MP = 400;
            this.MaxHP = this.HP;
            this.MaxMP = this.MP;
            this.type = PokemonType.Fire;
            this.owner = null;

            attackActions.Add(FireballShooting);
            attackActions.Add(Shouting);
            idleActions.Add(Powering);

        }

        private static FireDragon newDragon = new FireDragon();

        public void FireballShooting(PokemonTemplate enemy)
        {
            if (this.MP >= 150)
            {
                enemy.GetHP -= 150;
                this.MP -= 150;
            }
        }

        public void Shouting(PokemonTemplate enemy)
        {
            if (this.MP >= 80)
            {
                enemy.GetHP -= 100;
                this.MP -= 80;
            }
        }

        public void Powering(PokemonTemplate _)
        {
            if (this.MP + 100 <= this.MaxMP)
                this.MP += 100;
            else
                this.MP = this.MaxMP;
        }

        public override object Clone()
        {
            FireDragon newDragonCopy = newDragon.MemberwiseClone() as FireDragon;
            return newDragonCopy;
        }
    }

    public class Elsa : PokemonTemplate
    {
        public Elsa()
        {
            this.ID = 4;
            this.Name = "Elsa";
            this.HP = 500;
            this.MP = 300;
            this.MaxHP = this.HP;
            this.MaxMP = this.MP;
            this.type = PokemonType.Ice;
            this.owner = null;

            attackActions.Add(IceballShooting);
            attackActions.Add(Icing);
            idleActions.Add(Singing);
        }

        private static Elsa newElsa = new Elsa();

        public void IceballShooting(PokemonTemplate enemy)
        {
            if (this.MP >= 180)
            {
                enemy.GetHP -= 50;
                this.MP -= 180;
            }
        }

        public void Icing(PokemonTemplate enemy)
        {
            if (this.MP >= 250)
            {
                enemy.GetHP -= 20;
                enemy.GetMP -= 50;
                this.MP -= 250;
            }
        }

        public void Singing(PokemonTemplate _)
        {
            if (this.MP + 200 <= this.MaxMP)
                this.MP += 200;
            else
                this.MP = this.MaxMP;
        }

        public override object Clone()
        {
            Elsa newElsaCopy = newElsa.MemberwiseClone() as Elsa;
            return newElsaCopy;
        }
    }

    public class Killer : PokemonTemplate
    {
        public Killer()
        {
            this.ID = 5;
            this.Name = "Eddy";
            this.HP = 1000;
            this.MP = 800;
            this.MaxHP = this.HP;
            this.MaxMP = this.MP;
            this.type = PokemonType.Fire;
            this.owner = null;

            attackActions.Add(Murdering);
            idleActions.Add(Smiling);

        }

        private static Killer newKiller = new Killer();

        public void Murdering(PokemonTemplate enemy)
        {
            if (this.MP >= 300)
            {
                enemy.GetHP -= 200;
                this.MP -= 300;
            }
        }

        public void Smiling(PokemonTemplate _)
        {
            if (this.MP + 500 <= this.MaxMP)
                this.MP += 500;
            else
                this.MP = this.MaxMP;
        }

        public override object Clone()
        {
            Killer newKillerCopy = newKiller.MemberwiseClone() as Killer;
            return newKillerCopy;
        }
    }
}
