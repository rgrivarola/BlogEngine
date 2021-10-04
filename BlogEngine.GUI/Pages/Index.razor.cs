using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlogEngine.Core;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;

namespace BlogEngine.GUI.Pages
{
    public partial class Index : PageBase
    {
        Post NewPost;

        List<Post> AllPosts;
        
        public Index()
        {

        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            NewPost = new Post();

            AllPosts = await BackendService.GetFromURL <List<Post>>("api/post"); //await Initialize<List<Post>>(Url);

            
        }
        void GUIBtnDetails_Click(int PostId)
        {
            try
            {
            BackendService.NavigateTo($"/posts/{PostId.ToString()}");

            }
            catch (Exception ex)
            {
                ShowMessage(ex);
            }
            StateHasChanged();
        }

        async Task GUIBtnSubmitPost_Click()
        {
            try
            {
                
                await BackendService.PostURL<Post>($"api/post", NewPost);
                NewPost = new Post();
            }
            catch (Exception ex)
            {
                ShowMessage(ex);
            }
            StateHasChanged();
        }
    }
}
