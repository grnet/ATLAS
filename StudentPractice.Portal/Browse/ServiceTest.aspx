<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="el" xml:lang="el">
<head id="Head1" runat="server">
    <title>Σύστημα Κεντρικής Υποστήριξης της Πρακτικής Άσκησης Φοιτητών ΑΕΙ"</title>
    <asp:PlaceHolder runat="server">
        <meta name="version" content="<%= StudentPractice.Portal.Global.VersionNumber %>" />
    </asp:PlaceHolder>
    <meta name="description" content="Σύστημα Κεντρικής Υποστήριξης της Πρακτικής Άσκησης Φοιτητών ΑΕΙ" />
    <meta name="keywords" content="Φοιτητής, Πρακτική Άσκηση, Υπουργείο Παιδείας" />
    <meta name="language" content="Modern Greek (1453-)" />
    <meta http-equiv="Content-Language" content="el" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <link href="/_css/style-common.css" rel="stylesheet" type="text/css" />
    <link href="/_css/style-secure.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="/_js/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="/_js/jquery-migrate-1.1.1.min.js"></script>
    <script type="text/javascript" src="/_js/Imis.Lib.js"></script>
    <script type="text/javascript" src="./ServiceProxy.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            endpoint = "http://" + location.host + "/api/offices/v1/";

            $('#Services').change(function () {
                if ($(this).attr('value') == 'GET') {
                    $("#tbxID").show();
                    $("#tbxNumber").hide();
                    $("#tbxJSon").hide();
                }
                else if ($(this).attr('value') == 'GET2') {
                    $("#tbxID").show();
                    $("#tbxNumber").show();
                    $("#tbxJSon").hide();
                }
                else if ($(this).attr('value') == 'POST') {
                    $("#tbxID").hide();
                    $("#tbxNumber").hide();
                    $("#tbxJSon").show();
                }
            });

            $("#btnLogin").click(function () {
                var cred = {
                    Username: $("#tbxUsername").val(),
                    Password: $("#tbxPassword").val()
                };
                ServiceProxy.Login(cred,
                    function (msg) {
                        if (msg.Success == true) {
                            $("#preLogin").hide();
                            $("#txtAccessToken").html(msg.Result.AuthToken);
                            auth = msg.Result.AuthToken;
                            $("#postLogin").show();
                        }
                    },
                    function (error) {
                        alert(JSON.stringify(error));
                    });
            });

            $("#btnSend").click(function () {


                if ($('#Services').attr('value') == 'GET') {
                    window["ServiceProxy"][$("#Services option:selected").text()]($("#tbxID").val(),
                    function (msg) {
                        if (msg.Success == true) {
                            $("#tbxResult").html(JSON.stringify(msg.Result));
                        }
                        else {
                            $("#tbxResult").html(JSON.stringify(msg.Message));
                        }
                    },
                    function (error) {
                        $("#tbxResult").html(JSON.stringify(error));
                    });
                }
                else if ($('#Services').attr('value') == 'GET2') {
                    window["ServiceProxy"][$("#Services option:selected").text()]($("#tbxID").val(), $("#tbxNumber").val(),
                    function (msg) {
                        if (msg.Success == true) {
                            $("#tbxResult").html(JSON.stringify(msg.Result));
                        }
                        else {
                            $("#tbxResult").html(JSON.stringify(msg.Message));
                        }
                    },
                    function (error) {
                        $("#tbxResult").html(JSON.stringify(error));
                    });
                }
                else if ($('#Services').attr('value') == 'POST') {
                    var json;
                    if ($("#tbxJSon").val() != '') {
                        json = jQuery.parseJSON($("#tbxJSon").val());
                    }
                    else {
                        json = null;
                    }
                    window["ServiceProxy"][$("#Services option:selected").text()](json,
                    function (msg) {
                        if (msg.Success == true) {
                            $("#tbxResult").html(JSON.stringify(msg.Result));
                        }
                        else {
                            $("#tbxResult").html(JSON.stringify(msg.Message));
                        }
                    },
                    function (error) {
                        alert(JSON.stringify(error));
                    });
                }

            });
        });

    </script>
</head>
<body>
    <div id="body-container">
        <div id="header">
            <div id="header-logo">
                <a href="http://atlas.grnet.gr" target="_blank">
                    <img src="/_img/atlas_logo.png" alt="ΑΤΛΑΣ - Κόμβος Πρακτικής Άσκησης" />
                </a>
            </div>
            <div id="header-literal">
                <h1>Σύστημα Κεντρικής Υποστήριξης της Πρακτικής Άσκησης Φοιτητών ΑΕΙ</h1>
            </div>
        </div>
        <div id="container">
            <form id="form1" runat="server">
                <asp:ScriptManager ID="sm" runat="server">
                    <CompositeScript>
                        <Scripts>
                            <asp:ScriptReference Path="~/_js/jquery-1.9.1.min.js" />
                            <asp:ScriptReference Path="~/_js/jquery-migrate-1.1.1.min.js" />
                            <asp:ScriptReference Path="~/_js/Imis.Lib.js" />
                            <asp:ScriptReference Path="~/_js/SchoolSearch.js" />
                            <asp:ScriptReference Path="~/_js/popUp.js" />
                        </Scripts>
                    </CompositeScript>
                </asp:ScriptManager>
                <h1>Service Tester</h1>
                <br />
                <br />
                <div id="preLogin">
                    <input type="text" id="tbxUsername" title="Όνομα" placeholder="Όνομα χρήστη..." />
                    <input type="password" id="tbxPassword" title="Κωδικός" placeholder="Κωδικός πρόσβασης..." />
                    <input type="button" id="btnLogin" value="Login" />
                </div>
                <div id="postLogin" style="display: none">
                    Access Token:
                    <br />
                    <asp:TextBox ID="txtAccessToken" ClientIDMode="Static" ReadOnly="true" Style="width: 50%" runat="server" Rows="3" TextMode="MultiLine" />
                    <br />
                    <br />
                    <select id="Services">
                        <option></option>
                        <option value="POST">AssignStudent</option>
                        <option value="POST">CancelPosition</option>
                        <option value="POST">ChangeAssignedStudent</option>
                        <option value="POST">ChangeImplementationData</option>
                        <option value="POST">CompletePosition</option>
                        <option value="POST">DeleteAssignment</option>
                        <option value="POST">DeleteAssignmentInfo</option>
                        <option value="POST">DeleteFinishedPosition</option>
                        <option value="POST">FindAcademicIDNumber</option>
                        <option value="POST">FindStudentWithAcademicIDNumber</option>
                        <option value="GET">GetAcademics</option>
                        <option value="POST">GetAssignedPositions</option>
                        <option value="POST">GetAvailablePositionGroups</option>
                        <option value="GET">GetCities</option>
                        <option value="POST">GetCompletedPositions</option>
                        <option value="GET">GetCountries</option>
                        <option value="POST">GetFinishedPositions</option>
                        <option value="GET">GetInstitutions</option>
                        <option value="GET">GetPhysicalObjects</option>
                        <option value="GET">GetPositionGroupDetails</option>
                        <option value="POST">GetPreAssignedPositions</option>
                        <option value="GET">GetPrefectures</option>
                        <option value="GET">GetProviderDetails</option>
                        <option value="GET">GetProvidersByAFM</option>
                        <option value="POST">GetRegisteredStudents</option>
                        <option value="GET">GetStudentDetailsAcademicIDNumber</option>
                        <option value="GET">GetStudentDetailsID</option>
                        <option value="GET">GetStudentDetailsPrincipalName</option>
                        <option value="GET2">GetStudentDetailsStudentNumber</option>
                        <option value="POST">PreAssignPositions</option>
                        <option value="POST">RegisterFinishedPosition</option>
                        <option value="POST">RegisterNewStudent</option>
                        <option value="POST">RollbackPreAssignment</option>
                        <option value="POST">RollbackPreAssignmentInfo</option>
                        <option value="POST">UpdateStudent</option>
                        <%--<option value="POST">ChangeFundingType</option>
                        <option value="GET">GetFundingType</option>                        --%>
                    </select>
                    <input type="text" id="tbxID" style="display: none" placeholder="ID" />
                    <input type="text" id="tbxNumber" style="display: none" placeholder="StudentNumber" />
                    <asp:TextBox ID="tbxJSon" ClientIDMode="Static" Style="width: 50%" runat="server" Rows="3" TextMode="MultiLine" placeholder="Properties" />
                    <input type="button" id="btnSend" value="Send" />

                    <br />
                    <br />
                    <br />

                    <asp:TextBox ID="tbxResult" ClientIDMode="Static" Style="width: 100%" runat="server" Rows="3" ReadOnly="true" TextMode="MultiLine" placeholder="Result" />
                    <br />

                </div>
            </form>
        </div>
    </div>
</body>
</html>
