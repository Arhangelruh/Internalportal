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

        /// <summary>
        /// Build test.
        /// </summary>
        /// <returns>Test model</returns>
        Task<TestDto> BuildTestAsync(int cashTestId);

        /// <summary>
        /// Get tests.
        /// </summary>
        /// <returns>Test Lists</returns>
        Task<List<Test>> GetTestsAsync(string sortOrder, int page, int pageSize);

        /// <summary>
        /// Get test answers.
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        Task<List<TestsAnswers>> GetQuestionAnswersAsync(int testId);

        /// <summary>
        /// Get test table count.
        /// </summary>
        /// <returns>total count</returns>
        Task<int> TestCountAsync();
    }
}
