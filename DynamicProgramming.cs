using System;
using System.Collections.Generic;

namespace I_Projekt
{
    class DynamicProgramming : Graph
    {
        public Stack Route { get; private set; }
        private readonly int allVisitedVerticesMask;
        private int startingVertex;
        private readonly int[,] allVerticesSubsets; // reprezentacja tablicy
        private readonly int[,] subPathIndexes; // reprezentacja tablicy
        //private List<PartialTravel> allVerticesSubsets; // reprezentacja listy
        //private List<PartialTravel> subPathIndexes; // reprezentacja listy

        public DynamicProgramming(string filename, int choice) : base(filename, choice)
        {
            //allVerticesSubsets = new List<PartialTravel>(); // reprezentacja listy
            //subPathIndexes = new List<PartialTravel>(); // reprezentacja listy
            Route = new Stack();
            allVisitedVerticesMask = (1 << numOfCities) - 1;
            allVerticesSubsets = new int[numOfCities, 1 << numOfCities]; // reprezentacja tablicy
            subPathIndexes = new int[numOfCities, 1 << numOfCities]; // reprezentacja tablicy

            for (int i = 0; i < numOfCities; i++)
            {
                for (int j = 0; j < allVisitedVerticesMask + 1; j++)
                {
                    allVerticesSubsets[i, j] = int.MaxValue; // reprezentacja tablicy
                    subPathIndexes[i, j] = int.MaxValue; // reprezentacja tablicy
                }
            }
        }

        public void StartDynamicProgramming(int startingVertex)
        {
            this.startingVertex = startingVertex;
            int currentStateOfVertices = 1 << startingVertex;
            BestCycleCost = StartDP(startingVertex, currentStateOfVertices);
            int index = startingVertex;

            for (int i = 0; i < numOfCities; i++)
            {
                Route.Push(index);
                //int nextIndex = GetNextIndex(index, currentStateOfVertices); // reprezentacja listy
                int nextIndex = subPathIndexes[index, currentStateOfVertices]; // reprezentacja tablicy
                currentStateOfVertices |= 1 << nextIndex;
                index = nextIndex;
            }

            Route.Push(startingVertex);
        }

        /*private int GetNextIndex(int index, int currentStateOfVertices) // reprezentacja listy
        {
            foreach (var subPath in subPathIndexes)
            {
                if (subPath.CurrentVertexIndex == index
                    && subPath.CurrentVerticesMask == currentStateOfVertices) return subPath.Value;
            }
            return -1;
        }*/


        /*private int GetNextValue(int currentVertex, int currentVerticesStateMask) // reprezentacja listy
        {
            foreach (var subpathCost in allVerticesSubsets)
            {
                if (subpathCost.CurrentVertexIndex == currentVertex
                    && subpathCost.CurrentVerticesMask == currentVerticesStateMask) return subpathCost.Value;
            }

            return -1;
        }*/

        private int StartDP(int currentVertex, int currentVerticesStateMask)
        {
            if (currentVerticesStateMask == allVisitedVerticesMask) return costMatrix[currentVertex, startingVertex];

            // jeśli wartość ścieżki została obliczona wcześniej to zwróć ją
            if (allVerticesSubsets[currentVertex, currentVerticesStateMask] != int.MaxValue) return allVerticesSubsets[currentVertex, currentVerticesStateMask]; // reprezentacja tablicy
            //int currentSubpathValue = GetNextValue(currentVertex, currentVerticesStateMask); // reprezentacja listy
            //if (currentSubpathValue != -1) return currentSubpathValue; // reprezentacja listy

            int minimumCostOfTravel = int.MaxValue;
            int currentVertexIndex = -1;

            for (int i = 0; i < numOfCities; i++)
            {
                // opuść obliczanie wartości trasy jeśli ten wierzchołek
                // został już odwiedzony wcześniej
                if ((currentVerticesStateMask & (1 << i)) != 0) continue;

                int nextStateMask = currentVerticesStateMask | (1 << i);
                int newCostOfTravel = costMatrix[currentVertex, i] + StartDP(i, nextStateMask);

                if (newCostOfTravel < minimumCostOfTravel)
                {
                    minimumCostOfTravel = newCostOfTravel;
                    currentVertexIndex = i;
                }
            }

            //subPathIndexes.Add(new PartialTravel(currentVertex, currentVerticesStateMask, currentVertexIndex)); // reprezentacja listy
            //allVerticesSubsets.Add(new PartialTravel(currentVertex, currentVerticesStateMask, minimumCostOfTravel)); // reprezentacja listy

            //return GetNextValue(currentVertex, currentVerticesStateMask); // reprezentacja listy
            subPathIndexes[currentVertex, currentVerticesStateMask] = currentVertexIndex; // reprezentacja tablicy
            allVerticesSubsets[currentVertex, currentVerticesStateMask] = minimumCostOfTravel; // reprezentacja tablicy

            return allVerticesSubsets[currentVertex, currentVerticesStateMask]; // reprezentacja tablicy
        }
    }
}
