using AutoMapper;
using FitzEnterprise.API.Data;
using FitzEnterprise.API.DTO;
using FitzEnterprise.API.Models;
using FitzEnterprise.Models;
using Fitznterprise.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Ocsp;

namespace FitzEnterprise.API.Services
{
    public class VendaService
    {

        private ReturnModel ret = new ReturnModel();
        private readonly CommandMysql _mysql;
        private readonly IMapper _mapper;

        public VendaService(CommandMysql mysql, IMapper mapper)
        {
            _mysql = mysql;
            _mapper = mapper;
        }

        public async Task<ReturnModel> CreateVendaAsync(VendaDTO venda)
        {
            var modelVenda = _mapper.Map<VendaModel>(venda);
            ret = await _mysql.ExecuteInsert("tbl_Vendas", modelVenda);
            ret.Objeto = venda;

            return ret;

        }
        public async Task<ReturnModel> BuscarTodosVendas()
        {
            string query = "Select * from tbl_Vendas";
            MySqlConnection conn = new MySqlConnection();
            List<VendaModel> vendas = new List<VendaModel>();
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
                            VendaModel venda = new VendaModel
                            {
                                idVenda = Convert.ToInt32(retQuery["idVenda"]),
                                idCliente = Convert.ToInt32(retQuery["idCliente"]),
                                idProduto = Convert.ToInt32(retQuery["idProduto"]),
                                vltTotal = float.Parse(retQuery["vlrTotal"].ToString()),
                            };
                            vendas.Add(venda);

                        }
                        if (vendas.Count == 0) { ret.Mensagem = "Nenhum venda encontrado"; }
                        ret.Sucesso = true;
                        ret.Objeto = _mapper.Map<IEnumerable<VendaDTO>>(vendas); ;

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
        public async Task<ReturnModel> BuscarVenda(int id)
        {
            string query = $"Select * from tbl_Vendas where idVenda = {id}";
            MySqlConnection conn = new MySqlConnection();
            VendaModel venda = null;
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
                            venda = new VendaModel
                            {
                                idVenda = Convert.ToInt32(retQuery["idVenda"]),
                                idCliente = Convert.ToInt32(retQuery["idCliente"]),
                                idProduto = Convert.ToInt32(retQuery["idProduto"]),
                                vltTotal = float.Parse(retQuery["vlrTotal"].ToString()),
                                dthVenda = DateTime.Parse(retQuery["dthVenda"].ToString())
                            };

                        }
                    }
                }
                if (venda == null)
                {
                    ret.Mensagem = "O id não foi encontrado";
                }
                ret.Sucesso = true;
                ret.Objeto = _mapper.Map<VendaDTO>(venda);

            }
            catch (Exception ex)
            {
                ret.Sucesso = false;
                ret.Mensagem = ex.Message;
                _mysql.ResgistraLog(ex);
            }
            return ret;

        }
        public async Task<ReturnModel> UpdateVenda(VendaDTO vendaDTO)
        {
            var modelVenda = _mapper.Map<VendaModel>(vendaDTO);
            
            ret = await _mysql.ExecuteUpdate("tbl_Vendas", modelVenda);
            ret.Objeto = modelVenda;

            return ret;
        }


    }
}
