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
    public partial class StudentAttendanceUC : System.Web.UI.UserControl
    {
        commonfnx fn = new commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
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

            if(ddlSubject.SelectedValue=="Select Subject")
            {
                dt = fn.Fetch(@"Select Row_NUMBER() over(Order by(Select 1)) as [Sr.No],t.Name,ta.status,ta.Date from studentAttendance ta
                                      inner join Student t on t.addmin_no= ta.addmin_no where ta.Class_ID='" + ddlClass.SelectedValue+"' And addmin_no='"+txtroll.Text.Trim()+ "' DATEPART(yy,Date)='" + date.Year + "'" +
                                     "and  DATEPART(M,Date)='" + date.Month + "' and ta.status=1 ");
            }
            else
            {
                dt = fn.Fetch(@"Select Row_NUMBER() over(Order by(Select 1)) as [Sr.No],t.Name,ta.status,ta.Date from studentAttendance ta
                                      inner join Student t on t.addmin_no= ta.addmin_no where ta.Class_ID='" + ddlClass.SelectedValue + "' And addmin_no='" + txtroll.Text.Trim() + "' and Course_ID='"+ddlSubject.SelectedValue+"'and DATEPART(yy,Date)='" + date.Year + "'" +
                                     "and  DATEPART(M,Date)='" + date.Month + "' and ta.status=1 ");
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}