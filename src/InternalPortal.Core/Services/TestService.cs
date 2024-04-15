using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
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
        private readonly ICashTestService _cashTestService;

        public TestService(
            IRepository<Test> repository,
            IRepository<TestsAnswers> repositoryTestAnswers,
            ITestTopicService testTopicService,
            ITestQuestionService testQuestionService,
            ITestAnswerService testAnswerService,
            ICashTestService cashTestService
            )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _repositoryTestAnswers = repositoryTestAnswers ?? throw new ArgumentNullException(nameof(repositoryTestAnswers));
            _testTopicService = testTopicService ?? throw new ArgumentNullException(nameof(testTopicService));
            _testQuestionService = testQuestionService ?? throw new ArgumentNullException(nameof(testQuestionService));
            _testAnswerService = testAnswerService ?? throw new ArgumentNullException(nameof(testAnswerService));
            _cashTestService = cashTestService ?? throw new ArgumentNullException(nameof(cashTestService));
        }

        public async Task AddAsync(Test test)
        {
            ArgumentNullException.ThrowIfNull(test);

            var saveTest = new Test
            {
                StartTime = test.StartTime,
                EndTime = test.EndTime,
                ProfileId = test.ProfileId,
                CashTestId = test.CashTestId,
                PassResult = test.PassResult
            };

            await _repository.AddAsync(saveTest);
            await _repository.SaveChangesAsync();

            foreach (var answer in test.TestsAnswers)
            {
                await _repositoryTestAnswers.AddAsync(new TestsAnswers
                {
                    AnswerId = answer.AnswerId,
                    TestId = saveTest.Id,
                    AnswerStatus = answer.AnswerStatus
                });
                await _repositoryTestAnswers.SaveChangesAsync();
            }
        }

        public async Task<TestDto> BuildTestAsync(int cashTestId)
        {
            var cashTest = await _cashTestService.GetCashTestByIdAsync(cashTestId);
            var topics = await _testTopicService.GetActiveTopicsByCashTestAsync(cashTestId);

            var questions = await GetQuestionListAsync(topics, cashTest.TestQuestions);
            var answers = await GetAnswersAsync(questions);

            return new TestDto
            {
                Questions = questions,
                Answers = answers
            };
        }

        private TestQuestions GetRandomQuestionAsync(TestTopics topic, List<TestQuestions> allQuestions)
        {
            var random = new Random();

            var questions = allQuestions.Where(q => q.TestTopicId == topic.Id).ToList();

            if (questions.Count != 0)
            {
                int index = random.Next(questions.Count);
                return questions[index];
            }
            return null;
        }

        private async Task<List<TestQuestions>> GetQuestionListAsync(List<TestTopics> topics, int amounthQuestions)
        {
            List<TestQuestions> questions = [];
            var random = new Random();

            List<TestTopics> testTopicClear = [];
            List<TestQuestions> allQuestions = [];

            foreach (var topic in topics)
            {
                var getActualQuestions = await _testQuestionService.GetActualQuestionByTopicAsync(topic.Id);
                if (getActualQuestions.Count != 0)
                {
                    testTopicClear.Add(topic);
                    foreach (var getActualQuestion in getActualQuestions)
                    {
                        allQuestions.Add(getActualQuestion);
                    }
                }
            }

            if (testTopicClear.Count != 0)
            {
                if (testTopicClear.Count < amounthQuestions)
                {
                    foreach (var actualTopic in testTopicClear)
                    {
                        var question = GetRandomQuestionAsync(actualTopic, allQuestions);
                        if (question != null)
                        {
                            questions.Add(question);
                        }
                    }

                    if (allQuestions.Count > amounthQuestions)
                    {

                        for (int i = questions.Count; i < amounthQuestions;)
                        {
                            var randomindex = random.Next(topics.Count);
                            var randomQuestion = GetRandomQuestionAsync(topics[randomindex], allQuestions);
                            if (randomQuestion != null && !questions.Contains(randomQuestion))
                            {
                                questions.Add(randomQuestion);
                                i++;
                            }
                        }
                    }
                    else
                    {
                        foreach (var question in allQuestions)
                        {
                            if (!questions.Contains(question))
                            {
                                questions.Add(question);
                            }
                        }
                    }
                }
                else
                {
                    var tempListTopics = testTopicClear;
                    for (int i = 1; i <= amounthQuestions;)
                    {
                        var randomindex = random.Next(tempListTopics.Count);
                        var randomQuestion = GetRandomQuestionAsync(tempListTopics[randomindex], allQuestions);
                        if (randomQuestion != null)
                        {
                            questions.Add(randomQuestion);
                            i++;
                        }
                        tempListTopics.Remove(tempListTopics[randomindex]);
                    }
                }
            }

            RandomList<TestQuestions> randomList = new RandomList<TestQuestions>();
            randomList.Randomizer(questions);
            return questions;
        }

        private async Task<List<TestQuestionAnswers>> GetAnswersAsync(List<TestQuestions> questions)
        {
            List<TestQuestionAnswers> answers = [];

            foreach (var question in questions)
            {
                var questionAnswers = await _testAnswerService.GetAnswersByQuestionAsync(question.Id);
                if (questionAnswers != null)
                {
                    foreach (var answer in questionAnswers)
                    {
                        answers.Add(answer);
                    }
                }
            }

            RandomList<TestQuestionAnswers> randomList = new RandomList<TestQuestionAnswers>();
            randomList.Randomizer(answers);
            return answers;
        }

        public async Task<List<Test>> GetTestsAsync()
        {
            return await _repository.GetAll().AsNoTracking().ToListAsync();
        }

        public async Task<List<TestsAnswers>> GetQuestionAnswersAsync(int testId)
        {
            return await _repositoryTestAnswers
                .GetAll()
                .AsNoTracking()
                .Where(testanswer => testanswer.TestId == testId)
                .ToListAsync();
        }
    }

    public class RandomList<T>
    {
        public List<T> Randomizer(List<T> input)
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
