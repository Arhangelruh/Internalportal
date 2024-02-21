using InternalPortal.Core.Models;

namespace InternalPortal.Core.Interfaces
{
    /// <summary>
    /// Sevice for working with Test data.
    /// </summary>
    public interface ITestService
    {
        /// <summary>
        /// Add test.
        /// </summary>
        /// <param name="test">test model</param>

        Task AddAsync(Test test);
    }
}
