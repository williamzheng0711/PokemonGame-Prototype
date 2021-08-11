using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PokemonModel
{
    public static class PokemonDataLib
    {
        public enum types
        {
            Normal, Grass, Ice, Flying, Psychic, Bug, Rock,
            Dragon, Steel, Fairy, Poison, Fire, Water, Electric,
            Fighting, Ground
        }

        public enum rarity { Common = 1, Rare = 2, Legendary = 4 }

        public delegate void Attack(PokemonInstance attacker, PokemonInstance attacked, bool allyIsAttacked);
    }

    public class PokemonRecord
    {
        public string Name { get; protected set; }

        public PokemonDataLib.rarity Rarity { get; protected set; }

        public List<PokemonDataLib.types> Types { get; protected set; }

        public PokemonRecord(string name, PokemonDataLib.rarity rarity, List<PokemonDataLib.types> types)
        {
            Name = name;
            Rarity = rarity;
            Types = types;
        }
    }

    public class PokemonInstance : PokemonRecord
    {
        protected PokemonDataLib.Attack lightAttack;
        protected PokemonDataLib.Attack heavyAttack;

        public AttackRecord lightAttackInfo { get; protected set; }
        public AttackRecord heavyAttackInfo { get; protected set; }

        public int maxHP { get; protected set; }
        public int HP { get; protected set; }

        public int level { get; protected set; }

        public int maxHeavyCharge { get; protected set; }
        public int heavyCharge { get; protected set; }

        public string DisplayName { get; protected set; }

        private Random rand = new Random();

        public PokemonInstance(PokemonRecord pokemonInfo,
        FullAttack lightAttack, FullAttack heavyAttack) :
        base(pokemonInfo.Name, pokemonInfo.Rarity, pokemonInfo.Types)
        {
            this.lightAttack = lightAttack.attackDelegate;
            this.heavyAttack = heavyAttack.attackDelegate;

            lightAttackInfo = lightAttack.info;
            heavyAttackInfo = heavyAttack.info;
            DisplayName = Name;

            maxHP = rand.Next((int)Rarity * 50) + 50;

            level = rand.Next(PlayerModel.Player.Main.maxLevel) + 1;
            maxHP += level * (int)Rarity * 4;
            HP = maxHP;

            maxHeavyCharge = 4 + level;
            heavyCharge = 0;
        }

        public void DecrementHP(int damage)
        {
            HP -= damage;
            if (HP < 0)
                HP = 0;
        }

        public void LightAttack(PokemonInstance attacked, bool allyIsAttacked)
        {
            if (heavyCharge < maxHeavyCharge)
                heavyCharge++;
            lightAttack(this, attacked, allyIsAttacked);
        }

        public void HeavyAttack(PokemonInstance attacked, bool allyIsAttacked)
        {
            heavyAttack(this, attacked, allyIsAttacked);
            heavyCharge = 0;
        }
    }

    public class PokemonCaptured : PokemonInstance
    {
        public int ID { get; private set; }

        public int exp { get; private set; }
        public int maxExp { get; private set; }

        public PokemonInstance instance { get; private set; }

        private int lastEvolveLevel;

        public PokemonCaptured(PokemonInstance pokemon, int ID) : base(pokemon,
        new FullAttack(pokemon.lightAttackInfo, PokemonAttackLib.GetAttackDelegate(pokemon.lightAttackInfo.attackName)),
        new FullAttack(pokemon.heavyAttackInfo, PokemonAttackLib.GetAttackDelegate(pokemon.heavyAttackInfo.attackName)))
        {
            this.ID = ID;
            maxExp = 100 + level * 20;
            exp = 0;
            lastEvolveLevel = 0;
            DisplayName = Name + " " + ID.ToString();
        }

        public void GainXP(int I)
        {
            exp += I;
            while (exp > maxExp)
            {
                exp -= maxExp;
                LevelUp();
            }
            PlayerModel.Player.Main.OnPokemonChanged(ID);
        }

        public void ChangeDisplayName(string newName)
        {
            DisplayName = newName;
            PlayerModel.Player.Main.OnPokemonChanged(ID);
        }

        public void Restore()
        {
            HP = maxHP;
            heavyCharge = 0;
        }

        private void LevelUp()
        {
            level += 1;
            maxExp += 20;
            maxHeavyCharge += 1;
            maxHP += (int)Rarity * 3;
            if (level > PlayerModel.Player.Main.maxLevel)
                PlayerModel.Player.Main.ChangeMaxLevel(level);
            if (level % 6 == 0 && level != lastEvolveLevel)
            {
                lastEvolveLevel = level;
                Evolve();
            }
            Restore();
        }

        private void Evolve()
        {
            if (PokemonDictionary.Main.evolveDict.ContainsKey(Name))
            {
                PokemonRecord newRecord =
                    PokemonDictionary.Main.pokemonDict[PokemonDictionary.Main.evolveDict[Name]];
                Name = newRecord.Name;
                Types = newRecord.Types;
                Rarity = newRecord.Rarity;

                PokemonInstance newInstance = PokemonFactory.MakePokemon(newRecord);
                lightAttackInfo = newInstance.lightAttackInfo;
                heavyAttackInfo = newInstance.heavyAttackInfo;

                lightAttack = PokemonAttackLib.GetAttackDelegate(lightAttackInfo.attackName);
                heavyAttack = PokemonAttackLib.GetAttackDelegate(heavyAttackInfo.attackName);

                maxHP += 5 * (int)Rarity;

                GeneralEvents.OnEvolve(DisplayName, Name);
            }
        }
    }

    public class PokemonDictionary
    {
        private List<PokemonRecord> pokemonList;

        public List<PokemonRecord> commonList { get; private set; }
        public List<PokemonRecord> rareList { get; private set; }
        public List<PokemonRecord> legendaryList { get; private set; }

        public Dictionary<string, PokemonRecord> pokemonDict { get; private set; }

        public Dictionary<string, string> evolveDict { get; private set; }

        static private PokemonDictionary instance;
        static public PokemonDictionary Main
        {
            get
            {
                if (instance == null)
                    instance = new PokemonDictionary();
                return instance;
            }
        }

        private PokemonDictionary()
        {
            pokemonList = new List<PokemonRecord>();

            pokemonDict = new Dictionary<string, PokemonRecord>();
            evolveDict = new Dictionary<string, string>();

            commonList = new List<PokemonRecord>();
            rareList = new List<PokemonRecord>();
            legendaryList = new List<PokemonRecord>();

            string[] lines = File.ReadAllLines(@"PokemonLogs\PokemonInfo.txt");
            foreach (string line in lines)
            {
                string[] info = line.Split("; ");
                string Name = info[0];

                string rarity = info[1];
                PokemonDataLib.rarity Rarity = (PokemonDataLib.rarity)
                    Enum.Parse(typeof(PokemonDataLib.rarity), rarity, true);

                string[] types = info[2].Split(", ");
                List<PokemonDataLib.types> Types = new List<PokemonDataLib.types>();
                foreach (string type in types)
                    Types.Add((PokemonDataLib.types)
                        Enum.Parse(typeof(PokemonDataLib.types), type, true));

                pokemonList.Add(new PokemonRecord(Name, Rarity, Types));
            }
            foreach (PokemonRecord pokemon in pokemonList)
            {
                pokemonDict.Add(pokemon.Name, pokemon);
                if (pokemon.Rarity == PokemonDataLib.rarity.Common)
                    commonList.Add(pokemon);
                else if (pokemon.Rarity == PokemonDataLib.rarity.Rare)
                    rareList.Add(pokemon);
                else if (pokemon.Rarity == PokemonDataLib.rarity.Legendary)
                    legendaryList.Add(pokemon);
            }

            string[] evolutions = File.ReadAllLines(@"PokemonLogs\EvolveInfo.txt");
            foreach (string evolution in evolutions)
            {
                string[] evolveRecord = evolution.Split("; ");
                evolveDict.Add(evolveRecord[0], evolveRecord[1]);
            }
        }
    }

    public class AttackRecord
    {
        public List<PokemonDataLib.types> attackTypes { get; private set; }
        public string attackName { get; private set; }
        public int attackDamage { get; private set; }

        public AttackRecord(List<PokemonDataLib.types> types, string name, int damage)
        {
            attackTypes = types;
            attackName = name;
            attackDamage = damage;
        }
    }

    public class FullAttack : AttackRecord
    {
        public AttackRecord info { get; private set; }

        public PokemonDataLib.Attack attackDelegate { get; private set; }

        public FullAttack(AttackRecord attackBase, PokemonDataLib.Attack attackDelegate) :
        base(attackBase.attackTypes, attackBase.attackName, attackBase.attackDamage)
        {
            this.attackDelegate = attackDelegate;
            info = attackBase;
        }
    }

    public class PokemonAttackLib
    {
        public List<AttackRecord>
            lightAttackDict
        { get; private set; }

        public List<AttackRecord>
            heavyAttackDict
        { get; private set; }

        public Dictionary<string, int> damageDict { get; private set; }

        static private PokemonAttackLib instance;
        static public PokemonAttackLib Main
        {
            get
            {
                if (instance == null)
                    instance = new PokemonAttackLib();
                return instance;
            }
        }

        private PokemonAttackLib()
        {
            lightAttackDict = new
                List<AttackRecord>();
            heavyAttackDict = new
                List<AttackRecord>();

            damageDict = new Dictionary<string, int>();

            string[] lines = File.ReadAllLines(@"PokemonLogs\AttackInfo.txt");
            foreach (string line in lines)
            {
                string[] info = line.Split("; ");
                string AttackName = info[0];

                string AttackType = info[1];

                int AttackDamage = Convert.ToInt32(info[2]);
                damageDict.Add(AttackName, AttackDamage);

                string[] types = info[3].Split(", ");
                List<PokemonDataLib.types> Types = new List<PokemonDataLib.types>();
                foreach (string type in types)
                    Types.Add((PokemonDataLib.types)
                        Enum.Parse(typeof(PokemonDataLib.types), type, true));

                if (AttackType == "L")
                    lightAttackDict.Add(new AttackRecord(Types, AttackName, AttackDamage));
                else if (AttackType == "H")
                    heavyAttackDict.Add(new AttackRecord(Types, AttackName, AttackDamage));
            }
        }

        public static PokemonDataLib.Attack GetAttackDelegate(string attackName)
        {
            PokemonDataLib.Attack delegateToReturn = AttackLog.Tackle;

            Type temp = typeof(AttackLog);
            foreach (var attack in temp.GetMethods())
            {
                if (attack.Name == attackName)
                {
                    delegateToReturn = (PokemonDataLib.Attack)Delegate.CreateDelegate(
                        typeof(PokemonDataLib.Attack), attack);
                }
            }
            return delegateToReturn;
        }
    }

    public class PokemonFactory
    {

        static private Random rand = new Random();

        public static PokemonRecord GetPokemonBlueprint(string mode)
        {
            int[] rarityChances;
            switch (mode)
            {
                case "normal":
                    rarityChances = new int[2] { 40, 300 };
                    break;
                case "better":
                    rarityChances = new int[2] { 80, 550 };
                    break;
                case "good":
                    rarityChances = new int[2] { 150, 800 };
                    break;
                default:
                    Console.WriteLine("No Such Mode in GetPokemonBluePrint()!");
                    break;
            }

            List<PokemonRecord> chooseFromList;
            int rarityID = rand.Next(1000);
            if (rarityID < 40)
                chooseFromList = PokemonDictionary.Main.legendaryList;
            else if (rarityID < 300)
                chooseFromList = PokemonDictionary.Main.rareList;
            else
                chooseFromList = PokemonDictionary.Main.commonList;

            int pokemonID = rand.Next(chooseFromList.Count);
            return chooseFromList[pokemonID];
        }

        public static PokemonInstance GetFullPokemon(string mode)
        {
            PokemonRecord pokemon = GetPokemonBlueprint(mode);
            return MakePokemon(pokemon);
        }

        public static PokemonInstance MakePokemon(PokemonRecord pokemonData)
        {
            List<AttackRecord> lightAttackList = new List<AttackRecord>();
            List<AttackRecord> heavyAttackList = new List<AttackRecord>();

            foreach (var attack in PokemonAttackLib.Main.lightAttackDict)
            {
                foreach (var type in pokemonData.Types)
                {
                    if (attack.attackTypes.Contains(type) || attack.attackTypes.Contains(PokemonDataLib.types.Normal))
                        lightAttackList.Add(attack);
                }
            }

            foreach (var attack in PokemonAttackLib.Main.heavyAttackDict)
            {
                foreach (var type in pokemonData.Types)
                {
                    if (attack.attackTypes.Contains(type) || attack.attackTypes.Contains(PokemonDataLib.types.Normal))
                        heavyAttackList.Add(attack);
                }
            }

            int lightID = rand.Next(lightAttackList.Count);
            AttackRecord light = lightAttackList[lightID];
            PokemonDataLib.Attack lightAttackDelegate = PokemonAttackLib.GetAttackDelegate(light.attackName);
            FullAttack lightAttack = new FullAttack(light, lightAttackDelegate);

            int heavyID = rand.Next(heavyAttackList.Count);
            AttackRecord heavy = heavyAttackList[heavyID];
            PokemonDataLib.Attack heavyAttackDelegate = PokemonAttackLib.GetAttackDelegate(heavy.attackName);
            FullAttack heavyAttack = new FullAttack(heavy, heavyAttackDelegate);

            return new PokemonInstance(pokemonData, lightAttack, heavyAttack);
        }

        public static PokemonCaptured CapturePokemon(PokemonInstance pokemon, int ID)
        {
            return new PokemonCaptured(pokemon, ID);
        }
    }

    public class GeneralEvents
    {
        public delegate void EvolveEventHandler(object source, EventArgs args, string displayName, string newName);
        public static event EvolveEventHandler Evolve;
        public static void OnEvolve(string displayName, string newName)
        {
            Evolve?.Invoke(typeof(AttackEvents), EventArgs.Empty, displayName, newName);
        }
    }

    public class AttackEvents
    {

        public delegate void AttackEventHandler(object source, EventArgs args, bool allyIsAttacked,
            string attackName, PokemonPresenter.PokemonBattle attackerBattle, int attackedNewHP);
        public static event AttackEventHandler AttackPerformed;
        public static void OnAttackPerformed(bool allyIsAttacked, string attackName,
        PokemonPresenter.PokemonBattle attackerBattle, int attackedNewHP)
        {
            AttackPerformed?.Invoke(typeof(AttackEvents), EventArgs.Empty, allyIsAttacked,
                attackName, attackerBattle, attackedNewHP);
        }
    }

    public static class AttackLog
    {
        public static void Tackle(PokemonInstance attacker, PokemonInstance attacked, bool allyIsAttacked)
        {
            string attackName = "Tackle";
            int baseDamage = PokemonAttackLib.Main.damageDict[attackName];
            //Console.WriteLine(attackName + "  " + PokemonAttackLib.Main.damageDict[attackName]);

            attacked.DecrementHP(baseDamage);

            AttackEvents.OnAttackPerformed(allyIsAttacked, attackName,
                new PokemonPresenter.PokemonBattle(attacker), attacked.HP);
        }

        public static void Leech(PokemonInstance attacker, PokemonInstance attacked, bool allyIsAttacked)
        {
            string attackName = "Leech";
            int baseDamage = PokemonAttackLib.Main.damageDict[attackName];

            attacked.DecrementHP(baseDamage);
            attacker.DecrementHP(-attacker.heavyCharge * 2);

            AttackEvents.OnAttackPerformed(allyIsAttacked, attackName,
                new PokemonPresenter.PokemonBattle(attacker), attacked.HP);
        }

        public static void Thunder_Wave(PokemonInstance attacker, PokemonInstance attacked, bool allyIsAttacked)
        {
            string attackName = "Thunder_Wave";
            int baseDamage = PokemonAttackLib.Main.damageDict[attackName];

            attacked.DecrementHP(attacker.heavyCharge * 5 + baseDamage);

            AttackEvents.OnAttackPerformed(allyIsAttacked, attackName,
                new PokemonPresenter.PokemonBattle(attacker), attacked.HP);
        }

        public static void Scratch(PokemonInstance attacker, PokemonInstance attacked, bool allyIsAttacked)
        {
            string attackName = "Scratch";
            int baseDamage = PokemonAttackLib.Main.damageDict[attackName];

            attacked.DecrementHP(baseDamage);

            AttackEvents.OnAttackPerformed(allyIsAttacked, attackName,
                new PokemonPresenter.PokemonBattle(attacker), attacked.HP);
        }

        public static void Headbutt(PokemonInstance attacker, PokemonInstance attacked, bool allyIsAttacked)
        {
            string attackName = "Headbutt";
            int baseDamage = PokemonAttackLib.Main.damageDict[attackName];

            attacked.DecrementHP(attacker.heavyCharge * 3 + baseDamage);

            AttackEvents.OnAttackPerformed(allyIsAttacked, attackName,
                new PokemonPresenter.PokemonBattle(attacker), attacked.HP);
        }
    }

    //=========================================================================================

    //class Something {
    //    public static void OnBattleLost(object source, EventArgs args, bool state) {
    //        Console.WriteLine("It is " + state + " that Battle is won");
    //    }

    //    public static void OnAttackPerformed(object source, EventArgs args, bool allyIsAttacked, string attackName,
    //    PokemonPresenter.PokemonBattle attackerBattle, int attackedNewHP) {
    //        Console.WriteLine("Ally Attacked is " + allyIsAttacked + "; Attack Name is: " + attackName);
    //    }
    //}

    //public class test {

    //    public static void Main() {
    //        PokemonCaptured pokemon1;
    //        AttackEvents.AttackPerformed += Something.OnAttackPerformed;
    //        PokemonInstance Pokemonpokemon666 = PokemonFactory.GetFullPokemon("good");
    //        for (int i = 0; i < 100; i++) {
    //            pokemon1 = PokemonFactory.CapturePokemon(
    //                PokemonFactory.GetFullPokemon("normal"), 55);
    //            Console.WriteLine(pokemon1.DisplayName);
    //            pokemon1.LightAttack(Pokemonpokemon666, true);
    //            pokemon1.HeavyAttack(Pokemonpokemon666, true);
    //            Console.WriteLine("");
    //        }

    //        Console.WriteLine("");

    //        //PokemonDataLib.rarity temp = PokemonDataLib.rarity.Rare;
    //        //Console.WriteLine(temp);
    //        //temp = temp.Next();
    //        //Console.WriteLine(temp);

    //        //List<PokemonPresenter.PokemonDisplay> battleReadyPokemon =
    //        //new List<PokemonPresenter.PokemonDisplay>();

    //        //battleReadyPokemon.Add(null);
    //        //pokemon1 = PokemonFactory.GetFullPokemon("normal");
    //        //battleReadyPokemon.Add(new PokemonPresenter.PokemonDisplay(PokemonFactory.CapturePokemon(pokemon1, 55)));
    //        //battleReadyPokemon.Add(null);
    //        ////Console.WriteLine(battleReadyPokemon.Count);
    //        //for (int i = 0; i < battleReadyPokemon.Count; i++)
    //        //    if (null == battleReadyPokemon[i])
    //        //        Console.WriteLine("thidsdsf");
    //        //    else
    //        //        Console.WriteLine(battleReadyPokemon[i].Name);

    //        //pokemon1 = PokemonFactory.CapturePokemon(
    //        //    PokemonFactory.GetFullPokemon("normal"), 55);
    //        //Console.WriteLine("THIS\n\n");
    //        //Console.WriteLine(pokemon1.Name);
    //        //pokemon1.LightAttack(Pokemonpokemon666, true);
    //        //pokemon1.HeavyAttack(Pokemonpokemon666, true);
    //        //Console.WriteLine(pokemon1.level);
    //        //Console.WriteLine(pokemon1.exp);
    //        //Console.WriteLine(pokemon1.maxExp);

    //        //pokemon1.GainXP(900);
    //        //Console.WriteLine(pokemon1.Name);
    //        //pokemon1.LightAttack(Pokemonpokemon666, true);
    //        //pokemon1.HeavyAttack(Pokemonpokemon666, true);
    //        //Console.WriteLine(pokemon1.level);
    //        //Console.WriteLine(pokemon1.exp);
    //        //Console.WriteLine(pokemon1.maxExp);

    //        //AttackEvents.AttackPerformed -= Something.OnAttackPerformed;

    //        //List<int> arr = new List<int>() { 0, 1, 2, 3, 4};
    //        //Console.WriteLine("\n\n" + arr[0]);
    //        //Console.WriteLine(arr.Count);
    //        //arr.Insert(0, -325);
    //        //Console.WriteLine(arr[0]);
    //        //Console.WriteLine(arr.Count);

    //        MapModel.Map test = MapModel.Map.Main;

    //        //List<List<int>> test = MapDict[0];
    //        //foreach (var sublist in test) {
    //        //    foreach (var obj in sublist) {
    //        //        Console.Write(obj + "\t");
    //        //    }
    //        //    Console.WriteLine("");
    //        //}

    //        //Dictionary<Tuple<int, int>, string> testDict = new Dictionary<Tuple<int, int>, string>();

    //        //Tuple<int, int> testArr = new Tuple<int, int>(3, 4);


    //        //testDict.Add(testArr, "1111111");

    //        //int[] testArr2 = new int[2] { 3, 5 };
    //        //testDict.Add(testArr2, "3333333");

    //        //int[] testArr3 = new int[2];
    //        //testArr3[0] = 4;
    //        //testArr3[1] = 5;

    //        //Console.WriteLine(testDict.ContainsKey(testArr3));

    //        //CaveModel.Cave.OnCaveStart();

    //        CaveModel.Cave.OnCaveStart();
    //    }
    //}
}