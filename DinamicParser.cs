using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DPLL_DLIS;

namespace DPLL_DLIS
{

    /// <summary>
    /// Парсер CNF из файла Dimacs
    /// </summary>
    static public class DimacsParser
    {
        static public CNF ReadFile(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            
            List<string> comments = new List<string>();
            List<Clause> clauses  = new List<Clause>();
            List<Var> variables = new List<Var>();
            Regex regexSpaces = new Regex("[ ]+");
            Regex regexLeadSpaces = new Regex("^[ ]+");
            bool IsComments = true;
            bool IsHeader = true;
            uint N = 0;
            uint countDisjunct = 0;
            string strLine = "";
            int countLine = 0;
            while((strLine = reader.ReadLine()) != null)
            {
                if(!String.IsNullOrWhiteSpace(strLine))
                {
                    strLine = regexLeadSpaces.Replace(strLine, "");
                    strLine = regexSpaces.Replace(strLine, " ");

                    switch (strLine[0])
                    {
                        case 'c':
                            if(IsComments)
                            {
                                comments.Add(strLine.Substring(1));
                            }
                            else
                            {
                                throw new Exception("Комментарии могут быть только в начале файла.");
                            }
                            break;
                        case 'p':
                            if(IsHeader)
                            {
                                var p = strLine.Split(" ");
                                if(p.Length>3 && p[1]=="cnf" && uint.TryParse(p[2], out N) && uint.TryParse(p[3], out countDisjunct))
                                {
                                    IsHeader = false; IsComments = false;
                                }
                                else
                                {
                                    throw new Exception("Ошибка при разборе заголовка.");
                                }
                            }
                            else
                            {
                                throw new Exception("Файл содержит более одного заголовка.");
                            }
                            break;
                        default:
                            if(IsHeader)
                            {
                                throw new Exception("Не найден заголовок.");
                            }
                            var cl = new Clause(strLine, variables, N);
                            if(cl.Literals.Count >0)
                            {
                                clauses.Add(cl);
                                countLine++;
                            }
                            break;
                    }
                    if(countLine==countDisjunct && countDisjunct>0) break;
                }
            }
            if(clauses.Count<1) throw new Exception("В входных данных не найдены дизъюнкты.");
            Console.WriteLine("N = "+N.ToString()+" M = "+countDisjunct.ToString());
            //Console.WriteLine("Количесто комментариев:" + comments.Count.ToString());
            Console.WriteLine("Количесто дизъюнктов:" + clauses.Count.ToString());
            return new CNF(clauses, variables);
        }
    }
}
