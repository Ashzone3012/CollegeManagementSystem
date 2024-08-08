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
    public partial class AttendanceDetails : System.Web.UI.Page
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
                Getclass();

            }
        }
        private void Getclass()
        {
            DataTable dt = fn.Fetch("Select * from class");
            ddlClass.DataSource = dt;
            ddlClass.DataTextField = "Course_Name";
            ddlClass.DataValueField = "Class_ID";
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, "Select Class");
        }
        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            string classId = ddlClass.SelectedValue;
            DataTable dt = fn.Fetch("Select * from Subject where Class_ID= '" + classId + "' ");
            ddlSubject.DataSource = dt;
            ddlSubject.DataTextField = "Course_Title";
            ddlSubject.DataValueField = "Course_ID";
            ddlSubject.DataBind();
            ddlSubject.Items.Insert(0, "Select Subject");

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt;
            DateTime date = Convert.ToDateTime(txtMonth.Text);

            if (ddlSubject.SelectedValue == "Select Subject")
            {
                dt = fn.Fetch(@"Select Row_NUMBER() over(Order by(Select 1)) as [Sr.No], s.Name, sa.status, sa.Date 
               from studentAttendance sa inner join Student s on s.addmin_no = sa.addmin_no where sa.Class_ID='" + ddlClass.SelectedValue + "' And sa.addmin_no = '" + txtroll.Text.Trim() + "' and DATEPART(yy, Date) = '" + date.Year + "'" + "and DATEPART(M,Date)='" + date.Month + "' and sa.status = 1 ");
            }
            else
            {
                dt = fn.Fetch(@"Select Row_NUMBER() over(Order by(Select 1)) as [Sr.No],s.Name,sa.status,sa.Date from studentAttendance sa
                                      inner join Student s on s.addmin_no= sa.addmin_no where sa.Class_ID='" + ddlClass.SelectedValue + "' And sa.addmin_no='" + txtroll.Text.Trim() + "' and sa.Course_ID='" + ddlSubject.SelectedValue + "'and DATEPART(yy,Date)='" + date.Year + "'" +
                                     "and  DATEPART(M,Date)='" + date.Month + "' and sa.status=1 ");
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}