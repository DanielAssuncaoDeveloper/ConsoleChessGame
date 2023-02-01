using System;
using System.Collections.Generic;

using tabuleiro;

namespace Xadrez_Console.Game.Pieces
{
    class Horse : Piece
    {
        public Horse(Color cor, Board tab)
            : base(cor, tab) { }


        public bool MovimentoPossivel(PositionBoard pos)
        {
            Piece p = Board.GetPiece(pos);
            return p == null || p.Color != Color;
        }

        public override bool[,] MovimentosValidos()
        {
            bool[,] movPosiveis = new bool[Board.Linhas, Board.Colunas];

            PositionBoard pos = new PositionBoard(0, 0);

            // N + O
            pos.DefinirValores(Position.Linha - 2, Position.Coluna - 1);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // N + E
            pos.DefinirValores(Position.Linha - 2, Position.Coluna + 1);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // E + N
            pos.DefinirValores(Position.Linha - 1, Position.Coluna + 2);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // E + S
            pos.DefinirValores(Position.Linha + 1, Position.Coluna + 2);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // S + E
            pos.DefinirValores(Position.Linha + 2, Position.Coluna + 1);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // S + O
            pos.DefinirValores(Position.Linha + 2, Position.Coluna - 1);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // O + S
            pos.DefinirValores(Position.Linha + 1, Position.Coluna - 2);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
            }

            // O + N
            pos.DefinirValores(Position.Linha - 1, Position.Coluna - 2);
            if (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
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
