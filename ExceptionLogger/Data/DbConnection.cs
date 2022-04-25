using ExceptionLogger.Handler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogger.Data
{
    public class DbConnection
    {
        private const string queryString = @"
            IF OBJECT_ID('ApplicationException') IS NULL
            BEGIN
            CREATE TABLE ApplicationException( 
	            Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	            ExceptionJson VARCHAR(5000) NOT NULL,
	            CreatedAt DATETIME NOT NULL
            );
            END
            
        INSERT INTO ApplicationException ([ExceptionJson], CreatedAt) VALUES('{0}', GETDATE())";

        private SqlConnection _connection;
        public DbConnection(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }
        public async Task Execute(Exception exception)
        {
            try
            {
                var command = string.Format(queryString, JsonConvert.SerializeObject(exception));

                using (var cmd = new SqlCommand(command, _connection))
                {
                    _connection.Open();
                    var reader = await cmd.ExecuteScalarAsync();
                    _connection.Close();
                }
            }
            catch (SqlException ex)
            {
                ex.Write(@"C:\SqlException");
            }
            catch (Exception ex)
            {
                ex.Write(@"C:\Exception");
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}

