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

        public bool MovimentoPossivel(Position pos)
        {
            Piece p = tab.FindPeca(pos);
            return p == null || p.cor != cor;
        }

        public override bool[,] MovimentosValidos()
        {
            bool[,] movPosiveis = new bool[tab.Linhas, tab.Colunas];

            Position pos = new Position(0, 0);

            // norte
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // nordeste
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna + 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // oeste
            pos.DefinirValores(posicao.Linha, posicao.Coluna + 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // sudeste
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna + 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // sul 
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // suldoeste
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna - 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // leste
            pos.DefinirValores(posicao.Linha, posicao.Coluna - 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // noroeste
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna - 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
            }


            // Jogada Especial: Roque 
            if (QtdMovimento == 0 && !partida.xeque)
            {
                // Roque pequeno
                Position posTorre1 = new Position(posicao.Linha, posicao.Coluna + 3);
                if (TesteTorreParaRoque(posTorre1))
                {
                    Position pos1 = new Position(posicao.Linha, posicao.Coluna + 1);
                    Position pos2 = new Position(posicao.Linha, posicao.Coluna + 2);
                    if (tab.FindPeca(pos1) == null && tab.FindPeca(pos2) == null)
                    {
                        movPosiveis[posicao.Linha, posicao.Coluna + 2] = true;
                    }

                }

                // Roque Grande
                Position posTorre2 = new Position(posicao.Linha, posicao.Coluna - 4);
                if (TesteTorreParaRoque(posTorre1))
                {
                    Position pos1 = new Position(posicao.Linha, posicao.Coluna - 1);
                    Position pos2 = new Position(posicao.Linha, posicao.Coluna - 2);
                    Position pos3 = new Position(posicao.Linha, posicao.Coluna - 3);

                    if (tab.FindPeca(pos1) == null &&
                        tab.FindPeca(pos2) == null &&
                        tab.FindPeca(pos3) == null)
                    {
                        movPosiveis[posicao.Linha, posicao.Coluna - 2] = true;
                    }

                }


            }



            return movPosiveis;
        }

        private bool TesteTorreParaRoque(Position pos)
        {
            Piece torre = tab.FindPeca(pos);
            return torre != null && torre is Rook && torre.cor == cor && torre.QtdMovimento == 0;
        }

    }
}
