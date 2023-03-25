using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Runtime.Remoting.Metadata;
using System.Threading;
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
        bool _richiestaApertura;

        object _lockConsole;
        object _lockCorsia = new object();
        object _lockPassaPonte = new object();

        bool[] _corsieOccupate;
        SemaphoreSlim _s;
        List<Auto> _autoInTransito = new List<Auto>();

        const ConsoleColor COLORE_ACQUA = ConsoleColor.Blue;

        public Ponte(int xPonte, int yPonte, int corsie, SemaphoreSlim s, object lockConsole = null, int length = 27, char carattere = '=')
        {
            _aperto = false;
            _richiestaApertura = false;

            _length = length;
            _corsie = corsie;

            _latoChiuso = "";
            _acqua = "";
            _lockConsole = lockConsole ?? new object(); // ?? --> Se non è null fai quello a sinistra, se è null quello a destra.

            _corsieOccupate = new bool[_corsie];
            _s = s;


            for (int i = 0; i < length; i++)
            {
                _latoChiuso += carattere;
                _latoAperto += (i < 3) ? carattere.ToString() : "";
                _acqua += (i == 0 || i == length - 1) ? " " : "▓";
            }

            _x = xPonte;
            _y = yPonte;

            new Thread(CheckCars).Start();

            StampaPonte();
        }

        public void OpenPonte()
        {
            if (_aperto)
                return;

            lock (_lockRichiesta)
            {
                _richiestaApertura = true;
            }

            StampaPonte();

        }

        public void ChiudiPonte()
        {
            if (!_aperto && !_richiestaApertura)
                return;


            _aperto = false;
            _richiestaApertura = false;
            StampaPonte();

        }

        private void StampaPonte()
        {
            int currentY = _y;

            StampaAcqua();
            if (!_aperto)
            {
                if (!_richiestaApertura)
                {
                    Scrivi(_latoChiuso, x: _x, y: currentY++, lck: _lockConsole);
                    for (int i = 0; i < _corsie; i++)
                    {
                        Scrivi(_latoChiuso.Replace(_latoChiuso[0], ' '), x: _x, y: currentY++, lck: _lockConsole);
                    }
                    Scrivi(_latoChiuso, x: _x, y: currentY, lck: _lockConsole);
                }
                else
                {
                    Scrivi(_latoChiuso, x: _x, y: currentY++, lck: _lockConsole);
                    for (int i = 0; i < _corsie; i++)
                    {
                        Scrivi(_latoChiuso.Replace(_latoChiuso[0], ' '), x: _x, y: currentY++, lck: _lockConsole);
                        Scrivi("|", x: _x + 1, y: currentY - 1, fore: ConsoleColor.Red, lck: _lockConsole);

                    }
                    Scrivi(_latoChiuso, x: _x, y: currentY, lck: _lockConsole);
                }
            }
            else // Se è aperto
            {
                int d = (3 * _length) / 27;
                string s = "";
                string t = "";
                for (int i = 0; i < d; i++)
                {
                    s += _latoChiuso[0];
                    t += " ";
                }

                Scrivi(s, x: _x, y: currentY, lck: _lockConsole);
                Scrivi(s, x: _x + _length - 3, y: currentY++, lck: _lockConsole);
                for (int i = 0; i < _corsie; i++)
                {
                    Scrivi(t, x: _x, y: currentY, lck: _lockConsole);
                    Scrivi("|", x: _x, y: currentY, fore: ConsoleColor.Red, lck: _lockConsole);
                    Scrivi(t, x: _x + _length - 3, y: currentY++, lck: _lockConsole);
                }
                Scrivi(s, x: _x, y: currentY, lck: _lockConsole);
                Scrivi(s, x: _x + _length - 3, y: currentY++, lck: _lockConsole);

            }
        }

        private void StampaAcqua()
        {
            int xAcqua = _x;
            int yAcqua = 0;

            while (yAcqua < Console.WindowHeight)
                Scrivi(_acqua, x: xAcqua, y: yAcqua++, fore: COLORE_ACQUA, lck: _lockConsole);
        }

        public int NumeroAutoTransito
        {
            get => _autoInTransito.Count;
        }

        public void AggiungiMacchina(Auto auto)
        {
            bool trovata = false;
            for (int i = 0; i < _corsie; i++)
            {
                if (!_corsieOccupate[i] && !trovata)
                {
                    auto.Corsia = i;
                    auto.Y = _y + i + 1;
                    auto.X = 7;
                    _corsieOccupate[i] = !_corsieOccupate[i];
                    trovata = true;
                }
            }

            lock (_lockCorsia)
                _autoInTransito.Add(auto);

            auto.Ponte = this;
            auto.AvviaTransito();
            Debug.WriteLine($"Aggiunta macchina {auto} alla corsia {auto.Corsia}");

        }

        public int Lenght
        {
            get => _length;
        }

        object _lockRichiesta = new object();

        private void CheckCars()
        {
            while (true)
            {
                if (NumeroAutoTransito > 0)
                {
                    lock (_lockRichiesta)
                    {
                        if (_aperto)
                        {
                            for (int i = 0; i < NumeroAutoTransito; i++)
                            {
                                lock (_lockCorsia)
                                {
                                    if (_autoInTransito[i].X == LineaStopAperto(_autoInTransito[i]))
                                    {
                                        _autoInTransito[i].InTransito = false;
                                    }
                                }
                            }
                        }
                        else // Ponte chiuso (macchine possono passare)
                        {
                            if (!_richiestaApertura) // Non è richiesta l'apertura
                            {
                                for (int i = 0; i < NumeroAutoTransito; i++)
                                {
                                    lock (_lockCorsia)
                                    {
                                        if (!_autoInTransito[i].InTransito)
                                            _autoInTransito[i].InTransito = true;
                                    }
                                }
                            }
                            else // È richiesta l'apertura
                            {
                                for (int i = 0; i < NumeroAutoTransito; i++) // Per ogni auto
                                {
                                    lock (_lockCorsia)
                                    {
                                        if (_autoInTransito[i].X > LineaStopAperto(_autoInTransito[i])) // Se la macchina è sul ponte
                                        {
                                            _autoInTransito[i].InTransito = true;
                                            new Thread(new ParameterizedThreadStart(AspettaAuto))
                                                .Start(_autoInTransito[i]);
                                        }
                                        else if (_autoInTransito[i].X == LineaStopAperto(_autoInTransito[i])) // Se è sulla linea di fermata
                                        {
                                            _autoInTransito[i].InTransito = false;
                                        }
                                        else if (_autoInTransito[i].X < LineaStopAperto(_autoInTransito[i])) // Se è prima della linea di fermata
                                        {
                                            _autoInTransito[i].InTransito = true;
                                        }
                                    }
                                }
                            }

                        }
                    }

                }

                for (int i = 0; i < NumeroAutoTransito; i++) // Per ogni auto
                {
                    lock(_lockCorsia)
                    {
                        if (_autoInTransito[i].X > LineaGoal + _autoInTransito[i].Name.Length) // Se 
                        {
                            FermaTransito(i);
                        }
                    }
                }

                if (_richiestaApertura)
                {
                    // Il problema è che entra qui prima che tutte le auto abbiano finito.
                    lock (_lockPassaPonte)
                    {
                        _aperto = true;
                        _richiestaApertura = false;
                        StampaPonte();
                    }
                }
                
            }
        }
        
        private void AspettaAuto(object a)
        {
            Auto auto = (Auto)a;
            lock (_lockPassaPonte)
            {
                auto.T.Join();
            }
        }

        private int LineaStopAperto(Auto a)
        {
            return _x - a.Name.Length;
        }
        private int LineaGoal
        {
            get => _x + _length;
        }

        private void FermaTransito(int i)
        {
            lock (_lockCorsia)
            {
                _corsieOccupate[_autoInTransito[i].Corsia] = false;
                Scrivi("                               ", _lockConsole, x: LineaGoal, y: _y + _autoInTransito[i].Corsia + 1);
                _autoInTransito.Remove(_autoInTransito[i]);
                _s.Release();
            }
        }


        public int X { get => _x; }
        public int Y { get => _y; }
    }
}
