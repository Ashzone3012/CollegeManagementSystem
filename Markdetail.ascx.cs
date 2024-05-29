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
    public partial class WebUserControl1 : System.Web.UI.UserControl
    {
           commonfnx fn = new commonfnx();
            protected void Page_Load(object sender, EventArgs e)
            {
                if (!IsPostBack)
                {
                    Getclass();
                    GetMarks();

                }
            }

            private void GetMarks()
            {
                DataTable dt = fn.Fetch(@"Select ROW_NUMBER() OVER(ORDER BY (Select 1)) as [Sr.No],e.ExamID,e.Class_ID,
                                        c.Course_Name,e.Course_ID,s.Course_Title,e.Enrollment_Number,e.total_marks,
                                        e.OutOf_marks From Exam e inner join class c on c.Class_ID =e.Class_ID
                                        inner join subject s on s.Course_ID= e.Course_ID ");
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }

            private void Getclass()
            {
                DataTable dt = fn.Fetch("Select * from class");
                ddlClass.DataSource = dt;
                ddlClass.DataTextField = "Course_Name";
                ddlClass.DataValueField = "Class_Id";
                ddlClass.DataBind();
                ddlClass.Items.Insert(0, "Select Class");
            }

            protected void btnAdd_Click(object sender, EventArgs e)
            {
                try
                {
                    string classId = ddlClass.SelectedValue;
                    string roll = txtRoll.Text.Trim();
                    DataTable dt = fn.Fetch(@"Select ROW_NUMBER() OVER(ORDER BY (Select 1)) as [Sr.No],e.ExamID,e.Class_ID,
                                        c.Course_Name,e.Course_ID,s.Course_Title,e.Enrollment_Number,e.total_marks,
                                        e.OutOf_marks From Exam e inner join class c on c.Class_ID =e.Class_ID
                                        inner join subject s on s.Course_ID= e.Course_ID where e.Class_ID='" + classId + "' and e.Enrollment_Number ='" + roll + "'");
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
            {
                GridView1.PageIndex = e.NewPageIndex;

            }
        }
    }
