using System;
using System.Collections.Generic;

using tabuleiro;

namespace xadrez
{
    class Piao : Peca
    {
        public Piao(Cor cor, Tabuleiro tab)
            : base(cor, tab) { }



        public bool MovimentoPossivel(Posicao pos)
        {
            Peca p = tab.FindPeca(pos);
            if (pos.Coluna != posicao.Coluna)
            {
                return p != null && p.cor != cor;
            }
            return p == null;
        }

        public override bool[,] MovimentosValidos()
        {
            bool[,] movPosiveis = new bool[tab.Linhas, tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            if (cor == Cor.Branca)
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

            return movPosiveis;
        }

        public override string ToString()
        {
            return "P";
        }

    }
}
