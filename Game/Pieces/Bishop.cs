using System;
using System.Collections.Generic;

using tabuleiro;

namespace Xadrez_Console.Game.Pieces
{
    class Bishop : Piece
    {
        public Bishop(Color cor, Board tab)
            : base(cor, tab) { }

        public bool MovimentoPossivel(PositionBoard pos)
        {
            Piece p = Board.GetPiece(pos);
            return p == null || p.Color != Color;
        }

        public override bool[,] MovimentosValidos()
        {
            bool[,] movPossiveis = new bool[Board.Linhas, Board.Colunas];
            PositionBoard pos = new PositionBoard(0, 0);

            // NE
            pos.DefinirValores(Position.Linha - 1, Position.Coluna + 1);
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPossiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Linha--;
                pos.Coluna++;
            }

            // SE
            pos.DefinirValores(Position.Linha + 1, Position.Coluna + 1);
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPossiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Linha++;
                pos.Coluna++;
            }


            // SO
            pos.DefinirValores(Position.Linha + 1, Position.Coluna - 1);
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPossiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Linha++;
                pos.Coluna--;
            }


            // SE 
            pos.DefinirValores(Position.Linha - 1, Position.Coluna - 1);
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPossiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Linha--;
                pos.Coluna--;
            }
            return movPossiveis;
        }


        public override string ToString()
        {
            return "B";
        }

    }
}
