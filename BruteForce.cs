namespace I_Projekt
{
    class BruteForce : Graph
    {
        public Stack Route { get; private set; }
        public Stack AuxRoute { get; private set; }
        bool[] visited;
        int startingVertex;
        int auxCycleCost;

        public BruteForce(string filename, int choice) : base(filename, choice)
        {
            BestCycleCost = int.MaxValue;
            Route = new Stack();
            InitializeVisitTable();
            AuxRoute = new Stack();
            auxCycleCost = 0;
            startingVertex = 0;
        }

        private void InitializeVisitTable()
        {
            visited = new bool[numOfCities];

            for (int i = 0; i < numOfCities; i++)
            {
                visited[i] = false;
            }
        }

        public void SetStartingVertex(int startingVertex)
        {
            this.startingVertex = startingVertex;
        }

        public void StartBruteForce(int currentVertex)
        {
            int i;

            AuxRoute.Push(currentVertex);

            if (AuxRoute.StackSize < numOfCities)
            {
                visited[currentVertex] = true;

                for (i = 0; i < numOfCities; i++)
                {
                    if (neighborhoodMatrix[currentVertex, i] && !visited[i])
                    {
                        auxCycleCost += costMatrix[currentVertex, i];
                        StartBruteForce(i);
                        auxCycleCost -= costMatrix[currentVertex, i];
                    }
                }
                visited[currentVertex] = false;
            }
            else if(neighborhoodMatrix[startingVertex, currentVertex])
            {
                auxCycleCost += costMatrix[currentVertex, startingVertex];

                if (auxCycleCost < BestCycleCost)
                {
                    Route.Clear();
                    BestCycleCost = auxCycleCost;
                    Route.CopyFrom(AuxRoute);
                    Route.Push(startingVertex);
                }
                auxCycleCost -= costMatrix[currentVertex, startingVertex];
            }
            AuxRoute.Pop();
        }
    }
}
