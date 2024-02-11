using System.Drawing;
using System.Windows.Forms;

namespace ZombieShooter2
{
	class Player : GameObject
	{
		public int healt;
		public bool GoLeft, GoRight, Jumping, CollidingOnTop, CanShoot, FacingRight;

		public Player(PictureBox hitbox) : base(hitbox)
		{
			this.GoLeft = false;
			this.GoRight = false;
			this.Jumping = false;
			this.CollidingOnTop = false;
			this.CanShoot = true;
			this.FacingRight = true;
			this.healt = 100;
		}

		public void Shoot(Point targetPoint)
		{
			if (!this.CanShoot || Game.ammo < 1) return;

			Game.ammo--;
			Program.mainForm.ammoLabel.Text = Game.ammo.ToString(); 

			Bullet bullet = new Bullet(targetPoint, new PictureBox() 
			{
				Name = "bullet",
				Image = Media.bulletImage,
				Location = new Point(
				targetPoint.X < this.Hitbox.Right ? this.Hitbox.Right : this.Hitbox.Left,
				this.Hitbox.Location.Y + this.Hitbox.Height / 2)
			});

			Game.bullets.Add(bullet);
			Program.mainForm.gamePage.AddControl(bullet.Hitbox);

			if (Program.mainForm.optionsSoundLabel.Text == "Sounds: ON") Media.shootSound.Play();

			this.CanShoot = false;
		}

		public void SetFacing(Point mousePos)
		{
			Image rotatedImage = this.Hitbox.Image;

			bool newFacingRight = mousePos.X < this.Hitbox.Left + this.Hitbox.Width / 2 && Game.gameState == GameState.Running ? false : true;
			if (newFacingRight != this.FacingRight)
			{
				rotatedImage.RotateFlip(RotateFlipType.Rotate180FlipY);
				this.Hitbox.Image = rotatedImage;
			}

			this.FacingRight = newFacingRight;
		}
	}
}