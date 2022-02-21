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

        public PartidaDeXadrez()
        {
            this.tab = new Tabuleiro(8,8);
            this.jogadorAtual = Cor.Branca;
            this.turno = 1;
            jogoFinalizado = false;
            this.pecas = new HashSet<Peca>();
            this.capturadas = new HashSet<Peca>();
            ColocarPecasIniciais();
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

        }

        public void RealizarJogada(Posicao posInicial, Posicao posFinal)
        {
            Peca pecaMorta = ExecutarMovimento(posInicial, posFinal);
            if (EstaEmXeque(jogadorAtual))
            {
                DesfazerMovimento(posInicial, posFinal, pecaMorta);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
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
            ColocarNovaPeca('e', 1, new Rei(Cor.Branca, tab));
            ColocarNovaPeca('d', 1, new Dama(Cor.Branca, tab));
            ColocarNovaPeca('b', 1, new Cavalo(Cor.Branca, tab));
            ColocarNovaPeca('g', 1, new Cavalo(Cor.Branca, tab));
            ColocarNovaPeca('f', 1, new Bispo(Cor.Branca, tab));
            ColocarNovaPeca('c', 1, new Bispo(Cor.Branca, tab));
            ColocarNovaPeca('a', 2, new Piao(Cor.Branca, tab));
            ColocarNovaPeca('h', 2, new Piao(Cor.Branca, tab));
            ColocarNovaPeca('e', 2, new Piao(Cor.Branca, tab));
            ColocarNovaPeca('d', 2, new Piao(Cor.Branca, tab));
            ColocarNovaPeca('b', 2, new Piao(Cor.Branca, tab));
            ColocarNovaPeca('g', 2, new Piao(Cor.Branca, tab));
            ColocarNovaPeca('f', 2, new Piao(Cor.Branca, tab));
            ColocarNovaPeca('c', 2, new Piao(Cor.Branca, tab));


            ColocarNovaPeca('a', 8, new Torre(Cor.Preta, tab));
            ColocarNovaPeca('h', 8, new Torre(Cor.Preta, tab));
            ColocarNovaPeca('d', 8, new Rei(Cor.Preta, tab));
            ColocarNovaPeca('e', 8, new Dama(Cor.Preta, tab));
            ColocarNovaPeca('b', 8, new Cavalo(Cor.Preta, tab));
            ColocarNovaPeca('g', 8, new Cavalo(Cor.Preta, tab));
            ColocarNovaPeca('f', 8, new Bispo(Cor.Preta, tab));
            ColocarNovaPeca('c', 8, new Bispo(Cor.Preta, tab));
            ColocarNovaPeca('a', 7, new Piao(Cor.Preta, tab));
            ColocarNovaPeca('h', 7, new Piao(Cor.Preta, tab));
            ColocarNovaPeca('e', 7, new Piao(Cor.Preta, tab));
            ColocarNovaPeca('d', 7, new Piao(Cor.Preta, tab));
            ColocarNovaPeca('b', 7, new Piao(Cor.Preta, tab));
            ColocarNovaPeca('g', 7, new Piao(Cor.Preta, tab));
            ColocarNovaPeca('f', 7, new Piao(Cor.Preta, tab));
            ColocarNovaPeca('c', 7, new Piao(Cor.Preta, tab));

        }

    }
}
