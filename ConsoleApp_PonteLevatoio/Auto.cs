using System;
using System.Collections.Generic;
using System.Threading;
using static ConsoleApp_PonteLevatoio.MyConsoleUtils;

namespace ConsoleApp_PonteLevatoio
{
    class Auto
    {
        // Programmato da Andrea Maria Castronovo - 4°I - Data consegna: 15/04/2023

        private Thread _t; // Thread di movimento della macchina
        private object _lockConsole; // Lock per scrivere in console
        
        private string _name; // Nome automobile
        private bool _inTransito; // Se in transito o no

        private int _x; // Posizione X della macchina
        private int _y; // Posizione Y della macchina

        Ponte _p; // Ponte su cui la macchina sta transitando
        

        private int _speed; // Velocità di movimento della macchina

        static int nAuto = 1; // Contatore delle macchine create

        

        public Auto(object lck, int speed, Ponte p = null)
        {
            _name = $"Auto {nAuto++}";
            _speed = speed;

            _lockConsole = lck ?? new object(); // Se non è presente un lock, ne crea uno
            
            // Creo il thread che gestirà il transito di questa automobile
            _t = new Thread(Transita) { Name = $"Transito_{ToString()}" };
            
            _p = p;
        }

        public void AvviaTransito()
        {
            // Avvia il transito della macchina
            _t.Start();
            _inTransito = true;
        }

        public void Transita()
        {

            // Velocità va da 0 a 100

            // Più è alta la velocità, meno deve aspettare lo sleep
            // 150 attesa massima
            // 50 attesa minima
            while (true)
            {
                if (_inTransito) // Se la macchina sta transitando
                {
                    _x++; // Incrementa la sua X
                    // Stampa l'auto con spazio dietro in modo da cancellare l'avanzata precedente
                    Scrivi(" "+ToString(), _lockConsole, _x, _y);

                    // Aspetta tempo in base alla velocità della macchina
                    Thread.Sleep(150 - _speed);
                }
                else
                {
                    // Se non sta transitando, continua a scriverla comunuque in modo da non permettere 
                    // alla macchina di essere sovrascritta da altro, come per esempio l'apertura del ponte
                    Scrivi(" " + ToString(), _lockConsole, _x, _y);
                }
                if (_p != null && _x > _p.X + _p.Lenght + Name.Length)
                {
                    Scrivi("               ", _lockConsole, _x, _y);
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

        public override string ToString() => _name;

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
