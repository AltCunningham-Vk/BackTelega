﻿using System;

namespace BackTelega.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified); // Исправление

        public ICollection<ChatMember>? ChatMembers { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
