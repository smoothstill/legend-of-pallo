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
/// Piikit, joihin pelaaja kuolee koskiessaan. Piikit menevät päältä ja pois automaattisesti.
/// </summary>
public class Piikit : PhysicsObject
{


    //Koska kaikki piikit toimivat samalla tavalla, monet sen ominaisuudet voidaan laittaa staattisiksi
    private static Timer piikkiajastin = new Timer();
    private static bool paivita = false;
    private static Animation animaatio;

    private static bool paalla = false;

    private LegendOfPallo peli;
    private List<GameObject> oliot;
    

    /// <summary>
    /// Resetoi piikkien tilan kentän alussa.
    /// </summary>
    public static void AktioiPiikit()
    {


        Piikit.animaatio.Stop();
        paalla = false;
        piikkiajastin.Reset();
        piikkiajastin.Start();


    }


    /// <summary>
    /// Asettaa staattisten muuttujien asetukset.
    /// </summary>
    static Piikit()
    {

        Piikit.animaatio = new Animation(Animaatiot.SpikeImages);
        piikkiajastin.Interval = 2;
        piikkiajastin.Timeout += AktivoiDeaktivoi;

    }


    /// <summary>
    /// Asennetaan piikit
    /// </summary>
    /// <param name="p">Pelin viite.</param>
    /// <param name="paikka">Piikin paikka maailmassa.</param>
    public Piikit(LegendOfPallo p, Vector paikka) : base(64, 64)
    {

        peli = p;
        IsUpdated = true;
        MakeStatic();
        Animation = animaatio;
        IgnoresCollisionResponse = true;
        Position = paikka;

    }


    /// <summary>
    /// Aliohjelma, joka päivitetään pelin jokaisella framella.
    /// </summary>
    public override void Update(Time time)
    {



        if (paivita)
        {
            if (Animation != null) Animation.Step();

            paivita = false;
        }

        
        if (paalla)
        {

            oliot = peli.GetObjectsAt(Position, 24);

            foreach (var olio in oliot)
            {
                if ((string)olio.Tag == "pelaaja") //Jos pelaaja piikkien päällä, tuhotaan pelaaja.
                {
                    olio.Destroy();
                }

            }

        }


        base.Update(time);


    }


    /// <summary>
    /// Vaihtaa piikkien tilaa päälle tai pois.
    /// </summary>
    public static void AktivoiDeaktivoi()
    {
        paivita = true;
        paalla = paalla ? false : true; 
    }
    

}


