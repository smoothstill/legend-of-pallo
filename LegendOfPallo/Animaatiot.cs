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
/// Tähän staattiseen luokkaan on tallennettu kaikki kuvat ja animaatiot, joita pelissä käytetään.
/// </summary>
public static class Animaatiot
{

    public static readonly Image pelaajanKuvaN = Game.LoadImage("Pelaaja1N");
    public static readonly Image pelaajanKuvaNE = Game.LoadImage("Pelaaja1NE");
    public static readonly Image pelaajanKuvaE = Game.LoadImage("Pelaaja1E");
    public static readonly Image pelaajanKuvaSE = Game.LoadImage("Pelaaja1SE");
    public static readonly Image pelaajanKuvaS = Game.LoadImage("Pelaaja1S");
    public static readonly Image pelaajanKuvaSW = Game.LoadImage("Pelaaja1SW");
    public static readonly Image pelaajanKuvaW = Game.LoadImage("Pelaaja1W");
    public static readonly Image pelaajanKuvaNW = Game.LoadImage("Pelaaja1NW");

    public static readonly Image seinanKuva = Game.LoadImage("seina");
    public static readonly Image laatikkoImage = Game.LoadImage("laatikko");
    public static readonly Image seinaRikkuvaImage = Game.LoadImage("rikkuvaSeina");
    public static readonly Image BackgroundImage = Game.LoadImage("background4");
    public static readonly Image BackgroundSkullsImage = Game.LoadImage("skullfloor");
    public static readonly Image KeyDoorImage = Game.LoadImage("keyhole");
    public static readonly Image ExitImage = Game.LoadImage("exit");
    public static readonly Image TeleImage = Game.LoadImage("tele2");

    public static readonly Image SpikesOnImage = Game.LoadImage("spikesOn");
    public static readonly Image SpikesOffImage = Game.LoadImage("spikesOff");

    public static readonly Image[] SpikeImages = Game.LoadImages("spikesOff", "spikesOn");

    public static readonly Image KeyPickupImage = Game.LoadImage("key");
    public static readonly Image BombPickupImage = Game.LoadImage("bomb");
    public static readonly Image CoinPickupImage = Game.LoadImage("coin");
    public static readonly Image HeartPickupImage = Game.LoadImage("heart");
    public static readonly Image PotionPickupImage = Game.LoadImage("Potioni");

    public static readonly Image BombInfImage = Game.LoadImage("bombInf");

    public static readonly Image HeartEmptyImage = Game.LoadImage("heart2");
    public static readonly Image LivesImage = Game.LoadImage("elamat");

    public static readonly Image HumanoidImage = Game.LoadImage("humanoid");
    public static readonly Image AcidPoolImage = Game.LoadImage("splötsPool");
    public static readonly Image ShooterGrayImage = Game.LoadImage("Shooter");
    public static readonly Image ShooterGreenImage = Game.LoadImage("ShooterGreen");
    public static readonly Image ShooterRedImage = Game.LoadImage("ShooterRed");

    public static readonly Image[] SplotsImages = Game.LoadImages("splöts1", "splöts2");
    public static readonly Image[] BatImages = Game.LoadImages("bat1", "bat2", "bat3", "bat2");
    public static readonly Image[] DemonImages = Game.LoadImages("demon1", "demon2");
    public static readonly Image[] SwampyImages = Game.LoadImages("swampy1", "swampy2");
    public static readonly Image[] SwampyMeltImages = Game.LoadImages("swampyMelt1", "swampyMelt2");
    public static readonly Image[] SwampyReviveImages = Game.LoadImages("swampyMelt2", "swampyMelt1");
    public static readonly Image[] GhostImages = Game.LoadImages("wtf1", "wtf2");

    public static readonly Image[] BombImages = Game.LoadImages("bomb1", "bomb2");
    public static readonly Image BulletAImage = Game.LoadImage("ammusA");
    public static readonly Image BulletBImage = Game.LoadImage("ammusB");
    public static readonly Image[] BulletCImages = Game.LoadImages("ammusC1", "ammusC2");

    public static readonly Image[] PinkBallImages = Game.LoadImages("pinkball1", "pinkball2");

    public static readonly Image[] SplortsiImages = Game.LoadImages("Splörtsi1", "Splörtsi2");


}

