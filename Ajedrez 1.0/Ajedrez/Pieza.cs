﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ajedrez
{
    public class Pieza
    {
        public String roll;
        public bool amenaza,color;
        public Pieza(String r,bool c) 
        {
            roll = r;
            color = c;
            amenaza = false;
        }
    }
}
