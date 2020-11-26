﻿using Bakery.Core.Contracts;
using Bakery.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public OrdersController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderWithItemsDto>>> GetOrders()
        {
            var result = await _uow.Orders.GetAllWithItemsAsync();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/Orders/ordersByCustomerId/2
        [HttpGet("ordersByCustomerId/{id}")]
        public async Task<ActionResult<IEnumerable<OrderWithItemsDto>>> GetOrdersByCustomer(int id)
        {
            var result = await _uow.Orders.GetByIdWithItemsAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

    }
}
