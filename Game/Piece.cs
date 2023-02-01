using tabuleiro;

namespace Xadrez_Console.Game
{
    abstract class Piece
    {
        public Position posicao { get; set; }
        public Color cor { get; protected set; }
        public int QtdMovimento { get; protected set; }
        public Board tab { get; protected set; }

        public Piece(Color cor, Board tab)
        {
            posicao = null;
            this.cor = cor;
            this.tab = tab;
            QtdMovimento = 0;
        }

        public void IncrementarMovimentos()
        {
            QtdMovimento++;
        }
        public void DecrementarMovimentos()
        {
            QtdMovimento--;
        }

        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mov = MovimentosValidos();
            for (int i = 0; i < tab.Linhas; i++)
            {
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (mov[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool PodeMoverPara(Position pos)
        {
            return MovimentosValidos()[pos.Linha, pos.Coluna];
        }

        public abstract bool[,] MovimentosValidos();


    }
}
