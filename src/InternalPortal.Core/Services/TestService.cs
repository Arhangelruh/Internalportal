using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<TestService> _logger;


		public TestService(
            IRepository<Test> repository,
            IRepository<TestsAnswers> repositoryTestAnswers,
            ITestTopicService testTopicService,
            ITestQuestionService testQuestionService,
            ITestAnswerService testAnswerService,
            ICashTestService cashTestService,
			ILogger<TestService> logger
			)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _repositoryTestAnswers = repositoryTestAnswers ?? throw new ArgumentNullException(nameof(repositoryTestAnswers));
            _testTopicService = testTopicService ?? throw new ArgumentNullException(nameof(testTopicService));
            _testQuestionService = testQuestionService ?? throw new ArgumentNullException(nameof(testQuestionService));
            _testAnswerService = testAnswerService ?? throw new ArgumentNullException(nameof(testAnswerService));
            _cashTestService = cashTestService ?? throw new ArgumentNullException(nameof(cashTestService));
            _logger = logger ?? throw new ArgumentNullException( nameof(logger));
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
            if (cashTest != null)
            {
                var topics = await _testTopicService.GetActiveTopicsByCashTestAsync(cashTestId);
                if (topics.Count() > 0)
                {
                    var questions = await GetQuestionListAsync(topics, cashTest.TestQuestions);
                    if (questions.Count() == 0) {
						_logger.LogWarning($"Метод {(nameof(GetQuestionListAsync))} не вернул вопросов.");
					}

                    var answers = await GetAnswersAsync(questions);
                    if (answers.Count() == 0)
                    {
						_logger.LogWarning($"Метод {(nameof(GetAnswersAsync))} не вернул ответов.");
					}

                    return new TestDto
                    {
                        Questions = questions,
                        Answers = answers
                    };
                }
				_logger.LogWarning($"Не найдены темы теста id {cashTestId}");
				return new TestDto();
			}
            _logger.LogWarning($"Не найден тест с id {cashTestId}");
            return new TestDto();
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
                var questionAnswers = await _testAnswerService.GetActualAnswersByQuestionAsync(question.Id);
                if ( questionAnswers.Count()> 0)
                {
                    foreach (var answer in questionAnswers)
                    {
                        answers.Add(answer);
                    }
                }
                else
                {
                    _logger.LogWarning($"Сервис {nameof(_testAnswerService)} {question.QuestionText}");
                }
            }

            RandomList<TestQuestionAnswers> randomList = new RandomList<TestQuestionAnswers>();
            randomList.Randomizer(answers);
            return answers;
        }

        public async Task<List<Test>> GetTestsAsync(string sortOrder, int pageIndex, int pageSize)
        {
            var newRequest = _repository.GetAll();

            switch (sortOrder)
            {
                case "User":
                    newRequest = newRequest.OrderBy(n => n.Profile.LastName);
                    break;
                case "User_desc":
                    newRequest = newRequest.OrderByDescending(n => n.Profile.LastName);
                    break;
                case "DateBegin":
                    newRequest = newRequest.OrderBy(d => d.StartTime);
                    break;
                case "DateBegin_desc":
                    newRequest = newRequest.OrderByDescending(d => d.StartTime);
                    break;
                case "DateEnd":
                    newRequest = newRequest.OrderBy(d => d.EndTime);
                    break;
                case "DateEnd_desc":
                    newRequest = newRequest.OrderByDescending(d => d.EndTime);
                    break;
                case "TestName":
                    newRequest = newRequest.OrderBy(d => d.CashTest.TestName);
                    break;
                case "TestName_desc":
                    newRequest = newRequest.OrderByDescending(d => d.CashTest.TestName);
                    break;
                case "TestResult":
                    newRequest = newRequest.OrderBy(d => d.PassResult);
                    break;
                case "TestResult_desc":
                    newRequest = newRequest.OrderByDescending(d => d.PassResult);
                    break;
                default:
                    newRequest = newRequest.OrderByDescending(n => n.Id);
                    break;
            }            

            var list = await newRequest.Skip(pageIndex * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            return list;
        }

        public async Task<List<TestsAnswers>> GetQuestionAnswersAsync(int testId)
        {
            return await _repositoryTestAnswers
                .GetAll()
                .AsNoTracking()
                .Where(testanswer => testanswer.TestId == testId)
                .ToListAsync();
        }

        public async Task<int> TestCountAsync()
        {
            return await _repository.GetAll().CountAsync();
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
