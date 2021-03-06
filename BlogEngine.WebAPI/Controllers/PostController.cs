using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogEngine.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly BlogEngineDBContext _Context;

        public PostController(BlogEngineDBContext Context)
        {
            _Context = Context;
        }
        // GET: api/<PostController>
        [HttpGet]
        public IActionResult GetAllPublishedPosts()
        {
            try
            {
                return Ok(_Context.Posts.Where(p=> p.Status == ePostStatus.Approved));

            }
            catch (Exception ex)
            {
                return HandleErrors(ex);
            }



        }

        [HttpPost("{PostId}/comments")]
        public void AddCommentToPublishedPost(int PostId, [FromBody] PostComment value)
        {
         
            try
            {
                if (PostId != value.PostId)
                    throw new Exception("PostId and PostId from comments are different");

                Post Post = _Context.Posts.FirstOrDefault(p=> p.Id == PostId);
                if (Post == null)
                    throw new Exception($"Post {PostId} not found");
                else if (Post.Status != ePostStatus.Approved)
                    throw new Exception($"Post {PostId} is not published");

                Post.Comments.Add(value);
                _Context.SaveChanges();
            }
            catch (Exception ex)
            {
                HandleErrors(ex);
            }




        }

        [HttpGet("user")]
        public IActionResult GetOwnPosts()
        {
            try
            {
                User CurrentUser = GetCurrentUser();

                return Ok(_Context.Posts.Where(p => p.User.Id == CurrentUser.Id));
            }
            catch (Exception ex)
            {
                return HandleErrors(ex);
            }


        }

        [HttpGet("pending")]
        public IActionResult GetPendingPosts()
        {
            try
            {
                ValidateUserInRole(eRoles.Editor);

                User CurrentUser = GetCurrentUser();

                return Ok(_Context.Posts.Where(p => p.Status == ePostStatus.PendingApproval));
            }
            catch (Exception ex)
            {
                return HandleErrors(ex);
            }


        }


        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_Context.Posts.FirstOrDefault(p => p.Id == id));

            }
            catch (Exception ex)
            {
                return HandleErrors(ex);
            }

        }
                
        [HttpPost()]
        public void SubmitPost([FromBody] Post value)
        {
            try
            {
                ValidateUserInRole(eRoles.Writer);
                        
                value.PublishedDate = DateTime.Now;
                value.Status = ePostStatus.PendingApproval;
                value.User = GetCurrentUser();
                _Context.Posts.Add(value);
                _Context.SaveChanges();
            }
            catch (Exception ex)
            {
                HandleErrors(ex);
            }


        }

        

        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Post value)
        {
            try
            {
                ValidateUserInRole(eRoles.Writer);
            
                if (id != value.Id)
                    throw new Exception("Id and PostId are different");

                Post PostEdited = _Context.Posts.FirstOrDefault(p=> p.Id == id);
                if (PostEdited.Status == ePostStatus.Approved)
                    throw new Exception("Youn cannot edit published posts");

            
                PostEdited = value;
                _Context.SaveChanges();
            }
            catch (Exception ex)
            {
                HandleErrors(ex);
            }


        }
        [HttpPut("{id}/aproove")]
        public void AproovePost(int id)
        {
            try
            {
                ValidateUserInRole(eRoles.Editor);

                Post PostEdited = _Context.Posts.FirstOrDefault(p => p.Id == id);
                if (PostEdited.Status == ePostStatus.Approved)
                    throw new Exception("Post alredy approved");

                PostEdited.Status = ePostStatus.Approved;
                PostEdited.ReasonRejected = null;
                _Context.SaveChanges();
            }
            catch (Exception ex)
            {
                HandleErrors(ex);
            }


        }

        [HttpPut("{id}/reject")]
        public void RejectPost(int id,[FromBody] string reasonRejected)
        {
            try
            {
                ValidateUserInRole(eRoles.Editor);

                Post PostEdited = _Context.Posts.FirstOrDefault(p => p.Id == id);
                if (PostEdited.Status == ePostStatus.Rejected)
                    throw new Exception("Post alredy rejected");

                PostEdited.Status = ePostStatus.Rejected;
                PostEdited.ReasonRejected = reasonRejected;
                _Context.SaveChanges();
            }
            catch (Exception ex)
            {
                HandleErrors(ex);
            }


        }


        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }





















        public User GetCurrentUser()
        {
            return _Context.Users.FirstOrDefault(u => u.Login.ToLower().Trim() == GetCurrentClientUserLogin().ToLower().Trim());
            
        }
        public string GetCurrentClientUserLogin()
        {
            if (HttpContext != null && HttpContext.User != null && HttpContext.User.Identity != null)
                return HttpContext.User.Identity.Name;
            else
                return "";
        }
        public bool IsUserLoggedIn()
        {
            return !String.IsNullOrWhiteSpace(GetCurrentClientUserLogin());
        }

        public void ValidateUserInRole(eRoles Role)
        {
            var CurrentUser = GetCurrentUser();
            if (CurrentUser == null)
            {
                throw new Exception($"User {GetCurrentClientUserLogin()} doesn´t exists");
            }
            else if (!CurrentUser.HasRole(Role))
                throw new Exception($"User {GetCurrentClientUserLogin()} doesn´t have {Role.ToString()} role, necessary to perform this operation");
        }


        ObjectResult HandleErrors(Exception ex)
        {
            return StatusCode(500, new { ErrorCode = 500, ErrorMessage = ex.Message });
        }
    }
}
