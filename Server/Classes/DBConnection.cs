using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Server.Classes
{
	public class DBConnection
	{
		private static string conf = "server=localhost;port=3306;database=license;uid=root;pwd=root;";
		public static MySqlConnection OpenConnection()
		{
			MySqlConnection connection = new MySqlConnection(conf);
			connection.Open();
			return connection;
		}
		public static MySqlDataReader Query(string Query, MySqlConnection connection)
		{
			MySqlCommand command = new MySqlCommand(Query, connection);
			return command.ExecuteReader();
		}
		public static void CloseConnection(MySqlConnection connection)
		{
			connection.Close();
			MySqlConnection.ClearPool(connection);
		}
	}
}
