using System;

namespace I_Projekt
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph g = new Graph();
            string filename;
            int numOfChoice;

            while (true)
            {
                Console.Clear();
                Console.Write("Program do wyznaczania optymalnego cyklu Hamiltona dla asymetrycznego problemu komiwojażera (ATSP)");

                if (g.GetNumberOfCities() != 0)
                {
                    Console.WriteLine("\n\nLiczba wierzchołków aktualnie wczytanego grafu: " + g.GetNumberOfCities());
                    Console.Write(Environment.NewLine);
                }
                else
                {
                    Console.WriteLine("\n\nAktualnie nie wczytano żadnego grafu");
                    Console.Write(Environment.NewLine);
                }

                Console.WriteLine("Możliwości do wyboru: ");
                Console.WriteLine("1. Wczytaj małą macierz grafu");
                Console.WriteLine("2. Wczytaj dużą macierz grafu");
                Console.WriteLine("3. Wyświetl macierz kosztów");
                Console.WriteLine("4. Wyświetl macierz sąsiedztwa");
                Console.WriteLine("5. Rozwiąż problem komiwojażera za pomocą algorytmu Brute Force");
                Console.WriteLine("6. Rozwiąż problem komiwojażera za pomocą algorytmu Podziału i Ograniczeń");
                Console.WriteLine("7. Rozwiąż problem komiwojażera za pomocą metody programowania dynamicznego");
                Console.WriteLine("8. Zakończ działanie programu\n");
                Console.Write("Którą opcję chcesz wybrać? Podaj numer: ");
                numOfChoice = int.Parse(Console.ReadLine());

                switch (numOfChoice)
                {
                    case 1:
                        {
                            Console.Clear();
                            Console.Write("Podaj nazwę pliku z małym grafem: ");
                            filename = Console.ReadLine();
                            g = new Graph(filename, 0);
                            Console.Write("Wczytano graf z " + g.GetNumberOfCities() + " wierzchołkami\nAby kontynuować kliknij [ENTER]");
                            Console.ReadKey();
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            Console.Write("Podaj nazwę pliku z dużym grafem: ");
                            filename = Console.ReadLine();
                            g = new Graph(filename, 1);
                            Console.Write("Wczytano graf z " + g.GetNumberOfCities() + " wierzchołkami\nAby kontynuować kliknij [ENTER]");
                            Console.ReadKey();
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            if (g.GetNumberOfCities() != 0) g.DisplayCostMatrix();
                            else Console.WriteLine("Nie wczytano żadnego grafu do programu!");
                            Console.Write("\nAby kontynuować kliknij [ENTER]");
                            Console.ReadKey();
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            if (g.GetNumberOfCities() != 0) g.DisplayNeighborhoodMatrix();
                            else Console.WriteLine("Nie wczytano żadnego grafu do programu!");
                            Console.Write("\nAby kontynuować kliknij [ENTER]");
                            Console.ReadKey();
                            break;
                        }
                    case 5:
                        {
                            BruteForce bf = new BruteForce(g.Filename, 0);
                            bf.StartBruteForce(0);
                            Console.WriteLine("Najlepszy cykl ma wagę: " + bf.BestCycleCost);
                            Console.WriteLine("Optymalny cykl:");
                            bf.Route.Display();
                            Console.WriteLine("\nKoniec. Aby wrócić do głównego menu, kliknij dowolny klawisz...");
                            Console.ReadKey();
                            break;
                        }
                    case 6:
                        {
                            BranchAndBound bb = new BranchAndBound(g.Filename, 0);
                            bb.StartBranchAndBound(0);
                            Console.WriteLine("Najlepszy cykl ma wagę: " + bb.BestCycleCost);
                            Console.WriteLine("Optymalny cykl:");
                            bb.Route.Display();
                            Console.WriteLine("\nKoniec. Aby wrócić do głównego menu, kliknij dowolny klawisz...");
                            Console.ReadKey();
                            break;
                        }
                    case 7:
                        {
                            DynamicProgramming dp = new DynamicProgramming(g.Filename, 0);
                            dp.StartDynamicProgramming(0);
                            Console.WriteLine("Najlepszy cykl ma wagę: " + dp.BestCycleCost);
                            Console.WriteLine("Optymalny cykl:");
                            dp.Route.Display();
                            Console.WriteLine("\nKoniec. Aby wrócić do głównego menu, kliknij dowolny klawisz...");
                            Console.ReadKey();
                            break;
                        }
                    case 8:
                        {
                            Console.Write("\nZakończono działanie programu\nAby kontynuować kliknij [ENTER]");
                            Console.ReadKey();
                            return;
                        }
                }
            }
        }
    }
}
