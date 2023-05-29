using tabuleiro;
using ConsoleChessGame.Game.Enum;
using ConsoleChessGame.Game.Pieces.Abstract;

namespace ConsoleChessGame.Game.Pieces
{
    class King : Piece
    {
        public King(Color cor, BoardService tab)
            : base(cor, tab)
        {}

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
            pos.SetValues(Position.Row - 1, Position.Column);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // nordeste
            pos.SetValues(Position.Row - 1, Position.Column + 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // oeste
            pos.SetValues(Position.Row, Position.Column + 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // sudeste
            pos.SetValues(Position.Row + 1, Position.Column + 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // sul 
            pos.SetValues(Position.Row + 1, Position.Column);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // suldoeste
            pos.SetValues(Position.Row + 1, Position.Column - 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // leste
            pos.SetValues(Position.Row, Position.Column - 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
            }

            // noroeste
            pos.SetValues(Position.Row - 1, Position.Column - 1);
            if (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
            }


            // Jogada Especial: Roque 
            if (NumberOfMovements == 0 && !GameService.IsCheck)
            {
                // Roque pequeno
                PositionOnBoard posTorre1 = new PositionOnBoard(Position.Row, Position.Column + 3);
                if (TesteTorreParaRoque(posTorre1))
                {
                    PositionOnBoard pos1 = new PositionOnBoard(Position.Row, Position.Column + 1);
                    PositionOnBoard pos2 = new PositionOnBoard(Position.Row, Position.Column + 2);
                    if (Board.GetPiece(pos1) == null && Board.GetPiece(pos2) == null)
                    {
                        movPosiveis[Position.Row, Position.Column + 2] = true;
                    }

                }

                // Roque Grande
                PositionOnBoard posTorre2 = new PositionOnBoard(Position.Row, Position.Column - 4);
                if (TesteTorreParaRoque(posTorre1))
                {
                    PositionOnBoard pos1 = new PositionOnBoard(Position.Row, Position.Column - 1);
                    PositionOnBoard pos2 = new PositionOnBoard(Position.Row, Position.Column - 2);
                    PositionOnBoard pos3 = new PositionOnBoard(Position.Row, Position.Column - 3);

                    if (Board.GetPiece(pos1) == null &&
                        Board.GetPiece(pos2) == null &&
                        Board.GetPiece(pos3) == null)
                    {
                        movPosiveis[Position.Row, Position.Column - 2] = true;
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
