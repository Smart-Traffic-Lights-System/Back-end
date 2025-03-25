using System;
using AutoMapper;
using UserData.Entities;
using UserStore.Models;

namespace UserStore;

public class UserManagementProfile : Profile
{
    public UserManagementProfile()
    {
        CreateMap<User, RegisterUserDto>().ReverseMap();
    }
}
