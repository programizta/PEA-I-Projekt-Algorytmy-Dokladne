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

        public Travel(int index, int costOnLevel, int matrixSize, int[,] matrixCost)
        {
            this.index = index;
            this.matrixSize = matrixSize;
            this.costOnLevel = costOnLevel;

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
