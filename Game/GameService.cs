using System;
using System.Collections.Generic;
using System.Linq;
using tabuleiro;
using Xadrez_Console.Board.Exception;
using Xadrez_Console.Game.Enum;
using Xadrez_Console.Game.Pieces;
using Xadrez_Console.Game.Pieces.Abstract;

namespace Xadrez_Console.Game
{
    class GameService
    {
        public tabuleiro.BoardService Board { get; private set; }
        public Color CurrentPlayerColor { get; private set; }
        public int turno { get; private set; }
        public bool jogoFinalizado { get; set; }
        public bool xeque { get; set; }
        private HashSet<Piece> pecas;
        private HashSet<Piece> capturadas;
        public Piece vulneravelEnPassant;

        public GameService()
        {
            Board = new tabuleiro.BoardService(8, 8);
            CurrentPlayerColor = Color.White;
            turno = 1;
            jogoFinalizado = false;
            pecas = new HashSet<Piece>();
            capturadas = new HashSet<Piece>();
            ColocarPecasIniciais();
            vulneravelEnPassant = null;
        }

        public void IncrementarMovimentos()
        {
            turno++;
        }

        /// <summary>
        /// Verifica se a posição de origem informada pelo jogador é válida
        /// </summary>
        /// <param name="position">Posição no tabuleiro informada pelo jogador</param>
        public void ValidateOriginPosition(PositionOnBoard position)
        {
            if (Board.GetPiece(position) is null)
                throw new ExceptionBoard("Não existe peça na posição de origem escolhida!");

            if (Board.GetPiece(position).Color != CurrentPlayerColor)
                throw new ExceptionBoard("A peça de origem escolhida não é sua!");

            if (!Board.GetPiece(position).ExistsPossibleMoves()) 
                throw new ExceptionBoard("Não existe movimentos possíveis para a peça de origem escolhida!");
        }

        private Color Adversaria(Color cor)
        {
            if (cor == Color.White)
            {
                return Color.Red;
            }
            return Color.White;
        }

        public void ValidarPosicaoDestino(PositionOnBoard origem, PositionOnBoard destino)
        {
            if (!Board.GetPiece(origem).PodeMoverPara(destino))
            {
                throw new ExceptionBoard("Posição de Destino invalida");
            }
        }

        /// <summary>
        /// Obtem as peças capturadas na partida de uma determinada cor
        /// </summary>
        /// <param name="color">Cor das peças a serem obtidas</param>
        public HashSet<Piece> CapturedPieces(Color color) =>
            capturadas
                .Where(p => p.Color.Equals(color))
                .ToHashSet();
            
        public HashSet<Piece> PecasEmJogo(Color cor)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece p in pecas)
            {
                if (p.Color == cor)
                {
                    aux.Add(p);
                }
            }
            aux.ExceptWith(CapturedPieces(cor));
            return aux;
        }

        public Piece ExecutarMovimento(PositionOnBoard posInicial, PositionOnBoard posFinal)
        {
            Piece pecaMovimentada = Board.TirarPeca(posInicial);
            pecaMovimentada.IncrementarMovimentos();
            Piece pecaMorta = Board.TirarPeca(posFinal);
            Board.ColocarPeca(pecaMovimentada, posFinal);
            if (pecaMorta != null)
            {
                capturadas.Add(pecaMorta);
            }

            // Jogada Especial: Roque Pequeno
            if (pecaMovimentada is King && posFinal.Coluna == posInicial.Coluna + 2)
            {
                PositionOnBoard posTorre = new PositionOnBoard(posInicial.Linha, posInicial.Coluna + 3);
                PositionOnBoard destinoTorre = new PositionOnBoard(posInicial.Linha, posInicial.Coluna + 1);
                Piece torre = Board.TirarPeca(posTorre);
                torre.IncrementarMovimentos();
                Board.ColocarPeca(torre, destinoTorre);
            }
            // Jogada Especial: Roque Grande
            if (pecaMovimentada is King && posFinal.Coluna == posInicial.Coluna - 2)
            {
                PositionOnBoard posTorre = new PositionOnBoard(posInicial.Linha, posInicial.Coluna - 4);
                PositionOnBoard destinoTorre = new PositionOnBoard(posInicial.Linha, posInicial.Coluna - 1);
                Piece torre = Board.TirarPeca(posTorre);
                torre.IncrementarMovimentos();
                Board.ColocarPeca(torre, destinoTorre);
            }

            // Jogada Especial: En Passant
            if (pecaMovimentada is Pawn)
            {
                if (posFinal.Coluna != posInicial.Coluna &&
                    pecaMorta == null)
                {
                    PositionOnBoard posP;
                    if (pecaMovimentada.Color == Color.White)
                    {
                        posP = new PositionOnBoard(posFinal.Linha + 1, posFinal.Coluna);
                    }
                    else
                    {
                        posP = new PositionOnBoard(posFinal.Linha - 1, posFinal.Coluna);
                    }
                    pecaMorta = Board.TirarPeca(posP);
                    capturadas.Add(pecaMorta);
                }
            }

            return pecaMorta;
        }

        public void DesfazerMovimento(PositionOnBoard posInicial, PositionOnBoard posFinal, Piece pecaCapturada)
        {
            Piece p = Board.TirarPeca(posFinal);
            p.DecrementarMovimentos();
            if (pecaCapturada != null)
            {
                Board.ColocarPeca(pecaCapturada, posFinal);
                capturadas.Remove(p);
            }
            Board.ColocarPeca(p, posInicial);

            // Jogada Especial: Roque Pequeno
            if (p is King && posInicial.Coluna + 2 == posFinal.Coluna)
            {
                PositionOnBoard posTorre = new PositionOnBoard(posInicial.Linha, posInicial.Coluna + 3);
                PositionOnBoard destinoTorre = new PositionOnBoard(posInicial.Linha, posInicial.Coluna + 1);
                Piece torre = Board.TirarPeca(destinoTorre);
                torre.IncrementarMovimentos();
                Board.ColocarPeca(torre, posTorre);
            }

            // Jogada Especial: Roque Grande
            if (p is King && posInicial.Coluna - 2 == posFinal.Coluna)
            {
                PositionOnBoard posTorre = new PositionOnBoard(posInicial.Linha, posInicial.Coluna - 4);
                PositionOnBoard destinoTorre = new PositionOnBoard(posInicial.Linha, posInicial.Coluna - 1);
                Piece torre = Board.TirarPeca(destinoTorre);
                torre.IncrementarMovimentos();
                Board.ColocarPeca(torre, posTorre);
            }


            // Jogada Especial: En Passant
            if (p is Pawn)
            {
                if (posFinal.Coluna != posInicial.Coluna &&
                    pecaCapturada == vulneravelEnPassant)
                {
                    Piece peao = Board.TirarPeca(posFinal);
                    PositionOnBoard posP;
                    if (p.Color == Color.White)
                    {
                        posP = new PositionOnBoard(3, posFinal.Coluna);
                    }
                    else
                    {
                        posP = new PositionOnBoard(4, posFinal.Coluna);
                    }
                    Board.ColocarPeca(peao, posP);
                }
            }

        }

        public void RealizarJogada(PositionOnBoard posInicial, PositionOnBoard posFinal)
        {
            Piece pecaMorta = ExecutarMovimento(posInicial, posFinal);
            if (EstaEmXeque(CurrentPlayerColor))
            {
                DesfazerMovimento(posInicial, posFinal, pecaMorta);
                throw new ExceptionBoard("Você não pode se colocar em xeque!");
            }
            Piece possivelEnPassant = Board.GetPiece(posFinal);
            // Jogada Especial: Promoção
            if (possivelEnPassant is Pawn)
            {
                if (possivelEnPassant.Color == Color.White && posFinal.Linha == 0 ||
                    possivelEnPassant.Color == Color.Red && posFinal.Linha == 7)
                {
                    possivelEnPassant = Board.TirarPeca(posFinal);
                    pecas.Remove(possivelEnPassant);
                    Piece dama = new Queen(possivelEnPassant.Color, Board);
                    Board.ColocarPeca(dama, posFinal);
                    pecas.Add(dama);
                }
            }



            if (EstaEmXeque(Adversaria(CurrentPlayerColor)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if (TesteXequeMate(Adversaria(CurrentPlayerColor)))
            {
                jogoFinalizado = true;
            }
            else
            {
                IncrementarMovimentos();
                MudarJogador();
            }

            // Jogada Especial: En Passant
            if (possivelEnPassant is Pawn &&
                (posFinal.Linha - 2 == posInicial.Linha ||
                 posFinal.Linha + 2 == posInicial.Linha))
            {
                vulneravelEnPassant = possivelEnPassant;
            }
            else
            {
                vulneravelEnPassant = null;
            }
        }

        public bool EstaEmXeque(Color cor)
        {

            PositionOnBoard posicaoRei = FindRei(cor).Position;
            foreach (Piece p in PecasEmJogo(Adversaria(cor)))
            {
                if (p.GetValidMoves()[posicaoRei.Linha, posicaoRei.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequeMate(Color cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }

            foreach (Piece p in PecasEmJogo(cor))
            {
                bool[,] movPossiveis = p.GetValidMoves();
                for (int i = 0; i < Board.Linhas; i++)
                {
                    for (int j = 0; j < Board.Colunas; j++)
                    {
                        if (movPossiveis[i, j])
                        {
                            PositionOnBoard destino = new PositionOnBoard(i, j);
                            PositionOnBoard origem = p.Position;

                            Piece pecaCapturada = ExecutarMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazerMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }


        private Piece FindRei(Color cor)
        {
            foreach (Piece p in PecasEmJogo(cor))
            {
                if (p is King)
                {
                    return p;
                }
            }
            return null;
        }

        public void MudarJogador()
        {
            if (CurrentPlayerColor == Color.White)
            {
                CurrentPlayerColor = Color.Red;
            }
            else
            {
                CurrentPlayerColor = Color.White;
            }
        }

        public void ColocarNovaPeca(char coluna, int linha, Piece peca)
        {
            Board.ColocarPeca(peca, new PositionOnGame(coluna, linha).ConvertToPositionOnBoard());
            pecas.Add(peca);
        }

        private void ColocarPecasIniciais()
        {
            ColocarNovaPeca('a', 1, new Rook(Color.White, Board));
            ColocarNovaPeca('h', 1, new Rook(Color.White, Board));
            ColocarNovaPeca('e', 1, new King(Color.White, Board, this));
            ColocarNovaPeca('d', 1, new Queen(Color.White, Board));
            ColocarNovaPeca('b', 1, new Horse(Color.White, Board));
            ColocarNovaPeca('g', 1, new Horse(Color.White, Board));
            ColocarNovaPeca('f', 1, new Bishop(Color.White, Board));
            ColocarNovaPeca('c', 1, new Bishop(Color.White, Board));
            ColocarNovaPeca('a', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('h', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('e', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('d', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('b', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('g', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('f', 2, new Pawn(Color.White, Board, this));
            ColocarNovaPeca('c', 2, new Pawn(Color.White, Board, this));


            ColocarNovaPeca('a', 8, new Rook(Color.Red, Board));
            ColocarNovaPeca('h', 8, new Rook(Color.Red, Board));
            ColocarNovaPeca('e', 8, new King(Color.Red, Board, this));
            ColocarNovaPeca('d', 8, new Queen(Color.Red, Board));
            ColocarNovaPeca('b', 8, new Horse(Color.Red, Board));
            ColocarNovaPeca('g', 8, new Horse(Color.Red, Board));
            ColocarNovaPeca('f', 8, new Bishop(Color.Red, Board));
            ColocarNovaPeca('c', 8, new Bishop(Color.Red, Board));
            ColocarNovaPeca('a', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('h', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('e', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('d', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('b', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('g', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('f', 7, new Pawn(Color.Red, Board, this));
            ColocarNovaPeca('c', 7, new Pawn(Color.Red, Board, this));

        }

    }
}
