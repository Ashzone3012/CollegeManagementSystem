using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CollegeManagement_System.Models.CommonFn;

namespace CollegeManagement_System.Admin
{
    public partial class EmpAttendanceDetails : System.Web.UI.Page
    {
        commonfnx fn = new commonfnx();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["admin"] == null)
            {
                Response.Redirect("../login.aspx");
            }
            if (!IsPostBack)
            {
                GetTeacher();
            }
        }
        private void GetTeacher()
        {
            DataTable dt = fn.Fetch("Select * from Faculty");
            ddlTeacher.DataSource = dt;
            ddlTeacher.DataTextField = "Name";
            ddlTeacher.DataValueField = "Faculty_ID";
            ddlTeacher.DataBind();
            ddlTeacher.Items.Insert(0, "Select Faculty");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtmonths.Text);

            DataTable dt = fn.Fetch(@"Select Row_NUMBER() over(Order by(Select 1)) as [Sr.No],t.Name,ta.status,ta.Date from teacherAttendance ta
                                      inner join Faculty t on t.Faculty_ID= ta.Faculty_ID where DATEPART(yy,Date)='"+date.Year+"' And" +
                                      " DATEPART(M,Date)='"+ date.Month + "' and ta.status=1 and ta.Faculty_ID='"+ddlTeacher.SelectedValue+"'");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        
    }
}