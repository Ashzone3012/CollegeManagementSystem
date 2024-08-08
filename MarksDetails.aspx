<%@ Page Title="" Language="C#" MasterPageFile="~/Faculty/FacultyMst.Master" AutoEventWireup="true" CodeBehind="MarksDetails.aspx.cs" Inherits="CollegeManagement_System.Faculty.MarksDetails" %>
<%@ Register Src="~/Markdetail.ascx" TagPrefix="uc" TagName="Marksdetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc:Marksdetail runat="server" ID="Marksdetail"/>
</asp:Content>
