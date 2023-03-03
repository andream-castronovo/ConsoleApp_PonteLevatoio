using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleApp_PonteLevatoio
{
    class Auto
    {
        private Thread t;
        private string name;
        static int nAuto = 1;

        public Auto() 
        {
            name = $"Auto {nAuto}";
            nAuto++;
        }

        public string Name { get { return name; } }
    }
}
