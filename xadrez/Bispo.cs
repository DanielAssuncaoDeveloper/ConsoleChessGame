using System;
using System.Collections.Generic;

using tabuleiro;

namespace xadrez
{
    class Bispo : Peca
    {
        public Bispo(Cor cor, Tabuleiro tab)
            : base(cor, tab) { }

        public bool MovimentoPossivel(Posicao pos)
        {
            Peca p = tab.FindPeca(pos);
            return p == null || p.cor != cor;
        }

        public override bool[,] MovimentosValidos()
        {
            bool[,] movPossiveis = new bool[tab.Linhas, tab.Colunas];
            Posicao pos = new Posicao(0, 0);

            // NE
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna + 1);
            while (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPossiveis[pos.Linha, pos.Coluna] = true;
                if (tab.FindPeca(pos) != null && tab.FindPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Linha--;
                pos.Coluna++;
            }

            // SE
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna + 1);
            while (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPossiveis[pos.Linha, pos.Coluna] = true;
                if (tab.FindPeca(pos) != null && tab.FindPeca(pos).cor != cor )
                {
                    break;
                }
                pos.Linha++;
                pos.Coluna++;
            }


            // SO
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna - 1);
            while (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPossiveis[pos.Linha, pos.Coluna] = true;
                if (tab.FindPeca(pos) != null && tab.FindPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Linha++;
                pos.Coluna--;
            }


            // SE 
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna - 1);
            while (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPossiveis[pos.Linha, pos.Coluna] = true;
                if (tab.FindPeca(pos) != null && tab.FindPeca(pos).cor != cor)
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
