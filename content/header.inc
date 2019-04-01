<!--header start-->
<header class="py-3" style="background-image: linear-gradient(to right, rgba(220, 220, 220,0.3), rgba(220, 220, 220,0.9))">
    <div class="container">
        <nav class="navbar navbar-expand-md navbar-light px-0">
        <!--logo start-->
        <div class="navbar-container" id="navbar-container">
            <asp:Image id="logo" runat="server" CssClass="logo-image" imageurl="~/img/logo.jpg"/>
            <asp:Label id="lblscnsnaziv" runat="server" CssClass="scns-name pl-1 pl-sm-4">                               
                Studentski centar Novi Sad                                    
            </asp:Label>         
        </div><!--logo end-->
        <!---->
		<button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#main-menu" aria-controls="main-menu" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
			<!--header navigation start-->
			<div class="collapse navbar-collapse" id="main-menu">
				<ul class="navbar-nav ml-auto mt-2 px-lg-5">
					<li class="nav-item active">
						<a class="nav-link" href="ZaduzivanjeUsluga.aspx">Zaduživanje usluga <span class="sr-only">(current)</span></a>
					</li>
					<li class="nav-item">
						<a class="nav-link" href="EksternaPlacanja.aspx">Eksterna Plaćanja</a>
					</li>
				</ul>                        
			</div><!--header navigation end-->
        </nav>
    </div>
</header><!--header end-->
