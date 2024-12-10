using Fitznterprise.Models;

namespace FitzEnterprise.Models
{
    public class UsuarioDTO
    {
        public int idUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public CidadeModel Cidade { get; set; }
        public int idCidade {  get; set; }
        public DateTime dthCriacaoUsuario { get; set; }
        public DateTime dthAlteracaoUsuario { get; set; }

        public short Ativo { get; set; }
    }
    public class InsertUsuarioDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public int idCidade { get; set; }
        public DateTime dthCriacaoUsuario { get; set; }
        public DateTime dthAlteracaoUsuario { get; set; }
        public short Ativo { get; set; }

    }
}