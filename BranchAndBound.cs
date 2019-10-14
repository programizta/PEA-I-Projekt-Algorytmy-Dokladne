using System;
using System.Collections.Generic;

namespace I_Projekt
{
    class BranchAndBound : Graph
    {
        public Stack Route { get; private set; }
        public List<Travel> ListOfTravelParamsOnLevel;
        public List<Travel> ListOfSuspiciousVertexParams { get; set; }
        bool[] visited;
        private int[] smallestNumbersInRows;
        private int[] smallestNumbersInColumns;
        public int[,] auxCostMatrix;
        int optimumNewTravelCost;
        int[,] costMatrixBeforeUpdate;
        int optimumVertexOnCurrentLevel;
        int optimumVertexOnPreviousLevel;
        int bestValueOfTravelOnCurrentLevel;
        int secondBestValueOfTravelOnPreviousLevel;
        int betterVertex;
        public BranchAndBound(string filename, int choice) : base(filename, choice)
        {
            Route = new Stack();
            ListOfTravelParamsOnLevel = new List<Travel>();
            ListOfSuspiciousVertexParams = new List<Travel>();

            betterVertex = -1;
            smallestNumbersInRows = new int[numOfCities];
            smallestNumbersInColumns = new int[numOfCities];
            auxCostMatrix = new int[numOfCities, numOfCities];
            CopyMatrixFromTo(costMatrix, auxCostMatrix);
            visited = new bool[numOfCities];
            optimumNewTravelCost = int.MaxValue;
            costMatrixBeforeUpdate = new int[numOfCities, numOfCities];
            optimumVertexOnCurrentLevel = -1;

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

        private void ReduceRowsByConstants(int[,] currentCostMatrix)
        {
            for (int i = 0; i < numOfCities; i++)
            {
                for (int j = 0; j < numOfCities; j++)
                {
                    if (currentCostMatrix[i, j] != int.MaxValue) currentCostMatrix[i, j] -= smallestNumbersInRows[i];
                }
            }
        }

        private void ReduceColumnsByConstants(int[,] currentCostMatrix)
        {
            for (int j = 0; j < numOfCities; j++)
            {
                for (int i = 0; i < numOfCities; i++)
                {
                    if (currentCostMatrix[i, j] != int.MaxValue) currentCostMatrix[i, j] -= smallestNumbersInColumns[j];
                }
            }
        }

        private void SetInfinityInCell(int[,] currentCostMatrix, int numberOfRow, int numberOfColumn)
        {
            currentCostMatrix[numberOfRow, numberOfColumn] = int.MaxValue;
        }

        private void SetInfinityInRowAndColumn(int[,] currentCostMatrix, int numberOfRow, int numberOfColumn)
        {
            for (int i = 0; i < numOfCities; i++)
            {
                currentCostMatrix[numberOfRow, i] = int.MaxValue;
                currentCostMatrix[i, numberOfColumn] = int.MaxValue;
            }
        }

        private void FindSmallestNumbersInRows(int[,] costMatrix)
        {
            for (int i = 0; i < numOfCities; i++)
            {
                smallestNumbersInRows[i] = int.MaxValue;
                for (int j = 0; j < numOfCities; j++)
                {
                    if (costMatrix[i, j] < smallestNumbersInRows[i]) smallestNumbersInRows[i] = costMatrix[i, j];
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
                smallestNumbersInColumns[i] = int.MaxValue;
                for (int j = 0; j < numOfCities; j++)
                {
                    if (costMatrix[j, i] < smallestNumbersInColumns[i]) smallestNumbersInColumns[i] = costMatrix[j, i];
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

        private int ComputeOptimumVertexAndCost(int level, int valueOfTravelOnLevel, bool firstBest)
        {
            int secondValueOfTravelOnLevel = -1;
            int auxIndex = -1;
            valueOfTravelOnLevel = int.MaxValue;
            int index = -1;

            foreach (var travel in ListOfTravelParamsOnLevel)
            {
                if (travel.GetCostOnLevel() <= valueOfTravelOnLevel /*&& !visited[travel.GetIndex()]*/ && travel.GetLevel() == level)
                {
                    secondValueOfTravelOnLevel = valueOfTravelOnLevel;
                    auxIndex = index;
                    valueOfTravelOnLevel = travel.GetCostOnLevel();
                    index = travel.GetIndex();
                }
            }

            if (firstBest)
            {
                bestValueOfTravelOnCurrentLevel = valueOfTravelOnLevel;
                return index;
            }

            secondBestValueOfTravelOnPreviousLevel = secondValueOfTravelOnLevel;
            return auxIndex;
        }

        private int[,] GetOptimumCostMatrix(int index, int level)
        {
            foreach (var travel in ListOfTravelParamsOnLevel)
            {
                if (travel.GetIndex() == index && travel.GetLevel() == level) return travel.GetCurrentCostMatrix();
            }

            return null;
        }

        public void StartBranchAndBound(int startingVertex)
        {
            FindSmallestNumbersInRows(auxCostMatrix);
            int reducedValueInRows = CalculateRowReductionValue();
            ReduceRowsByConstants(auxCostMatrix);
            FindSmallestNumbersInColumns(auxCostMatrix);
            int reducedValueInColumns = CalculateColumnReductionValue();
            ReduceColumnsByConstants(auxCostMatrix);
            int firstTravelCost = reducedValueInRows + reducedValueInColumns;
            Route.Push(startingVertex);
            visited[startingVertex] = true;

            FindBestAdjacent(0, 0, firstTravelCost, 0);
        }

        public void FindBestAdjacent(int startingVertex, int currentVertex, int currentTravelCost, int level)
        {
            int valueOfReducingRows;
            int valueOfReducingColumns;
            int newTravelCost = 0;
            CopyMatrixFromTo(auxCostMatrix, costMatrixBeforeUpdate);

            if (Route.StackSize < numOfCities)
            {
                for (int i = 0; i < numOfCities; i++)
                {
                    if (!visited[i] && currentVertex != i)
                    {
                        SetInfinityInRowAndColumn(auxCostMatrix, currentVertex, i);
                        SetInfinityInCell(auxCostMatrix, currentVertex, startingVertex);
                        FindSmallestNumbersInRows(auxCostMatrix);
                        ReduceRowsByConstants(auxCostMatrix);
                        valueOfReducingRows = CalculateRowReductionValue();
                        FindSmallestNumbersInColumns(auxCostMatrix);
                        ReduceColumnsByConstants(auxCostMatrix);
                        valueOfReducingColumns = CalculateColumnReductionValue();
                        newTravelCost = currentTravelCost + valueOfReducingRows + valueOfReducingColumns + costMatrixBeforeUpdate[currentVertex, i];

                        ListOfTravelParamsOnLevel.Add(new Travel(i, newTravelCost, numOfCities, auxCostMatrix, level));
                        CopyMatrixFromTo(costMatrixBeforeUpdate, auxCostMatrix);
                    }
                }

                if (level == 0)
                {
                    optimumVertexOnCurrentLevel = ComputeOptimumVertexAndCost(level, bestValueOfTravelOnCurrentLevel, true);
                    optimumNewTravelCost = bestValueOfTravelOnCurrentLevel;
                    betterVertex = optimumVertexOnCurrentLevel;
                }
                else
                {
                    optimumVertexOnCurrentLevel = ComputeOptimumVertexAndCost(level, bestValueOfTravelOnCurrentLevel, true);
                    optimumVertexOnPreviousLevel = ComputeOptimumVertexAndCost(level - 1, secondBestValueOfTravelOnPreviousLevel, false);

                    if (bestValueOfTravelOnCurrentLevel <= secondBestValueOfTravelOnPreviousLevel)
                    {
                        optimumNewTravelCost = bestValueOfTravelOnCurrentLevel;
                        betterVertex = optimumVertexOnCurrentLevel;
                    }
                    else
                    {
                        optimumNewTravelCost = secondBestValueOfTravelOnPreviousLevel;
                        betterVertex = optimumVertexOnPreviousLevel;
                        level--;
                    }
                }

                BestCycleCost = optimumNewTravelCost;
                auxCostMatrix = GetOptimumCostMatrix(betterVertex, level);
                visited[betterVertex] = true;
                Route.Push(betterVertex);
                //ListOfTravelParamsOnLevel.Clear();
                FindBestAdjacent(startingVertex, betterVertex, optimumNewTravelCost, level + 1);
            }
            else
            {
                BestCycleCost += auxCostMatrix[Route.Top(), 0];
                Route.Push(0);
                return;
            }
        }
    }
}
