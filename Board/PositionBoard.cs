namespace tabuleiro
{
    /// <summary>
    /// Representa as posições da matriz do tabuleiro (0-7/0-7)
    /// </summary>
    class PositionBoard
    {
        public int Linha { get; set; }
        public int Coluna { get; set; }

        public PositionBoard(int linha, int coluna)
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
