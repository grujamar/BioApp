<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pregledIzvestaji.aspx.cs" Inherits="pregledIzvestaji" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Izvestaji</title>
    <!--#include virtual="~/content/head.inc"-->
</head>
<body class="login-bg">
    <form id="form1" runat="server">
        <!--header start-->
        <header class="py-3" style="background-image: linear-gradient(to right, rgba(220, 220, 220,0.3), rgba(220, 220, 220,0.9))">
            <div class="container">
                <nav class="navbar navbar-expand-md navbar-light px-0">
                    <!--logo start-->
                    <div class="navbar-container" id="navbar-container">
                        <asp:Image id="logo" runat="server" CssClass="logo-image" imageurl="~/images/logo.png"/>
                        <asp:Label id="lblscnsnaziv" runat="server" CssClass="scns-name pl-1 pl-sm-4">                               
                            biološki fakultet                                    
                        </asp:Label>         
                    </div><!--logo end-->
                    <!--header navigation start-->
			        <div class="collapse navbar-collapse" id="main-menu">
				        <article class="navbar-nav ml-auto mt-2 px-lg-5">
                            <span id="lbl_Operater" style="font-size:21px;">&nbsp;Dobrodošli, &nbsp;</span>
                            <asp:Label ID="lbl_Ime" runat="server" style="font-size:21px;" Text="LabelIme" Visible="true"></asp:Label>&nbsp;
                            <asp:Button ID="btnLogout" runat="server" Text="Odjava" CssClass="btn btn-outline-secondary ml-4 px-md-3 py-md-1" OnClick="btnLogout_Click" OnClientClick="unhook()"/>
				        </article>                        
			        </div><!--header navigation end-->
                </nav>
            </div>
        </header><!--header end-->
        <!--main start-->
        <main>
            <asp:ScriptManager ID="ScriptManager1" runat="server" />
            <section class="my-4">
                <div class="container">
                    <div class="row">
                        <div class="col-12 mb-1 text-left">
                            <asp:Label id="lblpredmetiKonacno" runat="server" CssClass="submit-label-db ml-2 text-uppercase font-weight-bold">Izveštaji:</asp:Label>
                            <ul id="izvestajiList" runat="server">
                                <!--<li runat="server" class="submit-label ml-2"></li>-->
                            </ul> 
                        </div>
                    </div>
                </div>
            </section>
            <div class="row">
                <!--div search start-->
                <div class="col-12 col-md-4 mb-1">
                </div>
                <div class="col-12 col-md-4 mb-1 text-center">
                    <asp:Button ID="btnBack" runat="server" Text="Nazad" CssClass="btn btn-success btn-lg px-5" OnClick="btnBack_Click" OnClientClick="unhook()"/>
                </div>
                <div class="col-12 col-md-4 mb-1">
                </div><!--div search end-->
            </div>
        </main> <!--main end-->
    </form>
</body>
</html>
