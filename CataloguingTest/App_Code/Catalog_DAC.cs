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
    public class Catalog_DAC
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog_DAC" /> class.
        /// </summary>
        public Catalog_DAC()
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

        /// <summary>
        /// Convert List Collection to Data Table
        /// </summary>
        /// <typeparam name="T">List Collection</typeparam>
        /// <param name="data"> List data</param>
        /// <returns>Data table</returns>
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public DataTable getIndex()
        {

            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            //cmd.Parameters.AddWithValue("@UserID", UserId);

            string query = " select QIId,Question_Desc,NoOfQuestions,MarksPerQuestion " +
                    " from Cat_Question_Index where IsActive=1 ";
            cmd.CommandText = query;

            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;
        }

        internal DataTable GetUserTestDetails(DateTime startDate, DateTime endDate)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("GetUserTestDetails_Proc")
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 3600
            };

            cmd.Parameters.AddWithValue("@StartDate", startDate);
            cmd.Parameters.AddWithValue("@EndDate", endDate);
            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;
        }
        internal DataTable getUserCitations(int citationId, long userId)
        {
            DataTable dt = new DataTable();
            //select MarcId,Question,Answar from Cat_Marc_Tags  where IsActive=1 and QIId=1
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@citationId", citationId);
            cmd.Parameters.AddWithValue("@userId", userId);

            string query = " SELECT CitationId,Authors,BookTitle,BookChapter,SeriesTitle,ISBN,Editors,Publisher,PlaceOfPublication, "
                + " StartPage,EndPage,PgCount,YearOfPublication,PublicationType,Comments,Result FROM Cat_User_Citations where CitationId = @citationId and UserId= @userId";
            cmd.CommandText = query;

            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;
        }

        internal DataTable getUserCatalogs(int catalogId, long userId)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@catalogId", catalogId);
            cmd.Parameters.AddWithValue("@userId", userId);

            string query = " select cat_Answer,Comments,Result from Cat_User_Cataloguing where CatalogId=@catalogId and UserId=@userId";
            cmd.CommandText = query;

            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;
        }

        internal int UpdateEvaluatorDetails(long userId, long evaluatorId)
        {
            int insertCnt = 0;
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("UpdateUserDetails_Proc")
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 3600
                };

                cmd.Parameters.AddWithValue("@evaluatorId", evaluatorId);
                cmd.Parameters.AddWithValue("@userId", userId);
                insertCnt = this.SqlHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                insertCnt = 0;
            }
            finally
            {
                this.SqlHelper.Close();
            }

            return insertCnt;
        }

        internal int UpdateUserDetails(long userId)
        {
            int insertCnt = 0;
            try
            {

                SqlCommand cmd = new SqlCommand();
                string query = " update cat_UserDetails set ended_time=getdate(),submitted_type='FINISH',tot_Sec=DATEDIFF(SECOND,started_time,getdate()) where userid=@userId and role_id=3";
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@userId", userId);
                insertCnt = this.SqlHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                insertCnt = 0;
            }
            finally
            {
                this.SqlHelper.Close();
            }

            return insertCnt;
        }

        public DataTable getTagValues()
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            //cmd.Parameters.AddWithValue("@UserID", UserId);

            string query = " SELECT TagId, TagValue FROM Cat_TagValues_Master where IsActive=1 order by newid() ";
            cmd.CommandText = query;

            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;
        }

        internal int InsertCataloguingDetails(int catalogId, string cat_Answer, long userId)
        {
            int insertCnt = 0;
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("InsertCataloguingDetails_Proc")
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 3600
                };

                cmd.Parameters.AddWithValue("@catalogId", catalogId);
                cmd.Parameters.AddWithValue("@cat_Answer", cat_Answer);
                cmd.Parameters.AddWithValue("@userId", userId);
                insertCnt = this.SqlHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                insertCnt = 0;
            }
            finally
            {
                this.SqlHelper.Close();
            }

            return insertCnt;

        }

        public DataTable getMarcQuesctions(int QIId, long UserId)
        {
            DataTable dt = new DataTable();
            //select MarcId,Question,Answar from Cat_Marc_Tags  where IsActive=1 and QIId=1
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@QIId", QIId);
            cmd.Parameters.AddWithValue("@UserId", UserId);

            string query = " select a.MarcId,a.Question,a.Answar as Answer, isnull(b.Answar,'select') as UsrAnswer,isnull(b.Result,0) as Result,b.Comments " +
                " from Cat_Marc_Tags a " +
                " left join  Cat_User_Marc_Tags b on a.MarcId=b.MarcId and b.UserId=@UserId " +
                " where a.IsActive=1 and a.QIId=@QIId";
            cmd.CommandText = query;

            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;
        }

        internal int UpdateEvaluatorCataloguingDetails(int catalogId, long userId, long evaluatorId, bool checkedResult)
        {
            int insertCnt = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                string sqlquery = " if exists(select 1 from Cat_User_Cataloguing where CatalogId =@CatalogId and UserId =@UserId) " +
                    " update Cat_User_Cataloguing set Result = @Result, EvaluatorId = @EvaluatorId " +
                    "where CatalogId =@CatalogId and UserId =@UserId;" +
                    " else insert into Cat_User_Cataloguing(CatalogId,UserId,EvaluatorId,Result) values(@CatalogId,@UserId,@EvaluatorId,@Result)";

                cmd.CommandText = sqlquery;
                cmd.Parameters.AddWithValue("@CatalogId", catalogId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@EvaluatorId", evaluatorId);
                cmd.Parameters.AddWithValue("@Result", checkedResult);

                insertCnt = this.SqlHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                insertCnt = 0;
            }
            finally
            {
                this.SqlHelper.Close();
            }

            return insertCnt;
        }

        internal DataTable getEvaluatorImageDetails(int imgMastId, long userId)
        {
            
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@ImgMastId", imgMastId);
            cmd.Parameters.AddWithValue("@UserId", userId);

            string query = " select Marks, Comments from Cat_Evaluator_ImageAnswers   where UserId = @UserId and ImgMastId = @ImgMastId";
            cmd.CommandText = query;

            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;

        }

        internal int InsertCitationDetails(int citationId, Dictionary<string, string> diCitations, long userId)
        {
            int insertCnt = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("InsertCitationDetails_Proc")
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 3600
                };

                foreach (KeyValuePair<string, string> entry in diCitations)
                {
                    cmd.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                }
                cmd.Parameters.AddWithValue("@CitationId", citationId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                insertCnt = this.SqlHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                insertCnt = 0;
            }
            finally
            {
                this.SqlHelper.Close();
            }

            return insertCnt;
        }

        internal int UpdateEevaluatorCitationDetails(int citationId, long userId, long evaluatorId, string comments, decimal result)
        {
            int insertCnt = 0;

            try
            {
                SqlCommand cmd = new SqlCommand();
                string sqlquery = "update Cat_User_Citations" +
                    "  set Comments = @Comments, Result = @Result, EvaluatorId = @EvaluatorId " +
                    "where CitationId =@CitationId and UserId =@UserId ";

                cmd.CommandText = sqlquery;
                cmd.Parameters.AddWithValue("@CitationId", citationId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@EvaluatorId", evaluatorId);
                cmd.Parameters.AddWithValue("@Result", result);
                cmd.Parameters.AddWithValue("@Comments", comments);

                insertCnt = this.SqlHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                insertCnt = 0;
            }
            finally
            {
                this.SqlHelper.Close();
            }

            return insertCnt;
        }

        internal int InsertMarcDetails(string marcIds, string marcAns, long userId,long evaluatorId, string comments)
        {
            int insertCnt =0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("InsertMarcDetails_Proc")
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 3600
                };

                cmd.Parameters.AddWithValue("@marcIds", marcIds);
                cmd.Parameters.AddWithValue("@marcAns", marcAns);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@evaluatorId", evaluatorId);
                cmd.Parameters.AddWithValue("@comments", comments);
                insertCnt = this.SqlHelper.ExecuteNonQuery(cmd);

            }
            catch(Exception ex)
            {
                insertCnt = 0;
            }
            finally
            {
                this.SqlHelper.Close();
            }

            return insertCnt;
        }

        internal int InsertEvaluatorImageDetails(int imgMastId, long userId, long evaluatorId, decimal marks, string comments)
        {
            int insertCnt = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                string sqlquery = " if exists(select 1 from Cat_Evaluator_ImageAnswers where ImgMastId =@ImgMastId and UserId =@UserId) " +
                    " update Cat_Evaluator_ImageAnswers set Marks = @Marks,Comments=@Comments, EvaluatorId = @EvaluatorId " +
                    "where ImgMastId =@ImgMastId and UserId =@UserId;" +
                    " else insert into Cat_Evaluator_ImageAnswers(ImgMastId,UserId,EvaluatorId,Marks,Comments) values(@ImgMastId,@UserId,@EvaluatorId,@Marks,@Comments)";

                cmd.CommandText = sqlquery;
                cmd.Parameters.AddWithValue("@ImgMastId", imgMastId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@EvaluatorId", evaluatorId);
                cmd.Parameters.AddWithValue("@Marks", marks);
                cmd.Parameters.AddWithValue("@Comments", comments);

                insertCnt = this.SqlHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                insertCnt = 0;
            }
            finally
            {
                this.SqlHelper.Close();
            }

            return insertCnt;
        }

        public DataTable getCitationQuesctions(int QIId)
        {
            DataTable dt = new DataTable();
            //select MarcId,Question,Answar from Cat_Marc_Tags  where IsActive=1 and QIId=1
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@QIId", QIId);

            string query = " SELECT CitationId,Citation,Authors,BookTitle,BookChapter,SeriesTitle,ISBN,Editors,Publisher,PlaceOfPublication,"
                +" StartPage,EndPage,PgCount,YearOfPublication,PublicationType FROM Cat_Citation_Master where IsActive = 1 and QIId = @QIId";
            cmd.CommandText = query;

            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;
        }
        public DataTable getCataloguingQuesctions(int QIId)
        {
            DataTable dt = new DataTable();
            //select MarcId,Question,Answar from Cat_Marc_Tags  where IsActive=1 and QIId=1
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@QIId", QIId);

            string query = " select CatalogId,Cat_Question,Cat_Answer from Cat_Cataloguing_Master where IsActive=1 and QIId=@QIId";
            cmd.CommandText = query;

            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;
        }

        internal int InsertimageDetails(string imgAnsIds, string imgAns, long userId)
        {
            int insertCnt = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("InsertImageDetails_Proc")
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 3600
                };

                cmd.Parameters.AddWithValue("@imgAnsIds", imgAnsIds);
                cmd.Parameters.AddWithValue("@imgAns", imgAns);
                cmd.Parameters.AddWithValue("@userId", userId);
                insertCnt = this.SqlHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                insertCnt = 0;
            }
            finally
            {
                this.SqlHelper.Close();
            }

            return insertCnt;
        }

        public DataTable getCatalogOptions()
        {
            DataTable dt = new DataTable();
            //select MarcId,Question,Answar from Cat_Marc_Tags  where IsActive=1 and QIId=1
            SqlCommand cmd = new SqlCommand();
            //cmd.Parameters.AddWithValue("@QIId", QIId);

            string query = " SELECT OptId, CatalogOption, CatalogId FROM Cat_Catalog_Options where IsActive=1";
            cmd.CommandText = query;

            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;
        }
        public DataTable getImagePartsQuesctions(int QIId)
        {
            DataTable dt = new DataTable();
            //select MarcId,Question,Answar from Cat_Marc_Tags  where IsActive=1 and QIId=1
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@QIId", QIId);

            string query = "SELECT ImgMastId, ImageDesc FROM Cat_ImageMaster where IsActive=1 and QIId =@QIId";
            cmd.CommandText = query;

            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;
        }


        public DataTable getImagePaths()
        {
            DataTable dt = new DataTable();
            //select MarcId,Question,Answar from Cat_Marc_Tags  where IsActive=1 and QIId=1
            SqlCommand cmd = new SqlCommand();
            //cmd.Parameters.AddWithValue("@QIId", QIId);

            string query = " SELECT PathId,ImagePath,ImgMastId FROM Cat_ImagePaths WHERE IsActive=1 ";
            cmd.CommandText = query;

            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;
        }
        public DataTable getImageAnswers(long userId)
        {
            DataTable dt = new DataTable();
            //select MarcId,Question,Answar from Cat_Marc_Tags  where IsActive=1 and QIId=1
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@userId", userId);

            string query = " SELECT a.ImgAnsId, ImgQuestion, a.ImgAnswer, QueType, ImgMastId,b.ImgAnswer as UsrAnswer " +
                " FROM Cat_ImageAnswers a " +
                " left join Cat_User_ImageAnswers b on a.ImgAnsId=b.ImgAnsId and b.UserId=@userId " +
                " WHERE IsActive=1 ";
            cmd.CommandText = query;

            SqlDataReader dr = this.SqlHelper.ExecuteDataReader(cmd);

            dt.Load(dr);
            if (!dr.IsClosed)
            {
                dr.Close();
            }

            this.SqlHelper.Close();
            return dt;
        }
    }
}