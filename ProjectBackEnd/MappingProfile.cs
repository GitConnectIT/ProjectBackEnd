using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities.Models;
using Shared.DTO;

namespace ProjectBackEnd;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Register
        CreateMap<UserRegisterDTO, ApplicationUser>().ReverseMap();
        CreateMap<UserListDTO, ApplicationUser>().ReverseMap();

        //EmailTemplate
        CreateMap<EmailTemplate, EmailTemplateDTO>().ReverseMap();

        //Menu
        CreateMap<ApplicationMenu, ApplicationMenuDTO>().ReverseMap();

        //Clients
        CreateMap<Clients, ClientListDTO>().ReverseMap();
    }
}