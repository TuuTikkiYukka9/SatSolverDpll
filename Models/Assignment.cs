using System;
using System.Collections.Generic;
using System.Linq;
using DPLL_DLIS;

namespace DPLL_DLIS
{

    public class AssignmentItem
    {
        public Literal Literal;
        public bool IsDecide;

        public AssignmentItem(Literal literal, bool isDecide)
        {
            Literal = literal;
            IsDecide = isDecide;
        }
    }

    public class Assignment
    {
        private Stack<AssignmentItem> assigmentStack;
        private ulong backjumpCount;
        private ulong decideCount;
        private ulong unitPropagateCount;


        /// <summary>
        /// Создает новый объект Assignment- присваение
        /// </summary>
        public Assignment()
        {
            assigmentStack = new Stack<AssignmentItem>();
            decideCount =0; backjumpCount = 0; unitPropagateCount =0;
        }

        /// <summary>
        /// Существует хотя бы один решающий литерал
        /// </summary>
        public bool IsAnyDecideLiteral()
        {
            return assigmentStack.Where(x=>x.IsDecide).Count()>0;
        }

        /// <summary>
        /// Xронологический возврат
        /// </summary>
        public void BackjumpDpll(CNF cnf)
        {
            bool isFound = false;
            while(!isFound)
            {
                if(!assigmentStack.TryPop(out var popItem)) break;//вообще исключение следует генерить
                
                if(popItem.IsDecide)
                {
                    var lit = new Literal(popItem.Literal.Var, true);//!popItem.Literal.isPositive);
                    lit.SetValue(false);
                    assigmentStack.Push(new AssignmentItem(lit, false));
                    isFound = true;
                }
                else
                {
                    popItem.Literal.SetValue(null);
                }
            }
            backjumpCount++;
        }


        /// <summary>
        /// выбирает неназначенный литерал, добавляет его в присваивание и помечает как решающий.
        /// </summary>
        /// <param name="cnf"></param>
        public void Decide(CNF cnf)
        {
            var literal = cnf.Dlis();
            var addLit = new Literal(literal.Var, false);
            addLit.SetValue(false);
            assigmentStack.Push(new AssignmentItem(addLit, true));//literal, true));
            decideCount++;
        }


        /// <summary>
        /// единичный дизъюнкт с неназначенным литералом, верен если этот литерал верен
        /// </summary>
        /// <param name="cnf"></param>
        public void UnitPropagate(CNF cnf)
        {
            foreach(var cl in cnf.Clauses)
            {
                if(cl.TryGetUnit(out var lit))
                {
                    lit.SetValue(true);
                    var literal = new Literal(lit.Var, !lit.IsPositive);
                    assigmentStack.Push(new AssignmentItem(literal, false));
                    unitPropagateCount++;
                    return;
                }
            }
        }

        public string GetStatistics()
        {
            return "decide="+decideCount.ToString()+"\n"+
                    "backjump="+backjumpCount.ToString()+"\n"+
                    "unitPropagate="+unitPropagateCount.ToString()+"\n";
        }

        private string GetAssigmentState(string ruleName = "")
        {
            string result = ruleName+" [M]: ";
            foreach(var el in assigmentStack)
            {
                result+=el.Literal.ToString()+(el.IsDecide?"(B)":"")+"; ";
            }
            return result;
        }
    }
}