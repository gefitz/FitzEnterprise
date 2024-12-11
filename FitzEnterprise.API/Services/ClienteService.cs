using AutoMapper;
using FitzEnterprise.API.Data;
using FitzEnterprise.API.Models;
using FitzEnterprise.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Ocsp;

namespace FitzEnterprise.API.Services
{
    public class ClienteService
    {

        private ReturnModel ret = new ReturnModel();
        private readonly CommandMysql _mysql;
        private readonly IMapper _mapper;

        public ClienteService(CommandMysql mysql, IMapper mapper)
        {
            _mysql = mysql;
            _mapper = mapper;
        }

        public async Task<ReturnModel> CreateClienteAsync(InsertClienteDTO cliente)
        {
            var modelCliente = _mapper.Map<InsertClienteModel>(cliente);
            modelCliente.dthCriacaoCliente = DateTime.Now;
            modelCliente.dthAlteracaoCliente= DateTime.Now;
            ret = await _mysql.ExecuteInsert("tbl_Clientes", modelCliente);
            ret.Objeto = cliente;

            return ret;

        }
        public async Task<ReturnModel> BuscarTodosClientes()
        {
            string query = "Select * from tbl_Clientes";
            MySqlConnection conn = new MySqlConnection();
            List<ClienteModel> clientes = new List<ClienteModel>();
            try
            {

                using (conn = _mysql.Open(conn))
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        MySqlCommand command = new MySqlCommand(query, conn);

                        var retQuery = await command.ExecuteReaderAsync();

                        while (retQuery.Read())
                        {
                            ClienteModel cliente = new ClienteModel
                            {
                                idCliente = Convert.ToInt32(retQuery["idCliente"]),
                                Nome = retQuery["Nome"].ToString(),
                                CNPJCPF = retQuery["CNPJCPF"].ToString(),
                                Email = retQuery["Email"].ToString(),
                                Endereco = retQuery["Endereco"].ToString(),
                                Telefone = retQuery["Telefone"].ToString(),
                                Ativo = short.Parse(retQuery["Ativo"].ToString()),
                                dthCriacaoCliente = DateTime.Parse(retQuery["dthCriacaoCliente"].ToString()),
                                dthAlteracaoCliente = DateTime.Parse(retQuery["dthAlteracaoCliente"].ToString()),
                                Cidade = new CidadeModel { idCidade = Convert.ToInt32(retQuery["idCidade"]) }
                            };
                            clientes.Add(cliente);

                        }
                        if (clientes.Count == 0) { ret.Mensagem = "Nenhum cliente encontrado"; }
                        ret.Sucesso = true;
                        ret.Objeto = _mapper.Map<IEnumerable<ClienteDTO>>(clientes); ;

                    }
                }
            }
            catch (Exception ex)
            {
                ret.Sucesso = false;
                ret.Mensagem = ex.Message;
                _mysql.ResgistraLog(ex);
            }
            return ret;
        }
        public async Task<ReturnModel> BuscarCliente(int id)
        {
            string query = $"Select * from tbl_Clientes where idCliente = {id}";
            MySqlConnection conn = new MySqlConnection();
            ClienteModel cliente = null;
            try
            {

                using (conn = _mysql.Open(conn))
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        MySqlCommand command = new MySqlCommand(query, conn);

                        var retQuery = await command.ExecuteReaderAsync();

                        while (retQuery.Read())
                        {
                            cliente = new ClienteModel
                            {
                                idCliente = Convert.ToInt32(retQuery["idCliente"]),
                                Nome = retQuery["Nome"].ToString(),
                                CNPJCPF = retQuery["CNPJCPF"].ToString(),
                                Email = retQuery["Email"].ToString(),
                                Endereco = retQuery["Endereco"].ToString(),
                                Telefone = retQuery["Telefone"].ToString(),
                                Ativo = short.Parse(retQuery["Ativo"].ToString()),
                                dthCriacaoCliente = DateTime.Parse(retQuery["dthCriacaoCliente"].ToString()),
                                dthAlteracaoCliente = DateTime.Parse(retQuery["dthAlteracaoCliente"].ToString()),
                                Cidade = new CidadeModel { idCidade = Convert.ToInt32(retQuery["idCidade"]) }
                            };

                        }
                    }
                }
                if (cliente == null)
                {
                    ret.Mensagem = "O id não foi encontrado";
                }
                ret.Sucesso = true;
                ret.Objeto = _mapper.Map<ClienteDTO>(cliente);

            }
            catch (Exception ex)
            {
                ret.Sucesso = false;
                ret.Mensagem = ex.Message;
                _mysql.ResgistraLog(ex);
            }
            return ret;

        }

        public async Task<ReturnModel> UpdateCliente(ClienteDTO clienteDTO)
        {
            var modelCliente = _mapper.Map<ClienteModel>(clienteDTO);
            modelCliente.dthAlteracaoCliente = DateTime.Now;
            ret = await _mysql.ExecuteUpdate("tbl_Clientes", modelCliente);
            ret.Objeto = modelCliente;

            return ret;
        }
    }
}
