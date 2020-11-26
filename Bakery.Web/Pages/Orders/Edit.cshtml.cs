using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Bakery.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public List<SelectListItem> Products { get; set; }
        [BindProperty]
        public int SelectedProductId { get; set; }

        [BindProperty]
        [Range(1, double.MaxValue, ErrorMessage = "Mindestbestellmenge: 1")]
        public int Amount { get; set; }

        public async Task<IActionResult> OnGet(int orderId)
        {
            await LoadDataAsync(orderId);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteItem(int itemId)
        {

            OrderItem orderItemInDb = await _uow.OrderItems.GetByIdAsync(itemId);
            int orderId = orderItemInDb.OrderId;
            _uow.OrderItems.Remove(orderItemInDb);

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            await LoadDataAsync(orderId);

            return Page();
        }

        public async Task<IActionResult> OnPostAddItem(int orderId)
        {
            if (!ModelState.IsValid)
            {
                await LoadDataAsync(orderId);
                return Page();
            }

            _uow.OrderItems.Add(new OrderItem()
                    {
                        OrderId = orderId,
                        ProductId = SelectedProductId,
                        Amount = Amount
                    });

            try
            {
                await _uow.SaveChangesAsync();
            }
            catch(ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            await LoadDataAsync(orderId);

            return Page();
        }

        private async Task LoadDataAsync(int orderId)
        {
            Products = (await _uow.Products.GetAllAsync())
                            .Select(p => new SelectListItem(
                                $"{p.Name}", p.Id.ToString()))
                            .ToList();        
            
            OrderWithItems = await _uow.Orders.GetByIdWithItemsAsync(orderId);

            if (OrderWithItems == null)
            {
                OrderWithItems = new OrderWithItemsDto();
            }
        }
    }
}

