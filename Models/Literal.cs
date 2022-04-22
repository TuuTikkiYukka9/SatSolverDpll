using System;
using System.Collections.Generic;
using DPLL_DLIS;


namespace DPLL_DLIS
{

    /// <summary>
    /// Literal - пропозициональная переменная, либо её отрицание.
    /// </summary>
    public class Literal
    {
        private bool _isPositive;
        public bool IsPositive {get=>_isPositive;}
        private Var _var;
        public Var Var {get=>_var;}

        /// <summary>
        /// Создает новый литерал
        /// </summary>
        /// <param name="pvar">ссылка на пропозиционную переменную</param>
        /// <param name="isPositive">true - переменная, false - отризание переменной</param>
        public Literal(Var pvar, bool isPositive = true)
        {
            _var = pvar;
            this._isPositive = isPositive;
        }


        public bool? GetValue()
        {
            return _var.Assignment == null ? null : _isPositive ? _var.Assignment : !(_var.Assignment);
        }

        public void SetValue(bool? value)
        {
            if(value == null) 
            {
                this._var.Assignment = null;
                return;
            }
            this._var.Assignment = _isPositive ? value : !(value);
        }

        public override bool Equals(Object obj)
        {
            if(obj is not Literal) return false;
            var literal = (Literal)obj;
            return _var.Equals(literal._var) && _isPositive==literal._isPositive;
        }

        public override int GetHashCode()
        {
            return _var.GetHashCode();
        }

        public override string ToString()
        {
            return _isPositive ? _var.ToString() : "-(" + _var.ToString() + ")";
        }
    }
}