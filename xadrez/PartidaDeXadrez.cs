using System;
using System.Collections.Generic;

using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; set; }
        private Cor jogadorAtual;
        private int turno;
        public bool jogoFinalizado { get; set; }

        public PartidaDeXadrez()
        {
            this.tab = new Tabuleiro(8,8);
            this.jogadorAtual = Cor.Branca;
            this.turno = 1;
            ColocarPecasIniciais();
            jogoFinalizado = false;
        }

        public void IncrementarMovimentos()
        {
            this.turno++;
        }

        public void ExecutarMovimento(Posicao posInicial, Posicao posFinal)
        {
            //if (tab.FindPeca(posInicial) == null);
            //{
            //    throw new Exception("");
            //}
            //if (tab.FindPeca(posFinal) != null && tab.FindPeca(posFinal).cor == jogadorAtual)
            //{
            //    throw new Exception("");
            //}

            Peca pecaMovimentada = tab.TirarPeca(posInicial);
            Peca pecaMorta = tab.TirarPeca(posFinal);
            tab.ColocarPeca(pecaMovimentada, posFinal);
            IncrementarMovimentos();
        }


        private void ColocarPecasIniciais()
        {
            Peca r = new Rei(Cor.Preta, tab);
            tab.ColocarPeca(r, new PosicaoXadrez('d', 1).ToPosicao());
            //Peca t = new Torre(Cor.Preta, tab);
            //tab.ColocarPeca(t, new Posicao(0, 1));
        }

    }
}
