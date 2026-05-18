using AutoMapper;
using BLL.DTOs;
using DAL.Models;

namespace BLL;

public static class MapperConfig
{
    private static readonly MapperConfiguration config = new(cfg =>
    {
        cfg.CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));
        cfg.CreateMap<User, LoginDto>();
        cfg.CreateMap<User, RegisterDto>().ReverseMap();
        cfg.CreateMap<Patient, RegisterDto>().ReverseMap();
        cfg.CreateMap<User, DoctorDto>()
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        cfg.CreateMap<Doctor, DoctorDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User.Gender))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.User.DateOfBirth))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ReverseMap()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore())
            .ForMember(dest => dest.Prescriptions, opt => opt.Ignore());
        cfg.CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.User.Name))
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.User.Name))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Doctor.Department.Name))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ReverseMap();
        cfg.CreateMap<Department, DepartmentDto>().ReverseMap();
        cfg.CreateMap<User, AdminDto>().ReverseMap();
        cfg.CreateMap<User, PatientDto>()
            .ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        cfg.CreateMap<Patient, PatientDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User.Gender))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.User.DateOfBirth))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.User.Address))
            .ReverseMap()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore());
        cfg.CreateMap<Prescription, PrescriptionDto>().ReverseMap();
    });

    public static Mapper GetMapper()
    {
        return new Mapper(config);
    }
}