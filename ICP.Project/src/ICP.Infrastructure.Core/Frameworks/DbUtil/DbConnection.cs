using ICP.Infrastructure.Abstractions.DbUtil;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Frameworks.DbUtil
{
    public abstract class DbConnection : Abstractions.DbUtil.IDbConnection
    {
        public int Timeout { get; set; } = 60;

        protected readonly Lazy<System.Data.IDbConnection> SqlConnection = null;

        public DbConnection() { }

        public DbConnection(string name)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[name]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(string.Format("請確認是否有加入 {0} 連接字串", name));
            }

            SqlConnection = new Lazy<System.Data.IDbConnection>(() =>
            {
                var sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                return sqlConnection;
            });
        }

        public virtual IDbTransaction BeginTransaction()
        {
            return SqlConnection.Value.BeginTransaction();
        }

        public virtual string GenerateParameter(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            var list = obj.GetType()
                          .GetProperties()
                          .Select(x => string.Format("@{0} = @{0}", x.Name));

            return " " + string.Join(",", list);
        }

        public virtual void Dispose()
        {
            if (SqlConnection != null && SqlConnection.IsValueCreated)
            {
                SqlConnection.Value.Close();
                SqlConnection.Value.Dispose();
            }
        }

        public abstract int Execute(string sql, object obj);
        public abstract List<T> Query<T>(string sql, object obj);
        public abstract T QuerySingleOrDefault<T>(string sql, object obj);
        public abstract List<IEnumerable<object>> QueryMultiple(Type[] types, string sql, object obj);
    }
}
