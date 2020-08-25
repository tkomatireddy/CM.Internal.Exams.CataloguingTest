<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Citations.aspx.cs" Inherits="CataloguingTest.Citations" %>

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

                            <div class="col-md-6">
                                <asp:BulletedList ID="BulletedList1" runat="server" DisplayMode="LinkButton" Style="list-style-type: circle;" OnClick="BulletedList1_Click" Font-Bold="True" OnPreRender="BulletedList1_PreRender">
                                </asp:BulletedList>
                            </div>
                            <div class="col-md-6">
                                <asp:HiddenField ID="hdfQn" runat="server" />
                                <asp:LinkButton ID="lnkHome" Style="float: right;" runat="server" Text="Home" PostBackUrl="~/Models/Home.aspx" />
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="col-md-offset-2 col-md-12 col-sm-12">
                            <%--<div class="tree-spaced margin-top">--%>
                            <div class="row">
                                <asp:Label ID="lblInstractions" runat="server" CssClass="text-dark" Style="font-weight: bold;" Text="Category-2: Read the question and capture the book related information in the provided fields.[3 X 5 Marks = 15]"></asp:Label>
                            </div>
                            <div class="row">
                                <div class="col-xs-12" runat="server" id="divDtlView">
                                    <asp:DetailsView ID="dvCitation" runat="server" Height="100px" Width="100%" AutoGenerateRows="false" CssClass="table table-striped table-hover  valign-middle">
                                        <Fields>
                                            <asp:BoundField DataField="CitationId" HeaderText="CitationId" />
                                            <asp:BoundField DataField="Citation" HeaderText="Citation" />
                                            <asp:TemplateField HeaderText="Authors">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAuthors" runat="server" Visible="false" Text='<%# Eval("Authors") %>' />
                                                    <asp:TextBox ID="txtAuthors" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="BookTitle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookTitle" runat="server" Visible="false" Text='<%# Eval("BookTitle") %>' />
                                                    <asp:TextBox ID="txtBookTitle" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="BookChapter">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookChapter" runat="server" Visible="false" Text='<%# Eval("BookChapter") %>' />
                                                    <asp:TextBox ID="txtBookChapter" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="SeriesTitle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSeriesTitle" runat="server" Visible="false" Text='<%# Eval("SeriesTitle") %>' />
                                                    <asp:TextBox ID="txtSeriesTitle" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ISBN">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblISBN" runat="server" Visible="false" Text='<%# Eval("ISBN") %>' />
                                                    <asp:TextBox ID="txtISBN" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Editors">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEditors" runat="server" Visible="false" Text='<%# Eval("Editors") %>' />
                                                    <asp:TextBox ID="txtEditors" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Publisher">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPublisher" runat="server" Visible="false" Text='<%# Eval("Publisher") %>' />
                                                    <asp:TextBox ID="txtPublisher" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PlaceOfPublication">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPlaceOfPublication" runat="server" Visible="false" Text='<%# Eval("PlaceOfPublication") %>' />
                                                    <asp:TextBox ID="txtPlaceOfPublication" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="StartPage">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStartPage" runat="server" Visible="false" Text='<%# Eval("StartPage") %>' />
                                                    <asp:TextBox ID="txtStartPage" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="EndPage">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEndPage" runat="server" Visible="false" Text='<%# Eval("EndPage") %>' />
                                                    <asp:TextBox ID="txtEndPage" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PgCount">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPgCount" runat="server" Visible="false" Text='<%# Eval("PgCount") %>' />
                                                    <asp:TextBox ID="txtPgCount" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="YearOfPublication">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYearOfPublication" runat="server" Visible="false" Text='<%# Eval("YearOfPublication") %>' />
                                                    <asp:TextBox ID="txtYearOfPublication" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PublicationType">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPublicationType" runat="server" Visible="false" Text='<%# Eval("PublicationType") %>' />
                                                    <asp:TextBox ID="txtPublicationType" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comments">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtComments" runat="server" Width="100%" CssClass="form-control" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Marks">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtResult" runat="server" Width="60px" ClientIDMode="Static" />
                                                    <asp:Label ID="lblMaxMarks" runat="server" Text=" / 5" />

                                                    <asp:RegularExpressionValidator ID="Regex1" runat="server" ValidationExpression="(([0-4]+)((\.[0,5])?)|[5])$"
                                                        ErrorMessage="Please enter valid integer or decimal number between 0 to 5 EX: 2/3/4.5 ect."
                                                        ControlToValidate="txtResult" ForeColor="Red" />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Fields>
                                        <FieldHeaderStyle CssClass="card-header" />
                                    </asp:DetailsView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-footer">

                        <div class="row text-left valign-middle">
                            <div class="col-md-12">
                                <asp:Button ID="btnCitationsPrevious" Text="Previous" runat="server" OnClick="btnCitationsPrevious_Click" CssClass="btn btn-info" />
                                <asp:Button ID="btnCitationsDone" Text="Save" runat="server" OnClick="btnCitationsDone_Click" CssClass="btn btn-success" Visible="false" />
                                <asp:Button ID="btnCitationsNext" Text="Next" runat="server" OnClick="btnCitationsNext_Click" CssClass="btn btn-info" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


    </form>
</body>
</html>
