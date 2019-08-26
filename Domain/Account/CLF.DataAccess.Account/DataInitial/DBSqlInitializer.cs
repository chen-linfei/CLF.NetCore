using CLF.Common.Configuration;
using CLF.Common.Infrastructure;
using CLF.Common.SecurityHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CLF.DataAccess.Account.DataInitial
{
    public  static  class DBSqlInitializer
    {
        public static List<string> GetInitialDataSqlScript()
        {
            var key = "CLF.DataAccess.Account.DataInitial.InitData.sql";
            var sqls = EmbeddedFileHelper.GetEmbeddedFileContents(key);
            return sqls;
        }

        public  static void Sql()
        {
            var config = EngineContext.Current.Resolve<AppConfig>();
            if (config != null)
            {
                var sqlBuilder = new StringBuilder();
                using (var connection = new SqlConnection(config.SqlServerConnectionString))
                {
                    connection.Open();
                    try
                    {
                        var sqlStatements = GetInitialDataSqlScript();
                        foreach (var line in sqlStatements)
                        {
                            string sqlLine = line;
                            if (line.Trim().ToUpper() == "GO")
                            {
                                string sql = sqlBuilder.ToString();
                                if (!string.IsNullOrWhiteSpace(sql))
                                {
                                    using (var sqlTransaction = connection.BeginTransaction())
                                    {
                                        try
                                        {
                                            using (var command = new SqlCommand(sql, connection))
                                            {
                                                try
                                                {
                                                    command.Transaction = sqlTransaction;
                                                    command.ExecuteNonQuery();
                                                    sqlTransaction.Commit();
                                                }
                                                finally
                                                {
                                                    command.Parameters.Clear();
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            break;
                                        }
                                    }
                                }
                                sqlBuilder.Clear();
                            }
                            else
                            {
                                sqlBuilder.AppendLine(sqlLine);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
