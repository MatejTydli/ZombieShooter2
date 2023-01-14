using System;
using System.Drawing;
using System.Windows.Forms;
using myUtilities;

namespace ZombieShooter2
{
	class Bullet : GameObject
	{
		internal Point TargetPoint { get; private set; }
		internal MyMath.Speed2 Speed { get; private set; }
		internal object Tag { get; private set; }

		internal Bullet(Point TargetPoint, PictureBox Hitbox) : base(Hitbox)
		{
			this.TargetPoint = TargetPoint;
			this.Speed = MyMath.SpeedBetween2Points(20, this.Hitbox.Location, this.TargetPoint);

			Image rotatedImage = Utilities.RotateImage(Media.bulletImage,
				Math.Atan2(TargetPoint.Y - this.Hitbox.Location.Y,
				TargetPoint.X - this.Hitbox.Location.X) * (180 / Math.PI));

			this.Hitbox.Image = rotatedImage;
			this.Hitbox.Size = rotatedImage.Size;
		}
	}
}
