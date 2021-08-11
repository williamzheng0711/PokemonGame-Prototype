using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CaveModel
{
    class CaveEvents
    {
        public delegate void MovePlayerEventHandler
            (object source, EventArgs args, int[] newPos);
        public static event MovePlayerEventHandler MovePlayer;
        protected static void OnMovePlayer(int[] newPos)
        {
            if (MovePlayer != null)
                MovePlayer(typeof(CaveEvents), EventArgs.Empty, newPos);
        }

        public delegate void EndCaveEventHandler
            (object source, EventArgs args, string name);
        public static event EndCaveEventHandler EndCave;
        protected static void OnEndCave(string name)
        {
            if (EndCave != null)
                EndCave(typeof(CaveEvents), EventArgs.Empty, name);
        }

        public delegate void OnMazeDrawEventHandler
            (object source, EventArgs args, List<List<int>> newList);
        public static event OnMazeDrawEventHandler MazeDraw;
        protected static void OnMazeDraw(List<List<int>> newList)
        {
            if (MazeDraw != null)
                MazeDraw(typeof(CaveEvents), EventArgs.Empty, newList);
        }
    }

    class Cave : CaveEvents
    {

        private static bool inCave = false;

        private List<List<int>> MazeMap;
        private int mazeNum;

        private int[] currentPos;
        private int[] winPos;

        private static Cave instance;

        private Random rand = new Random();

        private Cave()
        {

            Dictionary<int, List<List<int>>> MazeDict =
                new Dictionary<int, List<List<int>>>();

            string[] mapLines =
                File.ReadAllLines(@"PokemonLogs\MazeInfo.txt");

            List<List<int>> bufferList = new List<List<int>>();
            mazeNum = 0;
            foreach (string line in mapLines)
            {
                if (line == "")
                {
                    MazeDict.Add(mazeNum, bufferList);
                    bufferList = new List<List<int>>();
                    mazeNum++;
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
            int maze = rand.Next(mazeNum);
            MazeMap = MazeDict[maze];

            //foreach (var sublist in MazeMap) {
            //    foreach (var obj in sublist) {
            //        Console.Write(obj + " ");
            //    }
            //    Console.WriteLine("");
            //}
            //Console.WriteLine(""); 
            //Console.WriteLine("");

            //Random Stuff
            MazeMap = MazeGen.Generator.ChangeMaze(MazeMap);

            //foreach (var sublist in MazeMap) {
            //    foreach (var obj in sublist) {
            //        Console.Write(obj + " ");
            //    }
            //    Console.WriteLine("");
            //}
            //Console.WriteLine("");
            //Console.WriteLine("");

            MazeMap = Cave.ExtendMazeMap(MazeMap);

            currentPos = new int[2] { 1, 1 };
            winPos = new int[2] { MazeMap.Count - 2, MazeMap[0].Count - 2 };

            //Event OnMazeDraw
            OnMazeDraw(MazeMap);
            OnMovePlayer(currentPos);
        }

        private static List<List<int>> ExtendMazeMap(List<List<int>> MazeMap)
        {
            for (int i = 0; i < MazeMap.Count; i++)
            {
                MazeMap[i].Add(1);
                MazeMap[i].Insert(0, 1);

            }

            List<int> bufLine = new List<int>();
            for (int r = 0; r < MazeMap[0].Count; r++)
                bufLine.Add(1);
            MazeMap.Insert(0, bufLine);
            MazeMap.Add(bufLine);

            return MazeMap;
        }

        public static void OnCaveStart()
        {

            inCave = true;
            instance = new Cave();
        }

        public static void OnPlayerLeft()
        {
            instance = null;
            inCave = false;
            //Event End Cave (player left => "null")
            OnEndCave("null");
        }

        public static void OnPlayerMove(string dir)
        {
            int[] newPos = new int[2]
                { instance.currentPos[0], instance.currentPos[1] };

            switch (dir)
            {
                case "up":
                    newPos[0]--;
                    break;

                case "down":
                    newPos[0]++;
                    break;

                case "left":
                    newPos[1]--;
                    break;

                case "right":
                    newPos[1]++;
                    break;
            }
            if (instance.MazeMap[newPos[0]][newPos[1]] != 1)
            {
                instance.currentPos[0] = newPos[0];
                instance.currentPos[1] = newPos[1];

                //Event On Player Location Changed
                OnMovePlayer(instance.currentPos);

                if (newPos[0] == instance.winPos[0] && newPos[1] == instance.winPos[1])
                {
                    PokemonModel.PokemonInstance reward = PokemonModel.PokemonFactory.GetFullPokemon("better");

                    instance = null;
                    inCave = false;

                    //Event End Cave (win => somename)
                    OnEndCave(reward.Name);

                    PlayerModel.Player.Main.PokemonAdded(reward);
                }
            }
        }
    }
}
