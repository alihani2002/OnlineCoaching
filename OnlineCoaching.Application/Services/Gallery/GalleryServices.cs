using OnlineCoaching.Domain.Dtos.Gallery;
using OnlineCoaching.Domain.Entities.Gallery;

namespace OnlineCoaching.Application.Services
{
    public class GalleryServices : IGalleryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GalleryServices(IUnitOfWork unitOfWork , IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<GalleryDto> GetImages()
        {
            var images = _unitOfWork.Images.GetQueryable().Where(i => !i.IsDeleted).ToList();
            return _mapper.Map<IEnumerable<GalleryDto>>(images);
        }

        public GalleryDto GetImageById(int id)
        {
            var entity = _unitOfWork.Images.GetById(id);
            return _mapper.Map<GalleryDto>(entity);
        }

        public ProfileImageDto? GetProfileImage()
        {
            var profile = _unitOfWork.Images
                .GetQueryable()
                .Where(i => !i.IsDeleted && i.CoachImage != null)
                .FirstOrDefault();

            return profile == null ? null : _mapper.Map<ProfileImageDto>(profile);
        }

        public async Task<CreateGalleryDto> AddImage(CreateGalleryDto gallery)
        {
            var entity = _mapper.Map<HomeGallary>(gallery);
            await _unitOfWork.Images.AddAsync(entity);
            _unitOfWork.Complete();
            return gallery;
        }

        public GalleryDto EditImage(GalleryDto gallery)
        {
            var entity = _unitOfWork.Images.GetById(gallery.Id);
            if (entity == null) throw new Exception("Image not found");

            entity.Image = gallery.Image;
            _unitOfWork.Images.Update(entity);
            _unitOfWork.Complete();
            return _mapper.Map<GalleryDto>(entity);
        }

        public async Task DeleteImage(int id)
        {
            var entity = await _unitOfWork.Images.GetByIdAsync(id);
            if (entity == null) throw new Exception("Image not found");

            entity.IsDeleted = true;
            _unitOfWork.Images.Update(entity);
            _unitOfWork.Complete();
        }
    }
}
