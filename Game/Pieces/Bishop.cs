using System;
using System.Collections.Generic;

using tabuleiro;
using ConsoleChessGame.Game.Enum;
using ConsoleChessGame.Game.Pieces.Abstract;

namespace ConsoleChessGame.Game.Pieces
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
            pos.SetValues(Position.Row - 1, Position.Column + 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPossiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Row--;
                pos.Column++;
            }

            // SE
            pos.SetValues(Position.Row + 1, Position.Column + 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPossiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Row++;
                pos.Column++;
            }


            // SO
            pos.SetValues(Position.Row + 1, Position.Column - 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPossiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Row++;
                pos.Column--;
            }


            // SE 
            pos.SetValues(Position.Row - 1, Position.Column - 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPossiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Row--;
                pos.Column--;
            }
            return movPossiveis;
        }


        public override string ToString()
        {
            return "B";
        }

    }
}
