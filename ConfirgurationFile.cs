using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidProject_DB
{
    internal class ConfirgurationFile
    {
        String ConnectionStr = @"Data Source=(local);Initial Catalog=ProjectB;Integrated Security=True";
        SqlConnection con;
        private static ConfirgurationFile _instance;
        public static ConfirgurationFile getInstance()
        {
            if (_instance == null)
                _instance = new ConfirgurationFile();
            return _instance;
        }
        private ConfirgurationFile()
        {
            con = new SqlConnection(ConnectionStr);
         
        }
        public SqlConnection getConnection()
        {
            return con;
        }
    }
}
