using InternalPortal.Core.Models;

namespace InternalPortal.Core.Interfaces
{
    /// <summary>
    /// Sevice for working with Cash Test data.
    /// </summary>
    public interface ICashTestService
    {
        /// <summary>
        /// Add cash test.
        /// </summary>
        /// <param name="cashTest">Dto model</param>
        Task AddCashTestAsync(CashTest cashTest);

        /// <summary>
        /// Edit cash test.
        /// </summary>
        /// <param name="testTopic">Dto model</param>
        Task EditCashTestAsync(CashTest cashTest);

        /// <summary>
        /// Get cash test.
        /// </summary>
        /// <param name="cashTestId"></param>
        /// <returns>cash test data model</returns>
        Task<CashTest> GetCashTestByIdAsync(int cashTestId);

        /// <summary>
        /// Get all cash tests.
        /// </summary>
        /// <returns>All cash tests</returns>
        Task<List<CashTest>> GetAllCashTestsAsync();

        /// <summary>
        /// Get all active Cash Tests.
        /// </summary>
        /// <returns>Actual cash tests</returns>
        Task<List<CashTest>> GetActiveCashTestsAsync();

        /// <summary>
        /// Delete cash test.
        /// </summary>
        /// <param name="cashTestsId">Cash test id</param>        
        Task<bool> DeleteCashTestAsync(int cashTestsId);

        /// <summary>
        /// Change cash test status.
        /// </summary>
        /// <param name="cashTest">Cash test data model</param>        
        Task ChangecashTestStatusAsync(CashTest cashTest);
    }
}
