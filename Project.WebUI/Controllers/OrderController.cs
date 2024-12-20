using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.WebUI.Models;
using Project.Domain.TransactionScripts.OrderTransactionScripts;
using Project.Domain.TransactionScripts.BusinessPartrTransactionScripts;
using Project.Domain.TransactionScripts.MaterialTransactionScripts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Project.WebUI.Controllers
{
    public class OrderController : Controller
    {
        // GET: OrderController
        public ActionResult Index()
        {
            var script = new GetAllOrdersTransactionScript();
            script.Execute();
            List<OrderModel> orders = new List<OrderModel>();
            Dictionary<int, BusinessPartnerModel> businessPartners = new Dictionary<int, BusinessPartnerModel>();
            foreach (var order in script.Output)
            {
                if (!businessPartners.ContainsKey(order.CustomerId))
                {
                    var businessPartnerScript = new GetBusinessPartnerTransactionScript();
                    businessPartnerScript.BusinessPartnerId = order.CustomerId;
                    businessPartnerScript.Execute();
                    businessPartners.Add(order.CustomerId, new BusinessPartnerModel(businessPartnerScript.Output.Id, businessPartnerScript.Output.Name, businessPartnerScript.Output.Address, businessPartnerScript.Output.Incoterms, businessPartnerScript.Output.PaymentTerms, businessPartnerScript.Output.Role));
                }
                orders.Add(new OrderModel
                {
                    Id = order.Id,
                    BueinessPaetner = businessPartners[order.CustomerId],
                    ExpectedDeliveryDate = order.ExpectedDeliveryDate,
                    TotalPrice = order.TotalPrice,
                    Status = order.Status
                });
            }

            return View(orders);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            var scriptHeader = new GetOrderTransactionScript();
            scriptHeader.Id = id;
            scriptHeader.Execute();
            var scriptItems = new GetOrderItemsTransactionScript();
            scriptItems.Id = id;
            scriptItems.Execute();
            var businessPartnerScript = new GetBusinessPartnerTransactionScript();
            businessPartnerScript.BusinessPartnerId = scriptHeader.Output.CustomerId;
            businessPartnerScript.Execute();
            var order = new OrderModel(scriptHeader.Output.Id, new BusinessPartnerModel(businessPartnerScript.Output.Id, businessPartnerScript.Output.Name, businessPartnerScript.Output.Address, businessPartnerScript.Output.Incoterms, businessPartnerScript.Output.PaymentTerms, businessPartnerScript.Output.Role), scriptHeader.Output.ExpectedDeliveryDate, scriptHeader.Output.TotalPrice, scriptHeader.Output.Status);
            order.Items = new List<OrderItemModel>();
            Dictionary<int, MaterialModel> MaterialModels = new Dictionary<int, MaterialModel>();
            foreach (var item in scriptItems.Output)
            {
                if(MaterialModels.ContainsKey(item.Id))
                {
                    var materialScript = new GetlMaterialTransactionScript();
                    materialScript.Id = item.MaterialId;
                    materialScript.Execute();
                    MaterialModels.Add(item.MaterialId, new MaterialModel(materialScript.Output.Id, materialScript.Output.Description, materialScript.Output.Stock, materialScript.Output.UnitOfMeasure, materialScript.Output.Weight, materialScript.Output.Price));
                }
                order.Items.Add(new OrderItemModel
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    Material = MaterialModels[item.MaterialId],
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }


            return View(order);
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            var businessPartnerScript = new GetAllBusinessPartnersTransactionScript();
            businessPartnerScript.Execute();
            var materialScript = new GetAllMaterialsTransactionScript();
            materialScript.Execute();
            List<BusinessPartnerModel> businessPartnerModels = new List<BusinessPartnerModel>();
            List<MaterialModel> materialModels = new List<MaterialModel>();
            foreach (var businessPartner in businessPartnerScript.Output)
            {
                businessPartnerModels.Add(new BusinessPartnerModel(businessPartner.Id, businessPartner.Name, businessPartner.Address, businessPartner.Incoterms, businessPartner.PaymentTerms, businessPartner.Role));
            }
            foreach (var material in materialScript.Output)
            {
                materialModels.Add(new MaterialModel(material.Id, material.Description, material.Stock, material.UnitOfMeasure, material.Weight, material.Price));
            }
            ViewBag.BusinessPartners = new SelectList(businessPartnerModels, "Id", "Name");
            ViewBag.Materials = new SelectList(materialModels, "Id", "Description");

            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                OrderModel model = new OrderModel();
                model.BueinessPaetner.Id = int.Parse(collection[""]);
                //var script = new CreateOrderTransactionScript();
                

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
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

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
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
