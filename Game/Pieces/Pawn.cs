using System;
using System.Collections.Generic;
using tabuleiro;

namespace Xadrez_Console.Game.Pieces
{
    class Pawn : Piece
    {
        private GameService partida;

        public Pawn(Color cor, Board tab, GameService partida)
            : base(cor, tab)
        {
            this.partida = partida;
        }

        private bool ExisteInimigo(PositionBoard pos)
        {
            Piece p = Board.GetPiece(pos);
            return p != null && p.Color != Color;
        }

        public bool MovimentoPossivel(PositionBoard pos)
        {
            Piece p = Board.GetPiece(pos);
            if (pos.Coluna != Position.Coluna)
            {
                return ExisteInimigo(pos);
            }
            return p == null;
        }

        public override bool[,] MovimentosValidos()
        {
            bool[,] movPosiveis = new bool[Board.Linhas, Board.Colunas];

            PositionBoard pos = new PositionBoard(0, 0);

            if (Color == Color.Branca)
            {
                // Inicio do Jogo
                pos.DefinirValores(Position.Linha - 2, Position.Coluna);
                if (NumberOfMovements == 0)
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // N
                pos.DefinirValores(Position.Linha - 1, Position.Coluna);
                if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // NE
                pos.DefinirValores(Position.Linha - 1, Position.Coluna + 1);
                if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // NO
                pos.DefinirValores(Position.Linha - 1, Position.Coluna - 1);
                if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }


                // Jogada Especial: En Passant
                if (Position.Linha == 3)
                {
                    PositionBoard esquerda = new PositionBoard(Position.Linha, Position.Coluna - 1);
                    if (Board.PosicaoValida(esquerda) && ExisteInimigo(esquerda) &&
                        Board.GetPiece(esquerda) == partida.vulneravelEnPassant)
                    {
                        movPosiveis[esquerda.Linha - 1, esquerda.Coluna] = true;
                    }
                    PositionBoard direita = new PositionBoard(Position.Linha, Position.Coluna + 1);
                    if (Board.PosicaoValida(direita) && ExisteInimigo(direita) &&
                        Board.GetPiece(direita) == partida.vulneravelEnPassant)
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
                if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // NE
                pos.DefinirValores(Position.Linha + 1, Position.Coluna + 1);
                if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // NO
                pos.DefinirValores(Position.Linha + 1, Position.Coluna - 1);
                if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

            }


            // Jogada Especial: En Passant
            if (Position.Linha == 4)
            {
                PositionBoard esquerda = new PositionBoard(Position.Linha, Position.Coluna - 1);
                if (Board.PosicaoValida(esquerda) && ExisteInimigo(esquerda) &&
                    Board.GetPiece(esquerda) == partida.vulneravelEnPassant)
                {
                    movPosiveis[esquerda.Linha + 1, esquerda.Coluna] = true;
                }
                PositionBoard direita = new PositionBoard(Position.Linha, Position.Coluna + 1);
                if (Board.PosicaoValida(direita) && ExisteInimigo(direita) &&
                    Board.GetPiece(direita) == partida.vulneravelEnPassant)
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
