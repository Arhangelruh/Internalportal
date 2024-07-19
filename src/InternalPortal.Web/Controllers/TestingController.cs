using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using InternalPortal.Web.Constants;
using InternalPortal.Web.Services;
using InternalPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace InternalPortal.Web.Controllers
{
    public class TestingController : Controller
    {
        private readonly ConfigurationTest _configurationTest;
        private readonly ITestService _testService;
        private readonly IProfileService _profileService;
        private readonly ITestScoreService _testScoreService;
        private readonly ITestAnswerService _testAnswerService;
        private readonly ITestQuestionService _testQuestionService;
        private readonly ICashTestService _cashTestService;
		private readonly ILogger<TestingController> _logger;

		public TestingController(
            IOptions<ConfigurationTest> configurationTest,
            ITestService testService,
            IProfileService profileService,
            ITestScoreService testScoreService,
            ITestAnswerService testAnswerService,
            ITestQuestionService testQuestionService,
            ICashTestService cashTestService,
			ILogger<TestingController> logger
			)
        {
            _configurationTest = configurationTest.Value ?? throw new ArgumentNullException(nameof(configurationTest));
            _testService = testService ?? throw new ArgumentNullException(nameof(testService));
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            _testScoreService = testScoreService ?? throw new ArgumentNullException(nameof(testScoreService));
            _testAnswerService = testAnswerService ?? throw new ArgumentNullException(nameof(testAnswerService));
            _testQuestionService = testQuestionService ?? throw new ArgumentNullException(nameof(testQuestionService));
            _cashTestService = cashTestService ?? throw new ArgumentNullException(nameof(cashTestService));
            _logger = logger ?? throw new ArgumentNullException( nameof(logger));
        }

        /// <summary>
        /// Build test.
        /// </summary>
        /// <returns>TestViewModel model</returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Test(int cashTestId)
        {
            var profileSID = User.Claims.Where(claim => claim.Type == ClaimTypes.Sid).Select(claim => claim.Value).SingleOrDefault();
            var profile = await _profileService.GetProfileByUserSIDAsync(profileSID);
            var score = await _testScoreService.GetScoreByProfileIdAsync(profile.Id);

            if (score == null || score.Score < _configurationTest.Repeat)
            {
                var testData = await _testService.BuildTestAsync(cashTestId);

                if (testData.Questions.Count() > 0)
                {
                    List<TestCashQuestionViewModel> testQuestions = [];

                    foreach (var question in testData.Questions)
                    {
                        var answers = testData.Answers.Where(answer => answer.TestQuestionId == question.Id);
                        List<TestCashAnswerViewModel> testQuestionAnswers = [];
                        if ( answers.Count() > 0)
                        {
                            foreach (var answer in answers)
                            {
                                testQuestionAnswers.Add(new TestCashAnswerViewModel
                                {
                                    Id = answer.Id,
                                    Answer = answer.AnswerText,
                                    Meaning = answer.Meaning,
                                    Choise = false
                                });
                            }                            
                        }
                        else {
							_logger.LogWarning($"При формировании теста  {profile.Name.ToString()} {profile.LastName.ToString()} нет ответов на вопрос: {question.QuestionText}");
						}

                        testQuestions.Add(new TestCashQuestionViewModel
                        {
                            Id = question.Id,
                            Question = question.QuestionText,
                            Answers = testQuestionAnswers
                        });
                    }
                    var testModel = new TestViewModel
                    {
                        CashTestId = cashTestId,
                        CashQuestions = testQuestions,
                        startDate = DateTime.Now
                    };

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    };
                    var jsonModel = JsonSerializer.Serialize(testModel,options);
                    _logger.LogInformation( profile.Name.ToString() +" "+ profile.LastName.ToString() +" Запущен тест: "+ jsonModel);

                    return View(testModel);
                }
                ViewBag.ErrorMessage = "Вопросы по тестированию не найдены.";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            ViewBag.ErrorMessage = "Вы превысили лимит по количеству тестов.";
            ViewBag.ErrorTitle = "Ошибка";
            return View("~/Views/Error/Error.cshtml");
        }

        /// <summary>
        /// Create Test.
        /// </summary>        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Test(TestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var profileSID = User.Claims.Where(claim => claim.Type == ClaimTypes.Sid).Select(claim => claim.Value).SingleOrDefault();
                var profile = await _profileService.GetProfileByUserSIDAsync(profileSID);
                var endDate = DateTime.UtcNow;

                List<TestsAnswers> testsAnswers = [];
                var wrongAnswers = 0;

                foreach(var question in model.CashQuestions)
                {
                    var checkAnswer = question.Answers.FirstOrDefault(answer => answer.Choise == true);
                    if (checkAnswer == null) {
                        ViewBag.ErrorMessage = "Тест не пройден, не получены ответы на все вопросы!!!";
                        ViewBag.ErrorTitle = "Ошибка";
                        return View("~/Views/Error/Error.cshtml");
                    }
                }

                foreach (var question in model.CashQuestions)
                {
                    foreach (var answer in question.Answers)
                    {
                        testsAnswers.Add(new TestsAnswers { AnswerId = answer.Id, AnswerStatus = answer.Choise });

                        if (answer.Choise)
                        {
                            var getAnswer = await _testAnswerService.GetAnswerByIdAsync(answer.Id);
                            if (getAnswer.Meaning != answer.Choise)
                            {
                                wrongAnswers++;
                            }
                        }
                    }
                }

                var getTest = await _cashTestService.GetCashTestByIdAsync(model.CashTestId);

                bool testResult = wrongAnswers <= getTest.WrongAnswers;

                var test = new Test
                {
                    StartTime = model.startDate,
                    EndTime = endDate,
                    ProfileId = profile.Id,
                    TestsAnswers = testsAnswers,
                    CashTestId = model.CashTestId,
                    PassResult = testResult
                };

                await _testService.AddAsync(test);
                var score = await _testScoreService.GetScoreByProfileIdAsync(profile.Id);
                if (score == null)
                {
                    var newScore = new TestScore { Score = 1, ProfileId = profile.Id };
                    await _testScoreService.AddAsync(newScore);
                }
                else
                {
                    var editScore = new TestScore
                    {
                        Id = score.Id,
                        Score = score.Score + 1,
                        ProfileId = profile.Id
                    };
                    await _testScoreService.EditAsync(editScore);
                }
                if (testResult)
                {
                    ViewBag.ResulMessage = $"Поздравляем тест сдан успешно, допущено ошибок: {wrongAnswers}";
                    return View("FinishTest");
                }
                else
                {
                    ViewBag.ResulMessage = $"Тест не пройден, допущено ошибок: {wrongAnswers}";
                    return View("FinishTest");
                }

            }
            return View(model);
        }

        public IActionResult FinishTest()
        {
            return View();
        }

        /// <summary>
        /// Get test results.
        /// </summary>
        /// <returns>List test results</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> TestResults(string sortOrder, int? page, int? pagesize)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["UserSortParm"] = sortOrder == "User" ? "User_desc" : "User";
            ViewData["DateBeginSortParm"] = sortOrder == "DateBegin" ? "DateBegin_desc" : "DateBegin";
            ViewData["DateEndSortParm"] = sortOrder == "DateEnd" ? "DateEnd_desc" : "DateEnd";
            ViewData["TestNameSortParam"] = sortOrder == "TestName" ? "TestName_desc" : "TestName";
            ViewData["TestResultSortParam"] = sortOrder == "TestResult" ? "TestResult_desc" : "TestResult";


            int pageSize = (int)(pagesize == null ? 20 : pagesize);
            ViewData["PageSize"] = pageSize;

            List<TestResultViewModel> testResults = [];
            var tests = await _testService.GetTestsAsync(sortOrder, page ?? 0, pageSize);
            var getCount = await _testService.TestCountAsync();

            if (tests != null)
            {
                foreach (var test in tests)
                {
                    var profile = await _profileService.GetProfileByIdAsync(test.ProfileId);
                    var profileModel = new ProfileViewModel
                    {
                        Name = profile.Name,
                        LastName = profile.LastName,
                        MiddleName = profile.MiddleName,
                        Sid = profile.UserSid
                    };

                    var getTest = await _cashTestService.GetCashTestByIdAsync(test.CashTestId);

                    testResults.Add(new TestResultViewModel
                    {
                        TestId = test.Id,
                        StartDate = test.StartTime,
                        EndDate = test.EndTime,
                        Profile = profileModel,
                        TestName = getTest.TestName,
                        TestResult = test.PassResult
                    });
                }
            }            
            var paginatedList = new PaginatedList<TestResultViewModel>(testResults, getCount, page ?? 0, pageSize);
            return View(paginatedList);
        }

        /// <summary>
        /// Get test results.
        /// </summary>
        /// <returns>List test results</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> UserResult(int testId)
        {
            var testAnswers = await _testService.GetQuestionAnswersAsync(testId);
            List<TestCashQuestionViewModel> questions = [];

            if (testAnswers.Count != 0)
            {
                foreach (var testAnswer in testAnswers)
                {
                    var answer = await _testAnswerService.GetAnswerByIdAsync(testAnswer.AnswerId);
                    var question = await _testQuestionService.GetQuestionByIdAsync(answer.TestQuestionId);

                    var checkQuestion = questions.FirstOrDefault(q => q.Id == question.Id);

                    var answerModel = new TestCashAnswerViewModel
                    {
                        Id = answer.Id,
                        Answer = answer.AnswerText,
                        Meaning = answer.Meaning,
                        Choise = testAnswer.AnswerStatus
                    };

                    if (checkQuestion == null)
                    {

                        var newQuestion = new TestCashQuestionViewModel
                        {
                            Id = question.Id,
                            Question = question.QuestionText,
                            TestTopicId = question.TestTopicId,
                            Answers = []
                        };
                        newQuestion.Answers.Add(answerModel);
                        questions.Add(newQuestion);
                    }
                    else
                    {
                        questions.Find(c => c.Id == checkQuestion.Id).Answers.Add(answerModel);
                    }
                }

                foreach (var answer in questions)
                {
                    answer.Answers.OrderBy(a => a.Id).ToList();
                }
            }
            var models = questions.OrderBy(q => q.TestTopicId).ToList();
            return View(models);
        }

        /// <summary>
        /// Get users scores.
        /// </summary>
        /// <returns>List test results</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> GetUserScores()
        {
            var scores = await _testScoreService.GetScoresAsync();
            List<TestScoreViewModel> models = [];

            foreach (var score in scores)
            {
                var profile = await _profileService.GetProfileByIdAsync(score.ProfileId);

                models.Add(new TestScoreViewModel
                {
                    ProfileId = profile.Id,
                    UserName = profile.Name,
                    LastName = profile.LastName,
                    MiddleName = profile.MiddleName,
                    Score = score.Score
                });
            }

            return View(models);
        }

        /// <summary>
        /// Delete user score.
        /// </summary>
        /// <returns>Get user score view</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        public async Task<IActionResult> DeleteUserScore(int profileId)
        {

            var checkProfile = await _profileService.GetProfileByIdAsync(profileId);

            if (checkProfile != null)
            {
                await _testScoreService.DeleteScoreAsync(profileId);
                return RedirectToAction("GetUserScores");
            }
            ViewBag.ErrorMessage = "Профиль не найден.";
            ViewBag.ErrorTitle = "Ошибка";
            return View("~/Views/Error/Error.cshtml");
        }
    }
}
