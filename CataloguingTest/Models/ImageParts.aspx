<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageParts.aspx.cs" Inherits="CataloguingTest.ImageParts" %>

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
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://cdn.rawgit.com/elevateweb/elevatezoom/master/jquery.elevateZoom-3.0.8.min.js"></script>
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
    <style>
        .row {
            display: -ms-flexbox;
            display: flex;
            -ms-flex-wrap: wrap;
            flex-wrap: wrap;
            margin-right: 0px;
            margin-left: 0px;
        }
    </style>
    <script lang="javascript" type="text/javascript">

        var _lastCheckedRadio = new Array();
        $(document).ready(function () {
            $('input[type="radio"]').click(function () {
                var indexx = SearchIndex(this.id.substring(0, this.id.indexOf("_")));
                if (indexx > -1) {
                    if (this.id == _lastCheckedRadio[indexx]) {
                        $(this).attr('checked', false);
                        _lastCheckedRadio.splice(indexx, 1);
                        this.checked = false;
                    }
                    else {
                        _lastCheckedRadio.splice(indexx, 1);
                        _lastCheckedRadio.push(this.id);
                    }
                }
                else {
                    _lastCheckedRadio.push(this.id);
                    $(this).attr('checked', true);
                }
            });
        });

        function SearchIndex(Target) {
            var index = -1;
            for (i = 0; i < _lastCheckedRadio.length; i++) {
                if (_lastCheckedRadio[i].search(Target) > -1) {
                    index = i;
                    break;
                }
            }
            return index;
        }

        $(document).ready(function () {
            $('#tblImages img').width(400);
            $('#tblImages img').mouseover(function () {
                $(this).css("cursor", "pointer");
                $(this).animate({ width: "800px" }, 'slow');
            });
            $('#tblImages img').mouseout(function () {
                $(this).animate({ width: "400px" }, 'slow');
            });
        });  


        //$(function () {
        //    $("#1").elevateZoom({
        //        cursor: 'pointer',
        //        imageCrossfade: true,
        //        loadingIcon: 'loading.gif' });
        //});
        //$("#1").bind("click", function (e) {
        //    var ez = $('#1).data('elevateZoom');  
        //        return false;
        //});

        //$("#1").click(function () {
        //    $(this).zoomTo({
        //        targetsize: 1.0,
        //        root: $(".container")
        //    });
        //});
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
                            <div class="col-md-4" style="margin-left: 10px;">
                                <asp:Label ID="Label1" runat="server" CssClass="text-dark" Style="font-weight: bold;" Text="Category-3: Parts of the Book.[4 X 5 Marks = 20]"></asp:Label>
                            </div>
                            <div class="col-md-3">

                                <asp:BulletedList ID="BulletedList1" runat="server" DisplayMode="LinkButton" Style="list-style-type: circle;" OnClick="BulletedList1_Click" Font-Bold="True" OnPreRender="BulletedList1_PreRender">
                                </asp:BulletedList>
                            </div>
                            <div class="col-md-4">
                                <asp:HiddenField ID="hdfQn" runat="server" />
                                <asp:LinkButton ID="lnkHome" runat="server" Text="Home" PostBackUrl="~/Models/Home.aspx" style="float:right"/>
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
                                        <div class="row">
                                            <div class="col-xs-8">
                                                <asp:Table ID="tblImages" runat="server" CssClass="table table-striped table-hover text-center valign-middle" EnableViewState="true">
                                                </asp:Table>
                                            </div>
                                            <div class="col-xs-4">
                                                <asp:GridView ID="gvImagesAns" runat="server" CssClass="table table-striped table-hover text-left valign-middle"
                                                    AutoGenerateColumns="false" Width="600px" OnRowDataBound="gvImagesAns_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="#">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ImgAnsId" HeaderText="ImgAnsId" />
                                                        <asp:BoundField DataField="ImgQuestion" HeaderText="Metadata Field" />
                                                        <asp:TemplateField HeaderText="User Captured Text">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtImgAns" runat="server" Style="width: 400px" CssClass="form-control"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="ImgAnswer" HeaderText="DB Answer" />
                                                        <asp:BoundField DataField="UsrAnswer" HeaderText="User Answer" />
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:RadioButtonList ID="rbtnImgOpts" runat="server" CssClass="table table-bordered text-center valign-middle">
                                                </asp:RadioButtonList>
                                                <div id="divEval" runat="server" visible="false">
                                                    <label for="lblImgAns" style="color: red">A. </label>
                                                    <asp:Label ID="lblImgAns" runat="server"> </asp:Label><br />
                                                    <label for="lblUsrAns" style="color: red">User A. </label>
                                                    <asp:Label ID="lblUsrAns" runat="server"> </asp:Label>
                                                </div>
                                                <div id="dvImgEvl" runat="server" visible="false">
                                                    <label for="txtMarks">Marks</label>
                                                    <asp:TextBox ID="txtMarks" runat="server" Width="60px" ClientIDMode="Static" />
                                                    <asp:Label ID="lblMaxMarks" runat="server" Text=" / 5" />

                                                    <asp:RegularExpressionValidator ID="Regex1" runat="server" ValidationExpression="(([0-4]+)((\.[0,5])?)|[5])$"
                                                        ErrorMessage="Please enter valid integer or decimal number between 0 to 5 EX: 2/3/4.5 ect."
                                                        ControlToValidate="txtMarks" ForeColor="Red" />
                                                    <br />
                                                    <label for="txtComments">Comments</label>
                                                    <asp:TextBox ID="txtComments" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-footer">
                        <div class="row text-left valign-middle">
                            <div class="col-md-12">
                                <asp:Button ID="btnImagePartsPrevious" Text="Previous" runat="server" OnClick="btnImagePartsPrevious_Click" CssClass="btn btn-info" />
                                <asp:Button ID="btnImagePartsDone" Text="Save" runat="server" OnClick="btnImagePartsDone_Click" CssClass="btn btn-success" Visible="false" />
                                <asp:Button ID="btnImagePartsNext" Text="Next" runat="server" OnClick="btnImagePartsNext_Click" CssClass="btn btn-info" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
