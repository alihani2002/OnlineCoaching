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
        public List<CoachingPackage> GetPackageFree()
        {
            var entity = _unitOfWork.CoachingPackages
               .GetQueryable()
               .Include(r => r.AssignExercises!).ThenInclude(a => a.Exercise)
               .Include(r => r.AssignFoods!).ThenInclude(a => a.Food)
               .Where(C =>  C.IsFreePlan)
               .ToList();
            return entity;
        }
        public IEnumerable<CoachingPackageDto> GetCoachingPackages()
            {
                var packages = _unitOfWork.CoachingPackages.GetQueryable()
               .Where(C => !C.IsFreePlan)
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
                package.CreatedOn = DateTime.Now;

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

        public async Task<bool> RestoreCoachingPackageAsync(int id)
        {
            var package = await _unitOfWork.CoachingPackages
                .GetQueryable()
                .FirstOrDefaultAsync(p => p.Id == id && p.IsFreePlan);

            if (package == null) return false;
            if (!package.IsDeleted) return true; // Already restored/not deleted

            package.IsDeleted = false;
            package.LastUpdatedOn = DateTime.UtcNow;

            _unitOfWork.CoachingPackages.Update(package);
            _unitOfWork.Complete();

            return true;
        }
    }
    }
