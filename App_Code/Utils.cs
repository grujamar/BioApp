using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for Utils
/// </summary>
public static class Utils
{
    //Lofg4Net declare log variable
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static bool ValidateUsername(string Username, out string ErrorMessage)
    {
        bool returnValue = true;
        ErrorMessage = string.Empty;

        if (Username == string.Empty)
        {
            ErrorMessage = "Korisničko ime je obavezno polje. ";
            returnValue = false;
        }
        else
        {
            returnValue = true;
        }

        return returnValue;
    }


    public static bool ValidateFactureNumber(string FactureNumber, out string ErrorMessage)
    {
        bool returnValue = true;
        ErrorMessage = string.Empty;

        if (FactureNumber == string.Empty){
            ErrorMessage = "Broj fakture je obavezno polje. ";
            returnValue = false;
        }else if (!allowLettersNumbersMinusSlashSpace(FactureNumber)){
            ErrorMessage = "Moguće je uneti samo slova, cifre, minus, kosu crtu i razmak. ";
            returnValue = false;
        }else{
            returnValue = true;
        }

        return returnValue;
    }

    public static bool allowLettersNumbersMinusSlashSpace(string InputString)
    {
        try
        {
            Regex regex = new Regex(@"^([a-zA-Z0-9\/ČĆĐŠŽžšđćč -]*)$");
            Match match = regex.Match(InputString);
            if (match.Success)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static bool ValidatePrice(string Price, out string ErrorMessage)
    {
        bool returnValue = true;
        ErrorMessage = string.Empty;

        if (Price == string.Empty)
        {
            ErrorMessage = "Iznos je obavezno polje. ";
            returnValue = false;
        }
        else if (!allowNumbersDotComma(Price))
        {
            ErrorMessage = "Moguće je uneti samo cifre, tačku i zarez. ";
            returnValue = false;
        }
        else
        {
            returnValue = true;
        }

        return returnValue;
    }

    public static bool allowNumbersDotComma(string InputString)
    {
        try
        {
            Regex regex = new Regex(@"^([0-9.,]*)$");
            Match match = regex.Match(InputString);
            if (match.Success)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static bool ValidateDate(DateTime datum, out string ErrorMessage1)
    {
        bool returnValue = true;
        ErrorMessage1 = string.Empty;

        if (datum.ToString() == string.Empty){
            ErrorMessage1 = "Datum je obavezno polje. ";
            returnValue = false;
        }else if (datum > DateTime.ParseExact(DateTime.Now.ToString("dd.MM.yyy"), "dd.MM.yyyy", null)){
            log.Debug("DateTimeNow je: " + DateTime.ParseExact(DateTime.Now.ToString("dd.MM.yyy"), "dd.MM.yyyy", null));
            ErrorMessage1 = "Datum mora biti manji od današnjeg. ";
            returnValue = false;
        }else{
            returnValue = true;
        }            

        return returnValue;
    }

    public static bool ValidateOrganisation(string SelectedValue, string IDItem, out string ErrorMessage)
    {
        bool returnValue = true;
        ErrorMessage = string.Empty;

        if (SelectedValue == IDItem){
            ErrorMessage = "Organizacija je obavezno polje. ";
            returnValue = false;
        }else{
            returnValue = true;
        }

        return returnValue;
    }

    public static bool ValidateTypeOdPayment(string SelectedValue, string IDItem, out string ErrorMessage)
    {
        bool returnValue = true;
        ErrorMessage = string.Empty;

        if (SelectedValue == IDItem)
        {
            ErrorMessage = "Tip plaćanja je obavezno polje. ";
            returnValue = false;
        }
        else
        {
            returnValue = true;
        }

        return returnValue;
    }

    public static bool ValidateTypeOdService(string SelectedValue, string IDItem, out string ErrorMessage)
    {
        bool returnValue = true;
        ErrorMessage = string.Empty;

        if (SelectedValue == IDItem)
        {
            ErrorMessage = "Usluga je obavezno polje. ";
            returnValue = false;
        }
        else
        {
            returnValue = true;
        }

        return returnValue;
    }

    public static bool ValidateCashier(string SelectedValue, string IDItem, out string ErrorMessage)
    {
        bool returnValue = true;
        ErrorMessage = string.Empty;

        if (SelectedValue == IDItem)
        {
            ErrorMessage = "Osoba je obavezno polje. ";
            returnValue = false;
        }
        else
        {
            returnValue = true;
        }

        return returnValue;
    }

    public static bool ValidateOrganizationTxt(string OrganizationTxt, out string ErrorMessage)
    {
        bool returnValue = true;
        ErrorMessage = string.Empty;

        if (OrganizationTxt == string.Empty)
        {
            ErrorMessage = "Upišite organizaciju. ";
            returnValue = false;
        }
        else
        {
            returnValue = true;
        }

        return returnValue;
    }


}