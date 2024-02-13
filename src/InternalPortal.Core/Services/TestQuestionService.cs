using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalPortal.Core.Services
{
    public class TestQuestionService : ITestQuestionService
    {
        private readonly IRepository<TestQuestions> _repository;
        private readonly TestAnswerService _answerService;
        private readonly IRepository<TestsAnswers> _repositoryTests;

        public TestQuestionService(IRepository<TestQuestions> repository, TestAnswerService answerService, IRepository<TestsAnswers> repositoryTests)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _answerService = answerService ?? throw new ArgumentNullException(nameof(answerService));
            _repositoryTests = repositoryTests ?? throw new ArgumentNullException(nameof(repositoryTests));
        }

        public async Task AddAsync(TestQuestions testQuestion)
        {
            ArgumentNullException.ThrowIfNull(testQuestion);

            await _repository.AddAsync(testQuestion);
            await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int testQuestionId)
        {
            var getAnswers = await _answerService.GetAnswersByQuestionAsync(testQuestionId);
            if (getAnswers.Count != 0)
            {
                foreach (var answer in getAnswers)
                {
                    var delAnswer = await _answerService.DeleteAsync(answer.Id);
                    if (!delAnswer)
                    {
                        return false;
                    }
                }
            }

            var getquestion = await _repository.GetEntityAsync(question => question.Id.Equals(testQuestionId));

            _repository.Delete(getquestion);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditAsync(TestQuestions testQuestion)
        {
            var getAnswers = await _answerService.GetAnswersByQuestionAsync(testQuestion.Id);
            if (getAnswers.Count != 0)
            {
                foreach (var answer in getAnswers)
                {
                    var checkInTests = await _repositoryTests.GetEntityAsync(testanswer => testanswer.AnswerId.Equals(answer.Id));
                    if (checkInTests != null)
                    {
                        return false;
                    }
                }
            }

            var editQuestion = await _repository.GetEntityAsync(q => q.Id.Equals(testQuestion.Id));
            editQuestion.QuestionText = testQuestion.QuestionText;
            editQuestion.IsActual = testQuestion.IsActual;

            _repository.Update(editQuestion);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<TestQuestions> GetQuestionByIdAsync(int testQuestionId)
        {
            var getQuestion = await _repository.GetEntityAsync(question => question.Id.Equals(testQuestionId));

            return getQuestion;
        }

        public async Task<List<TestQuestions>> GetQuestionByTopicAsync(int testTopicId)
        {
            var getquestions = await _repository
                .GetAll()
                .AsNoTracking()
                .Where(question => question.TestTopicId == testTopicId)
                .ToListAsync();

            return getquestions;
        }

        public async Task ChangeStatusAsync(TestQuestions testQuestion)
        {
            var editQuestion = await _repository.GetEntityAsync(q => q.Id.Equals(testQuestion.Id));
            editQuestion.IsActual = testQuestion.IsActual;

            _repository.Update(editQuestion);
            await _repository.SaveChangesAsync();
        }
    }
}
