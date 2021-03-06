﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit"%>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 5.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Predavanja</title>
    <!--#include virtual="~/content/head.inc"-->
    <script src="js/jquery.tooltip.js" type="text/javascript"></script>
    <script type="text/javascript">
        function pickdate() {
            $("[id$=txtdate]").datepicker({
                showOn: 'button',
                buttonText: 'Izaberite datum',
                buttonImageOnly: true,                
                buttonImage: "images/calendar.png",
                dayNames: ['Nedelja', 'Ponedeljak', 'Utorak', 'Sreda', 'Četvrtak', 'Petak', 'Subota'],
                dayNamesMin: ['Ned', 'Pon', 'Uto', 'Sre', 'Čet', 'Pet', 'Sub'],
                dateFormat: 'dd.mm.yy',
                monthNames: ['Januar', 'Februar', 'Mart', 'April', 'Maj', 'Jun', 'Jul', 'Avgust', 'Septembar', 'Oktobar', 'Novembar', 'Decembar'],
                monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'Maj', 'Jun', 'Jul', 'Avg', 'Sep', 'Okt', 'Nov', 'Dec'],
                firstDay: 1,
                constrainInput: true,
                changeMonth: true,
                changeYear: true,
                yearRange: '1900:2100',
                showButtonPanel: false,
                closeText: "Zatvori",
                beforeShow: function () { try { FixIE6HideSelects(); } catch (err) { } },
                onClose: function () { try { FixIE6ShowSelects(); } catch (err) { } }
            });
            $(".ui-datepicker-trigger").mouseover(function () {
                $(this).css('cursor', 'pointer');
            });
            $(".ui-datepicker-trigger").css("margin-bottom", "3px");
            $(".ui-datepicker-trigger").css("margin-left", "3px");
        };
        function DisableCalendar() {
            $("[id$=txtdate]").datepicker('disable');
            return false;
        }
        function EnableCalendar() {
            $("[id$=txtdate]").datepicker('enable');
            return false;
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
            <div id="beforeStarting" runat="server">
                <div class="container">
                    <!--lead-section start-->
                    <section class="lead-section my-4">
                        <asp:Label id="lblstranicanaziv" runat="server" CssClass="page-name"> </asp:Label>
                    </section><!--lead-section end-->
                    <div class="row">
                        <!--div search start-->
                        <div class="col-12 col-md-4 mb-1">
                        </div>
                        <div class="col-12 col-md-4 mb-1 text-center">
                            <asp:Button ID="btnReport" runat="server" Text="Prethodna predavanja" CssClass="btn btn-outline-success" OnClick="btnReport_Click" OnClientClick="unhook()"/>
                        </div>
                        <div class="col-12 col-md-4 mb-1">
                        </div><!--div search end-->
                    </div>
                    <!--section checkbox start-->
                    <section class="checkbox-section">
                        <asp:Label 
                            ID="lblpredmeti"
                            runat="server" 
                            Text="Izaberite predmete" 
                            AssociatedControlID="CheckBoxList1"
                            Font-Underline="true"
                            Font-Bold="true"
                            Font-Size="Medium"
                            />
                        <asp:CheckBoxList 
                            ID="CheckBoxList1"
                            runat="server"
                            Font-Italic="false"
                            Font-Names="Times New Roman"
                            CssClass="mycheckbox"
                            Font-Size="Medium" DataSourceID="dsPredmeti" DataTextField="NazivPredmeta" DataValueField="IDPredmet"
                            >
                            <asp:ListItem></asp:ListItem>
                        </asp:CheckBoxList>
                        <asp:SqlDataSource ID="dsPredmeti" runat="server" ConnectionString="<%$ ConnectionStrings:BioConnectionString %>" SelectCommand="SELECT IDPredmet, NazivPredmeta, BrojAkreditacije FROM vPredavanjaNastavnika WHERE (IDOsoba = @idosoba) ORDER BY NazivPredmeta, SifraPredmeta">
                            <SelectParameters>
                                <asp:SessionParameter Name="idosoba" SessionField="lbl_loginID" DefaultValue="" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:CustomValidator ID="cvCheckbox" runat="server" ErrorMessage="" Display="Dynamic" ForeColor="Red" CssClass="submit-customValidator" OnServerValidate="CheckBoxList1_ServerValidation" ValidationGroup="AddCustomValidatorToGroup"></asp:CustomValidator>
                    </section><!--section checkbox end-->
                    <asp:ScriptManager ID="ScriptManager1" runat="server" />
                      <asp:UpdatePanel ID="UpdatePanel1" runat="server">     
                        <ContentTemplate>
                            <fieldset>
                                <div class="row izbor-section">
                                    <!--div ddlizbor start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="spanizbor" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="lblizbor" runat="server" CssClass="submit-label ml-2">Tip predavanja:</asp:Label> 
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:DropDownList ID="ddlizbor" runat="server" AppendDataBoundItems="True" CssClass="submit-dropdownlist" OnSelectedIndexChanged="ddlizbor_SelectedIndexChanged" TabIndex="2" DataSourceID="dsTipPredavanja" DataTextField="TipPredavanja" DataValueField="IDTipPredavanja">
                                        <asp:ListItem Selected="True" Value="0">--Izaberite--</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="dsTipPredavanja" runat="server" ConnectionString="<%$ ConnectionStrings:BioConnectionString %>" SelectCommand="SELECT [IDTipPredavanja], [TipPredavanja] FROM [vTipPredavanja]"></asp:SqlDataSource>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:CustomValidator runat="server" id="cvizbor" controltovalidate="ddlizbor" errormessage="" OnServerValidate="Cvizbor_ServerValidate" CssClass="submit-customValidator" Display="Dynamic" ForeColor="Red" ValidateEmptyText="true" ValidationGroup="AddCustomValidatorToGroup"/>
                                    </div><!--div ddlizbor end-->
                                    <!--div date start-->
                                    <div class="col-12 col-lg-2 mb-1 mb-md-4">
                                        <asp:Label id="spandate" runat="server" CssClass="submit-span">*</asp:Label><asp:Label id="lbldate" runat="server" CssClass="submit-label ml-2">Datum:</asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-5">
                                        <asp:TextBox ID="txtdate" runat="server" CssClass="price-textbox" maxlength="10" TabIndex="6" ontextchanged="txtdate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:Label id="dateexample" runat="server" CssClass="submit-example ml-2">Primer: 21.09.2010</asp:Label>
                                    </div>
                                    <div class="col-12 col-lg-5 mb-3 mb-lg-0">
                                        <asp:Label ID="errLabel2" runat="server" CssClass="submit-customValidator"></asp:Label>
                                        <asp:CustomValidator ID="cvdate" runat="server" ErrorMessage="" controltovalidate="txtdate" Display="Dynamic" ForeColor="Red" CssClass="submit-customValidator" ValidateEmptyText="true" OnServerValidate="Cvdate_ServerValidate" ValidationGroup="AddCustomValidatorToGroup"></asp:CustomValidator>
                                    </div><!--div date end-->
                                </div>
                            </fieldset>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--section search start-->
                    <section class="search-section py-1 py-md-2">
                        <div id="buttonStartVisible" class="row" runat="server">
                            <!--div search start-->
                            <div class="col-12 col-md-4 mb-1">
                            </div>
                            <div class="col-12 col-md-4 mb-1 mb-4 text-center">
                                <asp:Button ID="btnStart" runat="server" Text=">>>Započni predavanje<<<" CssClass="btn btn-info" OnClick="btnStart_Click" OnClientClick="unhook()" TabIndex="9"/>
                            </div>
                            <div class="col-12 col-md-4 mb-1">
                            </div><!--div search end-->
                        </div>
                        <div id="buttonEditVisible" class="row" runat="server">
                            <!--div search start-->
                            <div class="col-12 col-md-4 mb-1">
                            </div>
                            <div class="col-12 col-md-4 mb-1 mb-4 text-center">
                                <asp:Button ID="btnEdit" runat="server" Text=">>>Izmeni predavanje<<<" CssClass="btn btn-warning" OnClick="btnEdit_Click" OnClientClick="unhook()" TabIndex="10"/>
                            </div>
                            <div class="col-12 col-md-4 mb-1">
                            </div><!--div search end-->
                        </div>
                    </section><!--section search end-->
                </div>
            </div>
            <div id="reportHiding" runat="server">
                <div class="container">
                    <br />
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="800px" 
                        Height="600px" Font-Names="Verdana" Font-Size="8pt" ShowExportControls="False" 
                            InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana" 
                            WaitMessageFont-Size="14pt" AsyncRendering="False" BackColor="" ClientIDMode="AutoID" HighlightBackgroundColor="" InternalBorderColor="204, 204, 204" InternalBorderStyle="Solid" InternalBorderWidth="1px" LinkActiveColor="" LinkActiveHoverColor="" LinkDisabledColor="" PrimaryButtonBackgroundColor="" PrimaryButtonForegroundColor="" PrimaryButtonHoverBackgroundColor="" PrimaryButtonHoverForegroundColor="" SecondaryButtonBackgroundColor="" SecondaryButtonForegroundColor="" SecondaryButtonHoverBackgroundColor="" SecondaryButtonHoverForegroundColor="" SplitterBackColor="" ToolbarDividerColor="" ToolbarForegroundColor="" ToolbarForegroundDisabledColor="" ToolbarHoverBackgroundColor="" ToolbarHoverForegroundColor="" ToolBarItemBorderColor="" ToolBarItemBorderStyle="Solid" ToolBarItemBorderWidth="1px" ToolBarItemHoverBackColor="" ToolBarItemPressedBorderColor="51, 102, 153" ToolBarItemPressedBorderStyle="Solid" ToolBarItemPressedBorderWidth="1px" ToolBarItemPressedHoverBackColor="153, 187, 226">
                        <LocalReport ReportPath="Report.rdlc">
                            <DataSources>
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                                    Name="DataSetIzvestajOpste" />
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource2" Name="DataSetIzvestajDetalji" />
                                <rsweb:ReportDataSource DataSourceId="ObjectDataSource3" Name="DataSetZbirno" />
                            </DataSources>
                        </LocalReport>
                    </rsweb:ReportViewer> 

                    <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="IzvestajDataSetTableAdapters.spDnevniStatusZbirnoTableAdapter">
                        <SelectParameters>
                            <asp:Parameter DefaultValue="0" Name="idLokacija" Type="Int32" />
                            <asp:SessionParameter DefaultValue="" Name="idTerminPredavanja" SessionField="idTerminPredavanja" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>

                    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="IzvestajDataSetTableAdapters.spIzvestajTerminDetaljiTableAdapter">
                        <SelectParameters>
                            <asp:SessionParameter Name="idTerminPredavanja" SessionField="idTerminPredavanja" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="IzvestajDataSetTableAdapters.spIzvestajTerminOpsteTableAdapter">
                        <SelectParameters>
                            <asp:SessionParameter Name="idTerminPredavanja" SessionField="idTerminPredavanja" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </div>
            </div>
        </main><!--main end-->
    </form>
</body>
</html>
