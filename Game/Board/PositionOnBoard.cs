namespace tabuleiro
{
    /// <summary>
    /// Representa a posição em uma matriz referente ao tabuleiro de 8/8
    /// </summary>
    class PositionOnBoard
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public PositionOnBoard(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public void SetValues(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return $"{Row}, {Column}";
        }
    }
}
