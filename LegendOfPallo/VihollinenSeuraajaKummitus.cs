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
/// Pelaajaa seuraava vihollinen, joka voi kulkea seinien läpi. Tappaa pelaajan välittömästi koskiessa tätä.
/// </summary>
public class VihollinenSeuraajaKummitus : Peliolio
{


    public VihollinenSeuraajaKummitus(LegendOfPallo peli, double width, double height) : base(peli, 48, 44)
    {

        Score = 40;
        List<string> tagit = new List<string>() { "pelaaja" };

        Color = Color.OrangeRed;

        FollowerBrain aivot = new FollowerBrain();
        aivot.TagsToFollow = tagit;
        aivot.DistanceFar = 896;
        aivot.Speed = 90;

        Brain = aivot;

        IgnoresCollisionResponse = true;
        Animation = new Animation(Animaatiot.GhostImages);
        Animation.FPS = 3;
        Animation.Start();
        Mass = 0.3;
        Hp = 5;
        LinearDamping = 0.7;

        LinearDamping = 0.7;
        CanRotate = false;

        Tag = "immuuniLuodeille";
        Hp = 5;


    }


    public override void Destroy()
    {
        //Visuaalinen räjähdys -> ei aiheuta damagea
        Explosion rajahdys = new Explosion(LegendOfPallo.TILE_SIZE/2);
        rajahdys.ShockwaveColor = Color.White;
        rajahdys.Position = this.Position;
        rajahdys.Speed = 800.0;
        rajahdys.Force = 0;
        rajahdys.Sound = null;

        Game.Add(rajahdys);

        base.Destroy();
    }


}

