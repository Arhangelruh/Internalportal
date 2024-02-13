using InternalPortal.Core.Models;

namespace InternalPortal.Core.Interfaces
{
    /// <summary>
    /// Sevice for working with Test Questions.
    /// </summary>
    public interface ITestQuestionService
    {
        /// <summary>
        /// Add question.
        /// </summary>
        /// <param name="testQuestion">Question model</param>
        Task AddAsync(TestQuestions testQuestion);

        /// <summary>
        /// Edit question.
        /// </summary>
        /// <param name="testQuestion">Question model</param>
        /// <returns>result</returns>
        Task<bool> EditAsync(TestQuestions testQuestion);

        /// <summary>
        /// Get question from base.
        /// </summary>
        /// <param name="testQuestionId"></param>
        /// <returns>Test question model</returns>
        Task<TestQuestions> GetQuestionByIdAsync(int testQuestionId);

        /// <summary>
        /// Get questions by topic.
        /// </summary>
        /// <returns>List questions</returns>
        Task<List<TestQuestions>> GetQuestionByTopicAsync(int testTopicId);

        /// <summary>
        /// Delete question.
        /// </summary>
        /// <param name="testQuestionId">Test question id</param>
        /// <returns>result</returns>
        Task<bool> DeleteAsync(int testQuestionId);

        /// <summary>
        /// Change actual status.
        /// </summary>
        /// <param name="testQuestion">TestQuestion data model</param>        
        Task ChangeStatusAsync(TestQuestions testQuestion);
    }
}
