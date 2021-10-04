using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Core
{
    public class PostComment : DBEntity
    {
        public int PostId { get; set; }
        public int ItemId { get; set; }
        public string Text { get; set; }
        public virtual User User { get; set; }
    }
}
