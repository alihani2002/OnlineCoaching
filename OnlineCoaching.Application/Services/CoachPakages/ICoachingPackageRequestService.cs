using OnlineCoaching.Domain.Enums;

namespace OnlineCoaching.Application.Services
{
    public interface ICoachingPackageRequestService
    {
        Task<CoachingPackageRequestDto?> GetUserRequest(int clientId);
        CoachingPackageRequestDto? GetActiveOrPendingRequest(int clientId);
        Task<CoachingPackageRequest> CreateRequestAsync(int packageId, int clientId);

        Task<CoachingPackageRequestDto?> ManageRequestStatusAsync(int requestId, ClientStatus newStatus);
        IEnumerable<CoachingPackageRequestDto> GetRequests();
        Task<CoachingPackageRequestDto?> GetRequestByIdAsync(int id);
        Task<CoachingPackageRequestDto?> UpdateStatusAsync(int id, ClientStatus newStatus);
        Task<bool> DeleteRequestAsync(int id);
        public void CheckExpiredSubscriptions();

    }
}
