using tabuleiro;

namespace Xadrez_Console.Game
{
    /// <summary>
    /// Representa as posições do Xadrez (1-8/A-H)
    /// </summary>
    class PositionChess
    {
        public int Row { get; set; }
        public char Column { get; set; }

        public PositionChess(char column, int row)
        {
            Row = row;
            Column = column;
        }

        /// <summary>
        /// Realiza a conversão da classe PositionChess para a classe PositionBoard
        /// </summary>
        public PositionBoard ConvertToPositionBoard()
        {
            int rowPosition = 8 - Row;
            int columnPosition = Column - 'a';

            return new PositionBoard(rowPosition, columnPosition);
        }

        public override string ToString()
        {
            return $"{Column}{Row}";
        }
    }
}
