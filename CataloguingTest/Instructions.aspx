<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Instructions.aspx.cs" Inherits="NES.Instructions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="Styles/css/bootstrap.min.css" />
    <script src="Styles/css/jquery-3.4.1.slim.min.js"></script>
    <script src="Styles/css/popper.min.js"></script>
    <script src="Styles/css/bootstrap.min.js"></script>
    <link rel="stylesheet" href="Styles/css/exams.css" />
    <script src="Styles/js/exams.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scrpt" runat="server"></asp:ScriptManager>
        <div class="card text-white bg-info">
            <div class="card-header">
                <label for="lblLoginName">Cataloguing Online Test </label>

                <label for="lblLoginName" style="color: black; font-size: large; margin-left: 20px">Login Name:</label>
                <asp:Label ID="lblLoginName" Text="Login Name" runat="server" Style="color: yellow; font-size: large;"></asp:Label>
                <div style="float: right;">
                    <asp:HiddenField ID="hdfStartTime" runat="server" />
                    <asp:Label ID="lblTimer" Font-Bold="true" runat="server"></asp:Label>
                </div>
            </div>
            <div class="card-body bg-light text-dark ">
                <div class="card">
                    <div class="card-header font-weight-bold">
                        Instructions
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-12" style="margin-left: 10px;">
                                1. The test consist of questions and answers type.<br />
                                2. One question is displayed with answers needed to be entered.<br />
                                3. Click on question numbers to navigate to particular question number.<br />
                                4. Click on previous/next question for navigation.<br />
                                5. There is no negative marking.<br />
                                6. Click on submit to complete the test.
                            </div>
                        </div>
                    </div>

                    <div class="card-footer text-muted">
                        <asp:Button ID="btnStartTest" runat="server" Text="Start Test" class="btn btn-primary" OnClick="btnStartTest_Click" />
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
