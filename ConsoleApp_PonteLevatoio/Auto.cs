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

        Ponte _p;
        

        private int _speed;

        static int nAuto = 1;

        public int Goal { get; set; }

        public Auto(object lck, int speed, Ponte p = null) 
        {
            _name = $"Auto {nAuto}";
            _speed = speed;
            _lock = lck ?? new object();
            
            _t = new Thread(Transita);
            
            nAuto++;
            Goal = Console.WindowWidth;

            _p = p;
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
            {
                if (_inTransito)
                {
                    _x++;
                    Scrivi(" "+ToString(), _lock, _x, _y);
                    Thread.Sleep(200 / _speed);
                }
                if (_p != null && _x > _p.X + _p.Lenght + Name.Length)
                {
                    Scrivi("               ", _lock, _x, _y);
                    Thread.CurrentThread.Abort();
                }
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

        public Thread T
        {
            get => _t;
        }

        public Ponte Ponte
        {
            get => _p;
            set => _p = value;
        }
    }
}
