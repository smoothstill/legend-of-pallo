using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jypeli;


public partial class LegendOfPallo : PhysicsGame
{


    /// <summary>
    /// Suoritetaan kun peliolioon osuu ammus (CollisionHandlerin avulla).
    /// </summary>
    /// <param name="p">Peliolion viite</param>
    /// <param name="a">Ammuksen viite</param>
    private void PeliolioonOsuiAmmus(Peliolio p, Ammus a)
    {

        if (a.Ampuja.Tag != p.Tag && (string)p.Tag != "neutraali" && (string)p.Tag != "immuuniLuodeille")
        {
            a.Destroy();
            p.OtaOsumaa(1);
        }
    }


    /// <summary>
    /// Suoritetaan kun peliolioon osuu toinen peliolio (CollisionHandlerin avulla).
    /// </summary>
    /// <param name="p">Peliolion 1 viite</param>
    /// <param name="o">Peliolion 2 viite</param>
    private void PeliolioonOsuiPeliolio(Peliolio p, Peliolio o)
    {
        p.OtaOsumaa(1);
    }


    /// <summary>
    /// Suoritetaan kun peliolioon osuu toinen peliolio. (CollisionHandlerin avulla).
    /// Seuraa välitön kuolema.
    /// </summary>
    /// <param name="p">Peliolion 1 viite.</param>
    /// <param name="o">Peliolion 2 viite.</param>
    private void PeliolioonOsuiPeliolioValitonKuolema(Peliolio p, Peliolio o)
    {
        o.OtaOsumaa(o.CurrentHp);
    }


    /// <summary>
    /// Suoritetaan kun tietty paineaalto osuu peliolioon.
    /// </summary>
    /// <param name="olio">Peliolion viite.</param>
    /// <param name="shokki">Räjähdyksen voimavektori (kai?)</param>
    private void PaineaaltoOsuuPeliolioon(IPhysicsObject olio, Vector shokki)
    {

        Peliolio p = olio as Peliolio;

        if (p == null)
        {
            return;
        }

        p.OtaOsumaa(5);

    }


    /// <summary>
    /// Teleporttaa peliolion teleportin uloskäyntipaikkaan.
    /// </summary>
    /// <param name="t">Teleportin viite.</param>
    /// <param name="o">Peliolion viite.</param>
    private void Teleporttaa(Teleportti t, Peliolio o)
    {
        t.Teleporttaa(o);
    }


    /// <summary>
    /// Suorittaa pelin loppumiseen liittyvät tapahtumat kun pelaaja törmää pinkkiin palloon.
    /// (CollisionHandlerin avulla)
    /// </summary>
    /// <param name="o">Pinkin pallon viite</param>
    /// <param name="p">Pelaajan viite</param>
    private void VoitaPeli(PhysicsObject o, Pelaaja p)
    {
        if (o.Animation.CurrentFrameIndex == 0) o.Animation.Step();
        p.Hp = -1;

        HUD.LaskeKentanPisteen(pelaaja);
        IsPaused = true;
        PrinttaaTekstiJaOdotaInput("You have defeated sadistic and vile creature, Splörts, and freed your friend\nfrom his dungeon. Congratulations!", TallennaPisteetJaVoitaPeli);

    }


    /// <summary>
    /// Talletaa kentän pisteet ja (tarvittaessa) asettaa pelaajan parhaiden pisteiden joukkoon, jonka jälkeen palataan menuun.
    /// </summary>
    private void TallennaPisteetJaVoitaPeli()
    {
        ClearAll();
        //HUD.LaskeKentanPisteen(pelaaja);
        AsetaHighScoreJaPalaaMenuun();
    }


    /// <summary>
    /// Lataa seuraavan kentän, kun pelaaja osuu "maaliin".
    /// </summary>
    /// <param name="a">"Maalin" viite.</param>
    /// <param name="b">Pelaajan viite.</param>
    private void KenttaVaihtuu(PhysicsObject a, Pelaaja b)
    {
        HUD.LaskeKentanPisteen(b);
        ++CurrentLevel;
        pelaajanTiedot.Elamat = b.Tavarat.TavaranMaara((int)TavaraTyypit.Elama);
        pelaajanTiedot.Pommit = b.Tavarat.TavaranMaara((int)TavaraTyypit.Pommit);
        LataaKentta();
    }


    /// <summary>
    /// Jos pelaajalla on avain mukanaan, tuhoaa lukon kosketuksesta.
    /// </summary>
    /// <param name="ovi">Oven viite</param>
    /// <param name="p">Pelaajan viite</param>
    private void AvaaOvi(PhysicsObject ovi, Pelaaja p)
    {
        if (p.Tavarat.KaytaTavara((int)TavaraTyypit.Avaimet))
            ovi.Destroy();
    }


    /// <summary>
    /// Määrittelee mitä tapahtuu kun peliolio (joko pelaaja tai vihollinen) koskettaa eri tavaroita. Tavarat erotellaan niiden
    /// Tägin avulla toisistaan.
    /// </summary>
    /// <param name="tavara">Tavaran viite</param>
    /// <param name="p">Pelaajan viite</param>
    private void NostaTavara(PhysicsObject tavara, Peliolio p)
    {
        switch (tavara.Tag)
        {
            case "pommiNostettava":

                if (p.Tavarat.TavaranMaara((int)TavaraTyypit.Pommit) < 10)
                {
                    p.Tavarat.LisaaTavara((int)TavaraTyypit.Pommit, 1);
                    tavara.Destroy();
                }
                break;
            case "avainNostettava":
                if ((string)p.Tag != "pelaaja")
                {
                    PhysicsObject dumpKey = new PhysicsObject(32, 44);
                    dumpKey.Image = Animaatiot.KeyPickupImage;
                    dumpKey.Position = new Vector(16, -8);
                    dumpKey.IgnoresCollisionResponse = true;
                    p.Add(dumpKey);
                }
                p.Tavarat.LisaaTavara((int)TavaraTyypit.Avaimet, 1);
                tavara.Destroy();
                break;
            case "kolikkoNostettava":
                p.Tavarat.LisaaTavara((int)TavaraTyypit.Raha, 1);
                tavara.Destroy();
                break;
            case "sydanNostettava":
                if ((string)p.Tag == "pelaaja")
                {
                    if (p.AnnaElamaa(1)) tavara.Destroy();
                }
                else
                {
                    p.Tavarat.LisaaTavara((int)TavaraTyypit.Sydan, 1);
                    tavara.Destroy();
                }
                break;
            case "potioniNostettava":
                if ((string)p.Tag == "pelaaja")
                {
                    if (p.AnnaElamaa(100)) tavara.Destroy();
                }
                else
                {
                    p.Tavarat.LisaaTavara((int)TavaraTyypit.Potioni, 1);
                    tavara.Destroy();
                }
                break;
            case "pommiAareton":
                p.Tavarat.LisaaTavara((int)TavaraTyypit.Pommit, Byte.MaxValue);
                break;
            case "elamaNostettava":
                p.Tavarat.LisaaTavara((int)TavaraTyypit.Elama, 1);
                tavara.Destroy();
                break;
            default:
                break;
        }

    }


}

