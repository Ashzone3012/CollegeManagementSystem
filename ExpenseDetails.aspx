<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="ExpenseDetails.aspx.cs" Inherits="CollegeManagement_System.Admin.ExpenseDetails" %>
<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.20/js/jquery.dataTables.min.js"></script>
<link href="https://cdn.datatables.net/1.10.20/css/jquery.dataTables.css" rel="stylesheet" type="text/css" />


    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=GridView1.ClientID%>').DataTable({
                "paging": true,
                "ordering": true,
                "searching": true
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style ="background-image: url('../Image/bg2.jpg');width:100%;height:720px;background-repeat:no-repeat;background-size:cover;background-attachment:fixed;">
        <div class ="container p-md-4 p-sm-4">
            <h3 class ="text-center">Add Expense Details</h3>
            <div class="row mb-3 mr-lg-5 ml-lg-5">
                <div class="col-md-12">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" EmptyDataText="No Record to Display!" 
                        AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="Sr.No" HeaderText="Sr.No">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Course_Name" HeaderText="Class">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Course_Title" HeaderText="Subject">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Charge_amount" HeaderText="Charge Amount(Per Lecture)">
                            <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        
                        </Columns>
                        <HeaderStyle BackColor="#5558C9" ForeColor="White" />
                    </asp:GridView>
                </div>
                </div>
           </div>
         </div>
</asp:Content>
