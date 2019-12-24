using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;


public partial class LegendOfPallo : PhysicsGame
{


    /// <summary>
    /// Luo teleportin sisäänkäynnin.
    /// </summary>
    private void LuoTeleportEntrance(Vector paikka, double leveys, double korkeus)
    {
        Teleportti tele = new Teleportti(paikka, leveys, korkeus);
        Add(tele, -3);
        AddCollisionHandler<Teleportti, Peliolio>(tele, pelaaja, Teleporttaa);
    }
    
    /// <summary>
    /// Luo teleportin määränpään.
    /// </summary>
    private void LuoTeleportExit(Vector paikka, double leveys, double korkeus)
    {
        Teleportti.LisaaExit(paikka);
    }


    /// <summary>
    /// Malli, jonka avulla tavallisia ammuksia tehdään peliin.
    /// </summary>
    public Ammus AmmusLuoti1Malli()
    {
        Ammus ammus = new Ammus(10, 300);
        ammus.Color = Color.Yellow;
        ammus.IgnoresCollisionResponse = true;
        ammus.Image = Animaatiot.BulletAImage;
        ammus.Width = 24;
        ammus.Height = 24;

        ammus.Rajahdys = new Explosion(40.0);
        ammus.Rajahdys.Speed = 500.0;
        ammus.Rajahdys.Force = 200;
        ammus.Rajahdys.ShockwaveColor = Color.Yellow;
        ammus.Rajahdys.Sound = null;

        ammus.IgnoresExplosions = true;
        ammus.Tag = "ammus";

        return ammus;
    }


    /// <summary>
    /// Malli, jonka avulla hakeutuvia ammuksia tehdään peliin.
    /// </summary>
    public Ammus AmmusHakeutuva1Malli()
    {
        Ammus ammusHakeutuva = new Ammus(25, 250.0);
        ammusHakeutuva.Color = Color.Orange;
        ammusHakeutuva.IsUpdated = true;
        ammusHakeutuva.MaxVelocity = 220;
        ammusHakeutuva.Kohde = pelaaja;
        ammusHakeutuva.LifetimeLeft = TimeSpan.FromSeconds(10);

        ammusHakeutuva.IgnoresCollisionResponse = true;
        ammusHakeutuva.IgnoresExplosions = true;
        ammusHakeutuva.Tag = "ammus";

        ammusHakeutuva.Animation = new Animation(Animaatiot.BulletCImages);
        ammusHakeutuva.Animation.FPS = 5.6;
        ammusHakeutuva.Animation.Start();

        ammusHakeutuva.Rajahdys = new Explosion(30.0);
        ammusHakeutuva.Rajahdys.Speed = 500.0;
        ammusHakeutuva.Rajahdys.Force = 200;
        ammusHakeutuva.Rajahdys.ShockwaveColor = Color.Yellow;
        ammusHakeutuva.Rajahdys.Sound = null;

        return ammusHakeutuva;
    }


    /// <summary>
    /// Malli, jonka avulla kimpoilevia ammuksia tehdään peliin.
    /// </summary>
    public Ammus AmmusKimpoileva1Malli()
    {
        Ammus kimpoileva = new Ammus(15, 600);
        kimpoileva.Color = Color.Green;
        kimpoileva.Restitution = 1.0;
        kimpoileva.IgnoresExplosions = true;
        kimpoileva.Tag = "ammusKimpoileva";
        kimpoileva.LifetimeLeft = TimeSpan.FromSeconds(3.7);
        kimpoileva.Image = Animaatiot.BulletBImage;
        kimpoileva.Width = 48;
        kimpoileva.Height = 48;
        kimpoileva.CanRotate = false;

        kimpoileva.Rajahdys = new Explosion(40.0);
        kimpoileva.Rajahdys.Speed = 500.0;
        kimpoileva.Rajahdys.Force = 200;
        kimpoileva.Rajahdys.ShockwaveColor = Color.LimeGreen;
        kimpoileva.Rajahdys.Sound = null;

        return kimpoileva;
    }


    /// <summary>
    /// Spawnerin käyttämä malli kuljeksivista vihollisista, joita spawneri luo.
    /// </summary>
    public VihollinenKuljeksiva VihollinenKuljeksivaMalli()
    {

        VihollinenKuljeksiva vihollinen = new VihollinenKuljeksiva(this, 56, 48);
        vihollinen.Score = 0;
        PeliolionPerustiedot(vihollinen);

        return vihollinen;
    }


    /// <summary>
    /// Patsas, joka ampuu kohti pelaajaa ja on kuolematon. Versio A.
    /// </summary>
    private void LuoVihollinenAmpujaA(Vector paikka, double leveys, double korkeus)
    {

        Kanuuna<Ammus> kanuuna = AmpujaPerustiedot(paikka, leveys, korkeus, Animaatiot.ShooterGrayImage, 512);
        kanuuna.AsetaKanuuna(2, 2, 1);
        kanuuna.ammus += AmmusLuoti1Malli;

    }


    /// <summary>
    /// Luo pelin loppuvihollisen, Splortsin.
    /// </summary>
    private void LuoVihollinenSplortsi(Vector paikka, double leveys, double korkeus)
    {

        VihollinenSplortsi splortsi = new VihollinenSplortsi(this, paikka, leveys, korkeus);
        PeliolionPerustiedot(splortsi);
        AddCollisionHandler<Peliolio, Pelaaja>(splortsi, "pelaaja", CollisionHandler.DestroyTarget);
        Add(splortsi);
    }


    /// <summary>
    /// Patsas, joka ampuu kohti pelaajaa ja on kuolematon. Versio B.
    /// </summary>
    private void LuoVihollinenAmpujaB(Vector paikka, double leveys, double korkeus)
    {
        Kanuuna<Ammus> kanuuna = AmpujaPerustiedot(paikka, leveys, korkeus, Animaatiot.ShooterGreenImage, 512);
        kanuuna.AsetaKanuuna(1, 1, 1);
        kanuuna.ammus += AmmusKimpoileva1Malli;
    }


    /// <summary>
    /// Patsas, joka ampuu kohti pelaajaa ja on kuolematon. Versio C.
    /// </summary>
    private void LuoVihollinenAmpujaC(Vector paikka, double leveys, double korkeus)
    {
        Kanuuna<Ammus> kanuuna = AmpujaPerustiedot(paikka, leveys, korkeus, Animaatiot.ShooterRedImage, 512);
        kanuuna.AsetaKanuuna(5, 5, 1);
        kanuuna.ammus += AmmusHakeutuva1Malli;
    }


    /// <summary>
    /// Patsas, joka ampuu kohti pelaajaa ja on kuolematon. Versio S (ampuu jatkuvasti pelaajan ollessa lähellä).
    /// </summary>
    private void LuoVihollinenAmpujaS(Vector paikka, double leveys, double korkeus)
    {
        Kanuuna<Ammus> kanuuna = AmpujaPerustiedot(paikka, leveys, korkeus, Animaatiot.ShooterGrayImage, 640);
        kanuuna.AsetaKanuuna(0.2, 5, short.MaxValue);
        kanuuna.ammus += AmmusLuoti1Malli;
        kanuuna.AloitaTyhjana = true;

    }


    /// <summary>
    /// Ampuvan patsaan perustiedot, jotka ovat samat kaikilla erilaisilla ampujilla.
    /// </summary>
    /// <param name="paikka">Olion paikka pelissä.</param>
    /// <param name="leveys">Olion leveys.</param>
    /// <param name="korkeus">Olion korkeus</param>
    /// <param name="img">Olion kuva.</param>
    /// <param name="etaisyysKohteeseen">Etäisyys kohteeseen(pelaajaan).</param>
    /// <returns>Ampujan viite.</returns>
    private Kanuuna<Ammus> AmpujaPerustiedot(Vector paikka, double leveys, double korkeus, Image img, double etaisyysKohteeseen)
    {
        Kanuuna<Ammus> kanuuna = new Kanuuna<Ammus>(this, 64, 64);
        kanuuna.Image = img;
        kanuuna.AmpumisKohde = pelaaja;
        kanuuna.Position = paikka;
        kanuuna.MaxEtaisyysKohteeseen = etaisyysKohteeseen;
        Add(kanuuna);

        return kanuuna;
    }


    /// <summary>
    /// Malli, jonka avulla pelaajan heittämät pommit luodaan.
    /// </summary>
    public Ammus AmmusPommi1Malli()
    {
        Ammus pommi = new Ammus(16, 1000);
        pommi.LinearDamping = 0.7;
        pommi.Tag = "pommi";
        pommi.Color = Color.DarkBlue;
        pommi.LifetimeLeft = TimeSpan.FromSeconds(1.5);
        pommi.IgnoresExplosions = true;
        pommi.Animation = new Animation(Animaatiot.BombImages);
        pommi.Animation.FPS = 6;
        pommi.Animation.Start();
        pommi.CanRotate = false;
        pommi.Width = 48;
        pommi.Height = 56;

        //pommi.Rajahdys = new Explosion(TILE_SIZE * 1.5);
        pommi.Rajahdys = new Explosion(TILE_SIZE * 1.8);
        pommi.Rajahdys.Speed = 800.0;
        pommi.Rajahdys.Force = 1;
        pommi.Rajahdys.ShockwaveColor = Color.Orange;
        pommi.Rajahdys.Sound = null;

        pommi.Rajahdys.AddShockwaveHandler("vihollinen", PaineaaltoOsuuPeliolioon);
        pommi.Rajahdys.AddShockwaveHandler("pelaaja", PaineaaltoOsuuPeliolioon);
        pommi.Rajahdys.AddShockwaveHandler("seina", PaineaaltoOsuuPeliolioon);
        pommi.Rajahdys.AddShockwaveHandler("immuuniLuodeille", PaineaaltoOsuuPeliolioon);

        return pommi;
    }


    /// <summary>
    /// Aliohjelma, joka luo kummitus -vihollisen peliruutuun.
    /// </summary>
    private void LuoVihollinenKummitus(Vector paikka, double leveys, double korkeus)
    {
        VihollinenSeuraajaKummitus kummitus = new VihollinenSeuraajaKummitus(this, leveys, korkeus);
        PeliolionPerustiedot(kummitus, paikka);
        AddCollisionHandler<Peliolio, Pelaaja>(kummitus, "pelaaja", CollisionHandler.DestroyTarget);
        Add(kummitus, 3);

    }


    /// <summary>
    /// Aliohjelma, joka luo spawneri -vihollisen peliruutuun. Spawneri ampuu kuljeksivia vihollisia tietyn määräajoin.
    /// </summary>
    private void LuoVihollinenSpawneri(Vector paikka, double leveys, double korkeus)
    {
        Kanuuna<VihollinenKuljeksiva> spawneri = new Kanuuna<VihollinenKuljeksiva>(this, 64, 56);
        spawneri.Image = Animaatiot.AcidPoolImage;
        spawneri.Hp = 1;
        spawneri.Score = 100;
        spawneri.IgnoresCollisionResponse = true;
        spawneri.AmpumisKohde = pelaaja;
        spawneri.MaxEtaisyysKohteeseen = 1024;
        spawneri.Position = paikka;

        //Spawnaa maksimissaan 80 olioo
        spawneri.AloitaTyhjana = false;
        spawneri.AsetaKanuuna(10, Double.PositiveInfinity, 80);

        spawneri.ammus += VihollinenKuljeksivaMalli;
        spawneri.Tag = "seina";
        Add(spawneri, -2);
    }


    /// <summary>
    /// Luo piikit peliruutuun. Pelaaja kuolee astuessaan piikkeihin.
    /// </summary>
    private void LuoPiikit(Vector paikka, double leveys, double korkeus)
    {
        Piikit piikit = new Piikit(this, paikka);
        piikit.Tag = "neutraali";
        Add(piikit, -3);

    }


    /// <summary>
    /// Luo pinkin pallon peliruutuun. Pelaaja voittaa koskettamalla pinkkiä palloa.
    /// </summary>
    private void LuoPinkkiPallo(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject pallo = PhysicsObject.CreateStaticObject(64, 64);
        pallo.Animation = new Animation(Animaatiot.PinkBallImages);
        pallo.Tag = "neutraali";
        pallo.Position = paikka;
        AddCollisionHandler<PhysicsObject, Pelaaja>(pallo, "pelaaja", VoitaPeli);
        Add(pallo);
    }


    /// <summary>
    /// Luo humanoidi vihollisen peliruutuun.
    /// </summary>
    private void LuoVihollinenHumanoidi(Vector paikka, double leveys, double korkeus)
    {
        VihollinenHumanoidi humanoidi = new VihollinenHumanoidi(this, paikka, 36, 64);
        PeliolionPerustiedot(humanoidi);
        Add(humanoidi, 1);
    }


    /// <summary>
    /// Luo satunnaisesti kuljeksivan vihollisen peliruutuun.
    /// </summary>
    private void LuoVihollinenKuljeksiva(Vector paikka, double leveys, double korkeus)
    {
        VihollinenKuljeksiva vihollinen = new VihollinenKuljeksiva(this, 56, 48);
        PeliolionPerustiedot(vihollinen, paikka);
        Add(vihollinen);
    }


    /// <summary>
    /// Luo pelaajaa seuraavan vihollisen peliruutuun.
    /// </summary>
    private void LuoVihollinenSeuraaja(Vector paikka, double leveys, double korkeus)
    {
        VihollinenSeuraaja vihollinen = new VihollinenSeuraaja(this, leveys, korkeus);
        PeliolionPerustiedot(vihollinen, paikka);
        Add(vihollinen, -1);
    }


    /// <summary>
    /// Luo pelaajaa ampuvan ja seuraavan vihollisen peliruutuun.
    /// </summary>
    private void LuoVihollinenAmpuja(Vector paikka, double leveys, double korkeus)
    {
        VihollinenAmpuja vihollinen = new VihollinenAmpuja(this, 64, 64);
        PeliolionPerustiedot(vihollinen, paikka);
        Add(vihollinen);
    }


    /// <summary>
    /// Luo seinän peliruutuun.
    /// </summary>
    private void LuoSeina(Vector paikka, double leveys, double korkeus)
    {

        PhysicsObject seina = PhysicsObject.CreateStaticObject(leveys, korkeus);
        seina.Position = paikka;
        seina.Image = Animaatiot.seinanKuva;
        seina.Tag = "seina";
        seina.CollisionIgnoreGroup = (int)Types.Wall;
        AddCollisionHandler<PhysicsObject, Ammus>(seina, "ammus", CollisionHandler.DestroyTarget);
        Add(seina);
    }


    /// <summary>
    /// Luo kentän exitin peliruutuun.
    /// </summary>
    private void LuoExit(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject exit = new PhysicsObject(64, 40);
        exit.Position = paikka;
        exit.Image = Animaatiot.ExitImage;
        exit.Tag = "exit";
        exit.IgnoresCollisionResponse = true;
        exit.IgnoresExplosions = true;
        Add(exit, -3);

        AddCollisionHandler<PhysicsObject, Pelaaja>(exit, pelaaja, KenttaVaihtuu);

    }


    /// <summary>
    /// Luo pommeilla rikkuvan seinän peliruutuun.
    /// </summary>
    private void LuoRikkuvaSeina(Vector paikka, double leveys, double korkeus)
    {

        Peliolio seina = new Peliolio(this, leveys, korkeus);
        seina.MakeStatic();
        seina.Hp = 1;
        seina.Position = paikka;
        seina.Color = Color.Charcoal;
        seina.Image = Animaatiot.seinaRikkuvaImage;
        seina.Tag = "seina";
        seina.CollisionIgnoreGroup = (int)Types.Wall;
        AddCollisionHandler<Peliolio, Ammus>(seina, "ammus", CollisionHandler.DestroyTarget);
        Add(seina);

    }


    /// <summary>
    /// Luo lukitun oven peliruutuun (avataan avaimella)
    /// </summary>
    private void LuoLukittuOvi(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject lukittuOvi = PhysicsObject.CreateStaticObject(leveys, korkeus);
        lukittuOvi.Position = paikka;
        lukittuOvi.Image = Animaatiot.KeyDoorImage;
        lukittuOvi.Tag = "seina";
        lukittuOvi.CollisionIgnoreGroup = (int)Types.Wall;
        AddCollisionHandler<PhysicsObject, Ammus>(lukittuOvi, "ammus", CollisionHandler.DestroyTarget);
        AddCollisionHandler<PhysicsObject, Pelaaja>(lukittuOvi, "pelaaja", AvaaOvi);
        Add(lukittuOvi);
    }


    //Luo tyhjän laatikon peliruutuun
    private void LuoLaatikkoTyhja(Vector paikka, double leveys, double korkeus)
    {
        Peliolio laatikko = LaatikkoMalli(paikka, leveys, korkeus);
    }


    //Luo laatikon peliruutuun, jonka sisällä on avain
    private void LuoLaatikkoAvain(Vector paikka, double leveys, double korkeus)
    {
        Peliolio laatikko = LaatikkoMalli(paikka, leveys, korkeus);
        laatikko.Tavarat.LisaaTavara((int)TavaraTyypit.Avaimet, 1);
    }


    /// <summary>
    /// Luo ja lisää peliin laatikon ja palauttaa sen osoittimen
    /// </summary>
    /// <param name="paikka"></param>
    /// <param name="leveys"></param>
    /// <param name="korkeus"></param>
    /// <returns>Laatikon viite.</returns>
    private Peliolio LaatikkoMalli(Vector paikka, double leveys, double korkeus)
    {

        Peliolio laatikko = new Peliolio(this, leveys, korkeus);
        laatikko.Hp = 5;
        laatikko.Position = paikka;
        laatikko.LinearDamping = 0.7;
        laatikko.AngularDamping = 0.7;
        laatikko.CanRotate = false;
        laatikko.Mass = 5;
        laatikko.Tag = "seina";
        laatikko.Image = Animaatiot.laatikkoImage;
        AddCollisionHandler<Peliolio, Ammus>(laatikko, "ammus", PeliolioonOsuiAmmus);
        AddCollisionHandler<Peliolio, Ammus>(laatikko, "ammus", CollisionHandler.DestroyTarget);
        Add(laatikko);
        return laatikko;

    }


    /// <summary>
    /// Luo pelaajan peliruutuun.
    /// </summary>
    private void LuoPelaaja(Vector paikka, double leveys, double korkeus)
    {
        pelaaja = new Pelaaja(this, leveys, korkeus, pelaajanTiedot);
        //pelaaja.Position = paikka;

        PeliolionPerustiedot(pelaaja, paikka);
        AddCollisionHandler<Peliolio, Peliolio>(pelaaja, "vihollinen", PeliolioonOsuiPeliolio);

        Add(pelaaja);
    }


    /// <summary>
    /// Luo kimpoilevan vihollisen peliruutuun.
    /// </summary>
    private void LuoKimpoilija(Vector paikka, double leveys, double korkeus)
    {
        Kimpoilija kimpoilija = new Kimpoilija(this, 64, 56);
        //kimpoilija.Position = paikka;
        PeliolionPerustiedot(kimpoilija, paikka);
        Add(kimpoilija, 2);
    }


    /// <summary>
    /// Luo nostettavan pommin peliruutuun
    /// </summary>
    private void LuoNostettavaPommi(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject pommi = NostettavaPommiMalli(paikka, leveys, korkeus);
    }


    /// <summary>
    /// Malli, jonka avulla nostettavia pommeja voi luoda peliin.
    /// </summary>
    public PhysicsObject NostettavaPommiMalli(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject pommi = new PhysicsObject(48, 56);
        pommi = NostettavaTavaranOminaisuudet(pommi, paikka, true);
        pommi.Image = Animaatiot.BombPickupImage;
        pommi.Tag = "pommiNostettava";

        Add(pommi, 2);
        return pommi;
    }


    /// <summary>
    /// Luo nostettavan avaimen peliruutuun.
    /// </summary>
    private void LuoNostettavaAvain(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject avain = NostettavaAvainMalli(paikka, leveys, korkeus);
    }


    /// <summary>
    /// Malli, jonka avulla nostettavia avaimia voi luoda peliin.
    /// </summary>
    public PhysicsObject NostettavaAvainMalli(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject avain = new PhysicsObject(32, 44);
        avain = NostettavaTavaranOminaisuudet(avain, paikka, true);
        avain.Image = Animaatiot.KeyPickupImage;
        avain.Tag = "avainNostettava";

        Add(avain, 3);
        return avain;
    }


    /// <summary>
    /// Luo nostettavan kolikon peliruutuun.
    /// </summary>
    private void LuoNostettavaKolikko(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject kolikko = NostettavaKolikkoMalli(paikka, leveys, korkeus);

    }


    /// <summary>
    /// Malli, jonka avulla nostettavia kolikoita voi luoda peliin.
    /// </summary>
    public PhysicsObject NostettavaKolikkoMalli(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject kolikko = new PhysicsObject(36, 36);
        kolikko = NostettavaTavaranOminaisuudet(kolikko, paikka, true);
        kolikko.Image = Animaatiot.CoinPickupImage;
        kolikko.Tag = "kolikkoNostettava";

        Add(kolikko, 2);
        return kolikko;
    }


    /// <summary>
    /// Luo nostettavan sydämen peliruutuun.
    /// </summary>
    private void LuoNostettavaSydan(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject sydan = NostettavaSydanMalli(paikka, leveys, korkeus);
    }


    /// <summary>
    /// Malli, jonka avulla nostettavia sydämiä voi luoda peliin.
    /// </summary>
    public PhysicsObject NostettavaSydanMalli(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject sydan = new PhysicsObject(Animaatiot.HeartPickupImage.Width*4, Animaatiot.HeartPickupImage.Height*4);
        sydan = NostettavaTavaranOminaisuudet(sydan, paikka, false);
        sydan.Image = Animaatiot.HeartPickupImage;
        sydan.Tag = "sydanNostettava";

        Add(sydan, 2);
        return sydan;
    }


    /// <summary>
    /// Luo nostettavan potionin peliruutuun.
    /// </summary>
    private void LuoNostettavaPotioni(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject potioni = NostettavaPotioniMalli(paikka, leveys, korkeus);;
    }


    /// <summary>
    /// Malli, jonka avulla nostettavia potioneita voi luoda peliin.
    /// </summary>
    public PhysicsObject NostettavaPotioniMalli(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject potioni = new PhysicsObject(36, 52);
        potioni = NostettavaTavaranOminaisuudet(potioni, paikka, false);
        potioni.Image = Animaatiot.PotionPickupImage;
        potioni.Tag = "potioniNostettava";

        Add(potioni, 2);
        return potioni;
    }


    /// <summary>
    /// Luo nostettavan elämän peliruutuun.
    /// </summary>
    private void LuoNostettavaElama(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject elama = NostettavaElamaMalli(paikka, leveys, korkeus);
    }


    /// <summary>
    /// Malli, jonka avulla nostettavia elämiä voi luoda.
    /// </summary>
    public PhysicsObject NostettavaElamaMalli(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject elama = new PhysicsObject(44, 44);
        elama = NostettavaTavaranOminaisuudet(elama, paikka, false);
        elama.Image = Animaatiot.LivesImage;
        elama.Tag = "elamaNostettava";

        Add(elama, 2);
        return elama;
    }


    /// <summary>
    /// Luo äärettömän pommien lähteen, jolla pelaaja voi täydentää pommivarastoaan äärettömästi.
    /// </summary>
    private void LuoAaretonPommi(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject pommi = new PhysicsObject(48, 56);
        pommi.Position = paikka;
        pommi.MakeStatic();
        pommi.IgnoresCollisionResponse = true;
        pommi.Image = Animaatiot.BombInfImage;
        pommi.LinearDamping = 0.7;
        pommi.Tag = "pommiAareton";
        AddCollisionHandler<PhysicsObject, Peliolio>(pommi, "pelaaja", NostaTavara);
        Add(pommi, 2);
    }


    /// <summary>
    /// Yleiset ominaisuudet, jota tavaralla on.
    /// </summary>
    /// <param name="tavara">Tavaran viite.</param>
    /// <param name="paikka">Tavaran paikka.</param>
    /// <param name="hasCollision">Onko tavaralla collision (ts. meneekö se seinistä läpi vai ei)</param>
    /// <returns>Tavaran viite.</returns>
    private PhysicsObject NostettavaTavaranOminaisuudet(PhysicsObject tavara, Vector paikka, bool hasCollision)
    {

        tavara.Position = paikka;
        tavara.CanRotate = false;
        tavara.LinearDamping = 0.7;

        if (hasCollision)
        {
            tavara.CollisionIgnoreGroup = (int)Types.Player;
        }
        else
        {
            tavara.IgnoresCollisionResponse = true;
            tavara.IgnoresExplosions = true;
        }

        AddCollisionHandler<PhysicsObject, Peliolio>(tavara, "vihollinen", NostaTavara);
        AddCollisionHandler<PhysicsObject, Peliolio>(tavara, "pelaaja", NostaTavara);

        return tavara;
    }


    /// <summary>
    /// Tiedot, jotka ovat samat useilla pelioliolla.
    /// </summary>
    /// <param name="p">Peliolion viite.</param>
    private void PeliolionPerustiedot(Peliolio p)
    {
        AddCollisionHandler<Peliolio, Ammus>(p, "ammus", PeliolioonOsuiAmmus);
        AddCollisionHandler<Peliolio, Ammus>(p, "ammusKimpoileva", PeliolioonOsuiAmmus);
    }


    /// <summary>
    /// Tiedot, jotka ovat samat useilla peliolioilla. Asettaa collisionin ammuksia kohtaan ja
    /// halutessa myös olion paikan toisena parametrina.
    /// </summary>
    /// <param name="p">Peliolion viite.</param>
    /// <param name="paikka">Peliolion paikka.</param>
    private void PeliolionPerustiedot(Peliolio p, Vector paikka)
    {
        PeliolionPerustiedot(p);
        p.Position = paikka;
    }


}

