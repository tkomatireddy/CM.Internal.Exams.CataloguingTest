using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CataloguingTest
{
    /// <summary>
    /// Summary description for CurrentTime
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]   
    public class CurrentTime : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod()]
        public void SetSession(string crnttime)
        {
            //if (HttpContext.Current.Session["RoleId"] != null && HttpContext.Current.Session["RoleId"].ToString() != "3")
            {
                HttpContext.Current.Session["crnttime"] = crnttime.ToString();
            }
            //else
            //{
            //    HttpContext.Current.Session["crnttime"] = "3600";
            //}
           
        }
    }
}
