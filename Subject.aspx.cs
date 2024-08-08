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
    public partial class Subject : System.Web.UI.Page
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
                GetSubject();

            }
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
                string classVal = ddlClass.SelectedItem.Text;
                DataTable dt = fn.Fetch("Select * from Subject where Class_ID= '" + ddlClass.SelectedItem.Value +
                                        "' AND Course_Title = '"+txtSubject.Text.Trim()+"'");
                if (dt.Rows.Count == 0)
                {
                    string query = "Insert into Subject Values('" + ddlClass.SelectedItem.Value + "' ,'" + txtSubject.Text.Trim() + "')";
                    fn.Query(query);
                    lblmsg.Text = "Inserted Succesffully!";
                    lblmsg.CssClass = "alert alert-success";
                    ddlClass.SelectedIndex = 0;
                    txtSubject.Text = string.Empty;
                    GetSubject();
                }
                else
                {
                    lblmsg.Text = "Entered Subject already exists for <b>'" + classVal + "'</b>!";
                    lblmsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        private void GetSubject()
        {
            DataTable dt = fn.Fetch(@"Select Row_NUMBER() over(Order by(Select 1)) as [Sr.No],s.Course_ID,s.Class_ID,c.Course_Name,
                                    s.Course_Title from Subject s inner join class c on c.Class_Id = s.Class_Id");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GetSubject();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetSubject();
        }

       

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetSubject();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int SubId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string Class_ID = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("DropDownList1")).SelectedValue;
                string SubName = (row.FindControl("TextBox1") as TextBox).Text;
                fn.Query("Update Subject set Class_ID='" + Class_ID + "',Course_Title ='"+SubName+ "' where Course_ID = '" + SubId + "'");
                lblmsg.Text = "Subject Updated Succesffully";
                lblmsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetSubject();

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }

}
