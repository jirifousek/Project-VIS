using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.WebUI.Models;
using Project.Domain.TransactionScripts.OrderTransactionScripts;
using Project.Domain.TransactionScripts.BusinessPartrTransactionScripts;
using Project.Domain.TransactionScripts.MaterialTransactionScripts;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Data.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Project.WebUI.Controllers
{
    public class OrderController : Controller
    {
        // GET: OrderController
        [Authorize]
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
                    BusinessPartner = businessPartners[order.CustomerId],
                    ExpectedDeliveryDate = order.ExpectedDeliveryDate,
                    TotalPrice = order.TotalPrice,
                    Status = order.Status
                });
            }

            return View(orders);
        }

        // GET: OrderController/Details/5
        [Authorize]
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
                if(!MaterialModels.ContainsKey(item.Id))
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
        [Authorize]
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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                List<OrderItemDTO> items = new List<OrderItemDTO>();
                var script = new GetAllMaterialsTransactionScript();
                script.Execute();
                List<MaterialDTO> materials = script.Output;
                double totalPrice = 0;
                Console.WriteLine($"Item count: {collection["itemCount"]}");
                for (int i = 0; i < int.Parse(collection["itemCount"]); i++)
                {
                    var material = materials.Find(m => m.Id == int.Parse(collection[$"OrderItems[{i}].MaterialId"]));
                    if (material.Stock < int.Parse(collection[$"OrderItems[{i}].Quantity"]))
                    {
                        TempData["Message"] = $"Not enough stock for {material.Description}";
                        TempData["MessageType"] = "danger";
                        return RedirectToAction(nameof(Create));
                    }

                    items.Add(new OrderItemDTO
                    {
                        Id = i,
                        MaterialId = int.Parse(collection[$"OrderItems[{i}].MaterialId"]),
                        Quantity = int.Parse(collection[$"OrderItems[{i}].Quantity"]),
                        Price = material.Price * int.Parse(collection[$"OrderItems[{i}].Quantity"])
                    });
                    totalPrice += material.Price * int.Parse(collection[$"OrderItems[{i}].Quantity"]);
                }


                OrderHeaderDTO orderHeaderDTO = new OrderHeaderDTO
                {
                    CustomerId = int.Parse(collection["BusinessPartner.Id"]),
                    ExpectedDeliveryDate = DateTime.Parse(collection["ExpectedDeliveryDate"]),
                    TotalPrice = totalPrice,
                    Status = collection["Status"]
                };

                var createOrderScript = new CreateOrderTransactionScript(orderHeaderDTO, items);
                createOrderScript.Execute();
                if (createOrderScript.Output != -1) {
                    foreach (var item in items)
                    {
                        var material = materials.Find(m => m.Id == item.MaterialId);
                        var updateMaterialScript = new UpdateMaterialTransactionScript();
                        updateMaterialScript.Material = new MaterialDTO
                        {
                            Id = item.MaterialId,
                            Description = material.Description,
                            Stock = material.Stock - item.Quantity,
                            UnitOfMeasure = material.UnitOfMeasure,
                            Weight = material.Weight,
                            Price = material.Price
                        };
                        updateMaterialScript.Execute();
                    }
                    TempData["Message"] = $"Order {createOrderScript.Output} created successfully";
                    TempData["MessageType"] = "success";
                    return RedirectToAction(nameof(Index));
                } else {
                    TempData["Message"] = $"Error creating order";
                    TempData["MessageType"] = "danger";
                    return RedirectToAction(nameof(Create));
                }
            }
            catch
            {
                TempData["Message"] = $"Error creating order";
                TempData["MessageType"] = "danger";
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: OrderController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var script = new GetOrderTransactionScript();
            script.Id = id;
            script.Execute();
            var businessPartnerScript = new GetBusinessPartnerTransactionScript();
            businessPartnerScript.BusinessPartnerId = script.Output.CustomerId;
            businessPartnerScript.Execute();
            var order = new OrderModel(script.Output.Id, new BusinessPartnerModel(businessPartnerScript.Output.Id, businessPartnerScript.Output.Name, businessPartnerScript.Output.Address, businessPartnerScript.Output.Incoterms, businessPartnerScript.Output.PaymentTerms, businessPartnerScript.Output.Role), script.Output.ExpectedDeliveryDate, script.Output.TotalPrice, script.Output.Status);
            var itemScript = new GetOrderItemsTransactionScript();
            itemScript.Id = id;
            itemScript.Execute();
            order.Items = new List<OrderItemModel>();
            foreach (var item in itemScript.Output)
            {
                var materialScript = new GetlMaterialTransactionScript();
                materialScript.Id = item.MaterialId;
                materialScript.Execute();
                order.Items.Add(new OrderItemModel
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    Material = new MaterialModel(materialScript.Output.Id, materialScript.Output.Description, materialScript.Output.Stock, materialScript.Output.UnitOfMeasure, materialScript.Output.Weight, materialScript.Output.Price),
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }


            return View(order);
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var script = new UpdateStatusTransactionScript();
                script.Id = id;
                script.Status = collection["Status"];
                script.Execute();
                TempData["Message"] = $"Order {id} updated successfully";
                TempData["MessageType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Message"] = "Error updating order";
                TempData["MessageType"] = "danger";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: OrderController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
