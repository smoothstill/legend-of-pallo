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
/// Aliohjelma, joka näyttää pelin tiedot ruudulla (esim. elämien määrä, pommien määrä, jne.). HUD:iin on tallennettu myös
/// pelissa kerätyt pisteet.
/// </summary>
public static class HUD
{


    //Pisteet, jotka pelaajalla on jo kerättynä edellisitä kentistä.
    private static int vanhatPisteet;

    private static int hp;
    private static readonly int defaultHp = 1;
    private static List<Widget> healthBar;

    private static IntMeter pommiLaskuri;
    private static IntMeter avainLaskuri;
    private static IntMeter kolikkoLaskuri;
    private static IntMeter elamaLaskuri;
    private static IntMeter pisteLaskuri;
    private static IntMeter kenttaLaskuri;

    private static Widget sydamet;
    private static Widget pommit;
    private static Widget avaimet;
    private static Widget kolikot;
    private static Widget elamat;
    private static Widget score;
    private static Widget level;

    private static Timer kenttaAjastin;


    //Onko UI päällä vai pois
    public static bool Paalla { get; private set;}


    /// <summary>
    /// Jos pelaaja kuolee, poista pelaajan kentästä saadut pisteet.
    /// </summary>
    public static void ResetoiKentanPisteet()
    {
        kenttaAjastin.Reset();
        kenttaAjastin.Start();
        if (pisteLaskuri != null)
            Pisteet = vanhatPisteet;
    }


    /// <summary>
    /// Nollaa HUD:iin tallennetut pisteet.
    /// </summary>
    public static void NollaaPelinPisteet()
    {
        vanhatPisteet = 0;
        Pisteet = 0;
    }


    /// <summary>
    /// Kun kenttä on menty läpi, laskee pelaajan kentästä saadut pisteet.
    /// </summary>
    /// <param name="p">Pelaajan viite</param>
    public static void LaskeKentanPisteen(Pelaaja p)
    {
        //Laske pisteet pelaajan kolikoista, elämistä ja hp:sta
        Pisteet += 200 * p.Tavarat.TavaranMaara((int)TavaraTyypit.Raha);
        Pisteet += 400 * p.Tavarat.TavaranMaara((int)TavaraTyypit.Elama);
        Pisteet += 200 * p.CurrentHp;

        //Poistaa kaikki kolikot pelaajan inventorista. Ei tarvittava, koska kolikot eivät tallennu pelaajan tietoihin, jotka ladataan
        //seuraavassa kentässä.
        //p.Tavarat.KaytaTavara((int)TavaraTyypit.Raha, (byte)p.Tavarat.TavaranMaara((int)TavaraTyypit.Raha));

        //Anna pisteitä kentän suorituksen nopeudesta:
        const int maxPisteetAjasta = 2000;  //Maksimipisteet, jotka pelaaja voi saada nopeasta suorituksesta.
        const int minPisteetAjasta = 1000;  //Minimipisteet, jotka pelaaja saa joka kentän suorituksesta.
        const int maxAikavaatimus = 600;    //Aika sekuntteina, jonka jälkeen pelaaja saa vain minimipisteet kentästä.
        Pisteet += Math.Max((int)(maxPisteetAjasta - kenttaAjastin.CurrentTime * (maxPisteetAjasta / maxAikavaatimus)), minPisteetAjasta);
        kenttaAjastin.Stop();

        //Kentän pisteet tallennetaan "vanhatPisteet" muuttujaan, jolloin niitä ei voi enää ottaa pois pelaajalta.
        vanhatPisteet = Pisteet;
    }


    /// <summary>
    /// Pelaajan näkyvät pisteet näytöllä.
    /// </summary>
    public static int Pisteet
    {
        get
        {
            return pisteLaskuri.Value;
        }
        set
        {
            pisteLaskuri.SetValue(value >= 0 ? value : 0);
        }
    }


    /// <summary>
    /// Tämänhektisen kentän numero näytöllä.
    /// </summary>
    public static int Kentta
    {
        get
        {
            return kenttaLaskuri.Value;
        }
        set
        {
            kenttaLaskuri.SetValue(value >= 0 ? value : 0);
        }
    }


    /// <summary>
    /// Pommien lukumäärä näytöllä.
    /// </summary>
    public static int Pommit
    {
        get
        {
            return pommiLaskuri.Value;
        }
        set
        {
            pommiLaskuri.SetValue(value >= 0 ? value : 0);
        }
    }


    /// <summary>
    /// Avainten lukumäärä näytöllä.
    /// </summary>
    public static int Avaimet
    {
        get
        {
            return avainLaskuri.Value;
        }
        set
        {
            avainLaskuri.SetValue(value >= 0 ? value : 0);
        }
    }


    /// <summary>
    /// Elämien lukumäärä näytöllä.
    /// </summary>
    public static int Elamat
    {
        get
        {
            return elamaLaskuri.Value;
        }
        set
        {
            elamaLaskuri.SetValue(value >= 0 ? value : 0);
        }
    }


    /// <summary>
    /// Kolikoiden lukumäärä näytöllä.
    /// </summary>
    public static int Kolikot
    {
        get
        {
            return kolikkoLaskuri.Value;
        }
        set
        {
            kolikkoLaskuri.SetValue(value >= 0 ? value : 0);
        }
    }


    /// <summary>
    /// Asetetaan staattiset muuttujat.
    /// </summary>
    static HUD()
    {
        //Intmeterit
        pommiLaskuri = new IntMeter(0);
        avainLaskuri = new IntMeter(0);
        kolikkoLaskuri = new IntMeter(0);
        elamaLaskuri = new IntMeter(0);
        pisteLaskuri = new IntMeter(0);
        kenttaLaskuri = new IntMeter(0);

        //Widgetit
        sydamet = null;
        pommit = null;
        avaimet = null;
        kolikot = null;
        elamat = null;
        score = null;
        level = null;

        Paalla = false;

        healthBar = new List<Widget>();

        kenttaAjastin = new Timer();
        vanhatPisteet = 0;
        hp = defaultHp;
    }


    /// <summary>
    /// Piilottaa HUD:n ruudulta.
    /// </summary>
    public static void Piilota()
    {
        if (Paalla)
        {
            sydamet.Destroy();
            pommit.Destroy();
            avaimet.Destroy();
            kolikot.Destroy();
            elamat.Destroy();
            score.Destroy();
            level.Destroy();

            healthBar.Clear();

            Paalla = false;
        }
    }


    /// <summary>
    /// Päivittää UI:n koon näytöllä (esim. resoluution vaihtamisen takia voidaan suorittaa tämä aliohjelma)
    /// </summary>
    /// <param name="p"></param>
    public static void PaivitaKoko(LegendOfPallo p)
    {
        if (Paalla)
        {
            Piilota();
            Luo(p, hp);
        }
        else
            throw new Exception("UI hasn't been created yet.");
    }


    /// <summary>
    /// Luo ja asentaa HUD:n ruudulle.
    /// </summary>
    /// <param name="p">Pelin viite</param>
    /// <param name="sydantenMaara">Sydänten lukumäärä ruudulla</param>
    public static void Luo(LegendOfPallo p, int sydantenMaara)
    {

        Paalla = true;

        if (sydantenMaara >= 0) //Jos olio ei ole tuhoutumaton
        {
            hp = sydantenMaara;
        }


        HorizontalLayout asettelu = new HorizontalLayout();
        asettelu.Spacing = 8;
        asettelu.LeftPadding = 8;
        asettelu.RightPadding = 8;

        HorizontalLayout asetteluB = new HorizontalLayout();
        asetteluB.Spacing = 8;
        asetteluB.LeftPadding = 8;
        asetteluB.RightPadding = 48;


        sydamet = new Widget(asettelu);
        sydamet.Color = Color.Black;
        p.Add(sydamet);

        pommit = LuoTavaranWidget(asettelu, Animaatiot.BombPickupImage, pommiLaskuri);
        avaimet = LuoTavaranWidget(asettelu, Animaatiot.KeyPickupImage, avainLaskuri);
        kolikot = LuoTavaranWidget(asettelu, Animaatiot.CoinPickupImage, kolikkoLaskuri);
        elamat = LuoTavaranWidget(asettelu, Animaatiot.LivesImage, elamaLaskuri);
        score = LuoTavaranWidget(asetteluB, "Score: ", pisteLaskuri);
        level = LuoTavaranWidget(asetteluB, "Level: ", kenttaLaskuri);

        p.Add(pommit);
        p.Add(avaimet);
        p.Add(kolikot);
        p.Add(elamat);
        p.Add(score);
        p.Add(level);

        for (int i = 0; i < hp; ++i)
        {
            Widget sydanKuva = new Widget(32, 32);
            sydanKuva.Image = Animaatiot.HeartPickupImage;
            healthBar.Add(sydanKuva);
            sydamet.Add(healthBar[i]);
        }


        sydamet.Bottom = Game.Screen.Bottom + 8;
        sydamet.X = Game.Screen.Center.X;

        elamat.Left = Game.Screen.Left;
        elamat.Bottom = Game.Screen.Bottom + 8;
        pommit.Left = elamat.Right;
        pommit.Bottom = elamat.Bottom;
        avaimet.Left = pommit.Right;
        avaimet.Bottom = elamat.Bottom;
        kolikot.Left = avaimet.Right;
        kolikot.Bottom = elamat.Bottom;

        score.Right = Game.Screen.Right;
        score.Bottom = elamat.Bottom;
        level.Right = score.Left - 64;
        level.Bottom = elamat.Bottom;
    }


    /// <summary>
    /// Asettaa sydänten tilan ruudulla (onko sydämet menetetty vai ei)
    /// </summary>
    public static int Sydamet
    {
        set
        {
            for (int i = healthBar.Count - 1; i >= 0; --i)
            {
                if (i + 1 > value && value != -1)
                {
                    healthBar[i].Image = Animaatiot.HeartEmptyImage;
                }
                else
                {
                    healthBar[i].Image = Animaatiot.HeartPickupImage;
                }
            }
        }
    }


    /// <summary>
    /// Luo tavaralle widgetin ruudulle (kuvallinen).
    /// </summary>
    /// <param name="asettelu">Widgetin asettelu</param>
    /// <param name="kuva">Tavaran logo</param>
    /// <param name="laskuri">Tavaran laskurin viite</param>
    /// <returns>Muodostettu widgetti.</returns>
    private static Widget LuoTavaranWidget(HorizontalLayout asettelu, Image kuva, IntMeter laskuri)
    {
        Widget w = new Widget(asettelu);
        w.Color = Color.Black;

        Widget imageLogo = new Widget(32, 32);
        imageLogo.Image = kuva;
        w.Add(imageLogo);

        Label teksti = new Label();
        teksti.BindTo(laskuri);

        teksti.TextColor = Color.White;

        w.Add(teksti);

        return w;
    }


    /// <summary>
    /// Luo teksillisen widgetin ruudulle
    /// </summary>
    /// <param name="asettelu">Widgetin asettelu</param>
    /// <param name="laskurinTeksti">Widgetin laskurin otsikkoteksti</param>
    /// <param name="laskuri">Tavaran laskurin viite</param>
    /// <returns></returns>
    private static Widget LuoTavaranWidget(HorizontalLayout asettelu, string laskurinTeksti, IntMeter laskuri)
    {
        Widget w = new Widget(asettelu);
        w.Color = Color.Black;

        Label otsikko = new Label(laskurinTeksti);
        otsikko.TextColor = Color.White;
        w.Add(otsikko);

        Label teksti = new Label();
        teksti.BindTo(laskuri);

        teksti.TextColor = Color.White;

        w.Add(teksti);

        return w;
    }


}

