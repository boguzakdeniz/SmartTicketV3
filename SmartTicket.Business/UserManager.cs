
using SmartTicket.Business.Result;
using SmartTicket.Common.Helpers;
using SmartTicket.Entities;
using SmartTicket.Entities.Messages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace SmartTicket.Business
{
    public class UserManager:ManagerBase<User>
    {
        //MailHelper
        
        public BusinessLayerResult<User> Login(User model)
        {
            BusinessLayerResult<User> res = new BusinessLayerResult<User>();
            res.Result = Find(a => a.Username == model.Username);
            if (res.Result==null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı");
            }
            else if (res.Result.Password!=model.Password)
            {
                res.AddError(ErrorMessageCode.BadPassword, "Hatalı şifre");
            }
            return res;
        }

        public BusinessLayerResult<User> Register(User model)
        {
            BusinessLayerResult<User> res = new BusinessLayerResult<User>();

            res.Result = model;
          

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.RegisterNull, "Kayıt yapılamadı.");
            }
            else if (res.Result==Find(x=>x.Username==model.Username))
            {
                res.AddError(ErrorMessageCode.UsernameIsUsed, "Kullanıcı adı mevcut."); 
            }
            else if (res.Result == Find(x => x.Mail == model.Mail))
            {
                res.AddError(ErrorMessageCode.MailIsUsed, "E-Posta mevcut.");
            }
            else if (res.Result == Find(x => x.Phone == model.Phone))
            {
                res.AddError(ErrorMessageCode.PhoneIsUsed, "Telefon numarası mevcut.");
            }
            res.Result.Role = Role.User;
            res.Result.State = State.PendingApproval;
            res.Result.ActivedGuid = Guid.NewGuid();

            var guid = res.Result.ActivedGuid;
            var url = "http://localhost:50276/" + "Account/ConfirmEmail/" + guid;

            string body = "Lütfen hesabınızı onaylamak için <a href=\"" + url +"\">linke tıklayınız.</a><br/>";
            //string body = "Onay Kodunuz : " + res.Result.ActivedGuid + url;
          
            var to = res.Result.Mail;
            var subject = "E-Posta Aktivasyonu";

            MailHelper.SendMail(body,to,subject);

        

            return res;

        }

        public BusinessLayerResult<User> ConfirmEmail (Guid guid)
        {
            BusinessLayerResult<User> res = new BusinessLayerResult<User>();
            res.Result = Find(x => x.ActivedGuid == guid);

            if (res.Result != null)
            {
                if (res.Result.State==State.Approved)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.State = State.Approved;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı.");
            }

            return res;
        }




    }
}
