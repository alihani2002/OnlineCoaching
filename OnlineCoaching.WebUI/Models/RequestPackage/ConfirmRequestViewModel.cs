using OnlineCoaching.Domain.Common;

namespace OnlineCoaching.WebUI.Models.RequestPackage
{
    public class ConfirmRequestViewModel : BaseEntity
    {
        public int PackageId { get; set; }
        public string? PackageTitle { get; set; }
        public int PackagePrice { get; set; }
        public int DurationInMonths { get; set; }
        public int ClientId { get; set; } 
    }
}
