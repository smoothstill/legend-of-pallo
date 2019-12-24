using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jypeli;


/// <summary>
/// Pelin loppuvihollinen, Splörtsi.
/// </summary>
public class VihollinenSplortsi : Peliolio
{


    private LabyrinthWandererBrain aivot;
    private Kanuuna<Ammus> kanuunaHakeutuva;


    public VihollinenSplortsi(LegendOfPallo peli, Vector paikka, double leveys, double korkeus) : base(peli, leveys, korkeus)
    {
        Score = 1000;
        Hp = 150;
        Tag = "vihollinen";
        Position = paikka;

        Width = Animaatiot.SplortsiImages[0].Width*4;
        Height = Animaatiot.SplortsiImages[0].Height*4;

        aivot = new LabyrinthWandererBrain(64);
        aivot.Speed = 300.0;
        aivot.LabyrinthWallTag = "seina";
        aivot.DirectionChangeTimeout = 0.5;

        Mass = 10000;
        Brain = aivot;
        LinearDamping = 0.7;
        CanRotate = false;

        Animation = new Animation(Animaatiot.SplortsiImages);
        Animation.Start();
        Animation.FPS = 5;

        Tavarat.LisaaTavara((int)TavaraTyypit.Avaimet, 1);
        Tavarat.LisaaTavara((int)TavaraTyypit.Raha, 10);
        Tavarat.LisaaTavara((int)TavaraTyypit.Sydan, 10);

        LuoSplortsilleKanuunaTavallinen(peli, new Vector(96, 96), 1, true);
        LuoSplortsilleKanuunaTavallinen(peli, new Vector(-96, -96), 3, true);
        LuoSplortsilleKanuunaTavallinen(peli, new Vector(-96, 96), 2, false);
        LuoSplortsilleKanuunaTavallinen(peli, new Vector(96, -96), 4, false);
        kanuunaHakeutuva = LuoSplortsilleKanuunaHakeutuva(peli);

    }


    /// <summary>
    /// Suoritetaan jokaisella pelin Framella.
    /// </summary>
    public override void Update(Time time)
    {
        //Nopeuta liikkumisnopeutta kun Hp alle 50
        if (CurrentHp <= 50 && aivot.Speed == 300)
        {
            aivot.Speed = 600;
            kanuunaHakeutuva.AsetaKanuuna(2, 1, short.MaxValue);
        }


        base.Update(time);
    }


    /// <summary>
    /// Splörtsin kanuuna, joka ampuu "tavallisia" ammuksia pelaajaa kohti.
    /// </summary>
    /// <param name="peli">Pelin viite.</param>
    /// <param name="paikka">Kanuunan paikka suhteessa Splörtsin keskipisteeseen.</param>
    /// <param name="latausAika">Kuinka monen sekunnin päästä kanuuna alkaa ampumaan.</param>
    /// <param name="ennakoi">Ennakoiko kanuuna pelaajan liikettä ja ampuu kohtaan, johon pelaaja on kulkemassa.</param>
    private void LuoSplortsilleKanuunaTavallinen(LegendOfPallo peli, Vector paikka, double latausAika, bool ennakoi)
    {
        Kanuuna<Ammus> kanuuna = new Kanuuna<Ammus>(peli, 10, 10);
        kanuuna.ammus += Peli.AmmusLuoti1Malli;
        kanuuna.Ennakoi = ennakoi;
        kanuuna.IsVisible = false;
        kanuuna.IgnoresExplosions = false;
        kanuuna.IgnoresCollisionResponse = true;
        kanuuna.Tag = "neutraali";
        kanuuna.AsetaKanuuna(4, latausAika, short.MaxValue);
        kanuuna.AmpumisKohde = Peli.pelaaja;
        kanuuna.Position = paikka;
        Add(kanuuna);
    }


    /// <summary>
    /// Splortsin kanuuna, jonka luodit hakeutuvat pelaajaa kohti.
    /// </summary>
    /// <param name="peli">Pelin viite.</param>
    /// <returns>Kanuunan viite.</returns>
    private Kanuuna<Ammus> LuoSplortsilleKanuunaHakeutuva(LegendOfPallo peli)
    {
        Kanuuna<Ammus> kanuuna5 = new Kanuuna<Ammus>(peli, 10, 10);
        kanuuna5.ammus += Peli.AmmusHakeutuva1Malli;
        kanuuna5.IsVisible = false;
        kanuuna5.IgnoresExplosions = false;
        kanuuna5.AsetaKanuuna(1, 20, 6);
        kanuuna5.AmpumisKohde = Peli.pelaaja;
        kanuuna5.Tag = "neutraali";
        kanuuna5.IgnoresCollisionResponse = true;
        Add(kanuuna5);
        kanuuna5.Position = new Vector(0, 0);

        return kanuuna5;
    }

}

