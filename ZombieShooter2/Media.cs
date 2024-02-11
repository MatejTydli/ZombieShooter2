using System.Drawing;
using System.Media;

namespace ZombieShooter2
{
	static class Media
	{
		internal static Image pauseImage { get; private set; }
		internal static Image ammoImage { get; private set; }
		internal static Image coinsImage { get; private set; }
		internal static Image bulletImage { get; private set; }
		internal static Image shopImage { get; private set; }
		internal static Image enemyImage { get; private set; }
		internal static Image playerImage { get; private set; }
		internal static Image healImage { get; private set; }
		internal static SoundPlayer shootSound { get; private set; }

		internal static void Initialize()
		{
			pauseImage = Image.FromFile(@".\Media\pause.png");
			ammoImage = Image.FromFile(@".\Media\ammo.png");
			coinsImage = Image.FromFile(@".\Media\coin.png");
			bulletImage = Image.FromFile(@".\Media\bullet.png");
			shopImage = Image.FromFile(@".\Media\shop.png");
			enemyImage = Image.FromFile(@".\Media\enemy.png");
			playerImage = Image.FromFile(@".\Media\player.png");
			healImage = Image.FromFile(@".\Media\heal.png");
			shootSound = new SoundPlayer(@".\Media\shoot_sound.wav");
		}
	}
}
