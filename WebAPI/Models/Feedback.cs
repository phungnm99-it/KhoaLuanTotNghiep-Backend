using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public DateTime FeedbackTime { get; set; }
        public bool? IsReplied { get; set; }
        public string ReplyContent { get; set; }
        public DateTime ReplyTime { get; set; }
        public int? RepliedBy { get; set; }

        public virtual User RepliedByNavigation { get; set; }
    }
}
