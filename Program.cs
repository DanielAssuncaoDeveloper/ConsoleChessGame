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
            try
            {
                Console.WriteLine();
                PartidaDeXadrez partida = new PartidaDeXadrez();
                while (partida.jogoFinalizado == false)
                {
                    try
                    {
                        Console.Clear();
                        Tela.ImprimirPartida(partida);

                        Console.Write("Origem: ");
                        Posicao origem = Tela.LerPosicao().ToPosicao();
                        partida.ValidarPosicaoOrigem(origem);

                        bool[,] possicoesPossiveis = partida.tab.FindPeca(origem).MovimentosValidos();
                        Console.Clear();
                        Tela.ImprimirTabuleiro(partida.tab, possicoesPossiveis);

                        Console.WriteLine();
                        Console.Write("Destino: ");
                        Posicao destino = Tela.LerPosicao().ToPosicao();
                        partida.ValidarPosicaoDestino(origem, destino);

                        Console.WriteLine(destino);
                        partida.RealizarJogada(origem, destino);
                    }
                    catch (TabuleiroException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                }
            }
            catch (TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            } 
        }
    }
}
