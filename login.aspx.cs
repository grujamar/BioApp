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
        try
        {
            if (Page.IsValid)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BioConnectionString"].ToString());
                con.Open();

                string tableName = "vNastavnoOsoblje";
                SqlCommand cmd = new SqlCommand("select * from " + tableName + " WHERE Username=@Username", con);
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    string Ime = row[1].ToString();
                    string Prezime = row[2].ToString();
                    log.Info("successfully LOGIN with username: " + txtUsername.Text + ", name: " + Ime + " and password: " + Prezime);
                    Session["login_Ime"] = Ime;
                    Session["login_Prezime"] = Prezime;
                }
                if (dt.Rows.Count > 0)
                {
                    Response.Redirect("index.aspx", false); // this will tell .NET framework not to stop the execution of the current thread and hence the error will be resolved.
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Pogrešno Korisničko Ime! Pokušajte ponovo!')</script>");
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
            throw new Exception("Error while trying to LOGIN. " + ex.Message);
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