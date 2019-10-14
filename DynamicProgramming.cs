using System;

namespace I_Projekt
{
    class DynamicProgramming : Graph
    {
        public Stack Route { get; private set; }
        private readonly int allVisitedVerticesMask;
        private int startingVertex;
        private int[,] allVerticlesSubsets;
        private int[,] previousStateOfAllVertices;

        public DynamicProgramming(string filename, int choice) : base(filename, choice)
        {
            Route = new Stack();
            allVisitedVerticesMask = (1 << numOfCities) - 1;
            allVerticlesSubsets = new int[numOfCities, 1 << numOfCities];
            previousStateOfAllVertices = new int[numOfCities, 1 << numOfCities];

            for (int i = 0; i < numOfCities; i++)
            {
                for (int j = 0; j < allVisitedVerticesMask + 1; j++)
                {
                    allVerticlesSubsets[i, j] = int.MaxValue;
                    previousStateOfAllVertices[i, j] = int.MaxValue;
                }
            }
        }

        public void StartDynamicProgramming(int startingVertex)
        {
            this.startingVertex = startingVertex;
            int currentStateOfVertices = 1 << startingVertex;
            BestCycleCost = StartDP(startingVertex, currentStateOfVertices);
            int index = startingVertex;

            while (true)
            {
                Route.Push(index);
                int nextIndex = previousStateOfAllVertices[index, currentStateOfVertices];
                if (nextIndex == int.MaxValue) break;
                int nextSubProblemState = currentStateOfVertices | (1 << nextIndex);
                currentStateOfVertices = nextSubProblemState;
                index = nextIndex;
            }

            Route.Push(startingVertex);
        }

        private int StartDP(int currentVertex, int currentVerticesStateMask)
        {
            if (currentVerticesStateMask == allVisitedVerticesMask) return costMatrix[currentVertex, startingVertex];
            if (allVerticlesSubsets[currentVertex, currentVerticesStateMask] != int.MaxValue) return allVerticlesSubsets[currentVertex, currentVerticesStateMask];

            int minimumCostOfTravel = int.MaxValue;
            int currentVertexIndex = -1;

            for (int i = 0; i < numOfCities; i++)
            {
                if ((currentVerticesStateMask & (1 << i)) != 0) continue;

                int nextStateMask = currentVerticesStateMask | (1 << i);
                int newCostOfTravel = costMatrix[currentVertex, i] + StartDP(i, nextStateMask);

                if (newCostOfTravel < minimumCostOfTravel)
                {
                    minimumCostOfTravel = newCostOfTravel;
                    currentVertexIndex = i;
                }
            }

            previousStateOfAllVertices[currentVertex, currentVerticesStateMask] = currentVertexIndex;
            allVerticlesSubsets[currentVertex, currentVerticesStateMask] = minimumCostOfTravel;

            return allVerticlesSubsets[currentVertex, currentVerticesStateMask];
        }
    }
}
