namespace FitzEnterprise.Models
{
    public class UsuarioModel
    {
        public int idUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
        public CidadeModel Cidade { get; set; }
        public int idCidade { get; set; }
        public DateTime dthNascimento { get; set; }
        public DateTime dthCriacaoUsuario { get; set; }
        public DateTime dthAlteracaoUsuario { get; set; }
        public short Ativo { get; set; }
        public int TipoUsuario { get; set; }
    }
    public class InsertUsuario
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
        public int idCidade { get; set; }
        public DateTime dthNascimento { get; set; }
        public DateTime dthCriacaoUsuario { get; set; }
        public DateTime dthAlteracaoUsuario { get; set; }
        public short Ativo { get; set; }
        public int TipoUsuario { get; set; }
    }


}
