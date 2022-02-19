namespace tabuleiro
{
    abstract class Peca
    { 
        public Posicao posicao { get; set; }
        public Cor cor { get; protected set; }
        public int QtdMovimento { get; protected set; }
        public Tabuleiro tab { get; protected set; }

        public Peca (Cor cor, Tabuleiro tab)
        {
            this.posicao = null;
            this.cor = cor;
            this.tab = tab;
            QtdMovimento = 0;
        }

        public void IncrementarMovimentos()
        {
            QtdMovimento++;
        }

        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mov = MovimentosValidos();
            for (int i = 0; i < tab.Linhas; i++)
            {
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (mov[i,j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool PodeMoverPara(Posicao pos)
        {
            return MovimentosValidos()[pos.Linha, pos.Coluna];
        }

        public abstract bool[,] MovimentosValidos();


    }
}
