using FitzEnterprise.API.Models;
using FitzEnterprise.API.Services;
using FitzEnterprise.Models;
using Microsoft.AspNetCore.Identity.Data;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace FitzEnterprise.API.Data
{
    public class CommandMysql
    {
        private readonly string _connectionString;
        private string[] PropriedadesDeveIgnorar = ["idUsuario","Cidade"];

        public CommandMysql(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection Open(MySqlConnection conection)
        {
            if (conection.State == System.Data.ConnectionState.Closed)
            {
                conection.ConnectionString = _connectionString;
                conection.Open();
            }
            return conection;
        }

        public MySqlConnection Close(MySqlConnection conection)
        {
            if (conection.State == System.Data.ConnectionState.Open)
            {
                conection.ConnectionString = _connectionString;
                conection.Open();
            }
            return conection;
        }
        public async Task<ReturnModel> ExecuteInsert(string tabelaNome, object parametros)
        {
            MySqlConnection connection = new MySqlConnection();
            ReturnModel ret = new ReturnModel();
            try
            {

                string cmdInsert = $"Insert {tabelaNome} (colunas) values (";
                MySqlParameter[] sqlParameters = new MySqlParameter[parametros.GetType().GetProperties().Length];
                int i = 0;
                string colunas = "";
                // Itera sobre as propriedades do objeto de parâmetros
                foreach (var propriedade in parametros.GetType().GetProperties())
                {
                    if (!PropriedadesDeveIgnorar.Contains(propriedade.Name)) 
                    {
                        // Adiciona o nome da coluna à string de comando SQL
                        if (i == parametros.GetType().GetProperties().Length - 1)
                        {
                            cmdInsert += "@" + propriedade.Name;
                            colunas += propriedade.Name;
                        }
                        else
                        {
                            cmdInsert += "@" + propriedade.Name + ", ";
                            colunas += propriedade.Name + ",";
                        }

                        // Adiciona o parâmetro ao array de parâmetros
                        sqlParameters[i] = new MySqlParameter("@" + propriedade.Name, propriedade.GetValue(parametros));
                        i++;
                    }

                }
                cmdInsert = cmdInsert.TrimEnd(',', ' ') + "";
                colunas = colunas.TrimEnd(',', ' ') + "";

                cmdInsert = cmdInsert.Replace("colunas", colunas);
                cmdInsert += ")";
                using (connection = Open(connection))
                {
                    using (MySqlCommand command = new MySqlCommand(cmdInsert, connection))
                    {
                        command.Parameters.AddRange(sqlParameters);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                ret.Sucesso = true;
            }
            catch (Exception ex)
            {
                ret.Sucesso = false;
                ret.Mensagem = ex.Message;
                ResgistraLog(ex);

            }
            return ret;
        }
        public async Task<ReturnModel> ExecuteUpdate(string tabelaNome, object parametros)
        {
            MySqlConnection connection = new MySqlConnection();
            ReturnModel ret = new ReturnModel();

            try
            {
                // Comando de atualização
                string cmdUpdate = $"UPDATE {tabelaNome} SET ";
                MySqlParameter[] sqlParameters = new MySqlParameter[parametros.GetType().GetProperties().Length];
                int i = 0;
                string setClauses = "";
                string whereClause = " WHERE ";
                bool hasWhere = false;

                // Itera sobre as propriedades do objeto de parâmetros
                foreach (var propriedade in parametros.GetType().GetProperties())
                {
                    if (propriedade.Name.Contains("id"))
                    {
                        // Adiciona a condição WHERE para o ID
                        whereClause += $"{propriedade.Name} = @{propriedade.Name}";
                        sqlParameters[i] = new MySqlParameter("@" + propriedade.Name, propriedade.GetValue(parametros));
                        hasWhere = true;
                    }
                    else
                    {
                        // Adiciona a cláusula SET para os outros campos


                        if (i == parametros.GetType().GetProperties().Length - 1)
                        {
                            setClauses += $"{propriedade.Name} = @{propriedade.Name}";
                            sqlParameters[i] = new MySqlParameter("@" + propriedade.Name, propriedade.GetValue(parametros));
                        }
                        else
                        {

                            setClauses += $"{propriedade.Name} = @{propriedade.Name}";
                            sqlParameters[i] = new MySqlParameter("@" + propriedade.Name, propriedade.GetValue(parametros));
                            setClauses += ", ";
                        }
                    }
                    i++;
                }

                // Se não encontrar um ID (não pode atualizar sem identificador), lança um erro
                if (!hasWhere)
                {
                    ret.Mensagem = "A entidade precisa de um campo 'id' para realizar a atualização.";
                }

                // Construa o comando SQL completo
                cmdUpdate += setClauses + whereClause;

                // Executa o comando SQL no banco de dados
                using (connection = Open(connection)) // Método Open para abrir a conexão com o banco
                {
                    using (MySqlCommand command = new MySqlCommand(cmdUpdate, connection))
                    {
                        command.Parameters.AddRange(sqlParameters);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                ret.Sucesso = true;
            }
            catch (Exception ex)
            {
                ret.Sucesso = false;
                ret.Mensagem = ex.Message;
                ResgistraLog(ex);
            }

            return ret;
        }
        public async Task<ReturnModel> Delet(string tabela, string coluna, int id)
        {
            MySqlConnection connection = new MySqlConnection();
            ReturnModel ret = new ReturnModel();
            string query = $"Delete from {tabela} where {coluna} = {id}";
            try
            {
                using (connection = Open(connection))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);

                    await command.ExecuteNonQueryAsync();
                }
                ret.Sucesso = true;
                ret.Mensagem = "Item Excluido com sucesso";
            }
            catch (Exception ex)
            {
                ret.Sucesso = false;
                ret.Mensagem = ex.Message;
                ResgistraLog(ex);
            }

            return ret;

        }

        public async void ResgistraLog(Exception ex)
        {
            ErrorModel erro = new ErrorModel();

            if (ex != null)
            {
                erro.Erro = ex.Message;
                erro.dthErro = DateTime.Now;

                await ExecuteInsert("Log", erro);
            }
        }
    }
}
