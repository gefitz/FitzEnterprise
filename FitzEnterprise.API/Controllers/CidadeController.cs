using AutoMapper;
using FitzEnterprise.API.Models;
using FitzEnterprise.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitzEnterprise.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CidadeController : Controller
    {
        private readonly CidadeService _service;


        public CidadeController(CidadeService service, IMapper mapper)
        {
            _service = service;
        }
        [HttpGet("BuscarTodasCidades")]
        public async Task<ActionResult<ReturnModel>> Cidades()
        {
            ReturnModel ret = new ReturnModel();
            ret = await _service.BuscarTodosCidades();
            if (!ret.Sucesso) { return BadRequest(ret); }
            return Ok(ret);
        }
        [HttpGet("BuscarCidade")]
        public async Task<ActionResult<ReturnModel>> BuscarCidade(int id)
        {
            ReturnModel ret = new ReturnModel();
            if (id == 0)
            {
                ret.Sucesso = false;
                ret.Mensagem = "O id tem que ser maior que zero";
                return BadRequest(ret);
            }
            ret = await _service.BuscarCidade(id);
            return Ok(ret);
        }
        [HttpGet("BuscarTodasEstados")]
        public async Task<ActionResult<ReturnModel>> Estados()
        {
            ReturnModel ret = new ReturnModel();
            ret = await _service.BuscarTodosEstados();
            if (!ret.Sucesso) { return BadRequest(ret); }
            return Ok(ret);
        }
        [HttpGet("BuscarEstados")]
        public async Task<ActionResult<ReturnModel>> BuscarEstado(string sigla)
        {
            ReturnModel ret = new ReturnModel();
            if (sigla == "")
            {
                ret.Sucesso = false;
                ret.Mensagem = "deve passar a sigla do estado";
                return BadRequest(ret);
            }
            ret = await _service.BuscarEstado(sigla);
            return Ok(ret);
        }

    }
}
