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
/// Laatikko, jota pelaaja voi työntää ja jonka voi tuhota räjähteillä.
/// </summary>
public class Laatikko : PhysicsObject
{

    public Laatikko(double w, double h, double x, double y):base(w,h,Shape.Rectangle)
    {
        Color = Color.Brown;
        X = x;
        Y = y;
        LinearDamping = 0.7;
        MaxVelocity = 100;

    }

    public void TuhoaLaatikko()
    {
        Destroy();
    }

}

