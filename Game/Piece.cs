using tabuleiro;

namespace Xadrez_Console.Game
{
    abstract class Piece
    {
        public PositionBoard Position { get; set; }
        public Color Color { get; protected set; }
        public int NumberOfMovements { get; protected set; }
        public Board Board { get; protected set; }

        public Piece(Color color, Board board)
        {
            Position = null;
            this.Color = color;
            this.Board = board;
            NumberOfMovements = 0;
        }

        public void IncrementarMovimentos()
        {
            NumberOfMovements++;
        }
        public void DecrementarMovimentos()
        {
            NumberOfMovements--;
        }

        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mov = MovimentosValidos();
            for (int i = 0; i < Board.Linhas; i++)
            {
                for (int j = 0; j < Board.Colunas; j++)
                {
                    if (mov[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool PodeMoverPara(PositionBoard pos)
        {
            return MovimentosValidos()[pos.Linha, pos.Coluna];
        }

        public abstract bool[,] MovimentosValidos();


    }
}
