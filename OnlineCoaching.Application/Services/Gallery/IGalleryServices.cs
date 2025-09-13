using OnlineCoaching.Domain.Dtos.Gallery;

namespace OnlineCoaching.Application.Services
{
    public interface IGalleryServices
    {
        IEnumerable<GalleryDto> GetImages();              
        GalleryDto GetImageById(int id);                  
        ProfileImageDto? GetProfileImage();               
        Task<CreateGalleryDto> AddImage(CreateGalleryDto gallery);
        GalleryDto EditImage(GalleryDto gallery);
        Task DeleteImage(int id);
    }
}
