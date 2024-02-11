using System.Drawing;
using System.Windows.Forms;
using Utils.Drawing;

namespace ZombieShooter2
{
	class Bullet : GameObject
	{
		public Point TargetPoint { get; private set; }
		public Utils.Math.Speed2 Speed { get; private set; }
		public object Tag { get; private set; }

		public Bullet(Point TargetPoint, PictureBox Hitbox) : base(Hitbox)
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
