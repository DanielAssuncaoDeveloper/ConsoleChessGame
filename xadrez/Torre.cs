using tabuleiro;

namespace xadrez
{
    class Torre : Peca
    {
        public Torre(Cor cor, Tabuleiro tab)
            : base(cor, tab) { }

        public override string ToString()
        {
            return "T";
        }

        public bool MovimentoPossivel(Posicao pos)
        {
            Peca p = tab.FindPeca(pos);
            return p == null || p.cor != cor;
        }

        public override bool[,] MovimentosValidos()
        {
            bool[,] movPosiveis = new bool[tab.Linhas, tab.Colunas];
            int linha = posicao.Linha;
            int coluna = posicao.Coluna;

            Posicao pos = new Posicao(0, 0);

            // norte
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna);
            while (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (tab.FindPeca(pos) != null && tab.FindPeca(pos).cor != cor)
                {
                    break;
                }
                pos.Linha -= 1;

            }

            // oeste
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

            // sul
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


            return movPosiveis;
        }

    }
}
