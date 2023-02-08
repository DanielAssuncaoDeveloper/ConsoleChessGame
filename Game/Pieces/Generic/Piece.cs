using tabuleiro;
using ConsoleChessGame.Game.Enum;

namespace ConsoleChessGame.Game.Pieces.Abstract
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

        /// <summary>
        /// Realiza a incrementação de movimentos da peça
        /// </summary>
        public void IncreaseMovement()
        {
            NumberOfMovements++;
        }

        /// <summary>
        /// Realiza a decrementação de movimentos da peça
        /// </summary>
        public void DecrementMovement()
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

        /// <summary>
        /// Verifica se a peça pode mover para uma posição enviada por parametro
        /// </summary>
        /// <param name="position">Posição a qual deseja mover a peça</param>
        public bool CanMoveTo(PositionOnBoard position)
        {
            return GetValidMoves()[position.Linha, position.Coluna];
        }

        /// <summary>
        /// Obtêm os movimentos possíveis da peça
        /// </summary>
        /// <returns>Uma matriz boleana onde cada elemento <see langword="true"/> indica que a posição é válida e
        /// onde for <see langword="false"/>, indica que sa posição é inválida</returns>
        public abstract bool[,] GetValidMoves();
    }
}
