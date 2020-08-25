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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          if(!IsPostBack)
            {
                Session.Clear();
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string userName = string.Empty;
            string passWord = string.Empty;
            userName = txtExaminerName.Text;
            passWord = txtExaminerPassword.Text;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
            {
                if (userName.ToLower().EndsWith("@nes.co.in"))
                {
                    AdminEvaluator_Check(userName, passWord);
                }
                else
                {
                    UserLogin_Check(userName, passWord);
                }
            }
        }

        private void AdminEvaluator_Check(string userName, string passWord)
        {
            UserDetails umbl = new UserDetails();
            DataTable dtExaminerDetails = umbl.VerifyExaminerByExaminer(userName, passWord);
            if (dtExaminerDetails != null && dtExaminerDetails.Rows.Count > 0)
            {
                string strexaminerName = dtExaminerDetails.Rows[0]["examinerName"].ToString();

                if (strexaminerName.Length > 0) ////&& isStatus)
                {
                    Session["LoginName"] = userName;
                    int roleid = 1;

                    DataTable dtRoles = umbl.GetUserRole(userName);
                    if (dtRoles != null && dtRoles.Rows.Count > 0)
                    {
                        int.TryParse(dtRoles.Rows[0]["RoleId"].ToString(), out roleid);
                        Session["UserType"] = dtRoles.Rows[0]["RoleName"].ToString();
                    }
                    Session["RoleId"] = roleid.ToString();
                    long UserId = umbl.InsertCat_UserDetails(strexaminerName, strexaminerName, roleid);

                    Session["EvaluatorId"] = 0;
                    Session["UserId"] = 0;
                    if (roleid == 1)
                    {
                        Session["AdministratorId"] = UserId;
                    }
                    else if (roleid == 2)
                    {
                        Session["EvaluatorId"] = UserId;
                    }
                    if (UserId > 0)
                    {
                        Session["UserId"] = UserId;
                        Session["ModuleId"] = "44";
                        Response.Redirect("Models/AdminHome.aspx", false);

                        //Response.Redirect("http://10.68.98.83/CataloguingTest/models/AdminHome.aspx?UserType=" + Session["UserType"].ToString() + "&UserId=" + Session["UserId"].ToString() + "&LoginName=" + Session["userEmail"].ToString() + "", false);

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "window.open('UserDashboard.aspx','_parent');", true);
                    }
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "Popup", "window.open('ExaminerDashboard.aspx','_parent');", true);
                }
                else if (strexaminerName == "")
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "alert('User is not in active mode')", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "alert('Invalid username and password')", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "alert('Invalid username and password')", true);
            }
        }
        private void UserLogin_Check(string userName, string passWord)
        {
            UserDetails umbl = new UserDetails();
            int execStatus = 0;
            string result = "";
            DataTable dtCuntUsrDtls = umbl.GetCurrentUserDetails(userName, passWord, out execStatus, out result);
            if (execStatus != 0)
            {
                if (execStatus == 1)
                {
                    if (dtCuntUsrDtls != null & dtCuntUsrDtls.Rows.Count > 0)
                    {
                        DataRow dr = dtCuntUsrDtls.Rows[0];
                        Session["UserType"] = null;
                        Session["EvaluatorId"] = null;
                        Session["LoginName"] = null;
                        Session["StartTime"] = null;
                        Session["UserId"] = null;
                        Session["EvaluatorId"] = 0;
                        Session["UserName"] = null;
                        Session["RoleId"] = "3";

                        Session["UserType"] = "User";
                        Session["LoginName"] = dr["CreatedBy"].ToString();
                        Session["UserName"] = dr["Email"].ToString();
                        Session["UserId"] = dr["UserId"].ToString();

                        int TotMinutes = 1800;

                        int.TryParse(dr["TotMinutes"].ToString(), out TotMinutes);

                        //HttpCookie cookiename = new HttpCookie("cookiename");
                        //if (Request.Cookies["cookiename"] == null)
                        {
                            //Session["crnttime"]
                            Session["crnttime"] = Convert.ToString(TotMinutes * 60);

                            //cookiename.Value = "1200";
                            //cookiename.Expires = Dat eTime.Now.AddMinutes(20d);
                            //Response.Cookies.Add(cookiename);
                        }
                        Response.Redirect(@"Instructions.aspx");

                    }
                }
                else if (execStatus == 2)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "alert('Invalid username and password')", true);
                }
                else if (execStatus == 3)
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "alert('User is not in active mode')", true);
                }
            }
            if (execStatus == 0)
            {
                Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "alert('Invalid username and password')", true);
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}