using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace Game21
{
    internal delegate void OnKey(ConsoleKeyInfo key);

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);
            Console.CursorVisible = false;

            Casino casino = new Casino();
            casino.BeginGame();
        }
    }
}