using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.Web.Pages
{
    public class EditModel : PageModel
    {
        private readonly IUnitOfWork _uow;

        public EditModel(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public OrderWithItemsDto OrderWithItems { get; set; }

        public async Task<IActionResult> OnGet(int orderId)
        {

            OrderWithItems = await _uow.Orders.GetByIdWithItemsAsync(orderId);

            if (OrderWithItems == null)
            {
                OrderWithItems = new OrderWithItemsDto();
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostDelete(int itemId)
        {

            OrderItem orderItemInDb = await _uow.OrderItems.GetByIdAsync(itemId);
            int orderId = orderItemInDb.OrderId;
            _uow.OrderItems.Remove(orderItemInDb);

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch(ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            
            OrderWithItems = await _uow.Orders.GetByIdWithItemsAsync(orderId);

            return Page();
        }

    }
}
