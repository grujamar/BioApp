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

        try
        {
            string encryptedParameters = Request.QueryString["d"];
            log.Debug("encryptedParameters on Login page - " + encryptedParameters);

            if ((encryptedParameters != string.Empty) && (encryptedParameters != null))
            {
                // replace encoded plus sign "%2b" with real plus sign +
                encryptedParameters = encryptedParameters.Replace("%2b", "+");

                string decryptedParameters = AuthenticatedEncryption.AuthenticatedEncryption.Decrypt(encryptedParameters, Constants.CryptKey, Constants.AuthKey);

                if (decryptedParameters == null)
                {
                    throw new Exception("decryptedParameters error. ");
                }

                HttpRequest req = new HttpRequest("", "http://www.pis.rs", decryptedParameters);

                string data = req.QueryString["IDLokacija"];

                if ((data != string.Empty) && (data != null))
                {
                    Session["login-IDLokacija"] = data;
                }
                else
                {
                    Session["login-IDLokacija"] = "0";
                }

                if (!Page.IsPostBack)
                {
                    if (Session["login-IDLokacija"] != null)
                    {
                        int idLokacija = Convert.ToInt32(Session["login-IDLokacija"]);
                        if (idLokacija != 0)
                        {
                            log.Info("Aplication successfully start. IDLokacija is: " + idLokacija);
                        }
                        else
                        {
                            Response.Redirect("GreskaLokacija.aspx", false);
                            log.Error("Error. IDLokacija is: " + encryptedParameters);
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("GreskaLokacija.aspx", false);
                log.Error("Error. IDLokacija is: " + encryptedParameters);
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("GreskaLokacija.aspx", false);
            log.Error("Error. IDLokacija is: " + ex);
        }
    }

    private void AvoidCashing()
    {
        Response.Cache.SetNoStore();
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
    }

    protected void LoginButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                int IDLokacija = Convert.ToInt32(Session["login-IDLokacija"]);
                int IDLogPredavanja = 0;
                int IDOsoba = 0;
                string ImePrezime = string.Empty;
                int Result = 0;

                Utility utility = new Utility();

                utility.loginPredavanja(txtUsername.Text, IDLokacija, out IDLogPredavanja, out IDOsoba, out ImePrezime, out Result);
                log.Debug("Login Predavanja: " + " Sifra lokacije - " + IDLokacija + " " + ". Username - " + txtUsername.Text + " " + ". IDLogPredavanja - " + IDLogPredavanja + " " + ". idOsoba - " + IDOsoba + " " + ". Ime - " + ImePrezime + " " + ". Rezultat - " + Result);

                if (Result != 0)
                {
                    throw new Exception("Result from database is diferent from 0. Result is: " + Result);
                }
                else
                {
                    Session["login_Ime"] = ImePrezime;
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