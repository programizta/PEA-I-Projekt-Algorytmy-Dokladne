using System;
using System.Diagnostics;

namespace I_Projekt
{
    class Program
    {
        static string filename;
        static int choice;

        static void Tests()
        {
            int repeats;
            int choiceNumber;
            Console.Clear();
            Console.Write("Ile powtórzeń chciałbyś wykonać? Podaj liczbę: ");
            repeats = int.Parse(Console.ReadLine());
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nMożliwości do wyboru:\n");
                Console.WriteLine("1. Brute Force");
                Console.WriteLine("2. Programowanie dynamiczne");
                Console.WriteLine("3. Powrót do głównego menu\n");
                Console.Write("Którą opcję wybierasz? Wprowadź jej numer: ");
                choiceNumber = int.Parse(Console.ReadLine());

                switch (choiceNumber)
                {
                    case 1:
                        {
                            Stopwatch stopWatch = new Stopwatch();
                            stopWatch.Start();
                            for (int i = 0; i < repeats; i++)
                            {
                                BruteForce bf = new BruteForce(filename, choice);
                                bf.StartBruteForce(0);
                            }
                            stopWatch.Stop();
                            TimeSpan ts = stopWatch.Elapsed;
                            Console.WriteLine("\nŚredni czas wykonania algorytmu Brute Force wynosi " + ts.TotalMilliseconds / repeats + " ms");
                            Console.ReadKey();
                            break;
                        }
                    case 2:
                        {
                            Stopwatch stopWatch = new Stopwatch();
                            stopWatch.Start();
                            for (int i = 0; i < repeats; i++)
                            {
                                DynamicProgramming dp = new DynamicProgramming(filename, choice);
                                dp.StartDynamicProgramming(0);
                            }
                            stopWatch.Stop();
                            TimeSpan ts = stopWatch.Elapsed;
                            Console.WriteLine("\nŚredni czas wykonania metody programowania dynamicznego" +
                                " wynosi " + ts.TotalMilliseconds / repeats + " ms");
                            Console.ReadKey();
                            break;
                        }
                    case 3: return;
                    default:
                        {
                            Console.Clear();
                            Console.WriteLine("Taka opcja nie istnieje!");
                            Console.ReadKey();
                            break;
                        }
                }
            }
        }

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
                Console.WriteLine("5. Rozwiąż problem komiwojażera za pomocą metody Brute Force");
                Console.WriteLine("6. Rozwiąż problem komiwojażera za pomocą metody programowania dynamicznego");
                Console.WriteLine("7. Przeprowadź testy seryjne");
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
                            Program.filename = filename;
                            choice = 0;
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
                            Program.filename = filename;
                            choice = 1;
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

                            // by zadeklarować konkretny wierzchołek początkowy należy zmienić
                            // na zgodną wartość podawane liczby, będące numerem początkowego wierzchołka
                            bf.SetStartingVertex(0);
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
                            DynamicProgramming dp = new DynamicProgramming(g.Filename, choice);
                            dp.StartDynamicProgramming(0);
                            Console.WriteLine("Najlepszy cykl ma wagę: " + dp.BestCycleCost);
                            Console.WriteLine("Optymalny cykl:");
                            dp.Route.Display();
                            Console.WriteLine("\nKoniec. Aby wrócić do głównego menu, kliknij dowolny klawisz...");
                            Console.ReadKey();
                            break;
                        }
                    case 7:
                        {
                            Tests();
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
