using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Bakery.Web.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IUnitOfWork _uow;

        public OrderDto Order { get; set; }

        public DeleteModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IActionResult> OnGet(int orderId)
        {
            Order = await _uow.Orders.GetByIdWithItemsAsync(orderId);

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int orderId)
        {
            Order orderInDb = await _uow.Orders.GetByIdAsync(orderId);

            _uow.Orders.Remove(orderInDb);
            
            try
            {
                await _uow.SaveChangesAsync();
            }
            catch(ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            
            return RedirectToPage("../Index", orderId);
        }

    }
}
