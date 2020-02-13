<%@ Page Language="C#" AutoEventWireup="true" CodeFile="predavanje.aspx.cs" Inherits="predavanje" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 5.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Predavanja</title>
    <!--#include virtual="~/content/head.inc"-->
    <script src="js/jquery.tooltip.js" type="text/javascript"></script>
    <script type="text/javascript">
        function TooltipImages() {
            $(".gridImages").tooltip({
                track: true,
                delay: 0,
                showURL: false,
                fade: 100,
                bodyHandler: function () {
                    return $($(this).next().html());
                },
                showURL: false
            });
        }
        function onMouseOver(rowIndex) {
             document.getElementById("GridView1").rows[rowIndex].style.backgroundColor = "#d3ddcc";
        }
        function onMouseOut(rowIndex) {
            document.getElementById("GridView1").rows[rowIndex].style.backgroundColor = "#BEBEBE";
        }
        function onMouseOutWhite(rowIndex) {
            document.getElementById("GridView1").rows[rowIndex].style.backgroundColor = "#FFFFFF";
        }
        var new_var = true;
        window.onbeforeunload = function () {
            if (new_var) {
                return "Imate promene koje niste sačuvali, ako ih ostavite, oni će biti izgubljeni!!"                
            }
        }
        function unhook() {
            new_var = false;
        }
        function errorOpeningPage() {
            swal({
                title: 'Greška prilikom otvaranja stranice.',
                text: 'Pokušajte ponovo kasnije.',
                type: 'OK'
            });
        }
    </script>
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
            <div id="afterStarting" runat="server" class="after-starting">
                <section class="my-3">
                    <div class="container">
                        <div class="row">
                            <div class="col-12 col-md-3 mb-1 text-left">
                                <asp:Label id="lblpredmetiKonacno" runat="server" CssClass="submit-label-db ml-2">Predmeti:</asp:Label>
                                <ul id="predmetiList" runat="server">
                                    <!--<li runat="server" class="submit-label ml-2"></li>-->
                                </ul> 
                            </div>
                            <div class="col-12 col-md-3 mb-1 text-left">
                                <asp:Label id="lblIzborKonacno" runat="server" CssClass="submit-label-db ml-2">Tip predavanja:</asp:Label><br />
                                <asp:Label id="spanIzborKonacno" runat="server" CssClass="submit-label ml-2"></asp:Label>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ViewStateMode="Enabled">     
                                <ContentTemplate>
                                    <fieldset>
                                        <div class="col-12 col-md-3 mb-1 text-left">
                                            <asp:Timer ID="Timer1" runat="server" Interval="1000" ontick="Timer1_Tick"></asp:Timer>
                                            <asp:Label id="Label1" runat="server" CssClass="submit-label-db ml-2">Vreme:</asp:Label><br />
                                            <asp:Label id="lblTime" runat="server" CssClass="submit-Timer ml-2"></asp:Label>
                                        </div>
                                    </fieldset>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="col-12 col-md-3 mb-1 ml-auto py-2">
                                <div class="row">
                                    <div  class="col-12">
                                        <asp:Button ID="btnEnd" runat="server" Text="Završi predavanje" CssClass="btn btn-danger btn-lg px-5" OnClick="btnEnd_Click" OnClientClick="unhook()" TabIndex="10"/>
                                    </div>
                                    <div  class="col-12">
                                        <asp:Button ID="btnEdit" runat="server" Text="Izmeni predavanje" CssClass="btn btn-warning px-2 mt-4" OnClick="btnEdit_Click" OnClientClick="unhook()" TabIndex="12"/>
                                    </div>
                                </div>   
                            </div>
                        </div>
                        <div class="row mt-0">
                            <div class="col-12 text-center mb-3">
                                <asp:Label id="lblstranicanaziv" runat="server" CssClass="page-name"> </asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12 col-md-2 mb-1">
                            </div>
                            <div class="col-12 col-md-8 mb-1 text-center">
                                <asp:UpdatePanel id="UpdatePanel1" runat="server"  UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset>
                                            <div class="row">
                                                <div class="col-8">
                                                    <div class="row">
                                                        <div class="col-12 col-sm-6">
                                                            <asp:TextBox ID="txtIndexNumber" runat="server" CssClass="submit-textbox" maxlength="20" placeholder="Upišite ID" TabIndex="1"></asp:TextBox>
                                                            <br><p class="notification" style="margin-bottom: 1px;"><asp:Label id="lblnotification" runat="server" style="font-size:11px; font-weight: bold">Broj indeksa ili korisničko ime profesora.</asp:Label></p>
                                                        </div>
                                                        <div class="col-12 col-sm-6">
                                                            <asp:Button ID="btnAddIndex" runat="server" Text="+ Dodaj prisustvo" CssClass="btn btn-secondary px-3" OnClick="btnAddIndex_Click" OnClientClick="unhook()" TabIndex="2"/>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-4">
                                                    <asp:CustomValidator ID="cvIndexNumber" runat="server" ErrorMessage="" controltovalidate="txtIndexNumber" Display="Dynamic" ForeColor="Red" CssClass="submit-customValidator" ValidateEmptyText="true" OnServerValidate="cvIndexNumber_ServerValidate" ValidationGroup="AddCustomValidatorToGroup"></asp:CustomValidator>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-12 col-md-2 mb-1">
                            </div>
                        </div>
                    </div>
                </section>
                <!--section GridView start-->
                <section class="section-gridview mb-3 mb-md-5">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-12 col-md-9" style="overflow-y: scroll; overflow-x: hidden; height: 768px;">
                                <asp:UpdatePanel id="UpdatePanel4" runat="server" UpdateMode="Conditional" ViewStateMode="Enabled">
                                    <ContentTemplate>
                                        <fieldset>
                                            <div class="gridview-left-side">
                                                <asp:Timer ID="Timer2" runat="server" Interval="5000" ontick="Timer2_Tick"></asp:Timer>
                                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowSorting="True" ShowHeaderWhenEmpty="True" Width="100%" DataKeyNames="IDDnevniStatusOsobeNaLokaciji,IDOsoba" DataSourceID="dsGridViewEP" OnRowDataBound="GridView1_RowDataBound" Height="100%" PageSize="100" OnRowCommand="GridView1_RowCommand">
                                                    <Columns>
                                                        <asp:BoundField DataField="PoslednjaPromena" HeaderText="Poslednja promena" SortExpression="PoslednjaPromena"/>
                                                        <asp:BoundField DataField="TipOsobe" HeaderText="Tip Osobe" SortExpression="TipOsobe"  Visible="false"/>
                                                        <asp:BoundField DataField="Ime" HeaderText="Ime" SortExpression="Ime"/>
                                                        <asp:BoundField DataField="Prezime" HeaderText="Prezime" SortExpression="Prezime" />
                                                        <asp:TemplateField HeaderText="Fotografija" SortExpression="BrojSlike">
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("BrojSlike") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Image ID="Image1" class="img-holder size-gridImage gridImages" runat="server" Height="64px"
                                                                    ImageUrl='<%# System.Configuration.ConfigurationManager.AppSettings["FotografijeFolder"] + Eval("BrojSlike") + ".jpg" %>' 
                                                                    Width="52px"/>
                                                                    <div id="tooltip" style="display: none;">
                                                                    <table>
                                                                    <tr>
                                                                    <%--Image to Show on Hover--%>
                                                                    <td><asp:Image ID="imgUserName" Width="202px" Height="264px" ImageUrl='<%# System.Configuration.ConfigurationManager.AppSettings["FotografijeFolder"] + Eval("BrojSlike") + ".jpg" %>' runat="server" /></td>
                                                                    </tr>
                                                                    </table>
                                                                    </div>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="BrojIndeksa" HeaderText="ID" SortExpression="BrojIndeksa"/>
                                                        <asp:BoundField DataField="TipStatusa" HeaderText="Tip statusa" SortExpression="TipStatusa"/>
                                                        <asp:BoundField DataField="Boja" HeaderText="Boja" SortExpression="Boja" Visible="false"/>
                                                        <asp:BoundField DataField="IDOsoba" HeaderText="IDOsoba" SortExpression="IDOsoba" Visible="false"/>
                                                        <asp:BoundField DataField="IDDnevniStatusOsobeNaLokaciji" HeaderText="IDDnevniStatusOsobeNaLokaciji" InsertVisible="False" ReadOnly="True" SortExpression="IDDnevniStatusOsobeNaLokaciji" Visible="false"/>
                                                        <asp:BoundField DataField="DaLiSePrijavaOdnosiNaTrenutniTermin" HeaderText="DaLiSePrijavaOdnosiNaTrenutniTermin" ReadOnly="True" SortExpression="DaLiSePrijavaOdnosiNaTrenutniTermin" Visible="false"/>
                                                        <asp:TemplateField>                                
                                                        <ItemTemplate>                                    
                                                            <asp:Button ID="btnClear" runat="server" CommandName="DeleteProfile" ToolTip="" Text="" CommandArgument='<%# Container.DisplayIndex %>' CssClass="Submit-btnClear" OnClientClick="unhook()"/>                                 
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#333333" BorderColor="#333333" BorderWidth="2px" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <HeaderStyle ForeColor="White" BackColor="#333333" BorderColor="White" BorderWidth="2px" BorderStyle="Solid" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <PagerStyle BackColor="#CCCCCC" BorderColor="#999999" ForeColor="#333333" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <RowStyle BorderColor="Black" BorderWidth="1px" Font-Bold="False" Font-Names="Arial" ForeColor="Black" HorizontalAlign="Center" />
                                                </asp:GridView>
                                                <asp:SqlDataSource ID="dsGridViewEP" runat="server" ConnectionString="<%$ ConnectionStrings:BioConnectionString %>" SelectCommand="spDnevniStatus" OldValuesParameterFormatString="original_{0}"
                                                    UpdateCommand="UPDATE blEksternoPlacanje SET [BrojPlacanja] = @BrojPlacanja,[Iznos] = @Iznos,[DatumPlacanja] = @DatumPlacanja,[Opis] = @Opis WHERE [IDEksternoPlacanje] = @original_IDEksternoPlacanje"
                                                    DeleteCommand="UPDATE blEksternoPlacanje set [Ponisteno]=getDate() WHERE [IDEksternoPlacanje] = @original_IDEksternoPlacanje" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="idLokacija" SessionField="Predavanje_idLokacija" Type="Int32" />
                                                        <asp:SessionParameter Name="idTerminPredavanja" SessionField="Predavanje_idTerminPredavanja" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-12 col-md-3">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server"  UpdateMode="Conditional" ViewStateMode="Enabled">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="Timer2" EventName="Tick" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <fieldset>
                                            <div class="gridview-right-side">
                                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" ShowHeaderWhenEmpty="True" Width="100%" style="margin-top: 0px" OnRowDataBound="GridView2_RowDataBound" Height="100%" DataSourceID="dsDnevniStatusZbirno">
                                                <Columns>
                                                    <asp:BoundField DataField="TipStatusa" HeaderText="Tip statusa" SortExpression="TipStatusa"/>
                                                    <asp:BoundField DataField="Boja" HeaderText="Boja" SortExpression="Boja" Visible="false"/>
                                                    <asp:BoundField DataField="Ukupno" HeaderText="Ukupno" SortExpression="Ukupno"/>
                                                    <asp:BoundField DataField="DaLiSePrijavaOdnosiNaTrenutniTermin" HeaderText="DaLiSePrijavaOdnosiNaTrenutniTermin" SortExpression="DaLiSePrijavaOdnosiNaTrenutniTermin" Visible="false"/>
                                                    <asp:BoundField DataField="IDTipDnevnogStatusa" HeaderText="IDTipDnevnogStatusa" SortExpression="IDTipDnevnogStatusa" Visible="false"/>
                                                </Columns>
                                                <FooterStyle BackColor="#333333" BorderColor="#333333" BorderWidth="2px" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderStyle ForeColor="White" BackColor="#333333" BorderColor="White" BorderWidth="2px" BorderStyle="Solid" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <PagerStyle BackColor="#CCCCCC" BorderColor="#999999" ForeColor="#333333" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <RowStyle BorderColor="Black" BorderWidth="1px" Font-Bold="False" Font-Names="Arial" ForeColor="Black" HorizontalAlign="Center" />
                                                </asp:GridView>
                                                <asp:SqlDataSource ID="dsDnevniStatusZbirno" runat="server" ConnectionString="<%$ ConnectionStrings:BioConnectionString %>" SelectCommand="spDnevniStatusZbirno" SelectCommandType="StoredProcedure">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="idLokacija" SessionField="Predavanje_idLokacija" Type="Int32" />
                                                        <asp:SessionParameter Name="idTerminPredavanja" SessionField="Predavanje_idTerminPredavanja" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                            </div>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div> 
                    </div>
                </section><!--section GridView end-->
            </div>
        </main>
    </form>
</body>
</html>
