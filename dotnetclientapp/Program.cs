using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace SalesForecasting
{
    class Program
    {
        private static string connectionString = "server=localhost;user=root;database=SalesForecasting;port=3306;password=password";

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the year for sales data:");
            int year = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the percentage increment:");
            decimal percentage = decimal.Parse(Console.ReadLine());

            QuerySalesData(year);
            ApplyPercentageIncrement(year, percentage);
            GetIncrementedSales(year, percentage);
        }

        private static void QuerySalesData(int year)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("GetSalesByYear", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_year", year);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("State\t\tSales Year\tTotal Sales\tTotal Returns\tNet Sales");
                    while (reader.Read())
                    {
                        string state = reader["State"].ToString();
                        int salesYear = reader.GetInt32("SalesYear");
                        decimal totalSales = reader.GetDecimal("TotalSales");
                        decimal totalReturns = reader.GetDecimal("TotalReturns");
                        decimal netSales = reader.GetDecimal("NetSales");

                        string statePadding = new string(' ', Math.Max(0, 15 - state.Length));
                        Console.WriteLine($"{state}{statePadding}\t{salesYear}\t\t{totalSales,12:N2}\t{totalReturns,13:N2}\t{netSales,10:N2}");
                    }
                }

            }
        }

        private static void ApplyPercentageIncrement(int year, decimal percentage)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("GetIncrementedSalesByYear", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_year", year);
                cmd.Parameters.AddWithValue("p_increment", percentage);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("State\t\tSales Year\tTotal Sales\tNext Year\tPercentage Increase\tIncremented Sales\tIncrement Amount");
                    while (reader.Read())
                    {
                        string state = reader["State"].ToString();
                        int salesYear = reader.GetInt32("SalesYear");
                        decimal totalSales = reader.GetDecimal("TotalSales");
                        int nextYear = reader.GetInt32("NextYear");
                        decimal percentageIncrease = reader.GetDecimal("PercentageIncrease");
                        decimal incrementedSales = reader.GetDecimal("IncrementedSales");
                        decimal incrementAmount = reader.GetDecimal("IncrementAmount");

                        string statePadding = new string(' ', Math.Max(0, 15 - state.Length));
                        Console.WriteLine($"{state}{statePadding}\t{salesYear}\t\t{totalSales,10:N2}\t{nextYear}\t\t{percentageIncrease,12:N2}\t{incrementedSales,18:N2}\t{incrementAmount,15:N2}");
                    }
                }
            }
        }

        private static void GetIncrementedSales(int year, decimal percentage)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("GetIncrementedSalesByYearCSV", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_year", year);
                cmd.Parameters.AddWithValue("p_increment", percentage);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    using (StreamWriter writer = new StreamWriter("forecasted_data.csv"))
                    {
                        writer.WriteLine("State,Percentage Increase,Incremented Sales");
                        while (reader.Read())
                        {
                            string state = reader["State"].ToString();
                            decimal percentageIncrease = reader.GetDecimal("PercentageIncrease");
                            decimal incrementedSales = reader.GetDecimal("IncrementedSales");
                            writer.WriteLine ($"{state},{percentageIncrease},{incrementedSales}");                            
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Forecasted data has been written to forecasted_data.csv");
                }
            }
        }
    }
}
