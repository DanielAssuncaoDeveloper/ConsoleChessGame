using tabuleiro;

namespace Xadrez_Console.Game
{
    class PositionChess
    {
        public int Linha { get; set; }
        public char Coluna { get; set; }

        public PositionChess(char coluna, int linha)
        {
            Linha = linha;
            Coluna = coluna;
        }

        public Position ToPosicao()
        {
            return new Position(8 - Linha, Coluna - 'a');
        }

        public override string ToString()
        {
            return $"{Coluna}{Linha}";
        }
    }
}
