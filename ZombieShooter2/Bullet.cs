using System.Drawing;
using System.Windows.Forms;
using Utils.Drawing;

namespace ZombieShooter2
{
	class Bullet : GameObject
	{
		internal Point TargetPoint { get; private set; }
		internal Utils.Math.Speed2 Speed { get; private set; }
		internal object Tag { get; private set; }

		internal Bullet(Point TargetPoint, PictureBox Hitbox) : base(Hitbox)
		{
			this.TargetPoint = TargetPoint;
			this.Speed = Utils.Math.SpeedBetween2Points(20, this.Hitbox.Location, this.TargetPoint);

			Image rotatedImage = DrawingUtils.RotateImage(Media.bulletImage,
				(float)(System.Math.Atan2(TargetPoint.Y - this.Hitbox.Location.Y,
				TargetPoint.X - this.Hitbox.Location.X) * (180 / System.Math.PI)));

			this.Hitbox.Image = rotatedImage;
			this.Hitbox.Size = rotatedImage.Size;
		}
	}
}
