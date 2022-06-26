using System;
using System.ComponentModel.DataAnnotations;

namespace ChatApplication.Core.Domain
{
    public class ChatMessage
    {
        public string Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Room { get; set; }
        [Required]
        public string ConnectionId { get; set; }
        public MessageCode MessageCode { get; set; }
        public DateTime? When { get; set; }
    }
}
