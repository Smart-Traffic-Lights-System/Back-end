using System;
using AutoMapper;
using UserData.Entities;
using UserManagement.Models;

namespace UserManagement;

public class UserManagementProfile : Profile
{
    public UserManagementProfile()
    {
        CreateMap<User, UserDto>();
        // CreateMap<UserDto, User>();
        // CreateMap<UserActionLog, UserActionLogDto>();
        // CreateMap<UserActionLogDto, UserActionLog>();
        // CreateMap<UserRole, UserRoleDto>();
        // CreateMap<UserRoleDto, UserRole>();
    }
}
