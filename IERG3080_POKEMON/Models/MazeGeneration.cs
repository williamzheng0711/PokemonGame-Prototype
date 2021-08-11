using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MazeGen
{
    class Path
    {
        public static bool isSafe(int i, int j,
                          List<List<int>> matrix)
        {
            if (i >= 0 && i < matrix.Count &&
                j >= 0 && j < matrix[0].Count)
                return true;

            return false;
        }

        public static bool isPath(List<List<int>> matrix, int i, int j, int[] pos, bool[,] visited)
        {

            if (isSafe(i, j, matrix) && matrix[i][j] != 1 && !visited[i, j])
            {

                if (i == pos[0] && j == pos[1])
                    return true;

                visited[i, j] = true;

                bool up = isPath(matrix, i - 1, j, pos, visited);

                if (up)
                    return true;

                bool left = isPath(matrix, i, j - 1, pos, visited);

                if (left)
                    return true;

                bool down = isPath(matrix, i + 1, j, pos, visited);

                if (down)
                    return true;

                bool right = isPath(matrix, i, j + 1, pos, visited);

                if (right)
                    return true;
            }

            return false;
        }
    }

    class Generator
    {

        static public List<List<int>> ChangeMaze(List<List<int>> arr)
        {
            int xSize = arr[0].Count;
            int ySize = arr.Count;

            List<int> buf = new List<int>();
            buf.Add(1);
            buf.Add(((int)ySize / 2) - 1);
            buf.Add(ySize - 1);

            int iter = (int)xSize / 4;

            List<int[]> PosList = new List<int[]>();

            foreach (int line in buf)
            {
                int i = 0;
                while (i * iter < xSize - 2)
                {
                    PosList.Add(new int[] { line, i * iter });
                    i++;
                }
                PosList.Add(new int[] { line, xSize - 1 });
            }
            PosList.Add(new int[] { 12, 8 });

            List<List<int>> Arr = new List<List<int>>();
            foreach (List<int> line in arr)
                Arr.Add(new List<int>(line));

            Random rand = new Random();
            for (int i = 0; i < 300; i++)
            {
                List<List<int>> newArr = new List<List<int>>();
                foreach (List<int> line in Arr)
                    newArr.Add(new List<int>(line));

                int randY = rand.Next(ySize);
                int randX = rand.Next(xSize);
                newArr[randY][randX] = 1;
                bool validChange = true;

                foreach (int[] pos in PosList)
                {
                    bool[,] visited = new bool[ySize, xSize];
                    if (!Path.isPath(newArr, 0, 0, pos, visited))
                    {
                        validChange = false;
                        break;
                    }
                }
                if (validChange)
                    Arr = newArr;
            }
            return Arr;
        }
    }
}
