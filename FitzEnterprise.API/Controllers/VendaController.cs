using AutoMapper;
using FitzEnterprise.API.DTO;
using FitzEnterprise.API.Models;
using FitzEnterprise.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitzEnterprise.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendaController : Controller
    {
        private readonly VendaService _service;


        public VendaController(VendaService service, IMapper mapper)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReturnModel>> CreateVenda(InsertVendaDTO vendaDTO)
        {
            ReturnModel ret = new ReturnModel();
            if (vendaDTO == null)
            {
                ret.Sucesso = false;
                ret.Mensagem = "Voce deve fornece alguma informação do venda";
                return BadRequest(ret);
            }
            ret = await _service.CreateVendaAsync(vendaDTO);
            return Ok(ret);
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ReturnModel>> Vendas()
        {
            ReturnModel ret = new ReturnModel();
            ret = await _service.BuscarTodosVendas();
            if (!ret.Sucesso) { return BadRequest(ret); }
            return Ok(ret);
        }
        [HttpGet("BuscarVenda")]
        [Authorize]
        public async Task<ActionResult<ReturnModel>> BuscarVenda(int id)
        {
            ReturnModel ret = new ReturnModel();
            if (id == 0)
            {
                ret.Sucesso = false;
                ret.Mensagem = "O id tem que ser maior que zero";
                return BadRequest(ret);
            }
            ret = await _service.BuscarVenda(id);
            return Ok(ret);
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ReturnModel>> UpadteVenda(VendaDTO vendaDTO)
        {
            ReturnModel ret = new ReturnModel();
            if (vendaDTO == null)
            {
                ret.Sucesso = false;
                ret.Mensagem = "Deve passar um venda";
                return BadRequest(ret);
            }
            ret = await _service.UpdateVenda(vendaDTO);
            return Ok(ret);
        }

    }
}
