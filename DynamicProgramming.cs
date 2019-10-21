using System;
using System.Collections.Generic;

namespace I_Projekt
{
    class DynamicProgramming : Graph
    {
        public Stack Route { get; private set; }
        private readonly int allVisitedVerticesMask;
        private int startingVertex;
        private int[,] allVerticesSubsets;
<<<<<<< HEAD
        private int[,] subPathIndexes;
=======
        private int[,] previousStateOfAllVertices;
>>>>>>> 59beb6fb01648f14092ca129599744a993e53714

        public DynamicProgramming(string filename, int choice) : base(filename, choice)
        {
            Route = new Stack();
            allVisitedVerticesMask = (1 << numOfCities) - 1;
            allVerticesSubsets = new int[numOfCities, 1 << numOfCities];
<<<<<<< HEAD
            subPathIndexes = new int[numOfCities, 1 << numOfCities];
=======
            previousStateOfAllVertices = new int[numOfCities, 1 << numOfCities];
>>>>>>> 59beb6fb01648f14092ca129599744a993e53714

            for (int i = 0; i < numOfCities; i++)
            {
                for (int j = 0; j < allVisitedVerticesMask + 1; j++)
                {
                    allVerticesSubsets[i, j] = int.MaxValue;
<<<<<<< HEAD
                    subPathIndexes[i, j] = int.MaxValue;
=======
                    previousStateOfAllVertices[i, j] = int.MaxValue;
>>>>>>> 59beb6fb01648f14092ca129599744a993e53714
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
<<<<<<< HEAD
                int nextIndex = subPathIndexes[index, currentStateOfVertices];
                currentStateOfVertices |= 1 << nextIndex;
=======
                int nextIndex = previousStateOfAllVertices[index, currentStateOfVertices];
                if (nextIndex == int.MaxValue) break;
                currentStateOfVertices = currentStateOfVertices | (1 << nextIndex);
>>>>>>> 59beb6fb01648f14092ca129599744a993e53714
                index = nextIndex;
            }

            Route.Push(startingVertex);
        }

        private int StartDP(int currentVertex, int currentVerticesStateMask)
        {
            if (currentVerticesStateMask == allVisitedVerticesMask) return costMatrix[currentVertex, startingVertex];

            // jeśli wartość ścieżki została obliczona wcześniej to zwróć ją
            if (allVerticesSubsets[currentVertex, currentVerticesStateMask] != int.MaxValue) return allVerticesSubsets[currentVertex, currentVerticesStateMask];

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

<<<<<<< HEAD
            subPathIndexes[currentVertex, currentVerticesStateMask] = currentVertexIndex;
=======
            previousStateOfAllVertices[currentVertex, currentVerticesStateMask] = currentVertexIndex;
>>>>>>> 59beb6fb01648f14092ca129599744a993e53714
            allVerticesSubsets[currentVertex, currentVerticesStateMask] = minimumCostOfTravel;

            return allVerticesSubsets[currentVertex, currentVerticesStateMask];
        }
    }
}
