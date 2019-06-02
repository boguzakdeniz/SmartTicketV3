using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartTicket.WebAppUI.Models.ViewModels
{
    public class OkViewModel : NotifyViewModelBase<string>
    {
        public OkViewModel()
        {
            Title = "İşlem Başarılı.";
        }
    }
}