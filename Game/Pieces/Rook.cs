﻿using tabuleiro;

namespace Xadrez_Console.Game.Pieces
{
    class Rook : Piece
    {
        public Rook(Color cor, Board tab)
            : base(cor, tab) { }

        public override string ToString()
        {
            return "T";
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
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
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
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
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
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
            {
                movPosiveis[pos.Linha, pos.Coluna] = true;
                if (Board.GetPiece(pos) != null && Board.GetPiece(pos).Color != Color)
                {
                    break;
                }
                pos.Linha += 1;

            }

            pos.DefinirValores(Position.Linha, Position.Coluna - 1);
            while (Board.PosicaoValida(pos) && MovimentoPossivel(pos))
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