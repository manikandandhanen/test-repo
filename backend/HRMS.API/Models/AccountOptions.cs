using System;


namespace HRMS.API.Models
{
    public class AccountOptions
    {
        public static bool AllowRememberLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
        public static string UserDeactivated = "user deactivated, Please contact administrator";
    }
}
