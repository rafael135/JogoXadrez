using tabuleiro;
using xadrez;

namespace JogoXadrez
{
    internal class Program
    {
        static void Main(string[] args)
        {
            

            try
            {
                PartidaXadrez partida = new PartidaXadrez();
                while (!partida.Terminada)
                {
                    Console.Clear();

                    Tela.ImprimirTabuleiro(partida.tab);
                    Console.WriteLine();
                    Console.WriteLine("Turno: {0}", partida.Turno);
                    Console.WriteLine("Aguardando jogada: {0}", partida.JogadorAtual);

                    Console.WriteLine();

                    Console.Write("Origem: ");
                    Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();

                    bool[,] posicoesPossiveis = partida.tab.Peca(origem).MovimentosPossiveis();

                    Console.Clear(); // Limpo a tela para imprimir as possíveis posições
                    Tela.ImprimirTabuleiro(partida.tab, posicoesPossiveis);

                    Console.WriteLine();
                    Console.Write("Destino: ");
                    Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();

                    partida.RealizaJogada(origem, destino);

                }
                

                
            }
            catch(TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}