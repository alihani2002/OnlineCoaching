namespace OnlineCoaching.Application.Services
{
    public interface IClientService 
    {
        Task<Client?> GetClientAsync(string userId);
        IEnumerable<Client?> GetAllClients();
        Task CompleteClientData(Client client , string userId);

    }
}
