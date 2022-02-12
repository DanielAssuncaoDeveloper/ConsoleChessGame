using System;

using tabuleiro;

namespace Xadrez_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Tabuleiro tab = new Tabuleiro(9, 8);
            Tela.ImprimirTabuleiro(tab);

        }
    }
}
