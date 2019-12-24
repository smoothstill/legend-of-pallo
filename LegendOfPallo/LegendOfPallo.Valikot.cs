using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jypeli;
using Jypeli.Widgets;


/// <summary>
/// Valikossa navigointi ja mitä eri näppäimistä tapahtuu.
/// </summary>
public partial class LegendOfPallo : PhysicsGame
{


    /// <summary>
    /// Lataa kentän ja asentaa kentän pelattavaksi.
    /// </summary>
    private void LataaKentta()
    {

        IsPaused = false;
        if (pelaajanTiedot.Elamat >= 1)
        {
            ClearAll();

            Piikit.AktioiPiikit();
            LataaPelaaja('P');
            LataaKentanOliotPaitsiPelaaja();

            adisplay.Clear();
            Add(adisplay);
            adisplay.Add("Level " + CurrentLevel);

            HUD.Piilota();
            HUD.Luo(this, pelaajanTiedot.MaxHp);
            HUD.ResetoiKentanPisteet();
            Level.Background.Image = Animaatiot.BackgroundImage;
            //Level.Background.Image = Animaatiot.BackgroundSkullsImage;
            Level.Background.TileToLevel();
            Camera.ZoomTo(new Vector(0, 0), new Vector(CameraPixelsHeight, CameraPixelsHeight));
            AsetaOhjaimet();

            //Piilota cursori
            Mouse.IsCursorVisible = false;
        }
        else
        {
            AsetaHighScoreJaPalaaMenuun();
        }


    }


    /// <summary>
    /// Lataa kenttään teleportterin entrance ja exit merkkien avulla
    /// </summary>
    /// <param name="entrance">merkki, joka esittää teleportterin sisäänkäyntiä</param>
    /// <param name="exit">merkki, jonka kohdalla teleportterista tullaan ulos</param>
    private void LataaKenttaTeleportEntranceExit(char entrance, char exit)
    {
        TileMap ruudut = TileMap.FromLevelAsset("kentta" + CurrentLevel);
        ruudut.SetTileMethod(entrance, LuoTeleportEntrance);
        ruudut.SetTileMethod(exit, LuoTeleportExit);
        ruudut.Execute(TILE_SIZE, TILE_SIZE);
    }


    /// <summary>
    /// Lataa pelaajan kenttäruudukon merkeistä.
    /// </summary>
    /// <param name="pelaaja"></param>
    private void LataaPelaaja(char pelaaja)
    {
        TileMap ruudut = TileMap.FromLevelAsset("kentta" + CurrentLevel);
        ruudut.SetTileMethod(pelaaja, LuoPelaaja);
        ruudut.Execute(TILE_SIZE, TILE_SIZE);
    }


    /// <summary>
    /// Lataa kentän oliot ruudulle tilemapsista.
    /// </summary>
    private void LataaKentanOliotPaitsiPelaaja()
    {
        TileMap ruudut = TileMap.FromLevelAsset("kentta" + CurrentLevel);
        ruudut.SetTileMethod('#', LuoSeina);
        ruudut.SetTileMethod('B', LuoLaatikkoTyhja);
        ruudut.SetTileMethod('b', LuoNostettavaPommi);
        ruudut.SetTileMethod('k', LuoNostettavaAvain);
        ruudut.SetTileMethod('S', LuoNostettavaKolikko);
        ruudut.SetTileMethod('X', LuoLukittuOvi);
        ruudut.SetTileMethod('o', LuoKimpoilija);
        ruudut.SetTileMethod('v', LuoVihollinenKuljeksiva);
        ruudut.SetTileMethod('V', LuoVihollinenSeuraaja);
        ruudut.SetTileMethod('y', LuoVihollinenAmpuja);
        ruudut.SetTileMethod('i', LuoRikkuvaSeina);
        ruudut.SetTileMethod('E', LuoExit);
        ruudut.SetTileMethod('g', LuoVihollinenKummitus);
        ruudut.SetTileMethod('p', LuoVihollinenSpawneri);
        ruudut.SetTileMethod('H', LuoVihollinenHumanoidi);
        ruudut.SetTileMethod('F', LuoPinkkiPallo);
        ruudut.SetTileMethod('w', LuoPiikit);
        ruudut.SetTileMethod('+', LuoNostettavaSydan);
        ruudut.SetTileMethod('*', LuoNostettavaPotioni);
        ruudut.SetTileMethod('1', LuoVihollinenAmpujaA);
        ruudut.SetTileMethod('2', LuoVihollinenAmpujaB);
        ruudut.SetTileMethod('3', LuoVihollinenAmpujaC);
        ruudut.SetTileMethod('4', LuoVihollinenAmpujaS);
        ruudut.SetTileMethod('Q', LuoNostettavaElama);
        ruudut.SetTileMethod('7', LuoAaretonPommi);
        ruudut.SetTileMethod('6', LuoVihollinenSplortsi);
        ruudut.Execute(TILE_SIZE, TILE_SIZE);

        LataaKenttaTeleportEntranceExit('_', ',');
        LataaKenttaTeleportEntranceExit('9', '0');
    }


    /// <summary>
    /// Avaa parhaiden pisteiden listan, ja antaa pelaajan kirjoittaa nimensä listaan jos hänellä on tarpeeksi pisteitä siihen.
    /// Tämän jälkeen suorittaa aliohjelman TallennaPisteet, jossa lista tallennetaan ja palataan takaisin alkuvalikkoon.
    /// </summary>
    private void AsetaHighScoreJaPalaaMenuun()
    {
        HighScoreWindow topIkkuna = new HighScoreWindow(
                             "High Scores",
                             "Your score: %p. Enter your name:",
                             topLista, HUD.Pisteet);
        topIkkuna.Closed += TallennaPisteet;
        Add(topIkkuna);
    }


    /// <summary>
    /// Tallentaa parhaiden pisteiden listan ja asentaa alkuvalikon peliin
    /// </summary>
    /// <param name="sender"></param>
    private void TallennaPisteet(Window sender)
    {
        DataStorage.Save<ScoreList>(topLista, "pisteet.xml");
        HUD.Piilota();
        HUD.NollaaPelinPisteet();
        ClearAll();
        LataaMenunTausta();
        Alkuvalikko();
    }


    /// <summary>
    /// Lataa seuraavan kentän peliin.
    /// </summary>
    private void SeuraavaKentta()
    {
        ++CurrentLevel;
        LataaKentta();
    }


    /// <summary>
    /// Lataa taustan alkuvalikkoon. Taustana on pelin simulaatio yhdestä kohdasta ensimmäistä kenttää.
    /// </summary>
    private void LataaMenunTausta()
    {
        IsPaused = false;
        CurrentLevel = 1;
        LataaPelaaja('P');
        LataaKentanOliotPaitsiPelaaja();
        pelaaja.Position = new Vector(-265, -1408);
        pelaaja.Hp = -1;
        pelaaja.IsVisible = false;
        pelaaja.IgnoresCollisionResponse = true;
        pelaaja.Mass = double.PositiveInfinity;
        Level.Background.Image = Animaatiot.BackgroundImage;
        Level.Background.TileToLevel();
        Camera.ZoomTo(new Vector(0, 0), new Vector(CameraPixelsHeight, CameraPixelsHeight));
        Camera.Follow(pelaaja);
        Piikit.AktioiPiikit();
    }


    public delegate void Delegate1();
    private static Delegate1 _tapahtumaTekstinJalkeen;
    /// <summary>
    /// Printtaa tekstin keskelle ruutua, ja asettaa parametrifunktion avulla mitä tapahtuu tekstin jälkeen.
    /// Tekstin saa pois painamalla space-näppäintä.
    /// </summary>
    /// <param name="teksti">Teksti, joka ruudulle halutaan.</param>
    /// <param name="funktio">Funktio, joka määrittelee mitä tapahtuu kun painetaan spacea.</param>
    public void PrinttaaTekstiJaOdotaInput(string teksti, Delegate1 funktio)
    {

        _tapahtumaTekstinJalkeen = funktio;

        HorizontalLayout asettelu = new HorizontalLayout();
        asettelu.Spacing = 8;
        asettelu.LeftPadding = 8;
        asettelu.RightPadding = 8;

        Widget tausta = new Widget(asettelu);
        tausta.Color = Color.Black;
        Add(tausta);

        tausta.Position = Game.Screen.Center;

        Label tekstikentta = new Label(teksti + "\nPress space to continue.");
        tekstikentta.TextColor = Color.White;
        tausta.Add(tekstikentta);

    }


    /// <summary>
    /// Asentaa menu-valikon peliin.
    /// </summary>
    private void Alkuvalikko()
    {
        MultiSelectWindow valikko = new MultiSelectWindow("",
		"Start Game", "High Score", "Controls", "Quit Game");
        valikko.ItemSelected += PainettiinValikonNappiaStartMenu;
        valikko.DefaultCancel = -1;
        valikko.Color = Color.Black;
        valikko.SelectionColor = Color.DarkGray;
        Add(valikko);
        AsetaOhjaimet();
    }


    /// <summary>
    /// Asettaa valikon kesken pelin, kun painetaan esc-näppäintä.
    /// </summary>
    private void ValikkoKeskenPelin()
    {
        if (pelaaja.CurrentHp > 0)  //Jos pelaaja on kuollut, älä anna hänen mennä menuvalikkoon. Pelaaja voi jatkaa painalla space-näppäintä kuoleman jälkeen.
        {
            IsPaused = true;
            MultiSelectWindow valikko = new MultiSelectWindow("",
            "Continue", "Restart Level", "Controls", "Go to Menu");
            valikko.ItemSelected += PainettiinValikonNappiaInGame;
            valikko.DefaultCancel = -1;
            valikko.Color = Color.Black;
            valikko.SelectionColor = Color.DarkGray;
            Add(valikko);
        }
    }


    /// <summary>
    /// Avaa parhaiden pisteiden lista muuttamatta sitä.
    /// </summary>
    private void AvaaTopScore()
    {
        HighScoreWindow topIkkuna = new HighScoreWindow(
                              "High Scores",
                              topLista);
        topIkkuna.Closed += SuljeTopScore;
        Add(topIkkuna);
    }


    /// <summary>
    /// Sulkee parhaiden pisteiden listan ja palaa alkuvalikkoon.
    /// </summary>
    /// <param name="sender"></param>
    private void SuljeTopScore(Window sender)
    {
        Alkuvalikko();
    }


    /// <summary>
    /// Aliohjelma määrittelee, mitä tapahtuu alkivalikon eri painikkeita painamalla.
    /// </summary>
    /// <param name="id">Painikkeen indeksi.</param>
    private void PainettiinValikonNappiaStartMenu(int id)
    {


        switch (id)
		{

			case (0):
                PrinttaaTekstiJaOdotaInput("Your friend, Pink Ball, has been kidnapped by evil creature Splörts.\nYou must defeat Splörts and free your friend from his dark dungeon.", EnsimmaiseenKenttaan);
                break;
            case (1):
                AvaaTopScore();
                break;
            case (2):
                ShowControlHelp();
                Alkuvalikko();
                break;
            case (3):
                Exit();
                break;

        }

    }


    /// <summary>
    /// Aliohjelma määrittelee, mitä tapahtuu kesken pelin avatun valikon eri painikkeita painamalla.
    /// </summary>
    /// <param name="id">Painikkeen indeksi.</param>
    private void PainettiinValikonNappiaInGame(int id)
    {
        switch (id)
        {

            case (0):
                IsPaused = false;
                break;
            case (1):
                IsPaused = false;
                LataaKentta();
                break;
            case (2):
                ShowControlHelp();
                Alkuvalikko();
                IsPaused = false;
                break;
            case (3):
                HUD.ResetoiKentanPisteet();
                AsetaHighScoreJaPalaaMenuun();
                break;

        }
    }

}

