using System.Collections.Generic;
using tabuleiro;

namespace xadrez
{
    internal class PartidaXadrez
    {
        public Tabuleiro tab { get; private set; } // Armazena o tabuleiro onde o jogo está ocorrendo
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        private HashSet<Peca> Pecas; // Armazena as peças da partida
        private HashSet<Peca> PecasCapturadas; // Armazena as peças capturadas
        public bool Xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }

        public PartidaXadrez()
        {
            tab = new Tabuleiro(8, 8);
            Turno = 1;
            Terminada = false;
            JogadorAtual = Cor.Branco;
            Pecas = new HashSet<Peca>();
            PecasCapturadas = new HashSet<Peca>();
            Xeque = false;
            VulneravelEnPassant = null;
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.RetirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = tab.RetirarPeca(destino);
            tab.ColocarPeca(p, destino);

            if (pecaCapturada != null)
            {
                PecasCapturadas.Add(pecaCapturada);
            }

            // JogadaEspecial ROQUE PEQUENO
            if(p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQteMovimentos();
                tab.ColocarPeca(T, destinoT);
            }

            // JogadaEspecial ROQUE GRANDE
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQteMovimentos();
                tab.ColocarPeca(T, destinoT);
            }

            // JogadaEspecial EnPassant
            if(p is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == null) // Ele não sobrepõe a peça, por isso a captura deve ser feita manualmente
                {
                    Posicao posP;
                    if(p.Cor == Cor.Branco)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }

                    pecaCapturada = tab.RetirarPeca(posP);
                    PecasCapturadas.Add(pecaCapturada);
                }
            }




            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.RetirarPeca(destino); // Retiro a peça que fez o movimento
            p.DecrementarQteMovimentos(); // Decremento a quantidade de movimentos

            if (pecaCapturada != null) // Se houver uma peça capturada
            {
                tab.ColocarPeca(pecaCapturada, destino); // Coloca a peça capturada de volta em sua posição original
                PecasCapturadas.Remove(pecaCapturada); // Removo a peça da lista de peças capturadas
            }

            tab.ColocarPeca(p, origem); // Coloco a peça que fez o movimento em seu lugar original

            // JogadaEspecial ROQUE PEQUENO
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.RetirarPeca(destinoT);
                T.DecrementarQteMovimentos();
                tab.ColocarPeca(T, origemT);
            }

            // JogadaEspecial ROQUE GRANDE
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.RetirarPeca(destinoT);
                T.DecrementarQteMovimentos();
                tab.ColocarPeca(T, origemT);
            }

            // JogadaEspecial EnPassant
            if(p is Peao)
            {
                if(origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = tab.RetirarPeca(destino);
                    Posicao posP;
                    if(p.Cor == Cor.Branco)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }

                    tab.ColocarPeca(peao, posP);
                }
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino); // Executa o movimento e retorna a possível peça que foi capturada

            if (EstaEmCheque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em cheque!");
            }

            if (EstaEmCheque(Adversario(JogadorAtual)) == true) // Se a peça do adversário estiver em cheque
            {
                Xeque = true; // Seto xeque como verdadeiro
            }
            else
            {
                Xeque = false;
            }

            if (TesteXequeMate(Adversario(JogadorAtual)) == true) // Verifica se o adversário do jogador está em xeque mate
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }

            Peca p = tab.Peca(destino); // Peça que foi movida

            // JogadaEspecial EnPassant
            if(p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2)) // +2 e -2 pq o peão só pode andar 2 vezes se for seu primeiro movimento
            {
                VulneravelEnPassant = p;
            }
            else
            {
                VulneravelEnPassant = null;
            }
        }

        private void MudaJogador()
        {
            if (JogadorAtual == Cor.Branco)
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
            if (tab.Peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }

            if (JogadorAtual != tab.Peca(pos).Cor)
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
            if (tab.Peca(origem).MovimentoPossivel(destino) == false)
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }



        public HashSet<Peca> GetPecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach (Peca x in PecasCapturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }

            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach (Peca x in Pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }

            aux.ExceptWith(GetPecasCapturadas(cor));

            return aux;
        }

        private Cor Adversario(Cor cor)
        {
            if (cor == Cor.Branco)
            {
                return Cor.Preto;
            }
            else
            {
                return Cor.Branco;
            }
        }

        private Peca Rei(Cor cor)
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }

            return null;
        }

        public bool EstaEmCheque(Cor cor)
        {
            Peca R = Rei(cor);
            if (R == null)
            {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!");
            }

            foreach (Peca x in PecasEmJogo(Adversario(cor))) // Percorre cada peça do jogador atual
            {
                bool[,] mat = x.MovimentosPossiveis(); // Pega os movimentos possíveis da peça

                if (mat[R.Posicao.Linha, R.Posicao.Coluna] == true) // se o rei da cor informada estiver em uma das posições possíveis da peça
                {
                    return true; // Retorna verdadeiro
                }
            }

            return false;
        }

        public bool TesteXequeMate(Cor cor)
        {
            if (EstaEmCheque(cor) != true) // Verifica se alguma peça adversária está em cheque
            {
                return false;
            }
            
            foreach(Peca x in PecasEmJogo(cor)) // Para cada peça em jogo
            {
                bool[,] mat = x.MovimentosPossiveis(); // Pega os movimentos possíveis da peça

                for(int i = 0; i < tab.Linhas; i++)
                {
                    for (int j = 0; j < tab.Colunas; j++)
                    {
                        if (mat[i, j] == true) // Se o movimento na linha 'i' e coluna 'j' for possível
                        {
                            Posicao origem = x.Posicao; // Pega a origem da peça
                            Posicao destino = new Posicao(i, j); // Destino
                            Peca pecaCapturada = ExecutaMovimento(origem, destino); // Pega a possível peça capturada
                            bool testeXeque = EstaEmCheque(cor); // Verifica se a cor do adversário do
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if(testeXeque != true)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;


        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            //Branco
            ColocarNovaPeca('a', 1, new Torre(tab, Cor.Branco));
            ColocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branco));
            ColocarNovaPeca('c', 1, new Bispo(tab, Cor.Branco));
            ColocarNovaPeca('d', 1, new Dama(tab, Cor.Branco));
            ColocarNovaPeca('e', 1, new Rei(tab, Cor.Branco, this));
            ColocarNovaPeca('f', 1, new Bispo(tab, Cor.Branco));
            ColocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branco));
            ColocarNovaPeca('h', 1, new Torre(tab, Cor.Branco));
            ColocarNovaPeca('a', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('b', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('c', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('d', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('e', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('f', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('g', 2, new Peao(tab, Cor.Branco, this));
            ColocarNovaPeca('h', 2, new Peao(tab, Cor.Branco, this));


            // Preto
            ColocarNovaPeca('a', 8, new Torre(tab, Cor.Preto));
            ColocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preto));
            ColocarNovaPeca('c', 8, new Bispo(tab, Cor.Preto));
            ColocarNovaPeca('d', 8, new Dama(tab, Cor.Preto));
            ColocarNovaPeca('e', 8, new Rei(tab, Cor.Preto, this));
            ColocarNovaPeca('f', 8, new Bispo(tab, Cor.Preto));
            ColocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preto));
            ColocarNovaPeca('h', 8, new Torre(tab, Cor.Preto));
            ColocarNovaPeca('a', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('b', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('c', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('d', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('e', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('f', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('g', 7, new Peao(tab, Cor.Preto, this));
            ColocarNovaPeca('h', 7, new Peao(tab, Cor.Preto, this));
        }
    }
}
