using tabuleiro;

namespace Xadrez_Console.Game.Pieces
{
    class King : Piece
    {
        private GameService partida;

        public King(Color cor, Board tab, GameService partida)
            : base(cor, tab)
        {
            this.partida = partida;
        }

        public override string ToString()
        {
            return "R";
        }

        public bool MovimentoPossivel(PositionBoard pos)
        {
            Piece p = Board.GetPiece(pos);
            return p == null || p.Color != Color;
        }

        public override bool[,] MovimentosValidos()
        {
            bool[,] movPosiveis = new bool[Board.Linhas, Board.Colunas];

            PositionBoard pos = new PositionBoard(0, 0);

            // norte
            pos.DefinirValores(Position.Linha - 1, Position.Coluna);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // nordeste
            pos.DefinirValores(Position.Linha - 1, Position.Coluna + 1);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // oeste
            pos.DefinirValores(Position.Linha, Position.Coluna + 1);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // sudeste
            pos.DefinirValores(Position.Linha + 1, Position.Coluna + 1);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // sul 
            pos.DefinirValores(Position.Linha + 1, Position.Coluna);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // suldoeste
            pos.DefinirValores(Position.Linha + 1, Position.Coluna - 1);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // leste
            pos.DefinirValores(Position.Linha, Position.Coluna - 1);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // noroeste
            pos.DefinirValores(Position.Linha - 1, Position.Coluna - 1);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
            }


            // Jogada Especial: Roque 
            if (NumberOfMovements == 0 && !partida.xeque)
            {
                // Roque pequeno
                PositionBoard posTorre1 = new PositionBoard(Position.Linha, Position.Coluna + 3);
                if (TesteTorreParaRoque(posTorre1))
                {
                    PositionBoard pos1 = new PositionBoard(Position.Linha, Position.Coluna + 1);
                    PositionBoard pos2 = new PositionBoard(Position.Linha, Position.Coluna + 2);
                    if (Board.GetPiece(pos1) == null && Board.GetPiece(pos2) == null)
                    {
                        movPosiveis[Position.Linha, Position.Coluna + 2] = true;
                    }

                }

                // Roque Grande
                PositionBoard posTorre2 = new PositionBoard(Position.Linha, Position.Coluna - 4);
                if (TesteTorreParaRoque(posTorre1))
                {
                    PositionBoard pos1 = new PositionBoard(Position.Linha, Position.Coluna - 1);
                    PositionBoard pos2 = new PositionBoard(Position.Linha, Position.Coluna - 2);
                    PositionBoard pos3 = new PositionBoard(Position.Linha, Position.Coluna - 3);

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

        private bool TesteTorreParaRoque(PositionBoard pos)
        {
            Piece torre = Board.GetPiece(pos);
            return torre != null && torre is Rook && torre.Color == Color && torre.NumberOfMovements == 0;
        }

    }
}
