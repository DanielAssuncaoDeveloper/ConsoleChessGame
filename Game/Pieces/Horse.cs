using System;
using System.Collections.Generic;

using tabuleiro;

namespace Xadrez_Console.Game.Pieces
{
    class Horse : Piece
    {
        public Horse(Color cor, Board tab)
            : base(cor, tab) { }


        public bool MovimentoPossivel(Position pos)
        {
            Piece p = tab.FindPeca(pos);
            return p == null || p.cor != cor;
        }

        public override bool[,] MovimentosValidos()
        {
            bool[,] movPosiveis = new bool[tab.Linhas, tab.Colunas];

            Position pos = new Position(0, 0);

            // N + O
            pos.DefinirValores(posicao.Linha - 2, posicao.Coluna - 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // N + E
            pos.DefinirValores(posicao.Linha - 2, posicao.Coluna + 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // E + N
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna + 2);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // E + S
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna + 2);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // S + E
            pos.DefinirValores(posicao.Linha + 2, posicao.Coluna + 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // S + O
            pos.DefinirValores(posicao.Linha + 2, posicao.Coluna - 1);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // O + S
            pos.DefinirValores(posicao.Linha + 1, posicao.Coluna - 2);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // O + N
            pos.DefinirValores(posicao.Linha - 1, posicao.Coluna - 2);
            if (tab.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            return movPosiveis;
        }


        public override string ToString()
        {
            return "C";
        }
    }
}
