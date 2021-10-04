using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.Core;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.IO;
using System.Reflection;

namespace BlogEngine.Core
{
    public class Helpers
    {
        public static string ReadAppSettings()
        {
            
            string Path = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName + @"\BlogEngineConfig.json";

            var jsonBytes = File.ReadAllBytes(Path);
            var Document = JsonDocument.Parse(jsonBytes);
            return Document.RootElement.GetProperty("ConnectionString").GetString();
        }
        public static void InitialSetup()
        {
            
            using (var Context = new BlogEngineDBContext())
            {
                //Context.Database.EnsureDeleted();
                Context.Database.EnsureCreated();
            }
            User.EnsureCurrentServerUserCreated();
            Role.EnsureRolesCreated();
            Role.EnsureActionsCreated();
            Role.EnsureRelationsRoleActionsCreated();
            CreateExampleData();
        }

        public static void CreateExampleData()
        {
            using (var Context = new BlogEngineDBContext())
            {
                Context.Users.Add(new User()
                {
                    Login = "editor1"
                });
                Context.Users.Add(new User()
                {
                    Login = "editor2"
                });
                Context.Users.Add(new User()
                {
                    Login = "writer1"
                });
                Context.Users.Add(new User()
                {
                    Login = "writer2"
                });

                Context.SaveChanges();

                var post1 = new Post()
                {
                    Title = "My new post published"
                    ,
                    Content = "Content of My new post published"
                    ,
                    PublishedDate = DateTime.Now
                    ,
                    Status = ePostStatus.Approved
                    ,
                    User = Context.Users.FirstOrDefault(u => u.Login == "editor1")
                };

                post1.Comments.Add(new PostComment()
                {
                    ItemId = 1
                    ,
                    Text = "Comment 1 from post 1"
                    ,
                    User = Context.Users.FirstOrDefault(u => u.Login == "editor1")
                });
                post1.Comments.Add(new PostComment()
                {
                    ItemId = 2
                    ,
                    Text = "Comment 2 from post 1"
                    ,
                    User = Context.Users.FirstOrDefault(u => u.Login == "editor2")
                });


                Context.Posts.Add(post1);

                var CurrentUser = Context.Users.FirstOrDefault(u=> u.Login.ToLower().Trim() == User.GetCurrentServerUserLogin().ToLower().Trim());

                Context.Posts.Add(new Post()
                {
                    Title = "My new post published 2"
                    ,
                    Content = "Content of My new post published 2"
                    ,
                    PublishedDate = DateTime.Now
                    ,
                    Status = ePostStatus.Approved
                    ,
                    User = CurrentUser
                });
                Context.Posts.Add(new Post()
                {
                    Title = "My new post published 3"
                    ,
                    Content = "Content of My new post published 3"
                    ,
                    PublishedDate = DateTime.Now
                    ,
                    Status = ePostStatus.Approved
                    ,
                    User = CurrentUser
                });
                Context.Posts.Add(new Post()
                {
                    Title = "My new post pending"
                    ,
                    Content = "Content of My new post pending"
                    ,
                    PublishedDate = DateTime.Now
                    ,
                    Status = ePostStatus.PendingApproval
                    ,
                    User = CurrentUser
                });
                Context.Posts.Add(new Post()
                {
                    Title = "My new post pending 2"
                    ,
                    Content = "Content of My new post pending 2"
                    ,
                    PublishedDate = DateTime.Now
                    ,
                    Status = ePostStatus.PendingApproval
                    ,
                    User = CurrentUser
                });
                Context.Posts.Add(new Post()
                {
                    Title = "My new post pending 3"
                    ,
                    Content = "Content of My new post pending 3"
                    ,
                    PublishedDate = DateTime.Now
                    ,
                    Status = ePostStatus.PendingApproval
                    ,
                    User = CurrentUser
                });

                Context.Posts.Add(new Post()
                {
                    Title = "My new post rejected"
                    ,
                    Content = "Content of My new post rejected"
                    ,
                    PublishedDate = DateTime.Now
                    ,
                    Status = ePostStatus.Rejected
                    ,
                    ReasonRejected = "Text describing reason why post was rejected"
                    ,
                    User = CurrentUser
                });
                Context.Posts.Add(new Post()
                {
                    Title = "My new post rejected 2"
                    ,
                    Content = "Content of My new post rejected 2"
                    ,
                    PublishedDate = DateTime.Now
                    ,
                    Status = ePostStatus.Rejected
                    ,
                    ReasonRejected = "Text describing reason why post was rejected"
                    ,
                    User = CurrentUser
                });
                Context.Posts.Add(new Post()
                {
                    Title = "My new post rejected 3"
                    ,
                    Content = "Content of My new post rejected 3"
                    ,
                    PublishedDate = DateTime.Now
                    ,
                    Status = ePostStatus.Rejected
                    ,
                    ReasonRejected = "Text describing reason why post was rejected"
                    ,
                    User = CurrentUser
                });




                Context.SaveChanges();

            }
        }


    }
}
