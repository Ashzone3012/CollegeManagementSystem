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
    public partial class Student : System.Web.UI.Page
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
                GetStudent();
            }
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlGender.SelectedValue != "0")
                {
                    string AddminNo = txtAddmin.Text.Trim();
                    DataTable dt = fn.Fetch("Select * from student where Class_ID='"+ddlClass.SelectedValue+ "' and addmin_no='" + AddminNo + "'");
                    if (dt.Rows.Count == 0)
                    {
                        string query = "Insert into student Values('" + txtName.Text.Trim() + "','" + txtBOD.Text.Trim() + "','" +
                                        ddlGender.SelectedValue + "','" + txtMobile.Text.Trim() + "','" +
                                        txtAddress.Text.Trim() + "','" + ddlClass.SelectedValue + "','" + txtAddmin.Text.Trim() + "')";

                        fn.Query(query);
                        lblmsg.Text = "Inserted Succesffully!";
                        lblmsg.CssClass = "alert alert-success";
                        ddlGender.SelectedIndex = 0;
                        txtName.Text = string.Empty;
                        txtBOD.Text = string.Empty;
                        txtMobile.Text = string.Empty;
                        txtAddmin.Text = string.Empty;
                        txtAddress.Text = string.Empty;
                        ddlClass.SelectedIndex = 0;
                        GetStudent();
                    }
                    else
                    {
                        lblmsg.Text = "Entered  <b>'" + AddminNo + "'</b> already exists!";
                        lblmsg.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    lblmsg.Text = "Gender is required!";
                    lblmsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        private void GetStudent()
        {
            DataTable dt = fn.Fetch(@"Select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) as [Sr.No],
                           s.Enrollment_Number,s.[Name],s.DOB,s.Gender,s.Mobile,s.addmin_no,s.[Address],c.Class_ID,c.Course_Name
                            from student s inner join class c on c.Class_ID= s.Class_ID ");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetStudent();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetStudent();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetStudent();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int Enroll = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string name = (row.FindControl("txtName") as TextBox).Text;
                string mobile = (row.FindControl("txtMobile") as TextBox).Text;
                string roll = (row.FindControl("txtAddmin") as TextBox).Text;
                string address = (row.FindControl("txtAddress") as TextBox).Text;
                string ClassId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[4].FindControl("ddlClass")).SelectedValue;
                fn.Query("Update Student set Name='" + name.Trim() + "',Mobile ='" + mobile.Trim() + "',Address='" + address.Trim() + "',addmin_no='"+roll.Trim()+"',Class_ID ='"+ClassId+"' where Enrollment_Number = '" + Enroll + "'");
                lblmsg.Text = "Student Updated Succesffully";
                lblmsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetStudent();

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && GridView1.EditIndex==e.Row.RowIndex)
            { 
               DropDownList ddlClass = (DropDownList)e.Row.FindControl("ddlClass");
               DataTable dt = fn.Fetch("select * from Class");
               ddlClass.DataSource = dt;
               ddlClass.DataTextField = "Course_Name";
               ddlClass.DataValueField = "Class_ID";
               ddlClass.DataBind();
               ddlClass.Items.Insert(0, "Select Class");
               string selectedclass = DataBinder.Eval(e.Row.DataItem, "Course_Name").ToString();
                ddlClass.Items.FindByText(selectedclass).Selected = true;
            }
        }
    }
}