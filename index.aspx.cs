﻿using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using Microsoft.Reporting.WebForms;
using System.IO;

public partial class index : System.Web.UI.Page
{
    //Lofg4Net declare log variable
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public string SetLightGray = Constants.SetLightGray;
    public string SetWhite = Constants.SetWhite;
    public string SetDarkGray = Constants.SetDarkGray;

    protected void Page_Load(object sender, EventArgs e)
    {
        Utility utility = new Utility();
        bool ConnectionActive = utility.IsAvailableConnection();
        if (!ConnectionActive)
        {
            Response.Redirect("GreskaBaza.aspx", false);
        }
        AvoidCashing();
        ShowDatepicker();

        try
        {
            string encryptedParameters = Request.QueryString["d"];
            log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " encryptedParameters on Index page - " + encryptedParameters);

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

                string data = req.QueryString["IDTerminPredavanja"];

                if (data == "0")
                {
                    ChangeButtonVisibility(true);
                    Session["Predavanje_idTerminPredavanjaIzmena"] = data;
                    log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " Predavanje_idTerminPredavanjaIzmena is - " + data);
                }
                else {
                    ChangeButtonVisibility(false);
                    Session["Predavanje_idTerminPredavanjaIzmena"] = data;
                }

                if (!Page.IsPostBack)
                {
                    if (Convert.ToInt32(Session["Predavanje_idTerminPredavanjaIzmena"]) == 0)
                    {
                        int idOsoba = Convert.ToInt32(Session["lbl_loginID"]);
                        int IDTerminPredavanja = 0;
                        int IDLokacija = Convert.ToInt32(Session["login-IDLokacija"]);
                        int IDLogPredavanja = 0;
                        List<int> predmetiList = new List<int>();
                        TimeSpan d1 = new TimeSpan();
                        utility.getTerminPredavanjaKraj(idOsoba, IDLokacija, out IDTerminPredavanja, out IDLogPredavanja, out predmetiList, out d1);
                        log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " Check in table TerminPredavanja if Kraj is null. IDTerminPredavanja - " + IDTerminPredavanja + " IDLokacija - " + IDLokacija + " IDLogPredavanja - " + IDLogPredavanja + " predmetiList.Count - " + predmetiList.Count + " Pocetak - " + d1);

                        lblstranicanaziv.Text = utility.getImeLokacije(Convert.ToInt32(Session["login-IDLokacija"]));
                        log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " Location name - " + lblstranicanaziv.Text);

                        if (IDTerminPredavanja != 0)
                        {
                            List<int> idTerminiPredavanja = new List<int>();
                            utility.getIDTerminePredavanja(IDTerminPredavanja, out idTerminiPredavanja);
                            log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " +"IDTerminiPredavanja.Count - " + idTerminiPredavanja.Count);
                            Session["Predavanja-idPonovnogPredavanja"] = idTerminiPredavanja;
                            //Session["Predavanja-idTerminPonovnogPredavanja"] = IDTerminPredavanja;
                            Session["Predavanje_idTerminPredavanja"] = IDTerminPredavanja;
                            Session["Predavanje_idLokacija"] = IDLokacija;

                            Session["Predavanja_VremeZapocinjanja"] = d1;

                            List<string> predmetiNaziv = new List<string>();
                            predmetiNaziv = getPredmetiNaziv(utility, predmetiList, idOsoba);
                            Session["Predavanja_predmetiNazivi"] = predmetiNaziv;

                            string tipPredavanja = utility.getTipPredavanja(idOsoba, IDTerminPredavanja, IDLokacija, IDLogPredavanja);
                            Session["Predavanje_tipPredavanja"] = tipPredavanja;

                            Response.Redirect("predavanje.aspx", false);
                        }
                        else
                        {

                            if (Session["login_Ime"] != null && Session["lbl_loginID"] != null)
                            {
                                lbl_Ime.Text = Session["login_Ime"].ToString();

                                HideDatepicker();
                            }
                            else
                            {
                                int IDLokacija1 = Convert.ToInt32(Session["login-IDLokacija"]);
                                string location = @"idLokacija=" + IDLokacija1;
                                string encryptedParameters2 = AuthenticatedEncryption.AuthenticatedEncryption.Encrypt(location, Constants.CryptKey, Constants.AuthKey);
                                encryptedParameters2 = encryptedParameters2.Replace("+", "%252b");
                                log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " encryptedParameters, when trying to logout is - " + encryptedParameters2);
                                Response.Redirect(string.Format("~/login.aspx?d={0}", encryptedParameters2), false);
                            }
                        }
                    }
                    else
                    {
                        if (Session["login_Ime"] != null && Session["lbl_loginID"] != null)
                        {
                            lbl_Ime.Text = Session["login_Ime"].ToString();

                            HideDatepicker();
                        }
                        else
                        {
                            int IDLokacija1 = Convert.ToInt32(Session["login-IDLokacija"]);
                            string location = @"idLokacija=" + IDLokacija1;
                            string encryptedParameters2 = AuthenticatedEncryption.AuthenticatedEncryption.Encrypt(location, Constants.CryptKey, Constants.AuthKey);
                            encryptedParameters2 = encryptedParameters2.Replace("+", "%252b");
                            log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " encryptedParameters, when trying to logout is - " + encryptedParameters2);
                            Response.Redirect(string.Format("~/login.aspx?d={0}", encryptedParameters2), false);
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("GreskaTerminPredavanja.aspx", false);
                log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Error on Index page. ");
            }

            
        }
        catch (Exception ex)
        {
            Response.Redirect("GreskaTerminPredavanja.aspx", false);
            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Error. " + ex);
        }     
    }

    protected void ChangeButtonVisibility(bool isVisible)
    {
        buttonStartVisible.Visible = isVisible;
        buttonEditVisible.Visible = !isVisible;
    }

    public List<string> getPredmetiNaziv(Utility utility, List<int> predmetiList, int idOsoba)
    {
        List<string> predmeti = new List<string>();

        try
        {
            foreach (var idpredmet in predmetiList)
            {
                string predmet = utility.getPredmetNaziv(idOsoba, idpredmet);
                log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " Predmeti: " + predmet);
                predmeti.Add(predmet);
            }
        }
        catch (Exception ex)
        {
            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Error in function getPredmetiNaziv. " + ex.Message);
        }
        return predmeti;
    }

    private void AvoidCashing()
    {
        Response.Cache.SetNoStore();
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
    }

    protected void HideDatepicker() {
        ScriptManager.RegisterStartupScript(this, GetType(), "Disable", "DisableCalendar();", true);
        txtdate.Text = DateTime.Now.ToString("dd.MM.yyyy");
        Session["Predavanja_txtdate"] = txtdate.Text;
        txtdate.BackColor = ColorTranslator.FromHtml(SetDarkGray);
        txtdate.ReadOnly = true;
        cvdate.Enabled = false;
    }

    protected void ShowDatepicker()
    {
        //call function pickdate() every time after PostBack in ASP.Net
        ScriptManager.RegisterStartupScript(this, GetType(), "", "pickdate();", true);
        //Avoid: jQuery DatePicker TextBox selected value Lost after PostBack in ASP.Net
        txtdate.Text = Request.Form[txtdate.UniqueID];
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        try
        {
            int Result = 0;

            Utility utility = new Utility();

            utility.logoutPredavanja(Convert.ToInt32(Session["login_IDLogPredavanja"]), out Result);
            log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " Logout Predavanja: " + ". IDLogPredavanja - " + Convert.ToInt32(Session["login_IDLogPredavanja"]) + " " + ". Rezultat - " + Result);

            if (Result != 0)
            {
                throw new Exception("Result from database is diferent from 0. Result is: " + Result);
            }
            else
            {
                Session["login_Ime"] = null;
                Session["lbl_loginID"] = null;
                Session["login_IDLogPredavanja"] = null;

                int IDLokacija = Convert.ToInt32(Session["login-IDLokacija"]);
                string location = @"idLokacija=" + IDLokacija;
                string encryptedParameters = AuthenticatedEncryption.AuthenticatedEncryption.Encrypt(location, Constants.CryptKey, Constants.AuthKey);
                encryptedParameters = encryptedParameters.Replace("+", "%252b");
                log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " encryptedParameters, when trying to logout is - " + encryptedParameters);
                Response.Redirect(string.Format("~/login.aspx?d={0}", encryptedParameters), false);
   
            }
        }
        catch (Exception ex)
        {
            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Error while trying to LOGOUT. " + ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
        }
    }

    protected void CheckBoxList1_ServerValidation(object source, ServerValidateEventArgs args)
    {
        try
        {
            string ErrorMessage = string.Empty;
            Utility utility = new Utility();
            // Create the list to store.
            List<string> CheckBoxList = new List<string>();

            List<int> BrojAkreditacijeList = new List<int>();
            // Loop through each item.
            foreach (ListItem item in CheckBoxList1.Items)
            {
                if (item.Selected)
                {
                    // If the item is selected, add the value to the list.
                    CheckBoxList.Add(item.Value);
                    int brojAkreditacije = utility.getBrojAkreditacije(item.ToString());
                    BrojAkreditacijeList.Add(brojAkreditacije);
                }
            }

            int sizeOfList = CheckBoxList.Count;
            args.IsValid = Utils.ValidateListSize(sizeOfList, BrojAkreditacijeList, out ErrorMessage);
            cvCheckbox.ErrorMessage = ErrorMessage;
        }
        catch (Exception)
        {
            cvCheckbox.ErrorMessage = string.Empty;
            args.IsValid = false;
        }
    }


    protected void Cvdate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            if (txtdate.Text != string.Empty)
            {
                DateTime datum = DateTime.ParseExact(txtdate.Text, "dd.MM.yyyy", null);
                log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " Datum je: " + datum);
                string ErrorMessage1 = string.Empty;

                args.IsValid = Utils.ValidateDate(datum, out ErrorMessage1);
                cvdate.ErrorMessage = ErrorMessage1;
            }
            else
            {
                if (txtdate.Text == string.Empty)
                {
                    cvdate.ErrorMessage = "Datum je obavezno polje. ";
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Greska prilikom validacije cvdate. " + ex.Message);
            txtdate.Text = string.Empty;
            cvdate.ErrorMessage = "Datum je u pogrešnom formatu. ";
            args.IsValid = false;
        }
    }


    protected void btnStart_Click(object sender, EventArgs e)
    {
        int Result = 0;
        try
        {
            Page.Validate("AddCustomValidatorToGroup");

            if (Page.IsValid)
            {
                int IDLokacija = Convert.ToInt32(Session["login-IDLokacija"]);

                DateTime d1 = DateTime.Now;
                Session["Predavanja_VremeZapocinjanja"] = d1;
                TimeSpan d1TimeSpan = DateTime.Now.TimeOfDay;
                TimeSpan d1TimeSpanTrimmed = new TimeSpan(d1TimeSpan.Hours, d1TimeSpan.Minutes, d1TimeSpan.Seconds);

                increasePredmetiList();

                Session["Predavanje_spanIzborKonacno.Text"] = ddlizbor.SelectedItem.Text;

                int IDPredavanje = 0;
                int IDTerminPredavanja = 0;
                
                string FinalDate = string.Empty;
                string FormatDateTime = "dd.mm.yyyy";
                string FormatToString = "yyyy-mm-dd";
                parceDateTime(Session["Predavanja_txtdate"].ToString(), FormatDateTime, FormatToString, out FinalDate);

                // Create the list to store Object Predavanje.
                List <Predavanje> IDPredavanjeList = new List<Predavanje>();

                Utility utility = new Utility();

                utility.zapocinjanjeTermina(IDLokacija, Convert.ToDateTime(FinalDate), d1TimeSpanTrimmed, Convert.ToInt32(Session["lbl_loginID"]), Convert.ToInt32(Session["login_IDLogPredavanja"]), out IDTerminPredavanja, out Result);

                log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " Zapocinjanje termina: " + " Sifra lokacije - " + IDLokacija + " " + ". Datum - " + FinalDate + " " + ". Pocetak - " + d1TimeSpanTrimmed + " " + ". idOsoba - " + Session["lbl_loginID"].ToString() + " " + ". IDLogPredavanja - " + (Convert.ToInt32(Session["login_IDLogPredavanja"])).ToString() + ". idTerminPredavanja - " + IDTerminPredavanja + " " + ". Rezultat - " + Result);

                if (Result == 1)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "erroralerttermin", "erroralertTermin();", true);
                    HideDatepicker();
                    log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " Greška prilikom unosa termina. Morate završiti predavanje koje je u toku. (Zatvorena stranica)");
                }
                else
                {
                    if (Result != 0)
                    {
                        throw new Exception("Result from database is diferent from 0. Result is: " + Result);
                    }
                    else
                    {
                        Session["Predavanje_idLokacija"] = IDLokacija;
                        Session["Predavanje_idTerminPredavanja"] = IDTerminPredavanja;
                    }

                    foreach (ListItem item in CheckBoxList1.Items)
                    {
                        if (item.Selected)
                        {
                            utility.zapocinjanjePredavanja(Convert.ToInt32(Session["Predavanje_idTerminPredavanja"]), Convert.ToInt32(item.Value), Convert.ToInt32(ddlizbor.SelectedValue), out IDPredavanje, out Result);
                            log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " Upisivanje predavanja : " + " IDTerminPredavanja - " + Convert.ToInt32(Session["Predavanje_idTerminPredavanja"]) + " " + ". idPredmet - " + item.Value + " " + ". idTipPredavanja - " + ddlizbor.SelectedValue + " " + ". idPredavanje - " + IDPredavanje + " " + ". Rezultat - " + Result);

                            IDPredavanjeList.Add(new Predavanje(IDPredavanje, Convert.ToInt32(item.Value)));
                        }
                    }

                    if (Result != 0)
                    {
                        throw new Exception("Result from database is diferent from 0. Result is: " + Result);
                    }
                    else
                    {
                        Session["Predavanja-IDPredavanjeList"] = IDPredavanjeList;
                        Response.Redirect("predavanje.aspx", false);
                    }
                }
            }
            else if (!Page.IsValid)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
                HideDatepicker();
            }
        }
        catch (Exception ex)
        {
            if (Result == 1)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralerttermin", "erroralertTermin();", true);
            }
            else {
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
            }
            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Button submit error. " + ex.Message);
            HideDatepicker();
        }
    }


    /*
    protected void ShowHideDiv(bool after)
    {
        beforeStarting.Visible = !after;
        afterStarting.Visible = after;
    }
    */
    protected void parceDateTime(string dateTime, string FormatDateTime, string FormatToString, out string dateTimeFinal)
    {
        dateTimeFinal = string.Empty;
        DateTime FinalDate1 = DateTime.ParseExact(dateTime, FormatDateTime, CultureInfo.InvariantCulture);
        string FinalDate = FinalDate1.ToString(FormatToString);
        log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " FinalDate to import: " + FinalDate);

        dateTimeFinal = FinalDate;
    }

    protected void txtdate_TextChanged(object sender, EventArgs e)
    {
        CheckIfChannelHasChanged2();
    }

    private void CheckIfChannelHasChanged2()
    {
        try
        {
            bool ReturnValidation = false;

            if (txtdate.Text != string.Empty)
            {
                DateTime datum = DateTime.ParseExact(txtdate.Text, "dd.MM.yyyy", null);
                log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " Datum je: " + datum);
                string ErrorMessage1 = string.Empty;

                ReturnValidation = Utils.ValidateDate(datum, out ErrorMessage1);
                errLabel2.Text = ErrorMessage1;
                if (!ReturnValidation)
                {
                    Session["Predavanja-event_controle"] = txtdate;
                }
            }
            else
            {
                if (txtdate.Text == string.Empty)
                {
                    errLabel2.Text = "Datum je obavezno polje. ";
                    Session["Predavanja-event_controle"] = txtdate;
                }
            }
            SetFocusOnTextbox();
        }
        catch (Exception ex)
        {
            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Greska prilikom validacije txtdate. " + ex.Message);
            txtdate.Text = string.Empty;
            errLabel2.Text = "Datum je u pogrešnom formatu. ";
            Session["Predavanja-event_controle"] = txtdate;
            SetFocusOnTextbox();
        }

    }


    protected void increasePredmetiList()
    {
        try
        {
            List<string> PredmetiList = new List<string>();
            HtmlGenericControl li;
            for (int i = 0; i < CheckBoxList1.Items.Count; i++)
            {
                if (CheckBoxList1.Items[i].Selected)
                {
                    li = new HtmlGenericControl("li");
                    li.Attributes.Add("class", "submit-label ml-2");
                    li.InnerText = CheckBoxList1.Items[i].Text;
                    PredmetiList.Add(li.InnerText);
                    //predmetiList.Controls.Add(li);
                }
            }
            Session["Predavanja_predmetiList"] = PredmetiList;
        }
        catch (Exception ex)
        {
            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Error in function increasePredmetiList. " + ex.Message);
            throw new Exception();
        }
    }

    ///---------------------------------IZBOR-----------------------------------------------------

    protected void Cvizbor_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            string ErrorMessage = string.Empty;
            string IDItem = "0";

            args.IsValid = Utils.ValidateIzbor(ddlizbor.SelectedValue, IDItem, out ErrorMessage);
            cvizbor.ErrorMessage = ErrorMessage;
        }
        catch (Exception)
        {
            cvizbor.ErrorMessage = string.Empty;
            args.IsValid = false;
        }
    }

    protected void ddlizbor_SelectedIndexChanged(object sender, EventArgs e)
    {
        int SelectedValue = Convert.ToInt32(ddlizbor.SelectedValue);
        if (SelectedValue != 0)
        {
            //ddlizbor.BorderColor = ColorTranslator.FromHtml(SetGray);
            Session["Predavanja-event_controle-DropDownList"] = ((DropDownList)sender);
            SetFocusOnDropDownLists();
        }
        HideDatepicker();
    }

    ///-------------------------------------------------------------------------------------------

    public void SetFocusOnTextbox()
    {
        try
        {
            if (Session["Predavanja-event_controle"] != null)
            {
                TextBox controle = (TextBox)Session["Predavanja-event_controle"];
                //controle.Focus();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "foc", "document.getElementById('" + controle.ClientID + "').focus();", true);
            }
        }
        catch (InvalidCastException inEx)
        {
            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Problem with setting focus on control. Error: " + inEx);
        }
    }

    public void SetFocusOnDropDownLists()
    {
        try
        {
            if (Session["Predavanja-event_controle-DropDownList"] != null)
            {
                DropDownList padajucalista = (DropDownList)Session["Predavanja-event_controle-DropDownList"];
                //padajucalista.Focus();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "foc", "document.getElementById('" + padajucalista.ClientID + "').focus();", true);
            }
        }
        catch (InvalidCastException inEx)
        {
            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Problem with setting focus on control. Error: " + inEx);
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        int Result = 0;
        int IDTerminPredavanjeIzmena = Convert.ToInt32(Session["Predavanje_idTerminPredavanjaIzmena"]);
        try
        {
            Utility utility = new Utility();

            utility.brisanjePredavanjaIzTermina(IDTerminPredavanjeIzmena, out Result);
            log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " brisanjePredavanjaIzTermina: " + " IDTerminPredavanjeIzmena - " + IDTerminPredavanjeIzmena + " " + ". Rezultat - " + Result);

            if (Result != 0)
            {
                throw new Exception("Result from database is diferent from 0. Result is: " + Result);
            }
            else
            {
                Page.Validate("AddCustomValidatorToGroup");

                if (Page.IsValid)
                {
                    int IDLokacija = Convert.ToInt32(Session["login-IDLokacija"]);

                    TimeSpan d1 = utility.getPocetakTermina(IDTerminPredavanjeIzmena);
                    Session["Predavanja_VremeZapocinjanja"] = d1;

                    int IDPredavanje = 0;

                    increasePredmetiList();

                    Session["Predavanje_spanIzborKonacno.Text"] = ddlizbor.SelectedItem.Text;

                    string FinalDate = string.Empty;
                    string FormatDateTime = "dd.mm.yyyy";
                    string FormatToString = "yyyy-mm-dd";
                    parceDateTime(Session["Predavanja_txtdate"].ToString(), FormatDateTime, FormatToString, out FinalDate);

                    // Create the list to store Object Predavanje.
                    List<Predavanje> IDPredavanjeList = new List<Predavanje>();

                    foreach (ListItem item in CheckBoxList1.Items)
                    {
                        if (item.Selected)
                        {
                            utility.zapocinjanjePredavanja(Convert.ToInt32(Session["Predavanje_idTerminPredavanjaIzmena"]), Convert.ToInt32(item.Value), Convert.ToInt32(ddlizbor.SelectedValue), out IDPredavanje, out Result);
                            log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " Upisivanje predavanja nakon izmene: " + " IDTerminPredavanja - " + Convert.ToInt32(Session["Predavanje_idTerminPredavanjaIzmena"]) + " " + ". idPredmet - " + item.Value + " " + ". idTipPredavanja - " + ddlizbor.SelectedValue + " " + ". idPredavanje - " + IDPredavanje + " " + ". Rezultat - " + Result);  
                            IDPredavanjeList.Add(new Predavanje(IDPredavanje, Convert.ToInt32(item.Value)));
                        }
                    }

                    if (Result != 0)
                    {
                        throw new Exception("Result from database is diferent from 0. Result is: " + Result);
                    }
                    else
                    {
                        Session["Predavanja-IDPredavanjeList"] = IDPredavanjeList;
                        Response.Redirect("predavanje.aspx", false);
                    }
                }
                else if (!Page.IsValid)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
                    HideDatepicker();
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Button submit error. " + ex.Message);
            HideDatepicker();
        }
    }



    protected void Export(object sender, EventArgs e)
    {
        try
        {
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string encoding;
            string extension;

            string path = System.Configuration.ConfigurationManager.AppSettings["PDFurl"].ToString();
            Guid id = Guid.NewGuid();
            string fileName = "Izvestaj-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-id-" + id.ToString() + ".pdf";
            log.Info(Session["login-ImeLokacijeZaLog"].ToString() + " - " + " FileName for import to DB: " + fileName);
            Utility utility = new Utility();

            utility.upisiNazivFajla(Convert.ToInt32(Session["idTerminPredavanja"]), fileName);
        
            dynamic savePath2 = (path + "\\" + fileName);

            //Export the RDLC Report to Byte Array.
            byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out contentType, out encoding, out extension, out streamIds, out warnings);

            FileStream file = default(FileStream);

            file = new FileStream(savePath2, FileMode.Create);
            file.Write(bytes, 0, bytes.Length);

            file.Close();
            file.Dispose();
        }
        catch (Exception ex)
        {
            log.Error(Session["login-ImeLokacijeZaLog"].ToString() + " - " + "Error in Export. " + ex);
            ScriptManager.RegisterStartupScript(this, GetType(), "erroralertFileName", "erroralertFileName();", true);
        }
    }

    protected override void OnPreRenderComplete(EventArgs e)
    {
        int idTerminPredavanja = Convert.ToInt32(Session["idTerminPredavanja"]);
        if (idTerminPredavanja != 0)
        {
            Export(null, null);
            Session["idTerminPredavanja"] = 0;
        }
        reportHiding.Visible = false;
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        Response.Redirect("pregledIzvestaji.aspx", false);
    }
}