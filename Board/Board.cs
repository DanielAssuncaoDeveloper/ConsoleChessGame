using Xadrez_Console.Game;

namespace tabuleiro
{
    class Board
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }
        private Piece[,] pecas { get; set; }

        public Board(int linhas, int colunas)
        {
            Linhas = linhas;
            Colunas = colunas;
            pecas = new Piece[linhas, colunas];
        }

        /// <summary>
        /// Retorna a peça posicionada nas cordenadas informadas pelos argumentos
        /// </summary>
        /// <param name="row">Linha do tabuleiro</param>
        /// <param name="column">Coluna do tabuleiro</param>
        public Piece GetPiece(int row, int column)
        {
            return pecas[row, column];
        }

        /// <summary>
        /// Retorna a peça posicionada nas cordenadas informadas pelos argumentos
        /// </summary>
        /// <param name="row">Linha do tabuleiro</param>
        /// <param name="column">Coluna do tabuleiro</param>
        public Piece GetPiece(PositionBoard position)
        {
            return pecas[position.Linha, position.Coluna];
        }

        public void ColocarPeca(Piece p, PositionBoard pos)
        {
            if (PecaValida(pos))
            {
                throw new ExceptionBoard("Já existe uma peça nessa posição.");
            }
            pecas[pos.Linha, pos.Coluna] = p;
            p.Position = pos;
        }

        public Piece TirarPeca(PositionBoard pos)
        {
            if (GetPiece(pos) == null)
            {
                return null;
            }

            Piece pecaMorta = pecas[pos.Linha, pos.Coluna];
            pecaMorta.Position = null;
            pecas[pos.Linha, pos.Coluna] = null;
            return pecaMorta;
        }

        public bool PecaValida(PositionBoard pos)
        {
            ValidarPosicao(pos);
            return GetPiece(pos) != null;
        }

        public void ValidarPosicao(PositionBoard pos)
        {
            if (PosicaoValida(pos) == false)
            {
                throw new ExceptionBoard("Posição Inválida!");
            }
        }

        public bool PosicaoValida(PositionBoard pos)
        {
            if (pos.Linha < 0 || pos.Coluna < 0 || pos.Linha >= Linhas || pos.Coluna >= Colunas)
            {
                return false;
            }
            return true;
        }

    
    }
}
