using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace MapModel
{
    class MapEvents
    {
        public delegate void ChangeLocationEventHandler(object source, EventArgs args, int[] newPos);
        public static event ChangeLocationEventHandler ChangeLocation;
        protected static void OnChangeLocation(int[] newPos)
        {
            if (ChangeLocation != null)
                ChangeLocation(typeof(MapEvents), EventArgs.Empty, newPos);
        }

        public delegate void MapChangedEventHandler(object source, EventArgs args,
            List<List<int>> newMap);
        public static event MapChangedEventHandler MapChanged;
        protected static void OnMapChanged(List<List<int>> newMap)
        {
            if (MapChanged != null)
                MapChanged(typeof(MapEvents), EventArgs.Empty, newMap);
        }

        public delegate void TimerNullifyEventHandler(object source, EventArgs args);
        public static event TimerNullifyEventHandler TimerNullify;
        protected static void OnTimerNullify()
        {
            if (TimerNullify != null)
                TimerNullify(typeof(MapEvents), EventArgs.Empty);
        }

        public delegate void DisplayPokemonEventHandler(object source, EventArgs args,
            string name, int[] position, double setTimer);
        public static event DisplayPokemonEventHandler DisplayPokemon;
        protected static void OnDisplayPokemon(string name, int[] position, double setTimer)
        {
            if (DisplayPokemon != null)
                DisplayPokemon(typeof(MapEvents), EventArgs.Empty, name, position, setTimer);
        }

        public delegate void PokemonSteppedEventHandler(object source, EventArgs args,
            string name, int[] pos);
        public static event PokemonSteppedEventHandler PokemonStepped;
        protected static void OnPokemonStepped(string name, int[] pos)
        {
            if (PokemonStepped != null)
                PokemonStepped(typeof(MapEvents), EventArgs.Empty, name, pos);
        }

        public delegate void GymSteppedEventHandler(object source, EventArgs args);
        public static event GymSteppedEventHandler GymStepped;
        protected static void OnGymStepped()
        {
            if (GymStepped != null)
                GymStepped(typeof(MapEvents), EventArgs.Empty);
        }

        public delegate void CaveSteppedEventHandler(object source, EventArgs args);
        public static event CaveSteppedEventHandler CaveStepped;
        protected static void OnCaveStepped()
        {
            if (CaveStepped != null)
                CaveStepped(typeof(MapEvents), EventArgs.Empty);
        }
    }

    class Map : MapEvents
    {

        private Dictionary<int, List<List<int>>> MapDict;
        private int mapNum;

        private List<List<int>> GlobalMap;

        private int[] localPos = new int[2];
        private int[] globalPos = new int[2];
        private int currentMapKey;

        private int[] newLocalPos = new int[2];
        private int[] newGlobalPos = new int[2];
        private int newMapKey;

        private Dictionary<Tuple<int, int>, PokemonModel.PokemonRecord> PokemonByLocation;
        private bool CanSpawnPokemon;

        private List<int> PokemonTiles = new List<int>() { 0, 1 };
        private List<int> StopTiles = new List<int>() { 2 };

        private double delayFactor = 2;
        private PokemonModel.PokemonRecord CapturingPokemon;

        private static Map instance;
        public static Map Main
        {
            get
            {
                if (instance == null)
                    instance = new Map();
                return instance;
            }
        }

        private Random rand = new Random();

        private Map()
        {
            localPos[0] = 4;
            localPos[1] = 4;

            globalPos[0] = 0;
            globalPos[1] = 0;

            MapDict = new Dictionary<int, List<List<int>>>();

            string[] mapLines =
                File.ReadAllLines(@"PokemonLogs\MapInfo.txt");

            List<List<int>> bufferList = new List<List<int>>();
            mapNum = 0;
            foreach (string line in mapLines)
            {
                if (line == "")
                {
                    MapDict.Add(mapNum, bufferList);
                    bufferList = new List<List<int>>();
                    mapNum++;
                }
                else if (line == "end")
                    break;
                else
                {
                    List<int> lineBuf = new List<int>();
                    foreach (string S in line.Split("\t"))
                    {
                        lineBuf.Add(Convert.ToInt32(S));
                    }
                    bufferList.Add(lineBuf);
                }
            }

            GlobalMap = new List<List<int>>() { new List<int>() };
            GlobalMap[0].Add(rand.Next(mapNum));

            CanSpawnPokemon = true;

            CapturingPokemon = null;

            currentMapKey = GlobalMap[0][0];
            List<List<int>> newMap = MapDict[currentMapKey];

            PokemonByLocation =
                new Dictionary<Tuple<int, int>, PokemonModel.PokemonRecord>();

            // Display New Map Event
            OnMapChanged(newMap);
            OnChangeLocation(localPos);
        }

        private void CompareDownRight(int i, int j)
        {
            if (GlobalMap[i][j] == GlobalMap[i + 1][j] ||
            GlobalMap[i][j] == GlobalMap[i][j + 1])
            {
                List<int> restrict = new List<int>() {
                    GlobalMap[i + 1][j], GlobalMap[i][j + 1] };
                List<int> allow = new List<int>();
                for (int c = 0; c < mapNum; c++)
                {
                    if (!restrict.Contains(c))
                        allow.Add(c);
                }
                GlobalMap[i][j] = allow[rand.Next(allow.Count)];
            }
        }

        private void CompareLeftUp(int i, int j)
        {
            if (GlobalMap[i][j] == GlobalMap[i - 1][j] ||
            GlobalMap[i][j] == GlobalMap[i][j - 1])
            {
                List<int> restrict = new List<int>() {
                    GlobalMap[i - 1][j], GlobalMap[i][j - 1] };
                List<int> allow = new List<int>();
                for (int c = 0; c < mapNum; c++)
                {
                    if (!restrict.Contains(c))
                        allow.Add(c);
                }
                GlobalMap[i][j] = allow[rand.Next(allow.Count)];
            }
        }

        private void ExtendGlobalMap()
        {
            for (int i = 0; i < GlobalMap.Count; i++)
            {
                GlobalMap[i].Add(rand.Next(mapNum));
                GlobalMap[i].Insert(0, rand.Next(mapNum));
            }

            List<int> bufLine = new List<int>();
            for (int r = 0; r < GlobalMap[0].Count; r++)
                bufLine.Add(rand.Next(mapNum));
            GlobalMap.Insert(0, bufLine);
            bufLine = new List<int>();
            for (int r = 0; r < GlobalMap[0].Count; r++)
                bufLine.Add(rand.Next(mapNum));
            GlobalMap.Add(bufLine);

            globalPos[0]++;
            globalPos[1]++;
            newGlobalPos[0]++;
            newGlobalPos[1]++;

            for (int i = GlobalMap.Count - 2; i >= 0; i--)
            {
                if (i == 0)
                    CompareDownRight(0, 0);
                else
                {
                    CompareDownRight(i, 0);
                    CompareDownRight(0, i);
                }
            }

            int max = GlobalMap.Count - 1;
            for (int i = 1; i < GlobalMap.Count; i++)
            {
                if (i == max)
                    CompareLeftUp(max, max);
                else
                {
                    CompareLeftUp(i, max);
                    CompareLeftUp(max, i);
                }
            }
        }

        private void OutOfTheMap(string dir)
        {
            switch (dir)
            {
                case "up":
                    if (globalPos[0] - 1 < 0)
                        ExtendGlobalMap();
                    newGlobalPos[0]--;
                    newLocalPos[0] = Main.MapDict[Main.currentMapKey].Count - 1;

                    break;
                case "down":
                    if (globalPos[0] + 1 >= GlobalMap.Count)
                        ExtendGlobalMap();

                    newGlobalPos[0]++;
                    newLocalPos[0] = 0;
                    break;
                case "left":
                    if (globalPos[1] - 1 < 0)
                        ExtendGlobalMap();
                    newGlobalPos[1]--;
                    newLocalPos[1] = Main.MapDict[Main.currentMapKey][0].Count - 1;
                    break;
                case "right":
                    if (globalPos[1] + 1 >= GlobalMap[0].Count)
                        ExtendGlobalMap();
                    newGlobalPos[1]++;
                    newLocalPos[1] = 0;
                    break;
            }
            newMapKey = GlobalMap[newGlobalPos[0]][newGlobalPos[1]];
        }

        //timer runs out
        public static void OnCanSpawnPokemon()
        {
            Main.CanSpawnPokemon = true;
        }

        public static void OnPokemonCaptured()
        {

            if (Main.CapturingPokemon == null)
                throw new Exception("CapturingPokemon is NULL");

            PokemonModel.PokemonInstance pokemon =
                PokemonModel.PokemonFactory.MakePokemon(Main.CapturingPokemon);

            PlayerModel.Player.Main.PokemonAdded(pokemon);

            Main.CapturingPokemon = null;
        }

        private bool CheckIfMove()
        {
            int nextTile = MapDict[newMapKey][newLocalPos[0]][newLocalPos[1]];
            if (!StopTiles.Contains(nextTile))
            {
                localPos[0] = newLocalPos[0];
                localPos[1] = newLocalPos[1];

                globalPos[0] = newGlobalPos[0];
                globalPos[1] = newGlobalPos[1];

                Tuple<int, int> posTuple = new Tuple<int, int>(localPos[0], localPos[1]);

                if (currentMapKey != newMapKey)
                {
                    List<List<int>> newMap = MapDict[newMapKey];
                    currentMapKey = newMapKey;

                    // Display New Map Event
                    OnMapChanged(newMap);

                    PokemonByLocation =
                        new Dictionary<Tuple<int, int>, PokemonModel.PokemonRecord>();

                    CanSpawnPokemon = true;

                    // PokemonTimer Nullify Event
                    OnTimerNullify();
                }

                //Move Player Event
                OnChangeLocation(localPos);

                if (PokemonByLocation.ContainsKey(posTuple))
                {
                    CapturingPokemon = PokemonByLocation[posTuple];
                    string pokemonName = CapturingPokemon.Name;
                    Main.PokemonByLocation.Remove(posTuple);

                    // event pokemon is stepped on
                    OnPokemonStepped(pokemonName, localPos);
                }

                if (nextTile == -1)
                    OnGymStepped();

                if (nextTile == -2)
                    OnCaveStepped();

                if (CanSpawnPokemon && (rand.Next(100) <= 10))
                {
                    Tuple<int, int> randomPos;
                    for (int i = 0; i < 2; i++)
                    {
                        randomPos = new Tuple<int, int>(
                            rand.Next(MapDict[currentMapKey].Count),
                            rand.Next(MapDict[currentMapKey][0].Count)
                        );
                        if (PokemonTiles.Contains(
                        MapDict[currentMapKey][randomPos.Item1][randomPos.Item2]) &&
                        (randomPos != posTuple) && (!PokemonByLocation.ContainsKey(randomPos)))
                        {
                            CanSpawnPokemon = false;
                            PokemonModel.PokemonRecord newPokemon =
                                PokemonModel.PokemonFactory.GetPokemonBlueprint("normal");
                            PokemonByLocation.Add(randomPos, newPokemon);
                            double timerLength = 0 + Math.Pow(delayFactor, PokemonByLocation.Count);

                            int[] newPokemonPos = new int[2] { randomPos.Item1, randomPos.Item2 };

                            // Display the pokemon and set the timer event
                            OnDisplayPokemon(newPokemon.Name, newPokemonPos, timerLength);

                            break;
                        }
                    }
                }

                return true;
            }
            else
            {
                newMapKey = currentMapKey;
                newLocalPos[0] = localPos[0];
                newLocalPos[1] = localPos[1];

                return false;
            }

        }

        public static void OnPlayerMove(string dir)
        {
            Main.newLocalPos = new int[2]
                { Main.localPos[0], Main.localPos[1] };
            Main.newGlobalPos = new int[2]
                { Main.globalPos[0], Main.globalPos[1] };

            Main.newMapKey =
                Main.currentMapKey;

            switch (dir)
            {
                case "up":
                    if ((Main.localPos[0] - 1) < 0)
                    {
                        Main.OutOfTheMap("up");
                        break;
                    }
                    else
                    {
                        Main.newLocalPos[0]--;
                        break;
                    }

                case "down":
                    if ((Main.localPos[0] + 1) >= Main.MapDict[Main.currentMapKey].Count)
                    {
                        Main.OutOfTheMap("down");
                        break;
                    }
                    else
                    {
                        Main.newLocalPos[0]++;
                        break;
                    }
                case "left":
                    if ((Main.localPos[1]) - 1 < 0)
                    {
                        Main.OutOfTheMap("left");
                        break;
                    }
                    else
                    {
                        Main.newLocalPos[1]--;
                        break;
                    }
                case "right":
                    if ((Main.localPos[1] + 1) >= Main.MapDict[Main.currentMapKey][0].Count)
                    {
                        Main.OutOfTheMap("right");
                        break;
                    }
                    else
                    {
                        Main.newLocalPos[1]++;
                        break;
                    }
            }
            Main.CheckIfMove();
        }
    }
}
