using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using Jypeli.Physics;


/// <summary>
/// Ammus-luokka josta kaikki ammukset pelissä on tehty.
/// </summary>
public class Ammus : PhysicsObject
{

    public double Speed;
    public Explosion Rajahdys;
    public IGameObject Kohde;
    public IGameObject Ampuja;

    public Ammus(double r, double speed) : base(r*2, r*2)
    {
        Shape = Shape.Ellipse;
        Color = Color.Yellow;
        CollisionIgnoreGroup = (int)Types.Projectile;
        Tag = "ammus";
        Speed = speed;
        IsUpdated = true;

    }

    /// <summary>
    /// Tuhoutuessaan räjäytetään ammus jos sillä on räjähdys.
    /// </summary>
    public override void Destroy()
    {
        if (Rajahdys != null)
        {
            Rajahdys.Position = Position;
            Game.Add(Rajahdys);
        }  

        base.Destroy();
    }


    public override void Update(Time time)
    {
        //Jos ammus on hakeutuva kohteeseen, vaihdetaan kiihtyvyys kohti kohdetta.
        if (Kohde != null && this != null)
        {
            Acceleration = new Vector(Kohde.X - this.X, Kohde.Y - this.Y) * 20;
        }


        base.Update(time);
    }


}

