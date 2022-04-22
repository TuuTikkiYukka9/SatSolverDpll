using System;
using System.IO;

namespace DPLL_DLIS
{

    class Program
    {

        static void Main()
        {
            try
            {

                Console.WriteLine("Введите имя файла:");
                string fileName = Console.ReadLine();

                if (!File.Exists(fileName))
                {
                    throw new Exception($"Файл {fileName} не найден.");
                }
                var cnf = DimacsParser.ReadFile(fileName);

                bool sat = SatSolver.SolveDpll(cnf);
                Console.WriteLine($"SAT: {sat}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}

