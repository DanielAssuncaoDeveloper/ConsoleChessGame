using System;
using System.Collections.Generic;

using tabuleiro;

namespace xadrez
{
    class Dama : Peca
    {
        public Dama(Cor cor, Tabuleiro tab)
            : base(cor, tab) { }

       

        public bool MovimentoPossivel(Posicao pos)
        {
            Peca p = tab.FindPeca(pos);
            return p == null || p.cor != cor;
        }

        public override bool[,] MovimentosValidos()
        {
            bool[,] movPosiveis = new bool[tab.Linhas, tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            // N
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna);
            while (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (tab.FindPeca(pos) != null && tab.FindPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Linha--;

            }

            // NE
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna + 1);
            while (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (tab.FindPeca(pos) != null && tab.FindPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Linha--;
                pos.Coluna++;
            }

            // E
            pos.DefinirValores(posicao.Linha, posicao.Coluna + 1);
            while (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (tab.FindPeca(pos) != null && tab.FindPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Coluna += 1;

            }

            // SE
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna + 1);
            while (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (tab.FindPeca(pos) != null && tab.FindPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Linha++;
                pos.Coluna++;
            }

            // S
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna);
            while (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (tab.FindPeca(pos) != null && tab.FindPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Linha += 1;

            }

            // SO
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna + 1);
            while (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (tab.FindPeca(pos) != null && tab.FindPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Linha++;
                pos.Coluna++;
            }

            // O
            pos.DefinirValores(posicao.Linha, posicao.Coluna - 1);
            while (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (tab.FindPeca(pos) != null && tab.FindPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Coluna -= 1;

            }

            // NO
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna - 1);
            while (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (tab.FindPeca(pos) != null && tab.FindPeca(pos).cor != cor)
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
