using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ajedrez
{
    public class Rey
    {
        public int x, y;
        public bool reyM = false, RrookM = false, LrookM = false;
        public Rey(int i,int j) 
        {
            x = i;
            y = j;
        }
        public bool puedeEnrocarDer() 
        {
            if (!reyM && !RrookM)
                return true;
            return false;
        }
        public bool puedeEnrocarIzq()
        {
            if (!reyM && !LrookM)
                return true;
            return false;
        }
    }
}
