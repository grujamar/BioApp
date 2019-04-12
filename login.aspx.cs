using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class login : System.Web.UI.Page
{
    //Lofg4Net declare log variable
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    protected void Page_Load(object sender, EventArgs e)
    {
        Utility utility = new Utility();
        bool ConnectionActive = utility.IsAvailableConnection();
        if (!ConnectionActive)
        {
            Response.Redirect("GreskaBaza.aspx", false); // this will tell .NET framework not to stop the execution of the current thread and hence the error will be resolved.
        }
        AvoidCashing();

        if (!Page.IsPostBack)
        {
            log.Info("Aplication successfully start. ");
        }
    }

    private void AvoidCashing()
    {
        Response.Cache.SetNoStore();
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
    }

    protected void LoginButton_Click(object sender, EventArgs e)
    {
        int IDLokacija = 1;
        

        try
        {
            if (Page.IsValid)
            {
                int IDLogPredavanja = 0;
                int IDOsoba = 0;
                string Ime = string.Empty;
                string Prezime = string.Empty;
                int Result = 0;

                Utility utility = new Utility();

                utility.loginPredavanja(txtUsername.Text, IDLokacija, out IDLogPredavanja, out IDOsoba, out Ime, out Prezime, out Result);
                log.Debug("Login Predavanja: " + " Sifra lokacije - " + IDLokacija + " " + ". Username - " + txtUsername.Text + " " + ". IDLogPredavanja - " + IDLogPredavanja + " " + ". idOsoba - " + IDOsoba + " " + ". Ime - " + Ime + " " + ". Prezime - " + Prezime + " " + ". Rezultat - " + Result);

                if (Result != 0)
                {
                    throw new Exception("Result from database is diferent from 0. Result is: " + Result);
                }
                else
                {
                    Session["login_Ime"] = Ime;
                    Session["login_Prezime"] = Prezime;
                    Session["lbl_loginID"] = IDOsoba;
                    Session["login_IDLogPredavanja"] = IDLogPredavanja;

                    Response.Redirect("index.aspx", false); // this will tell .NET framework not to stop the execution of the current thread and hence the error will be resolved.
                }
            }
            else if (!Page.IsValid)
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
            }
        }
        catch (Exception ex)
        {
            log.Error("Error while trying to LOGIN. " + ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
        }
    }

    protected void cvUsername_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string newUsername = txtUsername.Text;
        string errMessage = string.Empty;

        args.IsValid = Utils.ValidateUsername(newUsername, out errMessage);
        cvUsername.ErrorMessage = errMessage;
    }


}