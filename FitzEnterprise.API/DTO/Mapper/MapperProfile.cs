using AutoMapper;
using FitzEnterprise.API.DTO;
using FitzEnterprise.Models;
using Fitznterprise.Models;

namespace FitzEnterprise.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ClienteModel, ClienteDTO>().ReverseMap();
            CreateMap<UsuarioModel, UsuarioDTO>().ReverseMap();
            CreateMap<ProdutosModel, ProdutoDTO>().ReverseMap();
            CreateMap<CidadeDTO,CidadeModel>().ReverseMap();
            CreateMap<VendaModel,VendaDTO>().ReverseMap();
            CreateMap<InsertUsuario, InsertUsuarioDTO>().ReverseMap();
            CreateMap<UsuarioModel, InsertUsuarioDTO>().ReverseMap();
            CreateMap<InsertClienteModel, InsertClienteDTO>().ReverseMap();
            CreateMap<InsertProdutosModel, InsertProdutoDTO>().ReverseMap();
            CreateMap<InsertVendaModel, InsertVendaDTO>().ReverseMap();

        }
    }
}
