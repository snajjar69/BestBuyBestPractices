using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace BestBuyCRUD
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _connection;

        //constructor - and will do some setup work for us
        public ProductRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _connection.Query<Product>("SELECT * FROM products;");
        }
    }
}
