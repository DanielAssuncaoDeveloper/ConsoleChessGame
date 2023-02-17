using System;
using System.Collections.Generic;
using tabuleiro;
using ConsoleChessGame.Game.Enum;
using ConsoleChessGame.Game.Pieces.Abstract;

namespace ConsoleChessGame.Game.Pieces
{
    class Pawn : Piece
    {
        private GameService partida;

        public Pawn(Color cor, tabuleiro.BoardService tab, GameService partida)
            : base(cor, tab)
        {
            this.partida = partida;
        }

        private bool ExisteInimigo(PositionOnBoard pos)
        {
            Piece p = Board.GetPiece(pos);
            return p != null && p.Color != Color;
        }

        public bool MovimentoPossivel(PositionOnBoard pos)
        {
            Piece p = Board.GetPiece(pos);
            if (pos.Coluna != Position.Coluna)
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
                pos.DefinirValores(Position.Linha - 2, Position.Coluna);
                if (NumberOfMovements == 0)
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // N
                pos.DefinirValores(Position.Linha - 1, Position.Coluna);
                if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // NE
                pos.DefinirValores(Position.Linha - 1, Position.Coluna + 1);
                if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // NO
                pos.DefinirValores(Position.Linha - 1, Position.Coluna - 1);
                if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }


                // Jogada Especial: En Passant
                if (Position.Linha == 3)
                {
                    PositionOnBoard esquerda = new PositionOnBoard(Position.Linha, Position.Coluna - 1);
                    if (Board.IsValidPosition(esquerda) && ExisteInimigo(esquerda) &&
                        Board.GetPiece(esquerda) == partida.VulnerablePieceForEnPassant)
                    {
                        movPosiveis[esquerda.Linha - 1, esquerda.Coluna] = true;
                    }
                    PositionOnBoard direita = new PositionOnBoard(Position.Linha, Position.Coluna + 1);
                    if (Board.IsValidPosition(direita) && ExisteInimigo(direita) &&
                        Board.GetPiece(direita) == partida.VulnerablePieceForEnPassant)
                    {
                        movPosiveis[direita.Linha - 1, direita.Coluna] = true;
                    }
                }



            }
            else
            {
                // Inicio do Jogo
                pos.DefinirValores(Position.Linha + 2, Position.Coluna);
                if (NumberOfMovements == 0)
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // N
                pos.DefinirValores(Position.Linha + 1, Position.Coluna);
                if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // NE
                pos.DefinirValores(Position.Linha + 1, Position.Coluna + 1);
                if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // NO
                pos.DefinirValores(Position.Linha + 1, Position.Coluna - 1);
                if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

            }


            // Jogada Especial: En Passant
            if (Position.Linha == 4)
            {
                PositionOnBoard esquerda = new PositionOnBoard(Position.Linha, Position.Coluna - 1);
                if (Board.IsValidPosition(esquerda) && ExisteInimigo(esquerda) &&
                    Board.GetPiece(esquerda) == partida.VulnerablePieceForEnPassant)
                {
                    movPosiveis[esquerda.Linha + 1, esquerda.Coluna] = true;
                }
                PositionOnBoard direita = new PositionOnBoard(Position.Linha, Position.Coluna + 1);
                if (Board.IsValidPosition(direita) && ExisteInimigo(direita) &&
                    Board.GetPiece(direita) == partida.VulnerablePieceForEnPassant)
                {
                    movPosiveis[direita.Linha + 1, direita.Coluna] = true;
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
