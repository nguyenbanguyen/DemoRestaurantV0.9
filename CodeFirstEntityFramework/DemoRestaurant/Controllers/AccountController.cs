using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DemoRestaurant.Models;
using System.Web.Security;
using DemoRestaurant.DAL;
using System.Net.Mail;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.NetworkInformation;

namespace DemoRestaurant.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        RestaurantDemoContext ResDb = new RestaurantDemoContext();
        private void MigrateShoppingCart(string UserName)
        {
            // Associate shopping cart items with logged-in user
            var cart = ShoppingCart.GetCart(this.HttpContext);
            
            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // remote check UserName
        public JsonResult doesUserNameExist(string UserName)
        {

            var user = Membership.GetUser(UserName);

            return Json(user == null);
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            // cần hoàn thiện function user logs sau

          //  var browser = Request.Browser;
          //  var ip = Request.UserHostAddress;
          //  string ip2=null;
          //  string device = Request.UserAgent;
          //  string Platform = browser.Platform;
          //  string name = browser.Browser;
          //  string marjoversion = browser.MajorVersion.ToString();
          //  string minoversion = browser.MinorVersion.ToString();
          //  var x1 = browser.IsMobileDevice;
          //  HttpContext context = System.Web.HttpContext.Current;
          //  string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
          //  string GetMAC = null;
          //{
          //      string macAddresses = "";

          //      foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
          //      {
          //          if (nic.OperationalStatus == OperationalStatus.Up)
          //          {
          //              macAddresses += nic.GetPhysicalAddress().ToString();
          //              break;
          //          }
          //      }
          //      GetMAC = macAddresses;
          //  }
          //  if (!string.IsNullOrEmpty(ipAddress))
          //  {
          //      string[] addresses1 = ipAddress.Split(',');
          //      if (addresses1.Length != 0)
          //      {
          //          ip2 = addresses1[0];
          //      }
          //  }
          //  else
          //  {
          //      ip2 = context.Request.ServerVariables["REMOTE_ADDR"];
          //  }

          //  ViewBag.temp ="Mac Andress"+GetMAC+ "Thiết bị :"+device +"Tên trình duyệt"+ name+"Phiên bản chính"+marjoversion+"phiên bản nhỏ"+minoversion+"Platform :"+browser+"địa chỉ IP :"+ip+"true ip"+ip2;
          //  ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            // Require the user to have a confirmed email before they can log on.  
            var user = await UserManager.FindByNameAsync(model.Name);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    //tự động gửi lại email kích hoạt tài khoản nếu tài khoản chưa kích hoạt, sau này thêm tùy chọn gửi lại email kích hoạt 
                    //hợp lý hơn
                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Xác nhận tài khoản Demo Restaurant-Gửi lại");
                    ViewBag.errorMessage = "Bạn cần xác nhận tải khoản để đăng nhập";
                    return View("Error");
                }
            }
            // cần làm hàm check client riêng
            //ClientDetail clientdetail = new ClientDetail()
            //{
            //    ClientIpAndress = Request.UserHostAddress,
            //    ClientTime = DateTime.Now,
            //    DeviceName = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName,
            //};
            //Client client = ResDb.Client.Find(model.Name);
            //if(client == null) {
            //    client.UserName = model.Name;
            //    client.ClientDetail.Add(clientdetail);
            //    client.ClientId = Guid.NewGuid().ToString();
            //    clientdetail.Client = client;
            //    client.status = "UnLock";
            //}
            //else if (!client.ClientDetail.Contains(clientdetail)) client.status = "Lock";
            //if (client.status == "Lock")
            //{
            //    string code =  UserManager.GenerateUserToken("LockIP", model.Name);
            //    var callbackUrl = Url.Action("ConfirmIp", "Account", new { UserName = model.Name, code = code }, protocol: Request.Url.Scheme);

            //    string Subject = "Tài khoản Demo Restaurant đã bị khóa do đăng nhập từ địa chỉ mới";
            //    UserManager.SendEmail(user.Id, Subject, "Vui lòng xác nhận tài khoản của bạn trên Demo Restaurant : <a href=\"" + callbackUrl + "\">Click vào đây</a>");
            //    ResDb.ClientDetail.Add(clientdetail);
            //    ResDb.Client.Add(client);
            //    ResDb.SaveChanges();
            //    return View("Lockout");
            //}
            var result = await SignInManager.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, shouldLockout: true);
            // cần thêm lựa chọn gửi mail khi tài khoản bị khóa do đăng nhập
            switch (result)
            {
                case SignInStatus.Success:
                    MigrateShoppingCart(model.Name);
                    FormsAuthentication.SetAuthCookie(model.Name,model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1
                        && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") &&
                        !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    EmailModel mail = new EmailModel();
                    mail.Subject = "tài khoản Demo Restaurant đã bị khóa do đăng nhập sai pass liên tục"; ;
                    mail.Body = "Đang có đăng nhập trái phép đến tài khoản của bạn tại hệ thống Demo Restaurant . Hiện Ban Quản trị đã tạm khóa tài khoản";
                    mail.Destination = user.Email;
                    mail.SendEmail();
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
        
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig

            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {    
                var user = new ApplicationUser { UserName = model.Name, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                Guid id = Guid.NewGuid();
                string customerid =id.ToString() ;
                //demo code, need to be deleted   DateTime date = new DateTime();
              
                if (result.Succeeded)
                {
                    //tạo mới ClientDetail, Client để quản lý người truy cập trước khi tạo tài khoản
                    //theo dõi ip của user dựa vào ClientDetail, nếu là Ip mới truy cập sẽ khóa acc, send mail kích hoạt acc
                    //hiện mới làm client quản lý IP, cần code thêm
                    //ClientDetail clientdetail = new ClientDetail()
                    //{
                    //    ClientIpAndress = Request.UserHostAddress,
                    //    ClientTime = DateTime.Now,
                    //    DeviceName = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName,
                    //};
                    //Client client = new Client();
                    //client.UserName = model.Name;
                    //client.ClientDetail.Add(clientdetail);
                    //client.ClientId = Guid.NewGuid().ToString();
                    //clientdetail.Client = client;
                    //client.status = "UnLock";
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    Customer customer = new Customer() { CustomerName = model.Name, CustomerPhone = model.Phone, ShippingAddress = model.ShippingAdress, CustomerId= customerid };
                    //ResDb.ClientDetail.Add(clientdetail);
                    //ResDb.Client.Add(client);
                    ResDb.Customer.Add(customer);
                    ResDb.SaveChanges();
                    FormsAuthentication.SetAuthCookie(model.Name, false);
                    //send mail xác nhận tài khoản
                    string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Xác nhận tài khoản Demo Restaurant");
                    
                    ViewBag.Message = "Vui lòng kiểm tra hòm thư để xác nhận tài khoản, bạn cần xác nhận tài khoản để đăng nhập vào hệ thống.";

                    return View("Info");
                    //return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

 
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        // đang code client
        //[AllowAnonymous]
        //public  ActionResult ConfirmIp(string UserName, string code) {
        //    if (UserName == null || code == null)
        //    {
        //        return View("Error");
        //    }
        //    var result =  UserManager.VerifyUserToken(UserName, "LockIP", code);
        //    if (result == true)
        //    {
               
        //    }
        //}

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {

                    // MS khuyên :V Don't reveal that the user does not exist or is not confirmed
                    // cần code thêm tính năng, hoặc lựa chọn báo tài khoản không tồn tại
                    string MessageFail = "Không tìm thấy mail đã nhập hoặc tài khoản chưa được kích hoạt vui lòng kiểm tra lại thông tin";
                    return RedirectToAction("ForgotPasswordConfirmation","Account", new { m = MessageFail });
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                 string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                //var callbackUrl = @HttpUtility.UrlDecode(Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code, Email = DestinatiolEmail }, protocol: Request.Url.Scheme));		
                //var callbackUrl =Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code}, protocol: Request.Url.Scheme);
                string email = UserManager.FindById(user.Id).Email;
                var callbackUrl = Url.Action("ResetPassword", "Account", new { Email = email, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Lấy lại mật khẩu tài khoản Demo Restaurant", "Vui lòng thiết lập lại mật khẩu : <a href=\"" + callbackUrl + "\">Click tại đây</a>");
                 return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation(  )
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string Email, string code)
        {
            //model.Email = UserManager.FindById(userId).Email;
            //model.Password = UserManager.FindById(userId).PasswordHash;
            if (code == null || Email == null) return View("Error");
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                // cần thêm lựa chọn khi không tìm thấy user trong database ứng với email
                 return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation( )
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            var user = UserManager.FindByEmail(loginInfo.Email);

            switch (result)
            {
                case SignInStatus.Success:
                    MigrateShoppingCart(loginInfo.DefaultUserName);

                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    EmailModel mail = new EmailModel();
                    mail.Subject = "tài khoản Demo Restaurant đã bị khóa do đăng nhập sai pass liên tục"; ;
                    mail.Body = "Đang có đăng nhập trái phép đến tài khoản của bạn tại hệ thống Demo Restaurant . Hiện Ban Quản trị đã tạm khóa tài khoản";
                    mail.Destination = loginInfo.Email;
                    mail.SendEmail();
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }
            //if (!ModelState.IsValid) { ViewBag.Exception = " model sai, cần check lại model gửi vào controller"; return View(); }
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();

                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
              
                    var user = new ApplicationUser { UserName = model.ExternalUserName, Email = model.Email };
                    var result = await UserManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        result = await UserManager.AddLoginAsync(user.Id, info.Login);
                        if (result.Succeeded)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        //lấy dữ liệu từ externllogin view model và add vào customer
                        //ClientDetail clientdetail = new ClientDetail()
                        //{
                        //    ClientIpAndress = Request.UserHostAddress,
                        //    ClientTime = DateTime.Now,
                        //    DeviceName = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName,
                        //};
                        //Client client = new Client();
                        //client.UserName = model.ExternalUserName;
                        //client.ClientDetail.Add(clientdetail);
                        //client.ClientId = Guid.NewGuid().ToString();
                        //clientdetail.Client = client;
                        //client.status = "UnLock";
                        Guid id = Guid.NewGuid();
                        string customerid = id.ToString();

                        Customer customer = new Customer() { CustomerName = model.ExternalUserName, CustomerPhone = model.Phone, ShippingAddress = model.ShippingAddress, CustomerId = customerid };
                         // Send an email with this link
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                        //ResDb.ClientDetail.Add(clientdetail);
                        //ResDb.Client.Add(client);
                        ResDb.Customer.Add(customer);
                        int i =  ResDb.SaveChanges();

                        return RedirectToLocal(returnUrl);

                        }
                    }
                    AddErrors(result);
                
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID, subject,"Vui lòng xác nhận tài khoản của bạn trên Demo Restaurant : <a href=\"" + callbackUrl + "\">Click vào đây</a>");
            return callbackUrl;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }

        }
        #endregion
    }
}