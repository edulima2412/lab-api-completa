using AutoMapper;
using Lab.Api.Dtos;
using Lab.Business.Models;

namespace Lab.Api.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Fornecedor, FornecedorDto>()
                .ReverseMap();

            CreateMap<Endereco, EnderecoDto>()
                .ReverseMap();

            CreateMap<ProdutoDto, Produto>();

            CreateMap<ProdutoImagemDto, Produto>()
                .ReverseMap();

            CreateMap<Produto, ProdutoDto>()
                .ForMember(dest => dest.NomeFornecedor, opt => opt.MapFrom(src => src.Fornecedor.Nome));
        }
    }
}
