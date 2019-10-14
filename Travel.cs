using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I_Projekt
{
    class Travel
    {
        int index;
        int costOnLevel;
        int[,] currentCostMatrix;
        int matrixSize;
        int level;

        public Travel(int index, int costOnLevel, int matrixSize, int[,] matrixCost, int level)
        {
            this.index = index;
            this.matrixSize = matrixSize;
            this.costOnLevel = costOnLevel;
            this.level = level;

            currentCostMatrix = new int[matrixSize, matrixSize];
            InitializeMatrix(matrixCost);
        }

        private void InitializeMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    currentCostMatrix[i, j] = matrix[i, j];
                }
            }
        }

        public int GetIndex()
        {
            return index;
        }

        public int GetLevel()
        {
            return level;
        }

        public int GetCostOnLevel()
        {
            return costOnLevel;
        }

        public int[,] GetCurrentCostMatrix()
        {
            return currentCostMatrix;
        }
    }
}
