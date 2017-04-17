using Microsoft.Owin;
using Owin;
using DemoRestaurant.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartupAttribute(typeof(DemoRestaurant.Startup))]
namespace DemoRestaurant
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //tạo role và user admin base, cần remove khi xong admin module
            createRolesandUsers();
            ConfigureAuth(app);
        }
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // Tạo sẵn tài khoản admin và role admin, sẽ remove sau khi finish admin module
                if (!RoleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var Role = new IdentityRole();
                Role.Name = "Admin";
                Role.Id = "Ad";
                RoleManager.Create(Role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "SuperAdmin";
                user.Email = "nguyenbanguyenth@gmail.com";
                user.EmailConfirmed = true;
                string userPWD = "A@aA@a";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }
            if (UserManager.FindByName("SuperAdmin") == null)
            {
                var user = new ApplicationUser();
                user.UserName = "SuperAdmin";
                user.Email = "nguyenbanguyenth@gmail.com";
                user.EmailConfirmed = true;
                string userPWD = "A@aA@a";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                    var result2 = UserManager.AddToRole(user.Id, "Manager");
                    var result3 = UserManager.AddToRole(user.Id, "Customer");

                }
            }

            // creating Creating Manager role    
            if (!RoleManager.RoleExists("Manager"))
            {
                var Role = new  IdentityRole();
                Role.Name = "Manager";
                Role.Id = "Mana";
                RoleManager.Create(Role);

            }

            // creating Creating Employee role    
            if (!RoleManager.RoleExists("Customer"))
            {
                var Role = new  IdentityRole();
                Role.Name = "Customer";
                Role.Id = "Cust";
                RoleManager.Create(Role);

            }
        }
    }
}
