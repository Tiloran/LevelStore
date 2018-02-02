﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevelStore.Models;
using LevelStore.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LevelStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (!cart.Lines.Any())
            {
                ModelState.AddModelError("", "Ваша корзина пуста!");
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
        
        public ViewResult ListOrder() => View(repository.Orders);

        [HttpPost]
        public IActionResult ChangeStatus(int status, int orderId)
        {
            if (Enum.IsDefined(typeof(OrderStatus), status))
            {
                repository.ChangeStatus((OrderStatus)status, orderId);
            }
            return RedirectToAction(nameof(ListOrder));
        }

        public IActionResult ChangeOrder(int orderId)
        {
            Order order = repository.Orders.FirstOrDefault(i => i.OrderID == orderId);
            TempData["JsonOrder"] = JsonConvert.SerializeObject(order);
            if (order == null)
            {
                return RedirectToAction("ListOrder");
            }
            return View("ViewSingleOrder", order);
        }

        [HttpPost]
        public ViewResult ChangeOrder(Order order)
        {
            return View("ListOrder");
        }

        [HttpPost]
        public IActionResult ChangeOrderAjax([FromBody] Order order)
        {
            if (order != null && Enum.IsDefined(typeof(OrderStatus), order.Status))
            {
                repository.ChangeOrder(order);
                return Json("Succes");
            }
            return Json("Fail");
        }
        

    }
}