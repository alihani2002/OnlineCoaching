using AutoMapper;
using OnlineCoaching.Domain.Dtos;

namespace OnlineCoaching.Web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
         

            #region Food 

            CreateMap<Food , FoodDto>().ReverseMap();
            CreateMap<CreateFoodDto , Food>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore()).ReverseMap();
            #endregion

            #region Muscles
            // Entity → DTO
            CreateMap<Muscle, MuscleDto>().ReverseMap();

            // Create DTO → Entity
            CreateMap<CreateMuscleDto, Muscle>();
            #endregion

            #region Exercise 
            // Entity -> DTO
            CreateMap<Exercise, ExerciseDto>()
                .ForMember(dest => dest.MuscleName, opt => opt.MapFrom(src => src.Muscle != null ? src.Muscle.Name : string.Empty))
                .ForMember(dest => dest.LinkUrls, opt => opt.MapFrom(src => src.LinkUrls != null ? src.LinkUrls : new List<string>()));

            // DTO -> Entity
            CreateMap<ExerciseDto, Exercise>()
                .ForMember(dest => dest.Muscle, opt => opt.Ignore()) // Prevent overwriting navigation property
                .ForMember(dest => dest.LinkUrls, opt => opt.MapFrom(src => src.LinkUrls != null ? src.LinkUrls : new List<string>()));

            // Create DTO -> Entity
            CreateMap<CreateExerciseDto, Exercise>()
                .ForMember(dest => dest.Muscle, opt => opt.Ignore()) // ignore navigation
                .ForMember(dest => dest.LinkUrls, opt => opt.MapFrom(src => src.LinkUrls != null ? src.LinkUrls : new List<string>()));
            #endregion

            #region coachingPackage
            // Entity -> DTO
            CreateMap<CoachingPackage, CoachingPackageDto>();

            // DTO -> Entity
            CreateMap<CreateCoachingPackageDto, CoachingPackage>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedById, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.Ignore());

            CreateMap<CoachingPackageDto, CoachingPackage>()
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedById, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.Ignore());
            #endregion


        }
    }
}