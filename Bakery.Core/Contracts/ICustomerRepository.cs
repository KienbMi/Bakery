﻿using Bakery.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bakery.Core.Contracts
{
    public interface ICustomerRepository
    {
        Task<int> GetCountAsync();
    }
}