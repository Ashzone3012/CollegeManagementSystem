using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CollegeManagement_System.Models.CommonFn;

namespace CollegeManagement_System
{
    public partial class login : System.Web.UI.Page
    {
        commonfnx fn = new commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnlogin_Click(object sender, EventArgs e)
        {
            string email = inputEmail.Value.Trim();
            string password = inputPassword.Value.Trim();
            DataTable dt = fn.Fetch("SELECT * FROM Faculty WHERE Email='"+email+"' AND Password='"+password+"'");

            if (email == "Admin" && password == "123")
            {
                Session["admin"] = email;
                Response.Redirect("Admin/AdminHome.aspx");
            }
            else if (dt.Rows.Count > 0)
            {
                Session["staff"] = email;
                Response.Redirect("Faculty/FacultyHome.aspx");
            }
            else
            {
                lblmasg.Text = "Login Failed!";
                lblmasg.ForeColor = System.Drawing.Color.Red;
            }
        }

    }
}