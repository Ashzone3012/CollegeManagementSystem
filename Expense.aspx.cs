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
    public partial class Expense : System.Web.UI.Page
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
                GetExpense();

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
        private void GetExpense()
        {
            DataTable dt = fn.Fetch(@"Select Row_NUMBER() over(Order by(Select 1)) as [Sr.No] ,
                                    e.Expense_Id ,e.Class_ID,c.Course_Name,e.Course_ID,s.Course_Title,
                                    e.Charge_amount from Expense e inner join class c on e.Class_ID=c.Class_ID 
                                    inner join Subject s on e.Course_ID=s.Course_ID ");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string classId = ddlClass.SelectedValue;
                string subjectId = ddlSubject.SelectedValue;
                string chargeAmt = txtExpenseAmt.Text.Trim();
                DataTable dt = fn.Fetch("SELECT * FROM Expense WHERE Class_ID = '" + classId + "' AND Course_ID = '" + subjectId + "' OR Charge_amount = '" + chargeAmt + "'");
                if (dt.Rows.Count == 0)
                {
                    string query = "Insert into Expense Values('" + classId + "' ,'" + subjectId + "','" + chargeAmt + "')";
                    fn.Query(query);
                    lblmsg.Text = "Inserted Succesffully!";
                    lblmsg.CssClass = "alert alert-success";
                    ddlClass.SelectedIndex = 0;
                    ddlSubject.SelectedIndex = 0;
                    txtExpenseAmt.Text = string.Empty;
                    GetExpense();
                }
                else
                {
                    lblmsg.Text = "Entered <b>Data</b> already exists!";
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
            GetExpense();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetExpense();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int expId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                fn.Query("Delete From Expense where Expense_Id = '" + expId + "'");
                lblmsg.Text = "Data  Deleted Succesffully";
                lblmsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetExpense();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GetExpense();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = GridView1.Rows[e.RowIndex];
                int expId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
                string Class_ID = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlClassGv")).SelectedValue;
                string sub_ID = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[2].FindControl("ddlSubjectGv")).SelectedValue;
                string chargeAmt = (row.FindControl("txtExpenseAmt") as TextBox).Text.Trim();
                fn.Query(@"Update Expense set Class_ID='" + Class_ID + "',Course_ID ='" + sub_ID + "' ,Charge_Amount ='" + chargeAmt + "' where Expense_Id = '" + expId + "'");
                lblmsg.Text = "Record Updated Succesffully";
                lblmsg.CssClass = "alert alert-success";
                GridView1.EditIndex = -1;
                GetExpense();

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
                    string expId = GridView1.DataKeys[e.Row.RowIndex].Value.ToString();
                    DataTable dataTable = fn.Fetch(@"Select e.Expense_Id ,e.Class_ID,e.Course_ID,s.Course_Title 
                                                    from Expense e inner join Subject s on e.Course_ID=s.Course_ID where e.Expense_Id='" + expId + "'");
                    ddlSubject.SelectedValue = dataTable.Rows[0]["Course_ID"].ToString();
                }
            }
        }
    }
}