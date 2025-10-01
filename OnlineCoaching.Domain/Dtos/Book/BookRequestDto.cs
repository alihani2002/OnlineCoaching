namespace OnlineCoaching.Domain.Dtos
{
    public class BookRequestDto :BaseEntity
    {
        //public int Id { get; set; }
        //public string? BookName { get; set; }
        //public int TotalPrice { get; set; }
        public ClientStatus Status { get; set; }
        public bool IsApproved { get; set; }

        public int ClientId { get; set; }
        public string? ClientName { get; set; }  
        public int BookId { get; set; }
        public string? BookTitle { get; set; }   
    }
}
