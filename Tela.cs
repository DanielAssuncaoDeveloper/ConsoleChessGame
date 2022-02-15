using System;
using tabuleiro;
using xadrez;

namespace Xadrez_Console
{
    class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write($" {8 - i}   ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    if (tab.FindPeca(i, j) != null)
                    {
                        ImprimirPeca(tab.FindPeca(i, j));
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("- ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("     a b c d e f g h");
        }

        public static PosicaoXadrez LerPosicao()
        {
            string pos = Console.ReadLine();
            char coluna = pos[0];
            int linha = int.Parse(pos[1].ToString());
            return new PosicaoXadrez(coluna, linha);
        }
        public static void ImprimirPeca(Peca peca)
        {
            if (peca.cor == Cor.Branca)
            {
                Console.Write(peca);
            }
            else
            {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write(peca);
                Console.ForegroundColor = aux;
            }
        } 

    }
}
