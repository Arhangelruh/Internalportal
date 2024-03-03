using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using InternalPortal.Web.Constants;
using InternalPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Web.Controllers
{
    public class TestController : Controller
    {
        private readonly ICashTestService _cashTestService;
        private readonly ITestTopicService _testTopicService;
        private readonly ITestQuestionService _testQuestionService;
        private readonly ITestAnswerService _testAnswerService;

        public TestController(ITestTopicService topicService, ITestQuestionService testQuestion, ITestAnswerService testAnswer, ICashTestService cashTestService)
        {
            _testAnswerService = testAnswer ?? throw new ArgumentNullException(nameof(testAnswer));
            _testTopicService = topicService ?? throw new ArgumentNullException(nameof(topicService));
            _testQuestionService = testQuestion ?? throw new ArgumentException(nameof(testQuestion));
            _cashTestService = cashTestService ?? throw new ArgumentException(nameof(cashTestService));
        }

        /// <summary>
        /// Model for create CashTest.
        /// </summary>
        /// <returns>View model for create Cash test</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public IActionResult AddCashTest()
        {
            return View();
        }

        /// <summary>
        /// Create Cash test.
        /// </summary>
        /// <returns>Cash test View</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCashTest(CashTestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var cashTest = new CashTest
                {
                    TestName = model.CashTestName,
                    IsActual = true
                };

                await _cashTestService.AddCashTestAsync(cashTest);
            }
            return RedirectToAction("GetCashTests", "Test");
        }

        /// <summary>
        /// Get Cash test List.
        /// </summary>
        /// <returns>List cash tests.</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> GetCashTests()
        {
            var cashTests = new List<CashTestViewModel>();
            var getTests = await _cashTestService.GetAllCashTestsAsync();
            if (getTests.Count != 0)
            {
                foreach (var test in getTests)
                {
                    cashTests.Add(
                        new CashTestViewModel
                        {
                            Id=test.Id,
                            CashTestName = test.TestName,
                            IsActual=test.IsActual
                        });                    
                }
            }
            return View(cashTests);
        }

        /// <summary>
        /// Model edit Cash test.
        /// </summary>
        /// <returns>View model for edit Cash test</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> EditCashTest(int cashTestId)
        {
            var getCashTest = await _cashTestService.GetCashTestByIdAsync(cashTestId);
            if (getCashTest != null)
            {
                var test = new CashTestViewModel {
                    Id = getCashTest.Id,
                    CashTestName = getCashTest.TestName,
                    IsActual = getCashTest.IsActual
                };
                
                return View(test);
            }
            else
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
        }

        /// <summary>
        /// Edit Cash test.
        /// </summary>
        /// <param name="editCashTest">Cash test model</param>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCashTest(CashTestViewModel editCashTest)
        {
            if (ModelState.IsValid)
            {
                var model = new CashTest
                {
                    Id = editCashTest.Id,
                    TestName = editCashTest.CashTestName,
                    IsActual = editCashTest.IsActual
                };

                await _cashTestService.EditCashTestAsync(model);
                return RedirectToAction("GetCashTests", "Test");
            }
            else
            {
                return View(editCashTest);
            }
        }

        /// <summary>
        /// Delete Cash Test.
        /// </summary>
        /// <returns>Result</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> DeleteCashTest(int cashTestId)
        {
            var getCashTest = await _cashTestService.GetCashTestByIdAsync(cashTestId);
            if (getCashTest == null)
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            else
            {
                var result = await _cashTestService.DeleteCashTestAsync(getCashTest.Id);
                if (result)
                {
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }
            }
        }

        /// <summary>
        /// Change cash test status.
        /// </summary>
        /// <param name="topicId">Topic id</param>       
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> ChangeCashTestStatus(int cashTestId)
        {
            var getTest = await _cashTestService.GetCashTestByIdAsync(cashTestId);
            if (getTest == null)
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            else
            {
                getTest.IsActual = getTest.IsActual == true ? false : true;
                await _cashTestService.ChangecashTestStatusAsync(getTest);
                return RedirectToAction("GetCashTests", "Test");
            }
        }

        /// <summary>
        /// Get Topic List.
        /// </summary>
        /// <returns>List topics.</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> GetTestTopics(int cashTestId)
        {
            var topics = new List<TestTopicViewModel>();
            var getTopics = await _testTopicService.GetTopicsByCashTestAsync(cashTestId);
            if (getTopics != null)
            {
                foreach (var topic in getTopics)
                {
                    topics.Add(new TestTopicViewModel
                    {
                        Id = topic.Id,
                        TopicName = topic.TopicName,
                        IsActual = topic.IsActual,
                        CashTestId = cashTestId
                    });
                }
            }
            ViewBag.IdCashTest = cashTestId;
            return View(topics);
        }

        /// <summary>
        /// Model for create Topic.
        /// </summary>
        /// <returns>View model for create Topic</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> AddTestTopic(int cashTestId)
        {
            var getcashTest = await _cashTestService.GetCashTestByIdAsync(cashTestId);
            if (getcashTest != null)
            {
                var topic = new TestTopicViewModel { CashTestId = getcashTest.Id };

                return View(topic);
            }
            else
            {
                ViewBag.ErrorMessage = "Тест не найден";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }            
        }

        /// <summary>
        /// Create Topic.
        /// </summary>
        /// <returns>Topic View</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTestTopic(TestTopicViewModel model)
        {
            if (ModelState.IsValid)
            {
                var topic = new TestTopics
                {
                    TopicName = model.TopicName,
                    IsActual = true,
                    CashTestId=model.CashTestId
                };

                await _testTopicService.AddAsync(topic);
            }
            return RedirectToAction("GetTestTopics", "Test", new { cashTestId = model.CashTestId });
        }

        /// <summary>
        /// Model edit Topic.
        /// </summary>
        /// <returns>View model for edit Topic</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> EditTopic(int topicId)
        {
            var getTopic = await _testTopicService.GetTestTopicByIdAsync(topicId);
            if (getTopic != null)
            {
                var topic = new TestTopicViewModel
                {
                    Id = getTopic.Id,
                    TopicName = getTopic.TopicName,
                    IsActual = true,
                    CashTestId = getTopic.CashTestId
                };
                return View(topic);
            }
            else
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
        }

        /// <summary>
        /// Edit Topic.
        /// </summary>
        /// <param name="editTopic">Topic model</param>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTopic(TestTopicViewModel editTopic)
        {
            if (ModelState.IsValid)
            {
                var model = new TestTopics
                {
                    Id = editTopic.Id,
                    TopicName = editTopic.TopicName,
                    IsActual = editTopic.IsActual
                };

                await _testTopicService.EditAsync(model);
                return RedirectToAction("GetTestTopics", "Test", new { cashTestId = editTopic.CashTestId });
            }
            else
            {
                return View(editTopic);
            }
        }

        /// <summary>
        /// Delete Topic.
        /// </summary>
        /// <returns>Result</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> DeleteTopic(int topicId)
        {
            var getTopic = await _testTopicService.GetTestTopicByIdAsync(topicId);
            if (getTopic == null)
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            else
            {
                var result = await _testTopicService.DeleteAsync(getTopic.Id);
                if (result)
                {
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }
            }
        }

        /// <summary>
        /// Change topic status.
        /// </summary>
        /// <param name="topicId">Topic id</param>       
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> ChangeTopicStatus(int topicId)
        {
            var getTopic = await _testTopicService.GetTestTopicByIdAsync(topicId);
            if (getTopic == null)
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            else
            {
                getTopic.IsActual = getTopic.IsActual == true ? false : true;
                await _testTopicService.ChangeStatusAsync(getTopic);
                return RedirectToAction("GetTestTopics", "Test", new { cashTestId = getTopic.CashTestId });
            }
        }

        /// <summary>
        /// Get Questions List.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> GetTestQuestions(int topicId)
        {
            var questions = new List<TestQuestionViewModel>();
            var getQuestions = await _testQuestionService.GetQuestionByTopicAsync(topicId);
            if (getQuestions != null)
            {
                foreach (var question in getQuestions)
                {
                    questions.Add(new TestQuestionViewModel
                    {
                        Id = question.Id,
                        QuestionText = question.QuestionText,
                        IsActual = question.IsActual,
                        TestTopicId = question.TestTopicId
                    });
                }
            }
            ViewBag.IdTopic = topicId;
            return View(questions);
        }

        /// <summary>
        /// Model for create Question.
        /// </summary>
        /// <returns>View model for create Question</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> AddTestQuestion(int topicId)
        {
            var getTopic = await _testTopicService.GetTestTopicByIdAsync(topicId);
            if (getTopic != null)
            {
                var question = new TestQuestionViewModel { TestTopicId = getTopic.Id };

                return View(question);
            }
            else
            {
                ViewBag.ErrorMessage = "Тема не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
        }

        /// <summary>
        /// Create Question.
        /// </summary>
        /// <returns>Questions View</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTestQuestion(TestQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var question = new TestQuestions
                {
                    QuestionText = model.QuestionText,
                    TestTopicId = model.TestTopicId,
                    IsActual = true
                };

                await _testQuestionService.AddAsync(question);
                return RedirectToAction("GetTestQuestions", new { topicId = model.TestTopicId });
            }

            return View(model);
        }

        /// <summary>
        /// Model edit Question.
        /// </summary>
        /// <returns>View model for edit Question</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> EditQuestion(int questionId)
        {
            var getQuestion = await _testQuestionService.GetQuestionByIdAsync(questionId);
            if (getQuestion != null)
            {
                var question = new TestQuestionViewModel
                {
                    Id = getQuestion.Id,
                    QuestionText = getQuestion.QuestionText,
                    IsActual = getQuestion.IsActual,
                    TestTopicId = getQuestion.TestTopicId
                };
                return View(question);
            }
            else
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
        }

        /// <summary>
        /// Edit Question.
        /// </summary>
        /// <param name="editQuestion"> model</param>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditQuestion(TestQuestionViewModel editQuestion)
        {
            if (ModelState.IsValid)
            {
                var model = new TestQuestions
                {
                    Id = editQuestion.Id,
                    QuestionText = editQuestion.QuestionText,
                    IsActual = editQuestion.IsActual,
                    TestTopicId = editQuestion.TestTopicId
                };

                var result = await _testQuestionService.EditAsync(model);

                if (result)
                {
                    return RedirectToAction("GetTestQuestions", new { topicId = model.TestTopicId });
                }
                else
                {
                    ViewBag.ErrorMessage = "Не возможно изменить вопрос, возможно по нему уже было проведено тестирование";
                    ViewBag.ErrorTitle = "Ошибка";
                    return View("~/Views/Error/Error.cshtml");
                }
            }
            else
            {
                return View(editQuestion);
            }
        }

        /// <summary>
        /// Delete Question.
        /// </summary>
        /// <returns>Result</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            var getQuestion = await _testQuestionService.GetQuestionByIdAsync(questionId);
            if (getQuestion == null)
            {
                ViewBag.ErrorMessage = "Вопрос не найден.";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            else
            {
                var result = await _testQuestionService.DeleteAsync(getQuestion.Id);
                if (result)
                {
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }
            }
        }

        /// <summary>
        /// Change Question status.
        /// </summary>
        /// <param name="questionId">Question id</param>       
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> ChangeQuestionStatus(int questionId)
        {
            var getQuestion = await _testQuestionService.GetQuestionByIdAsync(questionId);
            if (getQuestion == null)
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            else
            {
                getQuestion.IsActual = getQuestion.IsActual == true ? false : true;
                await _testQuestionService.ChangeStatusAsync(getQuestion);
                return RedirectToAction("GetTestQuestions", new { topicId = getQuestion.TestTopicId });
            }
        }

        /// <summary>
        /// Get Answers List.
        /// </summary>
        /// <returns>Answer list</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> GetAnswers(int questionId)
        {
            var answers = new List<TestQuestionAnswerViewModel>();
            var getAnswers = await _testAnswerService.GetAnswersByQuestionAsync(questionId);
            if (getAnswers != null)
            {
                foreach (var answer in getAnswers)
                {
                    answers.Add(new TestQuestionAnswerViewModel
                    {
                        Id = answer.Id,
                        AnswerText = answer.AnswerText,
                        IsActual = answer.IsActual,
                        Meaning = answer.Meaning,
                        TestQuestionId = answer.TestQuestionId,
                    });
                }
            }
            ViewBag.IdQuestion = questionId;
            return View(answers);
        }

        /// <summary>
        /// Model for create Answer.
        /// </summary>
        /// <returns>View model for create Answer</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> AddTestAnswer(int questionId)
        {
            var getQuestion = await _testQuestionService.GetQuestionByIdAsync(questionId);
            if (getQuestion != null)
            {
                var answer = new TestQuestionAnswerViewModel { TestQuestionId = getQuestion.Id };

                return View(answer);
            }
            else
            {
                ViewBag.ErrorMessage = "Тема не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
        }

        /// <summary>
        /// Create Answer.
        /// </summary>
        /// <returns>Answer View</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTestAnswer(TestQuestionAnswerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var answer = new TestQuestionAnswers
                {
                    AnswerText = model.AnswerText,
                    TestQuestionId = model.TestQuestionId,
                    Meaning = model.Meaning,
                    IsActual = true
                };

                await _testAnswerService.AddAsync(answer);
                return RedirectToAction("GetAnswers", new { questionId = model.TestQuestionId });
            }
            return View(model);
        }

        /// <summary>
        /// Model edit Answer.
        /// </summary>
        /// <returns>View model for edit Answer </returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> EditAnswer(int answerId)
        {
            var getAnswer = await _testAnswerService.GetAnswerByIdAsync(answerId);
            if (getAnswer != null)
            {
                var answer = new TestQuestionAnswerViewModel
                {
                    Id = getAnswer.Id,
                    AnswerText = getAnswer.AnswerText,
                    Meaning = getAnswer.Meaning,
                    IsActual = getAnswer.IsActual,
                    TestQuestionId = getAnswer.TestQuestionId
                };
                return View(answer);
            }
            else
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
        }

        /// <summary>
        /// Edit Answer.
        /// </summary>
        /// <param name="editAnswer"> model</param>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAnswer(TestQuestionAnswerViewModel editAnswer)
        {
            if (ModelState.IsValid)
            {
                var model = new TestQuestionAnswers
                {
                    Id = editAnswer.Id,
                    AnswerText = editAnswer.AnswerText,
                    Meaning = editAnswer.Meaning,
                    IsActual = editAnswer.IsActual,
                    TestQuestionId = editAnswer.TestQuestionId
                };

                var result = await _testAnswerService.EditAsync(model);

                if (result)
                {
                    return RedirectToAction("GetAnswers", new { questionId = model.TestQuestionId });
                }
                else
                {
                    ViewBag.ErrorMessage = "Не возможно изменить ответ, возможно он уже был применен во время тестирования.";
                    ViewBag.ErrorTitle = "Ошибка";
                    return View("~/Views/Error/Error.cshtml");
                }
            }
            else
            {
                return View(editAnswer);
            }
        }

        /// <summary>
        /// Delete Answer.
        /// </summary>
        /// <returns>Result</returns>
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> DeleteAnswer(int answerId)
        {
            var getAnswer = await _testAnswerService.GetAnswerByIdAsync(answerId);
            if (getAnswer == null)
            {
                ViewBag.ErrorMessage = "Ответ не найден.";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            else
            {
                var result = await _testAnswerService.DeleteAsync(getAnswer.Id);
                if (result)
                {
                    return Json("success");
                }
                else
                {
                    return Json("error");
                }
            }
        }

        /// <summary>
        /// Change Answer status.
        /// </summary>
        /// <param name="answerId">Answer id</param>       
        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpGet]
        public async Task<IActionResult> ChangeAnswerStatus(int answerId)
        {
            var getAnswer = await _testAnswerService.GetAnswerByIdAsync(answerId);
            if (getAnswer == null)
            {
                ViewBag.ErrorMessage = "Запись не найдена";
                ViewBag.ErrorTitle = "Ошибка";
                return View("~/Views/Error/Error.cshtml");
            }
            else
            {
                getAnswer.IsActual = getAnswer.IsActual == true ? false : true;
                await _testAnswerService.ChangeStatusAsync(getAnswer);
                return RedirectToAction("GetAnswers", new { questionId = getAnswer.TestQuestionId });
            }
        }
    }
}
