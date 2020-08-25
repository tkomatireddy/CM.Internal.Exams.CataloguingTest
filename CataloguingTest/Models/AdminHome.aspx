<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="CataloguingTest.AdminHome" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="../Styles/css/bootstrap.min.css" />
    <script src="../Styles/css/jquery-3.4.1.slim.min.js"></script>
    <script src="../Styles/css/popper.min.js"></script>
    <script src="../Styles/css/bootstrap.min.js"></script>
    <link rel="stylesheet" href="../Styles/css/exams.css" />
    <script src="../Styles/js/exams.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scrpt" runat="server"></asp:ScriptManager>
        <div class="card text-white bg-info">
            <div class="card-header">
                <label for="lblLoginName">Cataloguing Online Test </label>

                <label for="lblLoginName" style="color: black; font-size: large; margin-left: 20px">Login Name:</label>
                <asp:Label ID="lblLoginName" Text="Login Name" runat="server" Style="color: yellow; font-size: large;"></asp:Label>

                <asp:LinkButton ID="lblLogOut" Text="Logout" runat="server" Style="color: white; font-size: large; float: right; margin-left: 20px;" OnClick="lblLogOut_Click"></asp:LinkButton>
                <div style="float: right;">
                    <asp:HiddenField ID="hdfStartTime" runat="server" />
                    <asp:Label ID="lblTimer" Font-Bold="true" runat="server"></asp:Label>
                </div>
            </div>
            <div class="card-body bg-light text-dark ">
                <div class="card">
                    <div class="card-header">
                        <div class="row">
                            <div class="col-md-2" style="margin-left: 20px;">
                                <asp:Label ID="Label1" runat="server" Text="Label" Style="display: none"></asp:Label>
                            </div>
                            <div class="col-md-6">
                                <label for="txtFromDate">From:</label>
                                <asp:TextBox ID="txtFromDate" runat="server" Width="100px" onFocus="this.blur();" AutoCompleteType="Disabled"></asp:TextBox>

                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFromDate" Format="dd-MMM-yyyy" />
                                <label for="txtToDate">To:</label>
                                <asp:TextBox ID="txtToDate" runat="server" Width="100px" onFocus="this.blur();" AutoCompleteType="Disabled"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate" Format="dd-MMM-yyyy" />
                                <asp:Button runat="server" ID="btnSearch" Text="Search" CssClass="btn btn-info btn-md" OnClick="btnSearch_Click" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="return confirm('Are you sure want to close?');" CssClass="btn btn-warning" OnClick="btnClose_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:GridView ID="gvUserTestDtls" runat="server" CssClass="table table-striped table-hover text-center valign-middle"
                                    AutoGenerateColumns="false" OnRowDataBound="gvUserTestDtls_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="#">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="UserId" HeaderText="UserId" />
                                        <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                        <asp:BoundField DataField="TestDate" HeaderText="Test Date" />
                                        <asp:BoundField DataField="TestStatus" HeaderText="Test Status" />
                                        <asp:BoundField DataField="TimeTaken" HeaderText="Time Taken" />
                                        <asp:BoundField DataField="EvaluationDate" HeaderText="Evaluation Date" />
                                        <asp:BoundField DataField="EvaluatorStatus" HeaderText="Evaluator Status" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Total_Marks" HeaderText="Total Marks" />
                                        <asp:BoundField DataField="Obtained_Marks" HeaderText="Obtained Marks" />
                                        <asp:TemplateField HeaderText="Actions" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkMFSLoad" runat="server" OnClick="lnkGoToQuesctions_Click" CommandName='<%# Eval("UserName")%>' CommandArgument='<%# Eval("UserId")%>' Text="Go to Test Details" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
