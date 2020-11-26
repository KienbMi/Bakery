using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.Web.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _uow;

        [BindProperty]
        public Order Order { get; set; }

        public List<SelectListItem> Customers { get; set; }
        [BindProperty]
        public int CustomerId { get; set; }

        public CreateModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IActionResult> OnGet()
        {
            Order = new Order();

            Customers = (await _uow.Customers
                .GetAllAsync())
                .Select(c => new SelectListItem(
                           $"{c.FullName}", c.Id.ToString()))
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Customer customerInDb = await _uow.Customers.GetByIdAsync(CustomerId);

            _uow.Orders.Add(new Order
                {
                    OrderNr = Order.OrderNr,
                    Date = Order.Date,
                    Customer = customerInDb
                });

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch(ValidationException ex)
            {
                Order = new Order();
                Customers = (await _uow.Customers
                    .GetAllAsync())
                    .Select(c => new SelectListItem(
                               $"{c.FullName}", c.Id.ToString()))
                    .ToList();

                //ModelState.AddModelError($"{nameof(Order.OrderNr)}", ex.Message);
                ModelState.AddModelError($"", ex.Message);

                return Page();
            }

            return RedirectToPage("../Index");
        }

    }
}
