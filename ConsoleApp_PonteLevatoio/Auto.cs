using System;
using System.Collections.Generic;
using System.Threading;
using static ConsoleApp_PonteLevatoio.MyConsoleUtils;

namespace ConsoleApp_PonteLevatoio
{
    class Auto
    {
        private Thread _t;
        private object _lock;
        
        private string _name;
        private bool _inTransito;

        private int _x;
        private int _y;

        private int _speed;

        static int nAuto = 1;

        public Auto(object lck, int speed) 
        {
            _name = $"Auto {nAuto}";
            _speed = speed;
            _lock = lck ?? new object();
            
            _t = new Thread(Transita);
            
            nAuto++;
        }

        public void AvviaTransito()
        {
            _t.Start();
            _inTransito = true;
        }
        public void FermaTransito()
        {
            _t.Abort();
            _inTransito = false;
        }
        public void Transita()
        {
            while (true)
                if (_inTransito)
                {
                    _x++;
                    Scrivi(" "+ToString(), _lock, _x, _y);
                    Thread.Sleep(200 / _speed);
                }
        }

        public string Name { get { return _name; } }
        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }

        public bool InTransito
        {
            get => _inTransito; 
            set => _inTransito = value;
        }

        public int Corsia { get; set; }

        public override string ToString()
        {
            return _name;
        }
    }
}
