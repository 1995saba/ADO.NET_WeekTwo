using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NETWeekTwo
{
    // по id заказа узнать сотрудника и его другие заказы
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager
                  .ConnectionStrings["MyConnectionString"]
                  .ToString();
            DataSet dataSet = new DataSet();


            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string ordersSelectCommand = "SELECT * FROM Orders";
                sqlConnection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(ordersSelectCommand, connectionString))
                {
                    adapter.Fill(dataSet, "Orders");
                }
                sqlConnection.Close();
            }

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string employeesSelectCommand = "SELECT * FROM Employees";
                sqlConnection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(employeesSelectCommand, connectionString))
                {
                    adapter.Fill(dataSet, "Employees");
                }
                sqlConnection.Close();
            }

            Console.WriteLine("Enter OrderId:");
            string orderId = Console.ReadLine();
            string employeeId = "";
            foreach (var item in dataSet.Tables["Orders"].Rows)
            {
                if ((item as DataRow)["OrderID"].ToString() == orderId)
                {
                    employeeId = (item as DataRow)["EmployeeID"].ToString();
                }
            }

            foreach (DataRow item in dataSet.Tables["Employees"].Rows)
            {
                if (item["EmployeeID"].ToString() == employeeId)
                {
                    Console.WriteLine("Employee Name: "+item["FirstName"] +' '+item["LastName"]);
                }
            }
            Console.WriteLine();
            Console.WriteLine("Orders: ");
            foreach (DataRow item in dataSet.Tables["Orders"].Rows)
            {
                if (item["EmployeeID"].ToString() == employeeId)
                {
                    Console.WriteLine();
                    Console.WriteLine("OrderID: "+item["OrderID"]);
                    Console.WriteLine("OrderDate: "+item["OrderDate"]);
                    Console.WriteLine("ShipCountry: "+item["ShipCountry"]);
                    Console.WriteLine("ShipCity: "+item["ShipCity"]);
                    Console.WriteLine();
                }
            }

            Console.ReadLine();
        }
    }
}