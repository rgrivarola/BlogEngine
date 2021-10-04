using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.Core;
using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace BlogEngine.GUI.Pages
{
    public partial class PostDetail : PageBase
    {
        [Parameter]
        public string Id { get; set; }

        string TextReasonRejected = "";

        Post Post;

        string NewCommentText;

        public PostDetail()
        {

        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await base.OnInitializedAsync();
                Post = await BackendService.GetFromURL<Post>("api/post/" + Id);
                TextReasonRejected = Post.ReasonRejected;
            }
            catch (Exception ex)
            {
                ShowMessage(ex);
            }


        }
        public async void GUIBtnPostComment_Click()
        {
            try
            {
                int MaxId = 1;
                using (var Context = new BlogEngineDBContext())
                {
                    var tmp = Context.PostComments.Where(p => p.PostId == Post.Id);
                    if (tmp.Any())
                        MaxId = tmp.Max(x => x.ItemId) + 1;

                }

                var NewComment = new PostComment()
                {
                    ItemId = MaxId,
                    PostId = Post.Id,
                    Text = NewCommentText,
                    User = new User() { Login = User.GetCurrentServerUserLogin() }
                };

                await BackendService.PostURL<PostComment>($"api/post/{Post.Id}/comments", NewComment);

                NewCommentText = "";
            }
            catch (Exception ex)
            {
                ShowMessage(ex);
            }
            StateHasChanged();
        }

        public async void GUIBtnChangePostStatus_Click(bool NewPostStatus)
        {
            try
            {
                if (NewPostStatus)
                {
                    await BackendService.PutURL<string>($"api/post/{Post.Id}/aproove", null);
                }
                else
                {
                    await BackendService.PutURL<string>($"api/post/{Post.Id}/reject", TextReasonRejected);
                }

            }
            catch (Exception ex)
            {
                ShowMessage(ex);
            }
            StateHasChanged();
        }

    }
}
