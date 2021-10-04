using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Text.Json;

namespace BlogEngine.GUI
{

    

    public class PageBase : ComponentBase
    {
        
        public PageBase()
        {
            
        }
        public PageMessages Messages;

        public void ShowMessage(Exception ex)
        {
            if (Messages != null)
            {
                Messages.ClearAllMessages();
                Messages.ErrorMessages.Add(ex.Message);

            }
        }
    }


}
