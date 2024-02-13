using InternalPortal.Core.Models;

namespace InternalPortal.Core.Interfaces
{
    /// <summary>
    /// Sevice for working with Test Answers.
    /// </summary>
    public interface ITestAnswerService
    {
        /// <summary>
        /// Add answer.
        /// </summary>
        /// <param name="testQuestionAnswers">Answer model</param>
        Task AddAsync(TestQuestionAnswers testQuestionAnswers);

        /// <summary>
        /// Edit answer.
        /// </summary>
        /// <param name="testQuestionAnswers">Answer model</param>
        /// <returns>result</returns>
        Task<bool> EditAsync(TestQuestionAnswers testQuestionAnswers);

        /// <summary>
        /// Get answer from base.
        /// </summary>
        /// <param name="testAnswerId"></param>
        /// <returns>Test answer model</returns>
        Task<TestQuestionAnswers> GetAnswerByIdAsync(int testAnswerId);

        /// <summary>
        /// Get answers by question.
        /// </summary>
        /// <returns>List answers</returns>
        Task<List<TestQuestionAnswers>> GetAnswersByQuestionAsync(int testQuestionId);

        /// <summary>
        /// Delete answer.
        /// </summary>
        /// <param name="testAnswerId">Test answer id</param>
        /// <returns>result</returns>
        Task<bool> DeleteAsync(int testAnswerId);
    }
}
