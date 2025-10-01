namespace OnlineCoaching.Domain.Entities
{
    public class BookRequest : BaseEntity
    {
        public string? BookName { get; set; }
        //public int QuantityNumber { get; set; }
        //public int TotalPrice { get; set; } = 0 ;
        public ClientStatus Status { get; set; } = ClientStatus.Pending;
        public bool IsApproved { get; set; } = false;

        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
    
    }
}
