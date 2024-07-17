# salesforecast

## Technologies Used
- .NET Core C#
- MySQL

## Project Overview
This project is a Sales forecasting application using dotnet core with MySQL database to perform various queries and calculations related to sales data.

## Setup Instructions
1. **Database Setup:**
   - Execute the `create_database_query.txt` script from DatabaseScripts folder to create the MySQL database.
   - Execute the `create_tables_query.txt` to create the necessary tables such as orders, products, ordersreturns.
   - Save excel sheets to csv files in `C:\ProgramData\MySQL\MySQL Server 8.0\Uploads` path.
   - Execute the `load_csv_data_to_tables_query.txt` to load data from csv files to corresponding tables.
   - Execute the stored procedure scripts (`sp_getsalesbyyear_query.txt`, `sp_getincrementedsalesbyyear_query.txt`, `sp_getincrementedsalesbyyearCSV_query.txt`) to create the required stored procedures.

2. **Sales Forecasting Application Setup:**
   - Clone this repository.
   - Open the solution in Visual Studio to build and run the application.

3. **Running the Application:**
   - Follow the prompts in the console application to query sales data for a specific year, apply percentage increment.

4. **Notes:**
   - Ensure MySQL server connection string is correct as in code.

## Additional Notes
- Adjusted some values in excel sheet to match the datatype in mysql tables
