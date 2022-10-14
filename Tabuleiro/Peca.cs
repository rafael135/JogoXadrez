using System.Net.Http.Headers;

namespace tabuleiro
{
    internal abstract class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int qteMovimentos { get; protected set; }
        public Tabuleiro tab { get; protected set; }

        public Peca(Tabuleiro tab, Cor cor)
        {
            this.Posicao = null;
            this.tab = tab;
            this.Cor = cor;
            this.qteMovimentos = 0;
        }

        public void IncrementarQteMovimentos()
        {
            qteMovimentos++;
        }

        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mat = MovimentosPossiveis();
            for(int i = 0; i < tab.Linhas; i++)
            {
                for(int j = 0; j < tab.Colunas; j++)
                {
                    if (mat[i, j] == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool PodeMoverPara(Posicao pos)
        {
            return MovimentosPossiveis()[pos.Linha, pos.Coluna];
        }

        public abstract bool[,] MovimentosPossiveis();
    }
}
