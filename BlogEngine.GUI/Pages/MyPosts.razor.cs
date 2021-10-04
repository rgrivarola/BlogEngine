using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.Core;

namespace BlogEngine.GUI.Pages
{
    public partial class MyPosts : PageBase
    {
        List<Post> AllMyPosts;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //AllMyPosts = await BackendService.GetFromURL<List<Post>>("api/post/user"); //await Initialize<List<Post>>(Url);
            AllMyPosts = await BackendService.GetFromURL<List<Post>>("api/post/user");

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

    }
}
