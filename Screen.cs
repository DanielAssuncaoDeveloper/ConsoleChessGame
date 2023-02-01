using System;
using System.Collections.Generic;
using System.Linq;
using tabuleiro;
using Xadrez_Console.Game;

namespace Xadrez_Console
{
    class Screen
    {

        /// <summary>
        /// Imprime o jogo de xadrez
        /// </summary>
        /// <param name="game">Jogo a ser utilizado para a impressão</param>
        public static void PrintGame(GameService game)
        {
            // Imprimindo o tabuleiro
            PrintBoard(game.Board);
            Console.WriteLine();

            // Imprimindo peças capturadas
            PrintCapturedPieces(game);
            Console.WriteLine();

            // Imprimindo turno atual
            Console.WriteLine($"Turno: {game.turno}");

            if (!game.jogoFinalizado)
            {
                Console.WriteLine($"Aguardando jogada da peça: {game.CurrentPlayerColor}");

                if (game.xeque)
                {
                    Console.WriteLine("XEQUE!");
                }
            }
            else
            {
                Console.WriteLine("XEQUEMATE!");
                Console.WriteLine($"Jogador Vencedor: {game.CurrentPlayerColor}");
            }
        }

        /// <summary>
        /// Imprime as peças capturadas na partida
        /// </summary>
        /// <param name="partida">Instância de GameService para obter as peças capturadas na partida</param>
        public static void PrintCapturedPieces(GameService partida)
        {
            Console.WriteLine("Pecas Capturadas:");

            // Imprimindo peças Brancas capturadas
            Console.Write("Brancas: ");

            var capturedWhitePieces = partida.CapturedPieces(Color.Branca);
            PrintCapturedGroup(capturedWhitePieces);
            Console.WriteLine();
            
            ConsoleColor colorDefault = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;

            // Imprimindo peças Vermelhas
            Console.Write("Vermelhas: ");
            PrintCapturedGroup(partida.CapturedPieces(Color.Preta));

            Console.ForegroundColor = colorDefault;
            Console.WriteLine();

        }

        /// <summary>
        /// Imprime o grupo de peças informadas
        /// </summary>
        /// <param name="groupPieces">Grupo de peças a serem impressos</param>
        public static void PrintCapturedGroup(IEnumerable<Piece> groupPieces)
        {
            Console.Write('[');
            
            groupPieces.ToList()
                .ForEach(p => 
                        Console.Write(p.ToString())
                    );

            Console.Write(']');
        }

        /// <summary>
        /// Imprime o tabuleiro (juntamente com as peças e cordenadas)
        /// </summary>
        /// <param name="board">Tabuleiro a ser utilizado para a impressão</param>
        public static void PrintBoard(Board board)
        {
            for (int row = 0; row < board.Linhas; row++)
            {
                // Imprimindo a linha do tabuleiro
                Console.Write($" {8 - row}   ");

                for (int column = 0; column < board.Colunas; column++)
                {
                    // Consultando a peça referente a posição atual e a imprimindo
                    var piece = board.GetPiece(row, column);
                    PrintPiece(piece);                    
                }
                
                // Pulando para a próxima linha
                Console.WriteLine();
            }

            // Imprimindo colunas
            Console.WriteLine();
            Console.WriteLine("     a b c d e f g h");
        }
        public static void ImprimirTabuleiro(Board tab, bool[,] movimentosPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write($" {8 - i}   ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (movimentosPossiveis[i, j])
                    {
                        Console.BackgroundColor = fundoAlterado;
                    }
                    else
                    {
                        Console.BackgroundColor = fundoOriginal;
                    }
                    PrintPiece(tab.GetPiece(i, j));
                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("     a b c d e f g h");
            Console.BackgroundColor = fundoOriginal;
        }

        /// <summary>
        /// Lê uma posição informada pelo jogador
        /// </summary>
        /// <returns></returns>
        public static PositionBoard ReadPosition()
        {
            string positionInputed = Console.ReadLine().ToLower();

            char column = positionInputed[0];
            int row = int.Parse(positionInputed[1].ToString());

            var positionChess = new PositionChess(column, row);
            return positionChess.ConvertToPositionBoard();
        }

        /// <summary>
        /// Imprime a peça informada pelo argumento na tela
        /// </summary>
        /// <param name="piece">Peça que deseja ser impressa</param>
        public static void PrintPiece(Piece piece)
        {
            // Caso a peça seja nula, imprime a posição vazia
            if (piece is null)
            {
                Console.Write("-");
            }
            else
            {
                // Imprimindo a peça de acordo com sua cor
                if (piece.Color == Color.Branca)
                {
                    Console.Write(piece);
                }
                else
                {
                    ConsoleColor colorDefault = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    Console.Write(piece);
                    Console.ForegroundColor = colorDefault;
                }
            }

            // Imprimindo um espaço para a próxima peça
            Console.Write(" ");
        }

    }
}
