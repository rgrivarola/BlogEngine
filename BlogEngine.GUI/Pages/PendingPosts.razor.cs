using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.Core;
using BlogEngine.GUI;

namespace BlogEngine.GUI.Pages
{
    public partial class PendingPosts : PageBase
    {
        List<Post> AllPendingPosts;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //AllMyPosts = await BackendService.GetFromURL<List<Post>>("api/post/user"); //await Initialize<List<Post>>(Url);
            AllPendingPosts = await BackendService.GetFromURL<List<Post>>("api/post/pending");

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
