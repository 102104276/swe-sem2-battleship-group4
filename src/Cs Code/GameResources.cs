/*
Game Resources is in charge of loading and managing all game assets,
including the removal of them from memory once the game has concluded.
*/

using SwinGameSDK;
using System.Collections.Generic;
using System;

namespace BattleShips
{
	public class GameResources
	{
		// Loads all game fonts.
	    private static void LoadFonts()
		{
	        GameResources.newFont("ArialLarge", "arial.ttf", 80);
	        GameResources.newFont("Courier", "cour.ttf", 14);
	        GameResources.newFont("CourierSmall", "cour.ttf", 8);
	        GameResources.newFont("Menu", "ffaccess.ttf", 8);
	    }
		
		// Loads all images for the game.
	    private static void LoadImages()
		{
	        // Backgrounds
	        GameResources.newImage("Menu", "main_page.jpg");
	        GameResources.newImage("Discovery", "discover.jpg");
	        GameResources.newImage("Deploy", "deploy.jpg");
	        // Deployment
	        GameResources.newImage("LeftRightButton", "deploy_dir_button_horiz.png");
	        GameResources.newImage("UpDownButton", "deploy_dir_button_vert.png");
	        GameResources.newImage("SelectedShip", "deploy_button_hl.png");
	        GameResources.newImage("PlayButton", "deploy_play_button.png");
	        GameResources.newImage("RandomButton", "deploy_randomize_button.png");
	        // Ships
	        int i;
	        for(i = 1; i <= 5; i ++)
			{
	            GameResources.newImage(("ShipLR" + i), ("ship_deploy_horiz_" + (i + ".png")));
	            GameResources.newImage(("ShipUD" + i), ("ship_deploy_vert_" + (i + ".png")));
	        }
	        // Explosions
	        GameResources.newImage("Explosion", "explosion.png");
	        GameResources.newImage("Splash", "splash.png");
	    }
	    
	    // Loads all sounds required for the game.
	    private static void LoadSounds()
		{
	        GameResources.newSound("Error", "error.wav");
	        GameResources.newSound("Hit", "hit.wav");
	        GameResources.newSound("Sink", "sink.wav");
	        GameResources.newSound("Siren", "siren.wav");
	        GameResources.newSound("Miss", "watershot.wav");
	        GameResources.newSound("Winner", "winner.wav");
	        GameResources.newSound("Lose", "lose.wav");
	    }
	    
	    //Loads music for the game.
	    private static void LoadMusic()
		{
	        GameResources.newMusic("Background", "horrordrone.mp3");
	    }
		
	    // gets a Font Loaded in the Resources
	    // Parameters: Name of Font
		// returns: The Font Loaded with this Name
	    public static Font GameFont(string font)
		{
	        return _fonts(font);
	    }

	    // gets an Image loaded in the Resources
	    // Parameters: Name of image
	    // returns: The image loaded with this name
	    public static Bitmap GameImage(string image)
		{
	        return _images(image);
	    }
	    
		// gets a sound loaded in the Resources
	    // Parameters: Name of sound
	    // returns: The sound with this name
	    public static SoundEffect GameSound(string sound)
		{
	        return _sounds(sound);
	    }
	    
	    // gets the music loaded in the Resources
	    // Parameters: Name of music
	    // returns: The music with this name
	    public static Music GameMusic(string music)
		{
	        return _music(music);
	    }
	    
	    private Dictionary<string, Bitmap> _images = new Dictionary<string, Bitmap>();
	    
	    private Dictionary<string, Font> _fonts = new Dictionary<string, Font>();
	    
	    private Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();
	    
	    private Dictionary<string, Music> _music = new Dictionary<string, Music>();
	    
	    private Bitmap _background;
	    
	    private Bitmap _animation;
	    
	    private Bitmap _loaderFull;
	    
	    private Bitmap _loaderEmpty;
	    
	    private Font _loadingFont;
	    
	    private SoundEffect _startSound;
	    
	    /*
		The Resources class stores all of the Games Media Resources, such as Images, 
		Fonts, Sounds, Music.
	    */
	    public static void LoadResources()
		{
	        int width;
	        int height;
	        width = SwinGame.ScreenWidth();
	        height = SwinGame.ScreenHeight();
	        SwinGame.ChangeScreenSize(800, 600);
	        GameResources.ShowLoadingScreen();
	        GameResources.ShowMessage("Loading fonts...", 0);
	        GameResources.LoadFonts();
	        SwinGame.Delay(100);
	        GameResources.ShowMessage("Loading images...", 1);
	        GameResources.LoadImages();
	        SwinGame.Delay(100);
	        GameResources.ShowMessage("Loading sounds...", 2);
	        GameResources.LoadSounds();
	        SwinGame.Delay(100);
	        GameResources.ShowMessage("Loading music...", 3);
	        GameResources.LoadMusic();
	        SwinGame.Delay(100);
	        SwinGame.Delay(100);
	        GameResources.ShowMessage("Game loaded...", 5);
	        SwinGame.Delay(100);
	        GameResources.EndLoadingScreen(width, height);
	    }
	    
	    // Handles the display of the loading screen.
	    private static void ShowLoadingScreen()
		{
	        _background = SwinGame.LoadBitmap(SwinGame.PathToResource("SplashBack.png", ResourceKind.BitmapResource));
	        SwinGame.DrawBitmap(_background, 0, 0);
	        SwinGame.RefreshScreen();
	        SwinGame.ProcessEvents();
	        _animation = SwinGame.LoadBitmap(SwinGame.PathToResource("SwinGameAni.jpg", ResourceKind.BitmapResource));
	        _loadingFont = SwinGame.LoadFont(SwinGame.PathToResource("arial.ttf", ResourceKind.FontResource), 12);
	        _startSound = Audio.LoadSoundEffect(SwinGame.PathToResource("SwinGameStart.ogg", ResourceKind.SoundResource));
	        _loaderFull = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_full.png", ResourceKind.BitmapResource));
	        _loaderEmpty = SwinGame.LoadBitmap(SwinGame.PathToResource("loader_empty.png", ResourceKind.BitmapResource));
	        GameResources.PlaySwinGameIntro();
	    }
	    
	    // Plays the very annoying SwinGame intro sequence.
	    private static void PlaySwinGameIntro()
		{
	        const int ANI_CELL_COUNT = 11;
	        Audio.PlaySoundEffect(_startSound);
	        SwinGame.Delay(200);
	        int i;
	        for (i = 0; i <= (ANI_CELL_COUNT - 1); i ++) {
	            SwinGame.DrawBitmap(_background, 0, 0);
	            SwinGame.Delay(20);
	            SwinGame.RefreshScreen();
	            SwinGame.ProcessEvents();
	        }
	        
	        SwinGame.Delay(1500);
	    }
	    
	    // Displays a message.
	    // Parameter 'message': the message to display
	    // Parameter 'number': currently not in use.
	    private static void ShowMessage(string message, int number)
		{
	        const int BG_Y = 453;
	        int TX = 310;
	        int TY = 493;
	        int TW = 200;
	        int TH = 25;
	        int STEPS = 5;
	        int BG_X = 279;
	        int fullW;
	        Rectangle toDraw;
	        fullW = (260 * number);
	        STEPS;
	        SwinGame.DrawBitmap(_loaderEmpty, BG_X, BG_Y);
	        SwinGame.DrawCell(_loaderFull, 0, BG_X, BG_Y);
	        //  SwinGame.DrawBitmapPart(_LoaderFull, 0, 0, fullW, 66, BG_X, BG_Y)
	        toDraw.X = TX;
	        toDraw.Y = TY;
	        toDraw.Width = TW;
	        toDraw.Height = TH;
	        SwinGame.DrawTextLines(message, Color.White, Color.Transparent, _loadingFont, FontAlignment.AlignCenter, toDraw);
	        //  SwinGame.DrawTextLines(message, Color.White, Color.Transparent, _loadingFont, FontAlignment.AlignCenter, TX, TY, TW, TH)
	        SwinGame.RefreshScreen();
	        SwinGame.ProcessEvents();
	    }
	    
	    // Displays the end loading screen
	    // Parameter 'width': the width of the end loading screen.
	    // Parameter 'height': the height of the end loading screen.
	    private static void EndLoadingScreen(int width, int height)
		{
	        SwinGame.ProcessEvents();
	        SwinGame.Delay(500);
	        SwinGame.ClearScreen();
	        SwinGame.RefreshScreen();
	        SwinGame.FreeFont(_loadingFont);
	        SwinGame.FreeBitmap(_background);
	        SwinGame.FreeBitmap(_animation);
	        SwinGame.FreeBitmap(_loaderEmpty);
	        SwinGame.FreeBitmap(_loaderFull);
	        Audio.FreeSoundEffect(_startSound);
	        SwinGame.ChangeScreenSize(width, height);
	    }
	    
	    // Adds a new font to the game assets.
	    private static void newFont(string fontName, string filename, int size)
		{
	        _fonts.Add(fontName, SwinGame.LoadFont(SwinGame.PathToResource(filename, ResourceKind.FontResource), size));
	    }
	    
	    // Adds a new image to the game assets.
	    private static void newImage(string imageName, string filename)
		{
	        _images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(filename, ResourceKind.BitmapResource)));
	    }
	    
	    // Adds a new 'transparent colour image' to the game assets
	    private static void newTransparentColorImage(string imageName, string fileName, Color transColor)
		{
	        _images.Add(imageName, SwinGame.LoadBitmap(SwinGame.PathToResource(fileName, ResourceKind.BitmapResource)));
	    }
	    // A variation of the spelling for the above method.
	    private static void newTransparentColourImage(string imageName, string fileName, Color transColor)
		{
	        GameResources.newTransparentColorImage(imageName, fileName, transColor);
	    }
	    
	    // Adds a new sound to the game assets
	    private static void newSound(string soundName, string filename)
		{
	        _sounds.Add(soundName, Audio.LoadSoundEffect(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
	    }
	    
	    // Adds a new music file to the game assets
	    private static void newMusic(string musicName, string filename)
		{
	        _music.Add(musicName, Audio.LoadMusic(SwinGame.PathToResource(filename, ResourceKind.SoundResource)));
	    }
	    
	    // Removes all fonts from memory.
	    private static void FreeFonts()
		{
	        Font obj;
	        foreach (obj in _fonts.Values)
			{
	            SwinGame.FreeFont(obj);
	        }
	        
	    }
	    
	    // Removes all bitmaps from memory.
	    private static void FreeImages()
		{
	        Bitmap obj;
	        foreach (obj in _images.Values)
			{
	            SwinGame.FreeBitmap(obj);
	        }
	        
	    }
	    
	    // Removes all sounds from memory.
	    private static void FreeSounds()
		{
	        SoundEffect obj;
	        foreach (obj in _sounds.Values)
			{
	            Audio.FreeSoundEffect(obj);
	        }
	        
	    }
	    
	    // Removes all music files from memory.
	    private static void FreeMusic()
		{
	        Music obj;
	        foreach (obj in _music.Values)
			{
	            Audio.FreeMusic(obj);
	        }
	        
	    }
	    
	    // Collectively removes all resources for the game from memory.
	    public static void FreeResources()
		{
	        GameResources.FreeFonts();
	        GameResources.FreeImages();
	        GameResources.FreeMusic();
	        GameResources.FreeSounds();
	        SwinGame.ProcessEvents();
	    }
	}
}