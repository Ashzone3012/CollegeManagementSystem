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
    public partial class WebForm1 : System.Web.UI.Page
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
            DataTable dt = fn.Fetch("Select Row_NUMBER() over(Order by(Select 1)) as [Sr.No], Class_Id,Course_Name from Class");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = fn.Fetch("Select * from Class where Course_Name= '"+txtClass.Text.Trim()+"' ");
                if (dt.Rows.Count==0)
                {
                    string query = "Insert into Class Values('" + txtClass.Text.Trim() + "')";
                    fn.Query(query);
                    lblmsg.Text = "Inserted Succesffully!";
                    lblmsg.CssClass = "alert alert-success";
                    txtClass.Text = string.Empty;
                    Getclass();
                }
                else
                {
                    lblmsg.Text = "Entered Class already exists";
                    lblmsg.CssClass = "alert alert-danger";
                }
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Getclass();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            Getclass();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
                try
                {
                    GridViewRow row = GridView1.Rows[e.RowIndex];
                    int cId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                    string ClassName = (row.FindControl("txtclassEdit") as TextBox).Text;
                    fn.Query("Update class set Course_Name = '" + ClassName + "'where Class_ID = '" + cId + "' ");
                    lblmsg.Text = "Class Updated Succesffully";
                    lblmsg.CssClass = "alert alert-success";
                    GridView1.EditIndex = -1;
                    Getclass();
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
        }
    }
}