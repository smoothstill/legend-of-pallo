using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;



/// <summary>
/// Kanuuna on olio, joka ampuu automaattisesti halutulla tavalla.
/// </summary>
/// <typeparam name="T">Minkä tyyppisiä ammuksia kanuuna ampuu</typeparam>
public class Kanuuna<T> : Peliolio
{


    //Lippaan koko
    private short maxAmmukset;

    //Tämänhetkinen ammusten lukumäärä
    private short ammukset;

    //Lataukseen kuluva aika sekuntteina.
    private double latausAika;

    //Ammusten välillä kuluva aika sekunteina.
    private double aikavali;    

    //Ennakoiko kanuuna kohteensa paikkaa, vai ampuuko se vain paikkaan missä kohde on tällä hetkellä.
    public bool Ennakoi { get; set; }

    /// <summary>
    /// Aloitetaanko aina lipas tyhjänä (eli ase pitää ensin ladata)
    /// </summary>
    public bool AloitaTyhjana { get; set; }


    private Timer ajastin;
    private PhysicsObject kohde;
    private Vector suunta = new Vector(1, 0);


    /// <summary>
    /// Nollaa kanuunan ampumisajastimen ja käynnistää sen uudelleen.
    /// </summary>
    public void ResetoiKanuunanAjastin()
    {
        ajastin.Reset();
        ajastin.Start();
    }


    /// <summary>
    /// Asettaa kanuunalle ampumiskohteen. Kanuunalla voi olla joko AmpumisKohde tai AmpumisSuunta mutta
    /// ei molempia samaan aikaan.
    /// </summary>
    public PhysicsObject AmpumisKohde
    {
        get
        {
            return kohde;
        }
        set
        {
            suunta = Vector.Zero;
            kohde = value;
        }
    }


    /// <summary>
    /// Asettaa kanuunalle ampumissuunnan. Kanuunalla voi olla joko AmpumisKohde tai AmpumisSuunta mutta
    /// ei molempia samaan aikaan.
    /// </summary>
    public Vector AmpumisSuunta
    {
        get
        {
            return suunta;
        }
        set
        {
            kohde = null;
            suunta = value;
        }
    }

    //Maksimi etäisyys kohteeseen, jonka jälkeen kanuuna menee pois päältä. 
    public double MaxEtaisyysKohteeseen { get; set; }

    //Tätä käytetään asettamaan kanuunan ammus.
    private T ammus2;
    public delegate T AmmusMalli();
    public event AmmusMalli ammus;


    public Kanuuna(LegendOfPallo peli, double width, double height) : base(peli, width, height)
    {
        Ennakoi = false;
        AmpumisKohde = Peli.pelaaja;
        AloitaTyhjana = true;
        MaxEtaisyysKohteeseen = Double.PositiveInfinity;
        MakeStatic();
        aikavali = 1;
        maxAmmukset = 1;
        latausAika = 1;
        ammukset = 0;
        Hp = -1;

        Tag = "neutraali";

        ajastin = new Timer();
        ajastin.Timeout += KanuunaAmpuu;

    }


    /// <summary>
    /// Asetetaan kanuunaan tarvittavat tiedot, ja kanuuna laitetaan aktiiviseksi.
    /// </summary>
    /// <param name="ammustenAikavali">Kuinka pitkä aika kuluu ammusten välillä</param>
    /// <param name="latausAika">Kuinka pitkä aika kuluu kun ammuksen on loppu</param>
    /// <param name="ammustenMaara">Ammusten lukumäärä</param>
    public void AsetaKanuuna(double ammustenAikavali, double latausAika, short ammustenMaara)
    {

        aikavali = ammustenAikavali;
        this.latausAika = latausAika;
        maxAmmukset = ammustenMaara;

        if (AloitaTyhjana)
        {
            ajastin.Reset();
            ajastin.Interval = this.latausAika;
            ammukset = -1;
        }
        else
        {
            ajastin.Reset();
            ajastin.Interval = aikavali;
            ammukset = maxAmmukset;
        }

        ajastin.Start();

    }


    /// <summary>
    /// Aliohjelma, joka luo ammuksen ja "ampuu" sen halutulla tavalla.
    /// </summary>
    private void KanuunaAmpuu()
    {
        kohde = Peli.pelaaja;

        //Jos kohde tuhottu, palataan ja ei ammuta.
        if (kohde != null && kohde.IsDestroyed) return;

        //Jos kohde on kauempana kuin maxEtäisyys, palataan ja ei ammuta.
        if (kohde != null && (MaxEtaisyysKohteeseen < Vector.Distance( (Parent == null ? this : Parent).Position, kohde.Position)) && ajastin.Enabled)
        {
            //Laita kanuuna pois päältä kun kohde menee ulkopuolelle
            if (AloitaTyhjana)
            {
                ajastin.Reset();
                ajastin.Interval = this.latausAika;
                ammukset = -1;
            }
            else
            {
                ajastin.Reset();
                ajastin.Interval = aikavali;
                ammukset = maxAmmukset;
            }

            return;
        }

        if (!ajastin.Enabled) ajastin.Start();

        //Jos ei kohdetta eikä myöskään suuntaa, palataan.
        if (kohde == null && suunta == Vector.Zero) return;

        //Kun kanuuna on ladannut:
        if (ammukset == -1)
        {
            ajastin.Interval = aikavali;
            ammukset = maxAmmukset;
        }

        //Kun kanuunalla on vielä ammuksia jäljellä:
        if (ammukset > 0)
        {

            if (ammus != null) ammus2 = ammus.Invoke();

            if (ammus2 != null)
            {

                Ammus temp = ammus2 as Ammus;
                if (temp != null)
                {

                    //Asetetaan ammuksen ampujan viite
                    if (Parent != null)
                    {
                        temp.Ampuja = Parent;
                    }
                    else
                    {
                        temp.Ampuja = this;
                    }

                }

                if (kohde == null)
                {
                    Shoot(suunta, ammus2);
                }
                else
                {


                    if (ammus2 as Ammus != null && Ennakoi) //Yrittää ennakoida mihin kohde kulkee lineaarisesti ja ampuu sinne kohtaan.
                    {
                        Vector dist;

                        dist = new Vector(kohde.Position.X - this.AbsolutePosition.X, kohde.Position.Y - this.AbsolutePosition.Y);

                        Vector velocity2 = new Vector(0, 0);

                        double v1v2 = (kohde.Velocity.Magnitude * kohde.Velocity.Magnitude - (ammus2 as Ammus).Speed * (ammus2 as Ammus).Speed);
                        double dxdy = (dist.X * kohde.Velocity.X + dist.Y * kohde.Velocity.Y);
                        double time = (-dxdy - Math.Sqrt(dxdy * dxdy - (dist.Magnitude * dist.Magnitude) * v1v2)) / (v1v2);

                        if (!Double.IsNaN(time))
                        {
                            time *= 1.6;

                            velocity2.X = (dist.X + time * kohde.Velocity.X) / time;
                            velocity2.Y = (dist.Y + time * kohde.Velocity.Y) / time;

                            Shoot(velocity2, ammus2);
                        }

                    }
                    else //Jos kanuuna ei ennakoi, ammutaan kohteen paikkaan.
                    {
                        ShootAt(kohde.Position, ammus2);
                    }

                }

                //Ammusten määrä vähenee yhdellä.
                --ammukset;
            }


            //Jos lipas tyhjä, suoritetaan lataus.
            if (ammukset == 0)
            {

                ajastin.Reset();
                ajastin.Interval = latausAika;
                ajastin.Start();
                ammukset = -1;

            }

        }

    }


    public override void Destroy()
    {
        ajastin.Stop();
        base.Destroy();
    }


}

