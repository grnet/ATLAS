<%@ Page Title="Ολοκληρωμένη ΠΑ" Language="C#" MasterPageFile="~/PopUp.Master" AutoEventWireup="true" CodeBehind="ViewPositionCert.aspx.cs" Inherits="StudentPractice.Portal.Secure.Students.ViewPositionCert" %>


<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">
    <style type="text/css">
        @media print {
            .no-print, .no-print * {
                display: none !important;
            }
        }

        body {
            background-color: white;
        }

        #voucherCodeDiv {
            padding: 10px 0px 10px 0px;
        }

        #logosDiv {
            height: 120px;
        }

        #imgEinclusion {
            position: relative;
            float: left;
        }

        #imgEinclusionName {
            position: relative;
            float: right;
            margin-top: 30px;
        }

        #detailsDiv {
            font-size: 1.3em;
            padding: 10px;
            width: 100%;
        }

            #detailsDiv h2 {
                font-size: 1.3em;
                border-bottom: none;
                text-align: center;
            }

            #detailsDiv p {
                font-size: 1.3em;
            }

                #detailsDiv p span {
                    font-size: 1.0em;
                }

        #beneficiaryDetailsDiv {
            font-size: 1.3em;
            margin-bottom: 12px;
        }


        #headerDiv h1 {
            color: #01478C;
        }

        #headerDiv,
        #footerDiv {
            height: 150px;
            padding: 5px;
        }

            #headerDiv div,
            #footerDiv div {
                margin-left: auto;
                margin-right: auto;
            }

            #headerDiv img {
                float: left;
                margin-left: 1.2em;
                margin-right: 1.2em;
            }

        strong {
            font-weight: bold;
        }
    </style>

    <div style="width: 98%">
        <div id="headerDiv">
            <div>
                <table style="width: 100%">
                    <tr>
                        <td>
                            <a href="http://atlas.grnet.gr" target="_blank">
                                <img src="/_img/atlas_logo.png" alt="ΑΤΛΑΣ - Κόμβος Πρακτικής Άσκησης" />
                            </a>
                        </td>
                        <td>
                            <h1 style="text-align: center; width: 100%;">
                                <asp:Literal runat="server" Text="<%$ Resources:GlobalProvider, MainHeader %>" />
                            </h1>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="detailsDiv">
            <h2>Επιτυχής Ολοκλήρωση Πρακτικής Άσκησης</h2>
            <p>
                Ο/η 
                <asp:Literal ID="litContactName" runat="server" />
                φοιτητής/τρια στο τμήμα
                <asp:Literal ID="litDepartment" runat="server" />
                του Ιδρύματος
                <asp:Literal ID="litInstitution" runat="server" />
                με Αριθμό Μητρώου
                <asp:Literal ID="litStudentCode" runat="server" />
            </p>
            <p>
                <span style="font-weight: bold;">ολοκλήρωσε</span> την Πρακτική Άσκηση στο διάστημα 
                <asp:Literal ID="litStartDate" runat="server" />
                έως  
                <asp:Literal ID="litEndDate" runat="server" />
            </p>
            <p>
                στον Φορέα Υποδοχής Πρακτικής Άσκησης
                <span style="font-style: italic">
                    <asp:Literal ID="litProviderName" runat="server" /></span>
                με ΑΦΜ
                <asp:Literal ID="litProviderAFM" runat="server" />
                στον οποίο υπεύθυνος είναι ο/η 
                <asp:Literal ID="litProviderContactName" runat="server" />
                για την Θέση
                "<asp:Literal ID="litPositionTitle" runat="server" />"
                με κωδικό Θέσης
                <asp:Literal ID="litPositionID" runat="server" />
            </p>
        </div>
        <div id="footerDiv">
            <div>
                <table style="margin: 0px auto 0px auto;">
                    <tr>
                        <td>
                            <a href="http://minedu.gov.gr/" target="_blank">
                                <img src="/_img/minedu_logo_4.png" alt="Υπουργείο Παιδείας, Έρευνας και Θρησκευμάτων" height="60" /></a>
                        </td>
                        <td>
                            <a href="http://www.grnet.gr/" target="_blank">
                                <img src="/_img/grnet_logo.png" alt="ΕΔΕΤ" /></a>
                        </td>
                        <td>
                            <a href="http://www.edulll.gr" target="_blank">
                                <img src="/_img/edulll_logo.png" alt="Εκπαίδευση και Δια Βίου Μάθηση" /></a>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <table style="width: 100%">
        <tr>
            <td style="text-align: center;">
                <asp:LinkButton ID="btnPrint" runat="server" Text="Εκτύπωση" CssClass="icon-btn bg-print no-print"
                    CausesValidation="false" OnClientClick="window.print();" />
            </td>
            <td style="text-align: center;">
                <asp:LinkButton ID="btnCancel" runat="server" Text="Κλείσιμο" CssClass="icon-btn bg-cancel no-print"
                    CausesValidation="false" OnClientClick="window.parent.popUp.hide();" />
            </td>
        </tr>
    </table>
</asp:Content>
