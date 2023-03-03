using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_PonteLevatoio
{
    class Parcheggio
    {
        List<Auto> _listaMacchine;

        public Parcheggio()
        {
            _listaMacchine = new List<Auto>();
        }

        public void AggiungiMacchina(Auto auto)
        {
            _listaMacchine.Add(auto);
        }

        /// <summary>
        /// Rimuove la prima macchina nel parcheggio e la restituisce
        /// </summary>
        /// <returns>La prima macchina nel parcheggio</returns>
        public Auto FaiUscireAuto()
        {
            Auto temp = _listaMacchine[0];
            _listaMacchine.Remove(temp);
            return temp;
        }

    }
}
