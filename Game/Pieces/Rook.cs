using tabuleiro;
using ConsoleChessGame.Game.Enum;
using ConsoleChessGame.Game.Pieces.Abstract;

namespace ConsoleChessGame.Game.Pieces
{
    class Rook : Piece
    {
        public Rook(Color color, BoardService board) 
            : base(color, board) 
        { }

        public override string ToString()
        {
            return "T";
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
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Row -= 1;

            }

            // oeste
            pos.SetValues(Position.Row, Position.Column + 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Column += 1;

            }

            // sul
            pos.SetValues(Position.Row + 1, Position.Column);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Row += 1;

            }

            pos.SetValues(Position.Row, Position.Column - 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Row, pos.Column] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Column -= 1;

            }


            return movPosiveis;
        }

    }
}
