using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Data.Common;

namespace AICore.Utils
{
    public class DapperDataHelper
    {
        public DapperDataHelper()
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection() { ConnectionString = "" };
        }
        private IDbConnection conn { get; set; }

        public IList<T> Fill<T>(string sql) where T : class
        {
             return Dapper.SqlMapper.Query<T>(conn, sql).ToList();
        }
    }
}
