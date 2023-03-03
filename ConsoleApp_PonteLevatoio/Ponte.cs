using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using static ConsoleApp_PonteLevatoio.MyConsoleUtils;

namespace ConsoleApp_PonteLevatoio
{
    class Ponte
    {
        int _length;
        int _x;
        int _y;
        int _corsie;
        string _latoChiuso;
        string _latoAperto;
        string _acqua;
        bool _aperto;
        
        List<Auto> _autoInTransito = new List<Auto>();
        
        const ConsoleColor COLORE_ACQUA = ConsoleColor.Blue;

        public Ponte(int xPonte, int yPonte, int corsie, int length = 27, char carattere = '=')
        {
            _aperto = false;

            _length = length;
            _corsie = corsie;

            _latoChiuso = "";
            _acqua = "";

            for (int i = 0; i < length; i++)
            {
                _latoChiuso += carattere;
                _latoAperto += (i < 3) ? carattere.ToString() : "";
                _acqua += (i == 0 || i == length - 1) ? " " : "▓";
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

        public void ChiudiPonte()
        {
            if (!_aperto)
                return;

            _aperto = false;
            StampaPonte();
        }

        private void StampaPonte()
        {
            int currentY = _y;
            
            StampaAcqua();
            if (!_aperto)
            {
                Scrivi(_latoChiuso, x: _x, y: currentY++);
                for (int i = 0; i < _corsie; i++)
                    Scrivi(_latoChiuso.Replace(_latoChiuso[0], ' '), x: _x, y: currentY++);
                Scrivi(_latoChiuso, x: _x, y: currentY);
            }
            else
            {
                int d = (3 * _length) / 27;
                string s = "";
                string t = "";
                for (int i = 0; i < d; i++)
                {
                    s += _latoChiuso[0];
                    t += " ";
                }

                Scrivi(s, x: _x, y: currentY);
                Scrivi(s, x: _x+_length-3, y:currentY++);
                for (int i = 0;  i < _corsie; i++)
                {
                    Scrivi(t, x: _x, y:currentY);
                    Scrivi("|", x: _x, y: currentY, fore:ConsoleColor.Red);
                    Scrivi(t, x: _x+_length-3, y: currentY++);
                }
                Scrivi(s, x: _x, y: currentY);
                Scrivi(s, x: _x+_length-3, y: currentY++);

            }




        }

        private void StampaAcqua()
        {
            int xAcqua = _x;
            int yAcqua = 0;
            
            while (yAcqua < Console.WindowHeight)
                Scrivi(_acqua, x: xAcqua, y: yAcqua++, fore: COLORE_ACQUA);
        }
    }
}
