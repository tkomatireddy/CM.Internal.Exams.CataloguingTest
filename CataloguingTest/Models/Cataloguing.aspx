<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cataloguing.aspx.cs" Inherits="CataloguingTest.Cataloguing" %>

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
    <script src="../Styles/js/jquery.cookie.js"></script>

    <script type="text/javascript">

        function countdown() {
            if (document.getElementById("hdfRoleId").value == "3") {
                time = document.getElementById("hdfTimer").value;
                if (time > 0) {
                    document.getElementById("hdfTimer").value = time - 1;
                    time = time - 1

                    if (time == 300) {
                        alert('You have 5 minuts left..!');
                    }

                    var minutes = Math.floor(time / 60);
                    var seconds = time - minutes * 60;
                    document.getElementById("lblTimer").innerHTML = minutes + " : " + seconds

                    CataloguingTest.CurrentTime.SetSession(time);

                    setTimeout("countdown()", 1000);
                }
                if (time == 0) {
                    alert('Your exam is timed-out and hence, submitted for evaluation. Thank you, we will get back to you!');
                    document.getElementById("<%=btnTimeOut.ClientID %>").click();
                }
            }
        }

        setTimeout("countdown()", 1000);

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scrpt" runat="server">
            <Services>
                <asp:ServiceReference Path="~/CurrentTime.asmx" />
            </Services>
        </asp:ScriptManager>
        <div class="card text-white bg-info">
            <div class="card-header">
                <label for="lblLoginName">Cataloguing Online Test:</label>
                <label for="lblUserName" style="margin-left: 20px; color: yellow">User Name:</label>
                <asp:Label ID="lblUserName" Text="User Name" runat="server" Style="font-size: large;"></asp:Label>

                <asp:Button ID="btnTimeOut" runat="server" Text="Time Out" Style="display: none" CssClass="btn btn-warning" OnClick="btnTimeOut_Click" />
                <asp:LinkButton ID="lblLogOut" Text="Logout" runat="server" Style="color: white; font-size: large; float: right; margin-left: 20px;" OnClick="lblLogOut_Click"></asp:LinkButton>
                <div style="float: right;">
                    <asp:HiddenField ID="hdfTimer" runat="server" />
                    <asp:HiddenField ID="hdfRoleId" runat="server" />
                    <asp:Label ID="lblTimer" Font-Bold="true" runat="server"></asp:Label>
                </div>
            </div>
            <div class="card-body bg-light text-dark ">
                <div class="card">
                    <div class="card-header">
                        <div class="row">
                            <div class="col-md-4" style="margin-left: 10px;">
                                <asp:Label ID="Label1" runat="server" CssClass="text-dark" Style="font-weight: bold;" Text="Category-4: Cataloguing.[1 X 5 Marks = 5]"></asp:Label>

                            </div>
                            <div class="col-md-3">
                                <asp:BulletedList ID="BulletedList1" runat="server" DisplayMode="LinkButton" Style="list-style-type: circle;" OnClick="BulletedList1_Click" Font-Bold="True" OnPreRender="BulletedList1_PreRender">
                                </asp:BulletedList>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hdfQn" runat="server" />
                                <asp:LinkButton ID="lnkHome" runat="server" Text="Home" PostBackUrl="~/Models/Home.aspx" Style="float: right" />
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="col-md-offset-2 col-md-8 col-sm-12">
                            <div class="tree-spaced margin-top">

                                <div class="row">
                                    <div class="col-xs-12" runat="server" id="divDtlView">
                                        <label for="lblCtlgQn" style="color: red">Q. </label>
                                        <asp:Label ID="lblCtlgQn" runat="server"> </asp:Label>
                                        <div id="divEval" runat="server" visible="false">
                                            <label for="lblCtlgAns" style="color: red">A. </label>
                                            <asp:Label ID="lblCtlgAns" runat="server"> </asp:Label><br />
                                            <label for="lblUsrAns" style="color: red">User A. </label>
                                            <asp:Label ID="lblUsrAns" runat="server"> </asp:Label><br />
                                            <label for="chkResult" style="color: red">Evaluater Marks.</label>
                                            <asp:CheckBox ID="chkResult" runat="server" />
                                        </div>
                                        <asp:RadioButtonList ID="rbtnCtlgOpts" runat="server">
                                        </asp:RadioButtonList>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="card-footer">
                        <div class="row text-left valign-middle">
                            <div class="col-md-12">
                                <asp:Button ID="btnCataloguingPrevious" Text="Previous" runat="server" OnClick="btnCataloguingPrevious_Click" CssClass="btn btn-info" />
                                <asp:Button ID="btnCataloguingDone" Text="Submit" runat="server" OnClick="btnCataloguingDone_Click" CssClass="btn btn-info" OnClientClick="return confirm('Are you sure to submit your responses  to evaluator?');" />
                                <asp:Button ID="btnCataloguingNext" Text="Next" runat="server" OnClick="btnCataloguingNext_Click" CssClass="btn btn-info" />
                            </div>
                        </div>
                    </div>


                </div>
            </div>
        </div>

    </form>
</body>
</html>
