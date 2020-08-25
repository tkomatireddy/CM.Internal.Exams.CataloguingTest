using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CataloguingTest
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string userName = string.Empty;
            string passWord = string.Empty;
            userName = txtExaminerName.Text;
            passWord = txtExaminerPassword.Text;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
            {
                AdminEvaluator_Check(userName, passWord);
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

        protected void BtnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}