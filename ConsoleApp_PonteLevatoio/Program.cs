using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        
          Usare Semaphore solo per limitare le auto sul ponte!
         */

        const int MAX_AUTO = 6;

        static int[] COORDINATE_PONTE = { 45, 10 };
        static int[] COORDINATE_PARCHEGGIO = { 0, 6 };


        #region Ponte e acqua


        #endregion

        static object _lockConsole = new object();
        static object _lockParcheggio = new object();
        static object _lockPassa = new object();
        static object _lockCorsia = new object();


        //static List<Auto> _autoInParcheggio = new List<Auto>();

        static bool _levatoio = true;
        static bool[] _corsia = new bool[MAX_AUTO];

        static int[] COORDINATE_MENU = { 0, 0 };

        static SemaphoreSlim _semaphore = new SemaphoreSlim(MAX_AUTO);

        static Ponte _ponte;
        static Parcheggio _park;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            _ponte = new Ponte(COORDINATE_PONTE[0], COORDINATE_PONTE[1], MAX_AUTO, _semaphore, lockConsole: _lockConsole);
            _park = new Parcheggio(COORDINATE_PARCHEGGIO[0], COORDINATE_PARCHEGGIO[1], lockConsole: _lockConsole);

            Thread menu = new Thread(Menu);
            Thread parcheggio = new Thread(GestioneParcheggio);

            parcheggio.Start();
            menu.Start();


            StampaMenu();
        }



        static void GestioneParcheggio()
        {
            while (true)
            {
                if (_park.NumeroMacchine > 0)
                {
                    _semaphore.Wait();
                    Auto a = _park.FaiUscireAuto();
                    _ponte.AggiungiMacchina(a);
                    
                }
            }
        }

        static void GestionePonte()
        {
            while(true)
            {

            }
        }


        #region Stampe
        static void StampaMenu()
        {
            Scrivi(
                "Comandi:\n",
                x: COORDINATE_MENU[0],
                y: COORDINATE_MENU[1],
                lck: _lockConsole
            );
            Scrivi(
                "A) Auto\n",
                x: 0,
                y: COORDINATE_MENU[1] + 1,
                lck: _lockConsole
            );
            Scrivi(
                "O) Apri ponte (Open)\n",
                x: 0,
                y: COORDINATE_MENU[1] + 2,
                lck: _lockConsole
            );
            Scrivi("C) Chiudi ponte (Close)\n",
                x: 0,
                y: COORDINATE_MENU[1] + 3,
                lck: _lockConsole
            );
            Scrivi("U) Chiudi programma (Uscita)\n",
                x: 0,
                y: COORDINATE_MENU[1] + 4,
                lck: _lockConsole
            );
        }
        #endregion

        static void Menu()
        {
            Random rnd = new Random();
            do
            {
                char key = char.ToUpper(Console.ReadKey(true).KeyChar);

                switch (key)
                {
                    case 'O':
                        _ponte.OpenPonte();
                        break;
                    case 'C':
                        _ponte.ChiudiPonte();
                        break;
                    case 'A':
                        _park.AggiungiMacchina(new Auto(_lockConsole, rnd.Next(1,5)));
                        break;
                    case 'U':
                        Environment.Exit(0);
                        break;
                }

            } while (true);
        }
    }
}
