using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NES
{
    public partial class Instructions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserType"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {
                    lblLoginName.Text = Session["UserName"].ToString();
                }
            }           
        }

        protected void btnStartTest_Click(object sender, EventArgs e)
        {
            Response.Redirect(@"Models/Home.aspx");
        }
    }
}