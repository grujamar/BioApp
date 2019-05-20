using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class predavanje : System.Web.UI.Page
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

        if (!Page.IsPostBack)
        {
            if (Session["login_Ime"] != null && Session["lbl_loginID"] != null)
            {
                if (Session["Predavanja_predmetiNazivi"] != null)
                {
                    spanIzborKonacno.Text = Session["Predavanje_tipPredavanja"].ToString();
                    increasePredmetiNazivi();
                }
                else
                {
                    spanIzborKonacno.Text = Session["Predavanje_spanIzborKonacno.Text"].ToString();
                    increasePredmetiList();
                }
                lbl_Ime.Text = Session["login_Ime"].ToString();
                GridView1.DataBind();
                btnLogout.Enabled = false;
                TimeSpan trimmedSpan1;
                TimeSpanNow(out trimmedSpan1);
                lblstranicanaziv.Text = utility.getImeLokacije(Convert.ToInt32(Session["login-IDLokacija"]));
                log.Debug("Location name - " + lblstranicanaziv.Text);
            }
        }
    }

    
    protected void increasePredmetiList()
    {
        try
        {
            HtmlGenericControl li;
            List<string> PredmetiList;
            PredmetiList = (List<string>)Session["Predavanja_predmetiList"];
            for (int i = 0; i < PredmetiList.Count; i++)
            {
                li = new HtmlGenericControl("li");
                li.Attributes.Add("class", "submit-label ml-2");
                li.InnerText = PredmetiList[i];
                predmetiList.Controls.Add(li);
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in function increasePredmetiList. " + ex.Message);
            throw new Exception();
        }
    }

    protected void increasePredmetiNazivi()
    {
        try
        {
            HtmlGenericControl li;
            List<string> PredmetiList;
            PredmetiList = (List<string>)Session["Predavanja_predmetiNazivi"];
            for (int i = 0; i < PredmetiList.Count; i++)
            {
                li = new HtmlGenericControl("li");
                li.Attributes.Add("class", "submit-label ml-2");
                li.InnerText = PredmetiList[i];
                predmetiList.Controls.Add(li);
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in function increasePredmetiNazivi. " + ex.Message);
            throw new Exception();
        }
    }

    private void AvoidCashing()
    {
        Response.Cache.SetNoStore();
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        try
        {
            int IDLogPredavanja = 0;
            int Result = 0;

            Utility utility = new Utility();

            utility.logoutPredavanja(Convert.ToInt32(Session["login_IDLogPredavanja"]), out Result);
            log.Debug("Logout Predavanja: " + ". IDLogPredavanja - " + IDLogPredavanja + " " + ". Rezultat - " + Result);

            if (Result != 0)
            {
                throw new Exception("Result from database is diferent from 0. Result is: " + Result);
            }
            else
            {
                Session["login_Ime"] = null;
                Session["login_Prezime"] = null;
                Session["lbl_loginID"] = null;
                Session["login_IDLogPredavanja"] = null;

                Response.Redirect("Login.aspx", false);
            }
        }
        catch (Exception ex)
        {
            log.Error("Error while trying to LOGOUT. " + ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
        }
    }

    protected void btnEnd_Click(object sender, EventArgs e)
    {
        List<int> idTerminiPredavanja;
        List<Predavanje> IDPredavanjeList;

        try
        {
            TimeSpan d1TimeSpan = DateTime.Now.TimeOfDay;
            TimeSpan d1TimeSpanTrimmed = new TimeSpan(d1TimeSpan.Hours, d1TimeSpan.Minutes, d1TimeSpan.Seconds);

            DateTime d2 = DateTime.Now;
            TimeSpan trimmedSpan1;
            TimeSpanNow(out trimmedSpan1);
            decimal procenatZaPriznavanje = 50.00m;
            int Result = 0;

            Utility utility = new Utility();

            if (Session["Predavanja-idPonovnogPredavanja"] != null)
            {
                idTerminiPredavanja = new List<int>();
                idTerminiPredavanja = (List<int>)Session["Predavanja-idPonovnogPredavanja"];

                foreach (var item in idTerminiPredavanja)
                {
                    utility.zavrsavanjePredavanja(item, d1TimeSpanTrimmed, procenatZaPriznavanje, out Result);
                    log.Debug("Zavrsavanje predavanja : " + " IdPredavanje - " + item + " " + ". Kraj - " + d1TimeSpanTrimmed + " " + ". ProcenatZaPriznavanje - " + procenatZaPriznavanje + " " + ". Rezultat - " + Result);
                    if (Result != 0)
                    {
                        throw new Exception("Result from database is diferent from 0. Result is: " + Result);
                    }
                }
            }
            else
            {
                IDPredavanjeList = new List<Predavanje>();
                IDPredavanjeList = (List<Predavanje>)Session["Predavanja-IDPredavanjeList"];

                foreach (var item in IDPredavanjeList)
                {
                    utility.zavrsavanjePredavanja(item.Predavanje1, d1TimeSpanTrimmed, procenatZaPriznavanje, out Result);
                    log.Debug("Zavrsavanje predavanja : " + " IdPredavanje - " + item.Predavanje1 + " " + ". Kraj - " + d1TimeSpanTrimmed + " " + ". ProcenatZaPriznavanje - " + procenatZaPriznavanje + " " + ". Rezultat - " + Result);
                    if (Result != 0)
                    {
                        throw new Exception("Result from database is diferent from 0. Result is: " + Result);
                    }
                }
            }

            utility.zavrsavanjeTermina(Convert.ToInt32(Session["Predavanje_idTerminPredavanja"]), d1TimeSpanTrimmed, out Result);
            log.Debug("Zavrsavanje termina : " + " IdTerminPredavanja - " + Convert.ToInt32(Session["Predavanje_idTerminPredavanja"]) + " " + ". Kraj - " + d1TimeSpanTrimmed + " " + ". Rezultat - " + Result);
            if (Result != 0)
            {
                throw new Exception("Result from database is diferent from 0. Result is: " + Result);
            }
            else
            {
                btnLogout.Enabled = true;
                string PageToRedirect = "index.aspx";
                int idTerminPredavanjaIzmena = 0;
                try
                {
                    string idTerminPredavanjaIzmena1 = @"IDTerminPredavanja=" + idTerminPredavanjaIzmena;
                    log.Debug("idTerminPredavanjaIzmena is - " + idTerminPredavanjaIzmena1);
                    string editParameters = AuthenticatedEncryption.AuthenticatedEncryption.Encrypt(idTerminPredavanjaIzmena1, Constants.CryptKey, Constants.AuthKey);
                    editParameters = editParameters.Replace("+", "%252b");
                    log.Debug("Page to redirect. editParameters is - " + editParameters);
                    Response.Redirect(string.Format("~/" + PageToRedirect + "?d={0}", editParameters), false);
                }
                catch (Exception ex)
                {
                    log.Debug("Error while opening the Page: " + PageToRedirect + " . Error message: " + ex.Message);
                    throw new Exception("Error while opening the Page: " + PageToRedirect + " . Error message: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            if (Session["Predavanja-idPonovnogPredavanja"] != null)
            {
                increasePredmetiNazivi();
            }
            else {
                increasePredmetiList();
            }
            log.Error("End button submit error. " + ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        TimeSpan trimmedSpan1;
        TimeSpanNow(out trimmedSpan1);
    }

    protected void TimeSpanNow(out TimeSpan trimmedSpan1)
    {

        DateTime d2 = DateTime.Now;

        TimeSpan span1 = d2 - Convert.ToDateTime(Session["Predavanja_VremeZapocinjanja"].ToString());
        trimmedSpan1 = new TimeSpan(span1.Hours, span1.Minutes, span1.Seconds);

        lblTime.Text = trimmedSpan1.ToString();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            bool DaLiSePrijavaOdnosiNaTrenutniTermin = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "DaLiSePrijavaOdnosiNaTrenutniTermin"));
            string TipStatusa = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "TipStatusa"));

            if (TipStatusa == "Poništen ručno")
            {
                Button btnButton = (Button)e.Row.FindControl("btnClear");
                btnButton.Text = "Vrati prisustvo";
            }
            else { 
                Button btnButton = (Button)e.Row.FindControl("btnClear");
                btnButton.Text = "Poništi prisustvo";
            }

            if (!DaLiSePrijavaOdnosiNaTrenutniTermin)
            {
                e.Row.BackColor = ColorTranslator.FromHtml(SetLightGray);
                e.Row.Attributes["onmouseover"] = "onMouseOver('" + (e.Row.RowIndex + 1) + "')";
                e.Row.Attributes["onmouseout"] = "onMouseOut('" + (e.Row.RowIndex + 1) + "')";
            }
            else
            {
                e.Row.BackColor = ColorTranslator.FromHtml(SetWhite);
                e.Row.Attributes["onmouseover"] = "onMouseOver('" + (e.Row.RowIndex + 1) + "')";
                e.Row.Attributes["onmouseout"] = "onMouseOutWhite('" + (e.Row.RowIndex + 1) + "')";
            }

            e.Row.Cells[6].BackColor = ColorTranslator.FromHtml(Convert.ToString((DataBinder.Eval(e.Row.DataItem, "Boja"))));
        }
    }

    protected void Timer2_Tick(object sender, EventArgs e)
    {
        GridView1.DataBind();
        GridView gw = (GridView)UpdatePanel3.FindControl("GridView2");
        gw.DataBind();
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "TooltipImages", "TooltipImages()", true);
    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            bool DaLiSePrijavaOdnosiNaTrenutniTermin = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "DaLiSePrijavaOdnosiNaTrenutniTermin"));

            if (!DaLiSePrijavaOdnosiNaTrenutniTermin)
            {
                e.Row.BackColor = ColorTranslator.FromHtml(SetLightGray);
            }
            else
            {
                e.Row.BackColor = ColorTranslator.FromHtml(SetWhite);
            }


            e.Row.Cells[2].BackColor = ColorTranslator.FromHtml(Convert.ToString((DataBinder.Eval(e.Row.DataItem, "Boja"))));
        }
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("DeleteProfile"))
            {
                int rowno = Convert.ToInt32(e.CommandArgument);
                int IDDnevniStatusOsobeNaLokaciji = Convert.ToInt32(GridView1.DataKeys[rowno]["IDDnevniStatusOsobeNaLokaciji"]);
                int Result = 0;

                Utility utility = new Utility();
                utility.ponistavanjePrisustva(IDDnevniStatusOsobeNaLokaciji, out Result);
                log.Debug("Ponistavanje prisustva : " + " RowNo - " + rowno + " IDDnevniStatusOsobeNaLokaciji - " + IDDnevniStatusOsobeNaLokaciji + " " + ". Rezultat - " + Result);
                if (Result != 0)
                {
                    throw new Exception("Result from database is diferent from 0. Result is: " + Result);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error("Clear GW button error. " + ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
        }
    }


    protected void btnAddIndex_Click(object sender, EventArgs e)
    {
        try
        {
            Page.Validate("AddCustomValidatorToGroup");

            if (Page.IsValid)
            {
                Utility utility = new Utility();

                DateTime dateTime = DateTime.Now;
                DateTime dateOnly = dateTime.Date;
                TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
                TimeSpan timeOnly = new TimeSpan(timeOfDay.Hours, timeOfDay.Minutes, timeOfDay.Seconds);

                int Result = 0;

                utility.upisivanjePrisustvaRucno(txtIndexNumber.Text, Convert.ToInt32(Session["login-IDLokacija"]), dateOnly, timeOnly, out Result);
                log.Debug("upisivanjePrisustvaRucno : " + " BrojIndeksa - " + txtIndexNumber.Text + " " + ". IDLokacije - " + Convert.ToInt32(Session["login-IDLokacija"]) + " " + ". Datum - " + dateOnly + " " + ". Vreme - " + timeOnly + " " + ". Rezultat - " + Result);
                if (Result != 0)
                {
                    throw new Exception("Result from database is diferent from 0. Result is: " + Result);
                }
                else {
                    txtIndexNumber.Text = string.Empty;
                }
            }
            else if (!Page.IsValid)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
            }
        }
        catch (Exception ex)
        {
            log.Error("btnAddIndex submit error. " + ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
        }
    }

    protected void cvIndexNumber_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            string ErrorMessage = string.Empty;
            args.IsValid = Utils.ValidateIndexNumber(txtIndexNumber.Text, out ErrorMessage);
            cvIndexNumber.ErrorMessage = ErrorMessage;
        }
        catch (Exception)
        {
            cvIndexNumber.ErrorMessage = string.Empty;
            args.IsValid = false;
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string PageToRedirect = "index.aspx";
        int idTerminPredavanjaIzmena = Convert.ToInt32(Session["Predavanje_idTerminPredavanja"]);
        try
        {
            string idTerminPredavanjaIzmena1 = @"IDTerminPredavanja=" + idTerminPredavanjaIzmena;
            log.Debug("Edit predavanje started. idTerminPredavanjaIzmena is - " + idTerminPredavanjaIzmena1);
            string editParameters = AuthenticatedEncryption.AuthenticatedEncryption.Encrypt(idTerminPredavanjaIzmena1, Constants.CryptKey, Constants.AuthKey);
            editParameters = editParameters.Replace("+", "%252b");
            log.Debug("Page to redirect. editParameters is - " + editParameters);
            Response.Redirect(string.Format("~/" + PageToRedirect + "?d={0}", editParameters), false);
        }
        catch (Exception ex)
        {
            log.Debug("Error while opening the Page: " + PageToRedirect + " . Error message: " + ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "errorOpeningPage", "errorOpeningPage();", true);
        }
    }

}