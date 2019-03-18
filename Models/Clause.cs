using System;
using System.Collections.Generic;
using System.Linq;
using DPLL_DLIS;


namespace DPLL_DLIS
{
    public class Clause
    {
        private List<Literal> _literals;
        public List<Literal> Literals {get=>_literals;}

        public Clause(string srtClause, List<Var> variables, uint bound)
        {
            try
            {
                _literals = new List<Literal>();
                var arr = srtClause.Split(" ");
                for(int i = 0; i < arr.Length; i++)
                {
                    var lit = long.Parse(arr[i]);
                    uint absLit = (uint)Math.Abs(lit);
                    if(absLit>bound)
                    {
                        string N = bound.ToString();
                        throw new Exception("Дизъюнкт '"+lit.ToString()+"' имеет значение за пределами {-"+N+","+N+"}.");
                    }
                    if(absLit==0) return; //  0 - конец строки
                    if(_literals.Where(x=>x.Var.Id==absLit).Count()>1)
                    {
                        throw new Exception("Дизъюнкт '"+ srtClause +"' содержит одинаковые литералы.");
                    }

                    var v = variables.Find(x=>x.Id == absLit);
                    if(v == null)
                    {
                        var newVar = new Var(absLit);
                        variables.Add(newVar);
                        _literals.Add(new Literal(newVar, lit>=0));
                    }
                    else
                    {
                        _literals.Add(new Literal(v, lit>=0));
                    }
                }
            }
            catch(Exception ex)
            {
                _literals = null;
                throw new Exception("Не удалось разобрать строку с дизъюнктом литералов."+ex.Message);
            }
        }

        public bool TryGetUnit(out Literal unit)
        {
            unit = null;
            if (_literals==null) return false;
            var uLiterals = _literals.Where(x=>x.Var.Assignment==null);
            if (uLiterals.Count()!=1) return false;

            var result = _literals.Where(x=>x.Var.Assignment!=null).All(x=>x.GetValue()==false);
            if(result) unit = uLiterals.First();
            return result;
        }

        public bool IsFalse()
        {
            return _literals.All(x=>x.GetValue()==false);
        }

        public bool IsAnyUnknownLiteral()
        {
            return _literals.Any(x=>x.GetValue()==null);
        }

        public override string ToString()
        {
            string result = "";
            foreach(var lit in _literals)
            {
                result += lit.ToString()+" V ";
            }
            if(result.Count()>2) result = result.Substring(0, result.Length - 3);
            return result;
        }
    }
}
