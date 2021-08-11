using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonPresenter
{
    public class PokemonDisplay : PokemonBattle
    {
        public int ID { get; private set; }

        public string DisplayName { get; private set; }

        public int exp { get; private set; }
        public int maxExp { get; private set; }

        public PokemonDisplay(PokemonModel.PokemonCaptured pokemon) :
        base(pokemon)
        {
            ID = pokemon.ID;
            DisplayName = pokemon.DisplayName;
            exp = pokemon.exp;
            maxExp = pokemon.maxExp;
        }
    }

    public class PokemonBattle : IComparable<PokemonDisplay>
    {

        public string Name { get; private set; }

        public int maxHeavyCharge { get; private set; }
        public int heavyCharge { get; private set; }

        public int maxHP { get; private set; }
        public int HP { get; private set; }

        public int level { get; private set; }

        public string lightAttackName { get; private set; }
        public string heavyAttackName { get; private set; }

        public PokemonBattle(PokemonModel.PokemonInstance pokemon)
        {
            Name = pokemon.Name;
            maxHeavyCharge = pokemon.maxHeavyCharge;
            heavyCharge = pokemon.heavyCharge;
            maxHP = pokemon.maxHP;
            HP = pokemon.HP;
            level = pokemon.level;
            lightAttackName = pokemon.lightAttackInfo.attackName;
            heavyAttackName = pokemon.heavyAttackInfo.attackName;
        }

        public int CompareTo(PokemonDisplay other)
        {
            if (level > other.level) return 1;
            else if (level == other.level) return 0;
            else return -1;
        }
    }
}
