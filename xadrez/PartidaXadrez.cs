using System;
using tabuleiro;

namespace xadrez
{
    internal class PartidaXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }

        public PartidaXadrez()
        {
            tab = new Tabuleiro(8, 8);
            Turno = 1;
            Terminada = false;
            JogadorAtual = Cor.Branco;
            ColocarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.RetirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = tab.RetirarPeca(destino);
            tab.ColocarPeca(p, destino);
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecutaMovimento(origem, destino);
            Turno++;
            MudaJogador();

        }

        private void MudaJogador()
        {
            if(JogadorAtual == Cor.Branco)
            {
                JogadorAtual = Cor.Preto;
            }
            else
            {
                JogadorAtual = Cor.Branco;
            }
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if(tab.Peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }

            if(JogadorAtual != tab.Peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }

            if (tab.Peca(pos).ExisteMovimentosPossiveis() == false)
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (tab.Peca(origem).PodeMoverPara(destino) == false)
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        private void ColocarPecas()
        {
            tab.ColocarPeca(new Torre(tab, Cor.Branco), new PosicaoXadrez('c', 1).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Branco), new PosicaoXadrez('c', 2).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Branco), new PosicaoXadrez('d', 2).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Branco), new PosicaoXadrez('e', 2).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Branco), new PosicaoXadrez('e', 1).ToPosicao());
            tab.ColocarPeca(new Rei(tab, Cor.Branco), new PosicaoXadrez('d', 1).ToPosicao());

            tab.ColocarPeca(new Torre(tab, Cor.Preto), new PosicaoXadrez('c', 7).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Preto), new PosicaoXadrez('c', 8).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Preto), new PosicaoXadrez('d', 7).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Preto), new PosicaoXadrez('e', 7).ToPosicao());
            tab.ColocarPeca(new Torre(tab, Cor.Preto), new PosicaoXadrez('e', 8).ToPosicao());
            tab.ColocarPeca(new Rei(tab, Cor.Preto), new PosicaoXadrez('d', 8).ToPosicao());

        }
    }
}
