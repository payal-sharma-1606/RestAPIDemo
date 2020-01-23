using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InformaticaAPIs.Helpers
{
    public class SqlDbHelper
    {
        static SqlDbHelper()
        {
        }
        public static void ExecuteStoreProcedure(string spName, Dictionary<string, object> _parameters, DataSet _ds)
        {
            string _connectionString = ConfigurationManager.ConnectionStrings["DLW2"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;
                connection.Open();
                sqlCommand.CommandText = spName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                try
                {
                    formatSqlParameters(sqlCommand, _parameters);
                    using (SqlDataAdapter _adapter = new SqlDataAdapter())
                    {
                        _adapter.SelectCommand = sqlCommand;
                        _adapter.Fill(_ds);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void ExecuteStoreProcedure(string spName)
        {

        }

        static void formatSqlParameters(SqlCommand _command, Dictionary<string, object> _parameters)
        {
            foreach (var _obj in _parameters)
            {
                string _variable = "@" + _obj.Key;
                var _value = _obj.Value;
                _command.Parameters.Add(new SqlParameter(_variable, _value));
            }
        }
    }
}