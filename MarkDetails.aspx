<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMst.Master" AutoEventWireup="true" CodeBehind="MarkDetails.aspx.cs" Inherits="CollegeManagement_System.Admin.MarkDetails" %>
<%@ Register Src="~/Markdetail.ascx" TagPrefix="uc" TagName="Markdetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc:Markdetail runat="server" ID="Markdetail"/>
</asp:Content>
