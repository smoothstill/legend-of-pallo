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
/// Peliolio on luokka, josta viholliset ja pelaaja on tehty. Sen alaluokka on PhysicsObject, mutta sille on lisätty esim. elämät ja olion
/// kantamat esineet ym ominaisuuksia.
/// </summary>
public partial class Peliolio : PhysicsObject
{

    public int Damage { get; protected set; } //Peliolion tekemä damage sen koskiessa toiseen peliolioon (ei käytössä)
    public int Score { get; set; }  //Peliolion kuolemasta saadut pisteet.


    /// <summary>
    /// Olion tämänhetkinen elämä.
    /// </summary>
    private int _currentHp;
    public int CurrentHp
    {
        get
        {
            return _currentHp;
        }
        set
        {
            if (Hp == -1 || IsDestroyed) return;

            if (value < 0 || value > Hp) return; //throw error?

            _currentHp = value;

        }
    }


    /// <summary>
    /// Olion maksimielämät.
    /// </summary>
    private int _hp;
    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            CurrentHp = value;
        }
    }


    /// <summary>
    /// Palauttaa true jos annettiin elämää, muussa tapauksessa false
    /// </summary>
    /// <param name="maara"></param>
    /// <returns></returns>
    public bool AnnaElamaa(int maara)
    {
        if (maara < 0 || CurrentHp == Hp) return false;

        maara = Math.Min(maara, (Hp - CurrentHp));

        CurrentHp += maara;
        //CurrentHp = Math.Min(maara, Hp); // maara <= Hp ? maara : Hp;

        return true;
    }


    /// <summary>
    /// Tavara-luokka, johon on tallennettu olion kantamat esineet
    /// </summary>
    public TavaraLista Tavarat { get; }


    /// <summary>
    /// Pelin osoitin, johon olio on luotu
    /// </summary>
    protected LegendOfPallo Peli { get; set; }


    /// <summary>
    /// Luo peliolion
    /// </summary>
    /// <param name="p">Peli, johon olio luodaan</param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Peliolio(LegendOfPallo p, double leveys, double korkeus): base(leveys, korkeus)
    {
        Peli = p;
        Tavarat = new TavaraLista(this);
        Hp = 1;
        Score = 20;
    }


    /// <summary>
    /// Aliohjelma, jolla olio ampuu halutun tyyppisen ammuksen jotain haluttua pistettä kohti.
    /// </summary>
    /// <typeparam name="T">Ammuksen tyyppi</typeparam>
    /// <param name="position">Piste, jota kohti ammutaan</param>
    /// <param name="ammus">Ammus</param>
    /// <param name="piipunPituus">Mille etäisyydelle olion keskipisteestä ammus luodaan.</param>
    public void ShootAt<T>(Vector position, T ammus, double piipunPituus = 8)
    {
        if (Parent == null)
            Shoot(new Vector(position.X - this.X, position.Y - this.Y), ammus, piipunPituus);
        else
            Shoot(new Vector(position.X - this.AbsolutePosition.X, position.Y - this.AbsolutePosition.Y), ammus, piipunPituus);
        //Shoot(new Vector(position.X - (Parent.X + this.X), position.Y - (Parent.Y + this.Y)), ammus, piipunPituus);
    }


    /// <summary>
    /// Aliohjelma, joka laskee olion elämät osuman jälkeen ja pitäisikö olion kuolla vai ei.
    /// </summary>
    /// <param name="damage">Osuman määrä</param>
    public void OtaOsumaa(int damage)
    {
        if (Hp == -1 || IsDestroyed) return;

        damage = Math.Min(CurrentHp, damage);

        CurrentHp -= damage;

        if (ShouldDie())
        {
            Destroy();
        }

    }


    /// <summary>
    /// Aliohjelma luo ammuksen, joka ammutaan haluttuun suuntaan.
    /// </summary>
    /// <typeparam name="T">Ammuksen tyyppi</typeparam>
    /// <param name="direction">Ammuksen suunta</param>
    /// <param name="ammus">Ammus</param>
    /// <param name="piipunPituus">Mille etäisyydelle olion keskipisteestä ammus luodaan</param>
    public void Shoot<T>(Vector direction, T ammus, double piipunPituus = 8)
    {
      
        if (ammus as Ammus != null || ammus as Peliolio != null)
        {
            PhysicsObject p = ammus as PhysicsObject;

            direction = direction.Normalize();

            if (Parent == null)
                p.Position = Position + direction * piipunPituus;
            else
                p.Position = AbsolutePosition + direction * piipunPituus;

            if (p as Ammus != null) //Jos ammutaan "Ammus" tyyppinen ammus, laitetaan sen nopeudeksi ammus luokan sisällä määritelty nopeus.
            {
                (p as Ammus).Velocity = (p as Ammus).Speed * direction;
            }

            p.CollisionIgnoreGroup = CollisionIgnoreGroup;

            Game.Add(p, 2);

        }


    }


    /// <summary>
    /// Palauttaa totuusarvon, pitäisikö olion olla kuollut (eli Hp = 0)
    /// </summary>
    /// <returns>Totuusarvo</returns>
    protected bool ShouldDie()
    {
        if (CurrentHp == 0)
            return true;

        return false;
    }


    /// <summary>
    /// Käydään läpi peliolion kuollessa.
    /// </summary>
    public override void Destroy()
    {
        //Jos olio on tuhoutumaton, älä tuhoa sitä
        if (Hp == -1) return;

        //Jos destroy aliohjelmaa kutsutaan itsenäisesti, laitetaan CurrentHp nollaksi
        if (CurrentHp != 0) CurrentHp = 0;

        //Pudota olion tavarat maahan
        Tavarat.PudotaTavarat();

        //Lisää pisteet olion kuolemasta, jos olio on vihollinen
        if ((string)Tag == "vihollinen" || (string)Tag == "immuuniLuodeille")
            HUD.Pisteet += Score;


        base.Destroy();
        

    }


}


