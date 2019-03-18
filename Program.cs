using System;
using System.Collections.Generic;
using System.IO;


//lab2ex.cnf.txt
//simple_v3_c2.cnf.txt
//quinn.cnf.txt
//lec2ex.cnf.txt
//par8-1-c.cnf.txt
//aim-50-1_6-yes1-4.cnf.txt
// unsat --- hole6.cnf.txt
// unsat --- dubois20.cnf.txt

namespace DPLL_DLIS
{

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //if(args.Length==0)
                //{
                //    throw new Exception("Вы не выбради CNF file");
                //}
                //string fileName = args[0];

                string fileName = "/home/julija/Рабочий стол/DPLL_DLIS/cnf/quinn.cnf";
                if(!File.Exists(fileName))
                {
                    throw new Exception("Файл "+fileName+" не найден.");
                }
                var cnf = DimacsParser.ReadFile(fileName);
  
                bool sat = SatSolver.SolveDpll(cnf);
                Console.WriteLine("SAT:"+sat.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ошибка:"+ ex.Message);
            }
        }


    }

}

