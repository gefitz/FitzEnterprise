﻿namespace FitzEnterprise
{
    public class ProdutoDTO
    {
        public int idProduto { get; set; }
        public int CodigoProduto { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
        public int qtd { get; set; }
        public float vlrProduto { get; set; }
        public short Ativo { get; set; }
        public DateTime dthCriacaoProduto { get; set; }
        public DateTime dthAlteracaoProduto { get; set; }
    }    public class InsertProdutoDTO
    {
        public int CodigoProduto { get; set; }
        public string Nome { get; set; }
        public string Marca { get; set; }
        public int qtd { get; set; }
        public float vlrProduto { get; set; }
        public short Ativo { get; set; }
        public DateTime dthCriacaoProduto { get; set; }
        public DateTime dthAlteracaoProduto { get; set; }
    }
}