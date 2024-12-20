using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Data.DTO;
using Project.Domain.TransactionScripts.BusinessPartrTransactionScripts;
using Project.WebUI.Models;
using System.Text;

namespace Project.WebUI.Controllers
{
    public class BusinessPartnerController : Controller
    {
        // GET: BusinessPartnerController
        [Authorize]
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
        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BusinessPartnerController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusinessPartnerController/Create
        [Authorize]
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
        [Authorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BusinessPartnerController/Edit/5
        [Authorize]
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
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BusinessPartnerController/Delete/5
        [Authorize]
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

        // GET: BusinessPartnerController/Upload
        [Authorize]
        public ActionResult Upload()
        {
            return View();
        }

        // POST: BusinessPartnerController/Upload
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(IFormFile file)
        {
            try
            {
                List<BusinessPartnerDTO> businessPartners = new List<BusinessPartnerDTO>();
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    bool firstLine = true;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (firstLine) 
                        {
                            firstLine = false;
                            continue;
                        }
                        
                        var values = line.Split(';');
                        var businessPartner = new BusinessPartnerDTO
                        {
                            Name = values[0],
                            Address = values[1],
                            Incoterms = values[2],
                            PaymentTerms = values[3],
                            Role = values[4]
                        };
                        var script = new CreateBusinessPartnerTransactionScript
                        {
                            BusinessPartner = businessPartner
                        };
                        script.Execute();
                        businessPartner.Id = script.Output;
                        businessPartners.Add(businessPartner);
                    }
                }
                var csv = new StringBuilder();
                csv.AppendLine("Id;Name;Address;Incoterms;PaymentTerms;Role");
                foreach (var item in businessPartners)
                {
                    var newLine = string.Format("{0};{1};{2};{3};{4};{5}", item.Id, item.Name, item.Address, item.Incoterms, item.PaymentTerms, item.Role);
                    csv.AppendLine(newLine);
                }
                return File(new System.Text.UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "created_business_partners.csv");

            }
            catch
            {
                TempData["Message"] = "An error occurred while uploading the file.";
                TempData["MessageType"] = "danger";
                return RedirectToAction(nameof(Index));
            }
        }
}
}
