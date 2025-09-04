using OnlineCoaching.Domain.Enums;

namespace OnlineCoaching.Application.Services
{
    public class CoachingPackageRequestService(IUnitOfWork unitOfWork, IMapper mapper) : ICoachingPackageRequestService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        public IEnumerable<CoachingPackageRequestDto> GetRequests()
        {
            var requests = _unitOfWork.CoachingPackageRequests.GetQueryable().Include(u=>u.Client)
                .Where(r => !r.IsDeleted)
                .ToList();

            return _mapper.Map<IEnumerable<CoachingPackageRequestDto>>(requests);
        }



        public async Task<CoachingPackageRequestDto?> GetUserRequest(int clientId)
        {
            var request = await _unitOfWork.CoachingPackageRequests.GetQueryable()
                .Include(u => u.Client)
                .FirstOrDefaultAsync(r => !r.IsDeleted && r.ClientId == clientId);
            if (request == null) return null;

            return _mapper.Map<CoachingPackageRequestDto>(request);
        }

      

        public async Task<CoachingPackageRequestDto?> GetRequestByIdAsync(int id)
        {
            var request = await _unitOfWork.CoachingPackageRequests.GetQueryable()
            .Include(r => r.Client)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
            if (request == null || request.IsDeleted) return null;

            return _mapper.Map<CoachingPackageRequestDto>(request);
        }


       
        public async Task<CoachingPackageRequest> CreateRequestAsync(int packageId, int clientId)
        {
            var package = await _unitOfWork.CoachingPackages.GetByIdAsync(packageId);
            if (package == null || package.IsDeleted)
                throw new Exception("Package not found");

            var existingActive = _unitOfWork.CoachingPackageRequests
                .GetAll()
                .FirstOrDefault(r =>
                    r.PackageId == packageId &&
                    r.ClientId == clientId &&
                    r.Status == ClientStatus.Active &&
                    !r.IsDeleted);

            if (existingActive != null)
                return existingActive;

            var request = new CoachingPackageRequest
            {
                PackageId = packageId,
                ClientId = clientId,
                Titles = package.Title,
                Status = ClientStatus.Pending,
                Price = package.Price,
                IsAnswerQuestion = false,
                CreatedOn = DateTime.UtcNow,
                CreatedById = package.CreatedById,
            };

            var added = await _unitOfWork.CoachingPackageRequests.AddAsync(request);
            _unitOfWork.Complete();

            return _mapper.Map<CoachingPackageRequest>(added);
        }


        public async Task<CoachingPackageRequestDto?> ManageRequestStatusAsync(int requestId, ClientStatus newStatus)
        {
            var request = await _unitOfWork.CoachingPackageRequests.GetByIdAsync(requestId);
            if (request == null || request.IsDeleted) return null;

            request.Status = newStatus;

            if (newStatus == ClientStatus.Active)
            {
                var package = await _unitOfWork.CoachingPackages.GetByIdAsync(request.PackageId);
                if (package != null)
                {
                    request.StartDate = DateTime.UtcNow;
                    request.EndDate = DateTime.UtcNow.AddMonths(package.DurationInMonths);
                }
            }
            else if (newStatus == ClientStatus.Suspended)
            {
                request.EndDate = DateTime.UtcNow; 
            }

            request.LastUpdatedOn = DateTime.UtcNow;

            _unitOfWork.CoachingPackageRequests.Update(request);
            _unitOfWork.Complete();

            return _mapper.Map<CoachingPackageRequestDto>(request);
        }


        public async Task<CoachingPackageRequestDto?> UpdateStatusAsync(int id, ClientStatus newStatus)
        {
            var existing = await _unitOfWork.CoachingPackageRequests.GetByIdAsync(id);
            if (existing == null || existing.IsDeleted) return null;

            existing.Status = newStatus;
            if (existing.Status == ClientStatus.Active)
            {
                existing.StartDate = DateTime.UtcNow;
            }
            existing.LastUpdatedOn = DateTime.UtcNow;

            _unitOfWork.CoachingPackageRequests.Update(existing);
            _unitOfWork.Complete();

            return _mapper.Map<CoachingPackageRequestDto>(existing);
        }



        public async Task<bool> DeleteRequestAsync(int id)
        {
            var request = await _unitOfWork.CoachingPackageRequests.GetByIdAsync(id);
            if (request == null || request.IsDeleted) return false;

            request.IsDeleted = true;
            _unitOfWork.CoachingPackageRequests.Update(request);
            _unitOfWork.Complete();

            return true;
        }


        public CoachingPackageRequestDto? GetActiveOrPendingRequest(int clientId)
        {
            var request = _unitOfWork.CoachingPackageRequests
                .GetQueryable().Include(x => x.Client )
                .FirstOrDefault(r =>
                    r.ClientId == clientId &&
                    (r.Status == ClientStatus.Pending || r.Status == ClientStatus.Active) &&
                    !r.IsDeleted);

            return _mapper.Map<CoachingPackageRequestDto?>(request);
        }



        public void CheckExpiredSubscriptions()
        {
            var today = DateTime.Now;

            var requests = _unitOfWork.CoachingPackageRequests
                            .GetAll()
                            .Where(r => !r.IsDeleted && r.Status == ClientStatus.Active)
                            .ToList();

            foreach (var request in requests)
            {
                if (request.EndDate.HasValue && request.EndDate.Value.Date < today.Date)
                {
                    request.Status = ClientStatus.Suspended;
                    request.LastUpdatedOn = DateTime.UtcNow;
                    _unitOfWork.CoachingPackageRequests.Update(request);
                }
            }

            _unitOfWork.Complete();
        }
    }
}
