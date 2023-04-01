using System;

namespace ConsoleApp_PonteLevatoio
{
    static class MyConsoleUtils
    {
        public static void Scrivi(string testo, object lck = null, int x = -1, int y = -1, ConsoleColor fore = ConsoleColor.Gray, ConsoleColor back = ConsoleColor.Black)
        {
            if (x == -1)
                x = Console.CursorLeft;
            if (y == -1)
                y = Console.CursorTop;

            if (x >= Console.BufferWidth || y >= Console.BufferHeight)
                return;

            if (lck == null)
            {
                Console.ForegroundColor = fore;
                Console.BackgroundColor = back;
                Console.SetCursorPosition(x, y);
                Console.Write(testo);
                return;
            }

            lock (lck)
            {
                Console.ForegroundColor = fore;
                Console.BackgroundColor = back;
                Console.SetCursorPosition(x, y);
                Console.Write(testo);
            }
        }
    }
}
