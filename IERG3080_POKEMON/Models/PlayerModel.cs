using System;
using System.Collections.Generic;
using System.Text;

namespace PlayerModel
{
    class PlayerEvents
    {
        public delegate void BattleReadyChangedEventHandler
            (object source, EventArgs args, List<PokemonPresenter.PokemonDisplay> newList);
        public static event BattleReadyChangedEventHandler BattleReadyChanged;
        protected static void OnBattleReadyChanged(List<PokemonPresenter.PokemonDisplay> newList)
        {
            if (BattleReadyChanged != null)
                BattleReadyChanged(typeof(PlayerEvents), EventArgs.Empty, newList);
        }

        public delegate void PokemonInventoryChangedEventHandler
            (object source, EventArgs args, List<PokemonPresenter.PokemonDisplay> newList);
        public static event PokemonInventoryChangedEventHandler PokemonInventoryChanged;
        protected static void OnPokemonInventoryChanged(List<PokemonPresenter.PokemonDisplay> newList)
        {
            if (PokemonInventoryChanged != null)
                PokemonInventoryChanged(typeof(PlayerEvents), EventArgs.Empty, newList);
        }

        public delegate void PotionsChangedEventHandler(object source, EventArgs args, int newNum);
        public static event PotionsChangedEventHandler PotionsChanged;
        protected static void OnPotionsChanged(int newNum)
        {
            if (PotionsChanged != null)
                PotionsChanged(typeof(PlayerEvents), EventArgs.Empty, newNum);
        }
    }

    class Player : PlayerEvents
    {

        public List<PokemonPresenter.PokemonDisplay> PokemonToDisplay { get; private set; }

        private List<PokemonPresenter.PokemonDisplay> battleReadyPokemon =
            new List<PokemonPresenter.PokemonDisplay>() { null };
        public List<PokemonPresenter.PokemonDisplay> BattleReadyPokemon
        {
            get
            {
                if (battleReadyPokemon.Count > 3)
                    throw new Exception("Too many Battle ready pokemons in the List!");
                return battleReadyPokemon;
            }
        }
        public int numBattlePokemon { get; private set; }

        private int ID;
        public Dictionary<int, PokemonPresenter.PokemonDisplay> PokemonDisplayDict { get; private set; }
        public Dictionary<int, PokemonModel.PokemonCaptured> PokemonModelDict { get; private set; }

        public int expPotions;

        public int maxLevel { get; private set; }

        private Player()
        {
            maxLevel = 1;
            expPotions = 2;
            ID = 0;  //TODOTODOTODOTODOTODOTODOTODOTOFDOTODOTOROTODOTOFOTODOOTODOTO
            numBattlePokemon = 0;
            PokemonToDisplay = new List<PokemonPresenter.PokemonDisplay>();
            PokemonDisplayDict = new Dictionary<int, PokemonPresenter.PokemonDisplay>();
            PokemonModelDict = new Dictionary<int, PokemonModel.PokemonCaptured>();

        }

        static private Player instance;
        static public Player Main
        {
            get
            {
                if (instance == null)
                    instance = new Player();
                return instance;
            }
        }

        public void ChangeMaxLevel(int level)
        {
            maxLevel = level;
        }

        public void UpdateBattleReady()
        {
            for (int i = 0; i < BattleReadyPokemon.Count; i++)
            {
                if (BattleReadyPokemon[i] != null)
                {
                    int tempID = BattleReadyPokemon[i].ID;
                    BattleReadyPokemon[i] = PokemonDisplayDict[tempID];
                }
            }
        }

        public void OnAddBattlePokemon(int ID)
        {
            if (ID == -1)
            {
                battleReadyPokemon =
                    new List<PokemonPresenter.PokemonDisplay>() { null };
                numBattlePokemon = 0;
                return;
            }

            PokemonPresenter.PokemonDisplay pokemonToAdd =
                PokemonDisplayDict[ID];

            if (BattleReadyPokemon[0] == null)
            {
                BattleReadyPokemon[0] = pokemonToAdd;
                numBattlePokemon++;
                return;
            }

            if (BattleReadyPokemon[0].ID == pokemonToAdd.ID)
                return;

            for (int i = 1; i < battleReadyPokemon.Count; i++)
            {
                if (BattleReadyPokemon[i].ID == pokemonToAdd.ID)
                {
                    battleReadyPokemon[i] = battleReadyPokemon[0];
                    battleReadyPokemon[0] = pokemonToAdd;

                    // EVENT Battle list has changed
                    OnBattleReadyChanged(BattleReadyPokemon);
                    return;
                }
            }
            if (BattleReadyPokemon.Count == 3)
                BattleReadyPokemon.RemoveAt(2);
            BattleReadyPokemon.Insert(0, pokemonToAdd);

            if (numBattlePokemon < 3)
                numBattlePokemon++;

            // EVENT Battle list has changed
            OnBattleReadyChanged(BattleReadyPokemon);
        }

        public void OnPokemonChanged(int ID)
        {
            PokemonDisplayDict.Remove(ID);
            PokemonDisplayDict.Add(ID,
                new PokemonPresenter.PokemonDisplay(PokemonModelDict[ID]));

            for (int i = 0; i < PokemonToDisplay.Count; i++)
            {
                if (PokemonToDisplay[i].ID == ID)
                {
                    PokemonToDisplay[i] = PokemonDisplayDict[ID];
                    break;
                }
            }

            UpdateBattleReady();

            PokemonToDisplay.Sort();

            //EVENT list has changed
            OnPokemonInventoryChanged(PokemonToDisplay);
        }

        public void OnPokemonSold(int ID)
        {
            PokemonDisplayDict.Remove(ID);

            expPotions += ((int)(PokemonModelDict[ID].level / 3) + 1);
            PokemonModelDict.Remove(ID);

            for (int t = 0; t < BattleReadyPokemon.Count; t++)
            {
                if (BattleReadyPokemon[t] != null)
                {
                    if (BattleReadyPokemon[t].ID == ID)
                    {
                        BattleReadyPokemon.RemoveAt(t);

                        //event Battle list changed
                        OnBattleReadyChanged(BattleReadyPokemon);
                    }
                }
            }

            for (int i = 0; i < PokemonToDisplay.Count; i++)
            {
                if (PokemonToDisplay[i].ID == ID)
                {
                    PokemonToDisplay.RemoveAt(i);
                    break;
                }
            }
            PokemonToDisplay.Sort();

            //EVENT list has changed
            OnPokemonInventoryChanged(PokemonToDisplay);

            //EVENT potion number has changed
            OnPotionsChanged(expPotions);
        }

        public void OnDrinkPotion(int ID)
        {

            if (expPotions > 0)
            {
                expPotions--;

                PokemonModelDict[ID].GainXP(90);

                //EVENT list has changed
                OnPokemonInventoryChanged(PokemonToDisplay);

                //EVENT potion number has changed
                OnPotionsChanged(expPotions);
            }
        }

        public  void OnPokemonRenamed(int ID, string newName)
        {
            PokemonModelDict[ID].ChangeDisplayName(newName);
        }

        public void PokemonAdded(PokemonModel.PokemonInstance pokemonInstance)
        {
            ID += 1;

            PokemonModel.PokemonCaptured newPokemon =
                PokemonModel.PokemonFactory.CapturePokemon(pokemonInstance, ID);

            PokemonModelDict.Add(ID, newPokemon);

            PokemonDisplayDict.Add(ID, new PokemonPresenter.PokemonDisplay(newPokemon));
            PokemonToDisplay.Add(PokemonDisplayDict[ID]);
            PokemonToDisplay.Sort();

            //EVENT list has changed
            OnPokemonInventoryChanged(PokemonToDisplay);
        }
    }
}
