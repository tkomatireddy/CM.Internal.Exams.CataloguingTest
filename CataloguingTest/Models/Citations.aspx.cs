using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CataloguingTest;
using System.Data;

namespace CataloguingTest
{
    public partial class Citations : System.Web.UI.Page
    {
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
                    hdfRoleId.Value = Session["RoleId"].ToString();
                }

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
                    btnCitationsDone.Enabled = false;
                }

                GetCitations();
                BindCitation();
                FillUserCitations();
            }
        }

        private void GetCitations()
        {
            BulletedList1.Items.Clear();
            Catalog_DAC dac = new Catalog_DAC();
            DataTable dt = dac.getCitationQuesctions(2);
            dac = null;
            ViewState["dtCitations"] = dt;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    ListItem li = new ListItem();
                    li.Text = i.ToString();
                    li.Value = dt.Rows[i - 1]["CitationId"].ToString();
                    BulletedList1.Items.Add(li);
                }
                hdfQn.Value = BulletedList1.Items[0].Value;
            }
        }
        private void BindCitation()
        {
            int CitationId = 0;
            if (hdfQn.Value.Length > 0)
            {
                int.TryParse(hdfQn.Value, out CitationId);
            }
            if (CitationId > 0)
            {
                DataTable dtCitations = ViewState["dtCitations"] as DataTable;

                if (dtCitations != null && dtCitations.Rows.Count > 0)
                {
                    DataTable dtTemp = dtCitations.Clone();
                    DataRow[] drCitations = dtCitations.Select("CitationId=" + CitationId + "");
                    dtTemp.ImportRow(drCitations[0]);
                    dvCitation.DataSource = dtTemp;
                    dvCitation.DataBind();
                }
            }
        }

        private void FillUserCitations()
        {
            foreach (DetailsViewRow dvr in dvCitation.Rows)
            {
                if (dvr.Cells[1].HasControls() == true && dvr.Cells[1].Controls.Count > 0)
                {
                    TextBox txt = dvr.Cells[1].Controls[1] as TextBox;
                    if (txt != null)
                    {
                        txt.Text = string.Empty;
                        if (txt.ID == "txtComments" || txt.ID == "txtResult")
                        {
                            if (Session["UserType"].ToString() == "User")
                            {
                                dvr.Visible = false;
                            }
                            else
                                dvr.Visible = true;

                            if (Session["UserType"].ToString() == "Evaluator")
                            {
                                dvr.Visible = true;
                                txt.Enabled = true;
                                txt.Visible = true;

                            }
                            else if (Session["UserType"].ToString() == "Administrator")
                            {
                                txt.Visible = true;
                                txt.Enabled = false;

                            }
                        }
                    }
                }
            }
            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);

            int CitationId = 0;
            if (hdfQn.Value.Length > 0)
            {
                int.TryParse(hdfQn.Value, out CitationId);
            }
            if (CitationId > 0)
            {
                Catalog_DAC dac = new Catalog_DAC();
                DataTable dt = dac.getUserCitations(CitationId, UserId);
                dac = null;

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    if (dvCitation.Rows.Count > 0)
                    {
                        foreach (DetailsViewRow dvr in dvCitation.Rows)
                        {
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (dc.ColumnName != "Citation")
                                {
                                    if (dvr.Cells[0].Text == dc.ColumnName)
                                    {
                                        TextBox txt = dvr.Cells[1].FindControl("txt" + dc.ColumnName) as TextBox;
                                        Label lbl = dvr.Cells[1].FindControl("lbl" + dc.ColumnName) as Label;
                                        if (txt != null)
                                        {
                                            txt.Text = dr[dc.ColumnName].ToString();
                                        }

                                        if (Session["UserType"].ToString() == "Evaluator" || Session["UserType"].ToString() == "Administrator")
                                        {
                                            if (lbl != null && lbl.Visible == false)
                                            {
                                                lbl.Visible = true;
                                            }
                                        }

                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void BulletedList1_Click(object sender, BulletedListEventArgs e)
        {
            //Label1.Text = "SelectedIndex=" + e.Index.ToString() + ",Value=" + BulletedList1.Items[e.Index].Value;
            hdfQn.Value = BulletedList1.Items[e.Index].Value;
            BindCitation();
            FillUserCitations();
        }

        protected void btnCitationsDone_Click(object sender, EventArgs e)
        {

            SaveCitationsData();
            BindCitation();
            FillUserCitations();
        }

        private void SaveCitationsData()
        {
            int CitationId = 0;
            int.TryParse(hdfQn.Value, out CitationId);

            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);

            DataTable dtCitations = ViewState["dtCitations"] as DataTable;
            if (Session["UserType"].ToString() == "User")
            {
                Dictionary<string, string> diCitations = new Dictionary<string, string>();

                foreach (DetailsViewRow dvr in dvCitation.Rows)
                {
                    foreach (DataColumn dc in dtCitations.Columns)
                    {
                        if (dc.ColumnName != "Citation")
                        {
                            TextBox txt = dvr.FindControl("txt" + dc.ColumnName) as TextBox;
                            if (txt != null)
                            {
                                diCitations.Add(dc.ColumnName, txt.Text.Trim());
                            }
                        }
                    }
                    break;
                }

                if (CitationId > 0 && diCitations.Count > 0)
                {
                    Catalog_DAC dac = new Catalog_DAC();
                    int res = dac.InsertCitationDetails(CitationId, diCitations, UserId);
                    if (res > 0)
                    {
                        // Page.ClientScript.RegisterStartupScript(typeof(Page), "marin", "alert('Recard saved.')", true);
                    }
                    dac = null;
                    //BindCitation();
                }
            }
            if (Session["UserType"].ToString() == "Evaluator")
            {
                long EvaluatorId = 0;
                decimal Marks = 0;
                string Comments = string.Empty;

                long.TryParse(Session["EvaluatorId"].ToString(), out EvaluatorId);

                foreach (DetailsViewRow dvr in dvCitation.Rows)
                {
                    TextBox txtComments = dvr.Cells[1].FindControl("txtComments") as TextBox;
                    if (txtComments != null)
                    {
                        Comments = txtComments.Text.Trim();
                    }
                    TextBox txtResult = dvr.Cells[1].FindControl("txtResult") as TextBox;
                    if (txtResult != null)
                    {
                        decimal.TryParse(txtResult.Text.Trim(), out Marks);
                    }
                }

                if (CitationId > 0 && EvaluatorId > 0)
                {

                    Catalog_DAC dac = new Catalog_DAC();
                    int res = dac.UpdateEevaluatorCitationDetails(CitationId, UserId, EvaluatorId, Comments, Marks);
                    if (res > 0)
                    {
                        //Page.ClientScript.RegisterStartupScript(typeof(Page), "marin", "alert('Recard saved.')", true);
                    }
                    dac = null;
                }
            }
        }

        protected void btnCitationsPrevious_Click(object sender, EventArgs e)
        {
            if (Session["UserType"] != null && Session["UserType"].ToString() != "Administrator")
            {
                SaveCitationsData();
            }
            int idx = BulletedList1.Items.IndexOf(BulletedList1.Items.FindByValue(hdfQn.Value));

            if (idx == 0)
            {
                Response.Redirect("MarcTags.aspx");
            }
            else
            {
                hdfQn.Value = BulletedList1.Items[(idx - 1)].Value;
                BindCitation();
                FillUserCitations();
            }
        }

        protected void btnCitationsNext_Click(object sender, EventArgs e)
        {
            if (Session["UserType"] != null && Session["UserType"].ToString() != "Administrator")
            {
                SaveCitationsData();
            }
            int idx = BulletedList1.Items.IndexOf(BulletedList1.Items.FindByValue(hdfQn.Value));

            if (idx == (BulletedList1.Items.Count - 1))
            {
                Response.Redirect("ImageParts.aspx");
            }
            else
            {
                hdfQn.Value = BulletedList1.Items[(idx + 1)].Value;
                BindCitation();
                FillUserCitations();
            }
        }
        protected void BulletedList1_PreRender(object sender, EventArgs e)
        {
            int idx = BulletedList1.Items.IndexOf(BulletedList1.Items.FindByValue(hdfQn.Value));
            BulletedList1.Items[idx].Attributes.Add("style", "color:white;background-color:mediumturquoise;border-color:#17a2b8;");
        }
        protected void lblLogOut_Click(object sender, EventArgs e)
        {
            hdfTimer.Value = string.Empty;
            Session.Clear();
            Response.Redirect("~/Login.aspx");
        }
        protected void btnTimeOut_Click(object sender, EventArgs e)
        {
            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);

            if (Session["UserType"] != null && Session["UserType"].ToString() == "User")
            {
                SaveCitationsData();

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