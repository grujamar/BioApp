<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GreskaBaza.aspx.cs" Inherits="GreskaBaza" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!--#include virtual="~/content/head.inc"-->
    <title>Greška-konekcija sa bazom</title>
</head>
<body>
    <form id="form1" runat="server">
        <!--#include virtual="~/content/header.inc"-->
        <!--main start-->
        <main>
            <!--lead-section start-->
            <section class="lead-section my-4">
                <div class="container">
                    <asp:Label id="lblstranicanaziv" runat="server" CssClass="page-name">
                        Greška prilikom konekcije sa bazom podataka. Proverite konekciju ili pokušajte kasnije!
                    </asp:Label>
                </div>
            </section><!--lead-section end-->
            <!--button-section start-->
            <section>
                <div class="container">
                    <div class="row" runat="server">
                        <!--div back start-->
                        <div class="col-12 col-lg-2 mb-1 mb-md-4">
                            <asp:Button ID="btnBack" runat="server" Text="Nazad" CssClass="btn btn-secondary" OnClick="btnBack_Click"/>
                        </div>
                        <div class="col-12 col-lg-5 mb-3 mb-md-4">     
                        </div>
                        <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                        </div><!--div back end-->
                    </div>
                </div>
            </section><!--button-section end-->
        </main>
    </form>
</body>
</html>
