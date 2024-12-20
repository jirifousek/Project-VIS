using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Data.DTO;
using Project.Domain.TransactionScripts;
using Project.Domain.TransactionScripts.MaterialTransactionScripts;
using Project.WebUI.Models;

namespace Project.WebUI.Controllers
{
    public class MaterialController : Controller
    {
        // GET: MaterialController
        [Authorize]
        public ActionResult Index()
        {
            var script = new GetAllMaterialsTransactionScript();
            script.Execute();
            List<MaterialModel> materials = new List<MaterialModel>();
            foreach (var item in script.Output)
            {
                materials.Add(new MaterialModel(item.Id, item.Description, item.Stock, item.UnitOfMeasure, item.Weight, item.Price));
            }
            return View(materials);
        }

        // GET: MaterialController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var script = new GetlMaterialTransactionScript();
            script.Id = id;
            script.Execute();
            var material = new MaterialModel(script.Output.Id, script.Output.Description, script.Output.Stock, script.Output.UnitOfMeasure, script.Output.Weight, script.Output.Price);
            return View(material);
        }

        // GET: MaterialController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MaterialController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var script = new CreateMaterialTransactionScript();
                script.Material = new MaterialDTO
                {
                    Description = collection["Description"],
                    Stock = int.Parse(collection["Stock"]),
                    UnitOfMeasure = collection["UnitOfMeasure"],
                    Weight = int.Parse(collection["Weight"]),
                    Price = double.Parse(collection["Price"])
                };
                script.Execute();
                if (script.Output > 0)
                {
                    TempData["Message"] = $"Material {script.Output} created successfully";
                    TempData["MessageType"] = "success";
                }
                else
                {
                    TempData["Message"] = "Material creation failed";
                    TempData["MessageType"] = "danger";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Message"] = "Material creation failed";
                TempData["MessageType"] = "danger";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: MaterialController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var script = new GetlMaterialTransactionScript();
            script.Id = id;
            script.Execute();
            var material = new MaterialModel(script.Output.Id, script.Output.Description, script.Output.Stock, script.Output.UnitOfMeasure, script.Output.Weight, script.Output.Price);
            return View(material);
        }

        // POST: MaterialController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var script = new UpdateMaterialTransactionScript();
                script.Material = new MaterialDTO
                {
                    Id = id,
                    Description = collection["Description"],
                    Stock = int.Parse(collection["Stock"]),
                    UnitOfMeasure = collection["UnitOfMeasure"],
                    Weight = int.Parse(collection["Weight"]),
                    Price = double.Parse(collection["Price"])
                };
                script.Execute();
                TempData["Message"] = "Material updated successfully";
                TempData["MessageType"] = "success";

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Message"] = "Material update failed";
                TempData["MessageType"] = "danger";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: MaterialController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MaterialController/Delete/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var script = new DeleteMaterialTransactionScript();
                script.Id = id;
                script.Execute();
                TempData["Message"] = "Material deleted successfully";
                TempData["MessageType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Message"] = "Material delete failed";
                TempData["MessageType"] = "danger";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
