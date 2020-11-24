using Bakery.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Bakery.ImportConsole
{
    public class ImportController
    {

        const string FileNameProducts = "Products.csv";
        const int Idx_ProdNr = 0;
        const int Idx_ProdName = 1;
        const int Idx_ProdPrice = 2;

        const string FileNameOrderItems = "OrderItems.csv";
        const int Idx_OrderNumber = 0;
        const int Idx_Date = 1;
        const int Idx_CustomerNr = 2;
        const int Idx_CustomerLastName = 3;
        const int Idx_CustomerFirstName = 4;
        const int Idx_ProductNr = 5;
        const int Idx_Amount = 6;

        public static IEnumerable<Product> ReadFromCsv()
        {
            string[][] matrix = MyFile.ReadStringMatrixFromCsv(FileNameProducts, true);

            //ProductNr; Name; Price
            //P311; Cola; 1
            //P322; Fanta; 0,9

            var products = matrix.Select(p => new Product
                {
                    Name = p[Idx_ProdName],
                    Price = double.Parse(p[Idx_ProdPrice]),
                    ProductNr = p[Idx_ProdNr]
                })
                .ToDictionary(_ => _.ProductNr);


            matrix = MyFile.ReadStringMatrixFromCsv(FileNameOrderItems, true);

            //OrderNr; Date; CustomerNr; LastName; FirstName; ProductNr; Amount
            //2020 / 1; 01.01.2020; K101; Berger; Herbert; P311; 3
            //2020 / 1; 01.01.2020; K101; Berger; Herbert; P322; 5

            var customers = matrix.GroupBy(_ => _[Idx_CustomerNr])
                .Select(grp => new Customer
                {
                    CustomerNr = grp.First()[Idx_CustomerNr],
                    FirstName = grp.First()[Idx_CustomerFirstName],
                    LastName = grp.First()[Idx_CustomerLastName]
                })
                .ToDictionary(_ => _.CustomerNr);

            var orders = matrix.GroupBy(_ => _[Idx_OrderNumber])
                .Select(grp => new Order
                {
                    Customer = customers[grp.First()[Idx_CustomerNr]],
                    Date = DateTime.Parse(grp.First()[Idx_Date]),
                    OrderNr = grp.First()[Idx_OrderNumber]
                })
                .ToDictionary(_ => _.OrderNr);
                       
            var orderItems = matrix
                .Select(_ => new OrderItem
                {
                    Amount = int.Parse(_[Idx_Amount]),
                    Order = orders[_[Idx_OrderNumber]],
                    Product = products[_[Idx_ProductNr]]
                })
                .ToList();

            foreach (var product in products.Values)
            {
                product.OrderItems = orderItems.Where(p => p.Product.ProductNr == product.ProductNr).ToArray();
            }

            return products.Values;
        }
    }
}
