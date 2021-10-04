using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Core
{
    public class Action : DBEntity
    {
        public int Id { get; set; }
        public eActions Name { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

        public Action()
        {
            this.Roles = new List<Role>();
        }
    }
}
