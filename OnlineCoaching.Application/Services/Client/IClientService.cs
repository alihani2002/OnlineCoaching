namespace OnlineCoaching.Application.Services
{
    public interface IClientService 
    {
        Client? GetClientById(int userId);
        Task<Client?> GetClientAsync(string userId);
        IEnumerable<Client?> GetAllClients();
        Task CompleteClientData(Client client , string userId);
        Task<bool> ToggleDeleteAsync(int clientId);
        Task<Client?> GetClientByIdAsync(int userId); 
        Task<bool> UpdateClientAsync(Client client); 


    }
}
