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
                while (partida.Terminada != true)
                {
                    try
                    {
                        Console.Clear();

                        Tela.ImprimirPartida(partida);

                        Console.WriteLine();

                        Console.Write("Origem: ");
                        Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                        partida.ValidarPosicaoDeOrigem(origem);

                        bool[,] posicoesPossiveis = partida.tab.Peca(origem).MovimentosPossiveis();

                        Console.Clear(); // Limpo a tela para imprimir as possíveis posições
                        Tela.ImprimirTabuleiro(partida.tab, posicoesPossiveis);

                        Console.WriteLine();
                        Console.Write("Destino: ");
                        Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
                        partida.ValidarPosicaoDeDestino(origem, destino);

                        partida.RealizaJogada(origem, destino);
                    }catch(TabuleiroException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.ReadLine();
                    }
                    

                }

                Console.Clear();
                Tela.ImprimirPartida(partida);
                

                
            }
            catch(TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine("Erro inesperado: " + e.Message);
            }
        }
    }
}