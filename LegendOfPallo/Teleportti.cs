using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jypeli;


/// <summary>
/// Yksisuuntaisen teleportin luontiin käytetty luokka
/// </summary>
public class Teleportti : PhysicsObject
{

    private int index;  //Indeksi, joka näyttää teleportterin exitin paikan.
    private static int nextIndex;
    private static List<Vector> teleportExit;   //Lista kaikista teleportterien exiteistä.


    /// <summary>
    /// Oletukset
    /// </summary>
    static Teleportti()
    {
        teleportExit = new List<Vector>();
        nextIndex = 0;
    }


    /// <summary>
    /// Lisätään uusi exit listalle
    /// </summary>
    /// <param name="paikka"></param>
    public static void LisaaExit(Vector paikka)
    {
        teleportExit.Add(paikka);
    }


    /// <summary>
    /// Kun teleportteri luodaan, sille asetetaan automaattisesti indeksi joka osoittaa teleportExit -listassa teleportterin exittiin.
    /// </summary>
    /// <param name="paikka">Teleportterin paikka</param>
    /// <param name="leveys">Teleportterin leveys</param>
    /// <param name="korkeus">Teleportterin korkeus</param>
    public Teleportti(Vector paikka, double leveys, double korkeus) : base(leveys, korkeus)
    {
        MakeStatic();
        IgnoresCollisionResponse = true;
        IgnoresExplosions = true;
        Image = Animaatiot.TeleImage;
        Position = paikka;
        index = nextIndex;
        ++nextIndex;
    }


    /// <summary>
    /// Kun olio osuu teleportteriin, vaihdetaan sen paikka kyseisen teleportterin exitin paikkaan.
    /// </summary>
    /// <param name="o"></param>
    public void Teleporttaa(PhysicsObject o)
    {
        o.Position = teleportExit[index];
    }


}

