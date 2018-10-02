/*
  GameResources is in charge of loading all of the game's resources/assets.
  This includes fonts, images, sounds etc.
*/

using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SwinGameSDK;

namespace BattleShips
{
	public static class GameResources
	{
		// Loads all fonts
		private static void LoadFonts()
		{
			NewFont("ArialLarge", "arial.ttf", 80);
			NewFont("Courier", "cour.ttf", 14);
			NewFont("CourierSmall", "cour.ttf", 8);
			NewFont("Menu", "ffaccess.ttf", 8);
		}

		// Loads all images
		private static void LoadImages()
		{
			// Backgrounds
			NewImage("Menu", "main_page.jpg");
            NewImage("Option", "option_page.jpg");
            NewImage("Discovery", "discover.jpg");
			NewImage("Deploy", "deploy.jpg");

			// Deployment
			NewImage("LeftRightButton", "deploy_dir_button_horiz.png");
			NewImage("UpDownButton", "deploy_dir_button_vert.png");
			NewImage("SelectedShip", "deploy_button_hl.png");
			NewImage("PlayButton", "deploy_play_button.png");
			NewImage("RandomButton", "deploy_randomize_button.png");

			// Ships
			int i = 0;
			for (i = 1; i <= 5; i++) {
				NewImage("ShipLR" + i, "ship_deploy_horiz_" + i + ".png");
				NewImage("ShipUD" + i, "ship_deploy_vert_" + i + ".png");
                //loads hover
                NewImage("ShipLR" + i + "_hover", "ship_deploy_horiz_" + i + "_hover.png");
                NewImage("ShipUD" + i + "_hover", "ship_deploy_vert_" + i + "_hover.png");
            }

			// Explosions
			NewImage("Explosion", "explosion.png");
			NewImage("Splash", "splash.png");

            //clear screen
            NewImage("Clear", "clear_sceen_button.png");

            //back button
            NewImage("Back", "back_button.png");

            //help
            NewImage("Help", "help.png");
		}

		// Loads all sounds
		private static void LoadSounds()
		{
			NewSound("Error", "error.wav");
			NewSound("Hit", "hit.wav");
			NewSound("Sink", "sink.wav");
			//NewSound("Siren", "siren.wav");
			NewSound("Miss", "watershot.wav");
			NewSound("Winner", "winner.wav");
			NewSound("Lose", "lose.wav");
		}

		// Loads all music files
		private static void LoadMusic()
		{
			NewMusic("Background", "BachAir.ogg");
            NewMusic("Background2", "CanoninDMajor.ogg");
            NewMusic("Background3", "FallToLight.ogg");
            NewMusic("Background4", "SynthTrack.ogg");

        }

		// Summary: Gets a Font Loaded in the Resources
		// Parameter: font - Name of Font
		// Returns: The Font Loaded with this Name
		public static Font GameFont(string font)
		{
			return _fonts[font];
		}

		// Summary: Gets an Image loaded in the Resources
		// Parameter: image - Name of image
		// Returns: The image loaded with this name
		public static Bitmap GameImage(string image)
		{
			return _images[image];
		}

		// Summary: Gets an sound loaded in the Resources
		// Parameter: sound - Name of sound
		// Returns: The sound with this name
		public static SoundEffect GameSound(string sound)
		{
			return _sounds[sound];
		}

		// Summary: Gets the music loaded in the Resources
		// Parameter: music - Name of music
		// Returns: The music with this name
		public static Music GameMusic(string music)
		{
			return _music[music];
		}

		private static Dictionary<string, Bitmap> _images = new Dictionary<string, Bitmap>();
		private static Dictionary<string, Font> _fonts = new Dictionary<string, Font>();
		private static Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();
		private static Dictionary<string, Music> _music = new Dictionary<string, Music>();
		private static Bitmap _background;
		private static Bitmap _animation;
		private static Bitmap _loaderFull;
		private static Bitmap _loaderEmpty;
		private static Font _loadingFont;

		private static SoundEffect _startSound;

		// Summary: Calls all of the loading methods. Just run this once and the assets are set up!
		public static void LoadResources()
		{
			int width = 0;
			int height = 0;

			width = SwinGame.ScreenWidth();
			height = SwinGame.ScreenHeight();

			SwinGame.ChangeScreenSize(800, 600);

			ShowLoadingScreen();

			ShowMessage("Loading fonts...", 0);
			LoadFonts();
			SwinGame.Delay(100);

			ShowMessage("Loading images...", 1);
			LoadImages();
			SwinGame.Delay(100);

			ShowMessage("Loading sounds...", 2);
			LoadSounds();
			SwinGame.Delay(100);

			ShowMessage("Loading music...", 3);
			LoadMusic();
			SwinGame.Delay(100);

			SwinGame.Delay(100);
			ShowMessage("Game loaded...", 5);
			SwinGame.Delay(100);
			//EndLoadingScreen(width, height);
		}

	    // Shows the loading screen.
		private static void ShowLoadingScreen()
		{
			_background = SwinGame.LoadBitmap("SplashBack.png");
			SwinGame.DrawBitmap(_background, 0, 0);
			SwinGame.RefreshScreen();
			SwinGame.ProcessEvents();

			_animation = SwinGame.LoadBitmap("SwinGameAni.jpg");
			_loadingFont = SwinGame.LoadFont("arial.ttf", 12);
			_startSound = Audio.LoadSoundEffect("SwinGameStart.ogg");

			_loaderFull = SwinGame.LoadBitmap("loader_full.png");
            _loaderEmpty = SwinGame.LoadBitmap("loader_empty.png");

			PlaySwinGameIntro();
		}

	    // Plays the swin game intro.
		private static void PlaySwinGameIntro()
		{
			const int ANI_X = 143;
			const int ANI_Y = 134;
			const int ANI_W = 546;
			const int ANI_H = 327;
			const int ANI_V_CELL_COUNT = 6;
			const int ANI_CELL_COUNT = 11;

			Audio.PlaySoundEffect(_startSound);
			SwinGame.Delay(200);

			int i = 0;
			for (i = 0; i <= ANI_CELL_COUNT - 1; i++)
			{
				SwinGame.DrawBitmap(_background, 0, 0);
				//SwinGame.DrawBitmapPart(_animation, (i / ANI_V_CELL_COUNT) * ANI_W, (i % ANI_V_CELL_COUNT) * ANI_H, ANI_W, ANI_H, ANI_X, ANI_Y);
				SwinGame.Delay(20);
				SwinGame.RefreshScreen();
				SwinGame.ProcessEvents();
			}
			SwinGame.Delay(1500);
		}

		// Summary: Presumably draws a message...
		// Parameter: message - Message to display?
		// Parameter: number - currently unused
		private static void ShowMessage(string message, int number)
		{
			const int TX = 310;
			const int TY = 493;
			const int TW = 200;
			const int TH = 25;
			const int STEPS = 5;
			const int BG_X = 279;
			const int BG_Y = 453;

			int fullW = 0;

			fullW = 260 * number / STEPS;
			SwinGame.DrawBitmap(_loaderEmpty, BG_X, BG_Y);

	        // TODO: Do this the right way
	        SwinGame.DrawCell (_loaderFull, 0, BG_X, BG_Y);
			//Draw Bitmap Part   (src, srcX, srcY, srcW, srcH, x, y)
			//SwinGame.DrawBitmapPart(_LoaderFull, 0, 0, fullW, 66, BG_X, BG_Y);

			Rectangle toDraw = new Rectangle ();
	        toDraw.X = TX;
			toDraw.Y =  TY;
			toDraw.Width = TW;
	        toDraw.Height = TH;

	        SwinGame.DrawTextLines(message, Color.White, Color.Transparent,_loadingFont,FontAlignment.AlignCenter,toDraw);
			//SwinGame.DrawTextLines(message, Color.White, Color.Transparent, _loadingFont, FontAlignment.AlignCenter, TX, TY, TW, TH);

			SwinGame.RefreshScreen();
			SwinGame.ProcessEvents();
		}

		// Summary: Frees some resources and a sound, clears the screen, then changes the screen size.
		// Parameter: width - Width of screen to change to
		// Parameter: height - Height of screen to change to
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

		// Summary: Adds a new font to the fonts list
		// Parameter: fontName - The name of the font to use
		// Parameter: fileName - The file location to search for
		// Parameter: size - The size of the font
		private static void NewFont(string fontName, string fileName, int size)
		{
			_fonts.Add(fontName, SwinGame.LoadFont(fileName, size));
		}

		// Summary: Adds a new image to the image list
		// Parameter: imageName - The name of the image to use
		// Parameter: fileName - The file location to search for
		private static void NewImage(string imageName, string fileName)
		{
			_images.Add(imageName, SwinGame.LoadBitmap(fileName));
		}

		// Summary: Adds a new 'transparent colour image' to the images list
		// Parameter: imageName - The name of the image to use
		// Parameter: fileName - The file location to search for
		// Parameter: transColor - Currently unused
		private static void NewTransparentColorImage(string imageName, string fileName, Color transColor)
		{
	        Bitmap bitmap = SwinGame.LoadBitmap (SwinGame.PathToResource (fileName,ResourceKind.BitmapResource));
			//Bitmap bitmap = SwinGame.LoadBitmap (SwinGame.PathToResource (fileName, ResourceKind.BitmapResource), true, transColor);
			_images.Add(imageName, bitmap);
		}

		// Summary: Literally just calls the above, but is spelt 'colour' not 'color'
		private static void NewTransparentColourImage(string imageName, string fileName, Color transColor)
		{
			NewTransparentColorImage(imageName, fileName, transColor);
		}

		// Summary: Adds a new sound to the sound list
		// Parameter: soundName - The name to use for the sound file
		// Parameter: fileName - The file location to search for
		private static void NewSound(string soundName, string fileName)
		{
			_sounds.Add(soundName, Audio.LoadSoundEffect(SwinGame.PathToResource(fileName, ResourceKind.SoundResource)));
		}


		// Summary: Adds a new file to the music list
		// Parameter: musicName - The name to use for the music file
		// Parameter: fileName - The file location to search for
		private static void NewMusic(string musicName, string fileName)
		{
			_music.Add(musicName, Audio.LoadMusic(SwinGame.PathToResource(fileName, ResourceKind.SoundResource)));
		}

		// Summary: Frees all game fonts from memory
		private static void FreeFonts()
		{
			foreach (Font obj in _fonts.Values)
			{
				SwinGame.FreeFont(obj);
			}
		}

		// Summary: Frees all images from memory
		private static void FreeImages()
		{
			foreach (Bitmap obj in _images.Values)
			{
				SwinGame.FreeBitmap(obj);
			}
		}

		// Summary: Frees all sounds from memory
		private static void FreeSounds()
		{
			foreach (SoundEffect obj in _sounds.Values)
			{
				Audio.FreeSoundEffect(obj);
			}
		}

		// Summary: Frees all music files from memory
		private static void FreeMusic()
		{

			foreach (Music obj in _music.Values)
			{
				Audio.FreeMusic(obj);
			}
		}

		// Summary: Frees everything from memory!
		public static void FreeResources()
		{
			FreeFonts();
			FreeImages();
			FreeMusic();
			FreeSounds();
			SwinGame.ProcessEvents();
		}
	}
}
