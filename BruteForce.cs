namespace I_Projekt
{
    class BruteForce : Graph
    {
        public Stack Route { get; private set; }
        public Stack AuxRoute { get; private set; }
        bool[] visited;
        readonly int startingVerticle;
        int auxCycleCost;

        public BruteForce(string filename, int choice) : base(filename, choice)
        {
            BestCycleCost = int.MaxValue;
            Route = new Stack();
            InitializeVisitTable();
            AuxRoute = new Stack();
            auxCycleCost = 0;
            startingVerticle = 0;
        }

        private void InitializeVisitTable()
        {
            visited = new bool[numOfCities];

            for (int i = 0; i < numOfCities; i++)
            {
                visited[i] = false;
            }
        }

        public void StartBruteForce(int currentVerticle)
        {
            int i;

            AuxRoute.Push(currentVerticle);

            if (AuxRoute.StackSize < numOfCities)
            {
                visited[currentVerticle] = true;

                for (i = 0; i < numOfCities; i++)
                {
                    if (neighborhoodMatrix[currentVerticle, i] && !visited[i])
                    {
                        auxCycleCost += costMatrix[currentVerticle, i];
                        StartBruteForce(i);
                        auxCycleCost -= costMatrix[currentVerticle, i];
                    }
                }
                visited[currentVerticle] = false;
            }
            else if(neighborhoodMatrix[startingVerticle, currentVerticle])
            {
                auxCycleCost += costMatrix[currentVerticle, startingVerticle];

                if (auxCycleCost < BestCycleCost)
                {
                    BestCycleCost = auxCycleCost;
                    Route = AuxRoute;
                }
                auxCycleCost -= costMatrix[currentVerticle, startingVerticle];
            }
            AuxRoute.Pop();
        }
    }
}
