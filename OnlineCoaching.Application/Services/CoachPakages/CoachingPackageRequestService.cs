
using OnlineCoaching.Domain.Enums;

namespace OnlineCoaching.Application.Services
{
    public class CoachingPackageRequestService : ICoachingPackageRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoachingPackageRequestService(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
        }
        public void CheckExpiredSubscriptions()
        {
            var today = DateTime.UtcNow;

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
