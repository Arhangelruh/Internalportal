using InternalPortal.Core.Interfaces;
using InternalPortal.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Web.Controllers
{
    public class CashController : Controller
    {
        private readonly ICashTestService _cashTestService;

        public CashController(ICashTestService cashTestService)
        {
            _cashTestService = cashTestService ?? throw new ArgumentNullException(nameof(cashTestService));
        }

        public IActionResult Cash()
        {
            return View();
        }

        public async Task<IActionResult> Test()
        {
            List<CashTestViewModel> cashTestModels = [];
            var cashTests = await _cashTestService.GetActiveCashTestsAsync();
            foreach (var cashtest in cashTests) {
                cashTestModels.Add(new CashTestViewModel
                {
                    Id = cashtest.Id,
                    CashTestName = cashtest.TestName,
                    IsActual = cashtest.IsActual
                });
            }
            return View(cashTestModels);
        }
    }
}
