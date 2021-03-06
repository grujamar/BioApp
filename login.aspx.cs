﻿using log4net;
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
            log.Info("encryptedParameters on Login page - " + encryptedParameters);

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
                            lblLocation.Text = utility.getImeLokacije(idLokacija);
                            Session["login-ImeLokacijeZaLog"] = lblLocation.Text;
                            log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Location name - " + lblLocation.Text);
                        }
                        else
                        {
                            Response.Redirect("GreskaLokacija.aspx", false);
                            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Error. IDLokacija is: " + encryptedParameters);
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("GreskaLokacija.aspx", false);
                log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Error. IDLokacija is: " + encryptedParameters);
            }
        }
        catch (Exception ex)
        {
            if (Session["login-ImeLokacijeZaLog"] == null)
            {
                Session["login-ImeLokacijeZaLog"] = string.Empty;
            }
            Response.Redirect("GreskaLokacija.aspx", false);
            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Error. " + ex);
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
            Session["idTerminPredavanja"] = 0;

            if (Page.IsValid)
            {
                int IDLokacija = Convert.ToInt32(Session["login-IDLokacija"]);
                int IDLogPredavanja = 0;
                int IDOsoba = 0;
                string ImePrezime = string.Empty;
                int Result = 0;

                Utility utility = new Utility();

                utility.loginPredavanja(txtUsername.Text, IDLokacija, out IDLogPredavanja, out IDOsoba, out ImePrezime, out Result);
                log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Login Predavanja: " + " Sifra lokacije - " + IDLokacija + " " + ". Username - " + txtUsername.Text + " " + ". IDLogPredavanja - " + IDLogPredavanja + " " + ". idOsoba - " + IDOsoba + " " + ". Ime - " + ImePrezime + " " + ". Rezultat - " + Result);

                if (Result != 0)
                {
                    throw new Exception("Result from database is diferent from 0. Result is: " + Result);
                }
                else
                {
                    Session["login_Ime"] = ImePrezime;
                    Session["lbl_loginID"] = IDOsoba;
                    Session["login_IDLogPredavanja"] = IDLogPredavanja;

                    string PageToRedirect = "index.aspx";
                    int idTerminPredavanjaIzmena = 0;
                    try
                    {
                        string idTerminPredavanjaIzmena1 = @"IDTerminPredavanja=" + idTerminPredavanjaIzmena;
                        log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "idTerminPredavanjaIzmena is - " + idTerminPredavanjaIzmena1);
                        string editParameters = AuthenticatedEncryption.AuthenticatedEncryption.Encrypt(idTerminPredavanjaIzmena1, Constants.CryptKey, Constants.AuthKey);
                        editParameters = editParameters.Replace("+", "%252b");
                        log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Page to redirect. editParameters is - " + editParameters);
                        Response.Redirect(string.Format("~/" + PageToRedirect + "?d={0}", editParameters), false);
                    }
                    catch (Exception ex)
                    {
                        log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Error while opening the Page: " + PageToRedirect + " . Error message: " + ex.Message);
                        throw new Exception("Error while opening the Page: " + PageToRedirect + " . Error message: " + ex.Message);
                    }
                }
            }
            else if (!Page.IsValid)
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
            }
        }
        catch (Exception ex)
        {
            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Error while trying to LOGIN. " + ex.Message);
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