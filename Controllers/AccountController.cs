using Core.UseCases;
using Infrastructure.Data;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalesWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly LoginUseCase _loginUseCase;

        public AccountController()
        {
            var context = new AppDbContext();
            var userRepository = new UserRepository(context);
            _loginUseCase = new LoginUseCase(userRepository);
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["Username"] != null)
            {
                // Check if session is still valid
                var sessionId = Session["SessionId"]?.ToString();
                if (!string.IsNullOrEmpty(sessionId))
                {
                    var context = new AppDbContext();
                    var userRepository = new UserRepository(context);
                    var user = userRepository.GetByUsername(Session["Username"].ToString());

                    if (user != null && user.SessionId == sessionId && user.LastLogin.AddHours(3) > DateTime.Now)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (_loginUseCase.Execute(username, password, out var sessionId))
            {
                Session["Username"] = username;
                Session["SessionId"] = sessionId;

                // Set session timeout to 3 hours
                Session.Timeout = 180;

                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Invalid username or password";
            return View();
        }

        public ActionResult Logout()
        {
            var sessionId = Session["SessionId"]?.ToString();
            if (!string.IsNullOrEmpty(sessionId))
            {
                var context = new AppDbContext();
                var userRepository = new UserRepository(context);
                userRepository.ClearSession(sessionId);
            }

            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}