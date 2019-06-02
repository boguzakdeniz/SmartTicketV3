using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using SmartTicket.Business;
using SmartTicket.Business.Result;
using SmartTicket.DataAccess.EntityFramework;
using SmartTicket.Entities;
using SmartTicket.Entities.ViewModels;
using SmartTicket.WebAppUI.Models.ViewModels;

namespace SmartTicket.WebAppUI.Controllers
{
    public class AccountController : Controller
    {
        UserManager us = new UserManager();

        [AllowAnonymous]
        public ActionResult Login()
        {
            //Bakılacak burası nasıl olacak
            if (string.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                FormsAuthentication.SignOut();
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            
            if (ModelState.IsValid)
            {
                return View(model);
              

            }

            BusinessLayerResult<User> res = us.Login(model);
            if (res.Errors.Count > 0)
            {
                res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                return View(model);
            }

            if (res.Result.Role==Role.Editor)
            {
                return RedirectToAction("Index", "Activities", new { area = res.Result.Role.ToString() });
            }
            return RedirectToAction("Index", "Home", new { area = res.Result.Role.ToString() });


        }
      

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User model)
        {
            ModelState.Remove("CreateDate");
            ModelState.Remove("CreatedBy");
            ModelState.Remove("UpdateDate");
            ModelState.Remove("UpdatedBy");
            
            if (ModelState.IsValid)
            {
                BusinessLayerResult<User> res = us.Register(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                us.Insert(res.Result);
                //ConfirmEmail(res.Result.Id);
                return RedirectToAction("Index", "Home", new { area = res
                .Result.Role.ToString()});
                
            }
            return View();

        }
        public ActionResult ForgetPassword(User model)
        {
            return View(model);
        }

        public ActionResult ConfirmEmail(Guid guid)
        {
            {
                BusinessLayerResult<User> res = us.ConfirmEmail(guid);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Title = "Geçersiz İşlem",
                        Items = res.Errors
                    };

                    return View("Error", errorNotifyObj);
                }

                OkViewModel okNotifyObj = new OkViewModel()
                {
                    Title = "Hesap Aktifleştirildi",
                    RedirectingUrl = "/Account/Login"
                };

               

                return View("Ok", okNotifyObj);
            }
        }

    
        
      

    }
}