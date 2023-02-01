using System;
using System.Collections.Generic;

using tabuleiro;

namespace Xadrez_Console.Game.Pieces
{
    class Queen : Piece
    {
        public Queen(Color cor, Board tab)
            : base(cor, tab) { }



        public bool MovimentoPossivel(PositionBoard pos)
        {
            Piece p = Board.GetPiece(pos);
            return p == null || p.Color != Color;
        }

        public override bool[,] MovimentosValidos()
        {
            bool[,] movPosiveis = new bool[Board.Linhas, Board.Colunas];

            PositionBoard pos = new PositionBoard(0, 0);

            // N
            pos.DefinirValores(Position.Linha - 1, Position.Coluna);
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Linha--;

            }

            // NE
            pos.DefinirValores(Position.Linha - 1, Position.Coluna + 1);
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Linha--;
                pos.Coluna++;
            }

            // E
            pos.DefinirValores(Position.Linha, Position.Coluna + 1);
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Coluna += 1;

            }

            // SE
            pos.DefinirValores(Position.Linha + 1, Position.Coluna + 1);
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Linha++;
                pos.Coluna++;
            }

            // S
            pos.DefinirValores(Position.Linha + 1, Position.Coluna);
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Linha += 1;

            }

            // SO
            pos.DefinirValores(Position.Linha + 1, Position.Coluna + 1);
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Linha++;
                pos.Coluna++;
            }

            // O
            pos.DefinirValores(Position.Linha, Position.Coluna - 1);
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Coluna -= 1;

            }

            // NO
            pos.DefinirValores(Position.Linha - 1, Position.Coluna - 1);
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Linha--;
                pos.Coluna--;
            }


            return movPosiveis;
        }

        public override string ToString()
        {
            return "D";
        }

    }
}
