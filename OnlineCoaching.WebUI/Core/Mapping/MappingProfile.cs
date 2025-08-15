using AutoMapper;

namespace OnlineCoaching.Web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //#region User
            //CreateMap<ApplicationUser, UserViewModel>()
            //    .ReverseMap();

            //CreateMap<UserFormViewModel, CreateUserDto>()
            //    .ReverseMap();

            //CreateMap<UserFormViewModel, ApplicationUser>()
            //    .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email!.ToUpper()))
            //    .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()))
            //    .ReverseMap();
            //#endregion

     

        }
    }
}