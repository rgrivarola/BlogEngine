using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;

namespace BlogEngine.Core
{
    public class Role : DBEntity
    {
        public int Id { get; set; }
        public eRoles Name { get; set; }
        public virtual ICollection<Action> Actions { get; set; }
        public virtual ICollection<User> Users { get; set; }
                
        public Role()
        {
            this.Actions = new List<Action>();
            this.Users = new List<User>();
        }
        public static void EnsureRolesCreated()
        {
            using (var Context = new BlogEngineDBContext())
            {
                foreach (var Item in Enum.GetValues(typeof(eRoles)))
                {
                    var Role = Context.Roles.FirstOrDefault(r=> r.Name == (eRoles) Item);
                    if (Role == null)
                    {
                        Role = new Role() { Name = (eRoles)Item };
                        Context.Roles.Add(Role);
                    }
                }
                Context.SaveChanges();
            }
        }

        public static void EnsureActionsCreated()
        {
            
            using (var Context = new BlogEngineDBContext())
            {
                foreach (var Item in Enum.GetValues(typeof(eActions)))
                {
                    var Action = Context.Actions.FirstOrDefault(a => a.Name == (eActions)Item);
                    if (Action == null)
                    {
                        Action = new Action() { Name = (eActions)Item };
                        Context.Actions.Add(Action);
                    }
                }
                Context.SaveChanges();
            }
        }

        public static void EnsureRelationsRoleActionsCreated()
        {
                        
            using (var Context = new BlogEngineDBContext())
            {
                var WriterRole = Context.Roles.Include(r=> r.Actions).FirstOrDefault(r => r.Name == eRoles.Writer);
                foreach (Action Item in Context.Actions.Where(a => a.Name == eActions.CreatePost || a.Name == eActions.EditPost))
                {
                    if (!WriterRole.Actions.Any(a=> a.Name == Item.Name))
                        WriterRole.Actions.Add(Item);
                }
                    

                var EditorRole = Context.Roles.Include(r => r.Actions).FirstOrDefault(r => r.Name == eRoles.Editor);
                foreach (Action Item in Context.Actions.Where(a => a.Name == eActions.ChangePostStatus))
                {
                    if (!EditorRole.Actions.Any(a => a.Name == Item.Name))
                        EditorRole.Actions.Add(Item);
                }

                Context.SaveChanges();

            }
        }
    }
}
