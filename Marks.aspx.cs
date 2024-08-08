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
    public partial class Marks : System.Web.UI.Page
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
                GetClass();
                GetMarks();
            }
        }

        private void GetMarks()
        {
            DataTable dt = fn.Fetch(@"SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [Sr.No],e.ExamID,e.Class_ID,
                                      c.Course_Name,e.Course_ID,s.Course_Title,e.Enrollment_Number,e.total_marks,e.OutOf_marks FROM Exam e 
                                      INNER JOIN Class c ON e.Class_ID = c.Class_ID INNER JOIN Subject s ON e.Course_ID = s.Course_ID;");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        private void GetClass()
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
            try
            {
                string classId = ddlClass.SelectedValue;
                string subjectId = ddlSubject.SelectedValue;
                string roll = txtRoll.Text.Trim();
                string stumarks = txtStuMarks.Text.Trim();
                string outofmarks = txtoutofmarks.Text.Trim();
                DataTable dttbl = fn.Fetch("SELECT Enrollment_Number FROM Student WHERE Class_ID = '" + classId + "' AND addmin_no = '" + roll + "'");
                if (dttbl.Rows.Count > 0)
                {
                    DataTable dt = fn.Fetch("SELECT * FROM Exam WHERE Class_ID = '" + classId + "' AND Course_ID = '" + subjectId + "' AND Enrollment_Number = '" + roll + "'");
                    if (dt.Rows.Count == 0)
                    {
                        string query = "Insert into Exam Values('" + classId + "' ,'" + subjectId + "','" + roll + "','" + stumarks + "','" + outofmarks + "')";
                        fn.Query(query);
                        lblmsg.Text = "Inserted Succesffully!";
                        lblmsg.CssClass = "alert alert-success";
                        ddlClass.SelectedIndex = 0;
                        ddlSubject.SelectedIndex = 0;
                        txtRoll.Text = string.Empty;
                        txtStuMarks.Text = string.Empty;
                        txtoutofmarks.Text = string.Empty;
                        GetMarks();
                    }
                    else
                    {
                        lblmsg.Text = "Entered <b>Data</b> already exists!";
                        lblmsg.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    lblmsg.Text = "Entered Enrollment No <b>'"+roll+"'</b> does not exists for selected Class!";
                    lblmsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetMarks();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetMarks();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetMarks();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int examId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string Class_ID = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlClassGv")).SelectedValue;
                string sub_ID = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlSubjectGv")).SelectedValue;
                string Roll = (row.FindControl("txtrollGv") as TextBox).Text.Trim();
                string tolMar = (row.FindControl("txtStuMarksGv") as TextBox).Text.Trim();
                string OFM = (row.FindControl("txtOutOfMarksGv") as TextBox).Text.Trim();
                fn.Query(@"Update Exam set Class_ID='" + Class_ID + "',Course_ID ='" + sub_ID + "' ,Enrollment_Number ='" + Roll + "',total_marks ='"+tolMar+ "',OutOf_marks='"+OFM+"' where ExamID = '" + examId + "'");
                lblmsg.Text = "Record Updated Succesffully";
                lblmsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetMarks();

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlClass = (DropDownList)e.Row.FindControl("ddlClassGv");
                    DropDownList ddlSubject = (DropDownList)e.Row.FindControl("ddlSubjectGv");
                    DataTable dt = fn.Fetch("select * from Subject where Class_ID='" + ddlClass.SelectedValue + "'");
                    ddlSubject.DataSource = dt;
                    ddlSubject.DataTextField = "Course_Title";
                    ddlSubject.DataValueField = "Course_ID";
                    ddlSubject.DataBind();
                    ddlSubject.Items.Insert(0, "Select Subject");
                    string selectedSubject = DataBinder.Eval(e.Row.DataItem, "Course_Title").ToString();
                    ddlSubject.Items.FindByText(selectedSubject).Selected = true;
                    
                }
            }
        }

        protected void ddlClassGv_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlclassSelected = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlclassSelected.NamingContainer;
            if (row != null)
            {
                if ((row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlSubjectGV = (DropDownList)row.FindControl("ddlSubjectGv");
                    DataTable dt = fn.Fetch("select * from Subject where Class_ID='" + ddlclassSelected.SelectedValue + "'");
                    ddlSubjectGV.DataSource = dt;
                    ddlSubjectGV.DataTextField = "Course_Title";
                    ddlSubjectGV.DataValueField = "Course_ID";
                    ddlSubjectGV.DataBind();
                }
            }
        }
    }
}