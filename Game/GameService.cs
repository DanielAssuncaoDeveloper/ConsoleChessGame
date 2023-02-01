using System;
using System.Collections.Generic;
using System.Linq;
using tabuleiro;
using Xadrez_Console.Game.Pieces;

namespace Xadrez_Console.Game
{
    class GameService
    {
        public Board tab { get; private set; }
        public Color jogadorAtual { get; private set; }
        public int turno { get; private set; }
        public bool jogoFinalizado { get; set; }
        public bool xeque { get; set; }
        private HashSet<Piece> pecas;
        private HashSet<Piece> capturadas;
        public Piece vulneravelEnPassant;

        public GameService()
        {
            tab = new Board(8, 8);
            jogadorAtual = Color.Branca;
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

        public void ValidarPosicaoOrigem(Position pos)
        {
            if (tab.FindPeca(pos) == null)
            {
                throw new ExceptionBoard("Não existe peça na posição de origem escolhida!");
            }
            if (tab.FindPeca(pos).cor != jogadorAtual)
            {
                throw new ExceptionBoard("A peça de origem escolhida não é sua!");
            }
            if (!tab.FindPeca(pos).ExisteMovimentosPossiveis())
            {
                throw new ExceptionBoard("Não existe movimentos possíveis para a peça de origem escolhida!");
            }
        }

        private Color Adversaria(Color cor)
        {
            if (cor == Color.Branca)
            {
                return Color.Preta;
            }
            return Color.Branca;
        }

        public void ValidarPosicaoDestino(Position origem, Position destino)
        {
            if (!tab.FindPeca(origem).PodeMoverPara(destino))
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
                .Where(p => p.cor.Equals(color))
                .ToHashSet();
            
        public HashSet<Piece> PecasEmJogo(Color cor)
        {
            HashSet<Piece> aux = new HashSet<Piece>();
            foreach (Piece p in pecas)
            {
                if (p.cor == cor)
                {
                    aux.Add(p);
                }
            }
            aux.ExceptWith(CapturedPieces(cor));
            return aux;
        }

        public Piece ExecutarMovimento(Position posInicial, Position posFinal)
        {
            Piece pecaMovimentada = tab.TirarPeca(posInicial);
            pecaMovimentada.IncrementarMovimentos();
            Piece pecaMorta = tab.TirarPeca(posFinal);
            tab.ColocarPeca(pecaMovimentada, posFinal);
            if (pecaMorta != null)
            {
                capturadas.Add(pecaMorta);
            }

            // Jogada Especial: Roque Pequeno
            if (pecaMovimentada is King && posFinal.Coluna == posInicial.Coluna + 2)
            {
                Position posTorre = new Position(posInicial.Linha, posInicial.Coluna + 3);
                Position destinoTorre = new Position(posInicial.Linha, posInicial.Coluna + 1);
                Piece torre = tab.TirarPeca(posTorre);
                torre.IncrementarMovimentos();
                tab.ColocarPeca(torre, destinoTorre);
            }
            // Jogada Especial: Roque Grande
            if (pecaMovimentada is King && posFinal.Coluna == posInicial.Coluna - 2)
            {
                Position posTorre = new Position(posInicial.Linha, posInicial.Coluna - 4);
                Position destinoTorre = new Position(posInicial.Linha, posInicial.Coluna - 1);
                Piece torre = tab.TirarPeca(posTorre);
                torre.IncrementarMovimentos();
                tab.ColocarPeca(torre, destinoTorre);
            }

            // Jogada Especial: En Passant
            if (pecaMovimentada is Pawn)
            {
                if (posFinal.Coluna != posInicial.Coluna &&
                    pecaMorta == null)
                {
                    Position posP;
                    if (pecaMovimentada.cor == Color.Branca)
                    {
                        posP = new Position(posFinal.Linha + 1, posFinal.Coluna);
                    }
                    else
                    {
                        posP = new Position(posFinal.Linha - 1, posFinal.Coluna);
                    }
                    pecaMorta = tab.TirarPeca(posP);
                    capturadas.Add(pecaMorta);
                }
            }

            return pecaMorta;
        }

        public void DesfazerMovimento(Position posInicial, Position posFinal, Piece pecaCapturada)
        {
            Piece p = tab.TirarPeca(posFinal);
            p.DecrementarMovimentos();
            if (pecaCapturada != null)
            {
                tab.ColocarPeca(pecaCapturada, posFinal);
                capturadas.Remove(p);
            }
            tab.ColocarPeca(p, posInicial);

            // Jogada Especial: Roque Pequeno
            if (p is King && posInicial.Coluna + 2 == posFinal.Coluna)
            {
                Position posTorre = new Position(posInicial.Linha, posInicial.Coluna + 3);
                Position destinoTorre = new Position(posInicial.Linha, posInicial.Coluna + 1);
                Piece torre = tab.TirarPeca(destinoTorre);
                torre.IncrementarMovimentos();
                tab.ColocarPeca(torre, posTorre);
            }

            // Jogada Especial: Roque Grande
            if (p is King && posInicial.Coluna - 2 == posFinal.Coluna)
            {
                Position posTorre = new Position(posInicial.Linha, posInicial.Coluna - 4);
                Position destinoTorre = new Position(posInicial.Linha, posInicial.Coluna - 1);
                Piece torre = tab.TirarPeca(destinoTorre);
                torre.IncrementarMovimentos();
                tab.ColocarPeca(torre, posTorre);
            }


            // Jogada Especial: En Passant
            if (p is Pawn)
            {
                if (posFinal.Coluna != posInicial.Coluna &&
                    pecaCapturada == vulneravelEnPassant)
                {
                    Piece peao = tab.TirarPeca(posFinal);
                    Position posP;
                    if (p.cor == Color.Branca)
                    {
                        posP = new Position(3, posFinal.Coluna);
                    }
                    else
                    {
                        posP = new Position(4, posFinal.Coluna);
                    }
                    tab.ColocarPeca(peao, posP);
                }
            }

        }

        public void RealizarJogada(Position posInicial, Position posFinal)
        {
            Piece pecaMorta = ExecutarMovimento(posInicial, posFinal);
            if (EstaEmXeque(jogadorAtual))
            {
                DesfazerMovimento(posInicial, posFinal, pecaMorta);
                throw new ExceptionBoard("Você não pode se colocar em xeque!");
            }
            Piece possivelEnPassant = tab.FindPeca(posFinal);
            // Jogada Especial: Promoção
            if (possivelEnPassant is Pawn)
            {
                if (possivelEnPassant.cor == Color.Branca && posFinal.Linha == 0 ||
                    possivelEnPassant.cor == Color.Preta && posFinal.Linha == 7)
                {
                    possivelEnPassant = tab.TirarPeca(posFinal);
                    pecas.Remove(possivelEnPassant);
                    Piece dama = new Queen(possivelEnPassant.cor, tab);
                    tab.ColocarPeca(dama, posFinal);
                    pecas.Add(dama);
                }
            }



            if (EstaEmXeque(Adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if (TesteXequeMate(Adversaria(jogadorAtual)))
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

            Position posicaoRei = FindRei(cor).posicao;
            foreach (Piece p in PecasEmJogo(Adversaria(cor)))
            {
                if (p.MovimentosValidos()[posicaoRei.Linha, posicaoRei.Coluna])
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
                bool[,] movPossiveis = p.MovimentosValidos();
                for (int i = 0; i < tab.Linhas; i++)
                {
                    for (int j = 0; j < tab.Colunas; j++)
                    {
                        if (movPossiveis[i, j])
                        {
                            Position destino = new Position(i, j);
                            Position origem = p.posicao;

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
            if (jogadorAtual == Color.Branca)
            {
                jogadorAtual = Color.Preta;
            }
            else
            {
                jogadorAtual = Color.Branca;
            }
        }

        public void ColocarNovaPeca(char coluna, int linha, Piece peca)
        {
            tab.ColocarPeca(peca, new PositionChess(coluna, linha).ToPosicao());
            pecas.Add(peca);
        }

        private void ColocarPecasIniciais()
        {
            ColocarNovaPeca('a', 1, new Rook(Color.Branca, tab));
            ColocarNovaPeca('h', 1, new Rook(Color.Branca, tab));
            ColocarNovaPeca('e', 1, new King(Color.Branca, tab, this));
            ColocarNovaPeca('d', 1, new Queen(Color.Branca, tab));
            ColocarNovaPeca('b', 1, new Horse(Color.Branca, tab));
            ColocarNovaPeca('g', 1, new Horse(Color.Branca, tab));
            ColocarNovaPeca('f', 1, new Bishop(Color.Branca, tab));
            ColocarNovaPeca('c', 1, new Bishop(Color.Branca, tab));
            ColocarNovaPeca('a', 2, new Pawn(Color.Branca, tab, this));
            ColocarNovaPeca('h', 2, new Pawn(Color.Branca, tab, this));
            ColocarNovaPeca('e', 2, new Pawn(Color.Branca, tab, this));
            ColocarNovaPeca('d', 2, new Pawn(Color.Branca, tab, this));
            ColocarNovaPeca('b', 2, new Pawn(Color.Branca, tab, this));
            ColocarNovaPeca('g', 2, new Pawn(Color.Branca, tab, this));
            ColocarNovaPeca('f', 2, new Pawn(Color.Branca, tab, this));
            ColocarNovaPeca('c', 2, new Pawn(Color.Branca, tab, this));


            ColocarNovaPeca('a', 8, new Rook(Color.Preta, tab));
            ColocarNovaPeca('h', 8, new Rook(Color.Preta, tab));
            ColocarNovaPeca('e', 8, new King(Color.Preta, tab, this));
            ColocarNovaPeca('d', 8, new Queen(Color.Preta, tab));
            ColocarNovaPeca('b', 8, new Horse(Color.Preta, tab));
            ColocarNovaPeca('g', 8, new Horse(Color.Preta, tab));
            ColocarNovaPeca('f', 8, new Bishop(Color.Preta, tab));
            ColocarNovaPeca('c', 8, new Bishop(Color.Preta, tab));
            ColocarNovaPeca('a', 7, new Pawn(Color.Preta, tab, this));
            ColocarNovaPeca('h', 7, new Pawn(Color.Preta, tab, this));
            ColocarNovaPeca('e', 7, new Pawn(Color.Preta, tab, this));
            ColocarNovaPeca('d', 7, new Pawn(Color.Preta, tab, this));
            ColocarNovaPeca('b', 7, new Pawn(Color.Preta, tab, this));
            ColocarNovaPeca('g', 7, new Pawn(Color.Preta, tab, this));
            ColocarNovaPeca('f', 7, new Pawn(Color.Preta, tab, this));
            ColocarNovaPeca('c', 7, new Pawn(Color.Preta, tab, this));

        }

    }
}
