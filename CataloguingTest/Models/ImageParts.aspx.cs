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
    public partial class ImageParts : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    GetImgPaths();
            //    GeImgQuestions();
            //    BindCatalogs();
            //    Session["tblImages"] = tblImages;
            //}
            //tblImages = (Table)Session["tblImages"];
        }

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
                    btnImagePartsDone.Enabled = false;
                }

                GetImgPaths();
                GeImgQuestions();
                BindCatalogs();
                BindImgAnswers();
                //Session["tblImages"] = tblImages;
            }
        }

        private void GetImgPaths()
        {
            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);
            Catalog_DAC dac = new Catalog_DAC();
            DataTable dt = dac.getImagePaths();
            ViewState["dtImgPaths"] = dt;

            DataTable dt1 = dac.getImageAnswers(UserId);
            ViewState["dtImgAns"] = dt1;

            dac = null;
        }
        private void GeImgQuestions()
        {
            BulletedList1.Items.Clear();
            Catalog_DAC dac = new Catalog_DAC();
            DataTable dt = dac.getImagePartsQuesctions(3);
            dac = null;
            ViewState["dtImgQns"] = dt;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 1; i <= dt.Rows.Count; i++)
                {
                    ListItem li = new ListItem();
                    li.Text = i.ToString();
                    li.Value = dt.Rows[i - 1]["ImgMastId"].ToString();
                    BulletedList1.Items.Add(li);
                }
                hdfQn.Value = BulletedList1.Items[0].Value;
            }
        }
        private void BindCatalogs()
        {
            int ImgMastId = 0;
            if (hdfQn.Value.Length > 0)
            {
                int.TryParse(hdfQn.Value, out ImgMastId);
            }
            if (ImgMastId > 0)
            {
                DataTable dtImgPaths = ViewState["dtImgPaths"] as DataTable;
                DataTable dtImgAns = ViewState["dtImgAns"] as DataTable;

                if (dtImgPaths != null && dtImgPaths.Rows.Count > 0)
                {
                    lblCtlgQn.Text = string.Empty;

                    DataTable dtImgQns = ViewState["dtImgQns"] as DataTable;
                    if (dtImgQns != null && dtImgQns.Rows.Count > 0)
                    {
                        DataRow[] drImgQns = dtImgQns.Select("ImgMastId=" + ImgMastId + "");

                        lblCtlgQn.Text = drImgQns[0]["ImageDesc"].ToString();
                    }

                    DataRow[] drImgPaths = dtImgPaths.Select("ImgMastId=" + ImgMastId + "");
                    DataTable dtTemp = drImgPaths.CopyToDataTable();

                    DataRow[] drImgAns = dtImgAns.Select("ImgMastId=" + ImgMastId + "");
                    tblImages.Controls.Clear();
                    int r = 1;
                    TableRow tr = new TableRow();
                    foreach (DataRow dr in dtTemp.Rows)
                    {
                        if (r % 2 != 0)
                            tr = new TableRow();
                        tr.EnableViewState = true;
                        TableCell tc = new TableCell();
                        tc.EnableViewState = true;
                        Image ctrlimg = new Image();
                        ctrlimg.ID = dr["PathId"].ToString();
                        ctrlimg.ImageUrl = dr["ImagePath"].ToString();
                        ctrlimg.Attributes.Add("data-zoom-image", "file:///C:/Users/tkomatireddy/Downloads/Cover_TheQueenBee.pdf");
                        //data - zoom - image = '<%# ResolveUrl(Eval("ZoomImageUrl").ToString()) %>'
                        ctrlimg.EnableViewState = true;
                        ctrlimg.Style.Add("vertical-align", "top");

                        if (drImgAns != null && drImgAns.Length > 0 && drImgAns[0]["QueType"].ToString() == "CheckBox")
                        {
                            Label lbl = new Label();
                            lbl.ID = "lbl" + r.ToString();
                            lbl.Text = "Page(" + r.ToString() + ")";
                            //CheckBox chk = new CheckBox();
                            //chk.ID = "chk" + drImgAns[0]["ImgAnsId"].ToString();
                            //chk.Text = drImgAns[0]["ImgQuestion"].ToString();
                            //chk.EnableViewState = true;
                            tc.Controls.Add(lbl);
                        }
                        tc.Controls.Add(ctrlimg);
                        tr.Cells.Add(tc);
                        tblImages.Controls.Add(tr);
                        r++;
                    }
                }
            }
        }

        private void BindImgAnswers()
        {
            dvImgEvl.Visible = false;
            txtMarks.Text = string.Empty;
            txtComments.Text = string.Empty;

            rbtnImgOpts.Visible = true;
            divEval.Visible = false;

            lblImgAns.Text = string.Empty;
            lblUsrAns.Text = string.Empty;


            int ImgMastId = 0;
            if (hdfQn.Value.Length > 0)
            {
                int.TryParse(hdfQn.Value, out ImgMastId);
            }
            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);

            if (ImgMastId > 0)
            {
                DataTable dtImgAns = ViewState["dtImgAns"] as DataTable;
                DataRow[] drImgAns = dtImgAns.Select("ImgMastId=" + ImgMastId + "");
                gvImagesAns.DataSource = null;
                if (drImgAns != null && drImgAns.Length > 0 && drImgAns[0]["QueType"].ToString() == "TextBox")
                {
                    gvImagesAns.DataSource = drImgAns.CopyToDataTable();
                }
                gvImagesAns.DataBind();

                rbtnImgOpts.Items.Clear();
                if (drImgAns != null && drImgAns.Length > 0 && drImgAns[0]["QueType"].ToString() == "CheckBox")
                {
                    DataTable dtImgPaths = ViewState["dtImgPaths"] as DataTable;
                    DataRow[] drImgPaths = dtImgPaths.Select("ImgMastId=" + ImgMastId + "");
                    DataTable dtTemp = drImgPaths.CopyToDataTable();
                    int r = 1;
                    rbtnImgOpts.ToolTip = drImgAns[0]["ImgAnsId"].ToString();

                    foreach (DataRow dr in dtTemp.Rows)
                    {
                        ListItem li = new ListItem();
                        li.Text = "Page(" + r.ToString() + ")";
                        li.Value = dr["PathId"].ToString();
                        rbtnImgOpts.Items.Add(li);
                        r++;
                    }
                    rbtnImgOpts.SelectedIndex = rbtnImgOpts.Items.IndexOf(rbtnImgOpts.Items.FindByValue(drImgAns[0]["UsrAnswer"].ToString()));
                    if (Session["UserType"].ToString() == "Evaluator")
                    {
                        rbtnImgOpts.Visible = false;
                        divEval.Visible = true;

                        lblImgAns.Text = drImgAns[0]["ImgAnswer"].ToString();

                        lblUsrAns.Text = (rbtnImgOpts.SelectedIndex > -1 ? rbtnImgOpts.SelectedItem.Text : "");
                    }
                }
                if (Session["UserType"].ToString() == "Evaluator" || Session["UserType"].ToString() == "Administrator")
                {
                    dvImgEvl.Visible = true;
                    Catalog_DAC dac = new Catalog_DAC();
                    DataTable dt = dac.getEvaluatorImageDetails(ImgMastId, UserId);
                    dac = null;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        txtMarks.Text = dt.Rows[0]["Marks"].ToString();
                        txtComments.Text = dt.Rows[0]["Comments"].ToString();
                    }
                }
            }
        }

        protected void BulletedList1_Click(object sender, BulletedListEventArgs e)
        {
            //Label1.Text = "SelectedIndex=" + e.Index.ToString() + ",Value=" + BulletedList1.Items[e.Index].Value;
            hdfQn.Value = BulletedList1.Items[e.Index].Value;
            BindCatalogs();
            BindImgAnswers();
        }


        protected void gvImagesAns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            if (Session["UserType"].ToString() == "User")
            {
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[4].Visible = false;
                e.Row.Cells[5].Visible = false;
            }
            if (Session["UserType"].ToString() == "Evaluator")
            {
                e.Row.Cells[3].Visible = false;
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = true;
            }

            TextBox txtImgAns = e.Row.FindControl("txtImgAns") as TextBox;

            if (txtImgAns != null)
            {
                if (e.Row.Cells[5].Text != "&nbsp;")
                    txtImgAns.Text = e.Row.Cells[5].Text.Trim();
            }
        }

        
        private void SaveImagePartsData()
        {
            BindCatalogs();
            long UserId = 0;
            long.TryParse(Session["UserId"].ToString(), out UserId);
            int ImgMastId = 0;
            if (hdfQn.Value.Length > 0)
            {
                int.TryParse(hdfQn.Value, out ImgMastId);
            }
            if (ImgMastId > 0)
            {
                string ImgAnsIds = string.Empty;
                string ImgAns = string.Empty;

                DataTable dtImgAns = ViewState["dtImgAns"] as DataTable;
                if (Session["UserType"].ToString() == "User")
                {
                    DataRow[] drImgAns = dtImgAns.Select("ImgMastId=" + ImgMastId + "");
                    if (drImgAns != null && drImgAns.Length > 0 && drImgAns[0]["QueType"].ToString() == "CheckBox")
                    {
                        if (rbtnImgOpts.SelectedIndex > 0)
                        {
                            if (ImgAnsIds == string.Empty)
                            {
                                ImgAnsIds = rbtnImgOpts.ToolTip;
                            }
                            else
                            {
                                ImgAnsIds = ImgAnsIds + "~" + rbtnImgOpts.ToolTip;
                            }

                            if (ImgAns == string.Empty)
                            {
                                ImgAns = rbtnImgOpts.SelectedItem.Value;
                            }
                            else
                            {
                                ImgAns = ImgAns + "~" + rbtnImgOpts.SelectedItem.Value;
                            }
                        }

                    }
                    else if (drImgAns != null && drImgAns.Length > 0 && drImgAns[0]["QueType"].ToString() == "TextBox")
                    {
                        foreach (GridViewRow gvr in gvImagesAns.Rows)
                        {
                            TextBox txtImgAns = gvr.FindControl("txtImgAns") as TextBox;
                            if (txtImgAns != null)
                            {
                                if (ImgAnsIds == string.Empty)
                                {
                                    ImgAnsIds = gvr.Cells[1].Text;
                                }
                                else
                                {
                                    ImgAnsIds = ImgAnsIds + "~" + gvr.Cells[1].Text;
                                }

                                string strtxt = (txtImgAns.Text.Length == 0 ? null : txtImgAns.Text);

                                if (ImgAns == string.Empty)
                                {
                                    ImgAns = strtxt;
                                }
                                else
                                {
                                    ImgAns = ImgAns + "~" + strtxt;
                                }
                            }
                        }
                    }

                    if (ImgAnsIds != string.Empty)
                    {
                        Catalog_DAC dac = new Catalog_DAC();
                        int res = dac.InsertimageDetails(ImgAnsIds, ImgAns, UserId);
                        if (res > 0)
                        {
                           // Page.ClientScript.RegisterStartupScript(typeof(Page), "marin", "alert('Recard saved.')", true);
                        }
                        dac = null;
                                           }
                }
                if (Session["UserType"].ToString() == "Evaluator")
                {
                    long EvaluatorId = 0;
                    decimal Marks = 0;
                    string Comments = string.Empty;

                    long.TryParse(Session["EvaluatorId"].ToString(), out EvaluatorId);
                    if (EvaluatorId > 0)
                    {
                        decimal.TryParse(txtMarks.Text, out Marks);
                        Comments = txtComments.Text.Trim();

                        Catalog_DAC dac = new Catalog_DAC();
                        int res = dac.InsertEvaluatorImageDetails(ImgMastId, UserId, EvaluatorId, Marks, Comments);
                        if (res > 0)
                        {
                           // Page.ClientScript.RegisterStartupScript(typeof(Page), "marin", "alert('Recard saved.')", true);
                        }
                        dac = null;
                       
                    }
                }
            }
        }
        protected void btnImagePartsDone_Click(object sender, EventArgs e)
        {
            SaveImagePartsData();
            GetImgPaths();
            BindCatalogs();
            BindImgAnswers();
        }

        protected void btnImagePartsPrevious_Click(object sender, EventArgs e)
        {
            if (Session["UserType"] != null && Session["UserType"].ToString() != "Administrator")
            {
                SaveImagePartsData();
            }
            int idx = BulletedList1.Items.IndexOf(BulletedList1.Items.FindByValue(hdfQn.Value));

            if (idx == 0)
            {
                Response.Redirect("Citations.aspx");
            }
            else
            {
                hdfQn.Value = BulletedList1.Items[(idx - 1)].Value;
                GetImgPaths();
                BindCatalogs();
                BindImgAnswers();
            }
        }

        protected void btnImagePartsNext_Click(object sender, EventArgs e)
        {
            if (Session["UserType"] != null && Session["UserType"].ToString() != "Administrator")
            {
                SaveImagePartsData();
            }
            int idx = BulletedList1.Items.IndexOf(BulletedList1.Items.FindByValue(hdfQn.Value));

            if (idx == (BulletedList1.Items.Count - 1))
            {
                Response.Redirect("Cataloguing.aspx");
            }
            else
            {
                hdfQn.Value = BulletedList1.Items[(idx + 1)].Value;
                GetImgPaths();
                BindCatalogs();
                BindImgAnswers();
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
                SaveImagePartsData();

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
    public class CountDownTimer
        {

            public TimeSpan TimeLeft;
            System.Threading.Thread thread;
            public CountDownTimer(TimeSpan original)
            {
                this.TimeLeft = original;
            }
            public void Start()
            {
                // Start a background thread to count down time
                thread = new System.Threading.Thread(() =>
                {
                    while (true)
                    {
                        System.Threading.Thread.Sleep(1000);
                        TimeLeft = TimeLeft.Subtract(TimeSpan.Parse("00:00:01"));
                    }
                });
                thread.Start();
            }

        }
    }