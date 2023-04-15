using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

using static ConsoleApp_PonteLevatoio.MyConsoleUtils;

namespace ConsoleApp_PonteLevatoio
{
    internal class Program
    {
        // Programmato da Andrea Maria Castronovo - 4°I - Data consegna: 15/04/2023

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

        const int MAX_AUTO = 4; // Numero massimo auto sul ponte (quindi le corsie)

        static int[] COORDINATE_PONTE = { 45, 10 }; // Coordinate di spawn del ponte
        static int[] COORDINATE_PARCHEGGIO = { 0, 6 }; // Coordinate di inizio del parcheggio
        static int[] COORDINATE_MENU = { 0, 0 }; // Coordinate del menu

        static object _lockConsole = new object(); // Lock per gestire la scrittura nella Console
        
        static SemaphoreSlim _semaphore = new SemaphoreSlim(MAX_AUTO); // Semaforo per limitare le auto sul ponte

        static Ponte _ponte; // Ponte
        static Parcheggio _park; // Parcheggio

        static void Main(string[] args)
        {
            Console.CursorVisible = false; // Rimuovo la visibilità del cursore

            // Creo gli oggetti Ponte e Parcheggio con i relativi argomenti
            _ponte = new Ponte(COORDINATE_PONTE[0], COORDINATE_PONTE[1], MAX_AUTO, _semaphore, lockConsole: _lockConsole);
            
            _park = new Parcheggio(COORDINATE_PARCHEGGIO[0], COORDINATE_PARCHEGGIO[1], lockConsole: _lockConsole);

            // Thread che gestisce il menu
            Thread menu = new Thread(Menu) { Name="ConsoleMenu" };

            // Thread che gestisce l'uscita delle macchine dal parcheggio
            Thread parcheggio = new Thread(GestioneParcheggio) { Name="ConsoleGestioneParcheggio"};

            // Avvio i Thread
            parcheggio.Start();
            menu.Start();

            // Stampo il menu
            StampaMenu();

            // Non è necessario terminare i Thread in quanto il programma si fermerà solo quando
            // verrà premuto il tasto "U", e quando ciò accade ci sarà Enviroment.Exit(0) che uccide 
            // anche tutti i thread in foreground.
            // Anche se la Console viene chiusa dalla X in alto a destra o dalla gestione attività, verranno 
            // anche uccisi tutti i thread in foreground.
        }



        static void GestioneParcheggio()
        {
            while (true)
            {
                if (_park.NumeroMacchine > 0) // Se nel parcheggio sono presenti macchine
                {
                    _semaphore.Wait(); // Aspetta un posto libero nel ponte
                    Auto a = _park.FaiUscireAuto(); // Fai uscire un'auto dal parcheggio 
                    _ponte.AggiungiMacchina(a); // e mettila nel ponte
                    
                }
            }
        }

        #region Stampe
        /// <summary>
        /// Stampa il menù
        /// </summary>
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

        /// <summary>
        /// Gestione del menu
        /// </summary>
        static void Menu()
        {
            Random rnd = new Random(); // Seme randomico per velocità delle automobili
            do
            {
                // Input del carattere
                char key = char.ToUpper(Console.ReadKey(true).KeyChar);

                switch (key)
                {
                    case 'O': // Comando apertura ponte
                        _ponte.OpenPonte();
                        break;
                    case 'C': // Comando chiusura ponte
                        _ponte.ChiudiPonte();
                        break;
                    case 'A': // Comando aggiunta macchina al parcheggio
                        _park.AggiungiMacchina(new Auto(_lockConsole, rnd.Next(0,100)));
                        break;
                    case 'U': // Comando di uscita
                        // Environment.Exit(0) chiude l'intero processo, compresi i thread
                        // lo 0 è il codice di uscita, che quando è 0 generalmente significa
                        // che non ci sono stati errori.
                        Environment.Exit(0);
                        break;
                }

            } while (true);
        }
    }
}
