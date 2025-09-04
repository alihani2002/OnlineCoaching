namespace OnlineCoaching.Application.Services
{
    public class ClientService(IUnitOfWork unitOfWork) : IClientService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Client?> GetClientAsync(string userId)
        {
            return await _unitOfWork.Clients.
                   GetQueryable()
                .Include(x => x.User!)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }


        public Client? GetClientById(int userId)
        {
            return _unitOfWork.Clients.GetById(userId);
        }


        public IEnumerable<Client?> GetAllClients() =>
             _unitOfWork.Clients.GetQueryable().Include(u=>u.User);



        public async Task CompleteClientData(Client client , string userId)
        {
            var existingClient = await _unitOfWork.Clients
                .GetQueryable()
                .Include(x => x.User!)
                .FirstOrDefaultAsync(c => c.UserId == client.UserId);

            if (existingClient == null)
            {
                // Create new client if not found
                existingClient = new Client
                {
                    UserId = userId,
                    FullName = client.FullName,
                    BirthDate = client.BirthDate,
                    PhoneNumber = client.PhoneNumber,
                    Address = client.Address,
                    CreatedOn = DateTime.UtcNow ,
                };
                _unitOfWork.Clients.Add(existingClient);
                var user = await _unitOfWork.Users
               .GetQueryable()
               .FirstOrDefaultAsync(u => u.Id == userId);

                if (user != null)
                {
                    user.IsCompelteProfile = true; 
                    user.LastUpdatedOn = DateTime.UtcNow;
                }
            }
            else
            {
                // Update existing client
                existingClient.FullName = client.FullName;
                existingClient.BirthDate = client.BirthDate;
                existingClient.PhoneNumber = client.PhoneNumber;
                existingClient.Address = client.Address;
                existingClient.LastUpdatedOn = DateTime.UtcNow;
            }
          
            _unitOfWork.Complete();
        }



        public async Task<bool> ToggleDeleteAsync(int clientId)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(clientId);

            if (client == null)
                return false;

            client.IsDeleted = !client.IsDeleted;
            client.LastUpdatedOn = DateTime.UtcNow;
            _unitOfWork.Complete();

            return true;
        }



    }
}
