using tabuleiro;
using ConsoleChessGame.Game.Enum;
using ConsoleChessGame.Game.Pieces.Abstract;

namespace ConsoleChessGame.Game.Pieces
{
    class King : Piece
    {
        private GameService partida;

        public King(Color cor, tabuleiro.BoardService tab, GameService partida)
            : base(cor, tab)
        {
            this.partida = partida;
        }

        public override string ToString()
        {
            return "R";
        }

        public bool MovimentoPossivel(PositionOnBoard pos)
        {
            Piece p = Board.GetPiece(pos);
            return p == null || p.Color != Color;
        }

        public override bool[,] GetValidMoves()
        {
            bool[,] movPosiveis = new bool[Board.Linhas, Board.Colunas];

            PositionOnBoard pos = new PositionOnBoard(0, 0);

            // norte
            pos.DefinirValores(Position.Linha - 1, Position.Coluna);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // nordeste
            pos.DefinirValores(Position.Linha - 1, Position.Coluna + 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // oeste
            pos.DefinirValores(Position.Linha, Position.Coluna + 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // sudeste
            pos.DefinirValores(Position.Linha + 1, Position.Coluna + 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // sul 
            pos.DefinirValores(Position.Linha + 1, Position.Coluna);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // suldoeste
            pos.DefinirValores(Position.Linha + 1, Position.Coluna - 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // leste
            pos.DefinirValores(Position.Linha, Position.Coluna - 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // noroeste
            pos.DefinirValores(Position.Linha - 1, Position.Coluna - 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
            }


            // Jogada Especial: Roque 
            if (NumberOfMovements == 0 && !partida.IsCheck)
            {
                // Roque pequeno
                PositionOnBoard posTorre1 = new PositionOnBoard(Position.Linha, Position.Coluna + 3);
                if (TesteTorreParaRoque(posTorre1))
                {
                    PositionOnBoard pos1 = new PositionOnBoard(Position.Linha, Position.Coluna + 1);
                    PositionOnBoard pos2 = new PositionOnBoard(Position.Linha, Position.Coluna + 2);
                    if (Board.GetPiece(pos1) == null && Board.GetPiece(pos2) == null)
                    {
                        movPosiveis[Position.Linha, Position.Coluna + 2] = true;
                    }

                }

                // Roque Grande
                PositionOnBoard posTorre2 = new PositionOnBoard(Position.Linha, Position.Coluna - 4);
                if (TesteTorreParaRoque(posTorre1))
                {
                    PositionOnBoard pos1 = new PositionOnBoard(Position.Linha, Position.Coluna - 1);
                    PositionOnBoard pos2 = new PositionOnBoard(Position.Linha, Position.Coluna - 2);
                    PositionOnBoard pos3 = new PositionOnBoard(Position.Linha, Position.Coluna - 3);

                    if (Board.GetPiece(pos1) == null &&
                        Board.GetPiece(pos2) == null &&
                        Board.GetPiece(pos3) == null)
                    {
                        movPosiveis[Position.Linha, Position.Coluna - 2] = true;
                    }

                }


            }



            return movPosiveis;
        }

        private bool TesteTorreParaRoque(PositionOnBoard pos)
        {
            Piece torre = Board.GetPiece(pos);
            return torre != null && torre is Rook && torre.Color == Color && torre.NumberOfMovements == 0;
        }

    }
}
