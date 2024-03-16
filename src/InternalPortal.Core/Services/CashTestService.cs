using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalPortal.Core.Services
{
    /// <inheritdoc cref="ICashTestService"/>
    public class CashTestService : ICashTestService
    {
        private readonly IRepository<CashTest> _repository;
        private readonly ITestTopicService _testTopicService;

        public CashTestService(IRepository<CashTest> repository, ITestTopicService testTopicService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _testTopicService = testTopicService ?? throw new ArgumentNullException(nameof(testTopicService));
        }

        public async Task AddCashTestAsync(CashTest cashTest)
        {
            ArgumentNullException.ThrowIfNull(cashTest);

            await _repository.AddAsync(cashTest);
            await _repository.SaveChangesAsync();
        }

        public async Task ChangecashTestStatusAsync(CashTest cashTest)
        {
            var editCashTest = await _repository.GetEntityAsync(q => q.Id.Equals(cashTest.Id));
            editCashTest.IsActual = editCashTest.IsActual;

            _repository.Update(editCashTest);
            await _repository.SaveChangesAsync();
        }

        public async Task<bool> DeleteCashTestAsync(int cashTestsId)
        {
            var getTests = await _testTopicService.GetTopicsByCashTestAsync(cashTestsId);
            if (getTests.Count != 0)
            {
                foreach (var topic in getTests)
                {
                    var delTopic = await _testTopicService.DeleteAsync(topic.Id);
                    if (!delTopic)
                    {
                        return false;
                    }
                }
            }

            var cashTest = await _repository.GetEntityAsync(test => test.Id.Equals(cashTestsId));

            _repository.Delete(cashTest);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task EditCashTestAsync(CashTest cashTest)
        {
            ArgumentNullException.ThrowIfNull(cashTest);

            var editCashTest = await _repository.GetEntityAsync(q => q.Id.Equals(cashTest.Id));
            editCashTest.TestName = cashTest.TestName;
           
            _repository.Update(editCashTest);
            await _repository.SaveChangesAsync();
        }

        public async Task<List<CashTest>> GetActiveCashTestsAsync()
        {
            return await _repository
                .GetAll()
                .Where(test => test.IsActual == true)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<CashTest>> GetAllCashTestsAsync()
        {
            return await _repository.GetAll().AsNoTracking().ToListAsync();
        }

        public async Task<CashTest> GetCashTestByIdAsync(int cashTestId)
        {
            var cashTest = await _repository.GetEntityAsync(test => test.Id == cashTestId);

            if (cashTest is null)
            {
                return null;
            }

            return cashTest;
        }
    }
}
