namespace Fitznterprise.Models
{
    public class VendaModel
    {
        public int idVenda { get; set; }
        public int idCliente { get; set; }
        public int idProduto { get; set; }
        public int qtdVendido { get; set; }
        public float vltTotal { get; set; }
        public DateTime dthVenda { get; set; }
    }
}
