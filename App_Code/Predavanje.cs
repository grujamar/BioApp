using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Predavanje
/// </summary>
public class Predavanje
{
    private int IDPredavanje;
    private int IDPredmet;

    public Predavanje(int idpredavanje, int idpredmet)
    {
        this.IDPredavanje = idpredavanje;
        this.IDPredmet = idpredmet;
    }

    public int Predavanje1
    {
        get { return IDPredavanje; }
        set { IDPredavanje = value; }
    }

    public int Predmet1
    {
        get { return IDPredmet; }
        set { IDPredmet = value; }
    }
}