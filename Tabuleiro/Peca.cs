using System.Net.Http.Headers;

namespace tabuleiro
{
    internal abstract class Peca // Superclasse usada para representar genericamente todas as peças
    {
        public Posicao Posicao { get; set; } // Posição da peça
        public Cor Cor { get; protected set; } // Cor da peça
        public int qteMovimentos { get; protected set; } // Quantidade de movimentos que a peça realizou
        public Tabuleiro tab { get; protected set; } // Tabuleiro em que a peça está presente

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

        public void DecrementarQteMovimentos()
        {
            qteMovimentos--;
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

        public bool MovimentoPossivel(Posicao pos) // Verifica se a posição informada para a peça é possível de ser feita
        {
            return MovimentosPossiveis()[pos.Linha, pos.Coluna] == true;
        }

        public abstract bool[,] MovimentosPossiveis();
    }
}
