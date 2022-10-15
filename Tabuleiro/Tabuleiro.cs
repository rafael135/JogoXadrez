using tabuleiro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabuleiro
{
    internal class Tabuleiro
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }
        private Peca[,] Pecas; // Armazena as peças presentes no tabuleiro

        public Tabuleiro(int linhas, int colunas)
        {
            this.Linhas = linhas;
            this.Colunas = colunas;

            this.Pecas = new Peca[Linhas, Colunas];
        }

        public Peca Peca(Posicao pos)
        {
            return Pecas[pos.Linha, pos.Coluna]; // 
        }

        public bool ExistePeca(Posicao pos)
        {
            ValidarPosicao(pos); // Chama o método responsável por validar a posição passando a posição da possível peça informada
            return Peca(pos) != null; // Caso a peça exista, retorna verdadeiro
        }

        public Peca GetPeca(int linha, int coluna)
        {
            return Pecas[linha, coluna]; // Retorna a peça com linha e coluna informada
        }

        public void ColocarPeca(Peca p, Posicao pos)
        {
            if (ExistePeca(pos)) // Verifica se existe uma peça na posição
            {
                throw new TabuleiroException("Já existe uma peça na posição!");
            }
            Pecas[pos.Linha, pos.Coluna] = p; // Seta a peça 
            p.Posicao = pos;
        }

        public Peca RetirarPeca(Posicao pos)
        {
            if (Peca(pos) == null) // Se não houver nenhuma peça na posição
            {
                return null; // Retorno nulo
            }

            Peca aux = Peca(pos); // Pego a peça a ser retirada
            aux.Posicao = null; // Seto sua posição como 'nula' para retirar do jogo
            Pecas[pos.Linha, pos.Coluna] = null; // Retiro a peça da posição no tabuleiro
            return aux; // Retorno a peça retirada
        }

        public bool PosicaoValida(Posicao pos)
        {
            if(pos.Linha < 0 || pos.Linha >= Linhas || pos.Coluna < 0 || pos.Coluna >= Colunas) // Verifica se as linhas e colunas da posição não estão dentro do limite das do tabuleiro
            {
                return false; // retorna falso caso esteja fora do limite
            }

            return true;
        }

        public void ValidarPosicao(Posicao pos)
        {
            if (!PosicaoValida(pos)) // Verifica se posição informada é válida
            {
                throw new TabuleiroException("Posição inválida!"); // Joga uma nova exceção
            }
        }
    }
}
