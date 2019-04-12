<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link rel="stylesheet" href="/css/style-login.css" type="text/css" />
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.2.6/jquery.min.js"></script>
    <script type="text/javascript" src="js/Main.js"></script>
    <script type="text/javascript" src="js/sweetalert.js"></script>
    <!-- Bootstrap CSS -->
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css"/>
    <script type="text/javascript">
        function erroralert() {
            swal({
                title: 'Greška prilikom logovanja.',
                text: 'Ispravite podatke i pokušajte ponovo.',
                type: 'OK'
            });
        }
    </script>
</head>
<body class="login-bg">
    <div id="wrapper">
        <div class="user-icon mt-3"></div>
        <form name="login-form" class="login-form" method="post" runat="server" >
            <div class="header">
                <h1>Predavanja</h1>
                <span>Popunite vaše korisničke informacije kako bi se ulogovali u sistem.</span>
            </div>
            <div class="content">
                <asp:TextBox ID="txtUsername" runat="server" CssClass="input username" maxlength="25" TabIndex="1" placeholder="Korisničko ime"></asp:TextBox>
                <asp:Label ID="errUsername" runat="server" style="font-size:13px;" ForeColor="Red"></asp:Label>
                <asp:CustomValidator runat="server" id="cvUsername" controltovalidate="txtUsername" errormessage="" OnServerValidate="cvUsername_ServerValidate" Display="Dynamic" ForeColor="Red" style="font-size:13px;" ValidateEmptyText="true"/>
            </div>
            <div class="footer">
                <asp:Button ID="btnSubmit" runat="server" Text="Prijava" CssClass="button" OnClick="LoginButton_Click" TabIndex="2"/>
            </div>
        </form>
    </div>
    <div class="gradient"></div>
</body>
</html>
