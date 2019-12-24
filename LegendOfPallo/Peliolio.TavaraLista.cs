using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;


//Erityyppisten tavaroiden indeksit
enum TavaraTyypit
{
    Pommit,
    Raha,
    Avaimet,
    Sydan,
    Potioni,
    Elama,
    MaxTavarat,

}


public partial class Peliolio : PhysicsObject
{


    /// <summary>
    /// Peliolio luokan sisällä oleva Tavaralista-luokka, johon on tallennettu tiedot peliolion kantamista esineistä ja niiden määristä.
    /// </summary>
    public class TavaraLista
    {

        private Peliolio omistaja;
        private byte[,] tavarat;


        public TavaraLista(Peliolio peliolio)
        {
            omistaja = peliolio;
            tavarat = new byte[(int)TavaraTyypit.MaxTavarat, 2];
        }


        public void LisaaTavara(int tyyppi, byte maara)
        {
            if (tyyppi >= 0 && tyyppi < tavarat.Length && maara > 0)
            {
                maara = (byte)(Math.Min((byte)99, maara) + tavarat[tyyppi, 1]);

                //Pommeja maksimissaan 10
                if (tyyppi == (int)TavaraTyypit.Pommit)
                {
                    tavarat[tyyppi, 1] = Math.Min((byte)10, maara);
                }
                else //Muita tavaroita maksimissaan 99
                {
                    tavarat[tyyppi, 1] = Math.Min((byte)99, maara);
                }

            }

        }


        //Poistaa tavaran olion tavaroista. Palauttaa true jos tavara on pelaajalla, false jos pelaajalla ei ole tavaraa.
        public bool KaytaTavara(int tyyppi, byte maara = 1)
        {
            if (tyyppi >= 0 && tyyppi < tavarat.Length)
            {
                if (tavarat[tyyppi, 1] >= maara)
                {
                    tavarat[tyyppi, 1] -= maara;
                    return true;
                }

            }

            return false;
        }


        //Palauttaa kuinka monta kyseistä tavaratyyppiä pelaajalla on.
        public int TavaranMaara(int tyyppi)
        {
            if (tyyppi >= 0 && tyyppi < tavarat.Length)
            {

                return tavarat[tyyppi, 1];

            }

            return 0;

        }


        //Pudottaa kaikki tavarat oliolta ympärille
        public void PudotaTavarat()
        {
            PhysicsObject tavara;

            //Luo tavarat ja tiputa ne olion ympärille.
            for (int i = 0; i < (int)TavaraTyypit.MaxTavarat; ++i)
            {
             
                while (tavarat[i, 1] > 0)
                {
                    //Jos pelaajan elämät kyseessä, älä tiputa niitä:
                    if (i == (int)TavaraTyypit.Elama && (string)omistaja.Tag == "pelaaja") break;
 
                    tavara = LuoTavara(i);

                    if (tavara != null)
                    {
                        tavara.Position = omistaja.Position + RandomGen.NextVector(LegendOfPallo.TILE_SIZE / 3, LegendOfPallo.TILE_SIZE / 3);
                        tavara.Velocity = RandomGen.NextDirection().GetVector() * 500;
                        //omistaja.Game.Add(tavara, 2); Olion lisääminen peliin tapahtuu jo LuoTavara aliohjelmassa.
                    }

                    --tavarat[i, 1];
                }


            }


        }


        /// <summary>
        /// Luo tavaran aliohjelman avulla ja palauttaa sen viitteen.
        /// </summary>
        /// <param name="tyyppi">Tavaran tyyppi(indeksi).</param>
        /// <returns></returns>
        private PhysicsObject LuoTavara(int tyyppi)
        {

            PhysicsObject tavara = null;

            switch (tyyppi)
            {
                case (int)TavaraTyypit.Pommit:
                    tavara = omistaja.Peli.NostettavaPommiMalli(new Vector(0, 0), 20, 20);
                    break;
                case (int)TavaraTyypit.Avaimet:
                    tavara = omistaja.Peli.NostettavaAvainMalli(new Vector(0, 0), 20, 20);
                    break;
                case (int)TavaraTyypit.Raha:
                    tavara = omistaja.Peli.NostettavaKolikkoMalli(new Vector(0, 0), 20, 20);
                    break;
                case (int)TavaraTyypit.Sydan:
                    tavara = omistaja.Peli.NostettavaSydanMalli(new Vector(0, 0), 20, 20);
                    break;
                case (int)TavaraTyypit.Potioni:
                    tavara = omistaja.Peli.NostettavaPotioniMalli(new Vector(0, 0), 20, 20);
                    break;
                case (int)TavaraTyypit.Elama:
                    tavara = omistaja.Peli.NostettavaElamaMalli(new Vector(0, 0), 20, 20);
                    break;
                default:
                    break;
            }

            return tavara;

        }


    }


}
