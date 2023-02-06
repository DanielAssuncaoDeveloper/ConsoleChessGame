using tabuleiro;

namespace ConsoleChessGame.Game
{
    /// <summary>
    /// Representa as posições do Xadrez (1-8/A-H)
    /// </summary>
    class PositionOnGame
    {
        public int Row { get; set; }
        public char Column { get; set; }

        public PositionOnGame(char column, int row)
        {
            Row = row;
            Column = column;
        }

        /// <summary>
        /// Realiza a conversão da classe PositionOnGame para a classe PositionOnBoard
        /// </summary>
        public PositionOnBoard ConvertToPositionOnBoard()
        {
            int rowPosition = 8 - Row;
            int columnPosition = Column - 'a';

            return new PositionOnBoard(rowPosition, columnPosition);
        }

        public override string ToString()
        {
            return $"{Column}{Row}";
        }
    }
}
