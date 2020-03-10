using System;
using System.Collections.Generic;
using System.Data;
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

        public static void CreateTable(string connectionString, string tableName)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var query = $@"CREATE TABLE [{tableName}] (
	                                ID int PRIMARY KEY IDENTITY,
	                                SerializedEntity varchar(100) NOT NULL
                                )";

                    Console.WriteLine("Executing: {0}", query);

                    var command = new SqlCommand(query, connection);

                    connection.Open();

                    command.ExecuteNonQuery();     
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failure: {0}", ex.Message);
                throw ex;
            }
        }

        public static List<IDictionary<string, object>> ExecuteSelectQuery(string connectionString, string query)
        {

            var matchingObjects = new List<IDictionary<string, object>>();

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {           
                SqlCommand cmd = new SqlCommand(query, myConnection);

                myConnection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var columnNames = new string[reader.FieldCount];

                    foreach (var columnId in Enumerable.Range(0, reader.FieldCount))
                    {
                        columnNames[columnId] = reader.GetName(columnId);
                    }

                    while (reader.Read())
                    {
                        IDictionary<string, object> entity = new Dictionary<string, object>();

                        foreach (var columnId in Enumerable.Range(0, reader.FieldCount))
                        {
                            var columnName = columnNames[columnId];

                            Type type = reader[columnName].GetType();
                            if(type == typeof(string))
                            {
                                entity.Add(columnName, reader[columnName].ToString());                               
                            }
                            else if(type == typeof(int))
                            {
                                entity.Add(columnName, Convert.ToInt32(reader[columnName]));
                            }
                            else if (type == typeof(Boolean))
                            {
                                entity.Add(columnName, Convert.ToBoolean(reader[columnName]));
                            }
                            else
                            {
                                entity.Add(columnName, Convert.ToString(reader[columnName]));
                                
                            }

                        }                       

                        matchingObjects.Add(entity);
                    }

                    myConnection.Close();
                }
            }

            return matchingObjects;
        }

        public static IDictionary<string, object> ExecuteSelectFindQuery(string connectionString, string query)
        {
            IDictionary<string, object> entity = new Dictionary<string, object>();

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, myConnection);

                myConnection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var columnNames = new string[reader.FieldCount];

                    foreach (var columnId in Enumerable.Range(0, reader.FieldCount))
                    {
                        columnNames[columnId] = reader.GetName(columnId);
                    }

                    while (reader.Read())
                    {
                        foreach (var columnId in Enumerable.Range(0, reader.FieldCount))
                        {
                            var columnName = columnNames[columnId];

                            Type type = reader[columnName].GetType();

                            if (type == typeof(string))
                            {
                                entity.Add(columnName, reader[columnName].ToString());
                            }
                            else if (type == typeof(int))
                            {
                                entity.Add(columnName, Convert.ToInt32(reader[columnName]));
                            }
                            else if (type == typeof(Boolean))
                            {
                                entity.Add(columnName, Convert.ToBoolean(reader[columnName]));
                            }
                            else
                            {
                                entity.Add(columnName, Convert.ToString(reader[columnName]));

                            }

                        }
                    }

                    myConnection.Close();
                }
            }

            return entity;
        }

        public static void ExecuteQuery(string connectionString, string query)
        {
            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, myConnection);

                myConnection.Open();

                cmd.ExecuteNonQuery();

                myConnection.Close();
                
            }
        }

        public static int ExecuteInsertQuery(string connectionString, string query)
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

        public static bool ExecuteTableCheck(string connectionString, string tableName)
        {

            using (SqlConnection myConnection = new SqlConnection(connectionString))
            {
                SqlCommand checkTableCmd = new SqlCommand(@"IF EXISTS(
                                    SELECT 1 FROM INFORMATION_SCHEMA.TABLES 
                                    WHERE TABLE_NAME = @table) 
                                    SELECT 1 ELSE SELECT 0", myConnection);

                checkTableCmd.Parameters.Add("@table", SqlDbType.NVarChar).Value = tableName;
                myConnection.Open();
                int exists = (int)checkTableCmd.ExecuteScalar();
                return exists == 1;
            }
        }
    }
}
