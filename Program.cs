using System;
using tabuleiro;
using ConsoleChessGame.Board.Exception;
using ConsoleChessGame.Game;

namespace ConsoleChessGame
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                while (!GameService.FinishedGame)
                {
                    try
                    {
                        Console.Clear();
                        Screen.PrintGame();

                        // Lendo posição de origem para iniciar a jogada
                        Console.Write("Origem: ");
                        PositionOnBoard origin = Screen.ReadPosition();

                        GameService.ValidateOriginPosition(origin); // IM STOPPED HERE

                        bool[,] possicoesPossiveis = GameService.Board.GetPiece(origin).GetValidMoves();
                        Console.Clear();
                        Screen.ImprimirTabuleiro(GameService.Board, possicoesPossiveis);

                        Console.WriteLine();
                        Console.Write("Destino: ");
                        PositionOnBoard destino = Screen.ReadPosition();
                        GameService.ValidateDestinationPosition(origin, destino);

                        Console.WriteLine(destino);
                        GameService.MakeAPlay(origin, destino);
                    }
                    catch (ExceptionBoard e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    
                    }
                }
                Console.Clear();
                Screen.PrintGame();
            }
            catch (ExceptionBoard e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
