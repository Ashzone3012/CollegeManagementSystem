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
    public partial class TeacherSubject : System.Web.UI.Page
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
                GetTeacher();
                GetTeacherSubject();
            
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
        private void GetTeacher()
        {
            DataTable dt = fn.Fetch("Select * from Faculty");
            ddlTeacher.DataSource = dt;
            ddlTeacher.DataTextField = "Name";
            ddlTeacher.DataValueField = "Faculty_ID";
            ddlTeacher.DataBind();
            ddlTeacher.Items.Insert(0, "Select Faculty");
        }

        private void GetTeacherSubject()
        {
            DataTable dt = fn.Fetch(@"Select Row_NUMBER() over(Order by(Select 1)) as [Sr.No] ,
                                    ts.ID ,ts.Class_ID,c.Course_Name,ts.Course_ID,s.Course_Title,ts.Faculty_ID,
                                    t.Name from facultysubject ts inner join class c on ts.Class_ID=c.Class_ID 
                                    inner join Subject s on ts.Course_ID=s.Course_ID inner join Faculty t on ts.Faculty_ID=t.Faculty_ID");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }


        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            string classId = ddlClass.SelectedValue;
            DataTable dt = fn.Fetch("Select * from Subject where Class_ID= '"+classId+"' ");
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
                string facultyId = ddlTeacher.SelectedValue;
                DataTable dt = fn.Fetch("SELECT * FROM facultysubject WHERE Class_ID = '" + classId + "' AND Course_ID = '" + subjectId + "' AND Faculty_ID = '" + facultyId + "'");
                if (dt.Rows.Count == 0)
                {
                    string query = "Insert into facultysubject Values('" + classId+ "' ,'" +subjectId + "','"+facultyId+"')";
                    fn.Query(query);
                    lblmsg.Text = "Inserted Succesffully!";
                    lblmsg.CssClass = "alert alert-success";
                    ddlClass.SelectedIndex = 0;
                    ddlSubject.SelectedIndex = 0;
                    ddlTeacher.SelectedIndex = 0;
                    GetTeacherSubject();
                }
                else
                {
                    lblmsg.Text = "Entered <b>Faculty Subject</b> already exists!";
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
            GetTeacherSubject();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetTeacherSubject();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int TeacherSubjectID = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                fn.Query("Delete From facultysubject where ID = '" + TeacherSubjectID + "'");
                lblmsg.Text = "Faculty Subject Deleted Succesffully";
                lblmsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetTeacherSubject();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetTeacherSubject();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int TeacherSubjectID = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string Class_ID = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlClassGv")).SelectedValue;
                string sub_ID = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlSubjectGv")).SelectedValue;
                string fac_ID = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlTeacherGv")).SelectedValue;
                fn.Query(@"Update facultysubject set Class_ID='" + Class_ID + "',Course_ID ='" + sub_ID + "' ," +
                          "Faculty_ID='"+fac_ID+"' where ID = '" + TeacherSubjectID + "'");
                lblmsg.Text = "Record Updated Succesffully";
                lblmsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetTeacherSubject();

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void ddlClassGv_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlclassSelected = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlclassSelected.NamingContainer;
            if(row != null)
            {
                if((row.RowState & DataControlRowState.Edit) > 0)
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                if((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    DropDownList ddlClass = (DropDownList)e.Row.FindControl("ddlClassGv");
                    DropDownList ddlSubject = (DropDownList)e.Row.FindControl("ddlSubjectGv");
                    DataTable dt = fn.Fetch("select * from Subject where Class_ID='" + ddlClass.SelectedValue + "'");
                    ddlSubject.DataSource = dt;
                    ddlSubject.DataTextField = "Course_Title";
                    ddlSubject.DataValueField = "Course_ID";
                    ddlSubject.DataBind();
                    ddlSubject.Items.Insert(0, "Select Subject");
                    string teacherSubjectID = GridView1.DataKeys[e.Row.RowIndex].Value.ToString();
                    DataTable dataTable = fn.Fetch(@"Select  ts.ID ,ts.Class_ID,ts.Course_ID,s.Course_Title from facultysubject ts 
                                                     inner join Subject s on ts.Course_ID=s.Course_ID where ts.ID='"+ teacherSubjectID + "'");
                    ddlSubject.SelectedValue = dataTable.Rows[0]["Course_ID"].ToString();
                }
            }
        }
    }
}