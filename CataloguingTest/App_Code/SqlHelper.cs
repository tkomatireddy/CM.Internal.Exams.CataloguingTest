//----------------------------------------------------------------------
// <copyright file="SqlHelper.cs" company="EBsco">
//     Copyright (c) EBsco. All rights reserved.
// </copyright>
//----------------------------------------------------------------------

namespace CataloguingTest
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This Class contains SQL queries
    /// </summary>
    public class SqlHelper
    {
        // Internal members
        public string _connString = null;
        public SqlConnection _conn = null;
        public bool _disposed = false;

        /// <summary>
        /// Constructor using global connection string.
        /// </summary>
        public SqlHelper()
        {
            this._connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            _conn = new SqlConnection(_connString);
        }
        /// <summary>
        /// Constructure using connection string override
        /// </summary>
        /// <param name="connString">Connection string for this instance</param>
        public SqlHelper(string connString)
        {
            this._connString = connString;
            this.Connect();
        }



        /// <summary>
        /// Sets or returns the connection string use by all instances of this class.
        /// </summary>
        public static string ConnectionString { get; set; }





        // Creates a SqlConnection using the current connection string
        public void Connect()
        {
            //_connString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            //_conn = new SqlConnection(_connString);
            if (_conn != null && _conn.State == ConnectionState.Closed)
                _conn.Open();
        }

        public void Close()
        {
            if (_conn != null && _conn.State == ConnectionState.Open)
                _conn.Close();
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand)
        {
            int result = 0;
            sqlCommand.Connection = _conn;
            if (_conn.State != ConnectionState.Open)
                Connect();
            sqlCommand.Connection = _conn;
            using (SqlCommand cmd = sqlCommand)
            {
                result = cmd.ExecuteNonQuery();
            }

            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            return result;
        }

        public int ExecuteNonQuery(string query)
        {
            int result = 0;
            if (_conn.State != ConnectionState.Open)
                Connect();
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                result = cmd.ExecuteNonQuery();
            }

            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            return result;
        }

        public int ExecuteNonQueryProc(string procedureName)
        {
            int result = 0;
            if (_conn.State != ConnectionState.Open)
                Connect();

            using (SqlCommand cmd = new SqlCommand(procedureName, _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                result = cmd.ExecuteNonQuery();
            }

            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            return result;
        }

        public object ExecuteScalar(SqlCommand sqlCommand)
        {
            object result = null;
            sqlCommand.Connection = _conn;
            if (_conn.State != ConnectionState.Open)
                Connect();
            sqlCommand.Connection = _conn;
            using (SqlCommand cmd = sqlCommand)
            {
                result = cmd.ExecuteScalar();
            }

            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            return result;
        }

        public object ExecuteScalar(string query)
        {
            object result = null;
            if (_conn.State != ConnectionState.Open)
                Connect();
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                result = cmd.ExecuteScalar();
            }

            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            return result;
        }

        public object ExecuteScalarProc(string procedureName)
        {
            object result = null;
            if (_conn.State != ConnectionState.Open)
                Connect();
            using (SqlCommand cmd = new SqlCommand(procedureName, _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                result = cmd.ExecuteScalar();
            }

            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            return result;
        }

        public SqlDataReader ExecuteDataReader(SqlCommand sqlCommand)
        {
            SqlDataReader result = null;
            sqlCommand.Connection = _conn;
            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            if (_conn.State != ConnectionState.Open)
                Connect();
            sqlCommand.Connection = _conn;
            using (SqlCommand cmd = sqlCommand)
            {
                result = cmd.ExecuteReader();
            }
            ////if (_conn.State == ConnectionState.Open)
            ////    _conn.Close();
            return result;
        }

        public SqlDataReader ExecuteDataReader(string query)
        {
            SqlDataReader result = null;
            if (_conn.State != ConnectionState.Open)
                Connect();
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                result = cmd.ExecuteReader();
            }
            ////if (_conn.State == ConnectionState.Open)
            ////    _conn.Close();
            return result;
        }

        public SqlDataReader ExecuteDataReaderProc(string procedureName)
        {
            SqlDataReader result = null;
            if (_conn.State != ConnectionState.Open)
                Connect();
            using (SqlCommand cmd = new SqlCommand(procedureName, _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                result = cmd.ExecuteReader();
            }

            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            return result;
        }

        public DataSet ExecuteDataSet(SqlCommand sqlCommand)
        {
            DataSet result = null;
            sqlCommand.Connection = _conn;
            if (_conn.State != ConnectionState.Open)
                Connect();
            sqlCommand.Connection = _conn;
            using (SqlCommand cmd = sqlCommand)
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                result = ds;
            }

            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            return result;
        }

        public DataSet ExecuteDataSet(string query)
        {
            DataSet result = null;
            if (_conn.State != ConnectionState.Open)
                Connect();
            using (SqlCommand cmd = new SqlCommand(query, _conn))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                result = ds;
            }

            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            return result;
        }

        public DataSet ExecDataSetProc(string procedureName)
        {
            DataSet result = null;
            if (_conn.State != ConnectionState.Open)
                Connect();
            using (SqlCommand cmd = new SqlCommand(procedureName, _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                result = ds;
            }

            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            return result;
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Need to dispose managed resources if being called manually
                if (disposing)
                {
                    if (_conn != null)
                    {
                        this._conn.Dispose();
                        this._conn = null;
                    }
                }

                _disposed = true;
            }
        }

        #endregion


    }
}
