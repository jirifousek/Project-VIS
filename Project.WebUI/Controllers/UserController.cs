using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Domain.TransactionScripts.UserTransactionScripts;
using Project.WebUI.Models;
using System.Security.Claims;

namespace Project.WebUI.Controllers
{
    public class UserController : Controller
    {
        // GET: UserController
        [Authorize]
        public ActionResult Index()
        {
            var script = new GetAllBusinessPartnersTransactionScript();
            script.Execute();
            var users = script.Output;
            List<UserModel> models = new List<UserModel>();
            foreach (var user in users)
            {
                models.Add(new UserModel(user.Id, user.FirstName, user.LastName, user.Password, user.Role, user.Authorization, user.Status));
            }

            return View(models);
        }

        // GET: UserController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var script = new GetUserDetailsTransactionScript { Id = id };
            script.Execute();
            var user = script.Output;
            UserModel model = new UserModel(user.Id, user.FirstName, user.LastName, user.Password, user.Role, user.Authorization, user.Status);
            //ViewBag.User = model;
            return View(model);
        }

        // GET: UserController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var script = new CreateUserTransactionScript
                {
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    Password = collection["Password"],
                    Role = collection["Role"],
                    Authorization = collection["Authorization"],
                    Status = collection["Status"]

                };
                script.Execute();
                ViewBag.Message = "User was created";
                ViewBag.MessageType = "success";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Message = "There was and error during creation of a user";
                return View();
            }
        }

        // GET: UserController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Login
        public ActionResult Login()
        {
            return View(new UserModel(0, "", "", "", "", "", ""));
        }

        // POST: UserController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserModel model)
        {
            var script = new GetUserDetailsTransactionScript { Id = model.Id };

            script.Execute();

            if (script.Output.Status == "APPROVAL")
            {
                ViewBag.Message = "The request for creating an user was not processed yet";
                ViewBag.MessageType = "danger";
                return View();
            }

            if (script.Output.Status == "REJECTED")
            {
                ViewBag.Message = "Your reqest for creating an user was rejected";
                ViewBag.MessageType = "danger";
                return View();
            }


            if (script.Output.Password == model.Password)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, script.Output.Id.ToString()),
                    new Claim(ClaimTypes.Name, script.Output.FirstName + " " + script.Output.LastName),
                    new Claim(ClaimTypes.Role, script.Output.Authorization)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);

                TempData["Message"] = "You are logged in";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Wrong user credentials";
                ViewBag.MessageType = "danger";
                return View();
            }
        }

        // GET: UserController/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: UserController/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        // GET: UserController/CreationRequests
        [Authorize]
        [Authorize(Roles= "ADMIN")]
        public IActionResult CreationRequests()
        {
            var script = new GetUsersBasedOnStatusTransactionScript { Status = "APPROVAL" };
            script.Execute();
            var users = script.Output;
            List<UserModel> models = new List<UserModel>();
            foreach (var user in users)
            {
                models.Add(new UserModel(user.Id, user.FirstName, user.LastName, user.Password, user.Role, user.Authorization, user.Status));
            }
            return View(models);
        }

        // POST: UserController/ApproveCreationRequest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public IActionResult ApproveCreationRequest(int id)
        {
            
            var script = new GetUserDetailsTransactionScript { Id = id };
            script.Execute();
            var user = script.Output;
            user.Status = "ACTIVE";
            var updateScript = new UpdateUserTransactionScript
            {
                Id = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Role = user.Role,
                Authorization = user.Authorization,
                Status = user.Status
            };
            updateScript.Execute();
            TempData["Message"] = "The request was approved";
            TempData["MessageType"] = "success";
            return RedirectToAction("CreationRequests");
        }

        // POST: UserController/RejectCreationRequest/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public IActionResult RejectCreationRequest(int id)
        {
            var script = new GetUserDetailsTransactionScript { Id = id };
            script.Execute();
            var user = script.Output;
            user.Status = "REJECTED";
            var updateScript = new UpdateUserTransactionScript
            {
                Id = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Role = user.Role,
                Authorization = user.Authorization,
                Status = user.Status
            };
            updateScript.Execute();
            TempData["Message"] = "The request was rejected";
            TempData["MessageType"] = "success";
            return RedirectToAction("CreationRequests");
        }


    }
}
