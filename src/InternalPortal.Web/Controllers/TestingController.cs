using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using InternalPortal.Web.Constants;
using InternalPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;

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

        public TestingController(
            IOptions<ConfigurationTest> configurationTest,
            ITestService testService,
            IProfileService profileService,
            ITestScoreService testScoreService,
            ITestAnswerService testAnswerService,
            ITestQuestionService testQuestionService
            )
        {
            _configurationTest = configurationTest.Value ?? throw new ArgumentNullException(nameof(configurationTest));
            _testService = testService ?? throw new ArgumentNullException(nameof(testService));
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            _testScoreService = testScoreService ?? throw new ArgumentNullException(nameof(testScoreService));
            _testAnswerService = testAnswerService ?? throw new ArgumentNullException(nameof(testAnswerService));
            _testQuestionService = testQuestionService ?? throw new ArgumentNullException(nameof(testQuestionService));
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

                if (testData.Questions != null)
                {
                    List<TestCashQuestionViewModel> testQuestions = [];

                    foreach (var question in testData.Questions)
                    {
                        var answers = testData.Answers.Where(answer => answer.TestQuestionId == question.Id);
                        List<TestCashAnswerViewModel> testQuestionAnswers = [];
                        if (answers != null)
                        {
                            foreach (var answer in answers)
                            {
                                testQuestionAnswers.Add(new TestCashAnswerViewModel
                                {
                                    Id = answer.Id,
                                    Answer = answer.AnswerText,
                                    Choise = false
                                });
                            }
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
                        CashQuestions = testQuestions,
                        startDate = DateTime.Now
                    };

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

                foreach (var question in model.CashQuestions)
                {                    
                    foreach (var answer in question.Answers)
                    {
                        testsAnswers.Add(new TestsAnswers { AnswerId = answer.Id, AnswerStatus = answer.Choise });
                    }
                }

                var test = new Test
                {
                    StartTime = model.startDate,
                    EndTime = endDate,
                    ProfileId = profile.Id,
                    TestsAnswers = testsAnswers
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
                return View("FinishTest");
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
        public async Task<IActionResult> TestResults()
        {
            List<TestResultViewModel> testResults = [];
            var tests = await _testService.GetTestsAsync();

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

                    testResults.Add(new TestResultViewModel
                    {
                        TestId = test.Id,
                        StartDate = test.StartTime,
                        EndDate = test.EndTime,
                        Profile = profileModel,
                    });
                }
            }
            return View(testResults);
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

                foreach (var answer in questions) {
                    answer.Answers.OrderBy(a=>a.Id).ToList();
                }
            }
            var models = questions.OrderBy(q=>q.TestTopicId).ToList();
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

            foreach (var score in scores) {
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
        public async Task<IActionResult> DeleteUserScore(int profileId) { 
         
            var checkProfile =await _profileService.GetProfileByIdAsync(profileId);

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
