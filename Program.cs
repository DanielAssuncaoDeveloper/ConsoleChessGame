using System;
using System.Text.RegularExpressions;

using tabuleiro;
using xadrez;

namespace Xadrez_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Tabuleiro tab = new Tabuleiro(8, 8);
            Peca r = new Rei(Cor.Preta, tab);
            tab.ColocarPeca(r, new Posicao(0, 0));
            Peca t = new Torre(Cor.Branca, tab);
            tab.ColocarPeca(t, new Posicao(0, 1));


            Tela.ImprimirTabuleiro(tab);
        }
    }
}
