using AutoMapper;
using OnlineCoaching.Domain.Dtos;
using OnlineCoaching.Domain.Dtos.AssignmentCoaching;
using OnlineCoaching.Domain.Dtos.Cooking;
using OnlineCoaching.Domain.Dtos.ExerciseSheet;
using OnlineCoaching.Domain.Dtos.Gallery;
using OnlineCoaching.Domain.Dtos.Notes;
using OnlineCoaching.Domain.Entities.Gallery;
using OnlineCoaching.WebUI.Models.RequestPackage;

namespace OnlineCoaching.Web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Food 
            CreateMap<Food, FoodDto>().ReverseMap();
            CreateMap<CreateFoodDto, Food>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region Muscles
            CreateMap<Muscle, MuscleDto>().ReverseMap();
            CreateMap<CreateMuscleDto, Muscle>();
            #endregion

            #region Exercise 
            CreateMap<Exercise, ExerciseDto>()
                .ForMember(dest => dest.MuscleName, opt => opt.MapFrom(src => src.Muscle != null ? src.Muscle.Name : string.Empty))
                .ForMember(dest => dest.LinkUrls, opt => opt.MapFrom(src => src.LinkUrls ?? new List<string>()));

            CreateMap<ExerciseDto, Exercise>()
                .ForMember(dest => dest.Muscle, opt => opt.Ignore())
                .ForMember(dest => dest.LinkUrls, opt => opt.MapFrom(src => src.LinkUrls ?? new List<string>()));

            CreateMap<CreateExerciseDto, Exercise>()
                .ForMember(dest => dest.Muscle, opt => opt.Ignore())
                .ForMember(dest => dest.LinkUrls, opt => opt.MapFrom(src => src.LinkUrls ?? new List<string>()));
            #endregion

            #region CoachingPackage
            CreateMap<CoachingPackage, CoachingPackageDto>().ReverseMap();

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

            #region CoachingPackageRequest
            CreateMap<CoachingPackageRequest, CoachingPackageRequestDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client != null ? src.Client.FullName : string.Empty))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Client!.WhatsUpNumber))
                .ForMember(dest => dest.PackageTitle, opt => opt.MapFrom(src => src.Titles))
                .ReverseMap()
                .ForMember(dest => dest.Client, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
            #endregion

            #region Assignment (Exercises & Foods)
            CreateMap<AssignExercise, AssignExerciseDto>().ReverseMap();
            CreateMap<AssignFood, AssignFoodDto>()
                .ForMember(dest => dest.DayOfWeekName, opt => opt.MapFrom(src => src.DayOfWeek.ToString()))
                .ForMember(dest => dest.MealNumberName, opt => opt.MapFrom(src => src.MealNumber.ToString()))
                .ReverseMap();
            #endregion

            #region ExerciseSheetLog
            CreateMap<ExerciseSheetLog, ExerciseSheetLogDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client != null ? src.Client.FullName : string.Empty))
                .ForMember(dest => dest.ExerciseName, opt => opt.MapFrom(src => src.Exercise != null ? src.Exercise.Name : string.Empty));

            CreateMap<ExerciseSheetLogDto, ExerciseSheetLog>()
                .ForMember(dest => dest.Client, opt => opt.Ignore())
                .ForMember(dest => dest.Exercise, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<CreateExerciseSheetDto, ExerciseSheetLog>()
                .ForMember(dest => dest.Client, opt => opt.Ignore())
                .ForMember(dest => dest.Exercise, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
            #endregion

            #region Gallery
            CreateMap<HomeGallary, GalleryDto>().ReverseMap();
            CreateMap<GalleryDto, HomeGallary>().ReverseMap();
            CreateMap<CreateGalleryDto, HomeGallary>().ReverseMap();
            CreateMap<HomeGallary, ProfileImageDto>().ReverseMap();
            #endregion

            #region Cooking
            CreateMap<Cooking, Cookingdto>().ReverseMap();
            #endregion

            #region ExerciseNote
            CreateMap<ExerciseNotes, ExerciseNoteDto>().ReverseMap();
            CreateMap<CreateExerciseNoteDto, ExerciseNotes>().ReverseMap();
            #endregion

            #region Assigned Free Package
            CreateMap<CoachingPackageRequest, AssignedFreePackVM>()
                .ForMember(dest => dest.AssignedExercises, opt => opt.MapFrom(src => src.AssignExercises))
                .ForMember(dest => dest.AssignedFoods, opt => opt.MapFrom(src => src.AssignFoods));
            #endregion

            #region Book
            // Book mappings
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<CreateBookDto, Book>();

            // BookRequest mappings
            CreateMap<BookRequest, BookRequestDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client != null ? src.Client.FullName : string.Empty))
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : string.Empty));

            CreateMap<CreateBookRequestDto, BookRequest>();


            #endregion
        }
    }
}
