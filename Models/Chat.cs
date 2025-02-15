using System;
using System.Collections.Generic;

namespace BackTelega.Models;

public partial class Chat
{
    public int Id { get; set; }

    public string ChatName { get; set; } = null!;

    public string? ChatType { get; set; }
    //public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

    public virtual ICollection<ChatMember> ChatMembers { get; set; } = new List<ChatMember>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
