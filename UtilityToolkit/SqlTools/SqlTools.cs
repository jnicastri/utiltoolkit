using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using UtilityToolkit.Extensions;

namespace UtilityToolkit.SqlTools
{
    public static class SqlTools
    {
        public const string DefaultIdParamName = "@Id";

        public static SqlConnection GetNewSqlConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["Conn"].ConnectionString);
        }

        #region ExecuteNonQuery

        public static void ExecuteNonQuery(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = GetNewSqlConnection())
            {
                connection.Open();
                ExecuteNonQuery(connection, procedureName, parameters);
                connection.Close();
            }
        }

        public static void ExecuteNonQuery(SqlConnection openConnection, string procedureName, params SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(procedureName, openConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters ?? new SqlParams());
            command.ExecuteNonQuery();
        }

        public static void ExecuteNonQuery(string procedureName, int id)
        {
            ExecuteNonQuery(procedureName, new SqlParams(id));
        }

        public static void ExecuteNonQuery(SqlConnection connection, string procedureName, int id)
        {
            SqlParameter idParameter = new SqlParameter(DefaultIdParamName, SqlDbType.Int) { Value = id };
            ExecuteNonQuery(connection, procedureName, idParameter);
        }

        #endregion

        #region ExecuteScalar

        public static object ExecuteScalar(string procedureName, params SqlParameter[] parameters)
        {
            object scalar;
            using (SqlConnection connection = GetNewSqlConnection())
            {
                connection.Open();
                scalar = ExecuteScalar(connection, procedureName, parameters);
                connection.Close();
            }
            return scalar;
        }

        public static object ExecuteScalar(SqlConnection openConnection, string procedureName, params SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(procedureName, openConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);
            return command.ExecuteScalar();
        }

        public static object ExecuteScalar(string procedureName, int id)
        {
            return ExecuteScalar(procedureName, new SqlParams(id));
        }

        public static object ExecuteScalar(SqlConnection openConnection, string procedureName, int id)
        {
            return ExecuteScalar(openConnection, procedureName, new SqlParams(id));
        }

        #region Generic ExecuteScalar

        public static T ExecuteScalar<T>(string procedureName, params SqlParameter[] parameters) where T : struct
        {
            T scalar;
            using (SqlConnection connection = GetNewSqlConnection())
            {
                connection.Open();
                scalar = (T)ExecuteScalar(connection, procedureName, parameters);
                connection.Close();
            }
            return scalar;
        }

        public static T ExecuteScalar<T>(SqlConnection openConnection, string procedureName, params SqlParameter[] parameters) where T : struct
        {
            SqlCommand command = new SqlCommand(procedureName, openConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);
            return (T)command.ExecuteScalar();
        }

        public static T ExecuteScalar<T>(string procedureName, int id) where T : struct
        {
            return ExecuteScalar<T>(procedureName, new SqlParams(id));
        }

        public static T ExecuteScalar<T>(SqlConnection openConnection, string procedureName, int id) where T : struct
        {
            return ExecuteScalar<T>(openConnection, procedureName, new SqlParams(id));
        }

        #endregion

        #endregion

        #region ExecuteReader

        public static void ExecuteReader(string procedureName, int id, CommandBehavior behavior, Action<SqlDataReader> readerCallback)
        {
            ExecuteReader(procedureName, new SqlParams(id), behavior, readerCallback);
        }

        public static void ExecuteReader(SqlConnection openConnection, string procedureName, int id, CommandBehavior behavior, Action<SqlDataReader> readerCallback)
        {
            ExecuteReader(openConnection, procedureName, new SqlParams(id), behavior, readerCallback);
        }

        public static void ExecuteReader(string procedureName, int id, Action<SqlDataReader> readerCallback)
        {
            ExecuteReader(procedureName, id, CommandBehavior.Default, readerCallback);
        }

        public static void ExecuteReader(SqlConnection openConnection, string procedureName, int id, Action<SqlDataReader> readerCallback)
        {
            ExecuteReader(openConnection, procedureName, id, CommandBehavior.Default, readerCallback);
        }

        public static void ExecuteReader(string procedureName, SqlParams parameters, Action<SqlDataReader> readerCallback)
        {
            ExecuteReader(procedureName, parameters, CommandBehavior.Default, readerCallback);
        }

        public static void ExecuteReader(SqlConnection openConnection, string procedureName, SqlParams parameters, Action<SqlDataReader> readerCallback)
        {
            ExecuteReader(openConnection, procedureName, parameters, CommandBehavior.Default, readerCallback);
        }

        public static void ExecuteReader(string procedureName, SqlParams parameters, CommandBehavior behavior, Action<SqlDataReader> readerCallback)
        {
            using (SqlConnection connection = GetNewSqlConnection())
            {
                connection.Open();
                ExecuteReader(connection, procedureName, parameters, behavior, readerCallback);
                connection.Close(); //try/finally to close not needed as connection.Dispose closes the connection at end of using block.
            }
        }

        public static void ExecuteReader(SqlConnection openConnection, string procedureName, SqlParams parameters, CommandBehavior behavior, Action<SqlDataReader> readerCallback)
        {
            using (SqlCommand command = new SqlCommand(procedureName, openConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 180;
                command.Parameters.AddRange(parameters ?? new SqlParams());
                SqlDataReader reader = command.ExecuteReader(behavior);
                try
                {
                    readerCallback(reader);
                }
                finally
                {
                    if (reader != null) reader.Close();
                } //Close reader even if exception thrown
            } //dispose the command even if exception thrown
        }

        #endregion

        #region Reading DB Values from Reader

        public static T? ReadNullable<T>(object dbReaderValue) where T : struct
        {
            return (dbReaderValue == DBNull.Value) ? null : (T?)dbReaderValue;
        }

        public static T? ReadNullable<T>(object dbReaderValue, T? valueIfNull) where T : struct
        {
            return (dbReaderValue == DBNull.Value) ? valueIfNull : (T)dbReaderValue;
        }

        public static T Read<T>(object dbReaderValue) where T : struct
        {
            try
            {
                return (T)dbReaderValue;
            }
            catch (InvalidCastException e)
            {
                throw new NotImplementedException("Cannot cast 'dbReaderValue' to the specified type. If the specified type is meant to be nullable, use the ReadNullable<T> method.", e);
            }
        }

        #region Enums

        public static TEnum? ReadNullableEnumFromValue<TEnum>(object dataReaderValue) where TEnum : struct
        {
            return ReadNullableEnumFromValue<TEnum>(dataReaderValue, null);
        }

        /// <summary>
        /// Gets a Nullable Enum representing the data reader value provided.
        /// </summary>
        /// <typeparam name="TEnum">The Type of the Enum to return.</typeparam>
        /// <param name="dataReaderValue">The value of a field obtained using a DataReader object.</param>
        /// <returns>'null' if 'dataReaderValue' is DBNull, otherwise an Enum of type 'T' representing the value. </returns>
        public static TEnum? ReadNullableEnumFromValue<TEnum>(object dataReaderValue, TEnum? nullValue) where TEnum : struct
        {
            return (dataReaderValue == DBNull.Value) ? nullValue : (TEnum?)Enum.ToObject(typeof(TEnum), dataReaderValue);
        }

        public static TEnum? ReadNullableEnumFromLabel<TEnum>(object dataReaderValue) where TEnum : struct
        {
            return Enums.Nullable.FromLabel<TEnum>(dataReaderValue.ToString());
        }

        public static TEnum? ReadNullableEnumFromLabel<TEnum>(object dataReaderValue, TEnum? nullValue) where TEnum : struct
        {
            return Enums.Nullable.FromLabel(dataReaderValue.ToString(), nullValue);
        }

        public static TEnum? ReadNullableEnumFromLabel<TEnum>(object dataReaderValue, bool ignoreCase) where TEnum : struct
        {
            return Enums.Nullable.FromLabel<TEnum>(dataReaderValue.ToString(), ignoreCase);
        }

        public static TEnum? ReadNullableEnumFromLabel<TEnum>(object dataReaderValue, bool ignoreCase, TEnum? nullValue) where TEnum : struct
        {
            return Enums.Nullable.FromLabel(dataReaderValue.ToString(), ignoreCase, nullValue);
        }

        public static TEnum ReadEnumFromValue<TEnum>(object dbReaderValue) where TEnum : struct
        {
            return Read<TEnum>(dbReaderValue);
        }

        public static TEnum ReadEnumFromLabel<TEnum>(object dbReaderValue) where TEnum : struct
        {
            return Enums.FromLabel<TEnum>(dbReaderValue.ToString());
        }

        public static TEnum ReadEnumFromLabel<TEnum>(object dbReaderValue, bool ignoreCase) where TEnum : struct
        {
            return Enums.FromLabel<TEnum>(dbReaderValue.ToString(), ignoreCase);
        }

        #endregion

        #endregion

        /// <summary>
        /// Only used for generating reports with Aspose.Cells. This is not to be used to populate any site content.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="datasetName"></param>
        /// <param name="srcTable"></param>
        /// <returns></returns>
        public static DataSet GetDataset(string procedureName, SqlParams sqlParams, string datasetName, params string[] tableNames)
        {
            DataSet dataset = new DataSet(datasetName);
            using (SqlConnection connection = GetNewSqlConnection())
            {
                SqlCommand command = new SqlCommand(procedureName);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 320;
                command.Connection = connection;
                command.Parameters.AddRange(sqlParams);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(dataset);
                int i = 0;
                foreach (string tableName in tableNames)
                {
                    dataset.Tables[i++].TableName = tableName;
                }
            }
            return dataset;
        }

        public static DataTable GetDatatable(string procedureName, SqlParams sqlParams, string datatableName)
        {
            DataTable datatable = new DataTable(datatableName);
            using (SqlConnection connection = GetNewSqlConnection())
            {
                SqlCommand command = new SqlCommand(procedureName);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 320;
                command.Connection = connection;
                command.Parameters.AddRange(sqlParams);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataAdapter.Fill(datatable);
            }

            return datatable;
        }

        public static void ExecuteInConnection(Action<SqlConnection> sqlConnection)
        {
            using (SqlConnection connection = GetNewSqlConnection())
            {
                connection.Open();
                sqlConnection(connection);
                connection.Close();
            }
        }
    }
}
