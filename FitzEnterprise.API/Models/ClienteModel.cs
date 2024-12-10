namespace FitzEnterprise.Models
{
    public class ClienteModel
    {
        public int idCliente { get; set; }
        public string Nome { get; set; }
        public string CNPJCPF { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public DateTime dthCriacaoCliente { get; set; }
        public DateTime dthAlteracaoCliente { get; set; }
        public short Ativo { get; set; }
        public CidadeModel Cidade { get; set; }
        public IEnumerable<ProdutosModel> Produtos { get; set; }
    }
}
