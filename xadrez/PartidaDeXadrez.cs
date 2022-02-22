using System;
using System.Collections.Generic;

using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public int turno { get; private set; }
        public bool jogoFinalizado { get; set; }
        public bool xeque { get; set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public Peca vulneravelEnPassant;

        public PartidaDeXadrez()
        {
            this.tab = new Tabuleiro(8,8);
            this.jogadorAtual = Cor.Branca;
            this.turno = 1;
            jogoFinalizado = false;
            this.pecas = new HashSet<Peca>();
            this.capturadas = new HashSet<Peca>();
            ColocarPecasIniciais();
            vulneravelEnPassant = null;
        }

        public void IncrementarMovimentos()
        {
            turno++;
        }

        public void ValidarPosicaoOrigem(Posicao pos)
        {
            if (tab.FindPeca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if (tab.FindPeca(pos).cor != jogadorAtual)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!tab.FindPeca(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não existe movimentos possíveis para a peça de origem escolhida!");
            }
        }

        private Cor Adversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            return Cor.Branca;
        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!tab.FindPeca(origem).PodeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de Destino invalida");
            }
        }


        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca p in capturadas)
            {
                if (p.cor == cor)
                {
                    aux.Add(p);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca p in pecas)
            {
                if (p.cor == cor)
                {
                    aux.Add(p);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        public Peca ExecutarMovimento(Posicao posInicial, Posicao posFinal)
        {
            Peca pecaMovimentada = tab.TirarPeca(posInicial);
            pecaMovimentada.IncrementarMovimentos();
            Peca pecaMorta = tab.TirarPeca(posFinal);
            tab.ColocarPeca(pecaMovimentada, posFinal);
            if (pecaMorta != null)
            {
                capturadas.Add(pecaMorta);
            }

            // Jogada Especial: Roque Pequeno
            if (pecaMovimentada is Rei && posFinal.Coluna == posInicial.Coluna + 2)
            {
                Posicao posTorre = new Posicao(posInicial.Linha, posInicial.Coluna + 3);
                Posicao destinoTorre = new Posicao(posInicial.Linha, posInicial.Coluna + 1);
                Peca torre = tab.TirarPeca(posTorre);
                torre.IncrementarMovimentos();
                tab.ColocarPeca(torre, destinoTorre);
            }
            // Jogada Especial: Roque Grande
            if (pecaMovimentada is Rei && posFinal.Coluna == posInicial.Coluna - 2)
            {
                Posicao posTorre = new Posicao(posInicial.Linha, posInicial.Coluna - 4);
                Posicao destinoTorre = new Posicao(posInicial.Linha, posInicial.Coluna - 1);
                Peca torre = tab.TirarPeca(posTorre);
                torre.IncrementarMovimentos();
                tab.ColocarPeca(torre, destinoTorre);
            }

            // Jogada Especial: En Passant
            if (pecaMovimentada is Piao)
            {
                if (posFinal.Coluna != posInicial.Coluna &&
                    pecaMorta == null)
                {
                    Posicao posP;
                    if (pecaMovimentada.cor == Cor.Branca)
                    {
                        posP = new Posicao(posFinal.Linha + 1, posFinal.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(posFinal.Linha - 1, posFinal.Coluna);
                    }
                    pecaMorta = tab.TirarPeca(posP);
                    capturadas.Add(pecaMorta);
                }
            }

            return pecaMorta;
        }

        public void DesfazerMovimento(Posicao posInicial, Posicao posFinal, Peca pecaCapturada)
        {
            Peca p = tab.TirarPeca(posFinal);
            p.DecrementarMovimentos();
            if (pecaCapturada != null)
            {
                tab.ColocarPeca(pecaCapturada, posFinal);
                capturadas.Remove(p);
            }
            tab.ColocarPeca(p, posInicial);

            // Jogada Especial: Roque Pequeno
            if (p is Rei && posInicial.Coluna + 2 == posFinal.Coluna)
            {
                Posicao posTorre = new Posicao(posInicial.Linha, posInicial.Coluna + 3);
                Posicao destinoTorre = new Posicao(posInicial.Linha, posInicial.Coluna + 1);
                Peca torre = tab.TirarPeca(destinoTorre);
                torre.IncrementarMovimentos();
                tab.ColocarPeca(torre, posTorre);
            }

            // Jogada Especial: Roque Grande
            if (p is Rei && posInicial.Coluna - 2 == posFinal.Coluna)
            {
                Posicao posTorre = new Posicao(posInicial.Linha, posInicial.Coluna - 4);
                Posicao destinoTorre = new Posicao(posInicial.Linha, posInicial.Coluna - 1);
                Peca torre = tab.TirarPeca(destinoTorre);
                torre.IncrementarMovimentos();
                tab.ColocarPeca(torre, posTorre);
            }


            // Jogada Especial: En Passant
            if (p is Piao)
            {
                if (posFinal.Coluna != posInicial.Coluna &&
                    pecaCapturada == vulneravelEnPassant)
                {
                    Peca peao = tab.TirarPeca(posFinal);
                    Posicao posP;
                    if (p.cor == Cor.Branca)
                    {
                        posP = new Posicao(3, posFinal.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, posFinal.Coluna);
                    }
                    tab.ColocarPeca(peao, posP);
                }
            }

        }

        public void RealizarJogada(Posicao posInicial, Posicao posFinal)
        {
            Peca pecaMorta = ExecutarMovimento(posInicial, posFinal);
            if (EstaEmXeque(jogadorAtual))
            {
                DesfazerMovimento(posInicial, posFinal, pecaMorta);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }
            Peca possivelEnPassant = tab.FindPeca(posFinal);
            // Jogada Especial: Promoção
            if (possivelEnPassant is Piao)
            {
                if ((possivelEnPassant.cor == Cor.Branca && posFinal.Linha == 0) ||
                    (possivelEnPassant.cor == Cor.Preta && posFinal.Linha == 7))
                {
                    possivelEnPassant = tab.TirarPeca(posFinal);
                    pecas.Remove(possivelEnPassant);
                    Peca dama = new Dama(possivelEnPassant.cor, tab);
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
            if (possivelEnPassant is Piao && 
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

        public bool EstaEmXeque(Cor cor)
        {

            Posicao posicaoRei = FindRei(cor).posicao;
            foreach (Peca p in PecasEmJogo(Adversaria(cor)))
            {
                if (p.MovimentosValidos()[posicaoRei.Linha, posicaoRei.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequeMate(Cor cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }

            foreach (Peca p in PecasEmJogo(cor))
            {
                bool[,] movPossiveis = p.MovimentosValidos();
                for (int i = 0; i < tab.Linhas; i++)
                {
                    for (int j = 0; j < tab.Colunas; j++)
                    {
                        if (movPossiveis[i,j])
                        {
                            Posicao destino = new Posicao(i, j);
                            Posicao origem = p.posicao;

                            Peca pecaCapturada = ExecutarMovimento(origem, destino);
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


        private Peca FindRei(Cor cor)
        {
            foreach (Peca p in PecasEmJogo(cor))
            {
                if (p is Rei)
                {
                    return p;
                }
            }
            return null;
        }

        public void MudarJogador()
        {
            if (jogadorAtual == Cor.Branca)
            {
                jogadorAtual = Cor.Preta;
            }
            else
            {
                jogadorAtual = Cor.Branca;
            }
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            pecas.Add(peca);
        }

        private void ColocarPecasIniciais()
        {
            ColocarNovaPeca('a', 1, new Torre(Cor.Branca, tab));
            ColocarNovaPeca('h', 1, new Torre(Cor.Branca, tab));
            ColocarNovaPeca('e', 1, new Rei(Cor.Branca, tab, this));
            ColocarNovaPeca('d', 1, new Dama(Cor.Branca, tab));
            ColocarNovaPeca('b', 1, new Cavalo(Cor.Branca, tab));
            ColocarNovaPeca('g', 1, new Cavalo(Cor.Branca, tab));
            ColocarNovaPeca('f', 1, new Bispo(Cor.Branca, tab));
            ColocarNovaPeca('c', 1, new Bispo(Cor.Branca, tab));
            ColocarNovaPeca('a', 2, new Piao(Cor.Branca, tab, this));
            ColocarNovaPeca('h', 2, new Piao(Cor.Branca, tab, this));
            ColocarNovaPeca('e', 2, new Piao(Cor.Branca, tab, this));
            ColocarNovaPeca('d', 2, new Piao(Cor.Branca, tab, this));
            ColocarNovaPeca('b', 2, new Piao(Cor.Branca, tab, this));
            ColocarNovaPeca('g', 2, new Piao(Cor.Branca, tab, this));
            ColocarNovaPeca('f', 2, new Piao(Cor.Branca, tab, this));
            ColocarNovaPeca('c', 2, new Piao(Cor.Branca, tab, this));


            ColocarNovaPeca('a', 8, new Torre(Cor.Preta, tab));
            ColocarNovaPeca('h', 8, new Torre(Cor.Preta, tab));
            ColocarNovaPeca('e', 8, new Rei(Cor.Preta, tab, this));
            ColocarNovaPeca('d', 8, new Dama(Cor.Preta, tab));
            ColocarNovaPeca('b', 8, new Cavalo(Cor.Preta, tab));
            ColocarNovaPeca('g', 8, new Cavalo(Cor.Preta, tab));
            ColocarNovaPeca('f', 8, new Bispo(Cor.Preta, tab));
            ColocarNovaPeca('c', 8, new Bispo(Cor.Preta, tab));
            ColocarNovaPeca('a', 7, new Piao(Cor.Preta, tab, this));
            ColocarNovaPeca('h', 7, new Piao(Cor.Preta, tab, this));
            ColocarNovaPeca('e', 7, new Piao(Cor.Preta, tab, this));
            ColocarNovaPeca('d', 7, new Piao(Cor.Preta, tab, this));
            ColocarNovaPeca('b', 7, new Piao(Cor.Preta, tab, this));
            ColocarNovaPeca('g', 7, new Piao(Cor.Preta, tab, this));
            ColocarNovaPeca('f', 7, new Piao(Cor.Preta, tab, this));
            ColocarNovaPeca('c', 7, new Piao(Cor.Preta, tab, this));

        }

    }
}
