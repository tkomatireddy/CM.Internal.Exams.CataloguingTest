<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarcTags.aspx.cs" Inherits="CataloguingTest.MarcTags" %>

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
                <label for="lblLoginName">Cataloguing Online Test: </label>

                <label for="lblUserName" style="margin-left: 20px; color: yellow">User Name:</label>
                <asp:Label ID="lblUserName" Text="User Name" runat="server" Style="font-size: large;"></asp:Label>
                 <asp:Button ID="btnTimeOut" runat="server" Text="Time Out" style="display:none" CssClass="btn btn-warning" OnClick="btnTimeOut_Click" />
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
                            <div class="col-md-8" style="margin-left: 10px;">
                                <asp:Label ID="lblInstractions" runat="server" CssClass="text-dark" Style="font-weight: bold;" Text="Category-1: Assign the appropriate MARC numbers to the Book Metadata Fields:[10 X 1 Marks = 10]"></asp:Label>
                            </div>

                            <div class="col-md-3" >

                                <asp:LinkButton  style="float: right;"  ID="lnkHome" runat="server" Text="Home" PostBackUrl="~/Models/Home.aspx" />
                            </div>
                        </div>
                    </div>
                    <div class="card-body">

                        <div class="col-md-offset-2 col-md-12 col-sm-12">
                            <div class="tree-spaced margin-top">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <asp:GridView ID="gvMarcTags" runat="server" CssClass="table table-striped table-hover text-center valign-middle"
                                            AutoGenerateColumns="false" OnRowDataBound="gvMarcTags_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="#">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="MarcId" HeaderText="MarcId" />
                                                <asp:BoundField DataField="Question" HeaderText="Book Metadata Field" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Answer" HeaderText="Correct Answer" />
                                                <asp:TemplateField HeaderText="MARC Values">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlTagValues" runat="server"></asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="UsrAnswer" HeaderText="Evaluate Answer" />
                                                <asp:TemplateField HeaderText="Select Correct Answer ">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkReselt" runat="server" Checked='<%# bool.Parse(Eval("Result").ToString()) %>'></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Comments">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtComments" runat="server" Text='<%# Eval("Comments") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="card-footer">
                        <div class="row text-left valign-middle">
                            <div class="col-md-12">
                                <asp:Button ID="btnMarcDone" Text="Save" runat="server" OnClick="btnMarcDone_Click" CssClass="btn btn-success" Visible="false" />
                                <asp:Button ID="btnMarcNext" Text="Next" runat="server" OnClick="btnMarcNext_Click" CssClass="btn btn-info" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
