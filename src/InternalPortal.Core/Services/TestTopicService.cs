using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalPortal.Core.Services
{
    /// <inheritdoc cref="ITestTopicService"/>
    public class TestTopicService : ITestTopicService
    {
        private readonly IRepository<TestTopics> _repository;
        private readonly TestQuestionService _testQuestionService;
        
        public TestTopicService(IRepository<TestTopics> repository, TestQuestions testQuestionService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _testQuestionService = _testQuestionService ?? throw new ArgumentNullException(nameof(testQuestionService));
        }

        public async Task AddAsync(TestTopics testTopic)
        {
            ArgumentNullException.ThrowIfNull(testTopic);

            await _repository.AddAsync(testTopic);
            await _repository.SaveChangesAsync();
        }
        
        public async Task<bool> DeleteAsync(int topicId)
        {
            var getQuestions = await _testQuestionService.GetQuestionByTopicAsync(topicId);
            if (getQuestions.Count != 0)
            {
                foreach (var question in getQuestions)
                {
                    var delQuestion = await _testQuestionService.DeleteAsync(question.Id);
                    if (!delQuestion)
                    {
                        return false;
                    }
                }
            }

            var testTopic = await _repository.GetEntityAsync(topic => topic.Id.Equals(topicId));

            _repository.Delete(testTopic);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task EditAsync(TestTopics testTopic)
        {
            ArgumentNullException.ThrowIfNull(testTopic);

            var editTopic = await _repository.GetEntityAsync(q => q.Id.Equals(testTopic.Id));
            editTopic.TopicName = testTopic.TopicName;

            _repository.Update(editTopic);
            await _repository.SaveChangesAsync();
        }

        public async Task<List<TestTopics>> GetAllTopicsAsync()
        {
            return await _repository.GetAll().AsNoTracking().ToListAsync();
        }

        public async Task<TestTopics> GetTestTopicByIdAsync(int topicId)
        {
            var testTopic = await _repository.GetEntityAsync(topic => topic.Id == topicId);

            if (testTopic is null)
            {
                return new TestTopics();
            }

            return testTopic;
        }
    }
}
