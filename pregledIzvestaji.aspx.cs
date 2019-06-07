using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class pregledIzvestaji : System.Web.UI.Page
{
    //Lofg4Net declare log variable
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public List<vIzvestaji> IzvestajiVariables;

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
                lbl_Ime.Text = Session["login_Ime"].ToString();
                increaseReportList();
            }
        }
    }

    protected void increaseReportList()
    {
        try
        {
            HtmlGenericControl li;
            Utility utility = new Utility();
            IzvestajiVariables = utility.pronadjiPromenljiveIzvestaj(Convert.ToInt32(Session["lbl_loginID"]));
            string hrefPDFurl = System.Configuration.ConfigurationManager.AppSettings["hrefPDFurl"].ToString();

            foreach (var izvestajivariables in IzvestajiVariables)
            {
                li = new HtmlGenericControl("li");
                li.Attributes.Add("class", "submit-label ml-2");

                HtmlGenericControl anchor = new HtmlGenericControl("a");
                anchor.Attributes.Add("href", hrefPDFurl + izvestajivariables.Izvestaj);
                anchor.Attributes.Add("target", "_blank");
                anchor.InnerText = izvestajivariables.OpisTermina;
                //li.InnerText = '<asp:HyperLink ID="btnPrintRequest" runat="server" download="" target="_blank" class="btn-lg btn-default submit" style="margin-right: 8px; " Text="" />';

                li.Controls.Add(anchor);
                izvestajiList.Controls.Add(li);
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in function increaseReportList. " + ex.Message);
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
            int Result = 0;

            Utility utility = new Utility();

            utility.logoutPredavanja(Convert.ToInt32(Session["login_IDLogPredavanja"]), out Result);
            log.Debug("Logout Predavanja: " + ". IDLogPredavanja - " + Convert.ToInt32(Session["login_IDLogPredavanja"]) + " " + ". Rezultat - " + Result);

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
                log.Debug("encryptedParameters, when trying to logout is - " + encryptedParameters);
                Response.Redirect(string.Format("~/login.aspx?d={0}", encryptedParameters), false);

            }
        }
        catch (Exception ex)
        {
            log.Error("Error while trying to LOGOUT. " + ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "erroralert", "erroralert();", true);
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        string PageToRedirect = "index.aspx";
        int idTerminPredavanjaIzmena = 0;
        try
        {
            string idTerminPredavanjaIzmena1 = @"IDTerminPredavanja=" + idTerminPredavanjaIzmena;
            log.Debug("Back button. idTerminPredavanjaIzmena is - " + idTerminPredavanjaIzmena1);
            string editParameters = AuthenticatedEncryption.AuthenticatedEncryption.Encrypt(idTerminPredavanjaIzmena1, Constants.CryptKey, Constants.AuthKey);
            editParameters = editParameters.Replace("+", "%252b");
            log.Debug("Back button. Page to redirect. editParameters is - " + editParameters);
            Response.Redirect(string.Format("~/" + PageToRedirect + "?d={0}", editParameters), false);
        }
        catch (Exception ex)
        {
            log.Debug("Error while opening the Page: " + PageToRedirect + " . Error message: " + ex.Message);
            throw new Exception("Error while opening the Page: " + PageToRedirect + " . Error message: " + ex.Message);
        }
    }
}