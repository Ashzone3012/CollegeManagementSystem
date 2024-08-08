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
	public partial class ExpenseDetails : System.Web.UI.Page
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
				GetExpenseDetails();
            }
		}

        private void GetExpenseDetails()
        {
            DataTable dt = fn.Fetch(@"Select Row_NUMBER() over(Order by(Select 1)) as [Sr.No] ,
                                    e.Expense_Id ,e.Class_ID,c.Course_Name,e.Course_ID,s.Course_Title,
                                    e.Charge_amount from Expense e inner join class c on e.Class_ID=c.Class_ID 
                                    inner join Subject s on e.Course_ID=s.Course_ID ");
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}