namespace InternalPortal.Web.Interfaces
{
    public interface ISignInManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> SignIn(string username, string password);
        Task SignOut();
    }
}
