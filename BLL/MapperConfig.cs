using AutoMapper;
using BLL.DTOs;
using DAL.Models;

namespace BLL;

public static class MapperConfig
{
    private static readonly MapperConfiguration config = new(cfg =>
    {
        cfg.CreateMap<User, UserDto>();
        cfg.CreateMap<User, LoginDto>();
        cfg.CreateMap<User, RegisterDto>().ReverseMap();
        cfg.CreateMap<Patient, RegisterDto>().ReverseMap();
        cfg.CreateMap<User, DoctorDto>().ReverseMap();
        cfg.CreateMap<Doctor, DoctorDto>();
    });

    public static Mapper GetMapper()
    {
        return new Mapper(config);
    }
}