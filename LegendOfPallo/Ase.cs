using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using Jypeli.Physics;


public class Ase : GameObject
{

    private int maxAmmukset;
    private int ammukset;
    private List<GameObject> oliot;
    private int kohdeIndeksi;
    private Timer ajastin;
    private double aika;
    private double maxEtaisyys;


    public Ase(LegendOfPallo peli, string kohde) : base(0,0)
    {

        oliot = peli.GetObjectsWithTag(kohde);

    }

    public void AsetaAse(double ampumisNopeus, double latausAika, int lippaanKoko, double maxEtaisyysKohteesta)
    {

        maxEtaisyys = maxEtaisyysKohteesta;
        ajastin = new Timer();
        ajastin.Interval = latausAika;
        ajastin.Timeout += AmmuAseella;
        aika = ampumisNopeus;
        maxAmmukset = lippaanKoko;
        ammukset = -1;

    }

    private void AmmuAseella()
    {
        //if (oliot.)


        if (ammukset == -1)
        {
            double temp = aika;
            aika = ajastin.Interval;
            ajastin.Interval = temp;


            ammukset = maxAmmukset;
        }


    }


    private int lahinOlio(List<GameObject> oliot)
    {

        double pieninEtaisyys = Double.PositiveInfinity;
        double uusiEtaisyys = 0;
        int pieninKohde = -1;

        for (int i = 0; i < oliot.Count; ++i)
        {

            if (!oliot[i].IsDestroyed)
            {
                uusiEtaisyys = Vector.Distance(oliot[i].Position, this.Position);
                if (uusiEtaisyys < pieninEtaisyys)
                {
                    pieninEtaisyys = uusiEtaisyys;
                    pieninKohde = i;
                }
            }

        }

        return pieninKohde;

    }

}

