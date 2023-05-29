using System;
using System.Collections.Generic;

using tabuleiro;
using ConsoleChessGame.Game.Enum;
using ConsoleChessGame.Game.Pieces.Abstract;

namespace ConsoleChessGame.Game.Pieces
{
    class Queen : Piece
    {
        public Queen(Color cor, tabuleiro.BoardService tab)
            : base(cor, tab) { }



        public bool MovimentoPossivel(PositionOnBoard pos)
        {
            Piece p = Board.GetPiece(pos);
            return p == null || p.Color != Color;
        }

        public override bool[,] GetValidMoves()
        {
            bool[,] movPosiveis = new bool[Board.Linhas, Board.Colunas];

            PositionOnBoard pos = new PositionOnBoard(0, 0);

            // N
            pos.SetValues(Position.Row - 1, Position.Column);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Row--;

            }

            // NE
            pos.SetValues(Position.Row - 1, Position.Column + 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Row--;
                pos.Column++;
            }

            // E
            pos.SetValues(Position.Row, Position.Column + 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Column += 1;

            }

            // SE
            pos.SetValues(Position.Row + 1, Position.Column + 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Row++;
                pos.Column++;
            }

            // S
            pos.SetValues(Position.Row + 1, Position.Column);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Row += 1;

            }

            // SO
            pos.SetValues(Position.Row + 1, Position.Column + 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Row++;
                pos.Column++;
            }

            // O
            pos.SetValues(Position.Row, Position.Column - 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Column -= 1;

            }

            // NO
            pos.SetValues(Position.Row - 1, Position.Column - 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Row--;
                pos.Column--;
            }


            return movPosiveis;
        }

        public override string ToString()
        {
            return "D";
        }

    }
}
