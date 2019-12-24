using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;



enum Types
{
    Player,
    Wall,
    Projectile
}


/// <summary>
/// Pelaajan asetukset, jotka säilyvät pelaajalla seuraavaan kenttään.
/// </summary>
public struct PelaajanTiedot
{
    public int Elamat { get; set; }
    public int MaxHp { get; set; }
    public int CurrentHp { get; set; }
    public int Pommit { get; set; }
}


///@author Mika Lammi
///@version 8.4.2019
/// <summary>
/// Ohjelmointi 1 Kurssin Harjoitustyö: Peli
/// Nimi: The Legend of Pallo
/// </summary>
public partial class LegendOfPallo : PhysicsGame
{

    public static readonly double PIXEL_SIZE = 4;
    public static readonly double TILE_SIZE = 64;

    private MessageDisplay adisplay;

    public Pelaaja pelaaja;
    public PelaajanTiedot pelaajanTiedot;
    public ScoreList topLista;

    private double IkkunanLeveys;
    private double IkkunanKorkeus;
    private const int CameraPixelsHeight = 640;

    //Tämänhetkisen kentän numero
    public int CurrentLevel { get; private set; }


    /// <summary>
    /// Aliohjelma, joka käydään läpi ensimmäisenä pelin käynnistyessä.
    /// </summary>
    public override void Begin()
    {

        //Tallennetaan ikkunan leveys ja korkeus muuttujiin.
        IkkunanLeveys = Screen.WidthSafe;
        IkkunanKorkeus = Screen.HeightSafe;

        //Asetetaan alussa ikkunamuotoon.
        IsFullScreen = true;

        Window.AllowAltF4 = true;
        Window.Title = "tLoP";

        Window.AllowUserResizing = false;
        Camera.StayInLevel = true;

        //Pikseligrafiikat
        SmoothTextures = false;

        adisplay = new MessageDisplay();
        adisplay.TextColor = Color.White;
        adisplay.BackgroundColor = Color.Black;
        adisplay.MessageTime = TimeSpan.FromSeconds(5);
        Add(adisplay);

        //Luo ja hae top pisteet lista muistista
        topLista = new ScoreList(10, false, 0);
        topLista = DataStorage.TryLoad<ScoreList>(topLista, "pisteet.xml");

        //Luo pelin menu
        LataaMenunTausta();
        Alkuvalikko();

    }


    /// <summary>
    /// Asetetaan tarvittavat asetukset pelin alussa.
    /// </summary>
    private void Alkuasetukset()
    {
        pelaajanTiedot = new PelaajanTiedot();
        pelaajanTiedot.CurrentHp = 6;
        pelaajanTiedot.MaxHp = 6;
        pelaajanTiedot.Pommit = 0;
        pelaajanTiedot.Elamat = 10;

        CurrentLevel = 1;
    }


    /// <summary>
    /// Päivitysaliohjelma, joka suoritetaan pelin jokaisella framella.
    /// </summary>
    protected override void Update(Time time)
    {

        Camera.Follow(pelaaja);

        if (pelaaja != null)
        {
            HUD.Sydamet = pelaaja.Hp == -1 ? pelaaja.Hp : HUD.Sydamet = pelaaja.CurrentHp;
            HUD.Pommit = pelaaja.Tavarat.TavaranMaara((int)TavaraTyypit.Pommit);
            HUD.Kolikot = pelaaja.Tavarat.TavaranMaara((int)TavaraTyypit.Raha);
            HUD.Elamat = pelaaja.Tavarat.TavaranMaara((int)TavaraTyypit.Elama);
            HUD.Avaimet = pelaaja.Tavarat.TavaranMaara((int)TavaraTyypit.Avaimet);
            HUD.Kentta = CurrentLevel;
        }
        base.Update(time);
    }


    /// <summary>
    /// Vaihda fullscreenin ja ei-fullscreenin välillä. Ei toimi valikoissa.
    /// </summary>
    private void VaihdaFullscreeniin()
    {
        IsFullScreen = IsFullScreen ? false : true;
        if (!IsFullScreen)
        {
            SetWindowSize((int)IkkunanLeveys, (int)IkkunanKorkeus);
            //Camera.ZoomToLevel(0);
        }
        Camera.ZoomTo(new Vector(0, 0), new Vector(CameraPixelsHeight, CameraPixelsHeight));
        HUD.PaivitaKoko(this);
    }


    /// <summary>
    /// Asetetaan pelin kontrollit
    /// </summary>
    private void AsetaOhjaimet()
    {

        //Action doNothing = () => { };


        if (pelaaja != null)
        {

            Keyboard.ListenArrows(ButtonState.Down, v => pelaaja.Hit(v * 200), "Liiku");
            Keyboard.Listen(Key.Space, ButtonState.Down, pelaaja.PelaajaAmmu, "Ammu", pelaaja);
            Keyboard.Listen(Key.Z, ButtonState.Down, pelaaja.PelaajaHeitaKranaatti, "Heitä pommi", pelaaja);
            Keyboard.Listen(Key.LeftControl, ButtonState.Pressed, pelaaja.LukitseSuuntaPress, "Lukitse suunta", pelaaja);
            Keyboard.Listen(Key.LeftControl, ButtonState.Released, pelaaja.LukitseSuuntaRelease, null, pelaaja);

        }
        //else
        //{
            //Keyboard.ListenArrows(ButtonState.Down, v => new Vector(0,0), "Liiku");
            //Keyboard.Listen(Key.Space, ButtonState.Down, doNothing, "Ammu");
            //Keyboard.Listen(Key.Z, ButtonState.Down, doNothing, "Heitä pommi");
            //Keyboard.Listen(Key.LeftControl, ButtonState.Pressed, doNothing, "Lukitse suunta");
            //Keyboard.Listen(Key.LeftControl, ButtonState.Released, doNothing, null);
        //}
        

        if (HUD.Paalla)
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, ValikkoKeskenPelin, "Menu");
        else
            Keyboard.Listen(Key.Escape, ButtonState.Pressed, Alkuvalikko, "Menu");


        Keyboard.Listen(Key.F5, ButtonState.Pressed, VaihdaFullscreeniin, "Fullscreen");
        Keyboard.Listen(Key.Space, ButtonState.Pressed, TapahtumaTekstinJalkeen, null);
    }


    /// <summary>
    /// Aliohjelma suorittaa _tapahtumaTekstinJalkeen aliohjelman, joka on määritelty "PrinttaaTekstiJaOdotaInput" -aliohjelmassa.
    /// </summary>
    private void TapahtumaTekstinJalkeen()
    {

        _tapahtumaTekstinJalkeen?.Invoke(); //Jos _tapahtumaTekstinJalkeen ei ole null, suoritetaan se.
        _tapahtumaTekstinJalkeen = null;

    }


    /// <summary>
    /// Aliohjelma, jota suorittaessa kenttä ladataan uudelleen jos pelaaja on kuollut.
    /// </summary>
    public void JatkaKuolemanJalkeen()
    {
        if (pelaaja == null) return;

        if (pelaaja.CurrentHp <= 0)
        {
            HUD.ResetoiKentanPisteet();
            LataaKentta();
        }

    }


    /// <summary>
    /// Aliohjelma, joka lataa ensimmäisen kentän pelistä. 
    /// </summary>
    public void EnsimmaiseenKenttaan()
    {

        Alkuasetukset();
        LataaKentta();

    }


}
