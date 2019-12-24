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
/// Vihollinen, joka seuraa pelaajaa ja rupeaa ampumaan tätä tietyn välimatkan päästä. Ammuksina kimpoilevia ammuksia.
/// </summary>
class VihollinenAmpuja : Peliolio
{

    private FollowerBrain aivot;
    private Kanuuna<Ammus> kanuuna;
    bool vaihdaSuunta = false;


    public VihollinenAmpuja(LegendOfPallo peli, double leveys, double korkeus) : base(peli, leveys, korkeus)
    {

        Score = 80;
        List<string> tagit = new List<string>() { "pelaaja" };

        Color = Color.MediumPurple;

        LabyrinthWandererBrain aivot2 = new LabyrinthWandererBrain(64);
        aivot2.Speed = 150.0;
        aivot2.LabyrinthWallTag = "seina";
        aivot2.DirectionChangeTimeout = 2.0;

        aivot = new FollowerBrain();
        aivot.DistanceFar = 500;
        aivot.Speed = 200.0;
        aivot.TagsToFollow = tagit;
        aivot.FarBrain = aivot2;

        aivot.StopWhenTargetClose = true;
        aivot.DistanceClose = 400;

        Mass = 0.3;
        Hp = 10;
        Brain = aivot;
        CanRotate = false;
        LinearDamping = 0.7;
        IsUpdated = true;
        Tag = "vihollinen";

        Animation = new Animation(Animaatiot.DemonImages);
        Animation.FPS = 4;
        Animation.Start();

        kanuuna = new Kanuuna<Ammus>(Peli, 10, 10);
        kanuuna.IsVisible = false;
        kanuuna.AloitaTyhjana = false;
        kanuuna.AsetaKanuuna(0.2, 5.0, 3);
        kanuuna.Tag = "neutraali";
        kanuuna.AmpumisKohde = null;
        kanuuna.MaxEtaisyysKohteeseen = 500;
        kanuuna.IgnoresCollisionResponse = true;
        kanuuna.Position = new Vector(0,0);
        kanuuna.ammus += Peli.AmmusKimpoileva1Malli;
        Add(kanuuna);

        Tavarat.LisaaTavara((int)TavaraTyypit.Raha, 1);


    }


    /// <summary>
    /// Suoritetaan jokaisella pelin framella.
    /// </summary>
    public override void Update(Time time)
    {

        //Varmistetaan, että ampumiskohde on laitettu.
        if (aivot.CurrentTarget != null)
        {
            kanuuna.AmpumisKohde = (aivot.CurrentTarget as PhysicsObject);
        }

        //Käännetään vihollisen animaatio sen perusteella, mihin suuntaan kuljetaan.
        if (Velocity.X < 0 && Velocity.Magnitude > 1 &&  vaihdaSuunta == false)
        {
            vaihdaSuunta = true;
            Animation = Animation.Mirror(Animation);
        }
        else if (Velocity.X >= 0 && Velocity.Magnitude > 1 && vaihdaSuunta == true)
        {
            vaihdaSuunta = false;
            Animation = Animation.Mirror(Animation);
        }

        base.Update(time);

    }


}

