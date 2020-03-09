using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
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

        public static List<ExpandoObject> ExecuteQuery(string connectionString, string query)
        {

            var matchingObjects = new List<ExpandoObject>();

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {           
                SqlCommand cmd = new SqlCommand(query, myConnection);

                myConnection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dynamic entity = new ExpandoObject(); 
                        entity.Name = reader["Name"].ToString();
                        entity.ID = Convert.ToInt32(reader["ID"]);

                        matchingObjects.Add(entity);
                    }

                    myConnection.Close();
                }
            }

            return matchingObjects;
        }

        public static ExpandoObject ExecuteFindQuery(string connectionString, string query)
        {
            dynamic entity = new ExpandoObject();

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, myConnection);

                myConnection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        entity.Name = reader["Name"].ToString();
                        entity.ID = Convert.ToInt32(reader["ID"]);
                    }

                    myConnection.Close();
                }
            }

            return entity;
        }

        public static void ExecutePureQuery(string connectionString, string query)
        {
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, myConnection);

                myConnection.Open();

                cmd.ExecuteNonQuery();

                myConnection.Close();
                
            }
        }

        public static int ExecuteCreate(string connectionString, string query)
        {
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, myConnection);

                myConnection.Open();

                int id = (int)cmd.ExecuteScalar();

                myConnection.Close();

                return id;
            }
        }
    }
}
