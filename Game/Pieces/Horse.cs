using System;
using System.Collections.Generic;

using tabuleiro;
using ConsoleChessGame.Game.Enum;
using ConsoleChessGame.Game.Pieces.Abstract;

namespace ConsoleChessGame.Game.Pieces
{
    class Horse : Piece
    {
        public Horse(Color cor, tabuleiro.BoardService tab)
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

            // N + O
            pos.SetValues(Position.Row - 2, Position.Column - 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // N + E
            pos.SetValues(Position.Row - 2, Position.Column + 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // E + N
            pos.SetValues(Position.Row - 1, Position.Column + 2);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // E + S
            pos.SetValues(Position.Row + 1, Position.Column + 2);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // S + E
            pos.SetValues(Position.Row + 2, Position.Column + 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // S + O
            pos.SetValues(Position.Row + 2, Position.Column - 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // O + S
            pos.SetValues(Position.Row + 1, Position.Column - 2);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // O + N
            pos.SetValues(Position.Row - 1, Position.Column - 2);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            return movPosiveis;
        }


        public override string ToString()
        {
            return "C";
        }
    }
}
