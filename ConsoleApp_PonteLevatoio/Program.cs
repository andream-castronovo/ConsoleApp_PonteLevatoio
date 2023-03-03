using System;
using System.Collections.Generic;
using System.Threading;

using static ConsoleApp_PonteLevatoio.MyConsoleUtils;

namespace ConsoleApp_PonteLevatoio
{
    internal class Program
    {

        /*
         Traccia a pagina 163
            
          Prima versione semplificata:
           - Le barche hanno sempre la precedenza
           - Senso unico

           - 4 bottoni: 
            - A (Spawna una macchina) 
            - O (Apre il ponte)
            - C (Chiude il ponte)
            - U (Chiude il programma)

          Quando premo A una macchina spunta nello schermo e deve passare attraverso il ponte.
          Se viene richiesta l'apertura del ponte, le macchine che sono sopra finiscono di transitare, le altre non passano e aspettano l'apertura del ponte.
        
          Usare Semaphore SOLO per limitare le auto sul ponte!
         */

        const int MAX_AUTO = 4;
        
        static int[] COORDINATE_PONTE = { 45, 10 };
        static int[] COORDINATE_PARCHEGGIO = { 1, 8 };


        #region Ponte e acqua
        
        
        #endregion


        static SemaphoreSlim _sempahore = new SemaphoreSlim(MAX_AUTO);

        static object _lockConsole = new object();
        static object _lockParcheggio = new object();
        static object _lockPassa = new object();
        static object _lockCorsia = new object();

        
        //static List<Auto> _autoInParcheggio = new List<Auto>();
        
        static bool _levatoio = true;
        static bool[] _corsia = new bool[MAX_AUTO];

        static int[] COORDINATE_MENU = { 0, 0 };

        static Ponte _ponte;
        static Parcheggio _park;

        static void Main(string[] args)
        {
            Thread menu = new Thread(Menu);
            //Thread parcheggio = new Thread(Parcheggio);

            //parcheggio.Start();
            menu.Start();
            
            _ponte = new Ponte(COORDINATE_PONTE[0], COORDINATE_PONTE[1], MAX_AUTO);
            _park = new Parcheggio();

            StampaMenu();
        }

        static void Parcheggio()
        {
            while (true)
            {
                
            }
        }


        //static void Parcheggio()
        //{
        //    int old = 0;
        //    List<Auto> temp = new List<Auto>();
        //    while (true)
        //    {
        //        if (_autoInParcheggio.Count > 0 && _autoInParcheggio.Count != old)
        //        {
        //            old = _autoInParcheggio.Count;

        //            int x = COORDINATE_PARCHEGGIO[0];
        //            int y = COORDINATE_PARCHEGGIO[1];

        //            for (int i = 0; i < _autoInParcheggio.Count; i++)
        //            {
        //                if (!temp.Contains(_autoInParcheggio[i])) // Per evitare di stampare tutto ogni volta
        //                    Scrivi(_autoInParcheggio[i].Name, x: x, y: y+i);
        //            }

        //            temp = new List<Auto>(_autoInParcheggio);
        //        }

        //        if (_autoInParcheggio.Count > 0)
        //        {
        //            _sempahore.Wait();
        //            Auto tmp = _autoInParcheggio[0];
        //            _autoInParcheggio.RemoveAt(0);
        //            _autoInTransito.Add(tmp);
        //        }
        //    }
        //}

        //static void Transito()
        //{
        //    while (true)
        //    {

        //    }
        //}

        #region Stampe
        static void StampaMenu()
        {
            Scrivi(
                "Comandi:\n", 
                x: COORDINATE_MENU[0], 
                y: COORDINATE_MENU[1]
            );
            Scrivi(
                "A) Auto\n", 
                x: 0,
                y: COORDINATE_MENU[1] + 1
            );
            Scrivi(
                "O) Apri ponte (Open)\n", 
                x: 0,
                y: COORDINATE_MENU[1] + 2
            );
            Scrivi("C) Chiudi ponte (Close)\n", 
                x: 0,
                y: COORDINATE_MENU[1] + 3
            );
            Scrivi("U) Chiudi programma (Uscita)\n", 
                x: 0,
                y: COORDINATE_MENU[1] + 4
            );
        }
        
        #endregion


        static void Menu()
        {
            do
            {
                char key = char.ToUpper(Console.ReadKey(true).KeyChar);

                switch (key)
                {
                    case 'O':
                        _ponte.ApriPonte();
                        break;
                    case 'C':
                        _ponte.ChiudiPonte();
                        break;
                    case 'A':
                        _park.AggiungiMacchina(new Auto());
                        break;
                    case 'U':
                        Environment.Exit(0);
                        break;
                }

            } while (true);
        }
    }
}
