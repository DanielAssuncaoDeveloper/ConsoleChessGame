using tabuleiro;
using Xadrez_Console.Game.Enum;

namespace Xadrez_Console.Game.Pieces.Abstract
{
    /// <summary>
    /// Uma classe abstrata para ser herdada de outras peças implementadas
    /// </summary>
    abstract class Piece
    {
        public PositionOnBoard Position { get; set; }
        public Color Color { get; protected set; }
        public int NumberOfMovements { get; protected set; }
        public BoardService Board { get; protected set; }

        public Piece(Color color, BoardService board)
        {
            Position = null;
            Color = color;
            Board = board;
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

        /// <summary>
        /// Verifica se a peça tem algum movimento possível a ser realizado
        /// </summary>
        /// <returns>Boleano (<see langword="true"/> ou <see langword="false"/>)</returns>
        public bool ExistsPossibleMoves()
        {
            // Consultando os movimentos válidos da peça
            bool[,] validMoves = GetValidMoves();

            // Percorrendo cada posição para verificar se existe uma posição válida
            for (int row = 0; row < Board.Linhas; row++)
            {
                for (int column = 0; column < Board.Colunas; column++)
                {
                    if (validMoves[row, column])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool PodeMoverPara(PositionOnBoard pos)
        {
            return GetValidMoves()[pos.Linha, pos.Coluna];
        }

        /// <summary>
        /// Obtêm os movimentos possíveis da peça
        /// </summary>
        /// <returns>Uma matriz boleana onde cada elemento <see langword="true"/> indica que a posição é válida e
        /// onde for <see langword="false"/>, indica que sa posição é inválida</returns>
        public abstract bool[,] GetValidMoves();


    }
}
