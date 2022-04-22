using System;

namespace DPLL_DLIS
{
    /// <summary>
    /// Var - пропозициональные переменные
    /// </summary>
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
            return (Assignment == null ? "u" : Assignment==false ? "-" : "") + _id.ToString();
        }
    }
}