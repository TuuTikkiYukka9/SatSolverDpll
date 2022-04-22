using System;
using System.Collections.Generic;
using System.Linq;
using DPLL_DLIS;


namespace DPLL_DLIS
{

    public class CNF
    {
        private List<Var> variables;
        private List<Clause> clauses;
        public List<Clause> Clauses {get=>clauses;}

        /// <summary>
        /// Создает новый объект cnf
        /// </summary>
        /// <param name="clauses">Список дизъюнктов cnf</param>
        /// <param name="variables">Список переменных, используемых в cnf</param>
        public CNF(List<Clause> clauses, List<Var> variables)
        {
            this.clauses = clauses;
            this.variables = variables;
            this.variables.Sort(delegate(Var x, Var y){ return x.Id.CompareTo(y.Id);});
        }

        /// <summary>
        /// Проверяет, есть ли еденичные дизъюнкты
        /// </summary>
        public bool IsAnyUnitClause()
        {
            return clauses.Any(x=> x.TryGetUnit(out var lit));
        }

        /// <summary>
        /// Проверяет, есть ли ложные дизъюнкты
        /// </summary>
        public bool IsUnsat()
        {
            return clauses.Any(x=> x.IsFalse());
        }

        /// <summary>
        /// Проверяет имеет ли cnf неопределенные литералы
        /// </summary>
        public bool IsAnyUnknownLiteral()
        {
            return variables.Any(x=>x.Assignment==null); 
            //return clauses.Any(x=>x.IsAnyUnknownLiteral());
        }

        /// <summary>
        /// Выбирается неназначенный литерал, входящий в наибольшее количество неразрешённых дизъюнктов формулы.
        /// </summary>
        /// <returns>Выбранный литерал</returns>
        public Literal Dlis()
        {
            Dictionary<Literal, uint> literalsCount = new();

            foreach(var cl in clauses)
            {
                foreach(var lit in cl.Literals)
                {
                    if(lit.GetValue()==null)
                    {
                        if(literalsCount.ContainsKey(lit))
                        {
                            literalsCount[lit] += 1; 
                        }
                        else
                        {
                            literalsCount.Add(lit,1);
                        }
                    }  
                }
            }
            var maxCount = literalsCount.Max(x=>x.Value);
            return literalsCount.First(x=>x.Value==maxCount).Key;
        }

        public string VariablesValueToString()
        {
            string strVariables = "";
            foreach(var v in variables)
            {
                strVariables +=v.Id.ToString()+"="+
                        ((v.Assignment == null)?"u":(v.Assignment==true)? "1":"0")+"; ";
            }
            return strVariables;
        }
    }
}