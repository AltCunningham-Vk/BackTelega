using System;
using System.Collections.Generic;

namespace BackTelega.Models;

public partial class Message
{
    public int Id { get; set; }

    public int? ChatId { get; set; }

    public int? SenderId { get; set; }

    public string? MessageText { get; set; }

    public string? MediaType { get; set; }

    public string? MediaUrl { get; set; }

    private DateTime _sentAt = DateTime.UtcNow;
    public DateTime SentAt
    {
        get => _sentAt;
        set => _sentAt = DateTime.SpecifyKind(value, DateTimeKind.Unspecified);
    }
    public virtual Chat? Chat { get; set; }

    public virtual User? Sender { get; set; }
}
