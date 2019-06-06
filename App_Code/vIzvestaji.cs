using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class vIzvestaji
{
    public string OpisTermina { get; set; }
    public string Izvestaj { get; set; }

    public vIzvestaji(string opistermina, string izvestaj)
    {
        OpisTermina = opistermina;
        Izvestaj = izvestaj;
    }
}