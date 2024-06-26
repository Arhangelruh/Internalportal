﻿using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace InternalPortal.Core.Services
{
    public class TestScoreService : ITestScoreService
    {
        private readonly IRepository<TestScore> _repository;

        public TestScoreService(IRepository<TestScore> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task AddAsync(TestScore testScore)
        {
            ArgumentNullException.ThrowIfNull(testScore);

            await _repository.AddAsync(testScore);
            await _repository.SaveChangesAsync();
        }

        public async Task EditAsync(TestScore testScore)
        {
            var editScore = await _repository.GetEntityAsync(score => score.Id.Equals(testScore.Id));
            editScore.Score = testScore.Score;

            _repository.Update(editScore);
            await _repository.SaveChangesAsync();
        }

        public async Task<TestScore> GetScoreByIdAsync(int scoreId)
        {
            return await _repository.GetEntityAsync(score => score.Id.Equals(scoreId));
        }

        public async Task<TestScore> GetScoreByProfileIdAsync(int profoleId)
        {
            return await _repository.GetEntityAsync(score => score.ProfileId.Equals(profoleId));
        }

        public async Task DeleteScoreAsync(int profileId) {
            var getScore = await _repository.GetEntityAsync(score => score.ProfileId == profileId);
                        
            if (getScore != null)
            {
                _repository.Delete(getScore);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task<List<TestScore>> GetScoresAsync() {
            return await _repository.GetAll().AsNoTracking().ToListAsync();
        }
    }
}
