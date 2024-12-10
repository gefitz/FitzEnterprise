using AutoMapper;
using FitzEnterprise.API.Data;
using FitzEnterprise.API.Models;
using FitzEnterprise.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Ocsp;

namespace FitzEnterprise.API.Services
{
    public class CidadeService
    {
        private ReturnModel ret = new ReturnModel();
        private readonly CommandMysql _mysql;
        private readonly IMapper _mapper;

        public CidadeService(CommandMysql mysql, IMapper mapper)
        {
            _mysql = mysql;
            _mapper = mapper;
        }

        public async Task<ReturnModel> BuscarTodosCidades()
        {
            string query = "Select * from tbl_Cidades";
            MySqlConnection conn = new MySqlConnection();
            List<CidadeModel> cidades = new List<CidadeModel>();
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
                            CidadeModel cidade = new CidadeModel
                            {
                                idCidade = Convert.ToInt32(retQuery["idCidade"]),
                                Cidade = retQuery["Cidade"].ToString(),
                                Estado = retQuery["Estado"].ToString(),
                                Sigla = retQuery["Sigla"].ToString()
                            };
                            cidades.Add(cidade);

                        }
                        if (cidades.Count == 0) { ret.Mensagem = "Nenhum cidade encontrado"; }
                        ret.Sucesso = true;
                        ret.Objeto = _mapper.Map<IEnumerable<CidadeDTO>>(cidades); ;

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
        public async Task<ReturnModel> BuscarCidade(int id)
        {
            string query = $"Select * from tbl_Cidades where idCidade = {id}";
            MySqlConnection conn = new MySqlConnection();
            CidadeModel cidade = null;
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
                            cidade = new CidadeModel
                            {
                                idCidade = Convert.ToInt32(retQuery["idCidade"]),
                                Cidade = retQuery["Cidade"].ToString(),
                                Estado = retQuery["Estado"].ToString(),
                                Sigla = retQuery["Sigla"].ToString()
                            };

                        }
                    }
                }
                if (cidade == null)
                {
                    ret.Mensagem = "O id não foi encontrado";
                }
                ret.Sucesso = true;
                ret.Objeto = _mapper.Map<CidadeDTO>(cidade);

            }
            catch (Exception ex)
            {
                ret.Sucesso = false;
                ret.Mensagem = ex.Message;
                _mysql.ResgistraLog(ex);
            }
            return ret;

        }
        public async Task<ReturnModel> BuscarTodosEstados()
        {
            string query = "Select Distinct Estado,Sigla from tbl_Cidades";
            MySqlConnection conn = new MySqlConnection();
            List<CidadeModel> cidades = new List<CidadeModel>();
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
                            CidadeModel cidade = new CidadeModel
                            {
                                Estado = retQuery["Estado"].ToString(),
                                Sigla = retQuery["Sigla"].ToString()
                            };
                            cidades.Add(cidade);

                        }
                        if (cidades.Count == 0) { ret.Mensagem = "Nenhum cidade encontrado"; }
                        ret.Sucesso = true;
                        ret.Objeto = _mapper.Map<IEnumerable<CidadeDTO>>(cidades); ;

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
        public async Task<ReturnModel> BuscarEstado(string sigla)
        {
            string query = $"Select Distinct Estado,Sigla from tbl_Cidades where idCidade = '{sigla}'";
            MySqlConnection conn = new MySqlConnection();
            CidadeModel cidade = null;
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
                            cidade = new CidadeModel
                            {
                                idCidade = Convert.ToInt32(retQuery["idCidade"]),
                                Estado = retQuery["Estado"].ToString(),
                                Sigla = retQuery["Sigla"].ToString()
                            };

                        }
                    }
                }
                if (cidade == null)
                {
                    ret.Mensagem = "O id não foi encontrado";
                }
                ret.Sucesso = true;
                ret.Objeto = _mapper.Map<CidadeDTO>(cidade);

            }
            catch (Exception ex)
            {
                ret.Sucesso = false;
                ret.Mensagem = ex.Message;
                _mysql.ResgistraLog(ex);
            }
            return ret;

        }

    }
}
