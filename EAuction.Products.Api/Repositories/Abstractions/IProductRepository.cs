using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EAuction.Products.Api.Entities;

namespace EAuction.Products.Api.Repositories.Abstractions
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(string id);
        Task Create(Product product);
        Task<bool> Delete(string id);
    }
}