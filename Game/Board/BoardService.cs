using ConsoleChessGame.Board.Exception;
using ConsoleChessGame.Game.Pieces.Abstract;

namespace tabuleiro
{
    class BoardService
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }
        private Piece[,] Pieces { get; set; }

        public BoardService(int linhas, int colunas)
        {
            Linhas = linhas;
            Colunas = colunas;
            Pieces = new Piece[linhas, colunas];
        }

        /// <summary>
        /// Retorna a peça posicionada nas cordenadas informadas pelos argumentos
        /// </summary>
        /// <param name="row">Linha do tabuleiro</param>
        /// <param name="column">Coluna do tabuleiro</param>
        public Piece GetPiece(int row, int column)
        {
            return Pieces[row, column];
        }

        /// <summary>
        /// Retorna a peça posicionada nas cordenadas informadas pelos argumentos
        /// </summary>
        /// <param name="row">Linha do tabuleiro</param>
        /// <param name="column">Coluna do tabuleiro</param>
        public Piece GetPiece(PositionOnBoard position)
        {
            return Pieces[position.Linha, position.Coluna];
        }

        /// <summary>
        /// Coloca a peça informada na posição informada
        /// </summary>
        /// <param name="piece">Peça que deseja colocar no tabuleiro</param>
        /// <param name="destinyPosition">Posição que deseja colocar a peça</param>
        public void PutPiece(Piece piece, PositionOnBoard destinyPosition)
        {
            if (IsValidPiece(destinyPosition))
                throw new ExceptionBoard("Já existe uma peça nessa posição.");

            // Colocando a peça na matriz do tabuleiro
            Pieces[destinyPosition.Linha, destinyPosition.Coluna] = piece;
            piece.Position = destinyPosition;
        }

        /// <summary>
        /// Retira a peça referente a posição informada do tabuleiro
        /// </summary>
        /// <param name="position">Posição da peça que deve ser retirada</param>
        /// <returns>Uma instância de <see langword="Piece"/> da peça retirada</returns>
        public Piece RemovePiece(PositionOnBoard position)
        {
            Piece pieceRemoved = GetPiece(position);
            if (GetPiece(position) is null)
                return null;

            pieceRemoved.Position = null;

            Pieces[position.Linha, position.Coluna] = null;
            return pieceRemoved;
        }

        /// <summary>
        /// Verifica se a peça referente a posição informada é válida
        /// </summary>
        /// <param name="position"></param>
        /// <returns>Boleano (<see langword="true"/> caso a peça seja válida ou <see langword="false"/> caso não seja)</returns>
        public bool IsValidPiece(PositionOnBoard position)
        {
            ValidatePosition(position);

            return GetPiece(position) is not null;
        }

        /// <summary>
        /// Valida se a posição informada é valida
        /// </summary>
        /// <param name="position">Posição a ser validada</param>
        public void ValidatePosition(PositionOnBoard position)
        {
            if (!IsValidPosition(position))
                throw new ExceptionBoard("Posição Inválida!");
        }

        /// <summary>
        /// Verifica se a posição informada está dentro do tabuleiro
        /// </summary>
        /// <param name="position">Posição a ser validada</param>
        /// <returns>Boleano (<see langword="true"/> caso a posição seja válida ou <see langword="false"/> caso não seja)</returns>
        public bool IsValidPosition(PositionOnBoard position)
        {
            bool invalidPosition =
                position.Linha < 0 || // Linha é menor que a capacidade da matriz do tabuleiro
                position.Coluna < 0 || // Coluna é menor que a capacidade da matriz do tabuleiro
                position.Linha >= Linhas || // Linha é maior que a capacidade da matriz do tabuleiro
                position.Coluna >= Colunas; // Coluna é maior que a capacidade da matriz do tabuleiro

            return !invalidPosition;
        }
    }
}
