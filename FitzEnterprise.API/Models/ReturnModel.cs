using FitzEnterprise.Models;

namespace FitzEnterprise.API.Models
{
    public class ReturnModel
    {
        public object Objeto { get; set; }
        public string Mensagem { get; set; }
        public bool Sucesso { get; set; }
        public ErrorModel LogError { get; set; }

    }
}
