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
/// Seinistä yms esineistä kimpoileva vihollinen. Ei menetä nopeutta.
/// </summary>
public class Kimpoilija : Peliolio
{


    public Kimpoilija(LegendOfPallo peli, double width, double height) : base(peli, width, height)
    {

        Score = 20;
        IsUpdated = true;

        Shape = Shape.Rectangle;
        Color = Color.Red;
        Mass = 0.1;
        Restitution = 1.0;
        CanRotate = false;
        MaxVelocity = 500;
        LinearDamping = 1.0;
        Tag = "vihollinen";

        //Olio aloittaa liikkumisen satunnaiseen suuntaan.
        Hit(RandomGen.NextVector(500, 500));

        Animation = new Animation(Animaatiot.BatImages);
        Animation.FPS = 8;
        Animation.Start();


    }


    /// <summary>
    /// Aliohjelma suoritetaan joka pelin framella.
    /// </summary>
    public override void Update(Time time)
    {
        //Pidetään olion nopeus koko ajan max nopeudessa
        Velocity = new Vector(Velocity.X * 10, Velocity.Y * 10);
        base.Update(time);
    }


    /*
    public override void Destroy()
    {
        base.Destroy();
    }*/


}

