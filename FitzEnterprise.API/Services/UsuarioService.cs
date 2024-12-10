using AutoMapper;
using FitzEnterprise.API.Data;
using FitzEnterprise.API.DTO;
using FitzEnterprise.API.Models;
using FitzEnterprise.Models;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FitzEnterprise.API.Services
{
    public class UsuarioService
    {
        private ReturnModel ret = new ReturnModel();
        private readonly CommandMysql _mysql;
        private readonly IMapper _mapper;
        private byte[] hash;
        private byte[] salt;
        private readonly IConfiguration _configuration;

        public UsuarioService(CommandMysql mysql, IMapper mapper, IConfiguration configuration)
        {
            _mysql = mysql;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ReturnModel> CreateUsuarioAsync(InsertUsuarioDTO usuario)
        {
            var modelUsuario = _mapper.Map<InsertUsuario>(usuario);
            ret = CriptografiaSenha(usuario.password);
            if (ret.Sucesso)
            {
                modelUsuario.dthCriacaoUsuario = DateTime.Now;
                modelUsuario.dthAlteracaoUsuario = DateTime.Now;
                modelUsuario.Salt = salt;
                modelUsuario.Hash = hash;
                ret = await _mysql.ExecuteInsert("tbl_Usuarios", modelUsuario);
                if (ret.Sucesso)
                {

                    ret.Objeto = GenerateToken(_mapper.Map<UsuarioModel>(usuario));
                }
                ret.Objeto = usuario;
            }

            return ret;

        }
        public async Task<ReturnModel> BuscarTodosUsuarios()
        {
            string query = "Select * from tbl_Usuarios";
            MySqlConnection conn = new MySqlConnection();
            List<UsuarioModel> usuarios = new List<UsuarioModel>();
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
                            UsuarioModel usuario = new UsuarioModel
                            {
                                idUsuario = Convert.ToInt32(retQuery["idUsuario"]),
                                Nome = retQuery["Nome"].ToString(),
                                Hash = (byte[])retQuery["Hash"],
                                Email = retQuery["Email"].ToString(),
                                Salt = (byte[])retQuery["Salt"],
                                Ativo = short.Parse(retQuery["Ativo"].ToString()),
                                dthCriacaoUsuario = DateTime.Parse(retQuery["dthCriacaoUsuario"].ToString()),
                                dthAlteracaoUsuario = DateTime.Parse(retQuery["dthAlteracaoUsuario"].ToString()),
                                Cidade = new CidadeModel { idCidade = Convert.ToInt32(retQuery["idCidade"]) }
                            };
                            usuarios.Add(usuario);

                        }
                        if (usuarios.Count == 0) { ret.Mensagem = "Nenhum usuario encontrado"; }
                        ret.Sucesso = true;
                        ret.Objeto = _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios); ;

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
        public async Task<ReturnModel> BuscarUsuario(int id)
        {
            string query = $"Select * from tbl_Usuarios where idUsuario = {id}";
            MySqlConnection conn = new MySqlConnection();
            UsuarioModel usuario = null;
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
                            usuario = new UsuarioModel
                            {
                                idUsuario = Convert.ToInt32(retQuery["idUsuario"]),
                                Nome = retQuery["Nome"].ToString(),
                                Hash = (byte[])retQuery["Hash"],
                                Email = retQuery["Email"].ToString(),
                                Salt = (byte[])retQuery["Salt"],
                                Ativo = short.Parse(retQuery["Ativo"].ToString()),
                                dthCriacaoUsuario = DateTime.Parse(retQuery["dthCriacaoUsuario"].ToString()),
                                dthAlteracaoUsuario = DateTime.Parse(retQuery["dthAlteracaoUsuario"].ToString()),
                                Cidade = new CidadeModel { idCidade = Convert.ToInt32(retQuery["idCidade"]) }
                            };

                        }
                    }
                }
                if (usuario == null)
                {
                    ret.Mensagem = "O id não foi encontrado";
                }
                ret.Sucesso = true;
                ret.Objeto = _mapper.Map<UsuarioDTO>(usuario);

            }
            catch (Exception ex)
            {
                ret.Sucesso = false;
                ret.Mensagem = ex.Message;
                _mysql.ResgistraLog(ex);
            }
            return ret;

        }

        public async Task<ReturnModel> UpdateUsuario(UsuarioDTO usuarioDTO)
        {
            ret.Objeto = new UsuarioModel();
            var modelUsuario = _mapper.Map<UsuarioModel>(usuarioDTO);
            ret = CriptografiaSenha(usuarioDTO.password);
            if (ret.Sucesso)
            {
                modelUsuario.dthAlteracaoUsuario = DateTime.Now;
                modelUsuario.Salt = salt;
                modelUsuario.Hash = hash;
                ret = await _mysql.ExecuteUpdate("tbl_Usuarios", modelUsuario);
                ret.Objeto = modelUsuario;
            }

            return ret;
        }
        public async Task<ReturnModel> Authentication(UsuarioDTO usuarioDTO)
        {
            try
            {
                var usuarioModel = await BuscaLogin(usuarioDTO.Email);
                if (usuarioModel != null)
                {
                    if (await ValidaSenha(usuarioModel, usuarioDTO))
                    {
                       return GenerateToken(usuarioModel);

                    }
                    else
                    {
                        
                        ret.Mensagem = "Senha esta incorreta";
                    }
                }
                else
                {
                    ret.Mensagem = "Usuario não encontrado";
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
        private ReturnModel CriptografiaSenha(string password)
        {
            var ret = new ReturnModel();
            try
            {

                using (var hmac = new HMACSHA512())
                {
                    hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    salt = hmac.Key;
                }
                ret.Sucesso = true;
            }
            catch (Exception ex)
            {
                ret.Sucesso = false;
                ret.Mensagem = ex.Message;
                _mysql.ResgistraLog(ex);
            }
            return ret;
        }
        private ReturnModel GenerateToken(UsuarioModel login)
        {
            try
            {

            var claims = new[]
                   {
                        new Claim("id",login.idUsuario.ToString()),
                        new Claim("user", login.Email.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };
            var privateKy = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:secretkey"]));

            var crendentials = new SigningCredentials(privateKy, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(10);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["jwt:issuer"],
                audience: _configuration["jwt:audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: crendentials);
            ret.Objeto = new UserToken()
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                idUsuario = login.idUsuario
            };
            ret.Sucesso = true;
            }catch (Exception ex)
            {
                ret.Sucesso = false;
                ret.Mensagem = ex.Message;
                _mysql.ResgistraLog(ex);
            }
            return ret;
        }
        private async Task<bool> ValidaSenha(UsuarioModel usuarioModel, UsuarioDTO usuarioDTO)
        {

            using var hmac = new HMACSHA512(usuarioModel.Salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(usuarioDTO.password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != usuarioModel.Hash[i])
                {
                    return false;
                }
            }
            return true;
        }
        private async Task<UsuarioModel> BuscaLogin(string email)
        {
            string query = $"Select idUsuario,Email,Hash,Salt from tbl_Usuarios where Email = '{email}' and Ativo = 1";
            MySqlConnection conn = new MySqlConnection();
            UsuarioModel usuario = null;
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
                            usuario = new UsuarioModel
                            {
                                idUsuario = Convert.ToInt32(retQuery["idUsuario"]),
                                Email = retQuery["Email"].ToString(),
                                Hash = (byte[])retQuery["Hash"],
                                Salt = (byte[])retQuery["Salt"],
                            };

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _mysql.ResgistraLog(ex);
            }
            return usuario;
        }

    }
}
