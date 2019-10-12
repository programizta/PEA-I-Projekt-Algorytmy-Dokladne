using System;
using System.Collections.Generic;

namespace I_Projekt
{
    class BranchAndBound : Graph
    {
        /// <summary>
        /// Item1 - indeks
        /// Item2 - koszt drogi
        /// </summary>
        public List<Tuple<int, int>> NotVisitedVertexes { get; set; }
        /// <summary>
        /// Item1 - indeks
        /// Item2 - macierz kosztów
        /// </summary>
        public List<Tuple<int, int[,]>> ListOfCostMatrixes { get; set; }
        public Stack Route { get; private set; }
        bool[] visited;
        private int[] smallestNumbersInRows;
        private int[] smallestNumbersInColumns;
        public int[,] auxCostMatrix;
        private bool[,] auxNeighborhoodMatrix;
        int optimumNewTravelCost;
        int[,] costMatrixBeforeUpdate;
        int optimumVertex;
        int valueOfTravelOnLevel;
        public BranchAndBound(string filename, int choice) : base(filename, choice)
        {
            Route = new Stack();
            NotVisitedVertexes = new List<Tuple<int, int>>();
            ListOfCostMatrixes = new List<Tuple<int, int[,]>>();

            smallestNumbersInRows = new int[numOfCities];
            smallestNumbersInColumns = new int[numOfCities];
            auxCostMatrix = new int[numOfCities, numOfCities];
            auxCostMatrix = costMatrix;
            auxNeighborhoodMatrix = new bool[numOfCities, numOfCities];
            auxNeighborhoodMatrix = neighborhoodMatrix;
            visited = new bool[numOfCities];
            optimumNewTravelCost = int.MaxValue;
            costMatrixBeforeUpdate = new int[numOfCities, numOfCities];
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
                    if (auxCostMatrix[i, j] != int.MaxValue) auxCostMatrix[i, j] -= smallestNumbersInColumns[j];
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

        private int GetOptimumVertexIndex()
        {
            valueOfTravelOnLevel = int.MaxValue;
            int index = -1;

            foreach (var tuple in NotVisitedVertexes)
            {
                if (tuple.Item2 <= valueOfTravelOnLevel && !visited[tuple.Item1])
                {
                    valueOfTravelOnLevel = tuple.Item2;
                    index = tuple.Item1;
                }
            }

            return index;
        }

        private int[,] GetOptimumCostMatrix(int index)
        {
            foreach (var tupple in ListOfCostMatrixes)
            {
                if (tupple.Item1 == index) return tupple.Item2;
            }
            return null;
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

            FindBestAdjacent(0, firstTravelCost, auxCostMatrix);
        }

        public void FindBestAdjacent(int currentVertex, int currentTravelCost, int[,] currentMatrix)
        {
            int valueOfReducingRows;
            int valueOfReducingColumns;
            int newTravelCost;
            CopyMatrixFromTo(currentMatrix, costMatrixBeforeUpdate);
            int[,] costMatrixOnLevel = new int[numOfCities, numOfCities];

            if (Route.StackSize < numOfCities)
            {
                for (int i = 0; i < numOfCities; i++)
                {
                    if (!visited[i] && currentVertex != i)
                    {
                        CopyMatrixFromTo(costMatrixBeforeUpdate, currentMatrix);
                        SetInfinityInRowAndColumn(currentVertex, i);
                        SetInfinityInCell(i, currentVertex);
                        FindSmallestNumbersInRows(currentMatrix);
                        ReduceRowsByConstants();
                        valueOfReducingRows = CalculateRowReductionValue();
                        FindSmallestNumbersInColumns(currentMatrix);
                        ReduceColumnsByConstants();
                        valueOfReducingColumns = CalculateColumnReductionValue();
                        newTravelCost = currentTravelCost + valueOfReducingRows + valueOfReducingColumns + costMatrixBeforeUpdate[currentVertex, i];

                        NotVisitedVertexes.Add(new Tuple<int, int>(i, newTravelCost));
                        ListOfCostMatrixes.Add(new Tuple<int, int[,]>(i, currentMatrix));
                    }
                }

                optimumVertex = GetOptimumVertexIndex();
                optimumNewTravelCost = valueOfTravelOnLevel;
                BestCycleCost = optimumNewTravelCost;
                currentMatrix = GetOptimumCostMatrix(optimumVertex); // tu 
                visited[optimumVertex] = true;
                Route.Push(optimumVertex);
                NotVisitedVertexes.Clear();
                ListOfCostMatrixes.Clear();
                FindBestAdjacent(optimumVertex, optimumNewTravelCost, currentMatrix);
                visited[optimumVertex] = false;
                Route.Pop();
            }
            else
            {
                return;
            }
        }
    }
}
