using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace UtilityToolkit.SqlTools
{
    public class SqlParams
    {
        public readonly static SqlParams None = new SqlParams();

        Dictionary<string, SqlParameter> sqlParams;

        public SqlParameter this[string parameterName]
        {
            get
            {
                return sqlParams[parameterName];
            }
        }

        #region Constructors

        public SqlParams()
        {
            sqlParams = new Dictionary<string, SqlParameter>();
        }

        public SqlParams(int id)
            : this()
        {
            this.Add(SqlTools.DefaultIdParamName, SqlDbType.Int).Value = id;
        }

        public SqlParams(string name, SqlDbType dbType, object value)
            : this()
        {
            this.Add(name, dbType).Value = value;
        }

        public SqlParams(Dictionary<string, SqlParameter> sqlParams)
        {
            this.sqlParams = sqlParams;
        }

        public SqlParams(IEnumerable<SqlParameter> parameters)
        {
            this.sqlParams = parameters.ToDictionary(parameter => parameter.ParameterName, parameter => parameter);
        }

        public SqlParams(params SqlParameter[] parameters) : this((IEnumerable<SqlParameter>)parameters) { }

        #endregion

        public SqlParameter Add(object value)
        {
            SqlParameter parameter = new SqlParameter { Value = value };
            sqlParams.Add(parameter.ParameterName, parameter);
            return parameter;
        }

        public SqlParameter Add(string parameterName, object value)
        {
            SqlParameter parameter = new SqlParameter(parameterName, value);
            sqlParams.Add(parameter.ParameterName, parameter);
            return parameter;
        }

        public SqlParameter Add(string parameterName, SqlDbType dbType)
        {
            SqlParameter parameter = new SqlParameter(parameterName, dbType);
            sqlParams.Add(parameter.ParameterName, parameter);
            return parameter;
        }

        public SqlParameter Add(string parameterName, SqlDbType dbType, int size)
        {
            SqlParameter parameter = new SqlParameter(parameterName, dbType, size);
            sqlParams.Add(parameter.ParameterName, parameter);
            return parameter;
        }

        public static implicit operator SqlParameter[](SqlParams sqlParams)
        {
            return sqlParams.ToArray();
        }

        public static implicit operator SqlParams(SqlParameter[] sqlParamsArray)
        {
            return new SqlParams(sqlParamsArray);
        }

        public SqlParameter[] ToArray()
        {
            return sqlParams.Values.ToArray();
        }

        public static SqlParams WriteId(int id)
        {
            return new SqlParams(id);
        }
    }
}
