using System.Globalization;
using System.Text;
using AutoMapper;
using DAL.Entities;
using Model;
using Model.DTO;

namespace API.ClassMapping;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ForMember(dst => dst.Email,
            map => map.MapFrom<string>(src => Encoding.UTF8.GetString(src.Email)))
            .ForMember(dst => dst.UserName,
                map => map.MapFrom<string>(src => Encoding.UTF8.GetString(src.Username)));
    }
}