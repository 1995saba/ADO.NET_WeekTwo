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
    class Program
    {
        public void GetEmployeesStats(DateTime startDate, DateTime finishDate)
        {
            string connectionString = ConfigurationManager
                  .ConnectionStrings["MyConnectionString"]
                  .ToString();

            DataSet dataSet = new DataSet();
            string startDateAsString = $"{startDate.Year}-{startDate.Day}-{startDate.Month}";
            string finishDateAsString = $"{finishDate.Year}-{finishDate.Day}-{finishDate.Month}";

            string query = "SELECT Orders.OrderID, Orders.EmployeeID, Employees.FirstName, Employees.LastName, [Order Details].UnitPrice " +
                "FROM Orders" +
                "LEFT JOIN[Order Details] ON" +
                "Orders.OrderID =[Order Details].OrderID" +
                "LEFT JOIN Employees ON" +
                "Orders.EmployeeID = Employees.EmployeeID" +
                $"WHERE Orders.OrderDate BETWEEN '{startDateAsString}'  AND  '{finishDateAsString}'";

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString))
                {
                    adapter.Fill(dataSet, "Stats");
                }
                sqlConnection.Close();
            }

            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            
        }
    }
}
