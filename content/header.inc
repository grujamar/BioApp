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
                    <asp:Label ID="lbl_Prezime" runat="server" style="font-size:21px;" Text="LabelPrezime" Visible="true"></asp:Label>
                    <asp:Button ID="btnLogout" runat="server" Text="Odjava" CssClass="btn btn-outline-secondary ml-4 px-md-3 py-md-1" OnClick="btnLogout_Click" OnClientClick="unhook()"/>
				</article>                        
			</div><!--header navigation end-->
        </nav>
    </div>
</header><!--header end-->
