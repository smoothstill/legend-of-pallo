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
/// Satunnaisesti kulkeva vihollinen.
/// </summary>
public class VihollinenKuljeksiva : Peliolio
{


    public VihollinenKuljeksiva(LegendOfPallo peli, double width, double height):base(peli, width, height)
    {
        Score = 20;
        Color = Color.BloodRed;
        LabyrinthWandererBrain aivot = new LabyrinthWandererBrain(64);
        aivot.Speed = 300.0;
        aivot.LabyrinthWallTag = "seina";
        aivot.DirectionChangeTimeout = 0.5;

        Mass = 0.3;
        Hp = 1;
        Brain = aivot;
        LinearDamping = 0.7;
        CanRotate = false;
        Tag = "vihollinen";

        Animation = new Animation(Animaatiot.SplotsImages);
        Animation.Start();
        Animation.FPS = 5;

    }


}

