using System;
using AutoMapper;
using UserData.Entities;
using UserManagement.Models;

namespace UserManagement;

public class UserManagementProfile : Profile
{
    public UserManagementProfile()
    {
        CreateMap<User, RegisterUserDto>().ReverseMap();
        CreateMap<UserRole, RoleDto>().ReverseMap();
    }
}
