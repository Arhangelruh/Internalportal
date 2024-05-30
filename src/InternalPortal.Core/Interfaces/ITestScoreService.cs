using InternalPortal.Core.Models;

namespace InternalPortal.Core.Interfaces
{
    public interface ITestScoreService
    {
        /// <summary>
        /// Add score.
        /// </summary>
        /// <param name="testScore">TestScore model</param>
        Task AddAsync(TestScore testScore);

        /// <summary>
        /// Edit score.
        /// </summary>
        /// <param name="testScore">TestScore model</param>
        /// <returns>result</returns>
        Task EditAsync(TestScore testScore);

        /// <summary>
        /// Get score by id.
        /// </summary>
        /// <param name="scoreId">score id</param>
        /// <returns>Test score model</returns>
        Task<TestScore> GetScoreByIdAsync(int scoreId);

        /// <summary>
        /// Get score by profile.
        /// </summary>
        /// <param name="profileId">profile id</param>
        /// <returns>Test score model</returns>
        Task<TestScore> GetScoreByProfileIdAsync(int profoleId);

        /// <summary>
        /// Delete score.
        /// </summary>
        /// <param name="profileId">profile id</param>        
        Task DeleteScoreAsync(int profileId);

        /// <summary>
        /// Get all scores.
        /// </summary>
        /// <returns>list scores</returns>
        Task<List<TestScore>> GetScoresAsync();
    }
}
