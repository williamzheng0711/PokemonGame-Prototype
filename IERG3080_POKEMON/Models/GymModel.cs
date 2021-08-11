using System;
using System.Collections.Generic;
using System.Text;

namespace GymModel
{

    static class BattleEvents
    {
        public delegate void BattleEndedEventHandler(object source, EventArgs args, bool state);
        public static event BattleEndedEventHandler BattleEnded;
        public static void OnBattleEnded(bool isWon)
        {
            if (BattleEnded != null)
                BattleEnded(typeof(BattleEvents), EventArgs.Empty, isWon);
        }

        public delegate void PlayerTurnEventHandler(object source, EventArgs args);
        public static event PlayerTurnEventHandler PlayerTurn;
        public static void OnPlayerTurn()
        {
            if (PlayerTurn != null)
                PlayerTurn(typeof(BattleEvents), EventArgs.Empty);
        }

        public delegate void ChoiceShowEventHandler(object source, EventArgs args,
            List<PokemonPresenter.PokemonDisplay> availablePokemon);
        public static event ChoiceShowEventHandler ChoiceShow;
        public static void OnChoiceShow(List<PokemonPresenter.PokemonDisplay> availablePokemon)
        {
            if (ChoiceShow != null)
                ChoiceShow(typeof(BattleEvents), EventArgs.Empty, availablePokemon);
        }

        public delegate void PokemonShowEventHandler(object source, EventArgs args,
            PokemonPresenter.PokemonBattle pokemon, bool isAlly);
        public static event PokemonShowEventHandler PokemonShow;
        public static void OnPokemonShow(PokemonPresenter.PokemonBattle pokemon, bool isAlly)
        {
            if (PokemonShow != null)
                PokemonShow(typeof(BattleEvents), EventArgs.Empty, pokemon, isAlly);
        }
    }

    class GymBattle
    {
        public static bool inBattle = false;

        private int allyPokemonLeft;
        private int enemyPokemonLeft;

        private List<PokemonModel.PokemonCaptured> AllyPokemonList;
        private List<PokemonModel.PokemonInstance> EnemyPokemonList;

        private int CurrentAlly;
        private int CurrentEnemy;

        private PokemonModel.PokemonInstance CurrentAllyInstance;
        private PokemonModel.PokemonInstance CurrentEnemyInstance;

        private bool LeftBattle;

        private static GymBattle instance;
        private GymBattle()
        {
            allyPokemonLeft = PlayerModel.Player.Main.numBattlePokemon;
            enemyPokemonLeft = 2;

            AllyPokemonList = new List<PokemonModel.PokemonCaptured>();
            EnemyPokemonList = new List<PokemonModel.PokemonInstance>();

            foreach (PokemonPresenter.PokemonDisplay pokemon in PlayerModel.Player.Main.BattleReadyPokemon)
                AllyPokemonList.Add(PlayerModel.Player.Main.PokemonModelDict[pokemon.ID]);

            for (int i = 0; i < enemyPokemonLeft; i++)
            {
                EnemyPokemonList.Add(PokemonModel.PokemonFactory.GetFullPokemon("better"));
            }
            CurrentEnemy = 0;
            CurrentEnemyInstance = EnemyPokemonList[0];
            CurrentAlly = 0;
            CurrentAllyInstance = AllyPokemonList[0];

            LeftBattle = false;
        }

        public static void OnBattleStart()
        {
            if (inBattle == true)
                throw new Exception("New Battle cannot be started when" +
                    " there is already an ongoing Battle");

            inBattle = true;
            instance = new GymBattle();

            //SUBSCROPTIONS

            BattleEvents.OnPokemonShow(
                new PokemonPresenter.PokemonBattle(instance.CurrentAllyInstance), true);

            BattleEvents.OnPokemonShow(
                new PokemonPresenter.PokemonBattle(instance.CurrentEnemyInstance), false);

            BattleEvents.OnPlayerTurn();
        }

        public static void OnPokemonChangeClick()
        {
            List<PokemonPresenter.PokemonDisplay> availablePokemon =
                instance.GetAvailablePokemon();

            //Show Pokemon Choice List event
            BattleEvents.OnChoiceShow(availablePokemon);
        }

        private List<PokemonPresenter.PokemonDisplay> GetAvailablePokemon()
        {
            List<PokemonPresenter.PokemonDisplay> availablePokemon =
                new List<PokemonPresenter.PokemonDisplay>();
            for (int i = 0; i < instance.AllyPokemonList.Count; i++)
            {
                if (i != instance.CurrentAlly && instance.AllyPokemonList[i].HP > 0)
                    availablePokemon.Add(
                        new PokemonPresenter.PokemonDisplay(instance.AllyPokemonList[i]));
            }
            return availablePokemon;
        }

        public static void OnPokemonChanged(int ID)
        {
            int temp = 0;
            foreach (var pokemon in instance.AllyPokemonList)
            {
                if (pokemon.ID == ID)
                    break;
                temp++;
            }
            instance.CurrentAlly = temp;
            instance.CurrentAllyInstance = instance.AllyPokemonList[temp];
            //Show new pokemon event
            BattleEvents.OnPokemonShow(
                new PokemonPresenter.PokemonBattle(instance.CurrentAllyInstance), true);
        }

        public static void OnAttack(string def)
        {
            switch (def)
            {
                case "light":
                    instance.CurrentAllyInstance.LightAttack(instance.CurrentEnemyInstance, false);
                    break;
                case "heavy":
                    instance.CurrentAllyInstance.HeavyAttack(instance.CurrentEnemyInstance, false);
                    break;
                default:
                    throw new Exception("OnAttack() : Wrong Attack Type");
            }
            if (instance.CurrentEnemyInstance.HP <= 0)
            {
                instance.enemyPokemonLeft--;
                if (instance.enemyPokemonLeft > 0)
                {
                    instance.CurrentEnemy++;
                    instance.CurrentEnemyInstance =
                        instance.EnemyPokemonList[instance.CurrentEnemy];

                    //Show new pokemon (BattlePokemon pokemon, bool isAlly) event
                    BattleEvents.OnPokemonShow(
                        new PokemonPresenter.PokemonBattle(instance.CurrentEnemyInstance), false);

                    return;
                }
                else
                {
                    instance.EndBattle(true);
                    return;
                }
            }

            EnemyTurn();
            BattleEvents.OnPlayerTurn();
        }

        private static void EnemyTurn()
        {
            Random rand = new Random();
            int def = rand.Next(10 * instance.CurrentEnemyInstance.level);
            if (def <= 10)
                instance.CurrentEnemyInstance.HeavyAttack(instance.CurrentAllyInstance, true);
            else
                instance.CurrentEnemyInstance.LightAttack(instance.CurrentAllyInstance, true);

            if (instance.CurrentAllyInstance.HP <= 0)
            {
                instance.allyPokemonLeft--;
                if (instance.allyPokemonLeft > 0)
                {
                    List<PokemonPresenter.PokemonDisplay> availablePokemon =
                        instance.GetAvailablePokemon();

                    //Show Pokemon Choice List event
                    BattleEvents.OnChoiceShow(availablePokemon);
                }
                else
                    instance.EndBattle(false);
            }
        }

        public static void OnLeaveBattle()
        {
            instance.LeftBattle = true;
            instance.EndBattle(false);
        }

        private void EndBattle(bool won)
        {
            switch (won)
            {
                case true:
                    foreach (var pokemon in instance.AllyPokemonList)
                    {
                        pokemon.GainXP(100);
                        pokemon.Restore();
                    }
                    break;
                case false:
                    if (!instance.LeftBattle)
                    {
                        foreach (var pokemon in instance.AllyPokemonList)
                        {
                            pokemon.GainXP(35);
                            pokemon.Restore();
                        }
                    }
                    break;
            }
            inBattle = false;
            instance = null;
            BattleEvents.OnBattleEnded(won);
            //Unsubscribe
        }
    }
}