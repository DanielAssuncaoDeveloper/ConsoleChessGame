using System;
using System.Collections.Generic;
using tabuleiro;
using ConsoleChessGame.Game.Enum;
using ConsoleChessGame.Game.Pieces.Abstract;

namespace ConsoleChessGame.Game.Pieces
{
    class Pawn : Piece
    {
        public Pawn(Color cor, tabuleiro.BoardService tab)
            : base(cor, tab)
        { }

        private bool ExisteInimigo(PositionOnBoard pos)
        {
            Piece p = Board.GetPiece(pos);
            return p != null && p.Color != Color;
        }

        public bool MovimentoPossivel(PositionOnBoard pos)
        {
            Piece p = Board.GetPiece(pos);
            if (pos.Column != Position.Column)
            {
                return ExisteInimigo(pos);
            }
            return p == null;
        }

        public override bool[,] GetValidMoves()
        {
            bool[,] movPosiveis = new bool[Board.Linhas, Board.Colunas];

            PositionOnBoard pos = new PositionOnBoard(0, 0);

            if (Color == Color.White)
            {
                // Inicio do Jogo
                pos.SetValues(Position.Row - 2, Position.Column);
                if (NumberOfMovements == 0)
                {
                    movPosiveis[pos.Row, pos.Column] = true;
                }

                // N
                pos.SetValues(Position.Row - 1, Position.Column);
                if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Row, pos.Column] = true;
                }

                // NE
                pos.SetValues(Position.Row - 1, Position.Column + 1);
                if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Row, pos.Column] = true;
                }

                // NO
                pos.SetValues(Position.Row - 1, Position.Column - 1);
                if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Row, pos.Column] = true;
                }


                // Jogada Especial: En Passant
                if (Position.Row == 3)
                {
                    PositionOnBoard esquerda = new PositionOnBoard(Position.Row, Position.Column - 1);
                    if (Board.IsValidPosition(esquerda) && ExisteInimigo(esquerda) &&
                        Board.GetPiece(esquerda) == GameService.VulnerablePieceForEnPassant)
                    {
                        movPosiveis[esquerda.Row - 1, esquerda.Column] = true;
                    }
                    PositionOnBoard direita = new PositionOnBoard(Position.Row, Position.Column + 1);
                    if (Board.IsValidPosition(direita) && ExisteInimigo(direita) &&
                        Board.GetPiece(direita) == GameService.VulnerablePieceForEnPassant)
                    {
                        movPosiveis[direita.Row - 1, direita.Column] = true;
                    }
                }



            }
            else
            {
                // Inicio do Jogo
                pos.SetValues(Position.Row + 2, Position.Column);
                if (NumberOfMovements == 0)
                {
                    movPosiveis[pos.Row, pos.Column] = true;
                }

                // N
                pos.SetValues(Position.Row + 1, Position.Column);
                if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Row, pos.Column] = true;
                }

                // NE
                pos.SetValues(Position.Row + 1, Position.Column + 1);
                if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Row, pos.Column] = true;
                }

                // NO
                pos.SetValues(Position.Row + 1, Position.Column - 1);
                if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Row, pos.Column] = true;
                }

            }


            // Jogada Especial: En Passant
            if (Position.Row == 4)
            {
                PositionOnBoard esquerda = new PositionOnBoard(Position.Row, Position.Column - 1);
                if (Board.IsValidPosition(esquerda) && ExisteInimigo(esquerda) &&
                    Board.GetPiece(esquerda) == GameService.VulnerablePieceForEnPassant)
                {
                    movPosiveis[esquerda.Row + 1, esquerda.Column] = true;
                }
                PositionOnBoard direita = new PositionOnBoard(Position.Row, Position.Column + 1);
                if (Board.IsValidPosition(direita) && ExisteInimigo(direita) &&
                    Board.GetPiece(direita) == GameService.VulnerablePieceForEnPassant)
                {
                    movPosiveis[direita.Row + 1, direita.Column] = true;
                }
            }

            return movPosiveis;
        }

        public override string ToString()
        {
            return "P";
        }

    }
}
