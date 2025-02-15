namespace BackTelega.Models.DTOs
{
    public class CreateChatRequest
    {
        public string ChatName { get; set; } = string.Empty;
        public string ChatType { get; set; } = string.Empty;
    }

    public class ChatResponse
    {
        public int Id { get; set; }
        public string ChatName { get; set; } = string.Empty;
        public string ChatType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
