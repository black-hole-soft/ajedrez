using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ajedrez
{
    public class Rey
    {
        public int x, y;
        public bool jaque = false,puedeEnrocar=true;
        public Rey(int i,int j) 
        {
            x = i;
            y = j;
        }
    }
}
