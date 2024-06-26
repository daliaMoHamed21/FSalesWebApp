using Core.UseCases;
using Infrastructure.Data;
using Infrastructure.Repositories;
using SalesWebApp.Models;
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
        private readonly RegisterUseCase _registerUseCase;

        public AccountController()
        {
            var context = new AppDbContext();
            var userRepository = new UserRepository(context);
            _loginUseCase = new LoginUseCase(userRepository);
            _registerUseCase = new RegisterUseCase(userRepository);
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
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_loginUseCase.Execute(model.Username, model.Password, out var sessionId))
                {
                    Session["Username"] = model.Username;
                    Session["SessionId"] = sessionId;

                    // Set session timeout to 3 hours
                    Session.Timeout = 180;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid username or password";
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ViewBag.ErrorMessage = "Passwords do not match.";
                    return View(model);
                }

                if (_registerUseCase.Execute(model.Username, model.Password))
                {
                    return RedirectToAction("Login");
                }
                ViewBag.ErrorMessage = "Registration failed. Please try again.";
            }
            // If ModelState is not valid, return the view with validation errors
            return View(model);
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