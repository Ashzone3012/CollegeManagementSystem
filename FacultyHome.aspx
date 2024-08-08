<%@ Page Title="" Language="C#" MasterPageFile="~/Faculty/FacultyMst.Master" AutoEventWireup="true" CodeBehind="FacultyHome.aspx.cs" Inherits="CollegeManagement_System.Faculty.FacultyHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div style ="background-image: url('../Image/bg1.jpg');width:100%;height:720px;background-repeat:no-repeat;background-size:cover;background-attachment:fixed;">
        <div class ="container p-md-4 p-sm-4">
            <div>
                <asp:Label ID="lblmsg" runat="server"></asp:Label>
            </div>
            <h2 class ="text-center">Faculty Home Page </h2>
        </div>
    </div>
</asp:Content>
