using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace ConsoleApp2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var connstring = ConfigurationManager.ConnectionStrings["SqlServerConnString"].ConnectionString;

            List<Customer> ret;
            using (var db = new SqlConnection(connstring))
            {
                var sql =
                    "select C.[Id], C.[Name], C.[Email], C.[Phone], A.CustomerId, A.Street, A.City, A.Country, A.ZIPCode from customer C " +
                    "inner join[Address] A on A.CustomerId = C.Id";

                ret = db.Query<Customer, Address, Customer>(sql, (customer, address) =>
                {
                    customer.Address = address;
                    return customer;
                }, splitOn: "CustomerId").ToList();
            }

            foreach (var customer in ret)
            {
                Console.WriteLine(customer.Name);
                Console.WriteLine(customer.Address.City);
            }
            Console.ReadLine();
        }
    }
}