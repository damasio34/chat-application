using System;
using System.ComponentModel.DataAnnotations;

namespace ChatApplication.Core.Domain
{
    public class Message
    {
        public string Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Room { get; set; }
        public DateTime? When { get; set; }
    }
}
