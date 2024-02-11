using System.Drawing;
using System.Windows.Forms;
using static ZombieShooter2.Program;

namespace ZombieShooter2
{
	abstract class GameObject
	{
		public PictureBox Hitbox { get; private set; }

		public GameObject(PictureBox hitbox)
		{
			this.Hitbox = hitbox;
		}

		public void Move(int X, int Y)
		{
			Hitbox.Location = new Point(Hitbox.Location.X + X, Hitbox.Location.Y + Y);
		}

		public void Teleport(int X, int Y)
		{
			Hitbox.Location = new Point(X, Y);
		}

		public virtual void FixCollision(string collidersName)
		{
			Control collider = this.GetCollider(collidersName);

			if (this.Hitbox.Top < collider.Bottom && this.Hitbox.Top > collider.Top)
				this.Move(0, collider.Bottom - this.Hitbox.Top);
			if (this.Hitbox.Bottom < collider.Bottom && this.Hitbox.Bottom > collider.Top)
				this.Move(0, collider.Top - this.Hitbox.Bottom + 2);
		}

		public void FixFormCollision()
		{
			if (this.Hitbox.Right >= mainForm.Width) this.Teleport(mainForm.Width - this.Hitbox.Width * 2, this.Hitbox.Top);
			if (this.Hitbox.Left <= 0) this.Teleport(0, this.Hitbox.Top);
			if (this.Hitbox.Top <= 0) this.Teleport(this.Hitbox.Left, 0);
			if (this.Hitbox.Bottom >= mainForm.Height) this.Teleport(this.Hitbox.Left, mainForm.gamePlatform_Floor.Top);
		}

		public Control GetCollider(string collidersName)
		{
			foreach (Control control in mainForm.Controls)
				if (control.Name == collidersName && this.Hitbox.Bounds.IntersectsWith(control.Bounds)) return control;

			return null;
		}

		public bool IsCollidingWithMainForm()
		{
			if (this.Hitbox.Right >= mainForm.Width || this.Hitbox.Top <= 0 ||
				this.Hitbox.Bottom >= mainForm.Height || this.Hitbox.Left <= 0) return true;
			return false;
		}
	}
}