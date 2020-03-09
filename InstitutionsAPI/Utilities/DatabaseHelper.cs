using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InstitutionsAPI.Utilities
{
    public class DatabaseHelper
    {
        public static void CreateDatabase(string dbName)
        {
            try
            {
                using (var connection = new SqlConnection(@"Data Source =.\SQLEXPRESS; User ID = sa; Password = P@ssw0rd"))
                {
                    var query = $@"CREATE DATABASE {dbName}  
                                ON   
                                ( NAME = {dbName}_dat,  
                                    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\{dbName}dat.mdf',  
                                    SIZE = 10,  
                                    MAXSIZE = 50,  
                                    FILEGROWTH = 5 )  
                                LOG ON  
                                ( NAME = Sales_log,  
                                    FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\{dbName}log.ldf',  
                                    SIZE = 5MB,  
                                    MAXSIZE = 25MB,  
                                    FILEGROWTH = 5MB )";

                    Console.WriteLine("Executing: {0}", query);

                    var command = new SqlCommand(query, connection);

                    connection.Open();
                    Console.WriteLine("SQL Connection successful.");

                    command.ExecuteNonQuery();
                    Console.WriteLine("SQL Query execution successful.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failure: {0}", ex.Message);
                throw ex;
            }
        }

        public static void ExecuteQuery(string dbName)
        {
            
        }


    }
}
