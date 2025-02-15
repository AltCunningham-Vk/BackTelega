namespace BackTelega.Models.DTOs
{
    public class SendMessageRequest
    {
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public string? MediaType { get; set; }
        public string? MediaUrl { get; set; }
    }

    public class MessageResponse
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public string MessageText { get; set; } = string.Empty;
        public string? MediaType { get; set; }
        public string? MediaUrl { get; set; }
        public DateTime SentAt { get; set; }
    }
}
