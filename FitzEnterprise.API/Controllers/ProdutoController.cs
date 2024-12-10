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
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;


        public ProdutoController(ProdutoService service, IMapper mapper)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReturnModel>> CreateProduto(ProdutoDTO produtoDTO)
        {
            ReturnModel ret = new ReturnModel();
            if (produtoDTO == null)
            {
                ret.Sucesso = false;
                ret.Mensagem = "Voce deve fornece alguma informação do produto";
                return BadRequest(ret);
            }
            ret = await _service.CreateProdutoAsync(produtoDTO);
            return Ok(ret);
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ReturnModel>> Produtos()
        {
            ReturnModel ret = new ReturnModel();
            ret = await _service.BuscarTodosProdutos();
            if(!ret.Sucesso) { return BadRequest(ret); }
            return Ok(ret);
        }
        [HttpGet("BuscarProduto")]
        [Authorize]
        public async Task<ActionResult<ReturnModel>> BuscarProduto(int id)
        {
            ReturnModel ret = new ReturnModel();
            if (id == 0)
            {
                ret.Sucesso = false;
                ret.Mensagem = "O id tem que ser maior que zero";
                return BadRequest(ret);
            }
            ret = await _service.BuscarProduto(id);
            return Ok(ret);
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ReturnModel>> UpadteProduto(ProdutoDTO produtoDTO)
        {
            ReturnModel ret = new ReturnModel();
            if (produtoDTO == null)
            {
                ret.Sucesso = false;
                ret.Mensagem = "Deve passar um produto";
                return BadRequest(ret);
            }
            ret = await _service.UpdateProduto(produtoDTO);
            return Ok(ret);
        }
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<ReturnModel>> DeleteProduto(ProdutoDTO produtoDTO)
        {
            ReturnModel ret = new ReturnModel();
            if (produtoDTO == null)
            {
                ret.Sucesso = false;
                ret.Mensagem = "Deve passar o produto";
                return BadRequest(ret);
            }
            ret = await _service.UpdateProduto(produtoDTO);
            return Ok(ret);
        }
    }
}
