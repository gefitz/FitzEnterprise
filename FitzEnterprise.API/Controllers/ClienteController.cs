using AutoMapper;
using FitzEnterprise.API.Models;
using FitzEnterprise.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitzEnterprise.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : Controller
    {
        private readonly ClienteService _service;


        public ClienteController(ClienteService service, IMapper mapper)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ReturnModel>> CreateCliente(InsertClienteDTO clienteDTO)
        {
            ReturnModel ret = new ReturnModel();
            if (clienteDTO == null)
            {
                ret.Sucesso = false;
                ret.Mensagem = "Voce deve fornece alguma informação do cliente";
                return BadRequest(ret);
            }
            ret = await _service.CreateClienteAsync(clienteDTO);
            return Ok(ret);
        }
        [HttpGet]
        public async Task<ActionResult<ReturnModel>> Clientes()
        {
            ReturnModel ret = new ReturnModel();
            ret = await _service.BuscarTodosClientes();
            if (!ret.Sucesso) { return BadRequest(ret); }
            return Ok(ret);
        }
        [HttpGet("BuscarCliente")]
        public async Task<ActionResult<ReturnModel>> BuscarCliente(int id)
        {
            ReturnModel ret = new ReturnModel();
            if (id == 0)
            {
                ret.Sucesso = false;
                ret.Mensagem = "O id tem que ser maior que zero";
                return BadRequest(ret);
            }
            ret = await _service.BuscarCliente(id);
            return Ok(ret);
        }
        [HttpPut]
        public async Task<ActionResult<ReturnModel>> UpadteCliente(ClienteDTO clienteDTO)
        {
            ReturnModel ret = new ReturnModel();
            if (clienteDTO == null)
            {
                ret.Sucesso = false;
                ret.Mensagem = "Deve passar um cliente";
                return BadRequest(ret);
            }
            ret = await _service.UpdateCliente(clienteDTO);
            return Ok(ret);
        }
        [HttpDelete]
        public async Task<ActionResult<ReturnModel>> DeleteCliente(ClienteDTO clienteDTO)
        {
            ReturnModel ret = new ReturnModel();
            if (clienteDTO == null)
            {
                ret.Sucesso = false;
                ret.Mensagem = "Deve passar o cliente";
                return BadRequest(ret);
            }
            ret = await _service.UpdateCliente(clienteDTO);
            return Ok(ret);
        }

    }
}
