using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BlogEngine.Core
{
    public class Post : DBEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ReasonRejected { get; set; }

        public DateTime PublishedDate { get; set; }
        public ePostStatus Status { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<PostComment> Comments { get; set; }

        public Post()
        {
            this.Comments = new List<PostComment>();
        }
        
    }
}
