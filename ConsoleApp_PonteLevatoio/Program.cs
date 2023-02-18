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

        #region Ponte e acqua
        
        
        #endregion


        static SemaphoreSlim _sempahore;

        static object _lockConsole = new object();
        static object _lockParcheggio = new object();
        static object _lockPassa = new object();
        static object _lockCorsia = new object();

        static List<Auto> _autoInTransito;
        static List<Auto> _autoInParcheggio;
        
        static bool _levatoio = true;
        static bool[] _corsia = new bool[MAX_AUTO];


        static Ponte _p;

        static void Main(string[] args)
        {
            StampaAcqua();
            StampaPonte();
            StampaMenu();
            Console.ReadLine();
        }

        #region Stampe
        static void StampaPonte()
        {
            int xPonte = COORDINATE_PONTE[0];
            int yPonte = COORDINATE_PONTE[1];

            _p = new Ponte(xPonte, yPonte, MAX_AUTO);
        }
        static void StampaAcqua()
        {
            


        }
        static void StampaMenu()
        {
            Scrivi(
                "Comandi:\n", 
                x: 0, 
                y: 0
            );
            Scrivi(
                "A) Auto\n", 
                x: 0
            );
            Scrivi(
                "O) Apri ponte (Open)\n", 
                x: 0
            );
            Scrivi("C) Chiudi ponte (Close)\n", 
                x: 0
            );
            Scrivi("U) Chiudi programma (Uscita)\n", 
                x: 0
            );
        }
        
        #endregion

    }
}
