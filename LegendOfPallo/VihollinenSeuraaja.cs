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
/// Pelaajaa seuraavan olion luokka. Kuolee tilapäisesti ja herää eloon tietyn ajan kuluttua. Voidaan tappaa lopullisesti vain pommeilla.
/// </summary>
public class VihollinenSeuraaja : Peliolio
{


    private Timer KuolemaAjastin;
    private FollowerBrain aivot;
    private LabyrinthWandererBrain aivot2;
    private bool heraaEloon = false;


    public VihollinenSeuraaja(LegendOfPallo peli, double width, double height) : base(peli, width, height)
    {
        Score = 60;
        List<string> tagit = new List<string>() { "pelaaja" };

        Color = Color.OrangeRed;

        aivot2 = new LabyrinthWandererBrain(64);
        aivot2.Speed = 150.0;
        aivot2.LabyrinthWallTag = "seina";
        aivot2.DirectionChangeTimeout = 2.0;

        aivot = new FollowerBrain();
        aivot.DistanceFar = 500;
        aivot.Speed = 200.0;
        aivot.TagsToFollow = tagit;
        aivot.FarBrain = aivot2;

        Mass = 0.3;
        Brain = aivot;
        CanRotate = false;
        LinearDamping = 0.7;
        IsUpdated = true;

        AsetaTiedot();

        KuolemaAjastin = new Timer();
        KuolemaAjastin.Interval = 10;
        KuolemaAjastin.Timeout += HeraaEloon;


    }


    /// <summary>
    /// Vihollisen perustiedot.
    /// </summary>
    private void AsetaTiedot()
    {
        Animation = new Animation(Animaatiot.SwampyImages);
        Animation.FPS = 4;
        Animation.Start();
        Hp = 5;
        aivot.Speed = 200;
        aivot2.Speed = 150;
        IgnoresCollisionResponse = false;
        IgnoresExplosions = false;
        Tag = "vihollinen";
    }


    /// <summary>
    /// Suoritetaan kun vihollisen on tarkoitus herätä henkiin uudestaan.
    /// </summary>
    private void HeraaEloon()
    {
        heraaEloon = true;

        Animation = new Animation(Animaatiot.SwampyReviveImages);
        Animation.FPS = 4;
        Animation.Start(1);
        Animation.StopOnLastFrame = true;
    }


    /// <summary>
    /// Suoritetaan kun vihollinen tuhoutuu/kuolee.
    /// </summary>
    public override void Destroy()
    {
        if ((string)Tag == "vihollinen")    //vihollinen ei kuole kokonaan, vaan herää henkiin vähän ajan päästä.
        {
            Tavarat.PudotaTavarat();
            Animation = new Animation(Animaatiot.SwampyMeltImages);
            Animation.FPS = 4;
            Animation.Start(1);
            Animation.StopOnLastFrame = true;
            aivot.Speed = 0;
            aivot2.Speed = 0;
            KuolemaAjastin.Start(1);
            Clear();
            Tag = "immuuniLuodeille";   //vihollista ei pysty tappamaaan luodeilla, kun se on tilapäisesti kuollut. Vain pommit tehoavat.
            Hp = 1;
            IgnoresCollisionResponse = true;
            IgnoresExplosions = false;
        }
        else    //Jos vihollinen on jo tilapäisesti kullut ja se tapetaan pommeilla, tuhoa vihollinen lopullisesti.
        {
            Tavarat.PudotaTavarat();
            base.Destroy();
        }
    }


    /// <summary>
    /// Aliohjelma suoritetaan joka pelin framella.
    /// </summary>
    public override void Update(Time time)
    {
        if (heraaEloon && !Animation.IsPlaying) //Kun aika herätä eloon ja heräämisanimaatio on toistettu kerran.
        {
            AsetaTiedot();

            heraaEloon = false;
        }

        base.Update(time);
    }


}

