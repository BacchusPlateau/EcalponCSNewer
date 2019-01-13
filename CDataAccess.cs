using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;


	/// <summary>
	/// Wraps up data access with easy to use, intuitive methods
	/// </summary>
	public class CDataAccess
	{
		OleDbConnection conn = null;

		public CDataAccess()
		{
			DBOpenConnection(); 
		}

		/// <summary>
		/// Destroy the connection
		/// </summary>
		~CDataAccess()
		{
			if(conn==null) 
			{
				conn.Close();
			}
		}

		/// <summary>
		/// Fill an empty data reader with results from a SQL query string
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="SQL"></param>
		public void FillDataReader(out OleDbDataReader reader, string SQL) 
		{
			OleDbCommand comm = new OleDbCommand(SQL, conn);
			reader = null;

			try
			{
				reader = comm.ExecuteReader();
			}
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}

		}

		/// <summary>
		/// Execute a non-select statement
		/// </summary>
		/// <param name="SQL"></param>
		public void ExecuteStatement(string SQL) 
		{
			OleDbCommand comm = new OleDbCommand(SQL, conn);

			try
			{
				comm.ExecuteNonQuery();
			}
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}
		}

		/// <summary>
		/// Open a connection to the database
		/// </summary>
		public void DBOpenConnection()
		{
			conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" +
				"Data Source=" + Application.StartupPath + "\\ecalpon.mdb;" +
				"Jet OLEDB:Database Password=alsatian;");

			try 
			{
				conn.Open();
				
			}
			catch(Exception e)
			{
				MessageBox.Show(e.ToString());
			}
			

		}
	}

