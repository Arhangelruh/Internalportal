using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InternalPortal.Web.Interfaces
{
    public interface ISignInManager
    {
        /// <summary>
        /// Sign in method.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> SignIn(string username, string password, ModelStateDictionary modelState);

        /// <summary>
        /// Sign out method.
        /// </summary>
        /// <returns></returns>
        Task SignOut();
    }
}
