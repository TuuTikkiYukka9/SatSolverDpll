using System;
using System.Collections.Generic;
using DPLL_DLIS;


namespace DPLL_DLIS
{
    //Var - пропозициональные переменные
    // identifier : an unique integer ID
    // assignment : an assignment (True, False or None)
    //public enum Ternary{ False = 0, True =1, None=2};
    public class Var
    {
        private uint _id; 
        public uint Id {get=>_id;}
        public bool? Assignment {get;set;}

        public Var(uint id)
        {
            _id = id;
            Assignment = null;
        }
        
        public override bool Equals(Object obj)
        {
            return (obj is Var) ? _id == ((Var)obj)._id : false;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public override string ToString()
        {
            return (Assignment == null ? "u" : Assignment==false ? "-" : "")+_id.ToString();
        }
    }
}