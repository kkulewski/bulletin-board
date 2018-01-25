namespace BulletinBoard.Models.ErrorViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public string Response { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}