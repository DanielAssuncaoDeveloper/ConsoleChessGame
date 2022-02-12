
namespace tabuleiro
{
    class Tabuleiro
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }
        private Peca[,] pecas { get; set; }

        public Tabuleiro(int linhas, int colunas)
        {
            Linhas = linhas;
            Colunas = colunas;
            pecas = new Peca[linhas, colunas];
        }

        public Peca FindPeca(int linha, int coluna)
        {
            return pecas[linha, coluna];
        }
        public Peca FindPeca(Posicao pos)
        {
            return pecas[pos.Linha, pos.Coluna];
        }

        public void ColocarPeca(Peca p, Posicao pos)
        {
            pecas[pos.Linha, pos.Coluna] = p;
            p.posicao = pos;
        }


        public bool PecaValida(Posicao pos)
        {
            ValidarPosicao(pos);
            return FindPeca(pos) != null;
        }

        public void ValidarPosicao(Posicao pos)
        {
            if (!PosicaoValida(pos))
            {
                throw new TabuleiroException("Posição Inválida!");
            }
        }

        public bool PosicaoValida(Posicao pos)
        {
            if (pos.Linha < 0 || pos.Coluna < 0 || pos.Linha > Linhas || pos.Coluna > Colunas)
            {
                return false;
            }
            return true;
        }

    
    }
}
