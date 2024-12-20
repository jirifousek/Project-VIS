using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Domain.TransactionScripts.BusinessPartrTransactionScripts;
using Project.WebUI.Models;

namespace Project.WebUI.Controllers
{
    public class BusinessPartnerController : Controller
    {
        // GET: BusinessPartnerController
        public ActionResult Index()
        {
            var script = new GetAllBusinessPartnersTransactionScript();
            script.Execute();
            var businessPartners = script.Output;
            List<BusinessPartnerModel> models = new List<BusinessPartnerModel>();
            foreach (var businessPartner in businessPartners)
            {
                models.Add(new BusinessPartnerModel(businessPartner.Id, businessPartner.Name, businessPartner.Address, businessPartner.Incoterms, businessPartner.PaymentTerms, businessPartner.Role));
            }

            return View(models);
        }

        // GET: BusinessPartnerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BusinessPartnerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessPartnerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: BusinessPartnerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BusinessPartnerController/Edit/5
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

        // GET: BusinessPartnerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BusinessPartnerController/Delete/5
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
