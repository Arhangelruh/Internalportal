using InternalPortal.Core.Models;

namespace InternalPortal.Core.Interfaces
{
    /// <summary>
    /// Sevice for working with Test Topics data.
    /// </summary>
    public interface ITestTopicService
    {
        /// <summary>
        /// Add topic.
        /// </summary>
        /// <param name="testTopic">Dto model</param>
        Task AddAsync(TestTopics testTopic);

        /// <summary>
        /// Edit topic.
        /// </summary>
        /// <param name="testTopic">Dto model</param>
        Task EditAsync(TestTopics testTopic);        

        /// <summary>
        /// Get topic from base.
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns>test topic data model</returns>
        Task<TestTopics> GetTestTopicByIdAsync(int topicId);

        /// <summary>
        /// Get all Test Topics.
        /// </summary>
        /// <returns>All test topics</returns>
        Task <List<TestTopics>> GetAllTopicsAsync();

        /// <summary>
        /// Get all active Test Topics.
        /// </summary>
        /// <returns>Actual test topics</returns>
        Task<List<TestTopics>> GetActiveTopicsAsync();

        /// <summary>
        /// Delete TestTopic.
        /// </summary>
        /// <param name="topicId">Test topic id</param>        
        Task<bool> DeleteAsync(int topicId);

        /// <summary>
        /// Change actual status.
        /// </summary>
        /// <param name="testTopic">Test topic data model</param>        
        Task ChangeStatusAsync(TestTopics testTopic);
    }
}
