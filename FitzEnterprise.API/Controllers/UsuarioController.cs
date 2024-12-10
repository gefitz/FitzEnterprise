using AutoMapper;
using FitzEnterprise.API.Models;
using FitzEnterprise.API.Services;
using FitzEnterprise.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitzEnterprise.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _service;


        public UsuarioController(UsuarioService service, IMapper mapper)
        {
            _service = service;
        }

        [HttpPost("Cradastrar")]
        [AllowAnonymous]
        public async Task<ActionResult<ReturnModel>> CreateUsuario(InsertUsuarioDTO usuarioDTO)
        {
            ReturnModel ret = new ReturnModel();
            if (usuarioDTO == null)
            {
                ret.Sucesso = false;
                ret.Mensagem = "Voce deve fornece alguma informação do produto";
                return BadRequest(ret);
            }
            ret = await _service.CreateUsuarioAsync(usuarioDTO);
            return Ok(ret);
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<ReturnModel>> Login(UsuarioDTO usuario)
        {
            ReturnModel ret = new ReturnModel();
            if (usuario == null) 
            {
                ret.Sucesso=false;
                ret.Mensagem = "Deve passar as informações de login";
                return BadRequest(ret);
            }
            ret = await _service.Authentication(usuario);
            if (!ret.Sucesso) { return BadRequest(ret); }
            return Ok(ret);
        }
        [HttpGet]
        public async Task<ActionResult<ReturnModel>> Usuarios()
        {
            ReturnModel ret = new ReturnModel();
            ret = await _service.BuscarTodosUsuarios();
            if (!ret.Sucesso) { return BadRequest(ret); }
            return Ok(ret);
        }
        [HttpGet("BuscarUsuario")]
        public async Task<ActionResult<ReturnModel>> BuscarUsuario(int id)
        {
            ReturnModel ret = new ReturnModel();
            if (id == 0)
            {
                ret.Sucesso = false;
                ret.Mensagem = "O id tem que ser maior que zero";
                return BadRequest(ret);
            }
            ret = await _service.BuscarUsuario(id);
            return Ok(ret);
        }
        [HttpPut]
        public async Task<ActionResult<ReturnModel>> UpadteUsuario(UsuarioDTO produtoDTO)
        {
            ReturnModel ret = new ReturnModel();
            if (produtoDTO == null)
            {
                ret.Sucesso = false;
                ret.Mensagem = "Deve passar um produto";
                return BadRequest(ret);
            }
            ret = await _service.UpdateUsuario(produtoDTO);
            return Ok(ret);
        }
        [HttpDelete]
        public async Task<ActionResult<ReturnModel>> DeleteUsuario(UsuarioDTO produtoDTO)
        {
            ReturnModel ret = new ReturnModel();
            if (produtoDTO == null)
            {
                ret.Sucesso = false;
                ret.Mensagem = "Deve passar o produto";
                return BadRequest(ret);
            }
            ret = await _service.UpdateUsuario(produtoDTO);
            return Ok(ret);
        }

    }
}
