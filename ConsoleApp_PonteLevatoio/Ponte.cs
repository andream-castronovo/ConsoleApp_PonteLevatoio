using System;
using static ConsoleApp_PonteLevatoio.MyConsoleUtils;

namespace ConsoleApp_PonteLevatoio
{
    class Ponte
    {
        int _length;
        int _x;
        int _y;
        int _corsie;
        string _lato;

        bool _aperto;


        const string ACQUA = " ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ "; // FULL ASCII 178
        const ConsoleColor COLORE_ACQUA = ConsoleColor.Blue;

        public Ponte(int xPonte, int yPonte, int corsie, int length = 27, char carattere = '=')
        {
            _aperto = false;

            _length = length;
            _corsie = corsie;

            _lato = "";
            

            for (int i = 0; i < length; i++)
            {
                _lato += carattere;
            }
            
            _x = xPonte;
            _y = yPonte;

            StampaPonte();
        }

        public void ApriPonte() 
        {
            if (_aperto)
                return;
            
            _aperto = true;
            StampaPonte();
        }
        


        private void StampaPonte()
        {
            Scrivi(_lato, x: _x, y: _y); // I ":" servono a decidere quale parametro facoltativo assegnare

            for (int i = 1; i <= _corsie; i++)
            {
                Scrivi("                          ", x: _x, y: ++_y);
            }

            Scrivi(_lato, x: _x, y: _y);
        }

        private void StampaAcqua()
        {
            int xAcqua = _x;
            int yAcqua = 0;
            
            while (yAcqua < Console.WindowHeight)
                Scrivi(ACQUA, x: xAcqua, y: yAcqua++, fore: COLORE_ACQUA);
        }
    }
}
