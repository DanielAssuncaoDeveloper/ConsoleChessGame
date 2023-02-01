using System;
using tabuleiro;
using Xadrez_Console.Game;

namespace Xadrez_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                GameService game = new GameService();

                while (!game.jogoFinalizado)
                {
                    try
                    {
                        Console.Clear();
                        Screen.PrintGame(game); // IM STOPPED HERE

                        // Lendo posição de origem para iniciar a jogada.
                        Console.Write("Origem: ");
                        Position origem = Screen.LerPosicao().ToPosicao();

                        game.ValidarPosicaoOrigem(origem);

                        bool[,] possicoesPossiveis = game.tab.FindPeca(origem).MovimentosValidos();
                        Console.Clear();
                        Screen.ImprimirTabuleiro(game.tab, possicoesPossiveis);

                        Console.WriteLine();
                        Console.Write("Destino: ");
                        Position destino = Screen.LerPosicao().ToPosicao();
                        game.ValidarPosicaoDestino(origem, destino);

                        Console.WriteLine(destino);
                        game.RealizarJogada(origem, destino);
                    }
                    catch (ExceptionBoard e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    
                    }
                }
                Console.Clear();
                Screen.PrintGame(game);
            }
            catch (ExceptionBoard e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
