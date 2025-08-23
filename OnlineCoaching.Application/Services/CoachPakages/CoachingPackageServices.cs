namespace OnlineCoaching.Application.Services
{
    public class CoachingPackageServices : ICoachingPackageServices
    {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public CoachingPackageServices(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public IEnumerable<CoachingPackageDto> GetCoachingPackages()
            {
                var packages = _unitOfWork.CoachingPackages.GetAll()
                                    .Where(p => !p.IsDeleted)
                                    .ToList();
                return _mapper.Map<IEnumerable<CoachingPackageDto>>(packages);
            }

            public async Task<CoachingPackageDto?> GetCoachingPackageByIdAsync(int id)
            {
                var package = await _unitOfWork.CoachingPackages.GetByIdAsync(id);
                if (package == null || package.IsDeleted) return null;

                return _mapper.Map<CoachingPackageDto>(package);
            }

            public async Task<CoachingPackageDto> AddCoachingPackageAsync(CreateCoachingPackageDto dto)
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                    throw new ValidationException("Package title is required.");

                var package = _mapper.Map<CoachingPackage>(dto);
                package.CreatedOn = DateTime.UtcNow;

                var addedPackage = await _unitOfWork.CoachingPackages.AddAsync(package);
                _unitOfWork.Complete();

                return _mapper.Map<CoachingPackageDto>(addedPackage);
            }

            public async Task<CoachingPackageDto?> UpdateCoachingPackageAsync(CoachingPackageDto dto)
            {
                var existing = await _unitOfWork.CoachingPackages.GetByIdAsync(dto.Id);
                if (existing == null || existing.IsDeleted) return null;

                _mapper.Map(dto, existing);
                existing.LastUpdatedOn = DateTime.UtcNow;

                _unitOfWork.CoachingPackages.Update(existing);
                _unitOfWork.Complete();

                return _mapper.Map<CoachingPackageDto>(existing);
            }

            public async Task<bool> DeleteCoachingPackageAsync(int id)
            {
                var package = await _unitOfWork.CoachingPackages.GetByIdAsync(id);
                if (package == null || package.IsDeleted) return false;

                package.IsDeleted = true;
                package.LastUpdatedOn = DateTime.UtcNow;

                _unitOfWork.CoachingPackages.Update(package);
                _unitOfWork.Complete();

                return true;
            }
        }
    }
