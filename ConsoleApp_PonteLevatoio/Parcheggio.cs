using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp_PonteLevatoio.MyConsoleUtils;

namespace ConsoleApp_PonteLevatoio
{
    class Parcheggio
    {
        List<Auto> _listaMacchine;

        int _x;
        int _y;
        object _lockConsole;
        public Parcheggio(int x, int y, object lockConsole = null)
        {
            _lockConsole = lockConsole ?? new object();

            _listaMacchine = new List<Auto>();
            
            _x = x;
            _y = y+2;
        }

        public void AggiungiMacchina(Auto auto)
        {
            _listaMacchine.Add(auto);
            Stampa();
        }

        public int NumeroMacchine
        {
            get => _listaMacchine.Count;
        }

        public override string ToString()
        {
            string s = "Parcheggio:\n\n";

            
            for (int i = 0; i < NumeroMacchine; i++)
            {
                s += _listaMacchine[i].ToString() + "\n";
            }
            return s;
        }

        public void Stampa()
        {
            Scrivi(ToString()+"         ", _lockConsole, _x, _y);
        }

        /// <summary>
        /// Rimuove la prima macchina nel parcheggio e la restituisce
        /// </summary>
        /// <returns>La prima macchina nel parcheggio</returns>
        public Auto FaiUscireAuto()
        {
            if (NumeroMacchine == 0)
                throw new Exception("Nessuna macchina");

            Auto temp = _listaMacchine[0];
            _listaMacchine.Remove(temp);
            Stampa();
            return temp;
        }

        

    }
}
