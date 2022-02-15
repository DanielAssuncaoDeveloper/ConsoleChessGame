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
            PartidaDeXadrez partida = new PartidaDeXadrez();
            while (partida.jogoFinalizado == false)
            {
                Console.Clear();
                Tela.ImprimirTabuleiro(partida.tab);

                Console.Write("Origem: ");
                Posicao origem = Tela.LerPosicao().ToPosicao();
                Console.Write("Destino: ");
                Posicao destino = Tela.LerPosicao().ToPosicao();
                Console.WriteLine(destino);
                partida.ExecutarMovimento(origem, destino);

            }
        }
    }
}
