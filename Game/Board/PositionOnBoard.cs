namespace tabuleiro
{
    /// <summary>
    /// Representa a posição em uma matriz referente ao tabuleiro de 8/8
    /// </summary>
    class PositionOnBoard
    {
        public int Linha { get; set; }
        public int Coluna { get; set; }

        public PositionOnBoard(int linha, int coluna)
        {
            Linha = linha;
            Coluna = coluna;
        }

        public void DefinirValores(int linha, int coluna)
        {
            Linha = linha;
            Coluna = coluna;
        }

        public override string ToString()
        {
            return $"{Linha}, {Coluna}";
        }
    }
}
