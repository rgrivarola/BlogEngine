using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogEngine.Core
{
    public class User : DBEntity
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public virtual ICollection<Role> Roles { get; set; }

        public User()
        {
            this.Roles = new List<Role>();
        }

        public bool HasRole(eRoles Role)
        {
            return Roles.Any(x=> x.Name == Role);
        }

        public static void EnsureCurrentServerUserCreated()
        {
            using (var Context = new BlogEngineDBContext())
            {
                string LoginCurrentUser = GetCurrentServerUserLogin();

                User CurrentUser = Context.Users.FirstOrDefault(u => u.Login.ToLower().Trim() == LoginCurrentUser.ToLower().Trim());

                //Just to easy testing we insert current user if not exists in database
                if (CurrentUser == null)
                {
                    CurrentUser = new User() { Login = LoginCurrentUser };
                    Context.Users.Add(CurrentUser);
                    Context.SaveChanges();
                }
                
            }
        }

        public static string GetCurrentServerUserLogin()
        {
            return String.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName);
        }
        public static User GetCurrentServerUser()
        {
            using (var Context = new BlogEngineDBContext())
                return Context.Users.FirstOrDefault(u => u.Login.ToLower().Trim() == GetCurrentServerUserLogin().ToLower().Trim());
        }
    }
}
