using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalPortal.Core.Services
{
    /// <inheritdoc cref="ITestAnswerService"/>
    public class TestAnswerService : ITestAnswerService
    {
        private readonly IRepository<TestQuestionAnswers> _repository;
        private readonly IRepository<TestsAnswers> _repositoryTests;

        public TestAnswerService(IRepository<TestQuestionAnswers> repository, IRepository<TestsAnswers> repositoryTests)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _repositoryTests = repositoryTests ?? throw new ArgumentNullException(nameof(repositoryTests));
        }

        public async Task AddAsync(TestQuestionAnswers testQuestionAnswers)
        {
            ArgumentNullException.ThrowIfNull(testQuestionAnswers);

            await _repository.AddAsync(testQuestionAnswers);
            await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int testAnswerId)
        {
            var getTestsResult = await _repositoryTests.GetEntityAsync(test => test.AnswerId.Equals(testAnswerId));
            if (getTestsResult == null) {
                var getAnswer = await _repository.GetEntityAsync(answer =>answer.Id.Equals(testAnswerId));
                
                _repository.Delete(getAnswer);
                await _repository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> EditAsync(TestQuestionAnswers testQuestionAnswers)
        {
            ArgumentNullException.ThrowIfNull(testQuestionAnswers);

            var getTestsResult =await _repositoryTests.GetEntityAsync(test => test.AnswerId.Equals(testQuestionAnswers.Id));
            if (getTestsResult == null)
            {
                var editAnswer = await _repository.GetEntityAsync(q => q.Id.Equals(testQuestionAnswers.Id));
                editAnswer.AnswerText = testQuestionAnswers.AnswerText;
                editAnswer.Meaning = testQuestionAnswers.Meaning;
                editAnswer.IsActual = testQuestionAnswers.IsActual;

                _repository.Update(editAnswer);
                await _repository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<TestQuestionAnswers>> GetAnswersByQuestionAsync(int testQuestionId)
        {
            var getanswers = await _repository
                .GetAll()
                .AsNoTracking()
                .Where(answer => answer.TestQuestionId == testQuestionId)
                .ToListAsync();
            
            return getanswers;
        }

        public async Task<List<TestQuestionAnswers>> GetActualAnswersByQuestionAsync(int testQuestionId)
        {
            var getanswers = await _repository
                .GetAll()
                .AsNoTracking()
                .Where(answer => answer.TestQuestionId == testQuestionId && answer.IsActual==true)
                .ToListAsync();

            return getanswers;
        }

        public async Task<TestQuestionAnswers> GetAnswerByIdAsync(int testAnswerId)
        {
            var getAnswer = await _repository.GetEntityAsync(answer=> answer.Id.Equals(testAnswerId));

            return getAnswer;
        }

        public async Task ChangeStatusAsync(TestQuestionAnswers testQuestionAnswers)
        {
            var editAnswer = await _repository.GetEntityAsync(q => q.Id.Equals(testQuestionAnswers.Id));
            editAnswer.IsActual = testQuestionAnswers.IsActual;

            _repository.Update(editAnswer);
            await _repository.SaveChangesAsync();
        }
    }
}
