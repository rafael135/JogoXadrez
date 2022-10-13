using tabuleiro;

namespace xadrez
{
    internal class Torre : Peca
    {
        public Torre(Tabuleiro tab, Cor cor)
            : base(tab, cor)
        {

        }

        private bool PodeMover(Posicao pos)
        {
            Peca p = tab.Peca(pos);
            return p == null || p.Cor != this.Cor;

        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[tab.Linhas, tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            // Acima
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
            while(tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if(tab.Peca(pos) != null && tab.Peca(pos).Cor != Cor)
                {
                    break;
                }

                pos.Linha = pos.Linha - 1;
            }

            // Abaixo
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
            while (tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.Peca(pos) != null && tab.Peca(pos).Cor != Cor)
                {
                    break;
                }

                pos.Linha = pos.Linha + 1;
            }

            // Direita
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
            while (tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.Peca(pos) != null && tab.Peca(pos).Cor != Cor)
                {
                    break;
                }

                pos.Coluna = pos.Coluna + 1;
            }

            // Esquerda
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
            while (tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (tab.Peca(pos) != null && tab.Peca(pos).Cor != Cor)
                {
                    break;
                }

                pos.Coluna = pos.Coluna - 1;
            }

            return mat;
        }

        public override string ToString()
        {
            return "T";
        }
    }
}
