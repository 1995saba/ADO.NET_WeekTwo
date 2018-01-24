using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager
                  .ConnectionStrings["MyConnectionString"]
                  .ToString();
            DataSet dataSet = new DataSet();

            string firstSelectCommand = @"Select LastName, FirstName, BirthDate From Employees";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(firstSelectCommand, connectionString))
                {
                    adapter.Fill(dataSet, "Employees");
                }
                sqlConnection.Close();
            }

            int minAge, maxAge;
            Console.Write("Enter min age ");
            minAge = Int32.Parse(Console.ReadLine());
            Console.Write("Enter max age ");
            maxAge = Int32.Parse(Console.ReadLine());
            Console.WriteLine();

            foreach (DataRow item in dataSet.Tables["Employees"].Rows)
            {
                int age = DateTime.Now.Year - DateTime.Parse(item.ItemArray[2].ToString()).Year;
                if (age >= minAge && age <= maxAge)
                    Console.WriteLine("{0} {1} - {2}", item.ItemArray[0], item.ItemArray[1], age);

            }
            Console.WriteLine();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string secondSelectCommand = @"Select Country, CONCAT(LastName,' ', FirstName) as FIO From Employees Order by Country";
                sqlConnection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(secondSelectCommand, connectionString))
                {
                    adapter.Fill(dataSet, "Employees");
                }
                sqlConnection.Close();
            }

            foreach (DataRow item in dataSet.Tables["Employees"].Rows)
            {
                Console.WriteLine("{0} - {1}", item.ItemArray[0], item.ItemArray[1]);

            }
            Console.WriteLine();

            string thirdSelectCommand = @"Select CONCAT(empl.LastName,' ', empl.FirstName) as FIO, CONCAT(boss.LastName,' ', boss.FirstName) as Boss, CONCAT(slave.LastName,' ', slave.FirstName) as Slave
                                                From Employees empl
                                                LEFT Join Employees boss ON empl.ReportsTo = boss.EmployeeID
                                                LEFT Join Employees slave ON empl.EmployeeID = slave.ReportsTo";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                
                sqlConnection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(thirdSelectCommand, connectionString))
                {
                    adapter.Fill(dataSet, "Employees");
                }
                sqlConnection.Close();
            }

            foreach (DataRow item in dataSet.Tables["Employees"].Rows)
            {
                Console.WriteLine("Employee {0} reports to {1} and is boss for {2}", item.ItemArray[0],
                                     item.ItemArray[1].ToString() == " " ? "nobody" : item.ItemArray[1],
                                     item.ItemArray[2].ToString() == " " ? "nobody" : item.ItemArray[2]);
            }
            Console.WriteLine();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string fourthSelectCommand = @"Select ord.OrderID, sup.ContactName From [Order Details] ord
                                           Left Join Products pro On pro.ProductID = ord.ProductID 
                                           Left Join Suppliers sup ON sup.SupplierID = pro.SupplierID
                                           Order by ord.OrderID";
                sqlConnection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(fourthSelectCommand, connectionString))
                {
                    adapter.Fill(dataSet, "Employees");
                }
                sqlConnection.Close();
            }
            int orderId = 0;
            foreach(DataRow item in dataSet.Tables["Employees"].Rows)
            {
               if(orderId != Int32.Parse(item.ItemArray[0].ToString()))
                {
                    Console.WriteLine();
                    orderId = Int32.Parse(item.ItemArray[0].ToString());
                    Console.WriteLine("Order Number {0}:",orderId);
                    Console.WriteLine("  Suplliers:");
                }
                Console.WriteLine(item.ItemArray[1]);
            }
            Console.WriteLine();


            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string fifthSelectCommand = @"Select ProductName, UnitsInStock From Products
                                              Order by UnitsInStock Desc";
                sqlConnection.Open();
                using(SqlDataAdapter adapter = new SqlDataAdapter(fifthSelectCommand, connectionString))
                {
                    adapter.Fill(dataSet, "Products");
                }
                sqlConnection.Close();
            }
            
            foreach(DataRow item in dataSet.Tables["Products"].Rows)
            {
              Console.WriteLine("{0} - {1}", item.ItemArray[0], item.ItemArray[1]);
            }
            Console.ReadLine();
        }
    }
}
