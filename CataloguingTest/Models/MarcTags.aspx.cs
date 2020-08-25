using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using CataloguingTest;

namespace CataloguingTest
{
    public partial class MarcTags : System.Web.UI.Page
    {
        /// <summary>
        /// log4net class
        /// </summary>
        private static readonly log4net.ILog Log = Logging.AddLogger(typeof(Home));

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["UserType"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                if (Session["crnttime"] != null)
                {
                    hdfTimer.Value = Session["crnttime"].ToString();
                    hdfRoleId.Value = Session["RoleId"].ToString();
                }

                //HttpCookie cookiename = Request.Cookies.Get("cookiename");

                //// Check if cookie exists in the current request.
                //if (cookiename == null)
                //{
                //    hdfTimer.Value = cookiename.Value;
                //}

                //if (Session["LoginName"] != null)
                //{
                //    lblLoginName.Text = Session["LoginName"].ToString();
                //}
                if (Session["UserName"] != null)
                {
                    lblUserName.Text = Session["UserName"].ToString();
                }

                if (Session["UserType"] != null && Session["UserType"].ToString() == "Administrator")
                {
                    btnMarcDone.Enabled = false;
                }

                GetTagValues();
                GetMarcTags();
            }
        }

        private void GetTagValues()
        {
            Catalog_DAC dac = new Catalog_DAC();
            DataTable dt = dac.getTagValues();
            if (dt != null && dt.Rows.Count > 0)
            {
                ViewState["TagValues"] = dt;
            }            
        }
        private void GetMarcTags()
        {
            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);

            gvMarcTags.DataSource = null;
            Catalog_DAC dac = new Catalog_DAC();
            DataTable dt = dac.getMarcQuesctions(1, UserId);
            dac = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                gvMarcTags.DataSource = dt;
            }
            gvMarcTags.DataBind();
        }


         protected void gvMarcTags_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[5].Visible = false;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[7].Visible = false;

            if (Session["UserType"] != null && ( Session["UserType"].ToString() == "Evaluator" || Session["UserType"].ToString() == "Administrator"))
            {
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
                e.Row.Cells[7].Visible = true;
            }

           if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if(ViewState["TagValues"] != null)
                {
                    DataTable dt = ViewState["TagValues"] as DataTable;
                    if(dt != null && dt.Rows.Count>0)
                    {
                        DropDownList ddlTagValues = e.Row.FindControl("ddlTagValues") as DropDownList;
                        if (ddlTagValues != null)
                        {
                            ddlTagValues.DataSource = dt;
                            ddlTagValues.DataTextField = "TagValue";
                            ddlTagValues.DataValueField = "TagId";
                            ddlTagValues.DataBind();

                            ddlTagValues.Items.Insert(0, new ListItem("select", "0"));
                        }
                        ddlTagValues.SelectedIndex = ddlTagValues.Items.IndexOf(ddlTagValues.Items.FindByText(e.Row.Cells[5].Text));
                    }
                }
            }
        }

        protected void btnMarcDone_Click(object sender, EventArgs e)
        {
            SaveMarcData();
            Response.Redirect("Citations.aspx");
        }

        private void SaveMarcData()
        {
            string MarcIds = string.Empty;
            string MarcAns = string.Empty;
            string Comments = string.Empty;

            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);

            long EvaluatorId = 0;

            if (Session["UserType"].ToString() == "User")
            {
                foreach (GridViewRow gvr in gvMarcTags.Rows)
                {
                    if (MarcIds == string.Empty)
                    {
                        MarcIds = gvr.Cells[1].Text;
                    }
                    else
                    {
                        MarcIds = MarcIds + "~" + gvr.Cells[1].Text;
                    }

                    DropDownList ddlTV = gvr.FindControl("ddlTagValues") as DropDownList;
                    if (MarcAns == string.Empty)
                    {
                        MarcAns = ddlTV.SelectedItem.Text;
                    }
                    else
                    {
                        MarcAns = MarcAns + "~" + ddlTV.SelectedItem.Text;
                    }
                }
                if (MarcIds != string.Empty)
                {
                    Catalog_DAC dac = new Catalog_DAC();
                    int res = dac.InsertMarcDetails(MarcIds, MarcAns, UserId, EvaluatorId, Comments);
                    if(res>0)
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "marin", "alert('Recard saved.')", true);
                    }
                    dac = null;
                    GetMarcTags();
                }
            }

            int chkres = 0;
            if (Session["UserType"].ToString() == "Evaluator")
            {
                long.TryParse(Session["EvaluatorId"].ToString(), out EvaluatorId);

                foreach (GridViewRow gvr in gvMarcTags.Rows)
                {
                    chkres = 0;
                    if (MarcIds == string.Empty)
                    {
                        MarcIds = gvr.Cells[1].Text;
                    }
                    else
                    {
                        MarcIds = MarcIds + "~" + gvr.Cells[1].Text;
                    }

                    CheckBox chkReselt = gvr.FindControl("chkReselt") as CheckBox;
                    chkres = (chkReselt.Checked ? 1 : 0);

                    if (MarcAns == string.Empty)
                    {
                        MarcAns = chkres.ToString();
                    }
                    else
                    {
                        MarcAns = MarcAns + "~" + chkres.ToString();
                    }

                    TextBox txtComments = gvr.FindControl("txtComments") as TextBox;
                    string tempcmt = (txtComments.Text.Trim().Length > 0 ? txtComments.Text : null);

                    if (Comments == string.Empty)
                    {
                        Comments = tempcmt;
                    }
                    else
                    {
                        Comments = Comments + "~" + tempcmt;
                    }

                }
                if (MarcIds != string.Empty)
                {
                    Catalog_DAC dac = new Catalog_DAC();
                    int res = dac.InsertMarcDetails(MarcIds, MarcAns, UserId, EvaluatorId, Comments);
                    if (res > 0)
                    {
                        //Page.ClientScript.RegisterStartupScript(typeof(Page), "marin", "alert('Recard saved.')", true);
                    }
                    dac = null;
                    GetMarcTags();
                }

            }
        }

        protected void btnMarcNext_Click(object sender, EventArgs e)
        {
            if (Session["UserType"] != null && Session["UserType"].ToString() != "Administrator")
            {
                SaveMarcData();
            }

            Response.Redirect("Citations.aspx");
        }

        protected void lblLogOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            hdfTimer.Value = string.Empty;
            Response.Redirect("~/Login.aspx");
        }

        protected void btnTimeOut_Click(object sender, EventArgs e)
        {
            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);

            if (Session["UserType"] != null && Session["UserType"].ToString() == "User")
            {
                SaveMarcData();

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
            }           
        }
    }
}