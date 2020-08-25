<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="CataloguingTest.Home" %>

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
                    var minutes = Math.floor(time / 60);
                    var seconds = time - minutes * 60;
                    document.getElementById("lblTimer").innerHTML = minutes + " : " + seconds

                    CataloguingTest.CurrentTime.SetSession(time);
                    setTimeout("countdown()", 1000);
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
                            <div class="col-md-2" style="margin-left: 20px;">
                                <asp:Label ID="Label1" runat="server" Text="Label" Style="display: none"></asp:Label>
                            </div>
                            <div class="col-md-3">
                            </div>
                            <div class="col-md-2">
                                <asp:LinkButton ID="lnkHome" Style="float: right;" runat="server" Text="Admin Home" PostBackUrl="~/Models/AdminHome.aspx" />
                                <asp:Button ID="btnFinish" runat="server" Text="Finished" OnClientClick="return confirm('Are you sure finished the test?');" CssClass="btn btn-warning" OnClick="btnFinish_Click" Visible="false" />
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="col-md-offset-2 col-md-8 col-sm-12">
                            <div class="tree-spaced margin-top">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <asp:GridView ID="gvIndex" runat="server" CssClass="table table-striped table-hover text-center valign-middle"
                                            AutoGenerateColumns="false" OnRowDataBound="gvIndex_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="#">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="QIId" HeaderText="QIId" />
                                                <asp:BoundField DataField="Question_Desc" HeaderText="Question Description" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="NoOfQuestions" HeaderText="Questions" />
                                                <asp:BoundField DataField="MarksPerQuestion" HeaderText="Marks Per Question" DataFormatString="{0:N0}" />
                                                <asp:TemplateField HeaderText="Actions" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkMFSLoad" runat="server" OnClick="lnkGoToQuesctions_Click" CommandName='<%# Eval("QIId")%>' CommandArgument='<%# Eval("QIId")%>' Text="Go to Questions" />
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
            </div>
        </div>

    </form>
</body>
</html>
