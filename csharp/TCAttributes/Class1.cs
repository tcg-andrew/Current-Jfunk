using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCAttributes
{
    public static class Interface
    {
        public static void CreateInput(string server, string database, string username, string password, string input)
        {
            SqlCommand command = new SqlCommand("exec dbo.sp_TC_CreateInput @Input");
            command.Parameters.AddWithValue("Input", input);
            SQLAccess.SQLAccess.NonQuery(server, database, username, password, command);
        }
    }
}
