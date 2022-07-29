﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EAuction.Products.Api.Data.Abstractions;
using EAuction.Products.Api.Entities;
using EAuction.Products.Api.Repositories.Abstractions;
using MongoDB.Driver;

namespace EAuction.Products.Api.Repositories
{
    public class ProductRepository:IProductRepository
    {
        private readonly IProductContext _context;

        public ProductRepository(IProductContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProduct(string id)
        {
            var result = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
            return result;
        }      

        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> Delete(string id)
        {
            var result = false;
            var placedBids = _context.Bids.CountDocuments(b => b.ProductId == id);

            if (placedBids == 0)
            {
                var product = await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

                if (product != null)
                {
                    if (product.BidEndDate < DateTime.Now)
                    {
                        throw new Exception("The selected product cannot be deleted because Bid End date is expired");
                    }
                    else
                    {
                        var filter = Builders<Product>.Filter.Eq(m => m.Id, id);
                        DeleteResult deleteResult = await _context.Products.DeleteOneAsync(filter);
                        result = deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
                    }
                }
            }
            else
            {
                throw new Exception("The selected product has valid bids. It cannot be deleted!!");
            }
          
            return result ;
        }
    }
}