using AutoMapper;
using ClienteAPI_.DTOs;
using ClienteAPI_.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ClienteDto, Cliente>().ReverseMap();
        CreateMap<ContatoDto, Contato>().ReverseMap();
        CreateMap<EnderecoDto, Endereco>().ReverseMap();



        CreateMap<ClienteDto, Cliente>()
     .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src =>
         string.IsNullOrEmpty(src.DataCadastro)
             ? DateTime.Today.ToString("yyyy-MM-dd")
             : src.DataCadastro
     ));

        CreateMap<Cliente, ClienteDto>()
            .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.DataCadastro));

    }
}