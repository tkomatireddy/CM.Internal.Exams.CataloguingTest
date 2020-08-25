<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CataloguingTest.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="Styles/css/bootstrap.min.css" />
    <script src="Styles/css/jquery-3.4.1.slim.min.js"></script>
    <script src="Styles/css/popper.min.js"></script>
    <script src="Styles/css/bootstrap.min.js"></script>
    <link rel="stylesheet" href="Styles/css/exams.css" />
    <script type="text/javascript">
        function Validation() {
            document.getElementById("<% =lblErrorMessage.ClientID%>").innerHTML = '';
            var username = document.getElementById("<% =txtExaminerName.ClientID%>");
            if (username)
                if (username.value.trim() == '') {
                    document.getElementById("<% =lblErrorMessage.ClientID%>").innerHTML = '***Please enter username';
                    return false;
                }
            var password = document.getElementById("<% =txtExaminerPassword.ClientID%>");
            if (password)
                if (password.value.trim() == '') {
                    document.getElementById("<% =lblErrorMessage.ClientID%>").innerHTML = '***Please enter password';
                    return false;
                }

            return true;
        }
        $(document).ready(
            function () {
                document.getElementById("<% =txtExaminerName.ClientID%>").focus();
            }
        );
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scrpt" runat="server"></asp:ScriptManager>
        <div class="card text-white bg-info">
            <div class="card-header">
                <label for="lblUserName">NES Cataloguing Online Test </label>
            </div>
            <div class="card-body bg-light text-dark ">
                <div class="row" style="margin-top: 12px;">
                    <div class="col-md-3" style="margin-left: 20px;">
                    </div>
                    <div class="col-md-3 text-md-center">
                        <div class="card bg-light text-dark text-left">
                            <div class="card-header">
                                <label id="lblHeader">User Login </label>
                            </div>

                            <div class="card-body">
                                <div class="form-group">
                                    <label for="txtUserName">User Id</label>
                                    <asp:TextBox ID="txtExaminerName" runat="server" CssClass="form-control" placeholder="username"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="txtExaminerPassword">Password</label>
                                    <asp:TextBox ID="txtExaminerPassword" TextMode="Password" runat="server" CssClass="form-control" placeholder="password"></asp:TextBox>
                                </div>
                            </div>
                            <div class="card-footer">
                                <asp:Button ID="BtnLogin" runat="server" Text="Login" OnClientClick="return Validation(); " OnClick="BtnLogin_Click" CssClass="btn btn-primary" />
                                <asp:Button ID="BtnCancel" runat="server" Text="Cancel" OnClientClick="return ClearAll();" OnClick="BtnCancel_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                        <div id="divErrors">
                            <asp:Label ID="lblErrorMessage" runat="server" Style="color: red;"></asp:Label>
                        </div>
                    </div>
                    <div class="col-md-2">
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
