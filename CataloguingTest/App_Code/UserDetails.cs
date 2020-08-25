using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;
using CataloguingTest;


namespace CataloguingTest
{
    public class UserDetails
    {
        public UserDetails()
        {
            if (this.SqlHelper == null)
            {
                this.SqlHelper = new SqlHelper();
            }
        }
        /// <summary>
        /// Gets or sets the SQL Helper class
        /// </summary>
        public SqlHelper SqlHelper { get; set; }

        public DataTable GetCurrentUserDetails(string userName, string passWord,out int execStatus, out string result)
        {
            execStatus = 0;
            result = "";
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("GetCurrentUserDetails_Proc")
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 3600
            };

            cmd.Parameters.AddWithValue("@userName", userName);
            cmd.Parameters.AddWithValue("@passWord", passWord);

            SqlParameter pExecStatus = new SqlParameter();
            pExecStatus.ParameterName = "@execStatus";
            pExecStatus.Direction = ParameterDirection.Output;
           // pExecStatus.Value = execStatus;
            pExecStatus.SqlDbType = SqlDbType.Int;
            cmd.Parameters.Add(pExecStatus);

            SqlParameter pResult = new SqlParameter();
            pResult.ParameterName = "@result";
            pResult.Direction = ParameterDirection.Output;
            //pResult.Value = result;
            pResult.SqlDbType = SqlDbType.VarChar;
            pResult.Size = 200;
            cmd.Parameters.Add(pResult);
            DataSet ds = this.SqlHelper.ExecuteDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }

            execStatus = Convert.ToInt32(pExecStatus.Value);
            result = pResult.Value.ToString();
            

            //dt.Load(dr);
            //if (!dr.IsClosed)
            //{
            //    dr.Close();
            //}

            this.SqlHelper.Close();
            return dt;
        }

        internal DataTable VerifyExaminerByExaminer(string examinerName, string examinerPassword)
        {
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Nes_SqlConnectionString"].ConnectionString);
            try
            {
                if (cn != null && cn.State != ConnectionState.Open)
                    cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("ex_VerifyExaminerByExaminerName_Proc", cn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 3600
                };

                cmd.Parameters.AddWithValue("@ExaminerName", examinerName);
                cmd.Parameters.AddWithValue("@ExaminerPassword", examinerPassword);
                SqlDataReader dr = cmd.ExecuteReader();

                dt.Load(dr);
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                if (cn != null && cn.State == ConnectionState.Open)
                    cn.Close();
            }
            catch (Exception ex)
            {
                if (cn != null && cn.State != ConnectionState.Closed)
                    cn.Close();
            }
            finally
            {
                if (cn != null && cn.State != ConnectionState.Closed)
                    cn.Close();
            }

            return dt;
        }
        internal DataTable GetUserRole(string userName)
        {
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["Nes_SqlConnectionString"].ConnectionString);
            try
            {
                if (cn != null && cn.State != ConnectionState.Open)
                    cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.AddWithValue("@UserEmail", userName);

                string query = "select top 1 r.RoleId, r.RoleName From examiners e inner join Roles r on e.roleid = r.roleid where e.examinerName =@UserEmail";
                cmd.CommandText = query;
                cmd.Connection = cn;
                SqlDataReader dr = cmd.ExecuteReader();

                dt.Load(dr);
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                if (cn != null && cn.State == ConnectionState.Open)
                    cn.Close();
            }
            catch (Exception ex)
            {
                if (cn != null && cn.State != ConnectionState.Closed)
                    cn.Close();
            }
            finally
            {
                if (cn != null && cn.State != ConnectionState.Closed)
                    cn.Close();
            }

            return dt;
        }

        internal long InsertCat_UserDetails(string userEmail, string examinerName, int roleId)
        {
            long UserId = 0;           

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Cat_InsertUserDetails_Proc")
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 3600
                };

                cmd.Parameters.AddWithValue("@UserName", userEmail);
                cmd.Parameters.AddWithValue("@TakenBy", examinerName);
                cmd.Parameters.AddWithValue("@RoleId", roleId);

                SqlParameter pUserId = new SqlParameter();
                pUserId.ParameterName = "@UserId";
                pUserId.Direction = ParameterDirection.InputOutput;
                pUserId.Value = UserId;
                pUserId.SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.Add(pUserId);
                int res = this.SqlHelper.ExecuteNonQuery(cmd);

                long.TryParse(pUserId.Value.ToString(), out UserId);
                this.SqlHelper.Close();
            }
            catch (Exception ex)
            {
                this.SqlHelper.Close();
            }
            finally
            {
                this.SqlHelper.Close();
            }

            return UserId;
        }
    }
}