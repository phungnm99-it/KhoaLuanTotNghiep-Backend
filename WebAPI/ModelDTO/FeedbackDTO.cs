using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.ModelDTO
{
    public class FeedbackDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public DateTime FeedbackTime { get; set; }
        public bool IsReplied { get; set; }
        public string ReplyContent { get; set; }
        public DateTime ReplyTime { get; set; }
        public string ReplierName { get; set; }

    }
}
