using System;

namespace I_Projekt
{
    class BranchAndBound : Graph
    {
        public Stack Route { get; private set; }
        bool[] visited;
        private int[] smallestNumbersInRows;
        private int[] smallestNumbersInColumns;
        public int[,] auxCostMatrix;
        private bool[,] auxNeighborhoodMatrix;
        int optimumNewTravelCost;
        int[,] costMatrixBeforeUpdate;
        int[,] veryLastOptimumCostMatrix;
        int optimumVertex;

        public BranchAndBound(string filename, int choice) : base(filename, choice)
        {
            Route = new Stack();

            smallestNumbersInRows = new int[numOfCities];
            smallestNumbersInColumns = new int[numOfCities];
            auxCostMatrix = new int[numOfCities, numOfCities];
            auxCostMatrix = costMatrix;
            auxNeighborhoodMatrix = new bool[numOfCities, numOfCities];
            auxNeighborhoodMatrix = neighborhoodMatrix;
            visited = new bool[numOfCities];
            optimumNewTravelCost = int.MaxValue;
            costMatrixBeforeUpdate = new int[numOfCities, numOfCities];
            veryLastOptimumCostMatrix = new int[numOfCities, numOfCities];
            optimumVertex = -1;

            for (int i = 0; i < numOfCities; i++)
            {
                visited[i] = false;
                smallestNumbersInRows[i] = 0;
                smallestNumbersInColumns[i] = 0;
            }
        }

        private int CalculateRowReductionValue()
        {
            int cost = 0;

            for (int i = 0; i < numOfCities; i++)
            {
                cost += smallestNumbersInRows[i];
            }

            return cost;
        }

        private int CalculateColumnReductionValue()
        {
            int cost = 0;

            for (int i = 0; i < numOfCities; i++)
            {
                cost += smallestNumbersInColumns[i];
            }

            return cost;
        }

        private void ReduceRowsByConstants()
        {
            for (int i = 0; i < numOfCities; i++)
            {
                for (int j = 0; j < numOfCities; j++)
                {
                    if (auxCostMatrix[i, j] != int.MaxValue) auxCostMatrix[i, j] -= smallestNumbersInRows[i];
                }
            }
        }

        private void ReduceColumnsByConstants()
        {
            for (int i = 0; i < numOfCities; i++)
            {
                for (int j = 0; j < numOfCities; j++)
                {
                    if (auxCostMatrix[i, j] != int.MaxValue) auxCostMatrix[j, i] -= smallestNumbersInColumns[i];
                }
            }
        }

        private void SetInfinityInCell(int numberOfRow, int numberOfColumn)
        {
            auxCostMatrix[numberOfRow, numberOfColumn] = int.MaxValue;
        }

        private void SetInfinityInRowAndColumn(int numberOfRow, int numberOfColumn)
        {
            for (int i = 0; i < numOfCities; i++)
            {
                auxCostMatrix[numberOfRow, i] = int.MaxValue;
                auxCostMatrix[i, numberOfColumn] = int.MaxValue;
            }
        }

        private void FindSmallestNumbersInRows(int[,] costMatrix)
        {
            for (int i = 0; i < numOfCities; i++)
            {
                smallestNumbersInRows[i] = costMatrix[i, 0];
                for (int j = 1; j < numOfCities; j++)
                {
                    if (smallestNumbersInRows[i] > costMatrix[i, j]) smallestNumbersInRows[i] = costMatrix[i, j];
                }
            }

            for (int i = 0; i < numOfCities; i++)
            {
                if (smallestNumbersInRows[i] == int.MaxValue) smallestNumbersInRows[i] = 0;
            }
        }

        private void FindSmallestNumbersInColumns(int[,] costMatrix)
        {
            for (int i = 0; i < numOfCities; i++)
            {
                smallestNumbersInColumns[i] = costMatrix[0, i];
                for (int j = 1; j < numOfCities; j++)
                {
                    if (smallestNumbersInColumns[i] > costMatrix[j, i]) smallestNumbersInColumns[i] = costMatrix[j, i];
                }
            }

            for (int i = 0; i < numOfCities; i++)
            {
                if (smallestNumbersInColumns[i] == int.MaxValue) smallestNumbersInColumns[i] = 0;
            }
        }

        private void CopyMatrixFromTo(int[,] tableFrom, int[,] tableTo)
        {
            for (int i = 0; i < numOfCities; i++)
            {
                for (int j = 0; j < numOfCities; j++)
                {
                    tableTo[i, j] = tableFrom[i, j];
                }
            }
        }

        public void StartBranchAndBound(int startingVertex)
        {
            FindSmallestNumbersInRows(auxCostMatrix);
            int reducedValueInRows = CalculateRowReductionValue();
            ReduceRowsByConstants();
            FindSmallestNumbersInColumns(auxCostMatrix);
            int reducedValueInColumns = CalculateColumnReductionValue();
            ReduceColumnsByConstants();
            int firstTravelCost = reducedValueInRows + reducedValueInColumns;
            Route.Push(startingVertex);
            visited[startingVertex] = true;

            FindBestAdjacent(0, 0, 0, firstTravelCost);
        }

        public void FindBestAdjacent(int currentVertexLevel, int startingVertex, int currentVertex, int currentTravelCost)
        {
            int level = currentVertexLevel;
            int valueOfReducingRows;
            int valueOfReducingColumns;
            int newTravelCost;
            CopyMatrixFromTo(auxCostMatrix, costMatrixBeforeUpdate);

            if (Route.StackSize < numOfCities)
            {
                for (int i = 0; i < numOfCities; i++)
                {
                    if (!visited[i] && currentVertex != i)
                    {
                        SetInfinityInRowAndColumn(currentVertex, i);
                        SetInfinityInCell(i, currentVertex);
                        FindSmallestNumbersInRows(auxCostMatrix);
                        ReduceRowsByConstants();
                        valueOfReducingRows = CalculateRowReductionValue();
                        FindSmallestNumbersInColumns(auxCostMatrix);
                        ReduceColumnsByConstants();
                        valueOfReducingColumns = CalculateColumnReductionValue();
                        newTravelCost = currentTravelCost + valueOfReducingRows + valueOfReducingColumns + costMatrixBeforeUpdate[currentVertex, i];

                        if (currentVertexLevel == level || newTravelCost <= optimumNewTravelCost)
                        {
                            level++;
                            optimumNewTravelCost = newTravelCost;
                            BestCycleCost = optimumNewTravelCost;
                            optimumVertex = i;
                        }
                        CopyMatrixFromTo(costMatrixBeforeUpdate, auxCostMatrix);
                    }
                }

                visited[optimumVertex] = true;
                Route.Push(optimumVertex);
                FindBestAdjacent(currentVertexLevel + 1, startingVertex, optimumVertex, optimumNewTravelCost);
                visited[optimumVertex] = false;
                Route.Pop();
            }
            else
            {
                //optimumNewTravelCost += veryLastOptimumCostMatrix[optimumVertex, startingVertex];
                //BestCycleCost = optimumNewTravelCost;
                return;
            }
        }
    }
}
