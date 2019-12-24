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
/// Pelaajan luokka.
/// </summary>
public class Pelaaja : Peliolio
{

    private bool LukitseSuunta = false;

    public const double AmpumisVali = 0.25;
    public const double KranaatinHeittoVali = 1.0;

    private Vector pelaajanSuunta = new Vector(0.0, 1.0);
    private Timer AmpumisAjastin;
    private Timer KranaattiAjastin;


    /// <summary>
    /// Rakentaja, jossa pelaajan tiedot asetetaan.
    /// </summary>
    /// <param name="peli">Pelin viite.</param>
    /// <param name="width">Pelaajan korkeus.</param>
    /// <param name="height">Pelaajan leveys.</param>
    /// <param name="tiedot">Pelaajan tiedot.</param>
    public Pelaaja(LegendOfPallo peli, double width, double height, PelaajanTiedot tiedot) : base(peli, width, height)
    {

        Hp = tiedot.MaxHp;
        CurrentHp = Math.Min(Math.Max(1, tiedot.CurrentHp), tiedot.MaxHp);
        Tavarat.LisaaTavara((int)TavaraTyypit.Pommit, (byte)tiedot.Pommit);
        Tavarat.LisaaTavara((int)TavaraTyypit.Elama, (byte)tiedot.Elamat);
        Image = Animaatiot.pelaajanKuvaN;

        Width = 56;
        Height = 56;
        Shape = Shape.Ellipse;
        Color = Color.Red;
        Restitution = 0.0;
        CanRotate = false;
        MaxVelocity = 160;
        LinearDamping = 0.7;
        IsUpdated = true;
        Tag = "pelaaja";
        CollisionIgnoreGroup = (int)Types.Player;

        AmpumisAjastin = new Timer();
        AmpumisAjastin.Interval = AmpumisVali;

        KranaattiAjastin = new Timer();
        KranaattiAjastin.Interval = KranaatinHeittoVali;


    }


    /// <summary>
    /// Kun painetaan ampumisnappia, pelaaja ampuu halutun ammuksen
    /// </summary>
    /// <param name="p">pelaaja</param>
    public void PelaajaAmmu(Pelaaja p)
    {  


        if (p.AmpumisAjastin.Enabled || p.ShouldDie())
            return;
        else
        {
            Ammus ammus = Peli.AmmusLuoti1Malli();
            ammus.Speed = 700;
            ammus.Position = p.Position;
            ammus.Ampuja = this;


            p.Shoot(p.pelaajanSuunta, ammus);
            //p.AmpumisAjastin.Reset();
            p.AmpumisAjastin.Start(1);
        }
    }


    /// <summary>
    /// Kun painetaan pomminheittonappia, pelaaja heittää pommin.
    /// </summary>
    /// <param name="p">pelaaja</param>
    public void PelaajaHeitaKranaatti(Pelaaja p)
    {


        if (p.KranaattiAjastin.Enabled || p.ShouldDie())
            return;
        else
        {
            if (p.Tavarat.KaytaTavara((int)TavaraTyypit.Pommit))
            {
                Ammus ammus = Peli.AmmusPommi1Malli();
                ammus.Position = p.Position;
                ammus.Ampuja = this;
                //ammus.Rajahdys.AddShockwaveHandler("pelaaja", Harjoittelutyö.PaineaaltoOsuu);

                p.Shoot(p.pelaajanSuunta, ammus);
                //p.KranaattiAjastin.Reset();
                p.KranaattiAjastin.Start(1);

            }
        }




    }


    /// <summary>
    /// Kun painetaan pohjassa, pelaajan katsomissuunta pysyy lukittuna (voi jatkaa ampumista kyseiseen suuntaan).
    /// </summary>
    /// <param name="p">pelaaja</param>
    public void LukitseSuuntaPress(Pelaaja p)
    {

        p.LukitseSuunta = true;

    }


    public void LukitseSuuntaRelease(Pelaaja p)
    {

        p.LukitseSuunta = false;

    }


    /// <summary>
    /// Aliohjelma päivitetään jokaisella pelin framella.
    /// </summary>
    public override void Update(Time time)
    {

        //Tallentaa pelaajan suunnan muuttujaan, jos pelaaja ei ole paikallaan.
        if (Velocity.Magnitude >= 1.0 && !LukitseSuunta)
        {

            pelaajanSuunta = Velocity;

        }


        //Tästä eteenpäin: asetetaan pelaajan kuva pelaajan katsomissuunnan perusteella.
        Angle kulma1 = new Angle();
        Angle kulma2 = new Angle();
        Angle kulma3 = new Angle();
        Angle kulma4 = new Angle();
        Angle kulma5 = new Angle();
        Angle kulma6 = new Angle();
        Angle kulma7 = new Angle();
        Angle kulma8 = new Angle();


        kulma1.Degrees = 22.5;
        kulma2.Degrees = 67.5;
        kulma3.Degrees = 112.5;
        kulma4.Degrees = 157.5;
        kulma5.Degrees = -157.5;
        kulma6.Degrees = -112.5;
        kulma7.Degrees = -67.5;
        kulma8.Degrees = -22.5;


        if (pelaajanSuunta.Angle > kulma2 && pelaajanSuunta.Angle <= kulma3)    //Katsoo pohjoiseen
            Image = Animaatiot.pelaajanKuvaN;
        else if (pelaajanSuunta.Angle > kulma1 && pelaajanSuunta.Angle <= kulma2)   //Katsoo koiliseen
            Image = Animaatiot.pelaajanKuvaNE;
        else if (pelaajanSuunta.Angle > kulma8 && pelaajanSuunta.Angle <= kulma1)   //Katsoo itään
            Image = Animaatiot.pelaajanKuvaE;
        else if (pelaajanSuunta.Angle > kulma7 && pelaajanSuunta.Angle <= kulma8)   //Katsoo kaakkoon
            Image = Animaatiot.pelaajanKuvaSE;
        else if (pelaajanSuunta.Angle > kulma6 && pelaajanSuunta.Angle <= kulma7)   //Katsoo etelään
            Image = Animaatiot.pelaajanKuvaS;
        else if (pelaajanSuunta.Angle > kulma5 && pelaajanSuunta.Angle <= kulma6)   //Katsoo lounaaseen
            Image = Animaatiot.pelaajanKuvaSW;
        else if (pelaajanSuunta.Angle > kulma4 && pelaajanSuunta.Angle <= kulma5)   //Katsoo länteen
            Image = Animaatiot.LivesImage;
        else if (pelaajanSuunta.Angle > kulma3 && pelaajanSuunta.Angle <= kulma4)   //Katsoo luoteeseen
            Image = Animaatiot.pelaajanKuvaNW;
        else   //Katsoo länteen
            Image = Animaatiot.pelaajanKuvaW;



        base.Update(time);
    }


    /// <summary>
    /// Käydään läpi pelaajan tuhoutuessa/kuollessa.
    /// </summary>
    public override void Destroy()
    {
        if (Hp == -1) return;

        if (CurrentHp != 0) CurrentHp = 0;

        Peli.pelaajanTiedot.Elamat = Tavarat.TavaranMaara((int)TavaraTyypit.Elama);
        --Peli.pelaajanTiedot.Elamat;
        Tavarat.KaytaTavara((int)TavaraTyypit.Elama, 1);

        Peli.PrinttaaTekstiJaOdotaInput("You died.", Peli.JatkaKuolemanJalkeen);
        base.Destroy();
    }


}


