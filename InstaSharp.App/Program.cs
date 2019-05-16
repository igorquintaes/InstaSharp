using InstaSharp.Shared;
using InstaSharp.Shared.Pages;
using InstaSharp.Shared.Pages.Modals;
using InstaSharp.Shared.Pages.Modals.Components;
using System.Linq;

namespace InstaSharp.App
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var driver = Setup.Configure())
            {
                var loginPage = new LogInPage(driver, null, null, null);
                var itemListComponent = new ItemListComponent(driver);
                var userFollowingModal = new UserFollowingModal(driver, itemListComponent);
                var userPage = new UserPage(driver, null, userFollowingModal);

                loginPage.Navigate();
                loginPage.Username = "";
                loginPage.Password = "";
                loginPage.LoginButton();

                userPage.Navigate("viorlecinn");
                userPage.FollowingButton();

                var users = itemListComponent.Obtain(500);
                users.Reverse();
                var quantityToUnfollow = 5;
                foreach(var user in users)
                {
                    if (quantityToUnfollow <= 0)
                        break;

                    itemListComponent.Unfollow(user);
                    quantityToUnfollow--;
                }
            }
        }
    }
}
