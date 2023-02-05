using System;
using System.Collections.Generic;

using tabuleiro;
using Xadrez_Console.Game.Enum;
using Xadrez_Console.Game.Pieces.Abstract;

namespace Xadrez_Console.Game.Pieces
{
    class Bishop : Piece
    {
        public Bishop(Color cor, tabuleiro.BoardService tab)
            : base(cor, tab) { }

        public bool MovimentoPossivel(PositionOnBoard pos)
        {
            Piece p = Board.GetPiece(pos);
            return p == null || p.Color != Color;
        }

        public override bool[,] GetValidMoves()
        {
            bool[,] movPossiveis = new bool[Board.Linhas, Board.Colunas];
            PositionOnBoard pos = new PositionOnBoard(0, 0);

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
