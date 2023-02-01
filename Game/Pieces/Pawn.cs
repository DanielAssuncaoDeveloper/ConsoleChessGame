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

        private bool ExisteInimigo(Position pos)
        {
            Piece p = tab.FindPeca(pos);
            return p != null && p.cor != cor;
        }

        public bool MovimentoPossivel(Position pos)
        {
            Piece p = tab.FindPeca(pos);
            if (pos.Coluna != posicao.Coluna)
            {
                return ExisteInimigo(pos);
            }
            return p == null;
        }

        public override bool[,] MovimentosValidos()
        {
            bool[,] movPosiveis = new bool[tab.Linhas, tab.Colunas];

            Position pos = new Position(0, 0);

            if (cor == Color.Branca)
            {
                // Inicio do Jogo
                pos.DefinirValores(posicao.Linha - 2, posicao.Coluna);
                if (QtdMovimento == 0)
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // N
                pos.DefinirValores(posicao.Linha - 1, posicao.Coluna);
                if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // NE
                pos.DefinirValores(posicao.Linha - 1, posicao.Coluna + 1);
                if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // NO
                pos.DefinirValores(posicao.Linha - 1, posicao.Coluna - 1);
                if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }


                // Jogada Especial: En Passant
                if (posicao.Linha == 3)
                {
                    Position esquerda = new Position(posicao.Linha, posicao.Coluna - 1);
                    if (tab.PosicaoValida(esquerda) && ExisteInimigo(esquerda) &&
                        tab.FindPeca(esquerda) == partida.vulneravelEnPassant)
                    {
                        movPosiveis[esquerda.Linha - 1, esquerda.Coluna] = true;
                    }
                    Position direita = new Position(posicao.Linha, posicao.Coluna + 1);
                    if (tab.PosicaoValida(direita) && ExisteInimigo(direita) &&
                        tab.FindPeca(direita) == partida.vulneravelEnPassant)
                    {
                        movPosiveis[direita.Linha - 1, direita.Coluna] = true;
                    }
                }



            }
            else
            {
                // Inicio do Jogo
                pos.DefinirValores(posicao.Linha + 2, posicao.Coluna);
                if (QtdMovimento == 0)
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // N
                pos.DefinirValores(posicao.Linha + 1, posicao.Coluna);
                if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // NE
                pos.DefinirValores(posicao.Linha + 1, posicao.Coluna + 1);
                if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

                // NO
                pos.DefinirValores(posicao.Linha + 1, posicao.Coluna - 1);
                if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
                {
                    movPosiveis[pos.Linha, pos.Coluna] = true;
                }

            }


            // Jogada Especial: En Passant
            if (posicao.Linha == 4)
            {
                Position esquerda = new Position(posicao.Linha, posicao.Coluna - 1);
                if (tab.PosicaoValida(esquerda) && ExisteInimigo(esquerda) &&
                    tab.FindPeca(esquerda) == partida.vulneravelEnPassant)
                {
                    movPosiveis[esquerda.Linha + 1, esquerda.Coluna] = true;
                }
                Position direita = new Position(posicao.Linha, posicao.Coluna + 1);
                if (tab.PosicaoValida(direita) && ExisteInimigo(direita) &&
                    tab.FindPeca(direita) == partida.vulneravelEnPassant)
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
