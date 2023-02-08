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
            pos.DefinirValores(Position.Linha - 1, Position.Coluna);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Linha -= 1;

            }

            // oeste
            pos.DefinirValores(Position.Linha, Position.Coluna + 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Coluna += 1;

            }

            // sul
            pos.DefinirValores(Position.Linha + 1, Position.Coluna);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Linha += 1;

            }

            pos.DefinirValores(Position.Linha, Position.Coluna - 1);
            while (Board.IsValidPosition(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Coluna -= 1;

            }


            return movPosiveis;
        }

    }
}
