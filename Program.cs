using System;
using tabuleiro;
using Xadrez_Console.Board.Exception;
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
                        Screen.PrintGame(game);

                        // Lendo posição de origem para iniciar a jogada
                        Console.Write("Origem: ");
                        PositionOnBoard origin = Screen.ReadPosition();

                        game.ValidateOriginPosition(origin); // IM STOPPED HERE

                        bool[,] possicoesPossiveis = game.Board.GetPiece(origin).GetValidMoves();
                        Console.Clear();
                        Screen.ImprimirTabuleiro(game.Board, possicoesPossiveis);

                        Console.WriteLine();
                        Console.Write("Destino: ");
                        PositionOnBoard destino = Screen.ReadPosition();
                        game.ValidarPosicaoDestino(origin, destino);

                        Console.WriteLine(destino);
                        game.RealizarJogada(origin, destino);
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
