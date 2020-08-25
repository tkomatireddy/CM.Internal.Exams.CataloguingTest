using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CataloguingTest;
using System.Data;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace CataloguingTest
{
    public partial class AdminHome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["UserType"] != null)
                {
                    hdfStartTime.Value = string.Empty;
                    Session["StartTime"] = "";
                    Session["UserName"] = null;
                    Session["UserType"] = null;
                    Session["EvaluatorId"] = null;
                    Session["LoginName"] = null;

                    Session["UserType"] = Request.QueryString["UserType"];

                    if (Request.QueryString["UserType"] == "Evaluator")
                    {
                        if (Request.QueryString["UserId"] != null)
                        {
                            Session["EvaluatorId"] = Request.QueryString["UserId"];
                        }
                    }
                    if (Request.QueryString["LoginName"] != null)
                    {
                        Session["LoginName"] = Request.QueryString["LoginName"];
                    }
                }
                

                if (Session["LoginName"] != null)
                {
                    lblLoginName.Text = Session["LoginName"].ToString();
                }
                //Session["StartTime"] = DateTime.Now.ToShortTimeString();
                txtFromDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
                txtToDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
                GetUserTestDetails();
            }
        }

        private void GetUserTestDetails()
        {
            DateTime StartDate = DateTime.Now.Date;
            DateTime EndDate = DateTime.Now.Date;
            StartDate = Convert.ToDateTime(txtFromDate.Text);
            EndDate = Convert.ToDateTime(txtToDate.Text);
            gvUserTestDtls.DataSource = null;
            Catalog_DAC dac = new Catalog_DAC();
            {
                DataTable dt = dac.GetUserTestDetails(StartDate, EndDate);
                gvUserTestDtls.DataSource = dt;
            }
            gvUserTestDtls.DataBind();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {

        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetUserTestDetails();
        }
        protected void lnkGoToQuesctions_Click(object sender, EventArgs e)
        {
            LinkButton lnkGoToQuesctions = (LinkButton)sender;
            long UserId = 0;
            long.TryParse(lnkGoToQuesctions.CommandArgument, out UserId);

            if (UserId > 0)
            {
                Session["UserId"] = UserId;
                Session["UserName"] = lnkGoToQuesctions.CommandName;
                Response.Redirect("Home.aspx");
            }
        }

        protected void gvUserTestDtls_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void lblLogOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            //hdfTimer.Value = string.Empty;
            Response.Redirect("~/Login.aspx");
        }

        [WebMethod(EnableSession = true)]
        public static string SetSession(string StartTime)
        {
            HttpContext.Current.Session["StartTime"] = StartTime;
            return "Hello " + HttpContext.Current.Session["StartTime"] + Environment.NewLine + "The Current Time is: " + DateTime.Now.ToString();
        }
    }
}