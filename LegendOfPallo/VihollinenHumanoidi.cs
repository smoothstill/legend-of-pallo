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
/// Vihollinen, joka ilmestyy satunnaiseen paikkaan tietyn väliajoin, ja katoaa taas hetkeksi näkyvistä.
/// </summary>
public class VihollinenHumanoidi: Peliolio
{


    private Kanuuna<Ammus> ase;
    private Timer ajastin;
    private bool nakyva = true;
    private Vector spawnPaikka;


    public VihollinenHumanoidi(LegendOfPallo peli, Vector paikka, double width, double height) : base(peli, width, height)
    {

        Score = 60;
        spawnPaikka = paikka;
        Image = Animaatiot.HumanoidImage;
        MakeStatic();
        CanRotate = false;
        Tag = "vihollinen";
        IsVisible = false;
        IsUpdated = true;
        IgnoresCollisionResponse = true;

        ajastin = new Timer();
        ajastin.Interval = 7;
        ajastin.Timeout += KatoaNakyvista;

        ase = new Kanuuna<Ammus>(Peli, 1, 1);
        ase.IsVisible = false;
        ase.Tag = "neutraali";
        ase.Position = new Vector(0, 0);
        ase.IgnoresCollisionResponse = true;
        ase.ammus += Peli.AmmusLuoti1Malli;
        ase.AmpumisKohde = Peli.pelaaja;
        ase.MaxEtaisyysKohteeseen = 1200;
        Add(ase);

        KatoaNakyvista();

    }


    /// <summary>
    /// Suoritetaan, kun vihollisen on määrä kadota pelikentästä.
    /// </summary>
    private void KatoaNakyvista()
    {

        if (nakyva == true)
        {
            IsVisible = false;
            nakyva = false;
            Position = new Vector(-9999, -9999);    //Teleporttaa pois kentästä tarpeeksi kauas.
            ajastin.Start(1);
            Hp = 6;
        }

    }


    /// <summary>
    /// Suoritetaan jokaisella pelin framella.
    /// </summary>
    public override void Update(Time time)
    {

        PhysicsObject p = Peli.pelaaja;

        //Jos pelaaja on tuhoutunut -> palataan.
        if (p.IsDestroyed) return;

        //Jos pelaaja on liian kaukana spawnaus paikasta -> palataan
        if (Vector.Distance(spawnPaikka, p.Position) >= 800) return;
        else spawnPaikka = p.Position;


        //Jos vihollinen on piilossa ja ajastin on menty läpi, etsitään sopivaa paikkaa spawnata pelaajan läheisyyteen.
        //Jos sopivaa paikkaa ei löydy -> ei spawnata, yritetään uudestaan seuraavalla framella.
        if (nakyva == false && ajastin.Times == 0)
        {

            //Haetaan satunnainen paikka pelaajan lähettyviltä:
            Vector newPosition = RandomGen.NextVector(LegendOfPallo.TILE_SIZE * 2, LegendOfPallo.TILE_SIZE * 6);
            newPosition += p.Position;

            //Jos satunnainen paikka on pelialueen ulkopuolella -> ei kelpaa joten palataan.
            if (newPosition.X < Peli.Level.Left || newPosition.X > Peli.Level.Right ||
                newPosition.Y < Peli.Level.Bottom || newPosition.Y > Peli.Level.Top)
                return;

            //Jos satunnaisesti valittu paikka on tyhjä, spawnataan olio siihen paikkaan ja muutetaan näkyväksi.
            //Jostain syystä tämän pitää olla <= 1 eikä == 0, tai muuten ei toimi fullscreenissa tai suurella resoluutiolla
            if (Peli.GetObjectsAt(newPosition, LegendOfPallo.TILE_SIZE).Count <= 1)
            {
                nakyva = true;
                IsVisible = true;
                Position = newPosition;
                ase.AsetaKanuuna(2, 2, 1);
                ajastin.Start(1);
            }

        }

        base.Update(time);
    }


}

