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
            aux.ExceptWith(PecasEmJogo(cor));
            return aux;
        }

        public void ExecutarMovimento(Posicao posInicial, Posicao posFinal)
        {
            Peca pecaMovimentada = tab.TirarPeca(posInicial);
            pecaMovimentada.IncrementarMovimentos();
            Peca pecaMorta = tab.TirarPeca(posFinal);
            tab.ColocarPeca(pecaMovimentada, posFinal);
            if (pecaMorta != null)
            {
                capturadas.Add(pecaMorta);
            }
        }

        public void RealizarJogada(Posicao posInicial, Posicao posFinal)
        {
            ExecutarMovimento(posInicial, posFinal);
            IncrementarMovimentos();
            MudarJogador();
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
            ColocarNovaPeca('c', 4, new Torre(Cor.Branca, tab));
            ColocarNovaPeca('c', 1, new Rei(Cor.Preta, tab));
        }

    }
}
