using System.Drawing;
using System.Media;

namespace ZombieShooter2
{
	static class Media
	{
		public static Image pauseImage { get; private set; }
		public static Image ammoImage { get; private set; }
		public static Image coinsImage { get; private set; }
		public static Image bulletImage { get; private set; }
		public static Image shopImage { get; private set; }
		public static Image enemyImage { get; private set; }
		public static Image playerImage { get; private set; }
		public static Image healImage { get; private set; }
		public static SoundPlayer shootSound { get; private set; }

		public static void Initialize()
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
