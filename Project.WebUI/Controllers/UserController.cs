using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Domain.TransactionScripts.UserTransactionScripts;
using Project.WebUI.Models;

namespace Project.WebUI.Controllers
{
    public class UserController : Controller
    {
        // GET: UserController
        public ActionResult Index()
        {
            var script = new GetAllUsersTransactionScript();
            script.Execute();
            var users = script.Output;
            List<UserModel> models = new List<UserModel>();
            foreach (var user in users)
            {
                models.Add(new UserModel(user.Id, user.FirstName, user.LastName, user.Password, user.Role));
            }

            return View(models);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            var script = new GetUserDetailsTransactionScript { Id = id };
            script.Execute();
            var user = script.Output;
            UserModel model = new UserModel(user.Id, user.FirstName, user.LastName, user.Password, user.Role);
            //ViewBag.User = model;
            return View(model);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var script = new CreateUserTransactionScript
                {
                    FirstName = collection["FirstName"],
                    LastName = collection["LastName"],
                    Password = collection["Password"],
                    Role = collection["Role"]
                };
                script.Execute();
                ViewBag.Message = "User was created";
                ViewBag.MessageType = "success";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Message = "Failed to create user";
                return View();
            }
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}
