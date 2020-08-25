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
    public partial class Cataloguing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {
                btnCataloguingDone.Visible = false;
                btnCataloguingNext.Visible = true;
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
                    btnCataloguingDone.Enabled = false;
                }

                GetCatalogOptions();
                GetCatalogs();
                BindCatalogs();
                FillUserCatalogs();
                //BindCatalogs();
            }

            if (IsPostBack)
            {
                if (BulletedList1.Items.Count > 0)
                {
                    int idx = BulletedList1.Items.IndexOf(BulletedList1.Items.FindByValue(hdfQn.Value));
                    if (idx == (BulletedList1.Items.Count - 1))
                    {
                        btnCataloguingDone.Visible = true;
                        btnCataloguingNext.Visible = false;
                    }
                }
            }
        }

        private void GetCatalogOptions()
        {
            Catalog_DAC dac = new Catalog_DAC();
            DataTable dt = dac.getCatalogOptions();
            dac = null;
            ViewState["dtCtlgOpts"] = dt;
        }

        private void GetCatalogs()
        {
            Catalog_DAC dac = new Catalog_DAC();
            DataTable dt = dac.getCataloguingQuesctions(4);
            dac = null;
            ViewState["dtCtlgQns"] = dt;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    ListItem li = new ListItem();
                    li.Text = i.ToString();
                    li.Value = dt.Rows[i - 1]["CatalogId"].ToString();
                    BulletedList1.Items.Add(li);
                }
                hdfQn.Value = BulletedList1.Items[0].Value;
            }
        }

        private void BindCatalogs()
        {
            int CatalogId = 0;
            if (hdfQn.Value.Length > 0)
            {
                int.TryParse(hdfQn.Value, out CatalogId);
            }
            if (CatalogId > 0)
            {
                DataTable dtCtlgOpts = ViewState["dtCtlgOpts"] as DataTable;

                if (dtCtlgOpts != null && dtCtlgOpts.Rows.Count > 0)
                {
                    lblCtlgQn.Text = string.Empty;

                    DataTable dtCtlgQns = ViewState["dtCtlgQns"] as DataTable;
                    if (dtCtlgQns != null && dtCtlgQns.Rows.Count > 0)
                    {
                        DataRow[] drCtlgQn = dtCtlgQns.Select("CatalogId=" + CatalogId + "");
                        lblCtlgQn.Text = drCtlgQn[0]["Cat_Question"].ToString();
                        lblCtlgAns.Text = drCtlgQn[0]["Cat_Answer"].ToString();
                    }

                    rbtnCtlgOpts.Items.Clear();
                    rbtnCtlgOpts.ClearSelection();
                    DataTable dtTemp = dtCtlgOpts.Clone();
                    DataRow[] drCtlg = dtCtlgOpts.Select("CatalogId=" + CatalogId + "");

                    if (Session["UserType"].ToString() == "User")
                    {
                        foreach (DataRow dr in drCtlg)
                        {
                            ListItem li = new ListItem();
                            li.Text = dr["CatalogOption"].ToString();
                            li.Value = dr["OptId"].ToString();
                            rbtnCtlgOpts.Items.Add(li);
                        }
                    }
                }
            }
        }
        protected void FillUserCatalogs()
        {
            lblUsrAns.Text = string.Empty;
            chkResult.Checked = false;

            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);

            int CatalogId = 0;
            if (hdfQn.Value.Length > 0)
            {
                int.TryParse(hdfQn.Value, out CatalogId);
            }
            if (CatalogId > 0)
            {
                rbtnCtlgOpts.ClearSelection();

                Catalog_DAC dac = new Catalog_DAC();
                DataTable dt = dac.getUserCatalogs(CatalogId, UserId);
                dac = null;

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Session["UserType"].ToString() == "User")
                    {
                        rbtnCtlgOpts.SelectedIndex = rbtnCtlgOpts.Items.IndexOf(rbtnCtlgOpts.Items.FindByText(dt.Rows[0]["Cat_Answer"].ToString()));
                    }
                    if (Session["UserType"].ToString() == "Evaluator" || Session["UserType"].ToString() == "Administrator")
                    {
                        rbtnCtlgOpts.Visible = false;
                        divEval.Visible = true;
                        lblUsrAns.Text = dt.Rows[0]["Cat_Answer"].ToString();
                        bool chkres = false;
                        bool.TryParse(dt.Rows[0]["Result"].ToString(), out chkres);
                        chkResult.Checked = chkres;
                    }
                }
            }
        }

        protected void BulletedList1_Click(object sender, BulletedListEventArgs e)
        {
            //Label1.Text = "SelectedIndex=" + e.Index.ToString() + ",Value=" + BulletedList1.Items[e.Index].Value;
            hdfQn.Value = BulletedList1.Items[e.Index].Value;
            BindCatalogs();
            FillUserCatalogs();
        }

        private void SaveCataloguingData(bool IsSubmit)
        {
            string Cat_Answer = string.Empty;
            int CatalogId = 0;
            int.TryParse(hdfQn.Value, out CatalogId);

            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);

            long EvaluatorId = 0;
            bool CheckedResult = false;

            if (Session["UserType"].ToString() == "User")
            {
                if (rbtnCtlgOpts.SelectedIndex >= 0)
                {
                    Cat_Answer = rbtnCtlgOpts.SelectedItem.Text;
                }
                Catalog_DAC dac = new Catalog_DAC();
                int res = dac.InsertCataloguingDetails(CatalogId, Cat_Answer, UserId);
                if (res > 0)
                {
                    if (IsSubmit)
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "marin", "alert('Recard saved.')", true);
                    }
                }
                dac = null;

            }
            if (Session["UserType"].ToString() == "Evaluator")
            {
                bool.TryParse(chkResult.Checked.ToString(), out CheckedResult);

                Catalog_DAC dac = new Catalog_DAC();
                int res = dac.UpdateEvaluatorCataloguingDetails(CatalogId, UserId, EvaluatorId, CheckedResult);
                if (res > 0)
                {
                    if (IsSubmit)
                    {
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "marin", "alert('Recard saved.')", true);
                    }
                }
                dac = null;
            }
        }

        protected void FinishData()
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
            if (Session["UserType"] != null && Session["UserType"].ToString() == "Administrator")
            {
                Session["UserId"] = null;
                Response.Redirect("AdminHome.aspx");
            }
        }

        protected void btnCataloguingDone_Click(object sender, EventArgs e)
        {
            SaveCataloguingData(true);
            BindCatalogs();
            FillUserCatalogs();
            FinishData();
        }

        protected void btnCataloguingPrevious_Click(object sender, EventArgs e)
        {
            if (Session["UserType"] != null && Session["UserType"].ToString() != "Administrator")
            {
                SaveCataloguingData(false);
            }
            int idx = BulletedList1.Items.IndexOf(BulletedList1.Items.FindByValue(hdfQn.Value));

            if (idx == 0)
            {
                Response.Redirect("ImageParts.aspx");
            }
            else
            {
                hdfQn.Value = BulletedList1.Items[(idx - 1)].Value;
                BindCatalogs();
                FillUserCatalogs();
            }

        }
        protected void btnCataloguingNext_Click(object sender, EventArgs e)
        {
            if (Session["UserType"] != null && Session["UserType"].ToString() != "Administrator")
            {
                SaveCataloguingData(false);
            }
            int idx = BulletedList1.Items.IndexOf(BulletedList1.Items.FindByValue(hdfQn.Value));

            if (idx < (BulletedList1.Items.Count - 1))
            {
                hdfQn.Value = BulletedList1.Items[(idx + 1)].Value;
                BindCatalogs();
                FillUserCatalogs();

                if ((idx + 1) == (BulletedList1.Items.Count - 1))
                {
                    btnCataloguingDone.Visible = true;
                    btnCataloguingNext.Visible = false;
                }
            }
        }
        protected void BulletedList1_PreRender(object sender, EventArgs e)
        {
            int idx = BulletedList1.Items.IndexOf(BulletedList1.Items.FindByValue(hdfQn.Value));
            BulletedList1.Items[idx].Attributes.Add("style", "color:white;background-color:mediumturquoise;border-color:#17a2b8;");
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
                SaveCataloguingData(true);

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