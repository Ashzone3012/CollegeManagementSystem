using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CollegeManagement_System.Models.CommonFn;

namespace CollegeManagement_System.Faculty
{
    public partial class StudentAttendance : System.Web.UI.Page
    {
        commonfnx fn = new commonfnx();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["staff"] == null)
            {
                Response.Redirect("../login.aspx");
            }
            if (!IsPostBack)
            {
                Getclass();
                btnMarkAttendance.Visible = false;

            }
        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Lbltime.Text = DateTime.Now.ToString();
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
            DataTable dt = fn.Fetch(@"Select Enrollment_number, addmin_no,Name,Mobile from student  where Class_ID='"+ddlClass.SelectedValue+"'");
            GridView1.DataSource = dt;
            GridView1.DataBind();
            if (dt.Rows.Count>0)
            {
                btnMarkAttendance.Visible = true;
            }
            else
            {
                btnMarkAttendance.Visible = false;
            }
        }

        protected void btnMarkAttendance_Click(object sender, EventArgs e)
        {
            bool isTrue = false;
            foreach (GridViewRow row in GridView1.Rows)
            {
                string Roll = row.Cells[2].Text.Trim();
                RadioButton rb1 = (row.Cells[0].FindControl("RadioButton1") as RadioButton);
                RadioButton rb2 = (row.Cells[0].FindControl("RadioButton2") as RadioButton);
                int status = rb1.Checked ? 1 : 0; // Simplified status assignment
                fn.Query(@"Insert into studentAttendance values('" + ddlClass.SelectedValue + "','" + ddlSubject.SelectedValue + "', '" + Roll + "','" + DateTime.Now.ToString("yyyy/MM/dd") + "','" + status + "')");
                isTrue = true;
            }
            lblmsg.Text = isTrue ? "Inserted Successfully" : "Something went wrong!";
            lblmsg.CssClass = isTrue ? "alert alert-success" : "alert alert-warning";
        }
    }
}