using System;
using System.Collections.Generic;
using System.IO;


namespace DPLL_DLIS
{

    static class SatSolver
    {
        /// <summary>
        /// DPLL - работает используя полный перебор. 
        /// Завершается sat - если удалось подобрать значения при которых нет ни одного ложного дизъюнкта.
        /// При обнаружении конфликта возвращаемся к хронологически последнему ветвлению.
        /// </summary>
        /// <param name="cnf">решаемый кнф</param>
        /// <returns>sat/unsat</returns>
        static public bool SolveDpll(CNF cnf)
        {
            var assignment = new Assignment();
            uint maxWhileCount =  uint.MaxValue;
            uint l = 0;
            //существует хотя бы один не назначенный литерал в cnf
            while(cnf.IsAnyUnknownLiteral())
            {
                //существует хотя бы один еденичный дизъюнкт
                while(cnf.IsAnyUnitClause())
                {
                    assignment.UnitPropagate(cnf);
                    while(cnf.IsUnsat())
                    {
                        //есть хоть один рещающий литерал, то откатываемся назад
                        if(assignment.IsAnyDecideLiteral())
                        {
                            assignment.BackjumpDpll(cnf);
                        }
                        else//правило Fail
                        {
                            return false;
                        }
                    }
                }
                
                if(cnf.IsAnyUnknownLiteral())
                {
                    assignment.Decide(cnf);
                }
                else
                {
                    break;
                }

                if(l>maxWhileCount)
                {
                    throw new Exception("Превышена максимально допустимая глубина цикличности.");
                } 
                l++;
            }

            Console.WriteLine("CNF");
            foreach(var cl in cnf.Clauses)
            {
                Console.WriteLine(cl.ToString());
            }
            Console.WriteLine(assignment.GetStatistics());
            Console.WriteLine("Variables");
            Console.WriteLine(cnf.VariablesValueToString());
            
            return true;
        }
    }
}