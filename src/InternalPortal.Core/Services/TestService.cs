using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace InternalPortal.Core.Services
{
    public class TestService : ITestService
    {
        private readonly IRepository<Test> _repository;
        private readonly IRepository<TestsAnswers> _repositoryTestAnswers;
        private readonly ITestTopicService _testTopicService;
        private readonly ITestQuestionService _testQuestionService;
        private readonly ITestAnswerService _testAnswerService;
        private readonly ConfigurationTest _configurationTest;

        public TestService(
            IRepository<Test> repository,
            IRepository<TestsAnswers> repositoryTestAnswers,
            ITestTopicService testTopicService,
            ITestQuestionService testQuestionService,
            ITestAnswerService testAnswerService,
            IOptions<ConfigurationTest> configurationTest
            )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _repositoryTestAnswers = repositoryTestAnswers ?? throw new ArgumentNullException(nameof(repositoryTestAnswers));
            _testTopicService = testTopicService ?? throw new ArgumentNullException(nameof(testTopicService));
            _testQuestionService = testQuestionService ?? throw new ArgumentNullException(nameof(testQuestionService));
            _testAnswerService = testAnswerService ?? throw new ArgumentNullException(nameof(testAnswerService));
            _configurationTest = configurationTest.Value ?? throw new ArgumentNullException(nameof(configurationTest.Value));
        }

        public async Task AddAsync(Test test)
        {
            ArgumentNullException.ThrowIfNull(test);

            var saveTest = new Test {
              StartTime = test.StartTime,
              EndTime = test.EndTime,
              ProfileId = test.ProfileId
            };

            await _repository.AddAsync(saveTest);
            await _repository.SaveChangesAsync();

            foreach(var answer in test.TestsAnswers)
            {
                await _repositoryTestAnswers.AddAsync(new TestsAnswers { 
                  AnswerId = answer.AnswerId,
                  TestId  = saveTest.Id
                });
                await _repositoryTestAnswers.SaveChangesAsync();
            }
        }

        public async Task<TestDto> BuildTestAsync()
        {                        
            var topics = await _testTopicService.GetActiveTopicsAsync();
            
            var questions = await GetQuestionListAsync(topics);
            var answers = await GetAnswersAsync(questions);

            return new TestDto { 
              Questions = questions,
              Answers = answers
            };
        }

        private async Task<TestQuestions> GetRandomQuestionAsync(TestTopics topic) {
            var random = new Random();

            var questions = await _testQuestionService.GetActualQuestionByTopicAsync(topic.Id);
            if (questions.Count != 0)
            {
                int index = random.Next(questions.Count);
                return questions[index];
            }
            return null;
        }

        private async Task<List<TestQuestions>> GetQuestionListAsync(List<TestTopics> topics)
        {
            List<TestQuestions> questions = [];
            var random = new Random();
           
            if (topics.Count != 0)
            {
                if (topics.Count < _configurationTest.Questions)
                {
                    foreach (var topic in topics)
                    {
                        var question = await GetRandomQuestionAsync(topic);
                        if (question != null)
                        {
                            questions.Add(question);
                        }
                    }

                    for (int i = questions.Count; i <= _configurationTest.Questions; i++)
                    {
                        var randomindex = random.Next(topics.Count);
                        var randomQuestion = await GetRandomQuestionAsync(topics[randomindex]);
                        if (randomQuestion != null && !questions.Contains(randomQuestion))
                        {
                            questions.Add(randomQuestion);
                        }
                    }
                }
                else
                {
                    var tempListTopics = topics;
                    for (int i = 0; i <= _configurationTest.Questions; i++)
                    {
                        var randomindex = random.Next(tempListTopics.Count);
                        var randomQuestion = await GetRandomQuestionAsync(tempListTopics[randomindex]);
                        if (randomQuestion != null)
                        {
                            questions.Add(randomQuestion);
                        }
                        tempListTopics.Remove(tempListTopics[randomindex]);
                    }

                    if (questions.Count < _configurationTest.Questions)
                    {
                        for (int i = questions.Count; i <= _configurationTest.Questions; i++)
                        {
                            var randomindex = random.Next(topics.Count);
                            var randomQuestion = await GetRandomQuestionAsync(topics[randomindex]);
                            if (randomQuestion != null && !questions.Contains(randomQuestion))
                            {
                                questions.Add(randomQuestion);
                            }
                        }
                    }
                }
            }

            RandomList<TestQuestions> randomList = new RandomList<TestQuestions>();
            randomList.Randomizer(questions);

            //TODO clean 
            //for (int i = 0; i < questions.Count; i++)
            //{                
            //    int j = random.Next(i, questions.Count);
            //    var temp = questions[i];
            //    questions[i] = questions[j];
            //    questions[j] = temp;
            //}
            return questions;
        }

        private async Task<List<TestQuestionAnswers>> GetAnswersAsync(List<TestQuestions> questions) {
            List<TestQuestionAnswers> answers = [];            

            foreach (var question in questions)
            {
                var questionAnswers = await _testAnswerService.GetAnswersByQuestionAsync(question.Id);
                if(questionAnswers != null)
                {
                    foreach (var answer in questionAnswers)
                    {
                        answers.Add(answer);
                    }
                }
            }

            RandomList<TestQuestionAnswers> randomList = new RandomList<TestQuestionAnswers>();
            randomList.Randomizer(answers);

            //TODO clean 
            //for (int i = 0; i < answers.Count; i++)
            //{
            //    int j = random.Next(i, answers.Count);
            //    var temp = answers[i];
            //    answers[i] = answers[j];
            //    answers[j] = temp;
            //}
            return answers;
        }      
    }    

    public class RandomList<T>
    {
        public List<T> Randomizer (List<T> input)
        {
            var random = new Random();

            for (int i = 0; i < input.Count; i++)
            {
                int j = random.Next(i, input.Count);
                var temp = input[i];
                input[i] = input[j];
                input[j] = temp;
            }
            return input;
        }
    }
}
