using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CataloguingTest;
using System.Data;
using System.Web.Services;


namespace CataloguingTest
{
    public partial class Home : System.Web.UI.Page
    {
        /// <summary>
        /// log4net class
        /// </summary>
        private static readonly log4net.ILog Log = Logging.AddLogger(typeof(Home));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                if (Session["crnttime"] != null)
                {
                    hdfTimer.Value = Session["crnttime"].ToString();
                    hdfRoleId.Value= Session["RoleId"].ToString();
                }


                //HttpCookie cookiename = Request.Cookies.Get("cookiename");

                //// Check if cookie exists in the current request.
                //if (cookiename != null)
                //{
                //    hdfTimer.Value = cookiename.Value;
                //}

                if (Request.QueryString["UserType"] != null)
                {
                    //hdfStartTime.Value = string.Empty;
                    Session["StartTime"] = "";

                    Session["UserType"] = null;
                    Session["EvaluatorId"] = null;
                    Session["LoginName"] = null;
                    Session["StartTime"] = null;

                    Session["UserType"] = Request.QueryString["UserType"];

                    if (Request.QueryString["UserType"] == "User")
                    {
                        if (Request.QueryString["UserId"] != null)
                        {
                            Session["UserId"] = Request.QueryString["UserId"];
                            Session["EvaluatorId"] = 0;
                        }
                    }
                }
                if (Request.QueryString["LoginName"] != null)
                {
                    Session["LoginName"] = Request.QueryString["LoginName"];
                }

                Catalog_DAC dac = new Catalog_DAC();
                {
                    DataTable dt = dac.getIndex();

                    gvIndex.DataSource = dt;
                    gvIndex.DataBind();
                }

                if (Session["UserType"] != null)
                {
                    if (Session["UserType"].ToString() == "User")
                    {
                        lnkHome.Visible = false;
                    }
                }

                if (Session["UserName"] != null)
                {
                    lblUserName.Text = Session["UserName"].ToString();
                }

            }
        }

        protected void gvIndex_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
            e.Row.Cells[1].Visible = false;
        }

        protected void lnkGoToQuesctions_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkDetails = (LinkButton)sender;
                int QIId = 0;
                int.TryParse(lnkDetails.CommandArgument, out QIId);

                if (QIId > 0)
                {
                    if (QIId == 1)
                    {
                        Response.Redirect("MarcTags.aspx");
                    }
                    else if (QIId == 2)
                    {
                        Response.Redirect("Citations.aspx");
                    }
                    else if (QIId == 3)
                    {
                        Response.Redirect("ImageParts.aspx");
                    }
                    else if (QIId == 4)
                    {
                        Response.Redirect("Cataloguing.aspx");
                    }
                    //Catalog_DAC dac = new Catalog_DAC();
                    //DataTable dtQn = dac.getIndexQuesctions(QIId);

                    //if (dtQn > 0)
                    //{
                    //    try
                    //    {

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Log.Error("Error while loding shipment", ex);
                    //        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Alert3", "alert('Error while Loding');", true);
                    //    }
                    //}
                }

                // ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "window.open('../Modules/AssignedUserDetails.aspx','','width=900,height=350,left=200,top=100');", true);
            }
            catch (Exception)
            {
            }
        }

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);

            if (Session["UserType"] != null && Session["UserType"].ToString() == "User")
            {
                Catalog_DAC dac = new Catalog_DAC();
                int res = dac.UpdateUserDetails(UserId);
                if (res > 0)
                {
                    Session["UserId"] = null;
                    Session["UserType"] = null;
                    Response.Redirect("~/Login.aspx");
                }
                dac = null;
            }
            if (Session["UserType"] != null && Session["UserType"].ToString() == "Evaluator")
            {
                long EvaluatorId = 0;
                long.TryParse(Session["EvaluatorId"].ToString(), out EvaluatorId);

                Catalog_DAC dac = new Catalog_DAC();
                int res = dac.UpdateEvaluatorDetails(UserId, EvaluatorId);
                dac = null;
                Session["UserId"] = null;
                Response.Redirect("AdminHome.aspx");
            }
            if (Session["UserType"] != null && Session["UserType"].ToString() == "Evaluator")
            {
                Session["UserId"] = null;
                Response.Redirect("AdminHome.aspx");
            }
        }

        [WebMethod()]
        public void SetSession(string time)
        {

            Session["crnttime"] = time;

        }

        protected void lblLogOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            hdfTimer.Value = string.Empty;
            Response.Redirect("~/Login.aspx");
        }
    }

}