using AutoMapper;
using OnlineCoaching.Domain.Dtos;

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


            CreateMap<Food , FoodDto>().ReverseMap();
            CreateMap<CreateFoodDto , Food>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore()).ReverseMap(); 
     

        }
    }
}