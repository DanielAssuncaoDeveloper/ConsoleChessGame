using tabuleiro;

namespace xadrez
{
    class Rei : Peca
    {
        public Rei(Cor cor, Tabuleiro tab) 
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

            // norte
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[posicao.Linha - 1, posicao.Coluna] = true;
            }

            // nordeste
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna + 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[posicao.Linha - 1, posicao.Coluna + 1] = true;
            }

            // oeste
            pos.DefinirValores(posicao.Linha, posicao.Coluna + 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[posicao.Linha, posicao.Coluna + 1] = true;
            }

            // sudeste
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna + 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[posicao.Linha + 1, posicao.Coluna + 1] = true;
            }

            // sul 
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[posicao.Linha + 1, posicao.Coluna] = true;
            }

            // suldoeste
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna - 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[posicao.Linha + 1, posicao.Coluna - 1] = true;
            }

            // leste
            pos.DefinirValores(posicao.Linha, posicao.Coluna - 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[posicao.Linha, posicao.Coluna - 1] = true;
            }

            // noroeste
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna - 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[posicao.Linha - 1, posicao.Coluna - 1] = true;
            }

            return movPosiveis;
        }


        public override string ToString()
        {
            return "R";
        }
    }
}
