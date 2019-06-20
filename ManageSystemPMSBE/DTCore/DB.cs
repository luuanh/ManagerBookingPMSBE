using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;

namespace ManageSystemPMSBE.DTCore
{
    public class DB
    {
        //private static readonly string Connection = WebConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        private static readonly string ConnectionBEPMS = WebConfigurationManager.ConnectionStrings["connectionStringBEPMS"].ConnectionString;
        //public static Func<DbConnection> ConnectionFactory = () => new SqlConnection(Connection);
        public static Func<DbConnection> ConnectionFactoryBEPMS = () => new SqlConnection(ConnectionBEPMS);
    }
}