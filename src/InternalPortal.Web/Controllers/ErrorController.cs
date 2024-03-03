﻿using Microsoft.AspNetCore.Mvc;

namespace InternalPortal.Web.Controllers
{
    public class ErrorController : Controller
    {
        //private readonly ILogger<ErrorController> logger;

        //public ErrorController(ILogger<ErrorController> logger)
        //{
        //    this.logger = logger;
        //}

        /// <summary>
        /// Error page.
        /// </summary>
        /// <returns>Error view</returns>
        [Route("Error")]
        public IActionResult Error()
        {
            //var exceptionHandlerPathFeature =
            //    HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //logger.LogError($"The path {exceptionHandlerPathFeature.Path} " +
            //    $"threw an exception {exceptionHandlerPathFeature.Error}");

            return View("Error");
        }

        /// <summary>
        /// If there is 404 status code, the route path will become Error/404
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns>NotFound view</returns>
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Запрашиваемая страница не найдена";
                    break;
                case 0:
                    ViewBag.ErrorMessage = "Упс что-то пошло не так.";
                    break;
            }

            return View("NotFound");
        }
    }
}
