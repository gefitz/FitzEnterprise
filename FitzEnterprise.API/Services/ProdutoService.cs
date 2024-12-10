using AutoMapper;
using FitzEnterprise.API.Data;
using FitzEnterprise.API.Models;
using FitzEnterprise.Models;
using MySql.Data.MySqlClient;

namespace FitzEnterprise.API.Services
{
    public class ProdutoService
    {
        private ReturnModel ret = new ReturnModel();
        private readonly CommandMysql _mysql;
        private readonly IMapper _mapper;

        public ProdutoService(CommandMysql mysql, IMapper mapper)
        {
            _mysql = mysql;
            _mapper = mapper;
        }

        public async Task<ReturnModel> CreateProdutoAsync(ProdutoDTO produto)
        {
            var modelProduto = _mapper.Map<ProdutosModel>(produto);
            produto.dthCriacaoProduto = DateTime.Now;
            produto.dthAlteracaoProduto = DateTime.Now;
            ret = await _mysql.ExecuteInsert("tbl_Produtos", modelProduto);
            ret.Objeto = produto;

            return ret;

        }
        public async Task<ReturnModel> BuscarTodosProdutos()
        {
            string query = "Select * from tbl_Produtos";
            MySqlConnection conn = new MySqlConnection();
            List<ProdutosModel> produtos = new List<ProdutosModel>();
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
                            ProdutosModel produto = new ProdutosModel
                            {
                                idProduto = Convert.ToInt32(retQuery["idProduto"]),
                                Nome = retQuery["Nome"].ToString(),
                                Marca = retQuery["Marca"].ToString(),
                                qtd = Convert.ToInt32(retQuery["qtd"]),
                                vlrProduto = float.Parse(retQuery["vlrProduto"].ToString()),
                                CodigoProduto = Convert.ToInt32(retQuery["CodigoProduto"]),
                                Ativo = short.Parse(retQuery["Ativo"].ToString()),
                                dthCriacaoProduto = DateTime.Parse(retQuery["dthCriacaoProduto"].ToString()),
                                dthAlteracaoProduto = DateTime.Parse(retQuery["dthAlteracaoProduto"].ToString())
                            };
                            produtos.Add(produto);

                        }
                        if(produtos.Count == 0) { ret.Mensagem = "Nenhum produto encontrado"; }
                        ret.Sucesso = true;
                        ret.Objeto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos); ;

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
        public async Task<ReturnModel> BuscarProduto(int id)
        {
            string query = $"Select * from tbl_Produtos where idProduto = {id}";
            MySqlConnection conn = new MySqlConnection();
            ProdutosModel produto = null;
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
                            produto = new ProdutosModel
                            {
                                idProduto = Convert.ToInt32(retQuery["idProduto"]),
                                Nome = retQuery["Nome"].ToString(),
                                Marca = retQuery["Marca"].ToString(),
                                qtd = Convert.ToInt32(retQuery["qtd"]),
                                vlrProduto = float.Parse(retQuery["vlrProduto"].ToString()),
                                CodigoProduto = Convert.ToInt32(retQuery["CodigoProduto"]),
                                Ativo = short.Parse(retQuery["Ativo"].ToString()),
                                dthCriacaoProduto = DateTime.Parse(retQuery["dthCriacaoProduto"].ToString()),
                                dthAlteracaoProduto = DateTime.Parse(retQuery["dthAlteracaoProduto"].ToString())
                            };

                        }
                    }
                }
                if (produto == null)
                {
                    ret.Mensagem = "O id não foi encontrado";
                }
                    ret.Sucesso = true;
                    ret.Objeto = _mapper.Map<ProdutoDTO>(produto);

            }
            catch (Exception ex)
            {
                ret.Sucesso = false;
                ret.Mensagem = ex.Message;
                _mysql.ResgistraLog(ex);
            }
            return ret;

        }
        
        public async Task<ReturnModel> UpdateProduto(ProdutoDTO produtoDTO)
        {
            var modelProduto = _mapper.Map<ProdutosModel>(produtoDTO);
            modelProduto.dthAlteracaoProduto = DateTime.Now;
            ret = await _mysql.ExecuteUpdate("tbl_Produtos", modelProduto);
            ret.Objeto = modelProduto;

            return ret;
        }

    }
}
