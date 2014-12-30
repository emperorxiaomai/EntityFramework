// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using EntityFramework.Microbenchmarks.Core.Models.Orders;
using Microsoft.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EntityFramework.Microbenchmarks.Models.Orders
{
    public class OrdersSeedData : OrdersSeedDataBase
    {
        public void EnsureCreated(
           string connectionString,
           int productCount,
           int customerCount,
           int ordersPerCustomer,
           int linesPerOrder)
        {
            using (var context = new OrdersContext(connectionString))
            {
                if (!context.Database.AsRelational().Exists())
                {
                    context.Database.EnsureCreated();
                    InsertSeedData(connectionString, productCount, customerCount, ordersPerCustomer, linesPerOrder);
                }

                Assert.Equal(productCount, context.Products.Count());
                Assert.Equal(customerCount, context.Customers.Count());
                Assert.Equal(customerCount * ordersPerCustomer, context.Orders.Count());
                Assert.Equal(customerCount * ordersPerCustomer * linesPerOrder, context.OrderLines.Count());
            }
        }

        public void InsertSeedData(
            string connectionString, 
            int productCount, 
            int customerCount, 
            int ordersPerCustomer, 
            int linesPerOrder)
        {
            List<Product> products = CreateProducts(productCount);
            using (var context = new OrdersContext(connectionString))
            {
                context.Products.Add(products.ToArray());
                context.SaveChanges();
            }

            List<Customer> customers = CreateCustomers(customerCount);
            using (var context = new OrdersContext(connectionString))
            {
                context.Customers.Add(customers.ToArray());
                context.SaveChanges();
            }

            List<Order> orders = CreateOrders(ordersPerCustomer, customers);
            using (var context = new OrdersContext(connectionString))
            {
                context.Orders.Add(orders.ToArray());
                context.SaveChanges();
            }

            List<OrderLine> lines = CreateOrderLines(linesPerOrder, products, orders);

            using (var context = new OrdersContext(connectionString))
            {
                context.OrderLines.Add(lines.ToArray());
                context.SaveChanges();
            }
        }
    }
}
